#include "stdafx.h"
#include "exception.h"

#include <stdarg.h>
#include <windows.h>

void CServerException::setMessage(const char *sFormat, ...) {
	memset(m_sMessage, 0, MAX_TEXTLEN);

	va_list vArgsList;
	va_start(vArgsList, sFormat);
	vsprintf_s(m_sMessage, sizeof(m_sMessage), sFormat, vArgsList);
	va_end(vArgsList);
}

void CServerException::setFuncSign(const char *sFuncSign) {
	memset(m_sFuncSign, 0, MAX_TEXTLEN);

	memcpy(m_sFuncSign, sFuncSign, strlen(sFuncSign));
}

void CServerException::setFilePath(const char *sFilePath) {
	memset(m_sFilePath, 0, MAX_TEXTLEN);

	memcpy(m_sFilePath, sFilePath, strlen(sFilePath));
}
