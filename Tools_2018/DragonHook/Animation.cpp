#include "stdafx.h"
#include "Animation.h"
#include "FiestaHook.h"
#include "Address.h"
#include "Utils.h"
#include "Detours.h"
#include <iostream>




void Animation::CheckAniFileMD5(std::string Name, std::string SubDir, std::string FileName)
{
	std::cout << Name << SubDir << FileName << std::endl;
}

void Animation::Test(const char* md5[500])
{

	std::cout << md5 <<  std::endl;
}


bool Animation::Attach()
{
	if ((PTR_ANIFILEMD5 = DragonHook::FiestaHook::FindPattern(PATTERN_ANIMASTION_CHECLANIFILE, MASK_ANIMASTION_CHECKANIFILE)) == 0)
	{
		DragonHook::Utils::Message("Can not find ANTICHEAT_CRCCHECK Adresss please update Pattern..");
		return false;
	}

	HOOK_Animastion = (void(__cdecl*)(const char*[500]))(DWORD*)PTR_ANIFILEMD5;


	DetourRestoreAfterWith();
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());


	DetourAttach(&(LPVOID&)PTR_ANIFILEMD5, Test);


	if (DetourTransactionCommit() != 0)
	{
		DragonHook::Utils::Message("Failed To Commit Detours please Check you Adresses..");
		return false;
	}
	return true;
}

void Animation::Detach()
{
//	throw gcnew System::NotImplementedException();
}

Animation::Animation()
{
}
