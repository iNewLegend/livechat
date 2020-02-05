#include "stdafx.h"
#include "object.h"
#include "dsProtocol.h"
#include "Packets.h"
#include "network.h"
#include "config.h"
#include "shared.h"
#include "protocol.h"
#include "libmsg.h"

OBJECTSTRUCT g_Obj[OBJECT_MAX];


int ObjectSearchFreeIndex()
{
	if(sLoginCount->getCount() < g_Config.m_aMaxUsers)
		for(int i = OBJECT_START_INDEX ; i != OBJECT_MAX ; i++)
			if(g_Obj[i].ObjectStatus == OBJ_STATUS_EMPTY)
				return i;
	return OBJECT_INVALID;

}

void ObjectAdd(int aIndex, SOCKET uSocket, const char *sIpAddr)
{
	if(g_Obj[aIndex].ObjectStatus == OBJ_STATUS_EMPTY)
	{
		g_Obj[aIndex].ObjectStatus = OBJ_STATUS_CONNECTED;

		g_Obj[aIndex].aIndex = aIndex;
		strcpy_s(g_Obj[aIndex].IpAddress, sizeof(g_Obj[aIndex].IpAddress), sIpAddr);
		g_Obj[aIndex].ObjectSocket = uSocket;
		g_Obj[aIndex].IdleTime = GetTickCount();
		g_Obj[aIndex].LoginAttmpCount = 0;
		memset(g_Obj[aIndex].AccountName,0,11);
		g_Obj[aIndex].uWaitForOpenConntactList = 0;

		g_Obj[aIndex].wsaBuff.buf = new char[MAX_NETWORK_BUFFER];
		g_Obj[aIndex].wsaBuff.len = MAX_NETWORK_BUFFER;

		sLoginCount->Increase();

		UpdateInstanceWindowTitle();
	}
}

void ObjectDelete(int aIndex)
{
	EnterCriticalSection(&CIocpSocket::m_IocpCriticalSection);

	if(g_Obj[aIndex].ObjectStatus != OBJ_STATUS_EMPTY)
	{
		sLoginCount->Decrease();
		ObjectJoinOut(aIndex);
		ContactListChangeState(aIndex,false);
		g_Obj[aIndex].aIndex = 0;
		g_Obj[aIndex].uWaitForOpenConntactList = 0;
		g_Obj[aIndex].uWaitForContactList = 0;
		g_Obj[aIndex].ObjectStatus = OBJ_STATUS_EMPTY;
		g_Obj[aIndex].IdleTime = 0;
		memset(g_Obj[aIndex].AccountName, 0, 11);
		g_Obj[aIndex].LoginAttmpCount = 0;
		closesocket(g_Obj[aIndex].ObjectSocket);

		if(g_Obj[aIndex].wsaBuff.buf != NULL)
		{
			delete [] g_Obj[aIndex].wsaBuff.buf;
			g_Obj[aIndex].wsaBuff.buf = NULL;
		}

		g_Obj[aIndex].wsaBuff.len = 0;

		UpdateInstanceWindowTitle();
	}
	LeaveCriticalSection(&CIocpSocket::m_IocpCriticalSection);
}
bool ObjectIsConnected(int aIndex)
{
	if(aIndex == OBJECT_INVALID)
		return false;
	if(g_Obj[aIndex].ObjectStatus > OBJ_STATUS_CONNECTED)
		return true;
	return false;
}
bool ObjectIsLogged(int aIndex)
{
	if(g_Obj[aIndex].ObjectStatus == OBJ_STATUS_LOGGED_IN)
		return true;
	return false;

}
int ObjectGetIndexByID(int ID)
{
	for(int i = OBJECT_START_INDEX ; i != OBJECT_MAX;i++)
		if(g_Obj[i].uID == ID)
			return i;
	return OBJECT_INVALID;
}
void ObjectJoinAttempt(int aIndex, const char *sAccountID, const char *sPassword)
{
	sLog->outString(LOG_CONNECTION, "[%d][%s][%s][%s] :: Join attempt.\n", aIndex, sAccountID,sPassword, g_Obj[aIndex].IpAddress);

	if(g_Obj[aIndex].LoginAttmpCount < 3)
	{
		g_Obj[aIndex].LoginAttmpCount++;
		DS_S_RequestAccountLogin(aIndex, sAccountID, sPassword);
		sLog->outString(LOG_CONNECTION, "[%d][%s][%s][%s] :: Join requested.\n", aIndex, sAccountID,sPassword, g_Obj[aIndex].IpAddress);
	}
	else
	{
		CS_S_AccountJoinResultSend(aIndex, JR_TOMANYATTEMPTS);
		sLog->outString(LOG_CONNECTION, "[%d][%s][%s] :: Too many join attempts.\n", aIndex, sAccountID, g_Obj[aIndex].IpAddress);
	}
}
void ObjectJoinOut(int aIndex)
{
	if(g_Obj[aIndex].ObjectStatus > OBJ_STATUS_CONNECTED)
	{
		g_Obj[aIndex].ObjectStatus = OBJ_STATUS_CONNECTED;
		DS_S_RequestAccountLogOut(g_Obj[aIndex].AccountName);
		sLog->outString(LOG_GAME, "[%d][%s] :: Account logout.\n", aIndex, g_Obj[aIndex].AccountName);
	}
}

