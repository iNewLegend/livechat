#include "stdafx.h"
#include "libconn.h"
#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>

void CWSAException::setMessage(const char *sFormat, ...) {
	memset(m_sMessage, 0, MAX_TEXTLEN);

	va_list vArgsList;
	va_start(vArgsList, sFormat);
	vsprintf_s(m_sMessage, sizeof(m_sMessage), sFormat, vArgsList);
	va_end(vArgsList);
}

void CWSAException::setFuncSign(const char *sFuncSign) {
	memset(m_sFuncSign, 0, MAX_TEXTLEN);

	memcpy(m_sFuncSign, sFuncSign, strlen(sFuncSign));
}

void CWSAException::setFilePath(const char *sFilePath) {
	memset(m_sFilePath, 0, MAX_TEXTLEN);

	memcpy(m_sFilePath, sFilePath, strlen(sFilePath));
}

// ----------------------------------------------------------------

CWinSocket::CWinSocket() {
	Cleanup();
	m_selectDelay.tv_sec = 0;
	m_selectDelay.tv_usec = 1;
}

void CWinSocket::Startup(int aSocketType, int aProtoType) {
	if(WSAStartup(2, &m_WSAData) == 0)
	{
		if(aSocketType == SOCK_TCP && aProtoType == PROTO_TCP || aSocketType == SOCK_UDP && aProtoType == PROTO_UDP)
		{
			m_uSocket = socket(AF_INET, aSocketType, aProtoType);

			if(m_uSocket != INVALID_SOCKET)
			{
				m_aSocketType = aSocketType;
				m_aProtoType = aProtoType;
			}

			else
			{
				CWSAException e;
				e.setFuncSign(__FUNCSIG__);
				e.setFilePath(__FILE__);
				e.setWSAError(WSAGetLastError());
				e.setMessage("Can't create socket. Calling socket() failed.");

				Cleanup();
				throw e;
			}
		}

		else
		{
				CWSAException e;
				e.setFuncSign(__FUNCSIG__);
				e.setFilePath(__FILE__);
				e.setWSAError(WSAGetLastError());
				e.setMessage("Incompatible socket type with protocol type.");

				Cleanup();
				throw e;
		}
	}

	else
	{
		CWSAException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setMessage("Can't startup WSA library.");

		Cleanup();
		throw e;
	}
}

void CWinSocket::Bind(int aPort) {
	m_InetAddr.sin_family = AF_INET;
	m_InetAddr.sin_addr.S_un.S_addr = htonl(ADDR_ANY);
	m_InetAddr.sin_port = htons(aPort);
	memset(m_InetAddr.sin_zero, 0, sizeof(m_InetAddr.sin_zero));

	if(bind(m_uSocket, (sockaddr*)&m_InetAddr, sizeof(sockaddr_in)) != SOCKET_ERROR)
	{
		m_aPort = aPort;

		if(m_aSocketType == SOCK_DGRAM)
		{
			m_bActive = true;
		}
	}

	else
	{
		CWSAException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setMessage("Can't bind socket. Calling bind() failed.");

		Cleanup();
		throw e;
	}
}

void CWinSocket::Listen() {
	if(m_aSocketType == SOCK_TCP)
	{
		if(listen(m_uSocket, SOMAXCONN) != SOCKET_ERROR)
		{
			m_bActive = true;
		}

		else
		{
			CWSAException e;
			e.setFuncSign(__FUNCSIG__);
			e.setFilePath(__FILE__);
			e.setWSAError(WSAGetLastError());
			e.setMessage("Can't listen on socket. Calling listen() failed.");

			Cleanup();
			throw e;
		}
	}

	else
	{
		CWSAException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setMessage("Can't listen on UDP socket.");

		Cleanup();
		throw e;
	}
}

