// libMsg.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "libMsg.h"


#ifdef _MANAGED
#pragma managed(push, off)
#endif

void CLogException::setMessage(const char *sFormat, ...) {
	memset(m_sMessage, 0, MAX_TEXTLEN);

	va_list vArgsList;
	va_start(vArgsList, sFormat);
	vsprintf_s(m_sMessage, sizeof(m_sMessage), sFormat, vArgsList);
	va_end(vArgsList);
}

void CLogException::setFuncSign(const char *sFuncSign) {
	memset(m_sFuncSign, 0, MAX_TEXTLEN);

	memcpy(m_sFuncSign, sFuncSign, strlen(sFuncSign));
}

void CLogException::setFilePath(const char *sFilePath) {
	memset(m_sFilePath, 0, MAX_TEXTLEN);

	memcpy(m_sFilePath, sFilePath, strlen(sFilePath));
}

CLog::CLog() {
	m_hConnectionLog = NULL;
	m_hGameLog = NULL;
	m_hSystemLog = NULL;
}

CLog::~CLog() {
}

void CLog::Initialize(int aLogs) {
	InitializeCriticalSection(&m_LogCriticalSection);

	if((aLogs & LOG_CONNECTION) == LOG_CONNECTION)
	{
		if(m_hConnectionLog == NULL)
		{
			fopen_s(&m_hConnectionLog, "LOGS\\connection.log", "a");

			if(m_hConnectionLog == NULL)
			{
				CLogException e;
				e.setFuncSign(__FUNCSIG__);
				e.setFilePath(__FILE__);
				e.setMessage("Couldn't open connection.log file.");

				Cleanup();
				throw e;
			}
		}
	}

	if((aLogs & LOG_GAME) == LOG_GAME)
	{
		if(m_hGameLog == NULL)
		{
			fopen_s(&m_hGameLog, "LOGS\\game.log", "a");

			if(m_hGameLog == NULL)
			{
				CLogException e;
				e.setFuncSign(__FUNCSIG__);
				e.setFilePath(__FILE__);
				e.setMessage("Couldn't open game.log file.");

				Cleanup();
				throw e;
			}
		}
	}

	if((aLogs & LOG_SYSTEM) == LOG_SYSTEM)
	{
		if(m_hSystemLog == NULL)
		{
			fopen_s(&m_hSystemLog, "LOGS\\system.log", "a");

			if(m_hSystemLog == NULL)
			{
				CLogException e;
				e.setFuncSign(__FUNCSIG__);
				e.setFilePath(__FILE__);
				e.setMessage("Couldn't open system.log file.");

				Cleanup();
				throw e;
			}
		}
	}
}

void CLog::Cleanup() {
	DeleteCriticalSection(&m_LogCriticalSection);

	if(m_hSystemLog != NULL)
	{
		fclose(m_hSystemLog);
	}

	if(m_hGameLog != NULL)
	{
		fclose(m_hGameLog);
	}

	if(m_hConnectionLog != NULL)
	{
		fclose(m_hConnectionLog);
	}
}

FILE* CLog::getFile(LOG_TYPE iType) {
	switch(iType) {
		case LOG_GAME:
			return m_hGameLog;
			break;
			
		case LOG_SYSTEM:
			return m_hSystemLog;
			break;

		case LOG_CONNECTION:
				return m_hConnectionLog;
				break;

		default:
			return m_hSystemLog;
			break;
	}
}

void CLog::outTime(LOG_TYPE iType) {
	SYSTEMTIME t;
	GetLocalTime(&t);

	printf("(%04d-%02d-%02d %02d:%02d:%02d): ", t.wYear, t.wMonth, t.wDay, t.wHour, t.wMinute, t.wSecond);
	fprintf(getFile(iType), "(%04d-%02d-%02d %02d:%02d:%02d): ", t.wYear, t.wMonth, t.wDay, t.wHour, t.wMinute, t.wSecond);
}

void CLog::outString(LOG_TYPE iType, const char *format, ...) {
	EnterCriticalSection(&m_LogCriticalSection);

	va_list argv;
	va_start(argv, format);

	outTime(iType);
	vprintf(format, argv);
	vfprintf(getFile(iType), format, argv);

	va_end(argv);

	LeaveCriticalSection(&m_LogCriticalSection);
}

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