void ObjectJoinResult(int aIndex, const char *sAccountID, unsigned char uJoinResult) 
{
	switch(uJoinResult)
	{
	case JR_FINE:
		g_Obj[aIndex].ObjectStatus = OBJ_STATUS_LOGGED_IN;
		memcpy(&g_Obj[aIndex].AccountName, sAccountID, 10);
		sLog->outString(LOG_GAME, "[%d][%s][%s] :: Account joined. Saving session.\n", aIndex, g_Obj[aIndex].AccountName, g_Obj[aIndex].IpAddress);
		break;

	case JR_INVALIDPASSWORD:
		sLog->outString(LOG_GAME, "[%d][%s][%s] :: Invalid password.\n", aIndex, sAccountID, g_Obj[aIndex].IpAddress);
			break;

	case JR_ACCOUNTINUSE:
		sLog->outString(LOG_GAME, "[%d][%s][%s] :: Account is currently in use.\n", aIndex, sAccountID, g_Obj[aIndex].IpAddress);
		break;

	case JR_INVALIDACCOUNT:
		sLog->outString(LOG_GAME, "[%d][%s][%s] :: Invalid account.\n", aIndex, sAccountID, g_Obj[aIndex].IpAddress);
		break;

	case JR_ACCOUNTBLOCKED:
		sLog->outString(LOG_GAME, "[%d][%s][%s] :: Account is blocked.\n", aIndex, sAccountID, g_Obj[aIndex].IpAddress);
		break;

	case DATABASE_ERROR:
		uJoinResult = JR_CONNECTIONERROR;
		sLog->outString(LOG_GAME, "[%d][%s][%s] :: Connection error. Check DataServer.\n", aIndex, sAccountID, g_Obj[aIndex].IpAddress);
		break;
	}

	CS_S_AccountJoinResultSend(aIndex, uJoinResult);
}
void ObjectRequstNick(int aIndex,int uTypeRequst)
{
	if(uTypeRequst == 0)
	{
		sLog->outString(LOG_GAME,"[%d][%s] :: Requesting Nick and ID .\n",aIndex,g_Obj[aIndex].AccountName,g_Obj[aIndex].IpAddress); 
		DS_S_RequestNickID(aIndex, g_Obj[aIndex].AccountName);
	}
	else if(uTypeRequst == 1)
	{
		sLog->outString(LOG_SYSTEM,"[%d][%d][%s] :: Requested nick accepted.\n",aIndex,g_Obj[aIndex].uID,g_Obj[aIndex].AccountName,g_Obj[aIndex].Nick);
		CS_S_SendNickID(aIndex, (unsigned char *)g_Obj[aIndex].Nick, g_Obj[aIndex].uID);
		g_Obj[aIndex].uWaitForOpenConntactList = 1;
	}
}
void ObjectRequestSearchResults(int aIndex, unsigned char *pRecv, int uTypeRequest)
{
	if(uTypeRequest == 0)
	{
		SC_CS_PROTOCOL::PMSG_OHTER_FINDUSER_REQUEST *FindUsers = (SC_CS_PROTOCOL::PMSG_OHTER_FINDUSER_REQUEST *)pRecv;
		sLog->outString(LOG_SYSTEM, "[%d][%s][%s] :: Requesting search results.\n", aIndex, g_Obj[aIndex].AccountName, g_Obj[aIndex].IpAddress);
		DS_S_RequestSearchResults(aIndex, FindUsers->m_sSearchValue, (USER_DEFINE)FindUsers->m_Option);
	}
	if(uTypeRequest == 1)
	{
		sLog->outString(LOG_SYSTEM, "[%d][%s][%s] :: Received search results from database.\n", aIndex, g_Obj[aIndex].AccountName, g_Obj[aIndex].IpAddress);
		CS_S_SERACHRESULTS(aIndex, pRecv);
	}
}

void ObjectProc(unsigned long uTick)
{
	for(int i = OBJECT_START_INDEX ; i != OBJECT_MAX ; i++)
	{
		if(g_Obj[i].uWaitForOpenConntactList == 1)
		{
			sLog->outString(LOG_SYSTEM, "[%d][%s] :: Send request to open ContactList.\n",i,g_Obj[i].AccountName);
			g_Obj[i].uWaitForOpenConntactList = 2;
			CS_S_SendOpenContactList(i);
		}
		if(g_Obj[i].uWaitForContactList == 2)
		{
			sLog->outString(LOG_SYSTEM, "[%d][%s] :: Send ContactList.\n",i,g_Obj[i].AccountName);
			g_Obj[i].uWaitForContactList = 3;
			ContactListSendContacts(i);
		}

	}
}