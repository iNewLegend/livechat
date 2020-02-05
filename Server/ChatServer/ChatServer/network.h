#ifndef GAMESERVER_NETWORK_H
#define GAMESERVER_NETWORK_H

#include "libconn.h"
#include "singleton.h"
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

class CDataServerSocket: public CWinSocket {
public:
	int		DataSend(unsigned char *pMsg, int aLen);

private:
	void	OnConnect(const char* sIpAddr, int aPort);
	void	OnReceive(SOCKET uSocket, sockaddr_in InetSenderAddr, char *pRecv, int aLen);
	void	OnClose(SOCKET uSocket);
};

class CUdpSocket: public CWinSocket {
public:
	int		DataSend(sockaddr_in *pInetAddr, unsigned char *pMsg, int aLen);

private:
	void	OnReceive(SOCKET uSocket, sockaddr_in InetSenderAddr, char *pRecv, int aLen);
};

int PacketGetLen(unsigned char *pMsg);
int PacketGetHeaderLen(unsigned char uHeaderCode);

#define sDataServerSock Singleton<CDataServerSocket>::getInstance()
#define sUdpSocket Singleton<CUdpSocket>::getInstance()

#endif //GAMESERVER_NETWORK_H