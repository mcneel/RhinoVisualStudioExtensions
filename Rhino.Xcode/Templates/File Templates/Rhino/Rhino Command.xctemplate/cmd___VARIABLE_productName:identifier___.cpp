//___FILEHEADER___

// Do NOT put the definition of class CCommand___VARIABLE_productName:identifier___ in a header file.
// There is only ONE instance of a CCommand___VARIABLE_productName:identifier___ class and that instance
// is the static the___VARIABLE_productName:identifier___Command that appears immediately below the class definition.

class CCommand___VARIABLE_productName:identifier___ : public CRhinoCommand
{
public:
  CCommand___VARIABLE_productName:identifier___() = default;
  ~CCommand___VARIABLE_productName:identifier___() = default;

  // Returns a unique UUID for this command.
  // If you try to use an id that is already being used, then
  // your command will not work.
  UUID CommandUUID() override
  {
    static const UUID id = ON_UuidFromString("___UUID___");
    return id;
  }

  // Returns the English command name.
  // If you want to provide a localized command name, then override
  // CRhinoCommand::LocalCommandName.
  const wchar_t* EnglishCommandName() override { return L"___VARIABLE_productName:identifier___"; }

  // Rhino calls RunCommand to run the command.
  CRhinoCommand::result RunCommand(const CRhinoCommandContext& context) override;
};

// The one and only CCommand___VARIABLE_productName:identifier___ object
// Do NOT create any other instance of a CCommand___VARIABLE_productName:identifier___ class.
static class CCommand___VARIABLE_productName:identifier___ the___VARIABLE_productName:identifier___Command;

CRhinoCommand::result CCommand___VARIABLE_productName:identifier___::RunCommand(const CRhinoCommandContext& context)
{
  // TODO: Add command code here.

  ON_wString str;
  str.Format(L"The \"%ls\" command is under construction.\n", EnglishCommandName());
  if (context.IsInteractive())
    RhinoMessageBox(str, EnglishCommandName(), MB_OK);
  else
    RhinoApp().Print(str);

  return CRhinoCommand::success;
}
