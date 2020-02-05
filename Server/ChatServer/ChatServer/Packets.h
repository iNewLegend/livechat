/* 
 * ###################################################################################################################
 * ## Developer : Leonid Vinikov (leo123)
 * ## MSN : loniao@walla.co.il
 * ## email : czf.leo123@gmail.com
 * ## Create Date : unknow
 * ## Last Edit :   3:49  1/23/2009
 * ## FileName : Packets.h
 * ## Info : contain all packets structs
 * ## FileVersion : 0.2
 * ###################################################################################################################
 */

#ifndef PACKETS_H
#define PACKETS_H

#include <windows.h>

#pragma pack(push, 1)

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

typedef struct PMSG_CONTACTLIST_RESULT {
	int				u_ID;
	bool			u_State;
	char			u_Nick[20];
	unsigned char	u_Spector;
} *PPMSG_CONTACTLIST_RESULT;

#define MAX_CONTACTS 2000
#define MAX_MESSAGE_SIZE 500
#define MAX_AWAY_SIZE 300

//--------------------------------------------------------------------------------------------
/* ---------------------- ChatServer TO Client / Client TO ChatServer ----------------------*/
namespace SC_CS_PROTOCOL {
	struct PMSG_AccountJoinResult 
	{
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		unsigned char	m_uJoinResult;
	};
	struct PMSG_ShowContactList
	{
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		unsigned char	m_uTypeRequest;	
	};
	struct PMSG_CONATCTLIST_RESULT {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		PMSG_CONTACTLIST_RESULT	m_Contacts[MAX_CONTACTS];
	};
	struct PMSG_CONTACTLIST_CHANGESTATE {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		int					m_uID;
		bool				m_uState;
	};
	struct PMSG_CONTACTLIST_ADD_CONTACT {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		bool				m_uState;
		int					m_uID;
		char				m_Nick[20];
	};
	struct PMSG_MESSAGE_SENDMESSAGE { 
		PMSG_HEADA			m_Head; // 2
		unsigned char		m_uSubType; // 3
		int					m_uID; // 7
		int					m_iLen;
		unsigned char		m_uText[MAX_MESSAGE_SIZE];
	};
	struct PMSG_MESSAGE_MSGBOXSEND {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		int					m_uType;
		char				m_uText[MAX_MESSAGE_SIZE];
	};
	struct PMSG_MESSAGE_TYPINGUSER_SEND {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		bool				m_uState;
		int					m_uID;
	};
	struct PMSG_MESSAGE_AWAY_SEND {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		int					m_uID;
		int					m_iLen;
		unsigned char		m_uText[MAX_AWAY_SIZE];
	};
	struct PMSG_OTHER_FINDUSER_RESULT {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		short				m_aIndex;
		short				userID;
		char				nickname[20];
		char				username[10];
		char				email[25];
		unsigned char		howManyRecords;
	};
	/* .....................::: Received FROM Client :::.....................  */

	struct PMSG_OHTER_FINDUSER_REQUEST {
		unsigned char		m_SubProto;
		unsigned char		m_uSubType;
		unsigned char		m_Option;
		char				m_sSearchValue[20];
	};
	struct PMSG_MESSAGE_SENDMESSAGE_REQUEST {
		unsigned char		m_SubProto;
		unsigned char		m_uSubType;
		int					m_uID; // 7
		int					m_iLen;
		unsigned char		m_uText[MAX_MESSAGE_SIZE];
	};
	struct PMSG_MESSAGE_TYPINGUSER_REUQEST {
		unsigned char		m_SubProto;
		unsigned char		m_uSubType;
		bool				m_uState;
		int					m_uID;
	};
	struct PMSG_MESSSAGE_CHANGEAWAY_REQUEST {
		unsigned char		m_SubProto;
		unsigned char		m_uSubType;
		int					m_iLen;
		unsigned char		m_uText[MAX_AWAY_SIZE];
	};
	struct PMSG_MESSSAGE_AWAY_REQUEST {
		unsigned char		m_SubProto;
		unsigned char		m_uSubType;
		int					m_uID;
	};
	struct PMSG_CONTACTLIST_REQUST_CONTACT {
		unsigned char		m_SubProto;
		unsigned char		m_uSubType;
		int					m_TargetID;
	};


}
//----------------------------------------------------------------------------------------------------




//----------------------------------------------------------------------------------------------------
/* ---------------------- DataServer TO ChatServer / ChatServer TO DataServer ----------------------*/
namespace CSDS_DSCS_PROTOCOL {
	/* .....................::: Send TO DataServer :::.....................  */
	struct MSG_HELLO {
		PMSG_HEADA		m_Head;
		unsigned char	m_uHello;
	};
	struct PMSG_NICK_ID_RESULT_SEND {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		int				m_uID; //3 bytes? ? 4
		char			m_uNick[20];
	};

	struct PMSG_AccountLoginRequset {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		short			m_aIndex;
		char			m_sAccountID[10]; 
		char			m_sPassword[10];
	};

	struct PMSG_AccountNickIDRequest {
		PMSG_HEADA		m_Head;
		short			m_aIndex;
		char			m_sAccountID[10];
	};
	struct PMSG_FindUsersRequest {
		PMSG_HEADA		m_Head;
		short			m_aIndex;
		unsigned char	m_Option;
		char			m_ValueSearch[20];
	};
	struct PMSG_ACCOUNT_SESSION_SAVE {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		char			m_sAccountID[10];
		char			m_sIpAddr[15];
		char			m_sGameServerName[20];
	};

	struct PMSG_ACCOUNT_JOINOUT {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		char			m_sAccountID[10];
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
	/* .....................::: Received FROM DataServer :::.....................  */
	struct PMSG_ACCOUNT_JOIN_RESULT {
		PMSG_HEADA		m_Head;
		unsigned char	m_uSubType;
		short			m_aUserIndex;
		char			m_sAccountID[10];
		unsigned char	m_uResult;
	};
	struct PMSG_NICK_ID_RESULT {
		PMSG_HEADA		m_Head;
		int				m_uID;
		short			m_UserIndex;
		char			m_uNick[20];
	};
	struct PMSG_FIND_USERS_RESULT {
		PMSG_HEADA		m_Head;
		short			m_aIndex;
		short			userID;
		char			nickname[20];
		char			username[10];
		char			email[25];
		unsigned char	howManyRecords;
	};
	struct PMSG_CONATCTLIST_RESULT {
		PMSG_HEADA			m_Head;
		unsigned char		m_uSubType;
		int					m_aIndex;
		int					m_uContactListCount;
		PMSG_CONTACTLIST	m_Contacts[MAX_CONTACTS];
};
}
//----------------------------------------------------------------------------------------------------------

#pragma pack(pop)

inline void HeadSetA(PPMSG_HEADA pHead, unsigned char uHeader, unsigned char uLen, unsigned char uProtocolId)
{
	pHead->m_uHeader		= uHeader;
	pHead->m_uLen			= uLen;
	pHead->m_uProtocolId	= uProtocolId;
}

inline void HeadSetB(PPMSG_HEADB pHead, unsigned char uHeader, unsigned short uLen, unsigned char uProtocolId) 
{
	pHead->m_uHeader		= uHeader;
	pHead->m_uLenHi			= HIBYTE(uLen);
	pHead->m_uLenLo			= LOBYTE(uLen);
	pHead->m_uProtocolId	= uProtocolId;
}

#endif //PACKETS_H