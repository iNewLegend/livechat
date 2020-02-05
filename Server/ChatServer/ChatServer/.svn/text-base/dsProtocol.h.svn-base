#ifndef CHATSERVER_DSPROTOCOL_H
#define CHATSERVER_DSPROTOCOL_H

#include <winsock2.h>
#include "object.h"
#include "protocol.h"

enum DSPROTOCOL_ENUM {
	DSPROTO_HELLO				= 0x01,
	DSPROTO_ACCOUNTJOIN			= 0xF1,
	DSPROTO_NICKID				= 0x02,
	DSPROTO_CONTACTLIST			= 0x03,
	DSPROTO_SEARCHRESULTS		= 0x04,
};
enum DSPROTO_CONTACTLIST {
	DSPROTO_CONTACTLIST_REQUEST			= 0x00,
	DSPROTO_CONTACTLIST_ADDCONTACT		= 0x01,
};
enum DSPROTO_ACCOUNTJOIN {
	DSPROTO_ACCOUNT_LOGIN_REQUEST = 0x01,
	DSPROTO_ACCOUNT_NICKID_REQUEST,
	DSPROTO_ACCOUNTJOIN_SESSION_INFO_SAVE_RESULT,
};

#define DATABASE_ERROR -1

void	DSProtocolCore(unsigned char *pRecv, int aLen);

// --- Send
void	DS_S_HelloSend();
void	DS_S_RequestAccountLogin(int aIndex, const char *sAccountID, const char *sPassword);
void	DS_S_RequestAccountLogOut(const char *sAccountID);
void	DS_S_RequestNickID(int aIndex,const char *sAccountID);
void	DS_S_RequestSearchResults(int aIndex, char *Value,USER_DEFINE Option);
void	DS_S_RequestContactList(int aIndex,int ID);
void	DS_S_AddContact(int OwnerID,int TargetID);

// --- Recv
void	DS_R_HelloSend(unsigned char *pRecv);
void	DS_R_AccontJoinResult(unsigned char *pRecv);
void	DS_R_NickIDResult(unsigned char *pRecv);
void	DS_R_SEARCHRESULTS(unsigned char *pRecv);
void	DS_R_ContactListRequestResult(unsigned char* pRecv);

#endif //CHATSERVER_DSPROTOCOL_H