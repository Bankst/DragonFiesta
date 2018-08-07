#include "stdafx.h"
#include "MainForm.h"
#include "LoadForm.h"
#include "Injector.h"
#include "Config.h"

#include <msclr\marshal_cppstd.h>

System::Void GUIForms::MainForm::Button_Launch_Click(System::Object ^ sender, System::EventArgs ^ e)
{
	if (TextBox_ServerIP->Text->Length == 0)
	{
		MessageBox::Show("We need Server IP To Connect...");

		return;
	}

	IPAddress^ Adress;
	if (!IPAddress::TryParse(TextBox_ServerIP->Text, Adress))
	{
		MessageBox::Show("Please we need Korrectly IP To Connect Sory i dont worry :D");
		return;
	}
	
	(gcnew GUIForms::LoadForm())->Show();
	
	this->Visible = false;

	Config::getInstance()->ServerIP = this->TextBox_ServerIP->Text;
	Config::getInstance()->Save();
	
	DragonLauncher::Injector::Start();
	
	GUIForms::LoadForm::StartFiesta(msclr::interop::marshal_as<std::string>("-osk_server " +TextBox_ServerIP->Text));
}


void GUIForms::MainForm::InitializeComponent(void)
{

	this->Button_Launch = (gcnew System::Windows::Forms::Button());
	this->TextBox_ServerIP = (gcnew System::Windows::Forms::TextBox());
	this->Label_ServerIP = (gcnew System::Windows::Forms::Label());
	this->SuspendLayout();
	// 
	// Button_Launch
	// 

	this->Button_Launch->Location = System::Drawing::Point(272, 22);
	this->Button_Launch->Name = L"Button_Launch";
	this->Button_Launch->Size = System::Drawing::Size(75, 23);
	this->Button_Launch->TabIndex = 0;
	this->Button_Launch->Text = L"Launch";
	this->Button_Launch->UseVisualStyleBackColor = true;
	this->Button_Launch->Click += gcnew System::EventHandler(this, &MainForm::Button_Launch_Click);
	// 
	// TextBox_ServerIP
	// 
	this->TextBox_ServerIP->Location = System::Drawing::Point(82, 22);
	this->TextBox_ServerIP->Name = L"TextBox_ServerIP";
	this->TextBox_ServerIP->Size = System::Drawing::Size(156, 20);
	this->TextBox_ServerIP->TabIndex = 1;
	this->TextBox_ServerIP->Text = Config::getInstance()->ServerIP;
	// 
	// Label_ServerIP
	// 
	this->Label_ServerIP->AutoSize = true;
	this->Label_ServerIP->Location = System::Drawing::Point(2, 22);
	this->Label_ServerIP->Name = L"Label_ServerIP";
	this->Label_ServerIP->Size = System::Drawing::Size(54, 13);
	this->Label_ServerIP->TabIndex = 2;
	this->Label_ServerIP->Text = L"ServerIP :";
	// 
	// MainForm
	// 
	this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
	this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
	this->ClientSize = System::Drawing::Size(351, 66);
	this->Controls->Add(this->Label_ServerIP);
	this->Controls->Add(this->TextBox_ServerIP);
	this->Controls->Add(this->Button_Launch);
	this->MaximizeBox = false;
	this->MinimizeBox = false;
	this->Name = L"MainForm";
	this->Text = L"DragonLauncher";
	this->ResumeLayout(false);
	this->PerformLayout();
}
