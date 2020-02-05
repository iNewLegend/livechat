#ifndef GAMESERVER_SHARED_H
#define GAMESERVER_SHRARED_H

#include "singleton.h"
#include <windows.h>

#define CHATSERVER_CURRENT_VERSION "0.7.0"

class CLoginCount {
public:
	CLoginCount() { Reset(); }
	inline int	getCount() { return m_aLoginCount; }
	inline void Increase() { m_aLoginCount++; }
	inline void Decrease() { if(m_aLoginCount > 0) {m_aLoginCount--;} }
	inline void Reset() { m_aLoginCount = 0; }

private:
	int m_aLoginCount;
};

void ServerWorker();
void TimeShedules();

unsigned long Role(unsigned int uStart, unsigned int uEnd);
void UpdateInstanceWindowTitle();

extern unsigned long	g_uTickCount;
extern SYSTEMTIME		g_TaskTime;

#define sLoginCount Singleton<CLoginCount>::getInstance()

#endif // GAMESERVER_SHARED_H