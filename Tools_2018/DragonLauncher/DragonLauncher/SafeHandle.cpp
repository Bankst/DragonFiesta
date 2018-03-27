#include "stdafx.h"

#include <cstdio>
#include <windows.h>
#include <tlhelp32.h>
#include <string>


DragonLauncher::SafeHandle DragonLauncher::SafeHandle::GetProcessByName(char* name)
{
	DWORD pid = 0;

	// Create toolhelp snapshot.
	HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	PROCESSENTRY32 process;
	ZeroMemory(&process, sizeof(process));
	process.dwSize = sizeof(process);

	// Walkthrough all processes.
	if (Process32First(snapshot, &process))
	{
		do
		{
	
			// Compare process.szExeFile based on format of name, i.e., trim file path
			// trim .exe if necessary, etc.
			//Todo make better...
			if (strcmp(process.szExeFile, name)== 0)
			{
				pid = process.th32ProcessID;
				break;
			}
		} while (Process32Next(snapshot, &process));
	}

	CloseHandle(snapshot);

	if (pid != 0)
	{
		return OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
	}

	// Not found


	return NULL;
}