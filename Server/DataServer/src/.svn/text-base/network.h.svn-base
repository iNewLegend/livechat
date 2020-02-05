#ifndef DATASERVER_NETWORK_H
#define DATASERVER_NETWORK_H

#include <winsock2.h>
#include <windows.h>

#define MAX_EXCEPTION_TEXTLEN 1024
#define MAX_NETWORK_BUFFER 65355

class CNetworkException {
public:
	void setMessage(const char *sFormat, ...);
	void setFuncSign(const char *sFuncSign);
	void setFilePath(const char *sFilePath);
	inline void setWSAError(int aWSAError) { m_aWSAError = aWSAError; }
	inline void setSysError(int aSysError) { m_aSysError = aSysError; }

	inline char* getMessage() { return m_sMessage; }
	inline char* getFuncSign() { return m_sFuncSign; }
	inline char* getFilePath() { return m_sFilePath; }
	inline int getWSAError() { return m_aWSAError; }
	inline int getSysError() { return m_aSysError; }

private:
	char m_sMessage[MAX_EXCEPTION_TEXTLEN];
	char m_sFuncSign[MAX_EXCEPTION_TEXTLEN];
	char m_sFilePath[MAX_EXCEPTION_TEXTLEN];
	int m_aWSAError;
	int m_aSysError;
};

class CIocpSocket {
public:
	static void WSAInitialize();
	static void Listen(unsigned uPort);

	static void	IocpInitialize();
	static int	IocpDataSend(int aIndex, unsigned char *pMsg, int aLen);
	static void	IocpDataReceive(int aIndex, unsigned char *pRecv, int aLen);
	static void Cleanup();

	static void IocpAcceptWorkerInitialize();
	static void IocpDataWorkerInitialize();
	static void IocpAcceptWorker();
	static void IocpDataWorker();

	static SOCKET			m_uSocket;

	static CRITICAL_SECTION m_IocpCriticalSection;
	static HANDLE			m_hIocpCompletionPort;
	static HANDLE			m_hIocpAcceptWorkerThread;
	static unsigned			m_uIocpDataWorkerThreadsCount;
	static HANDLE			*m_pIocpDataWorkerThreads;

	static unsigned			m_uPort;
	static bool				m_bStatus;
	static sockaddr_in		m_InetAddr;
};

int PacketGetLen(unsigned char *pMsg);

#endif //DATASERVER_NETWORK_H