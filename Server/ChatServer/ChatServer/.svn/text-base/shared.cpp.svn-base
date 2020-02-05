#include "stdafx.h"
#include "shared.h"
#include "config.h"
#include "network.h"
#include "object.h"

#include <windows.h>
#include <stdlib.h>

unsigned long	g_uTickCount = 0;
SYSTEMTIME		g_TaskTime = {0};

unsigned long Role(unsigned int uStart, unsigned int uEnd) {
	unsigned int uRange = (uEnd - uStart);

	if(uRange == 0)
	{
		uRange = 1;
	}

	srand(GetTickCount());
	return ((rand() % uRange) + uStart);
}

void UpdateInstanceWindowTitle() {
	char sWindowTitle[1024] = {"chuj"};
	sprintf_s(sWindowTitle, sizeof(sWindowTitle), "[%s] :: [%d/%d] %s :: [%s:%d]", CHATSERVER_CURRENT_VERSION, sLoginCount->getCount(), g_Config.m_aMaxUsers, g_Config.m_sInstanceName, sDataServerSock->getCurrentIpAddr(), sDataServerSock->getCurrentPort());
	SetConsoleTitleA(sWindowTitle);
}

void ServerWorker() {
	while(CIocpSocket::m_bStatus && sDataServerSock->getStatus())
	{
		sDataServerSock->DataMonitor(sDataServerSock->getSocket());

		TimeShedules();
	}
}

void TimeShedules() {
	register unsigned long uSystemTickCount = GetTickCount();

	if((uSystemTickCount - g_uTickCount) >= 1000) // second
	{
		ObjectProc(uSystemTickCount);
		g_uTickCount = uSystemTickCount;
		g_TaskTime.wSecond++;

		// ------------------------------------ TASKS SEC START
		if((g_TaskTime.wSecond % 20) == 0)
		{
		}
		// ------------------------------------ TASKS SEC END

		// ------------------------------------
		if(g_TaskTime.wSecond == 60) // minute
		{
			g_TaskTime.wMinute++;

			// ------------------------------------ TASKS MIN START
			if((g_TaskTime.wMinute % 1) == 0)
			{
				// One minute tasks
			}
			// ------------------------------------ TASKS MIN END

			// ------------------------------------
			if(g_TaskTime.wMinute == 60)
			{
				g_TaskTime.wMinute = 0;
			}
			// ------------------------------------

			g_TaskTime.wSecond = 0;
		}
		// ------------------------------------
	}
}