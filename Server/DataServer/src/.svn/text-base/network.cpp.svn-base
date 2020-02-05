#include "stdafx.h"
#include "network.h"
#include "object.h"
#include "libmsg.h"
#include "protocol.h"

void CNetworkException::setMessage(const char *sFormat, ...) {
	memset(m_sMessage, 0, MAX_EXCEPTION_TEXTLEN);

	va_list vArgsList;
	va_start(vArgsList, sFormat);
	vsprintf_s(m_sMessage, sizeof(m_sMessage), sFormat, vArgsList);
	va_end(vArgsList);
}

void CNetworkException::setFuncSign(const char *sFuncSign) {
	memset(m_sFuncSign, 0, MAX_EXCEPTION_TEXTLEN);

	memcpy(m_sFuncSign, sFuncSign, strlen(sFuncSign));
}

void CNetworkException::setFilePath(const char *sFilePath) {
	memset(m_sFilePath, 0, MAX_EXCEPTION_TEXTLEN);

	memcpy(m_sFilePath, sFilePath, strlen(sFilePath));
}

// ---------------------------------------------------------------------

SOCKET				CIocpSocket::m_uSocket = INVALID_SOCKET;
CRITICAL_SECTION	CIocpSocket::m_IocpCriticalSection;
HANDLE				CIocpSocket::m_hIocpCompletionPort = NULL;
unsigned			CIocpSocket::m_uIocpDataWorkerThreadsCount = NULL;
HANDLE				*CIocpSocket::m_pIocpDataWorkerThreads = NULL;
HANDLE				CIocpSocket::m_hIocpAcceptWorkerThread = NULL;
unsigned			CIocpSocket::m_uPort = 0;
bool				CIocpSocket::m_bStatus = false;
sockaddr_in			CIocpSocket::m_InetAddr = {0};

void CIocpSocket::WSAInitialize() {
	WSADATA wsaData = {0};

	if(WSAStartup(2, &wsaData) == 0)
	{
		CIocpSocket::m_uSocket = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, NULL, NULL, WSA_FLAG_OVERLAPPED);
		
		if(CIocpSocket::m_uSocket == INVALID_SOCKET)
		{
			CNetworkException e;
			e.setFuncSign(__FUNCSIG__);
			e.setFilePath(__FILE__);
			e.setWSAError(WSAGetLastError());
			e.setSysError(GetLastError());
			e.setMessage("[IOCP] :: Can't create socket. Calling socket() failed.");

			Cleanup();
			throw e;
		}
	}

	else
	{
		CNetworkException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setSysError(GetLastError());
		e.setMessage("[IOCP] :: Can't startup WSA library.");

		Cleanup();
		throw e;
	}
}

void CIocpSocket::Listen(unsigned uPort) {
	const char chOpt = 1;
	setsockopt(CIocpSocket::m_uSocket, IPPROTO_TCP, TCP_NODELAY, &chOpt, sizeof(char));

	CIocpSocket::m_InetAddr.sin_family = AF_INET;
	CIocpSocket::m_InetAddr.sin_addr.S_un.S_addr = htonl(ADDR_ANY);
	CIocpSocket::m_InetAddr.sin_port = htons(uPort);

	if(bind(CIocpSocket::m_uSocket, (sockaddr*)&CIocpSocket::m_InetAddr, sizeof(sockaddr_in)) != SOCKET_ERROR)
	{
		if(listen(CIocpSocket::m_uSocket, SOMAXCONN) != SOCKET_ERROR)
		{
			CIocpSocket::m_bStatus = true;
			CIocpSocket::m_uPort = uPort;
		}

		else
		{
			CNetworkException e;
			e.setFuncSign(__FUNCSIG__);
			e.setFilePath(__FILE__);
			e.setWSAError(WSAGetLastError());
			e.setSysError(GetLastError());
			e.setMessage("[IOCP] :: Can't listen on socket. Calling listen() failed.");

			Cleanup();
			throw e;
		}
	}

	else
	{
		CNetworkException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setSysError(GetLastError());
		e.setMessage("[IOCP] :: Can't bind socket. Calling bind() failed.");

		Cleanup();
		throw e;
	}
}

void CIocpSocket::IocpInitialize() {
	InitializeCriticalSection(&CIocpSocket::m_IocpCriticalSection);
	CIocpSocket::m_hIocpCompletionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, NULL, NULL);

	if(CIocpSocket::m_hIocpCompletionPort != NULL)
	{
		CIocpSocket::IocpDataWorkerInitialize();
		CIocpSocket::IocpAcceptWorkerInitialize();
	}

	else
	{
		CNetworkException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setSysError(GetLastError());
		e.setMessage("[IOCP] :: Create completion port failed.");
	}
}

