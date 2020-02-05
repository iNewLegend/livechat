/* 
 * ###################################################################################################################
 * ## Developer : [CzF]Leo123 (Leonid Vinikov)
 * ## MSN : loniao@walla.co.il 
 * ## email : leo1234u@gmail.com
 * ## Create Date : ý08/ý10/ý2008
 * ## FileName : ContactList.h
 * ## Info : Contact List system.
 * ## FileVersion : 0.1
 * ###################################################################################################################
 */

#ifndef	CHATSERVER_CONTACTLIST_H
#define CHATSERVER_CONTACTLIST_H
#include "packets.h"

struct ContactList
{
	int aIndex;
	int ID;
	char Nick[20];
	bool IsConnected;
};

void	ContactListRequestContactsList(int aIndex);
void	ContactListSendContacts(int aIndex);
void	ContactListRequestResult(unsigned char *pRecv);
void	ContactListSendChangeState(int aIndex,bool State,int ID);
void	ContactListChangeState(int aIndex,bool State);
void	ContactListRequestNewContact(int aIndex,unsigned char *pRecv);
void	ContactListAddContact(int aIndex,int TargetID);

#endif /*CHATSERVER_CONTACTLIST_H*/