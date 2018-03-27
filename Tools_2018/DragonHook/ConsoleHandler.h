#pragma once
#include <list>
#include <map>
#include <functional>
#include <vector>
#include <unordered_map>
#include <iostream>

class ConsoleHandler
{

public:


	static ConsoleHandler* getInstance();

	static bool InitialConsoleHandler();

	void HandleCommand(std::string Cmd);

	/*Handlers BEGIN*/


	static void TestFunc(std::vector<std::string> Test);

	static void TestPattern(std::vector<std::string> Pattern);

	/*Handlers END*/

private:
	static std::unordered_map<std::string, std::function<void(std::vector<std::string>)>> Instruction;

	static ConsoleHandler* Instance;

	ConsoleHandler();
	~ConsoleHandler();
};

