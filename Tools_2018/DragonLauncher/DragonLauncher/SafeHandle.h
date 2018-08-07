#include <iostream>
#include <Windows.h>
#include <memory>


namespace DragonLauncher
{

	class SafeHandle {
	public:
		SafeHandle(const SafeHandle&) = delete;
		SafeHandle& operator=(const SafeHandle& other) = delete;

		SafeHandle(HANDLE handle) {
			this->m_ManagedHandle = handle;
		}

		SafeHandle(SafeHandle&& other) {
			this->m_ManagedHandle = other.m_ManagedHandle;
			other.m_ManagedHandle = INVALID_HANDLE_VALUE;
		}

		SafeHandle& operator=(SafeHandle&& other) {
			if (this != &other) {
				this->m_ManagedHandle = other.m_ManagedHandle;
				other.m_ManagedHandle = INVALID_HANDLE_VALUE;
			}

			return *this;
		}

		~SafeHandle() {
			if (this->m_ManagedHandle != NULL && this->m_ManagedHandle != INVALID_HANDLE_VALUE)
				CloseHandle(this->m_ManagedHandle);
		}

	public:
		HANDLE get() const { return this->m_ManagedHandle; }
		static SafeHandle GetProcessByName(char* name);

	private:
		HANDLE m_ManagedHandle;
	};
}