#ifndef LIBCONN_LIBCONN_H
#define LIBCONN_LIBCONN_H

#include <winsock2.h>

#define MAX_TEXTLEN 1024

#ifdef LIBCONN_EXPORTS
#define LIBCONN_DECLSPEC __declspec(dllexport)
#else
#define LIBCONN_DECLSPEC __declspec(dllimport)
#endif

enum SOCKET_TYPE {
	SOCK_TCP = SOCK_STREAM,
	SOCK_UDP = SOCK_DGRAM,
};

enum PROTOCOL_TYPE {
	PROTO_TCP = IPPROTO_TCP,
	PROTO_UDP = IPPROTO_UDP,
};

enum SOCKET_FIONBIO {
	FIONBIO_BLOCKED = 0,
	FIONBIO_NONBLOCKED
};

class LIBCONN_DECLSPEC CWSAException {
public:
	void setMessage(const char *sFormat, ...);
	void setFuncSign(const char *sFuncSign);
	void setFilePath(const char *sFilePath);
	inline void setWSAError(int aWSAError) { m_aWSAError = aWSAError; }

	inline char* getMessage() { return m_sMessage; }
	inline char* getFuncSign() { return m_sFuncSign; }
	inline char* getFilePath() { return m_sFilePath; }
	inline int getWSAError() { return m_aWSAError; }

private:
	char m_sMessage[MAX_TEXTLEN];
	char m_sFuncSign[MAX_TEXTLEN];
	char m_sFilePath[MAX_TEXTLEN];
	int m_aWSAError;
};

class LIBCONN_DECLSPEC CWinSocket {
public:
	CWinSocket();
	void Startup(int aSocketType, int aProtoType);
	void Bind(int aPort);
	void Listen();
	bool Connect(const char *sIpAddr, int aPort);
	void SetBlocking(unsigned long uStatus);
	void AcceptMonitor();
	void DataMonitor(SOCKET uSocket);
	void Cleanup();

	virtual void OnReceive(SOCKET uSocket, sockaddr_in InetSenderAddr, char *pRecv, int aLen);
	virtual void OnClose(SOCKET uSocket);
	virtual void OnAccept(SOCKET uSocket, const char *sIpAddr);
	virtual void OnConnect(const char* sIpAddr, int aPort);

	inline SOCKET getSocket() { return m_uSocket; }
	inline bool getStatus() { return m_bActive; }
	inline char* getCurrentIpAddr() { return m_sIpAddr; }
	inline int getCurrentPort() { return m_aPort; }
	inline int getSocketType() { return m_aSocketType; }
	inline int getProtoType() { return m_aProtoType; }

private:
	SOCKET m_uSocket;
	WSADATA m_WSAData;
	sockaddr_in m_InetAddr;

	int m_aProtoType;
	int m_aSocketType;
	int m_aPort;
	bool m_bActive;
	char m_sIpAddr[16];
	timeval m_selectDelay;
};

#endif //LIBCONN_LIBCONN_H