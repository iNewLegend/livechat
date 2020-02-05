using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveChatClient
{
    public static class Packets
    {
        public const int C1HeaderSize = 3;
        public const int C2HeaderSize = 2;
        public enum PacketTypes
        {
            C1_SIZE_BYTE = 0xC1,
            C2_SIZE_WORD = 0xC2,
        }
        public enum ACCOUNTJOIN_RESULT
        {
            JR_INVALIDPASSWORD = 0,
            JR_FINE = 1,
            JR_INVALIDACCOUNT = 2,
            JR_ACCOUNTINUSE = 3,
            JR_SERVERISFULL = 4,
            JR_ACCOUNTBLOCKED = 5,
            JR_INVALIDVERSION = 6,
            JR_CONNECTIONERROR = 7,
            JR_TOMANYATTEMPTS = 8,
            JR_NOCHARGEINFO = 9,
            JR_AGERESTRICTS = 17,
        }
        public enum USER_DEFINE
        {
            USER_ID = 1,
            USER_NICK = 2,
            USER_EMAIL = 3,
        }
        public enum PROTO_ENUM
        {
            PROTO_LOGIN = 0,
            PROTO_CONTACTLIST = 1,
            PROTO_MESSAGE = 2,
            PROTO_PROFIL,
            PROTO_OTHER
        }
        public enum R_SUBPROTO_CONTACTLIST
        {
            CONTACTLIST_AddContact = 0,
            CONTACTLIST_ChangeContactState,
            CONTACTLIST_AddContacts,
            CONTACTLIST_ShowContactList
        }
        public enum S_SUBPROTO_CONTACTLIST
        {
            CONTACTLIST_ReuqestContactList = 0,
            CONTACTLIST_RequestChangeState,
            CONTACTLIST_RequestContact,
        }

        public enum R_SUBPROTO_MESSAGE
        {
            MESSAGE_SendMessage = 0x00,
            MESSAGE_TypingUser = 0x01,
            MESSAGE_SendMSGBOX = 0x02,
            MESSAGE_AWAY       = 0x03,
        }

        public enum S_SUBPROTO_MESSAGE
        {
            MESSAGE_SendMessage = 0x00,
            MESSAGE_TypingUser = 0x01,
            MESSAGE_ChangeAway = 0x02,
            MESSAGE_RequestAway = 0x03,
        }
        public enum SUBPROTO_OTHER
        {
            OTHER_RequestNickID = 0x00,
            OTHER_FindUser = 0x01,
        }
    }
}
