#ifndef DATASERVER_DATABASE_H
#define DATASERVER_DATABASE_H

#include <winsock2.h>
#include "mysql\mysql.h"
#include "singleton.h"

#define DATABASE_ERROR -1

class CDatabase {
public:
	void	Connect(const char *sServerIp, const char *sDbName, const char *sDbLogin, const char *sDbPassword);
	bool	MakeSyntax(const char *sFormat, ...);
	void	Cleanup();

	inline my_ulonglong		getDataRowsCount() { return mysql_num_rows(m_pMySQLRes); }
	MYSQL_ROW inline		getMySQLRow() { return m_MySQLRow; }
	MYSQL inline			getMySQLHandle() { return m_hMySQL; }
	MYSQL_RES inline		*getMySQLRes() { return m_pMySQLRes; }
	void inline				setResultsStored() { m_pMySQLRes = mysql_store_result(&m_hMySQL); }
	void inline				setResultsFree() { mysql_free_result(m_pMySQLRes); }
	int  inline				KeepAlive() { return mysql_ping(&m_hMySQL); }

private:
	MYSQL		m_hMySQL;
	MYSQL_ROW	m_MySQLRow;
	MYSQL_RES	*m_pMySQLRes;

	bool		m_bActive;
};

int safecpy(char *sDst, const char *sSrc, int aDstSize);

#define sDatabase Singleton<CDatabase>::getInstance()

#endif //DATASERVER_DATABASE_H