#pragma once
class Animation
{
private :
	DWORD PTR_ANIFILEMD5;
	void(__cdecl* HOOK_Animastion)(const char*[500]);

	
public :
	static void CheckAniFileMD5(std::string Name, std::string SubDir, std::string FileName);
	static void Test(const char* md5[500]);
    bool Attach();
	void Detach();

	Animation();
};

