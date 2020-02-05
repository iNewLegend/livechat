#include "stdafx.h"
#include "shared.h"
#include "config.h"
#include "network.h"
#include "exception.h"
#include "libmsg.h"

int main(int argc, char* argv[]) 
{
	/* Set console color :) */
	system("COLOR 0F");

	/* Hello message */
	printf("     *****************************************************\n");
	printf("     *                                                   *\n");
	printf("     *            ");
	printf("LiveChat - ChatServer Instance  ");
	printf("       *\n");
	printf("     *             ");
	#ifdef _DEBUG
	printf("  Version %s DEBUG   ", CHATSERVER_CURRENT_VERSION);
	#else
	printf("  Version %s RELEASE ", CHATSERVER_CURRENT_VERSION);
	#endif
	printf("              *\n");
	printf("     *                                                   *\n");
	printf("     *****************************************************\n\n");

	/* Singletons initialization */
	Singleton<CLog>::Create();
	Singleton<CLoginCount>::Create();
	Singleton<CDataServerSocket>::Create();

	/* Server initialization */
	try {
		/* Log */
		sLog->Initialize(LOG_CONNECTION | LOG_SYSTEM | LOG_GAME);
		sLog->outString(LOG_SYSTEM, "Initialized log instance.\n");

		/* Config */
		sLog->outString(LOG_SYSTEM, "Loading config.\n");
		ConfigReadCommandLine(&g_Config, (const char**)argv, argc);
		ConfigReadCommonConfig(&g_Config);
		sLog->outString(LOG_SYSTEM, "Loaded config.\n");

		/* DataServer connection */
		sLog->outString(LOG_SYSTEM, "Initializing DataServer connection.\n");
		sDataServerSock->Startup(SOCK_TCP, PROTO_TCP);

		do
		{
			if(!sDataServerSock->Connect(g_Config.m_sDataServIpAddr1, g_Config.m_aDataServPort1))
			{
				if(!sDataServerSock->Connect(g_Config.m_sDataServIpAddr2, g_Config.m_aDataServPort2))
				{
					sLog->outString(LOG_SYSTEM, "Couldn't connect to any DataServer. Trying again.\n");
				}
			}
		}
		while(!sDataServerSock->getStatus());
		sLog->outString(LOG_SYSTEM, "Initialized DataServer connection.\n");

		/* IOCP Connection */
		sLog->outString(LOG_SYSTEM, "Initializing IOCP system connection.\n");
		CIocpSocket::WSAInitialize();
		CIocpSocket::Listen(g_Config.m_aListenPort);
		CIocpSocket::IocpInitialize();
		sLog->outString(LOG_SYSTEM, "Initialized IOCP system connection.\n");

		sLog->outString(LOG_SYSTEM, "Instance [%s] active on port [%s].\n\n", g_Config.m_sInstanceName, argv[1]);
		
		UpdateInstanceWindowTitle();

		/* Server worker */
		ServerWorker();

		/* When server is shutting down */
		sLog->outString(LOG_SYSTEM, "ChatServer is going down...\n\n");
		sLog->Cleanup();
		sUdpSocket->Cleanup();
	}

	catch(CWSAException &e) {
		printf("[WSAException]: %s WinSocket error #%i.\n", e.getMessage(), e.getWSAError());

		#ifdef _DEBUG
		printf("[WSAException]: function %s in file %s\n", e.getFuncSign(), e.getFilePath());
		#endif
	}

	catch(CNetworkException &e) {
		printf("[WSAException]: %s WinSocket error #%i.\n", e.getMessage(), e.getWSAError());

		#ifdef _DEBUG
		printf("[WSAException]: function %s in file %s\n", e.getFuncSign(), e.getFilePath());
		#endif
	}

	catch(CLogException &e) {
		printf("[LogException]: %s\n", e.getMessage());

		#ifdef _DEBUG
		printf("[LogException]: function %s in file %s\n", e.getFuncSign(), e.getFilePath());
		#endif
	}

	catch(CServerException &e) {
		printf("[ServerException]: %s\n", e.getMessage());

		#ifdef _DEBUG
		printf("[ServerException]: function %s in file %s\n", e.getFuncSign(), e.getFilePath());
		#endif
	}

	/* Singletons cleanup */
	Singleton<CLog>::Release();
	Singleton<CLoginCount>::Release();
	Singleton<CDataServerSocket>::Release();
	Singleton<CUdpSocket>::Release();

	system("PAUSE");
	return EXIT_SUCCESS;
}