bool CWinSocket::Connect(const char *sIpAddr, int aPort) {
	m_InetAddr.sin_family = AF_INET;
	m_InetAddr.sin_port = htons(aPort);
	m_InetAddr.sin_addr.S_un.S_addr = inet_addr(sIpAddr);

	if(m_InetAddr.sin_addr.S_un.S_addr == INADDR_NONE)
	{
		hostent *pRemoteHost = gethostbyname(sIpAddr);
		if(pRemoteHost != NULL)
		{
			m_InetAddr.sin_addr.S_un.S_addr = *(u_long*)pRemoteHost->h_addr_list[0];
		}
	}	

	if(connect(m_uSocket, (sockaddr*)&m_InetAddr, sizeof(sockaddr)) != SOCKET_ERROR)
	{
		m_bActive = true;
		memcpy(m_sIpAddr, sIpAddr, 16);
		m_aPort = aPort;
		OnConnect(sIpAddr, aPort);
		return true;
	}

	else
	{
		register int aWSAError = WSAGetLastError();

		if(aWSAError == WSAETIMEDOUT || aWSAError == WSAECONNREFUSED || aWSAError == WSAECONNRESET)
		{
			return false;
		}

		else
		{
			CWSAException e;
			e.setFuncSign(__FUNCSIG__);
			e.setFilePath(__FILE__);
			e.setWSAError(aWSAError);
			e.setMessage("Error on calling connect().");

			Cleanup();
			throw e;
		}
	}
}

void CWinSocket::SetBlocking(unsigned long uStatus) {
	if(ioctlsocket(m_uSocket, FIONBIO, &uStatus) == SOCKET_ERROR)
	{
		CWSAException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setMessage("Can't change socket blocking mode. Error on calling ioctlsocket().");

		Cleanup();
		throw e;
	}
}

void CWinSocket::Cleanup() {
	closesocket(m_uSocket);

	m_aSocketType = 0;
	m_aProtoType = 0;
	m_aPort = 0;
	m_bActive = false;
	memset(m_sIpAddr, 0, 16);
}

void CWinSocket::AcceptMonitor() {
	sockaddr_in newInetAddr;
	int aInetAddrSize = sizeof(sockaddr_in);

	fd_set vServerSocket;
	FD_ZERO(&vServerSocket);
	FD_SET(m_uSocket, &vServerSocket);

	if(select(1, &vServerSocket, NULL, NULL, &m_selectDelay) != SOCKET_ERROR)
	{
		if(FD_ISSET(m_uSocket, &vServerSocket))
		{
			SOCKET uNewSocket = accept(m_uSocket, (sockaddr*)&newInetAddr, &aInetAddrSize);

			if(uNewSocket != INVALID_SOCKET)
			{
				OnAccept(uNewSocket, inet_ntoa(newInetAddr.sin_addr));
			}
		}
	}

	else
	{
		CWSAException e;
		e.setFuncSign(__FUNCSIG__);
		e.setFilePath(__FILE__);
		e.setWSAError(WSAGetLastError());
		e.setMessage("Error on accepting new socket.");

		Cleanup();
		throw e;
	}
}

void CWinSocket::DataMonitor(SOCKET uSocket) {
	fd_set vClientSockets;

	FD_ZERO(&vClientSockets);
	FD_SET(uSocket, &vClientSockets);

	if(select(1, &vClientSockets, NULL, NULL, &m_selectDelay) != SOCKET_ERROR)
	{
		if(FD_ISSET(uSocket, &vClientSockets))
		{
			char aBuffer[65355];

			if(m_aSocketType == SOCK_TCP && m_aProtoType == PROTO_TCP)
			{
				int aLen = recv(uSocket, aBuffer, 65355, 0);

				if(aLen > 0)
				{
					OnReceive(uSocket, m_InetAddr, aBuffer, aLen);
				}

				else
				{
					OnClose(uSocket);
				}
			}

			else if(m_aSocketType == SOCK_UDP && m_aProtoType == PROTO_UDP)
			{
				sockaddr_in InetSenderAddr;
				int aSockAddrInLen = sizeof(sockaddr_in);

				int aLen = recvfrom(m_uSocket, aBuffer, 65355, 0, (sockaddr*)&InetSenderAddr, &aSockAddrInLen);

				if(aLen > 0)
				{
					OnReceive(uSocket, InetSenderAddr, aBuffer, aLen);
				}
			}
		}
	}
}

void CWinSocket::OnReceive(SOCKET uSocket, sockaddr_in InetSenderAddr, char *pRecv, int aLen) {
}

void CWinSocket::OnClose(SOCKET uSocket) {
}

void CWinSocket::OnAccept(SOCKET uSocket, const char *sIpAddr) {
}

void CWinSocket::OnConnect(const char* sIpAddr, int aPort) {
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}