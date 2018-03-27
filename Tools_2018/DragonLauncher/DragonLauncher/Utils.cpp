#include "stdafx.h"



std::string DragonLauncher::Utils::GetStartUpPath()
{
	char module_name[MAX_PATH];
	GetModuleFileName(0, module_name, MAX_PATH);
	std::string path(module_name);
	return  path.erase(path.find_last_of('\\'), std::string::npos);
}

std::wstring DragonLauncher::Utils::ConvertToW2String(const std::string& s)
{
	int len;
	int slength = (int)s.length() + 1;
	len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0);
	wchar_t* buf = new wchar_t[len];
	MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
	std::wstring r(buf);
	delete[] buf;
	return r;
}