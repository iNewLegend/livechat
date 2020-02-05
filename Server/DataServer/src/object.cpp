#include "stdafx.h"
#include "object.h"
#include "protocol.h"
#include "network.h"
#include "libmsg.h"
#include "database.h"
#include "packets.h"

OBJECTSTRUCT g_Obj[OBJECT_MAX];

int gObjSearchFree() {
	for(int i = 0; i < OBJECT_MAX; i++)
	{
		if(g_Obj[i].m_aStatus == O_STATUS_EMPTY)
		{
			return i;
			break;
		}
	}

	return INVALID_OBJECT;
}

void gObjAdd(int aIndex, SOCKET uSocket, const char *sIpAddr) {
	if(g_Obj[aIndex].m_aStatus == O_STATUS_EMPTY)
	{
		g_Obj[aIndex].m_aStatus = O_STATUS_CONNECTED;
		g_Obj[aIndex].m_uSocket = uSocket;

		memset(g_Obj[aIndex].m_sIpAddr, 0, sizeof(g_Obj[aIndex].m_sIpAddr));
		strcpy_s(g_Obj[aIndex].m_sIpAddr, sizeof(g_Obj[aIndex].m_sIpAddr), sIpAddr);

		g_Obj[aIndex].m_uIdleTime = GetTickCount();
		g_Obj[aIndex].m_wsaBuff.buf = new char[MAX_NETWORK_BUFFER];
		g_Obj[aIndex].m_wsaBuff.len = MAX_NETWORK_BUFFER;

		DGHandshakeSend(aIndex);
	}
}

void gObjDel(int aIndex) {
	EnterCriticalSection(&CIocpSocket::m_IocpCriticalSection);

	if(g_Obj[aIndex].m_aStatus != O_STATUS_EMPTY)
	{
		g_Obj[aIndex].m_aStatus = O_STATUS_EMPTY;
		g_Obj[aIndex].m_uIdleTime = 0;
		//memset(g_Obj[aIndex].m_sIpAddr, 0, sizeof(g_Obj[aIndex].m_sIpAddr));
		closesocket(g_Obj[aIndex].m_uSocket);
		
		if(g_Obj[aIndex].m_wsaBuff.buf != NULL)
		{
			delete [] g_Obj[aIndex].m_wsaBuff.buf;
			g_Obj[aIndex].m_wsaBuff.buf = NULL;
		}

		g_Obj[aIndex].m_wsaBuff.len = 0;
	}

	LeaveCriticalSection(&CIocpSocket::m_IocpCriticalSection);
}

void gObjAccountCheck(int aIndex, PGAMESERVER_OBJECT pGSObj, char *sAccountPassword) 
{
	unsigned char uJoinResult = DATABASE_ERROR;

	if(sDatabase->MakeSyntax("CALL AccountLoginResult(@aResult, '%s', '%s');", pGSObj->m_sAccountID, sAccountPassword))
	{
		if(sDatabase->MakeSyntax("SELECT @aResult;"))
		{
			sDatabase->setResultsStored();

			MYSQL_ROW myRow = mysql_fetch_row(sDatabase->getMySQLRes());
			uJoinResult = atoi(myRow[0]);

			sDatabase->setResultsFree();
		}
	}

	DGAccountLoginResultSend(aIndex, pGSObj, uJoinResult);
}

void gObjAccountSessionSave(int aIndex, const char *sAccountID, const char *sIpAddr, const char *sGameServerName) 
{
	unsigned char uSaveResult = DATABASE_ERROR;

	if(sDatabase->MakeSyntax("CALL EEEEEE(@aResult, '%s', '%s', '%s');", sAccountID, sIpAddr, sGameServerName))
	{
		if(sDatabase->MakeSyntax("SELECT @aResult;"))
		{
			sDatabase->setResultsStored();

			MYSQL_ROW myRow = mysql_fetch_row(sDatabase->getMySQLRes());
			uSaveResult = atoi(myRow[0]);

			sDatabase->setResultsFree();
		}
	}

	DGAccountSessionSaveResultSend(aIndex, uSaveResult);
}

void gObjAccountLogout(unsigned char *pRecv) 
{
	PMSG_ACCOUNT_JOINOUT *msgAccJoinOut = (PMSG_ACCOUNT_JOINOUT*)pRecv;
	sDatabase->MakeSyntax("UPDATE accounts SET loggedin = 0 WHERE name = '%s'", msgAccJoinOut->m_sAccountID);
}

void gObjSendNickID(int aIndex, unsigned char *pRecv)
{
	PMSG_AccountNickIDRequest *MsgRequestNickID = (PMSG_AccountNickIDRequest*)pRecv;
	PMSG_NICK_ID_SEND MsgJoinID = {0};
	char sUserAccountID[10] = {0};
	safecpy(sUserAccountID,MsgRequestNickID->m_sAccountID,10);
	HeadSetA(&MsgJoinID.m_Head, 0xC1, sizeof(MsgJoinID), PROTO_NICKID);
	if(sDatabase->MakeSyntax("SELECT `id` , `nick` FROM `accounts` WHERE name = '%s'", sUserAccountID))
	{
		sDatabase->setResultsStored();
		MYSQL_ROW myRow = mysql_fetch_row(sDatabase->getMySQLRes());
		MsgJoinID.m_UserIndex = MsgRequestNickID->m_aIndex;
		MsgJoinID.m_uID = atoi(myRow[0]);
		safecpy(MsgJoinID.m_uNick,myRow[1],20);
	}
	sDatabase->setResultsFree();
	CIocpSocket::IocpDataSend(aIndex,(unsigned char *)&MsgJoinID, sizeof(MsgJoinID));
}

