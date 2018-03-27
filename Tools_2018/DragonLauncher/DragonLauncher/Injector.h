#pragma once

namespace DragonLauncher
{
	using namespace System::Threading;
	using namespace System;
	using namespace System::Diagnostics;
	using namespace System::Windows::Forms;

	ref class Injector
	{
	public:
		Injector();
		~Injector();
	public:
		static void Start();
		static void Stop();

	protected:

		static Thread ^InjectThread;

	private:
		static void InjectWorker();

	};
}