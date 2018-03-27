
#include <Windows.h>

#include <psapi.h>
#pragma comment(lib, "psapi.lib")
#include <stdio.h>

#include "cAntiCheat.h"
#include "Animation.h"

#pragma unmanaged

using namespace System;

namespace DragonHook {

	class  FiestaHook
	{
	public:
		static void Start();
		static void Stop();

		
		static  DWORD FindPattern(char *pattern, char *mask);
		static void WriteToMemory(uintptr_t addressToWrite, char* valueToWrite, int byteNum);

		static cAntiCheat* AntiCheats;
		static Animation* Anim;

	private:
		static DWORD HookThread;
		static DWORD WINAPI Worker(LPVOID lParam);
		static HANDLE WorkThread;
		static bool IsStartet;


		static void AttachFunctions();
		static void DetachFunctions();

	private:
		static DWORD BaseAddress;
		static DWORD ModuleSize;


		
	};
}