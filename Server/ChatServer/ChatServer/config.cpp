#include "stdafx.h"
#include "config.h"
#include "exception.h"
#include "libmsg.h"

#include <stdlib.h>
#include <string.h>
#include <windows.h>

CONFIGSTRUCT g_Config;

void ConfigReadCommandLine(PCONFIGSTRUCT pConfig, const char *pCmdLine[], int aArgsCount) {
	if(aArgsCount == 6)
	{
		pConfig->m_aListenPort = atoi(pCmdLine[1]);

		strcpy_s(pConfig->m_sDataServIpAddr1, sizeof(pConfig->m_sDataServIpAddr1), pCmdLine[2]);
		pConfig->m_aDataServPort1 = atoi(pCmdLine[3]);

		strcpy_s(pConfig->m_sDataServIpAddr2, sizeof(pConfig->m_sDataServIpAddr2), pCmdLine[4]);
		pConfig->m_aDataServPort2 = atoi(pCmdLine[5]);
	}

	else
	{
		CServerException e;
		e.setFilePath(__FILE__);
		e.setFuncSign(__FUNCSIG__);
		e.setMessage("Invalid command line.");
		throw e;
	}
}

void ConfigReadCommonConfig(PCONFIGSTRUCT pConfig) {
	GetPrivateProfileStringA(CONFIG_SECTION, "InstanceName", "eMU - Default Server", pConfig->m_sInstanceName, 21, SERVERCONFIG_FILE);
	pConfig->m_aMaxUsers = GetPrivateProfileIntA(CONFIG_SECTION, "MaxUsers", 1 ,SERVERCONFIG_FILE);
	if(pConfig->m_aMaxUsers < 1)
	{
		pConfig->m_aMaxUsers = 1;
	}
}
