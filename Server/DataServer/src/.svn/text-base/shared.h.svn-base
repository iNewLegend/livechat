#ifndef CONNECTSERVER_SHARED_H
#define CONNECTSERVER_SHARED_H

#include "config.h"
#include <winsock2.h>
#include <windows.h>

#define DATASERVER_CURRENT_VERSION "0.7.0"

bool AuthorizeConnectedAddress(const char *sIpAddr);
void UpdateInstanceWindowTitle();

void ServerWorker();
void TimeShedules();

extern unsigned long	g_uTickCount;
extern SYSTEMTIME		g_TaskTime;

#endif //CONNECTSERVER_SHARED_H