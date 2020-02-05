#include "stdafx.h"
#include "database.h"
#include "exception.h"
#include "libmsg.h"

void CDatabase::Connect(const char *sServerIp, const char *sDbName, const char *sDbLogin, const char *sDbPassword) {
	mysql_init(&m_hMySQL);

	if(mysql_real_connect(&m_hMySQL, sServerIp, sDbLogin, sDbPassword, sDbName, 3306, NULL, CLIENT_MULTI_RESULTS | CLIENT_MULTI_STATEMENTS))
	{
		m_bActive = true;
	}

	else
	{
		CServerException e;
		e.setFilePath(__FILE__);
		e.setFuncSign(__FUNCSIG__);
		e.setMessage("Couldn't connect to MySQL Server: %s.\n", mysql_error(&m_hMySQL));

		Cleanup();
		throw e;
	}
}

bool CDatabase::MakeSyntax(const char *sFormat, ...) {
	char sSyntax[1024];

    va_list vArgs;
    va_start(vArgs, sFormat);
    vsprintf_s(sSyntax, sFormat, vArgs);
    va_end(vArgs);

	int aRetn = 1;

	sLog->outString(LOG_SYSTEM, "[MySQL] %s\n", sSyntax);

    if(aRetn = mysql_real_query(&m_hMySQL, sSyntax, (unsigned long)strlen(sSyntax)) == 0)
    {
		return true;
    }

    else
    {
        sLog->outString(LOG_SYSTEM, "Couldn't execute query \"%s\" - reason: %s.\n", sSyntax, mysql_error(&m_hMySQL));
		return false;
    }
}

void CDatabase::Cleanup() {
	mysql_close(&m_hMySQL);
	m_bActive = false;
}

int safecpy(char *sDst, const char *sSrc, int aDstSize) 
{
		try
		{
			int aCopyCount = 0;

			for(int i = 0; i < aDstSize; i++)
			{
				if(sSrc[i] != 39)
				{
					sDst[aCopyCount] = sSrc[i];
					aCopyCount++;
				}
			}
			sDst[aCopyCount] = 0;
			return aCopyCount;
		}
		catch(char * d)
		{
			sLog->outString(LOG_SYSTEM,"%s .\n",d);
			return 0;
		}
}