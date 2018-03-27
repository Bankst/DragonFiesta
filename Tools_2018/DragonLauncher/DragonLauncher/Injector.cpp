
#include "stdafx.h"
#include "Injector.h"

#include "MainForm.h"

DragonLauncher::Injector::Injector()
{


}

DragonLauncher::Injector::~Injector()
{
	delete Injector::InjectThread;
}

void DragonLauncher::Injector::Start()
{
 Injector::InjectThread = gcnew Thread((gcnew ThreadStart(&Injector::InjectWorker)));
 Injector::InjectThread->Start();
}
void DragonLauncher::Injector::Stop()
{
	Injector::InjectThread->Abort();
}

void DragonLauncher::Injector::InjectWorker()
{



	while (true)
	{
		DragonLauncher::SafeHandle Proc = DragonLauncher::SafeHandle::GetProcessByName("Fiesta.bin");

		if (Proc.get() != NULL)
		{

			//HMM ... why workt it only this..
			std::string stringPath(Utils::GetStartUpPath() + "\\DragonHook.dll");

			LPCSTR DllPath = stringPath.c_str();


			// Allocate memory for the dllpath in the target process
			// length of the path string + null terminator
			Allocator mem(VirtualAllocEx(Proc.get(), nullptr, strlen(DllPath) + 1,
				MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE), Proc.get());


			if (!mem.get())
			{
				MessageBox::Show("Failed to Alocate Fiesta...");
				break;
			}


			// Write the path to the address of the memory we just allocated
			// in the target process
			if (!WriteProcessMemory(Proc.get(), mem.get(), DllPath, strlen(DllPath) + 1, nullptr))
			{
				MessageBox::Show("Failed to Write Hook Into Fiesta...");
				break;
			}

			//wait to alocate from fiesta...
			System::Threading::Thread::Sleep(500);

			// Create a Remote Thread in the target process which
				// calls LoadLibraryA as our dllpath as an argument -> program loads our dll
			HANDLE hLoadThread = CreateRemoteThread(Proc.get(), nullptr, 0,
				reinterpret_cast<LPTHREAD_START_ROUTINE>(GetProcAddress(GetModuleHandleA("Kernel32.dll"),
					"LoadLibraryA")), mem.get(), 0, nullptr);


			if (!hLoadThread)
			{
				MessageBox::Show("Failed to Start Remote Thread...");
				break;

			}

			WaitForSingleObject(hLoadThread, INFINITE);

			break;
		}
		System::Threading::Thread::Sleep(50);
	}


	//TODO FIX Memory Leak from MainForm..
	System::Environment::Exit(0);
}