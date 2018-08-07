#pragma once

#include <Windows.h>

class cAntiCheat
{

	
public:
	cAntiCheat();

	bool Attach();
	bool Detach();

	static void KILLFO();
	

private :
	
	DWORD PTR_CRCCHECK;
	DWORD PTR_ENUMCHECK;
	DWORD PTR_KILLFO;

	
	void(__cdecl* HOOK_COMPUTECRC)();
	bool(__cdecl* HOOK_ENUMCHECK)();
	void(__cdecl* HOOK_KILLFO)();

	static void  CRCCheck();
	static bool EnumCheck();




};


