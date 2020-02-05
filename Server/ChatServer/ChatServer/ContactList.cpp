#include "stdafx.h"
#include "ContactList.h"
#include "libmsg.h"
#include "Object.h"
#include "Packets.h"
#include "config.h"
#include "dsprotocol.h"
#include "protocol.h"
#include "network.h"
#include "Messages.h"

bool ContactListIsContactObjectExist(int aIndex,int TargetdID)
{
	for(int i = 0 ; i != MAX_CONTACTS ; i++)
		if(g_Obj[aIndex].uContacts[i].ID == TargetdID)
			return true;
	return false;
}

int ContactListSearchFreeContactSlot(int aIndex)
{
	return g_Obj[aIndex].Contacts_Count+1;
}
int ContactListAddContactObject(int aIndex,int TargetID)
{
	int TargetIndex = ObjectGetIndexByID(TargetID);
	int Free = ContactListSearchFreeContactSlot(aIndex);
	g_Obj[aIndex].Contacts_Count = Free;
	g_Obj[aIndex].uContacts[Free].aIndex = TargetIndex;
	g_Obj[aIndex].uContacts[Free].ID = TargetID;
	g_Obj[aIndex].uContacts[Free].IsConnected = ObjectIsConnected(TargetIndex);
	memcpy(g_Obj[aIndex].uContacts[Free].Nick,g_Obj[TargetIndex].Nick,20);
	return Free;
}
void ContactListRefreshStateStruct(int aIndex)
{
	int ContactCount = g_Obj[aIndex].Contacts_Count;

	for(int i = 0 ; i != ContactCount;i++)
	{
		int ID =  g_Obj[aIndex].uContacts[i].ID;
		g_Obj[aIndex].uContacts[i].IsConnected = ObjectIsConnected(ObjectGetIndexByID(ID));
	}
}
void ContactListChangeState(int aIndex,bool State)
{
	ContactListRefreshStateStruct(aIndex);
	int ContactsCount = g_Obj[aIndex].Contacts_Count;
	for(int i = 0 ; i != ContactsCount ; i++)
	{
		int ID = g_Obj[aIndex].uContacts[i].ID;
		if(g_Obj[aIndex].uContacts[i].IsConnected)
			ContactListSendChangeState(ObjectGetIndexByID(ID),State,g_Obj[aIndex].uID);
	}
}
/* ---------------------- Received  ---------------------- */
void ContactListRequestNewContact(int aIndex,unsigned char *pRecv)
{
	SC_CS_PROTOCOL::PMSG_CONTACTLIST_REQUST_CONTACT *msgContact = (SC_CS_PROTOCOL::PMSG_CONTACTLIST_REQUST_CONTACT*)pRecv;
	int TargetID = msgContact->m_TargetID;
	sLog->outString(LOG_SYSTEM, "[%d][%s] :: Request Contact ID : %d.\n",aIndex,g_Obj[aIndex].AccountName,TargetID);
	int TargetIndex = ObjectGetIndexByID(TargetID);
	if(aIndex == TargetIndex)
	{
		MessagesSendMessageBox(aIndex, MB_OK , "Error : You can't add your self");
		return;
	}
	if(TargetIndex == OBJECT_INVALID || !ObjectIsLogged(TargetIndex))
	{
		sLog->outString(LOG_SYSTEM, "[%d][%s] :: %d << offline || not allowed. \n",aIndex,g_Obj[aIndex].AccountName,TargetID);
		MessagesSendMessageBox(aIndex,MB_OK,"Error : you can't add this cotnact.\nThe cotnact is offline and not allow the offline adds.");
		return;
	}
	if(ContactListIsContactObjectExist(aIndex,TargetID))
	{
		MessagesSendMessageBox(aIndex, MB_OK, "Error : this is account is allready exist at your contact list.");
		return;
	}
	ContactListAddContact(aIndex,TargetID);
	ContactListAddContact(TargetIndex,g_Obj[aIndex].uID);
	char Temp[400] = {0};
	sprintf(Temp,"%s has been add you to ContactList",g_Obj[aIndex].Nick);
	MessagesSendMessageBox(TargetIndex , MB_OK , Temp);
}
void ContactListRequestContactsList(int aIndex)
{
	sLog->outString(LOG_SYSTEM, "[%d][%s] :: ContactList requested.\n",aIndex,g_Obj[aIndex].AccountName);
	DS_S_RequestContactList(aIndex,g_Obj[aIndex].uID);
	sLog->outString(LOG_SYSTEM, "[%d][%s] :: Send request for ContactList.\n",aIndex,g_Obj[aIndex].AccountName);
	g_Obj[aIndex].uWaitForContactList = 1;
}

