// Dies ist die Haupt-DLL.


#include "stdafx.h"
#include "FiestaHook.h"
#include "Animation.h"
#include <iostream>
#include <string>
//fix linking...
bool DragonHook::FiestaHook::IsStartet = false;
DWORD DragonHook::FiestaHook::HookThread = 0;
HANDLE DragonHook::FiestaHook::WorkThread = NULL;

unsigned long DragonHook::FiestaHook::BaseAddress;
unsigned long DragonHook::FiestaHook::ModuleSize;

cAntiCheat* DragonHook::FiestaHook::AntiCheats;
Animation* DragonHook::FiestaHook::Anim;


void DragonHook::FiestaHook::Start()
{
	if (!FiestaHook::IsStartet)
	{
		// Could be injected earlier than expected

		while (!(BaseAddress = (unsigned long)GetModuleHandle(NULL)))
			Sleep(100);




		MODULEINFO modinfo;

		while (!GetModuleInformation(GetCurrentProcess(), GetModuleHandle(NULL), &modinfo, sizeof(MODULEINFO)))
			Sleep(100);

		ModuleSize = modinfo.SizeOfImage;

		// Wait for the application to finish loading

		MEMORY_BASIC_INFORMATION meminfo;

		while (true)
		{
			if (VirtualQuery((void*)ModuleSize, &meminfo, sizeof(MEMORY_BASIC_INFORMATION)))
				if (!(meminfo.Protect &PAGE_EXECUTE_WRITECOPY))
					break;

			Sleep(100);
		}

		FiestaHook::AntiCheats = new cAntiCheat();
		FiestaHook::Anim = new Animation();

		FiestaHook::AttachFunctions();
	
		FiestaHook::IsStartet = true;

		//Need for devs..
		//DLLConsole::Open();

		FiestaHook::WorkThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&FiestaHook::Worker, 0, 0, &FiestaHook::HookThread);
	}
}

void DragonHook::FiestaHook::Stop()
{

	if (FiestaHook::IsStartet)
	{
		FiestaHook::IsStartet = false;

		DLLConsole::Close();

		//Close Work thread..
		WaitForSingleObject(FiestaHook::WorkThread, INFINITE);
		CloseHandle(FiestaHook::WorkThread);
	}
	ExitThread(0);
}



DWORD DragonHook::FiestaHook::Worker(LPVOID lParam)
{
	while (FiestaHook::IsStartet)
	{
		Sleep(1000);
	}


	return 1;
}

void DragonHook::FiestaHook::AttachFunctions()
{
	if (!FiestaHook::AntiCheats->Attach() || !FiestaHook::Anim->Attach())
	{
		std::exit(0);
	}
}

void DragonHook::FiestaHook::DetachFunctions()
{
	FiestaHook::AntiCheats->Detach();
	FiestaHook::Anim->Detach();
}




DWORD DragonHook::FiestaHook::FindPattern(char * pattern, char * mask)
{
	DWORD patternLength = (DWORD)strlen(mask);

	for (DWORD i = 0; i < ModuleSize - patternLength; i++)
	{
		bool found = true;
		for (DWORD j = 0; j < patternLength; j++)
		{
			//if we have a ? in our mask then we have true by default, 
			//or if the bytes match then we keep searching until finding it or not
			found &= mask[j] == '?' || pattern[j] == *(char*)(BaseAddress + i + j);
		}

		//found = true, our entire pattern was found
		//return the memory addy so we can write to it
		if (found)
		{
			return BaseAddress + i;
		}
	}

	return 0;
}

void DragonHook::FiestaHook::WriteToMemory(uintptr_t addressToWrite, char * valueToWrite, int byteNum)
{
	//used to change our file access type, stores the old
	//access type and restores it after memory is written
	unsigned long OldProtection;
	//give that address read and write permissions and store the old permissions at oldProtection
	VirtualProtect((LPVOID)(addressToWrite), byteNum, PAGE_EXECUTE_READWRITE, &OldProtection);

	//write the memory into the program and overwrite previous value
	memcpy((LPVOID)addressToWrite, valueToWrite, byteNum);

	//reset the permissions of the address back to oldProtection after writting memory
	VirtualProtect((LPVOID)(addressToWrite), byteNum, OldProtection, NULL);
}
