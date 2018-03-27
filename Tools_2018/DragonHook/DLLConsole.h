#pragma once

#include <Windows.h>

class DLLConsole
{
public:
	static void WriteLine(std::string Text);
	static void Open();
	static void Close();

private:
	static bool IsOpen;
	static DWORD ConsoleThreadId;
	static HANDLE ConsoleThread;
	static std::string GetLine();
	static DWORD WINAPI ConsoleWorker(LPVOID lParam);
};

