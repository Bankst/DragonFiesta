#include "stdafx.h"
#include "LoadForm.h"

#include <string>

void GUIForms::LoadForm::StartFiesta(std::string Arguments)
{	
	STARTUPINFO StartInfo = { 0 };
	PROCESS_INFORMATION ProccessInfo = { 0 };

	BOOL StartingOK = CreateProcessA(NULL, const_cast<LPSTR>(("Fiesta.bin " + Arguments).c_str()), NULL, NULL, TRUE,
		CREATE_NEW_PROCESS_GROUP, NULL, NULL, &StartInfo, &ProccessInfo);

	WaitForSingleObject(ProccessInfo.hProcess, INFINITE);

	if (!StartingOK)
	{
		MessageBox::Show("Failed to Start Fiesta...");
	}

	// Close process and thread handles. 
	CloseHandle(ProccessInfo.hProcess);
    CloseHandle(ProccessInfo.hThread);

}

void GUIForms::LoadForm::InitializeComponent(void)
{
	this->label1 = (gcnew System::Windows::Forms::Label());
	this->Label_Load = (gcnew System::Windows::Forms::Label());
	this->label2 = (gcnew System::Windows::Forms::Label());
	this->SuspendLayout();
	// 
	// label1
	// 
	this->label1->AutoSize = true;
	this->label1->Location = System::Drawing::Point(27, 92);
	this->label1->Name = L"label1";
	this->label1->Size = System::Drawing::Size(40, 13);
	this->label1->TabIndex = 1;
	this->label1->Text = L"Status:";
	// 
	// Label_Load
	// 
	this->Label_Load->AutoSize = true;
	this->Label_Load->Location = System::Drawing::Point(118, 92);
	this->Label_Load->Name = L"Label_Load";
	this->Label_Load->Size = System::Drawing::Size(72, 13);
	this->Label_Load->TabIndex = 2;
	this->Label_Load->Text = L"Wait of Fiesta";
	// 
	// label2
	// 
	this->label2->AutoSize = true;
	this->label2->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 20.25F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point,
		static_cast<System::Byte>(0)));
	this->label2->Location = System::Drawing::Point(24, 23);
	this->label2->Name = L"label2";
	this->label2->Size = System::Drawing::Size(190, 31);
	this->label2->TabIndex = 3;
	this->label2->Text = L"DragonFiesta";
	// 
	// LoadForm
	// 
	this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
	this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
	this->ClientSize = System::Drawing::Size(251, 127);
	this->ControlBox = false;
	this->Controls->Add(this->label2);
	this->Controls->Add(this->Label_Load);
	this->Controls->Add(this->label1);
	this->MaximizeBox = false;
	this->MinimizeBox = false;
	this->Name = L"LoadForm";
	this->ShowIcon = false;
	this->ShowInTaskbar = false;
	this->SizeGripStyle = System::Windows::Forms::SizeGripStyle::Hide;
	this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
	this->ResumeLayout(false);
	this->PerformLayout();

}