void ContactListRequestResult(unsigned char *pRecv)
{
	CSDS_DSCS_PROTOCOL::PMSG_CONATCTLIST_RESULT *msgContactList = (CSDS_DSCS_PROTOCOL::PMSG_CONATCTLIST_RESULT *)pRecv;
	int aIndex = msgContactList->m_aIndex;

	const int ContactCount = msgContactList->m_uContactListCount;
	g_Obj[aIndex].Contacts_Count = ContactCount;
	for(int i = 0 ; i != ContactCount;i++)
	{
		int ID =  msgContactList->m_Contacts[i].u_ID;
		g_Obj[aIndex].uContacts[i].ID = ID;
		memcpy(g_Obj[aIndex].uContacts[i].Nick,msgContactList->m_Contacts[i].u_Nick,20);
		int UserIndex = ObjectGetIndexByID(ID);
		g_Obj[aIndex].uContacts[i].aIndex = UserIndex;
		if(g_Obj[aIndex].uContacts[i].aIndex > OBJECT_INVALID)
			g_Obj[aIndex].uContacts[i].IsConnected = ObjectIsConnected(UserIndex);
	}
	ContactListChangeState(aIndex,true);
	if(g_Obj[aIndex].uWaitForContactList == 1)
		g_Obj[aIndex].uWaitForContactList = 2;
	
}
/* ---------------------- Sended  ---------------------- */
void ContactListSendContacts(int aIndex)
{
	ContactListRefreshStateStruct(aIndex);
	SC_CS_PROTOCOL::PMSG_CONATCTLIST_RESULT msgContactList = {0};
	msgContactList.m_uSubType = S_SUBPROTO_ContactList_GetAllContactList;
	HeadSetA(&msgContactList.m_Head, 0xC1, 0x4, PROTO_CONTACTLIST);
	int ContactsCount = g_Obj[aIndex].Contacts_Count;
	for(int i = 0 ; i != ContactsCount; i++)
	{
		msgContactList.m_Contacts[i].u_ID = g_Obj[aIndex].uContacts[i].ID;
		memcpy(msgContactList.m_Contacts[i].u_Nick , g_Obj[aIndex].uContacts[i].Nick , 20);
		msgContactList.m_Contacts[i].u_State = g_Obj[aIndex].uContacts[i].IsConnected;
		msgContactList.m_Contacts[i].u_Spector = 0xFF;
		msgContactList.m_Head.m_uLen += sizeof(PMSG_CONTACTLIST_RESULT);
	}
	CIocpSocket::IocpDataSend(aIndex,(unsigned char*)&msgContactList,msgContactList.m_Head.m_uLen);
	sLog->outString(LOG_SYSTEM, "[%d][%s] :: ContactList sended.\n",aIndex,g_Obj[aIndex].AccountName);
}
void ContactListSendChangeState(int aIndex,bool State,int ID)
{
	SC_CS_PROTOCOL::PMSG_CONTACTLIST_CHANGESTATE msgChangeState = {0};
	HeadSetA(&msgChangeState.m_Head,0xC1,sizeof(msgChangeState),PROTO_CONTACTLIST);
	msgChangeState.m_uID = ID;
	msgChangeState.m_uState = State;
	msgChangeState.m_uSubType = S_SUBPROTO_ContactList_ChangeState;
	
	CIocpSocket::IocpDataSend(aIndex,(unsigned char*)&msgChangeState,sizeof(msgChangeState));
	sLog->outString(LOG_SYSTEM,"[%d][%s] :: Send that %s is %d\n",aIndex,g_Obj[aIndex].AccountName,g_Obj[ObjectGetIndexByID(ID)].AccountName,State);
}

void ContactListAddContact(int aIndex,int TargetID)
{
	SC_CS_PROTOCOL::PMSG_CONTACTLIST_ADD_CONTACT msgAddContact = {0};
	if(ObjectIsConnected(aIndex))
	{
		HeadSetA(&msgAddContact.m_Head,0xC1,sizeof(msgAddContact),PROTO_CONTACTLIST);
		int TargetIndex = ObjectGetIndexByID(TargetID);
		int Free = ContactListAddContactObject(aIndex,TargetID);
		msgAddContact.m_uState = g_Obj[aIndex].uContacts[Free].IsConnected;
		msgAddContact.m_uID = g_Obj[aIndex].uContacts[Free].ID;
		memcpy(msgAddContact.m_Nick,g_Obj[aIndex].uContacts[Free].Nick,20);
		msgAddContact.m_uSubType = S_SUBPROTO_ContactList_AddContact;
		CIocpSocket::IocpDataSend(aIndex,(unsigned char*)&msgAddContact,sizeof(msgAddContact));
		sLog->outString(LOG_SYSTEM, "[%d][%s] :: New Contact sended.\n",aIndex,g_Obj[aIndex].AccountName);
		DS_S_AddContact(g_Obj[aIndex].uID,TargetID);
	}
}