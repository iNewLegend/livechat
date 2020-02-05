// DataServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "src\shared.h"
#include "src\libmsg.h"
#include "src\network.h"
#include "src\exception.h"
#include "src\config.h"
#include "src\object.h"
#include "src\database.h"

#include <stdlib.h>

int main( int argc, const char *argv[]) {
	/* Set console color :) */
	system("COLOR 0A");

	/* Hello message */
	printf("     *****************************************************\n");
	printf("     *                                                   *\n");
	printf("     *            ");
	printf("LiveChat - DataServer Instance       ");
	printf("       *\n");
	printf("     *             ");
	#ifdef _DEBUG
	printf("  Version %s DEBUG   ", DATASERVER_CURRENT_VERSION);
	#else
	printf("  Version %s RELEASE ", DATASERVER_CURRENT_VERSION);
	#endif
	printf("              *\n");
	printf("     *                                                   *\n");
	printf("     *****************************************************\n\n");

	/* Singletons initialization */
	Singleton<CLog>::Create();
	Singleton<CDatabase>::Create();

	/* Server initialization */
	try {
		/* Log */
		sLog->Initialize(LOG_CONNECTION | LOG_SYSTEM);
		sLog->outString(LOG_SYSTEM, "Initialized log instance.\n");

		/* Config */
		sLog->outString(LOG_SYSTEM, "Loading config.\n");
		ConfigReadCommandLine(&g_Config, argv, argc);
		ConfigReadAllowedList(&g_Config, ALLOWEDLIST_FILE);
		sLog->outString(LOG_SYSTEM, "Loaded config.\n");

		/* Database */
		sLog->outString(LOG_SYSTEM, "Initializing database connection.\n");
		sDatabase->Connect(g_Config.m_sDbServerAddr, g_Config.m_sDbName, g_Config.m_sDbLogin, g_Config.m_sDbPassword);
		sLog->outString(LOG_SYSTEM, "Connected to %s@%s database.\n", g_Config.m_sDbName, g_Config.m_sDbServerAddr);
		sLog->outString(LOG_SYSTEM, "Initialized database connection.\n");

		/* Socket / Connection */
		sLog->outString(LOG_SYSTEM, "Initializing IOCP system connection.\n");
		CIocpSocket::WSAInitialize();
		CIocpSocket::Listen(g_Config.m_aListenPort);
		CIocpSocket::IocpInitialize();
		sLog->outString(LOG_SYSTEM, "Initialized IOCP system connection.\n");

		sLog->outString(LOG_SYSTEM, "Server is now active on port [%s].\n\n", argv[1]);

		UpdateInstanceWindowTitle();

		/* Server worker */
		ServerWorker();

		/* When server is shutting down */
		sLog->outString(LOG_SYSTEM, "DataServer is going down...\n");
		sLog->Cleanup();
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
	Singleton<CDatabase>::Release();

	system("PAUSE");
	return EXIT_SUCCESS;
}