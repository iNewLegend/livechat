/* 
 * ###################################################################################################################
 * ## Developer : Leonid Vinikov (leo123)
 * ## MSN : loniao@walla.co.il
 * ## email : czf.leo123@gmail.com
 * ## Create Date : UNKNOW
 * ## Last Edit :   3:49  1/23/2009
 * ## FileName : protocol.h
 * ## Info : Protocol enums , defines , functions
 * ## FileVersion : 0.3
 * ###################################################################################################################
 */

#ifndef CHATSERVER_PROTOCOL_H
#define CHATSERVER_PROTOCOL_H

#include <winsock2.h>
#include "packets.h"
#include "object.h"

enum PROTO_ENUM {
	PROTO_LOGIN				= 0x00,
	PROTO_CONTACTLIST		= 0x01,
	PROTO_MESSAGE			= 0x02,
	PROTO_PROFIL,
	PROTO_OTHER
};
enum S_SUBPROTO_ContactList {						/* SEND */
	S_SUBPROTO_ContactList_AddContact =				0x00,
	S_SUBPROTO_ContactList_ChangeState =			0x01,
	S_SUBPROTO_ContactList_GetAllContactList =		0x02,
	S_SUBPROTO_ContactList_ShowContactList =		0x03,
};
enum R_SUBPROTO_ContactList {						/* RECV */
	R_SUBPROTO_ContactList_RequestContactList =		0x00,
	R_SUBPROTO_ContactList_RequestChangeState =		0x01,
	R_SUBPROTO_ContactList_RequestContact     =		0x02,
};
enum S_SUBPORTO_Message {							/* SEND */
	S_SUBPORTO_Message_SendMessage =				0x00,
	S_SUBPORTO_Message_TypingUser  =				0x01,
	S_SUBPROTO_Message_SendMSGBOX  =				0x02,
	S_SUBPROTO_Message_SendAway    =				0x03,
};
enum R_SUBPORTO_Message {							/* RECV	*/
	R_SUBPORTO_Message_SendMessage =				0x00,
	R_SUBPORTO_Message_TypingUser =					0x01,
	R_SUBPROTO_Message_ChangeAway =					0x02,
	R_SUBPROTO_Message_GetAway    =					0x03,
};
enum SUBPROTO_OTHER {								/* RECV	*/
	 SUBPROTO_OTHER_REQUESTNICKID					= 0x00,
	 SUBPROTO_OTHER_SEARCHUSERINFO					= 0x01,
};

enum ACCOUNTJOIN_RESULT {
	JR_INVALIDPASSWORD	= 0,
	JR_FINE				= 1,
	JR_INVALIDACCOUNT	= 2,
	JR_ACCOUNTINUSE		= 3,
	JR_SERVERISFULL		= 4,
	JR_ACCOUNTBLOCKED	= 5,
	JR_INVALIDVERSION	= 6,
	JR_CONNECTIONERROR	= 7,
	JR_TOMANYATTEMPTS	= 8,
	JR_NOCHARGEINFO		= 9,
	JR_AGERESTRICTS =	 17,
};

enum USER_DEFINE {
	USER_ID				= 1,
	USER_NICK			= 2,
	USER_EMAIL			= 3,
};

void	ProtocolCore(int aIndex, unsigned char *pRecv, int aLen);
/// --- Protocol

//------- Send
void	CS_S_AccountJoinResultSend(int aIndex, unsigned char uJoinResult);
void	CS_S_SendNickID(int aIndex, unsigned char *nick, unsigned int id);
void	CS_S_SendOpenContactList(int aIndex);
void	CS_S_SERACHRESULTS(int aIndex, unsigned char *pRecv);
// ------ Recv
void	CS_R_AccountJoinAttemptRecv(int aIndex, unsigned char *pRecv);
void	CS_R_RequestNickID(int aIndex);

#endif

