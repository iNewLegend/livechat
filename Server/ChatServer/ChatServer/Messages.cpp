/* 
 * ###################################################################################################################
 * ## Developer : Leonid Vinikov (leo123)
 * ## MSN : loniao@walla.co.il
 * ## email : czf.leo123@gmail.com
 * ## Create Date : 22:52 09/10/2008
 * ## Last Edit :   3:49  1/23/2009
 * ## FileName : Messages.cpp
 * ## Info : Chat system.
 * ## FileVersion : 0.2
 * ###################################################################################################################
 */

#include "stdafx.h"
#include "Messages.h"
#include "libmsg.h"
#include "Object.h"
#include "Packets.h"
#include "config.h"
#include "dsprotocol.h"
#include "protocol.h"
#include "network.h"

/* ---------------------- Sended  ---------------------- */

void MessagesSendMessageBox(int aIndex, int uType, char* uText)
{
	SC_CS_PROTOCOL::PMSG_MESSAGE_MSGBOXSEND msgSendBox = {0};
	int uLen = sizeof(SC_CS_PROTOCOL::PMSG_MESSAGE_MSGBOXSEND) - strlen(uText);
	HeadSetA(&msgSendBox.m_Head, 0xC1, uLen, PROTO_MESSAGE);
	msgSendBox.m_uSubType = S_SUBPROTO_Message_SendMSGBOX;
	msgSendBox.m_uType = uType;
	memcpy(msgSendBox.m_uText, uText, strlen(uText));
	CIocpSocket::IocpDataSend(aIndex,(unsigned char*)&msgSendBox, uLen);

}

void MessagesSendMessage(int aIndex, int tIndex, char *uText,int Len)
{
	SC_CS_PROTOCOL::PMSG_MESSAGE_SENDMESSAGE msgSendMessage = {0};
	int uLen = sizeof(SC_CS_PROTOCOL::PMSG_MESSAGE_SENDMESSAGE) - Len;
	HeadSetA(&msgSendMessage.m_Head, 0xC1, uLen , PROTO_MESSAGE);
	msgSendMessage.m_uID = g_Obj[aIndex].uID;
	msgSendMessage.m_uSubType = S_SUBPORTO_Message_SendMessage;
	memcpy(msgSendMessage.m_uText,uText,Len);
	msgSendMessage.m_iLen = Len;
	CIocpSocket::IocpDataSend(tIndex,(unsigned char*)&msgSendMessage, uLen);
	//if(IsDebug)
	sLog->outString(LOG_SYSTEM,"[%d][%s] :: Sent message to %s (ID: %d).\n", aIndex, g_Obj[aIndex].AccountName, g_Obj[tIndex].AccountName, tIndex);
}

void MessagesSendAway(int aIndex, int tIndex, char *uText, int Len)
{
	SC_CS_PROTOCOL::PMSG_MESSAGE_AWAY_SEND msgAwayMessage = {0};
	int uLen = sizeof(SC_CS_PROTOCOL::PMSG_MESSAGE_AWAY_SEND) - Len;
	HeadSetA(&msgAwayMessage.m_Head, 0xC1, uLen , PROTO_MESSAGE);
	msgAwayMessage.m_uID = g_Obj[tIndex].uID;
	msgAwayMessage.m_uSubType = S_SUBPROTO_Message_SendAway;
	memcpy(msgAwayMessage.m_uText,uText,Len);
	msgAwayMessage.m_iLen = Len;
	CIocpSocket::IocpDataSend(aIndex,(unsigned char*)&msgAwayMessage, uLen);
	sLog->outString(LOG_SYSTEM,"[%d][%s] :: Sent Away to %s (ID: %d).\n", aIndex, g_Obj[aIndex].AccountName, g_Obj[tIndex].AccountName, tIndex);
}

