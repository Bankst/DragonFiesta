
#include "Stdafx.h"
#include "Main.h"

#include <Windows.h>

#include "FiestaHook.h"



//DllMain 
BOOL APIENTRY DllMain(HINSTANCE hDll, DWORD callReason, LPVOID lpReserved) {

	switch (callReason)
	{
	case DLL_PROCESS_ATTACH:
		DragonHook::FiestaHook::Start();
		break;
	case DLL_PROCESS_DETACH:
		DragonHook::FiestaHook::Stop();
		break;
	}
	return 1;
}