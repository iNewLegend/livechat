/* 
 * ###################################################################################################################
 * ## Developer : Leonid Vinikov (leo123)
 * ## MSN : loniao@walla.co.il
 * ## email : czf.leo123@gmail.com
 * ## Create Date : 22:52 09/10/2008
 * ## Last Edit :   3:49  1/23/2009
 * ## FileName : Messages.h
 * ## Info : Chat system.
 * ## FileVersion : 0.2
 * ###################################################################################################################
 */

#ifndef	CHATSERVER_MESSAGES_H
#define CHATSERVER_MESSAGES_H
#include "packets.h"

// Send
void	MessagesSendMessage(int aIndex, int tIndex, char *uText,int Len);
void	MessagesSendTyping(int aIndex,bool State,int TyperID);
void	MessagesSendMessageBox(int aIndex, int uType, char* uText);

// Recv
void	MessagesRecvMessage(int aIndex, unsigned char *pRecv);
void	MessagesRecvTypingUser(int aIndex, unsigned char *pRecv);
void	MessagesRecvChangeAway(int aIndex, unsigned char *pRecv);
void	MessagesRecvRequestAway(int aIndex , unsigned char *pRecv);

#endif /*CHATSERVER_MESSAGES_H*/