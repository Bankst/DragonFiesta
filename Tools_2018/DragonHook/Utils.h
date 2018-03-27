#pragma once

#include <string>
#include <vector>
#include <Windows.h>

namespace DragonHook
{
	class  Utils
	{
	public:
		static void Message(std::string Message);

		static std::vector<std::string> Split(const std::string& input, const std::string& regex);

		static std::string AddressToString(DWORD Adress);
	};
}