void CIocpSocket::IocpDataWorkerInitialize() {
	SYSTEM_INFO sysInfo = {0};
	GetSystemInfo(&sysInfo);

	CIocpSocket::m_uIocpDataWorkerThreadsCount = sysInfo.dwNumberOfProcessors << 1;
	CIocpSocket::m_pIocpDataWorkerThreads = new HANDLE[CIocpSocket::m_uIocpDataWorkerThreadsCount];

	for(unsigned i = 0; i < CIocpSocket::m_uIocpDataWorkerThreadsCount; i++)
	{
		CIocpSocket::m_pIocpDataWorkerThreads[i] = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)CIocpSocket::IocpDataWorker, NULL, NULL, NULL);

		if(CIocpSocket::m_pIocpDataWorkerThreads[i] == NULL)
		{
			CNetworkException e;
			e.setFuncSign(__FUNCSIG__);
			e.setFilePath(__FILE__);
			e.setWSAError(WSAGetLastError());
			e.setSysError(GetLastError());
			e.setMessage("[IOCP] :: Create data worker thread (%d/%d) failed.", i + 1, CIocpSocket::m_uIocpDataWorkerThreadsCount);
		}
	}
}

void CIocpSocket::IocpAcceptWorkerInitialize() {
	CIocpSocket::m_hIocpAcceptWorkerThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)CIocpSocket::IocpAcceptWorker, NULL, NULL, NULL);

	if(CIocpSocket::m_hIocpAcceptWorkerThread == NULL)
	{
		CNetworkException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setSysError(GetLastError());
		e.setMessage("[IOCP] :: Can't create Iocp accept worker thread.");
	}
}

//---------------------------------------------------------------------------------------------------------------------------

void CIocpSocket::IocpAcceptWorker() {
	while(CIocpSocket::m_bStatus)
	{
		sockaddr_in IncomingInetAddr = {0};
		unsigned uSockAddrLen = sizeof(sockaddr);
		SOCKET uIncomingSocket = WSAAccept(CIocpSocket::m_uSocket, (sockaddr*)&IncomingInetAddr, (LPINT)&uSockAddrLen, NULL, NULL);

		if(uIncomingSocket != INVALID_SOCKET)
		{
			EnterCriticalSection(&CIocpSocket::m_IocpCriticalSection);

			int aIndex = gObjSearchFree();
			char sIncomingIpAddr[16] = {0};
			strcpy_s(sIncomingIpAddr, 16, inet_ntoa(IncomingInetAddr.sin_addr));

			if(aIndex != INVALID_OBJECT)
			{
				if(CreateIoCompletionPort((HANDLE)uIncomingSocket, CIocpSocket::m_hIocpCompletionPort, aIndex, NULL) != NULL)
				{
					gObjAdd(aIndex, uIncomingSocket, sIncomingIpAddr);

					sLog->outString(LOG_CONNECTION, "[IOCP] :: [%d][%s] :: User connected.\n", aIndex, sIncomingIpAddr);

					unsigned uIoRecvLen = 0, uFlags = 0;
			
					if(WSARecv(uIncomingSocket, &g_Obj[aIndex].m_wsaBuff, 1, (LPDWORD)&uIoRecvLen, (LPDWORD)&uFlags, &g_Obj[aIndex].m_wsaOverlapped, NULL) == SOCKET_ERROR)
					{
						unsigned uWSALastErr = WSAGetLastError();

						if(uWSALastErr != WSA_IO_PENDING)
						{
							sLog->outString(LOG_CONNECTION, "[IOCP] :: [%d][%s] - WSARecv() filed with error: #%d.\n", aIndex, sIncomingIpAddr, uWSALastErr);
							gObjDel(aIndex);
						}
					}
				}

				else
				{
					sLog->outString(LOG_CONNECTION, "[IOCP] :: [%d][%s] Can't associate completion port with socket - Error: #%d.\n", aIndex, sIncomingIpAddr, GetLastError());
					closesocket(uIncomingSocket);
					Cleanup();
				}
			}

			else
			{
				sLog->outString(LOG_CONNECTION, "[IOCP] :: [%s] Couldn't estabilished connection.\n", sIncomingIpAddr);
				closesocket(uIncomingSocket);
			}

			LeaveCriticalSection(&CIocpSocket::m_IocpCriticalSection);
		}

		else
		{
			sLog->outString(LOG_CONNECTION, "[IOCP] :: WSAAccept() failed - Error: #%d.\n", WSAGetLastError());
		}
	}
}

