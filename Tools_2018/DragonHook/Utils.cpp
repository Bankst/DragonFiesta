#include "Stdafx.h"

#include <Windows.h>

#include<iostream>

#include <fstream>
#include <iterator>
#include <regex>

void DragonHook::Utils::Message(std::string Text)
{
	MessageBox(NULL, Text.c_str(), NULL, MB_OK);
}

std::vector<std::string> DragonHook::Utils::Split(const std::string& input, const std::string& regex) {
	// passing -1 as the submatch index parameter performs splitting
	std::regex re(regex);
	std::sregex_token_iterator
		first{ input.begin(), input.end(), re, -1 },
		last;
	return { first, last };
}

std::string DragonHook::Utils::AddressToString(DWORD Adress)
{
	char szBuffer[1024];
	sprintf(szBuffer, "0x%02x", Adress);
	return std::string(szBuffer);
}
