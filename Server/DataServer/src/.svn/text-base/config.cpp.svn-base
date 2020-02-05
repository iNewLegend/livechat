#include "stdafx.h"
#include "config.h"
#include "exception.h"
#include "libmsg.h"

CONFIGSTRUCT g_Config;

void ConfigReadCommandLine(PCONFIGSTRUCT pConfig, const char *pCmdLine[], int aArgsCount) {
	if(aArgsCount == 6)
	{
		g_Config.m_aListenPort = atoi(pCmdLine[1]);
		strcpy_s(g_Config.m_sDbServerAddr, sizeof(g_Config.m_sDbServerAddr), pCmdLine[2]);
		strcpy_s(g_Config.m_sDbName, sizeof(g_Config.m_sDbName), pCmdLine[3]);
		strcpy_s(g_Config.m_sDbLogin, sizeof(g_Config.m_sDbLogin), pCmdLine[4]);
		strcpy_s(g_Config.m_sDbPassword, sizeof(g_Config.m_sDbPassword), pCmdLine[5]);
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

void ConfigReadAllowedList(PCONFIGSTRUCT pConfig, const char *sFileName) {
	FILE *hListFile;
	fopen_s(&hListFile, sFileName, "r");
	char sLine[1024];

	g_Config.m_vAllowedAddresses.clear();

	if(hListFile != NULL)
	{
		while(!feof(hListFile))
		{
			fgets(sLine, 1024, hListFile);

			if(strcmp(sLine, "end") != 0)
			{
				register size_t iLineLen = strlen(sLine);
				if(sLine[iLineLen - 1] == '\n' || sLine[iLineLen - 1] == '\r')
				{
					if(sLine[0] != '/' && sLine[1] != '/' && sLine[0] != '\n')
					{
						//sscanf(sLine, "%1023s", &sLine);
						sscanf_s(sLine, "%s", &sLine, sizeof(sLine));
						g_Config.m_vAllowedAddresses.push_back(sLine);
					}
				}
			}

			else
			{
				break;
			}
		}

		if(g_Config.m_vAllowedAddresses.empty())
		{
			sLog->outString(LOG_SYSTEM, "[WARNING] Server list is empty!\n");
			system("PAUSE");
		}

		fclose(hListFile);
	}

	else
	{
		CServerException e;
		e.setFilePath(__FILE__);
		e.setFuncSign(__FUNCSIG__);
		e.setMessage("Can't open %s list file.", sFileName);
		throw e;
	}
}