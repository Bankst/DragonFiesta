#pragma once

#include <iostream>
#include <Windows.h>
#include <memory>


namespace DragonLauncher
{
	class Allocator {
	public:
		Allocator(void* memory, HANDLE proc = NULL) {
			this->_memory = memory;
			this->_optionalProcess = proc;
		}

		~Allocator() {
			if (this->_memory) {
				if (this->_optionalProcess) {
					VirtualFreeEx(this->_optionalProcess, this->_memory, 0, MEM_RELEASE);
				}
				else {
					VirtualFree(this->_memory, 0, MEM_RELEASE);
				}
			}
		}

	public:
		void* get() const { return _memory; }
		
	private:
		HANDLE _optionalProcess;
		void* _memory;
	};
}