// CppCLR_WinformsProjekt.cpp: Hauptprojektdatei.

// #include "stdafx.h"
// using namespace System;

//int main(array<System::String ^> ^args)
//{
//    Console::WriteLine(L"Hello World");
//    return 0;
//}
#include "stdafx.h"
#include "Main.h"
#include "MainForm.h"

using namespace System;
using namespace System::Windows::Forms;

bool DragonLauncher::Main::CheckFilesExits()
{
	//Check hook Dll Exis...
	if (!File::Exists(Application::StartupPath + "\\DragonHook.dll"))
	{
		MessageBox::Show("Wee need DragonHook.dll to injecting...");
		return false;
	}
	//Check if Fiesta exis..
	else if (!File::Exists(Application::StartupPath + "\\Fiesta.bin"))
	{
		MessageBox::Show("Fiesta.bin not found!");
		return false;
	}
	return true;
}

[STAThread]
// int main(array<String^>^ args) { // Kann Fehler nach 'using namespace std;' verursachen
int main() {
	if (DragonLauncher::Main::CheckFilesExits())
	{
		Application::EnableVisualStyles();
		Application::SetCompatibleTextRenderingDefault(false);
		Application::Run(gcnew GUIForms::MainForm());
		return 0;
	}
	return 0;
}