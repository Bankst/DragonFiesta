#include "stdafx.h"
#include "DLLConsole.h"
#include "ConsoleHandler.h"
#include <iostream>


bool DLLConsole::IsOpen = false;
DWORD DLLConsole::ConsoleThreadId = 0;
HANDLE DLLConsole::ConsoleThread = NULL;

void DLLConsole::WriteLine(std::string Text)
{
	std::cout << Text << std::endl;
}

void DLLConsole::Open()
{
	if (!IsOpen)
	{
	
		AllocConsole();
		

		//handels..
		freopen("CONIN$", "r", stdin);
		freopen("CONOUT$", "w", stdout);
		freopen("CONOUT$", "w", stderr);
		

		IsOpen = true;

		DLLConsole::ConsoleThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&DLLConsole::ConsoleWorker, 0, 0, &DLLConsole::ConsoleThreadId);

	}
}

void DLLConsole::Close()
{
	if (IsOpen)
	{

		WaitForSingleObject(DLLConsole::ConsoleThread, INFINITE);
		FreeConsole();// Close Console..
		CloseHandle(DLLConsole::ConsoleThread);

		IsOpen = false;
	}
}

std::string DLLConsole::GetLine()
{
	std::string str;
	std::getline(std::cin, str);
	return str;
}

DWORD DLLConsole::ConsoleWorker(LPVOID lParam)
{
	while (IsOpen)
	{
		ConsoleHandler::getInstance()->HandleCommand(DLLConsole::GetLine());
	}

	return 1;
}
