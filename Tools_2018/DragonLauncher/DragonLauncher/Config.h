
#include "stdafx.h"

#pragma once




using namespace System;

using namespace System::IO;

using namespace System::Xml::Serialization;
using namespace System::Windows::Forms;

public ref class Config
{
public:
	Config();
	~Config();

	static Config^ getInstance();

	static Config^ Load();
	void Save();

	String^ ServerIP;
private:
	static Config^ Instance;

};


