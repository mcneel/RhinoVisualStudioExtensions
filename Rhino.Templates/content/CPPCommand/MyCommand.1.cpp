// cmdMyCommand.1.cpp
//

#include "stdafx.h"


////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
//
// BEGIN MyCommand__1 command
//

#pragma region MyCommand__1 command

class CCommandMyCommand__1 : public CRhinoCommand
{
public:
	CCommandMyCommand__1() = default;
	UUID CommandUUID() override
	{
		// {8EE67436-E89F-4576-904A-6525C595B531}
		static const GUID MyCommand__1Command_UUID =
		{0x8ee67436,0xe89f,0x4576,{0x90,0x4a,0x65,0x25,0xc5,0x95,0xb5,0x31}};
		return MyCommand__1Command_UUID;
	}
	const wchar_t* EnglishCommandName() override { return L"MyCommand__1"; }
	CRhinoCommand::result RunCommand(const CRhinoCommandContext& context) override;
};

// The one and only CCommandMyCommand__1 object
static class CCommandMyCommand__1 theMyCommand__1Command;

CRhinoCommand::result CCommandMyCommand__1::RunCommand(const CRhinoCommandContext& context)
{
	ON_wString str;
	str.Format( L"The \"%s\" command is under construction.\n", EnglishCommandName() );
	if (context.IsInteractive())
		RhinoMessageBox(str, EnglishCommandName(), MB_OK);
	else
		RhinoApp().Print(str);
	return CRhinoCommand::success;
}

#pragma endregion

//
// END MyCommand__1 command
//
////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
