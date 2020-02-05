#include "stdafx.h"
#include "protocol.h"
#include "packets.h"
#include "object.h"
#include "network.h"
#include "database.h"
#include "libmsg.h"

void ProtocolCore(int aIndex, unsigned char *pRecv, int aLen) 
{
	switch(pRecv[2])
	{
	case PROTO_HANDSHAKE:
		GDHandshakeRecv(aIndex, pRecv);
		break;

	case PROTO_NICKID:
			gObjSendNickID(aIndex, pRecv);
			break;
	case PROTO_CONTACTLIST:
		{
			switch(pRecv[3])
			{
			case PROTO_CONTACTLIST_REQUEST:
				GDContactListRequestRecv(aIndex,pRecv);
				break;
			case PROTO_CONTACTLIST_ADDCONTACT:
				GDContactListAddNewContactRecv(aIndex,pRecv);
				break;
			}
			break;
		}
	case 0x04:
			gObjGetSearchResults(aIndex, pRecv);
			break;

	case PROTO_ACCOUNTJOIN:
		switch(pRecv[3])
		{
		case PROTO_ACCOUNTJOIN_JOININ:
			GDAccountLoginRequestRecv(aIndex, pRecv);
			break;

		case PROTO_ACCOUNTJOIN_SESSION_INFO:
			GDAccountSessionRecv(aIndex, pRecv);
			break;

		case PROTO_ACCOUNTJOIN_JOINOUT:
			gObjAccountLogout(pRecv);
			break;
		}
		break;
	default:
		sLog->outString(LOG_SYSTEM, "%x, %x, %x, %x, %x\n", pRecv[0], pRecv[1], pRecv[2], pRecv[3], pRecv[4]);
		break;
	}
}

// --------- protocol

// ---- Send
void DGHandshakeSend(int aIndex) {
	PMSG_HANDSHAKE msgHello = {0};
	HeadSetA(&msgHello.m_Head, 0xC1, sizeof(PMSG_HANDSHAKE), 0x01);
	msgHello.m_uHello = 0xDA;
	
	CIocpSocket::IocpDataSend(aIndex, (unsigned char*)&msgHello, 4);
}

void DGAccountLoginResultSend(int aIndex, PGAMESERVER_OBJECT pGSObj, unsigned char uResult) {
	PMSG_ACCOUNT_JOIN_RESULT msgAccJoinResult = {0};
	HeadSetA(&msgAccJoinResult.m_Head, 0xC1, 17, 0xF1);
	msgAccJoinResult.m_uSubType = 0x01;
	msgAccJoinResult.m_aUserIndex = pGSObj->m_aIndex;
	msgAccJoinResult.m_uResult = uResult;
	memcpy(&msgAccJoinResult.m_sAccountID, pGSObj->m_sAccountID, 10);

	CIocpSocket::IocpDataSend(aIndex, (unsigned char*)&msgAccJoinResult, 17);
}

void DGAccountSessionSaveResultSend(int aIndex, unsigned char uResult) {
	PMSG_ACCOUNT_SESSION_SAVE_RESULT msgSessionSaveResult = {0};
	HeadSetA(&msgSessionSaveResult.m_Head,0xC1, 0x05, 0xF1);
	msgSessionSaveResult.m_uSubType = 0x02;
	msgSessionSaveResult.m_uResult = uResult;

	CIocpSocket::IocpDataSend(aIndex, (unsigned char*)&msgSessionSaveResult, 5);
}

// ---- Recv
void GDHandshakeRecv(int aIndex, unsigned char *pRecv) {
	if(pRecv[3] == 0xDF)
	{
		g_Obj[aIndex].m_aStatus = O_STATUS_HANDSHAKED;
		sLog->outString(LOG_CONNECTION, "[%d] :: GameServer handshaked. Start data transmision.\n", aIndex);
	}
}

void GDAccountLoginRequestRecv(int aIndex, unsigned char *pRecv) {
	PMSG_ACCOUNT_JOININ *msgAccJoinIn = (PMSG_ACCOUNT_JOININ*)pRecv;
	
	GAMESERVER_OBJECT GSObj = {0};
	GSObj.m_aIndex = msgAccJoinIn->m_aIndex;
	safecpy(GSObj.m_sAccountID, (const char*)&msgAccJoinIn->m_sAccountID, 10);

	char sAccountPassword[11] = {0};
	safecpy(sAccountPassword, (const char*)&msgAccJoinIn->m_sPassword, 10);

	gObjAccountCheck(aIndex, &GSObj, sAccountPassword);
}

void GDAccountSessionRecv(int aIndex, unsigned char *pRecv) {
	PMSG_ACCOUNT_SESSION_SAVE *msgAccSession = (PMSG_ACCOUNT_SESSION_SAVE*)pRecv;


	char sAccountID[11] = {0};
	safecpy(sAccountID, (const char*)msgAccSession->m_sAccountID, 10);

	char sIpAddr[16] = {0};
	safecpy(sIpAddr, (const char*)msgAccSession->m_sIpAddr, 15);

	char sGameServerName[21] = {0};
	safecpy(sGameServerName, (const char*)msgAccSession->m_sGameServerName, 20);

	gObjAccountSessionSave(aIndex, sAccountID, sIpAddr, sGameServerName);
}

void GDContactListRequestRecv(int aIndex, unsigned char *pRecv) {
	PMSG_CONTACTLIST_REQUEST *msgContactList = (PMSG_CONTACTLIST_REQUEST*)pRecv;
	
	gObjContactListSend(aIndex,msgContactList->m_ID,msgContactList->m_aIndex);

}
void GDContactListAddNewContactRecv(int aIndex, unsigned char *pRecv) 
{
	PMSG_CONTACTLIST_ADD_CONTACT *msgAddContact = (PMSG_CONTACTLIST_ADD_CONTACT*)pRecv;
	gObjAddNewContact(msgAddContact->m_OwnerID,msgAddContact->m_TargetID);
}