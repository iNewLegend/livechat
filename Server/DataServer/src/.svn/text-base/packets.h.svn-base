#ifndef DATASERVER_PACKETS_H
#define DATASERVER_PACKETS_H

#include <windows.h>

#pragma pack(push, 1)
#define MAX_CONTACTS	2000

/* .....................::: Sub structs :::.....................  */
typedef struct PMSG_HEADA {
	unsigned char m_uHeader;
	unsigned char m_uLen;
	unsigned char m_uProtocolId;
} *PPMSG_HEADA;

typedef struct PMSG_HEADB {
	unsigned char m_uHeader;
	unsigned char m_uLenHi;
	unsigned char m_uLenLo;
	unsigned char m_uProtocolId;
} *PPMSG_HEADB;

typedef struct PMSG_CONTACTLIST {
	int			u_ID;
	char		u_Nick[20];
} *PPMSG_CONTACTLIST;

struct PMSG_CONATCTLIST_RESULT {
	PMSG_HEADA			m_Head;
	unsigned char		m_uSubType;
	int					m_aIndex;
	int					m_uContactListCount;
	PMSG_CONTACTLIST	m_Contacts[MAX_CONTACTS];
};
/* .....................::: Send TO ChatServer :::.....................  */
struct PMSG_HANDSHAKE {						// 0xC1, 0x04, 0x01
	PMSG_HEADA		m_Head;
	unsigned char	m_uHello;
};

struct PMSG_ACCOUNT_JOIN_RESULT {			// 0xC1, 0x11, 0xF1 0x01
	PMSG_HEADA		m_Head;
	unsigned char	m_uSubType;
	short			m_aUserIndex;
	char			m_sAccountID[10];
	unsigned char	m_uResult;
};

struct PMSG_ACCOUNT_SESSION_SAVE_RESULT {	// 0xC1, 0x05, 0xF1 0x02
	PMSG_HEADA		m_Head;
	unsigned char	m_uSubType;
	unsigned char	m_uResult;
};
struct PMSG_NICK_ID_SEND {
	PMSG_HEADA		m_Head;
	int				m_uID;
	short			m_UserIndex;
	char			m_uNick[20];
};
struct PMSG_DBSEARCHResponse {
	PMSG_HEADA		m_Head;
	short			m_aIndex;
	unsigned char	userID;
	char			nickname[20];
	char			username[10];
	char			email[25];
	unsigned char	howManyRecords;
};
/* .....................::: Received FROM ChatServer :::.....................  */
struct PMSG_ACCOUNT_JOININ {			// 0xC1, 26, 0xF1 0x01
	PMSG_HEADA		m_Head;
	unsigned char	m_uSubType;
	short			m_aIndex;
	char			m_sAccountID[10];
	char			m_sPassword[10];
};

struct PMSG_ACCOUNT_SESSION_SAVE {			// 0xC1 49 0xF1 0x02
	PMSG_HEADA		m_Head;
	unsigned char	m_uSubType;
	char			m_sAccountID[10];
	char			m_sIpAddr[15];
	char			m_sGameServerName[20];
};

struct PMSG_ACCOUNT_JOINOUT {			// 0xC1 0x0D 0xF1 0x03
	PMSG_HEADA		m_Head;
	unsigned char	m_uSubType;
	char			m_sAccountID[10];
};
struct PMSG_AccountNickIDRequest {
		PMSG_HEADA		m_Head;
		short			m_aIndex;
		char			m_sAccountID[10];
};

struct PMSG_FIND_USER_REQUEST {
		PMSG_HEADA		m_Head;
		short			m_aIndex;
		unsigned char	m_Option;
		char			m_ValueSearch[20];
};

struct PMSG_CONTACTLIST_REQUEST {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		int				m_ID;
		short			m_aIndex;
};

struct PMSG_CONTACTLIST_ADD_CONTACT {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		int				m_OwnerID;
		int				m_TargetID;
};

#pragma pack(pop)

inline void HeadSetA(PPMSG_HEADA pHead, unsigned char uHeader, unsigned char uLen, unsigned char uProtocolId) {
	pHead->m_uHeader	= uHeader;
	pHead->m_uLen		= uLen;
	pHead->m_uProtocolId	= uProtocolId;
}

inline void HeadSetB(PPMSG_HEADB pHead, unsigned char uHeader, unsigned short uLen, unsigned char uProtocolId) {
	pHead->m_uHeader		= uHeader;
	pHead->m_uLenHi			= HIBYTE(uLen);
	pHead->m_uLenLo			= LOBYTE(uLen);
	pHead->m_uProtocolId	= uProtocolId;
}

#endif //DATASERVER_PACKETS_H