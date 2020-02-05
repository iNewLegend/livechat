#ifndef LIBMSG_H
#define LIBMSG_H

#include <stdio.h>
#include <string>
#include <windows.h>

#ifdef LIBMSG_EXPORTS
	#define LIBMSG_DECLSPEC _declspec(dllexport)
#else
	#define LIBMSG_DECLSPEC _declspec(dllimport)
#endif

#define MAX_TEXTLEN 1024

enum LOG_TYPE {
	LOG_GAME = 2,
	LOG_SYSTEM = 4,
	LOG_CONNECTION = 16,
};

class CLogException {
public:
	void setMessage(const char *sFormat, ...);
	void setFuncSign(const char *sFuncSign);
	void setFilePath(const char *sFilePath);

	inline char* getMessage() { return m_sMessage; }
	inline char* getFuncSign() { return m_sFuncSign; }
	inline char* getFilePath() { return m_sFilePath; }

private:
	char m_sMessage[MAX_TEXTLEN];
	char m_sFuncSign[MAX_TEXTLEN];
	char m_sFilePath[MAX_TEXTLEN];
};

class LIBMSG_DECLSPEC CLog {
public:
	CLog();
	~CLog();

	void Initialize(int aLogs);
	void Cleanup();

	void outString(LOG_TYPE iType, const char *format, ...);

private:
	void outTime(LOG_TYPE iType);
	FILE* getFile(LOG_TYPE iType);

	FILE *m_hGameLog;
	FILE *m_hConnectionLog;
	FILE *m_hSystemLog;

	CRITICAL_SECTION m_LogCriticalSection;
	bool m_bWritable;
};

#endif // ~LIBMSG_H