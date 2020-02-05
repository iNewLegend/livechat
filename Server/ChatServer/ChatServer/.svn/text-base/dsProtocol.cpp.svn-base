#include "stdafx.h"
#include "dsprotocol.h"
#include "packets.h"
#include "protocol.h"
#include "config.h"
#include "object.h"
#include "libmsg.h"
#include "network.h"

void DSProtocolCore(unsigned char *pRecv, int aLen)
{
	if(pRecv[0] == 0xC1)
	{
		switch(pRecv[2])
		{
		case DSPROTO_HELLO:
			DS_R_HelloSend(pRecv);
			break;
		case DSPROTO_NICKID:
				DS_R_NickIDResult(pRecv);
				break;
		case DSPROTO_CONTACTLIST:
			switch(pRecv[3])
			{
			case DSPROTO_CONTACTLIST_REQUEST:
				DS_R_ContactListRequestResult(pRecv);
				break;
			}
			break;
		case DSPROTO_SEARCHRESULTS:
			DS_R_SEARCHRESULTS(pRecv);
			break;
		case DSPROTO_ACCOUNTJOIN:
			switch(pRecv[3])
			{
			case DSPROTO_ACCOUNT_LOGIN_REQUEST:
				DS_R_AccontJoinResult(pRecv);
				break;
			}
			break;
		default:
			sLog->outString(LOG_SYSTEM, "DSProtocolCore() - Invalid ProtocolID(%x) received.\n",pRecv[2]);
			break;
		}
	}
	else
		sLog->outString(LOG_SYSTEM, "DSProtocolCore() - Invalid ProtocolType(%x) received.\n",pRecv[0]);
}

/* ---------------------- Protocol ChatServer TO ChatServer/ChatServer TO DataServer  ---------------------- */

/* ---------------------- Send TO DataServer  ---------------------- */

void DS_S_HelloSend() 
{
	CSDS_DSCS_PROTOCOL::MSG_HELLO msgHello = {0};
	HeadSetA(&msgHello.m_Head, 0xC1, 0x04, 0x01);

	msgHello.m_uHello = 0xDF;

	sDataServerSock->DataSend((unsigned char*)&msgHello, 4);
}

void DS_S_RequestAccountLogin(int aIndex, const char *sAccountID, const char *sPassword)
{
	CSDS_DSCS_PROTOCOL::PMSG_AccountLoginRequset msgAccountLoginRequest = {0};
	HeadSetA(&msgAccountLoginRequest.m_Head, 0xC1, 27, DSPROTO_ACCOUNTJOIN);
	msgAccountLoginRequest.m_uSubType = DSPROTO_ACCOUNT_LOGIN_REQUEST;
	msgAccountLoginRequest.m_aIndex = aIndex;
	memcpy(msgAccountLoginRequest.m_sAccountID, sAccountID, 10);
	memcpy(msgAccountLoginRequest.m_sPassword, sPassword, 10);
	sDataServerSock->DataSend((unsigned char *)&msgAccountLoginRequest, sizeof(msgAccountLoginRequest));

}
void DS_S_RequestAccountLogOut(const char *sAccountID)
{
	CSDS_DSCS_PROTOCOL::PMSG_ACCOUNT_JOINOUT msgAccJoinOut = {0};
	HeadSetA(&msgAccJoinOut.m_Head, 0xC1, 14, 0xF1);

	msgAccJoinOut.m_uSubType = 0x03;
	memcpy(&msgAccJoinOut.m_sAccountID, sAccountID, 10);

	sDataServerSock->DataSend((unsigned char*)&msgAccJoinOut, sizeof(msgAccJoinOut));
}
void DS_S_RequestNickID(int aIndex,const char *sAccountID)
{
	CSDS_DSCS_PROTOCOL::PMSG_AccountNickIDRequest RequestNick = {0};
	HeadSetA(&RequestNick.m_Head, 0xC1, sizeof(RequestNick), 0x02);
	memcpy(RequestNick.m_sAccountID, sAccountID, 10);
	RequestNick.m_aIndex = aIndex;
	sDataServerSock->DataSend((unsigned char *)&RequestNick, sizeof(RequestNick));
}

void DS_S_RequestSearchResults(int aIndex, char *Value,USER_DEFINE Option)
{
	CSDS_DSCS_PROTOCOL::PMSG_FindUsersRequest msgFindUser = {0};
	HeadSetA(&msgFindUser.m_Head, 0xC1 ,sizeof(msgFindUser), DSPROTO_SEARCHRESULTS);
	msgFindUser.m_aIndex = aIndex;
	msgFindUser.m_Option = (unsigned char)Option;
	memcpy(msgFindUser.m_ValueSearch,Value,20);
	sDataServerSock->DataSend((unsigned char *)&msgFindUser, sizeof(msgFindUser));
}

