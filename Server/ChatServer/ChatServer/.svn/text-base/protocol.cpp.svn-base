/* 
 * ###################################################################################################################
 * ## Developer : Leonid Vinikov (leo123)
 * ## MSN : loniao@walla.co.il
 * ## email : czf.leo123@gmail.com
 * ## Create Date : UNKNOW
 * ## Last Edit :   3:56  1/23/2009
 * ## FileName : protocol.cpp
 * ## Info : Protocol build using to understand client packets.
 * ## FileVersion : 0.3
 * ###################################################################################################################
 */
#include "stdafx.h"
#include "protocol.h"
#include "packets.h"
#include "config.h"
#include "object.h"
#include "network.h"
#include "libmsg.h"
#include "shared.h"
#include <cstring>

#include <winsock2.h>

void ProtocolCore(int aIndex, unsigned char *pRecv, int aLen)
{
	switch(pRecv[0])
	{
	case PROTO_LOGIN:
		CS_R_AccountJoinAttemptRecv(aIndex,pRecv);
		break;

	#pragma region ContactList
	case PROTO_CONTACTLIST:
		switch(pRecv[1])
		{
			case R_SUBPROTO_ContactList_RequestContactList:
				ContactListRequestContactsList(aIndex);
				break;
			case R_SUBPROTO_ContactList_RequestContact:
				ContactListRequestNewContact(aIndex,pRecv);
				break;
		}
		break;
	#pragma endregion ContactList

	case PROTO_MESSAGE:
		switch(pRecv[1])
		{
			case R_SUBPORTO_Message_SendMessage:
				MessagesRecvMessage(aIndex, pRecv);
				break;
			case R_SUBPORTO_Message_TypingUser:
				MessagesRecvTypingUser(aIndex, pRecv);
				break;
			case R_SUBPROTO_Message_ChangeAway:
				MessagesRecvChangeAway(aIndex, pRecv);
				break;
			case R_SUBPROTO_Message_GetAway:
				MessagesRecvRequestAway(aIndex, pRecv);
				break;
		}
		break;
	case PROTO_OTHER: 
		switch(pRecv[1])
		{
			case SUBPROTO_OTHER_REQUESTNICKID:
				ObjectRequstNick(aIndex,0);
				break;
			case SUBPROTO_OTHER_SEARCHUSERINFO:
				ObjectRequestSearchResults(aIndex, pRecv, 0);
				break;
		}
		break;
	default:
		sLog->outString(LOG_SYSTEM, "ProtocolCore() [%d][%s] :: invalid PacketID :: [%X][%X] .\n", aIndex, g_Obj[aIndex].IpAddress,pRecv[0], pRecv[1]);
		break;
	}
}
/* ---------------------- Protocol Client TO ChatServer/ChatServer TO Client  ---------------------- */
/* ---------------------- Send TO Client  ---------------------- */
void CS_S_AccountJoinResultSend(int aIndex,unsigned char uJoinResult)
{
	SC_CS_PROTOCOL::PMSG_AccountJoinResult msgJoinResult = {0};
	HeadSetA(&msgJoinResult.m_Head, 0xC1, 5, PROTO_LOGIN);
	msgJoinResult.m_uJoinResult = uJoinResult;
	CIocpSocket::IocpDataSend(aIndex,(unsigned char*)&msgJoinResult, sizeof(msgJoinResult));
}

void CS_S_SendNickID(int aIndex, unsigned char *nick, unsigned int id)
{
	CSDS_DSCS_PROTOCOL::PMSG_NICK_ID_RESULT_SEND NickID = {0};
	NickID.m_uID = id;
	strcpy_s(NickID.m_uNick, 20, (const char *)nick);
	NickID.m_uSubType = SUBPROTO_OTHER_REQUESTNICKID;
	HeadSetA(&NickID.m_Head, 0xC1, 27, PROTO_OTHER);
	CIocpSocket::IocpDataSend(aIndex, (unsigned char *)&NickID, sizeof(NickID));
}
void CS_S_SendOpenContactList(int aIndex)
{
	SC_CS_PROTOCOL::PMSG_ShowContactList msgShowContactList = {0};
	msgShowContactList.m_uSubType = S_SUBPROTO_ContactList_ShowContactList;
	msgShowContactList.m_uTypeRequest = 4;
	HeadSetA(&msgShowContactList.m_Head, 0xC1 ,sizeof(msgShowContactList),PROTO_CONTACTLIST);
	CIocpSocket::IocpDataSend(aIndex, (unsigned char *)&msgShowContactList,sizeof(msgShowContactList));
}
void CS_S_SERACHRESULTS(int aIndex, unsigned char *pRecv)
{
	CSDS_DSCS_PROTOCOL::PMSG_FIND_USERS_RESULT *FinalSearchResults = (CSDS_DSCS_PROTOCOL::PMSG_FIND_USERS_RESULT *)pRecv;
	SC_CS_PROTOCOL::PMSG_OTHER_FINDUSER_RESULT msgSearchResult = {0};
	HeadSetA(&msgSearchResult.m_Head, 0xC1, sizeof(CSDS_DSCS_PROTOCOL::PMSG_FIND_USERS_RESULT), PROTO_OTHER);
	msgSearchResult.m_uSubType = SUBPROTO_OTHER_SEARCHUSERINFO;
	msgSearchResult.m_aIndex = FinalSearchResults->m_aIndex;
	msgSearchResult.userID = FinalSearchResults->userID;
	memcpy(msgSearchResult.nickname,FinalSearchResults->nickname,20);
	memcpy(msgSearchResult.username,FinalSearchResults->username,10);
	memcpy(msgSearchResult.email,FinalSearchResults->email,25);
	msgSearchResult.howManyRecords = FinalSearchResults->howManyRecords;
	CIocpSocket::IocpDataSend(aIndex, (unsigned char *)&msgSearchResult, sizeof(SC_CS_PROTOCOL::PMSG_OTHER_FINDUSER_RESULT));
}
/* ---------------------- Receive FROM Client  ---------------------- */
void CS_R_AccountJoinAttemptRecv(int aIndex, unsigned char *pRecv) 
{
	char sAccountID[11] = {0};
	memcpy(sAccountID, &pRecv[1], 10);

	char sPassword[11] = {0};
	memcpy(sPassword, &pRecv[11], 10);

	ObjectJoinAttempt(aIndex, sAccountID, sPassword);
}