void MessagesSendTyping(int aIndex,bool State,int TyperID)
{
	SC_CS_PROTOCOL::PMSG_MESSAGE_TYPINGUSER_SEND msgSendTyping = {0};
	HeadSetA(&msgSendTyping.m_Head, 0xC1 , sizeof(msgSendTyping), PROTO_MESSAGE);
	msgSendTyping.m_uSubType = S_SUBPORTO_Message_TypingUser;
	msgSendTyping.m_uID = TyperID;
	msgSendTyping.m_uState = State;
	CIocpSocket::IocpDataSend(aIndex,(unsigned char *)&msgSendTyping,sizeof(msgSendTyping));
	
}

/* ---------------------- Received  ---------------------- */

void MessagesRecvMessage(int aIndex, unsigned char *pRecv)
{
	SC_CS_PROTOCOL::PMSG_MESSAGE_SENDMESSAGE_REQUEST *msgRecvMessage = (SC_CS_PROTOCOL::PMSG_MESSAGE_SENDMESSAGE_REQUEST *)pRecv;
	char Temp[MAX_MESSAGE_SIZE] = {0};
	int tIndex = ObjectGetIndexByID(msgRecvMessage->m_uID);
	if(tIndex == OBJECT_INVALID)
	{
		sLog->outString(LOG_SYSTEM,"[%d][%s] :: (MessagesRecvMessage) INVAILD ID.\n",aIndex,g_Obj[aIndex].AccountName);
		return;
	}
	memcpy(Temp,msgRecvMessage->m_uText,msgRecvMessage->m_iLen);
	MessagesSendMessage(aIndex, tIndex, Temp,msgRecvMessage->m_iLen);
}

void MessagesRecvTypingUser(int aIndex, unsigned char *pRecv)
{
	SC_CS_PROTOCOL::PMSG_MESSAGE_TYPINGUSER_REUQEST *msgRecvTyping = (SC_CS_PROTOCOL::PMSG_MESSAGE_TYPINGUSER_REUQEST *)pRecv;
	int tIndex = ObjectGetIndexByID(msgRecvTyping->m_uID);
	if(tIndex == OBJECT_INVALID)
	{
		sLog->outString(LOG_SYSTEM,"[%d][%s] :: (MessagesRecvTypingUser) INVAILD ID.\n",aIndex,g_Obj[aIndex].AccountName);
		return;
	}
	MessagesSendTyping(tIndex,msgRecvTyping->m_uState,g_Obj[aIndex].uID);

}

void MessagesRecvChangeAway(int aIndex, unsigned char *pRecv)
{
	sLog->outString(LOG_SYSTEM,"[%d][%s] :: Changed Personal Message.\n",aIndex,g_Obj[aIndex].AccountName);
	SC_CS_PROTOCOL::PMSG_MESSSAGE_CHANGEAWAY_REQUEST *msgChangeAway = (SC_CS_PROTOCOL::PMSG_MESSSAGE_CHANGEAWAY_REQUEST *)pRecv;
	memcpy(g_Obj[aIndex].AwayMessage,msgChangeAway->m_uText,msgChangeAway->m_iLen);
	g_Obj[aIndex].AwayMessageLen = msgChangeAway->m_iLen;
	g_Obj[aIndex].IsHasAway = true;
}

void MessagesRecvRequestAway(int aIndex , unsigned char *pRecv)
{
	SC_CS_PROTOCOL::PMSG_MESSSAGE_AWAY_REQUEST *msgAwayRequest = (SC_CS_PROTOCOL::PMSG_MESSSAGE_AWAY_REQUEST *)pRecv;
	char Temp[MAX_AWAY_SIZE] = {0};
	int tIndex = ObjectGetIndexByID(msgAwayRequest->m_uID);
	if(tIndex == OBJECT_INVALID)
		return;
	if(g_Obj[tIndex].IsHasAway)
	{
		int iLen = g_Obj[tIndex].AwayMessageLen;
		memcpy(Temp,g_Obj[tIndex].AwayMessage,iLen);
		MessagesSendAway(aIndex,tIndex,Temp,iLen);
	}

}