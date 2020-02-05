#ifndef GAMESERVER_CONFIG_H
#define GAMESERVER_CONFIG_H

#define SERVERCONFIG_FILE ".\\server.cfg"
#define CONFIG_SECTION "ChatServer"

typedef struct CONFIGSTRUCT {
	int				m_aListenPort;
	char			m_sDataServIpAddr1[16];
	int				m_aDataServPort1;
	char			m_sDataServIpAddr2[16];
	int				m_aDataServPort2;
	char			m_sInstanceName[21];
	int				m_aMaxUsers;

} *PCONFIGSTRUCT;

extern CONFIGSTRUCT g_Config;

void ConfigReadCommandLine(PCONFIGSTRUCT pConfig, const char *pCmdLine[], int aArgsCount);
void ConfigReadCommonConfig(PCONFIGSTRUCT pConfig);

#endif //GAMESERVER_CONFIG_H