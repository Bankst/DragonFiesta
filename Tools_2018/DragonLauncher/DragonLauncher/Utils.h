#pragma once
namespace DragonLauncher
{


	class Utils
	{
	public:
		static std::string GetStartUpPath();
		static std::wstring ConvertToW2String(const std::string& s);
	};
}