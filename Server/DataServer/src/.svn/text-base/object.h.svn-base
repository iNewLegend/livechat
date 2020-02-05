#ifndef DATASERVER_OBJECT_H
#define DATASERVER_OBJECT_H

#include "protocol.h"
#include "packets.h"
#include <winsock2.h>

#define OBJECT_MAX		100
#define INVALID_OBJECT	-1
#define MAXIDLETIMEMS	15000

enum OBJECT_STATUS {
	O_STATUS_EMPTY = 0,
	O_STATUS_CONNECTED,
	O_STATUS_HANDSHAKED,
};

typedef struct OBJECTSTRUCT {
	SOCKET			m_uSocket;
	char			m_sIpAddr[16];
	WSAOVERLAPPED	m_wsaOverlapped;
	WSABUF			m_wsaBuff;
	int				m_aStatus;
	unsigned long	m_uIdleTime;
} *PCLIENTOBJECTSTRUCT;

extern OBJECTSTRUCT g_Obj[OBJECT_MAX];

int		gObjSearchFree();
void	gObjAdd(int aIndex, SOCKET uSocket, const char *sIpAddr);
void	gObjDel(int aIndex);
void	gObjAccountCheck(int aIndex, PGAMESERVER_OBJECT pGSObj, char *sAccountPassword);
void	gObjAccountLogout(unsigned char *pRecv);
void	gObjAccountSessionSave(int aIndex, const char *sAccountID, const char *sIpAddr, const char *sGameServerName);
void	gObjSendNickID(int aIndex, unsigned char *pRecv);
void	gObjCheckObjectsIdleTime(unsigned long uTick);
void	gObjGetSearchResults(int aIndex, unsigned char *pRecv);
void	gObjContactListSend(int aIndex,int ID,int UserIndex);
void	gObjAddNewContact(int OwnerID,int TargetID);

#endif //DATASERVER_OBJECT_H