void gObjGetSearchResults(int aIndex, unsigned char *pRecv)
{
	PMSG_FIND_USER_REQUEST *msgSearch = (PMSG_FIND_USER_REQUEST *)pRecv;
	PMSG_DBSEARCHResponse SearchResults = {0};
	HeadSetA(&SearchResults.m_Head, 0xC1, sizeof(SearchResults), 0x04);
	SearchResults.m_aIndex = msgSearch->m_aIndex;
	MYSQL_ROW row;
	switch(msgSearch->m_Option)
	{
		case 0x01:
			sDatabase->MakeSyntax("SELECT `id`, `nick`, `name`, `email` FROM `accounts` WHERE `id` = '%s'", msgSearch->m_ValueSearch);
			break;

		case 0x02:
			sDatabase->MakeSyntax("SELECT `id`, `nick`, `name`, `email` FROM `accounts` WHERE `nick` LIKE '%%%s%%'", msgSearch->m_ValueSearch);
			break;

		case 0x03:
			sDatabase->MakeSyntax("SELECT `id`, `nick`, `name`, `email` FROM `accounts` WHERE `email` = '%s'", msgSearch->m_ValueSearch);
			break;
	}
	sDatabase->setResultsStored();
	SearchResults.howManyRecords = (int)sDatabase->getDataRowsCount();
	while( (row = mysql_fetch_row(sDatabase->getMySQLRes())) )
	{
		SearchResults.userID = atoi(row[0]); // id
		safecpy(SearchResults.nickname, row[1],20); // nickname
		safecpy(SearchResults.username, row[2],10); // username
		safecpy(SearchResults.email, row[3],25); // email
		CIocpSocket::IocpDataSend(aIndex, (unsigned char *)&SearchResults, 62);
	}
}
void gObjContactListSend(int aIndex,int ID,int UserIndex) 
{
	PMSG_CONATCTLIST_RESULT msgConactList = {0};
	msgConactList.m_aIndex = UserIndex;
	msgConactList.m_uSubType = PROTO_CONTACTLIST_REQUEST;
	HeadSetA(&msgConactList.m_Head, 0xC1, 0xC, PROTO_CONTACTLIST);
	
	sDatabase->MakeSyntax("SELECT ContactID FROM contacts WHERE AccountID = '%d'",ID);
	sDatabase->setResultsStored();
	int ContactCount = (unsigned char)sDatabase->getDataRowsCount();

	if(ContactCount > MAX_CONTACTS)
	{
			ContactCount = 5;
			sLog->outString(LOG_SYSTEM, "[WARNING] AccountID(%d) has more than max contacts!\n", ID);
	}

	msgConactList.m_uContactListCount = ContactCount;
	for(int i = 0; i != ContactCount ; i++)
	{
		MYSQL_ROW myRow = mysql_fetch_row(sDatabase->getMySQLRes());

		memset(&msgConactList.m_Contacts[i],0,sizeof(PMSG_CONTACTLIST));
		
		msgConactList.m_Contacts[i].u_ID = atoi(myRow[0]);
		msgConactList.m_Head.m_uLen += (unsigned char)sizeof(PMSG_CONTACTLIST);
	}
	for(int i = 0 ; i != ContactCount ; i++)
	{
		sDatabase->MakeSyntax("SELECT nick FROM accounts WHERE id = '%d'",msgConactList.m_Contacts[i].u_ID);
		sDatabase->setResultsStored();
		MYSQL_ROW sNickRow = mysql_fetch_row(sDatabase->getMySQLRes());
		strcpy_s(msgConactList.m_Contacts[i].u_Nick,20,sNickRow[0]);
	}
	sDatabase->setResultsFree();

	CIocpSocket::IocpDataSend(aIndex, (unsigned char*)&msgConactList, msgConactList.m_Head.m_uLen);
}

void gObjAddNewContact(int OwnerID,int TargetID)
{
	sDatabase->MakeSyntax("CALL AddContact('%d', '%d');", OwnerID,TargetID);
}

void gObjCheckObjectsIdleTime(unsigned long uTick) {
	for(int i = 0; i < OBJECT_MAX; i++)
	{
		if(g_Obj[i].m_aStatus == O_STATUS_CONNECTED)
		{
			if((uTick - g_Obj[i].m_uIdleTime) >= MAXIDLETIMEMS)
			{
				sLog->outString(LOG_CONNECTION, "[%d] :: Exceeded max idle time. Disconnected.\n", i);
				gObjDel(i);
			}
		}
	}
}