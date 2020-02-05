/* 
 * ###################################################################################################################
 * ## Developer : Leonid Vinikov (leo123)
 * ## MSN : loniao@walla.co.il
 * ## email : czf.leo123@gmail.com
 * ## Create Date : UNKNOW
 * ## Last Edit :   3:49  1/23/2009
 * ## FileName : Object.h
 * ## Info : contain the User Object struct
 * ## FileVersion : 0.3
 * ###################################################################################################################
 */
#ifndef CHATSERVER_OBJECT_H
#define CHATSERVER_OBJECT_H

#include <winsock2.h>
#include "packets.h"
#include "ContactList.h"
#include "Messages.h"

#define OBJECT_START_INDEX 1
#define OBJECT_MAX 1000
#define OBJECT_INVALID -1


enum OBJECT_STATUS_ENUM {
	OBJ_STATUS_EMPTY		= 0,
	OBJ_STATUS_CONNECTED,
	OBJ_STATUS_LOGGED_IN,
};

int		ObjectSearchFreeIndex();
void	ObjectProc(unsigned long uTick);
void	ObjectAdd(int aIndex, SOCKET uSocket, const char *sIpAddr);
void	ObjectDelete(int aIndex);
void	ObjectJoinOut(int aIndex);
void	ObjectJoinAttempt(int aIndex, const char *sAccountID, const char *sPassword);
void	ObjectJoinResult(int aIndex, const char *sAccountID, unsigned char uJoinResult);
void	ObjectRequstNick(int aIndex,int uTypeRequst);	
void	ObjectRequestSearchResults(int aIndex, unsigned char *pRecv, int uTypeRequest);
int		ObjectGetIndexByID(int ID);
bool	ObjectIsConnected(int aIndex);
bool	ObjectIsLogged(int aIndex);

typedef struct OBJECTSTRUCT {
	int				aIndex;
	SOCKET			ObjectSocket;
	char			IpAddress[16];
	WSAOVERLAPPED	wsaOverlapped;
	WSABUF			wsaBuff;
	int				Contacts_Count;
	int				ObjectStatus;
	unsigned long	IdleTime;
	int				LoginAttmpCount;
	char			AccountName[11];
	int				uID;
	ContactList		uContacts[MAX_CONTACTS];
	char			Nick[20];
	int				uWaitForOpenConntactList;
	int				uWaitForContactList;
	unsigned char   AwayMessage[MAX_AWAY_SIZE];
	int				AwayMessageLen;
	bool			IsHasAway;
} *UserObject;

extern OBJECTSTRUCT g_Obj[OBJECT_MAX];

#endif //CHATERVER_OBJECT_H