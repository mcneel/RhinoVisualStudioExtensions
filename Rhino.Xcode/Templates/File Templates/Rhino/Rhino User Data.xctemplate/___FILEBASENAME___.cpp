//___FILEHEADER___

#include "___FILEBASENAME___.h"

// Defined by RHINO_PLUG_IN_ID in the plug-in's main source file.
extern "C" const wchar_t* RhinoPlugInId(void);

// The id identifies this class in .3dm files; never change it once files are saved.
ON_OBJECT_IMPLEMENT(___FILEBASENAMEASIDENTIFIER___, ON_UserData, "___UUID___");

___FILEBASENAMEASIDENTIFIER___::___FILEBASENAMEASIDENTIFIER___()
{
  m_userdata_uuid = ON_CLASS_ID(___FILEBASENAMEASIDENTIFIER___);
  // Rhino loads this plug-in when it reads a file containing this data.
  m_application_uuid = ON_UuidFromString(RhinoPlugInId());
  // Keep the data when the object it is attached to is copied.
  m_userdata_copycount = 1;
}

___FILEBASENAMEASIDENTIFIER___::___FILEBASENAMEASIDENTIFIER___(const ___FILEBASENAMEASIDENTIFIER___& src)
  : ON_UserData(src)
  , m_notes(src.m_notes)
{
  m_userdata_uuid = ON_CLASS_ID(___FILEBASENAMEASIDENTIFIER___);
  m_application_uuid = src.m_application_uuid;
}

___FILEBASENAMEASIDENTIFIER___& ___FILEBASENAMEASIDENTIFIER___::operator=(const ___FILEBASENAMEASIDENTIFIER___& src)
{
  if (this != &src)
  {
    ON_UserData::operator=(src);
    m_notes = src.m_notes;
  }
  return *this;
}

bool ___FILEBASENAMEASIDENTIFIER___::GetDescription(ON_wString& description)
{
  description = L"___FILEBASENAMEASIDENTIFIER___";
  return true;
}

bool ___FILEBASENAMEASIDENTIFIER___::Archive() const
{
  // Save this data in .3dm files.
  return true;
}

bool ___FILEBASENAMEASIDENTIFIER___::Transform(const ON_Xform& xform)
{
  // TODO: Transform any points or vectors you store.
  return ON_UserData::Transform(xform);
}

bool ___FILEBASENAMEASIDENTIFIER___::Write(ON_BinaryArchive& archive) const
{
  // Bump the minor version when you add fields, and the major version
  // when older code can no longer read the data.
  if (!archive.BeginWrite3dmChunk(TCODE_ANONYMOUS_CHUNK, 1, 0))
    return false;
  bool rc = archive.WriteString(m_notes);
  if (!archive.EndWrite3dmChunk())
    rc = false;
  return rc;
}

bool ___FILEBASENAMEASIDENTIFIER___::Read(ON_BinaryArchive& archive)
{
  int major_version = 0;
  int minor_version = 0;
  if (!archive.BeginRead3dmChunk(TCODE_ANONYMOUS_CHUNK, &major_version, &minor_version))
    return false;
  bool rc = (1 == major_version) && archive.ReadString(m_notes);
  if (!archive.EndRead3dmChunk())
    rc = false;
  return rc;
}
