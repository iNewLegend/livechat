#ifndef DATASERVER_PROTOCOL_H
#define DATASERVER_PROTOCOL_H

enum PROTOCOL_ENUM {
	PROTO_HANDSHAKE			= 0x01,
	PROTO_NICKID			= 0x02,
	PROTO_CONTACTLIST		= 0x03,
	PROTO_ACCOUNTJOIN		= 0xF1,
	PROTO_SEARCHRESULTS		= 0xD
};
enum PROTO_CONTACTLIST {
	PROTO_CONTACTLIST_REQUEST = 0x00,
	PROTO_CONTACTLIST_ADDCONTACT = 0x01,
};
enum PROTO_ACCOUNTJOIN_ENUM {
	PROTO_ACCOUNTJOIN_JOININ = 0x01,
	PROTO_ACCOUNTJOIN_SESSION_INFO = 0x02,
	PROTO_ACCOUNTJOIN_JOINOUT = 0x03
};

enum ACCOUNTJOIN_RESULT {
	JOINRESULT_FINE = 0,
	JOINRESULT_INVALID_ACCOUNT,
	JOINREUSLT_INVALID_PASSWORD,
	JOINRESULT_ACCOUNT_INUSE,
	JOINRESULT_ACCOUNT_BLOCKED
};



typedef struct GAMESERVER_OBJECT {
	short	m_aIndex;
	char	m_sAccountID[11];
} *PGAMESERVER_OBJECT;

void	ProtocolCore(int aIndex, unsigned char *pRecv, int aLen);

// ---- Send
void	DGHandshakeSend(int aIndex);
void	DGAccountLoginResultSend(int aIndex, PGAMESERVER_OBJECT pGSObj, unsigned char uResult);
void	DGAccountSessionSaveResultSend(int aIndex, unsigned char uResult);

// ---- Recv
void	GDHandshakeRecv(int aIndex, unsigned char *pRecv);
void	GDAccountLoginRequestRecv(int aIndex, unsigned char *pRecv);
void	GDAccountSessionRecv(int aIndex, unsigned char *pRecv);
void	GDContactListRequestRecv(int aIndex, unsigned char *pRecv);
void	GDContactListAddNewContactRecv(int aIndex, unsigned char *pRecv);

#endif //DATSERVER_PROTOCOL_H