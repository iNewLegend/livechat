#ifndef DATASERVER_CONFIG_H
#define DATASERVER_CONFIG_H

#include <vector>
#include <string>

#define ALLOWEDLIST_FILE ".\\AllowedList.dat"

using namespace std;

typedef struct CONFIGSTRUCT {
	int		m_aListenPort;
	char	m_sDbLogin[20];
	char	m_sDbPassword[20];
	char	m_sDbName[20];
	char	m_sDbServerAddr[30];
	vector<std::string> m_vAllowedAddresses;
} *PCONFIGSTRUCT;

extern CONFIGSTRUCT g_Config;

void ConfigReadCommandLine(PCONFIGSTRUCT pConfig, const char *pCmdLine[], int aArgsCount);
void ConfigReadAllowedList(PCONFIGSTRUCT pConfig, const char *sFileName);

#endif //DATASERVER_CONFIG_H