void DS_S_RequestContactList(int aIndex,int ID)
{
	CSDS_DSCS_PROTOCOL::PMSG_CONTACTLIST_REQUEST msgContactList = {0};
	HeadSetA(&msgContactList.m_Head, 0xC1 ,sizeof(msgContactList),DSPROTO_CONTACTLIST);
	msgContactList.m_uSubType = DSPROTO_CONTACTLIST_REQUEST;
	msgContactList.m_aIndex = aIndex;
	msgContactList.m_ID = ID;

	sDataServerSock->DataSend((unsigned char *)&msgContactList ,sizeof(msgContactList));
}

void DS_S_AddContact(int OwnerID,int TargetID)
{
	CSDS_DSCS_PROTOCOL::PMSG_CONTACTLIST_ADD_CONTACT msgAddContact = {0};
	HeadSetA(&msgAddContact.m_Head , 0xC1 , sizeof(msgAddContact), DSPROTO_CONTACTLIST);
	msgAddContact.m_uSubType = DSPROTO_CONTACTLIST_ADDCONTACT;
	msgAddContact.m_OwnerID = OwnerID;
	msgAddContact.m_TargetID = TargetID;

	sDataServerSock->DataSend((unsigned char*)&msgAddContact,sizeof(msgAddContact));
}

/* ---------------------- Receive FROM DataServer  ---------------------- */

void DS_R_HelloSend(unsigned char *pRecv) 
{
	if(pRecv[3] == 0xDA)
	{
		DS_S_HelloSend();
	}
}

void DS_R_AccontJoinResult(unsigned char* pRecv)
{
	CSDS_DSCS_PROTOCOL::PMSG_ACCOUNT_JOIN_RESULT *msgAccJoinResult = (CSDS_DSCS_PROTOCOL::PMSG_ACCOUNT_JOIN_RESULT *)pRecv;

	char sAccountID[11] = {0};
	memcpy(sAccountID,msgAccJoinResult->m_sAccountID,10);

	if(msgAccJoinResult->m_aUserIndex >= OBJECT_START_INDEX && msgAccJoinResult->m_aUserIndex < OBJECT_MAX)
		ObjectJoinResult(msgAccJoinResult->m_aUserIndex, sAccountID, msgAccJoinResult->m_uResult);
	else
		sLog->outString(LOG_SYSTEM, "DS_R_AccontJoinResult() - Invalid aIndex(%d) received for login(%s).\n", msgAccJoinResult->m_aUserIndex, sAccountID);
}

void DS_R_NickIDResult(unsigned char* pRecv)
{
	CSDS_DSCS_PROTOCOL::PMSG_NICK_ID_RESULT *MsgNickIDResult = (CSDS_DSCS_PROTOCOL::PMSG_NICK_ID_RESULT *)pRecv;
	int aIndex = MsgNickIDResult->m_UserIndex;
	char sNick[21] = {0};
	memcpy(sNick, MsgNickIDResult->m_uNick, 20);

	if(MsgNickIDResult->m_UserIndex >= OBJECT_START_INDEX && MsgNickIDResult->m_UserIndex < OBJECT_MAX)
	{
		g_Obj[aIndex].uID = MsgNickIDResult->m_uID;
		memcpy(g_Obj[aIndex].Nick, sNick, 20);
		ObjectRequstNick(aIndex, 1);
	}
	else
		sLog->outString(LOG_SYSTEM, "DS_R_NickIDResult() - Invalid aIndex(%d) received for nick(%s).\n", MsgNickIDResult->m_UserIndex, sNick);
}
void DS_R_ContactListRequestResult(unsigned char* pRecv)
{
	CSDS_DSCS_PROTOCOL::PMSG_CONATCTLIST_RESULT *MsgContactList = (CSDS_DSCS_PROTOCOL::PMSG_CONATCTLIST_RESULT *)pRecv;
	if(MsgContactList->m_aIndex >= OBJECT_START_INDEX && MsgContactList->m_aIndex < OBJECT_MAX)
		ContactListRequestResult(pRecv);
	else
		sLog->outString(LOG_SYSTEM, "DS_R_ContactListRequestResult() - Invalid aIndex(%d) received for ContactList.\n", MsgContactList->m_aIndex);

}

void DS_R_SEARCHRESULTS(unsigned char *pRecv)
{
	CSDS_DSCS_PROTOCOL::PMSG_FIND_USERS_RESULT *FinalSearchResults = (CSDS_DSCS_PROTOCOL::PMSG_FIND_USERS_RESULT *)pRecv;
	int aIndex = FinalSearchResults->m_aIndex;

	if(FinalSearchResults->m_aIndex >= OBJECT_START_INDEX && FinalSearchResults->m_aIndex < OBJECT_MAX)
		ObjectRequestSearchResults(aIndex, pRecv, 1);
	else
		sLog->outString(LOG_SYSTEM, "DS_R_SEARCHRESULTS() - Invalid aIndex(%d) received.\n", FinalSearchResults->m_aIndex);
}