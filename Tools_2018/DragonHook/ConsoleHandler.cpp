#include "stdafx.h"
#include "ConsoleHandler.h"
#include "FiestaHook.h"

#include <iostream>
#include <string>
#include <vector>

ConsoleHandler* ConsoleHandler::Instance = 0;

std::unordered_map<std::string,
	std::function<void(std::vector<std::string>)>> ConsoleHandler::Instruction = 

	std::unordered_map<std::string,
	std::function<void(std::vector<std::string>)>>();

ConsoleHandler::ConsoleHandler()
{
}

ConsoleHandler::~ConsoleHandler()
{

}

ConsoleHandler* ConsoleHandler::getInstance()
{
	if (Instance == 0)
	{
		Instance = new ConsoleHandler();
		Instance->InitialConsoleHandler();
	}

	return Instance;
}

bool ConsoleHandler::InitialConsoleHandler()
{

	Instruction.emplace("KLO", &ConsoleHandler::TestFunc);
	Instruction.emplace("TestPattern", &ConsoleHandler::TestPattern);
	
	return false;
}

void ConsoleHandler::HandleCommand(std::string Cmd)
{
	std::vector<std::string> args = DragonHook::Utils::Split(Cmd, " ");
	if (args.size() > 0)
	{

		auto Handler = Instruction.find(args[0]);

		args.erase(args.begin()); //Remove command string..

		if (Handler != Instruction.end())
		{
			Handler->second(args); //Call Handler...
		}
		else
		{
			std::cout << "Command not found!" << std::endl;
		}
	}
}

void ConsoleHandler::TestFunc(std::vector<std::string> Test)
{

		DragonHook::FiestaHook::AntiCheats->KILLFO();
	
}

void ConsoleHandler::TestPattern(std::vector<std::string> args)
{
	if (args.size() > 2)
	{
		std::cout << "invalid arguments" << std::endl;
		return;
	}
	//TODO Fix it...
	DWORD Addy = DragonHook::FiestaHook::FindPattern(strdup(args[0].c_str()), strdup(args[1].c_str()));

	if (Addy == 0)
	{
		std::cout << "No Match found! by pattern " << args[0] << "Mask" << args[1] << std::endl;

	}
	else
	{
		std::cout << "Pattern match found on adress " << Addy << std::endl;
	}

}
