#include "stdafx.h"
#include "cAntiCheat.h"

#include "FiestaHook.h"
#include "Address.h"

#include <Windows.h>
#include <stdio.h>
#include <detours.h>
#pragma comment(lib, "detours.lib")

#include <iostream>

using namespace DragonHook;




cAntiCheat::cAntiCheat()
{
}



bool cAntiCheat::Attach()
{



	if ((PTR_CRCCHECK = FiestaHook::FindPattern(PATTERN_ANTICHEAT_CRCCHECK, MASK_ANTICHEAT_CRCCHECK)) == 0)
	{
		Utils::Message("Can not find ANTICHEAT_CRCCHECK Adresss please update Pattern..");
		return false;
	}


	if ((PTR_ENUMCHECK = FiestaHook::FindPattern(PATTERN_ANTICHEAT_ENUMCHECK, MASK_ANTICHEAT_ENUMCHECK)) == 0)
	{
		Utils::Message("Can not find PATTERN_ANTICHEAT_ENUMCHECK Adresss please update Pattern..");
		return false;
	}

	if ((PTR_KILLFO = FiestaHook::FindPattern(PATTERN_ANTICHEAT_KILLFO, MASK_ANTICHEAT_KILLFO)) == 0)
	{
		Utils::Message("Can not find PATTERN_ANTICHEAT_ENUMCHECK Adresss please update Pattern..");
		return false;
	}

	HOOK_COMPUTECRC = (void(__cdecl*)())(DWORD*)PTR_CRCCHECK;
	HOOK_ENUMCHECK = (bool(__cdecl*)())(DWORD*)PTR_ENUMCHECK;
	HOOK_KILLFO = (void(__cdecl*)())(DWORD*)PTR_KILLFO;

	DetourRestoreAfterWith();
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	

	DetourAttach(&(LPVOID&)HOOK_KILLFO, KILLFO);
	DetourAttach(&(LPVOID&)HOOK_ENUMCHECK, EnumCheck);
	DetourAttach(&(LPVOID&)HOOK_COMPUTECRC, CRCCheck);

	if (DetourTransactionCommit() != 0)
	{
		Utils::Message("Failed To Commit Detours please Check you Adresses..");
		return false;
	}


	return true;
}

bool cAntiCheat::Detach()
{
	return false;
}


void cAntiCheat::CRCCheck()
{
	while (true)
	{


		//TODO CUSTOME CRC CHECK :D

		std::cout << "CRC32 Check wozu Braucht man es noch?" << std::endl;
		Sleep(20000);
	}

}

bool cAntiCheat::EnumCheck()
{
	while (true)
	{
		std::cout << "EnumCheck wozu Braucht man es noch?" << std::endl;
		Sleep(5000);
	}

	return 1;
}

void cAntiCheat::KILLFO()
{
	std::cout << "ZU blöd das ich hier bin und nichts mache schliese dich selber !" << std::endl;

	FiestaHook::AntiCheats->HOOK_KILLFO();
}


