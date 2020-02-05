#ifndef GAMESERVER_EXCEPTION_H
#define GAMESERVER_EXCEPTION_H

#define MAX_TEXTLEN 1024

class CServerException {
public:
	void setMessage(const char *sFormat, ...);
	void setFuncSign(const char *sFuncSign);
	void setFilePath(const char *sFilePath);

	inline char*	getMessage() { return m_sMessage; }
	inline char*	getFuncSign() { return m_sFuncSign; }
	inline char*	getFilePath() { return m_sFilePath; }

private:
	char m_sMessage[MAX_TEXTLEN];
	char m_sFuncSign[MAX_TEXTLEN];
	char m_sFilePath[MAX_TEXTLEN];
};

#endif //GAMESERVER_EXCEPTION_H