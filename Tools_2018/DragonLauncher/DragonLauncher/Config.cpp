#include "Config.h"

#include "stdafx.h"

Config::Config()
{
}

Config::~Config()
{
}

Config ^ Config::getInstance()
{
	if (Instance == nullptr)
	{
		Instance = Config::Load();

		if (Instance == nullptr)
		{
			Instance = gcnew Config();

			//Set default
			Instance->ServerIP = "46.253.154.134";
		}

	}

	return Instance;
}

Config^ Config::Load()
{
	if (File::Exists("Config.xml"))
	{
		auto reader = gcnew XmlSerializer(Config::typeid);
		auto file = gcnew StreamReader("Config.xml");

		Config^ xml = (Config^)reader->Deserialize(file);

		file->Close();

		return xml;
	}
	else
	{
		return  nullptr;
	}
}

void Config::Save()
{
	auto writer = gcnew XmlSerializer(Config::typeid);

	auto  file = gcnew StreamWriter("Config.xml");

	writer->Serialize(file, this);

	file->Close();
}
