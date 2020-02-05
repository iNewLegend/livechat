#include "stdafx.h"
#include "shared.h"
#include "object.h"
#include "protocol.h"
#include "network.h"
#include "libmsg.h"
#include "database.h"

unsigned long	g_uTickCount;
SYSTEMTIME		g_TaskTime;

bool AuthorizeConnectedAddress(const char *sIpAddr) {
	for(unsigned i = 0; i < g_Config.m_vAllowedAddresses.size(); i++)
	{
		if(strcmp(sIpAddr, g_Config.m_vAllowedAddresses.at(i).c_str()) == 0)
		{
			return true;
			break;
		}
	}

	return false;
}

void ServerWorker() {
	while(CIocpSocket::m_bStatus)
	{
		TimeShedules();
		Sleep(1);
	}
}

void TimeShedules() {
	register unsigned long uSystemTickCount = GetTickCount();

	if((uSystemTickCount - g_uTickCount) >= 1000) // second
	{
		gObjCheckObjectsIdleTime(uSystemTickCount);
		g_uTickCount = uSystemTickCount;
		g_TaskTime.wSecond++;

		if(g_TaskTime.wSecond == 60) // minute
		{
			g_TaskTime.wMinute++;

			if((g_TaskTime.wMinute % 5) == 0)
			{
				sLog->outString(LOG_CONNECTION, "[TASK] Keeping alive MySQL connection.\n");

				while(sDatabase->KeepAlive() != 0)
				{
					sLog->outString(LOG_CONNECTION, "[TASK] MySQL server is gone away. Try reconnecting\n");
				}
			}

			if(g_TaskTime.wMinute == 60)
			{
				g_TaskTime.wMinute = 0;
			}

			g_TaskTime.wSecond = 0;
		}
	}
}

void UpdateInstanceWindowTitle() {
	char sWindowTitle[1024];
	sprintf_s(sWindowTitle, sizeof(sWindowTitle), "[%s] :: DataServer Instance. :: Port[%d].", DATASERVER_CURRENT_VERSION, CIocpSocket::m_uPort);
	SetConsoleTitleA(sWindowTitle);
}