// cmd$fileinputname$.cpp
//

#include "stdafx.h"


////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
//
// BEGIN $safefileinputname$ command
//

#pragma region $safefileinputname$ command

class CCommand$safefileinputname$ : public CRhinoCommand
{
public:
	CCommand$safefileinputname$() = default;
	UUID CommandUUID() override
	{
		// {$guid1$}
		static const GUID $safefileinputname$Command_UUID =
		$guid1x$;
		return $safefileinputname$Command_UUID;
	}
	const wchar_t* EnglishCommandName() override { return L"$safefileinputname$"; }
	CRhinoCommand::result RunCommand(const CRhinoCommandContext& context) override;
};

// The one and only CCommand$safefileinputname$ object
static class CCommand$safefileinputname$ the$safefileinputname$Command;

CRhinoCommand::result CCommand$safefileinputname$::RunCommand(const CRhinoCommandContext& context)
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
// END $safefileinputname$ command
//
////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