void CIocpSocket::IocpDataWorker() {
	while(CIocpSocket::m_bStatus)
	{
		int	aIoRecvLen = 0;
		int	aIndex;
		LPWSAOVERLAPPED	lpWSAOverlapped;

		BOOL bCompletionQueueStatus = GetQueuedCompletionStatus(CIocpSocket::m_hIocpCompletionPort, (LPDWORD)&aIoRecvLen, (PULONG_PTR)&aIndex, &lpWSAOverlapped, INFINITE);

		if(!bCompletionQueueStatus)
		{
			unsigned uLastErr = GetLastError();

			if(uLastErr != ERROR_NETNAME_DELETED && uLastErr != ERROR_CONNECTION_ABORTED && uLastErr != ERROR_OPERATION_ABORTED)
			{
				sLog->outString(LOG_CONNECTION, "[IOCP] :: GetQueuedCompletionStatus() abnormal error - #%d.\n", uLastErr);
				Cleanup();
			}
		}

		EnterCriticalSection(&CIocpSocket::m_IocpCriticalSection);

		if(aIoRecvLen > 0)
		{
			int aRecvLen = 0, uFlags = 0;

			CIocpSocket::IocpDataReceive(aIndex, (unsigned char*)g_Obj[aIndex].m_wsaBuff.buf, aIoRecvLen);
					
			if(WSARecv(g_Obj[aIndex].m_uSocket, &g_Obj[aIndex].m_wsaBuff, 1, (LPDWORD)&aRecvLen, (LPDWORD)&uFlags, &g_Obj[aIndex].m_wsaOverlapped, NULL) == SOCKET_ERROR)
			{
				unsigned uWSALastErr = WSAGetLastError();

				if(uWSALastErr != WSA_IO_PENDING)
				{
					sLog->outString(LOG_CONNECTION, "[IOCP] :: [%d][%s] :: WSARecv() failed - Error: #%d.\n", aIndex, g_Obj[aIndex].m_sIpAddr, uWSALastErr);
					gObjDel(aIndex);
				}
			}
		}

		else if(aIoRecvLen <= 0)
		{
			sLog->outString(LOG_CONNECTION, "[IOCP] :: [%d][%s] :: User disconnected.\n", aIndex, g_Obj[aIndex].m_sIpAddr);
			gObjDel(aIndex);
		}

		LeaveCriticalSection(&CIocpSocket::m_IocpCriticalSection);
	}
}

//---------------------------------------------------------------------------------------------------------------------------

int CIocpSocket::IocpDataSend(int aIndex, unsigned char *pMsg, int aLen) {
	EnterCriticalSection(&CIocpSocket::m_IocpCriticalSection);
	int aSendRet = send(g_Obj[aIndex].m_uSocket, (const char*)pMsg, aLen, 0);
	LeaveCriticalSection(&CIocpSocket::m_IocpCriticalSection);

	return aSendRet;
}

void CIocpSocket::IocpDataReceive(int aIndex, unsigned char *pRecv, int aLen) {
	int aTotalLen = 0;

	do
	{
		unsigned short uPacketLen = PacketGetLen((unsigned char*)&pRecv[aTotalLen]);
		unsigned char *pTmpRecv = new unsigned char[uPacketLen];
		memcpy(pTmpRecv, &pRecv[aTotalLen], uPacketLen);

		ProtocolCore(aIndex, pTmpRecv, uPacketLen);

		delete [] pTmpRecv;
		aTotalLen += uPacketLen;
	}
	while(aTotalLen < aLen);
}

void CIocpSocket::Cleanup() {
	CIocpSocket::m_bStatus = false;
	closesocket(CIocpSocket::m_uSocket);
	CIocpSocket::m_uPort = 0;
	memset(&CIocpSocket::m_InetAddr, 0, sizeof(sockaddr_in));

	DeleteCriticalSection(&CIocpSocket::m_IocpCriticalSection);
	CloseHandle(CIocpSocket::m_hIocpCompletionPort);
	
	for(unsigned i = 0; i < CIocpSocket::m_uIocpDataWorkerThreadsCount; i++)
	{
		TerminateThread(CIocpSocket::m_pIocpDataWorkerThreads[i], 0);
		CloseHandle(CIocpSocket::m_pIocpDataWorkerThreads[i]);
	}

	CIocpSocket::m_uIocpDataWorkerThreadsCount = 0;
	delete [] CIocpSocket::m_pIocpDataWorkerThreads;
	CIocpSocket::m_pIocpDataWorkerThreads = NULL;

	TerminateThread(CIocpSocket::m_hIocpAcceptWorkerThread, 0);
	CloseHandle(CIocpSocket::m_hIocpAcceptWorkerThread);
}

//---------------------------------------------------------------------------------------------------------------------------

int PacketGetLen(unsigned char *pMsg) {
	if(pMsg[0] == 0xC1 || pMsg[0] == 0xC3)
	{
		return pMsg[1];
	}

	else if(pMsg[0] == 0xC2 || pMsg[0] == 0xC4)
	{
		return MAKEWORD(pMsg[2], pMsg[1]);
	}

	else
	{
		return 0;
	}
}