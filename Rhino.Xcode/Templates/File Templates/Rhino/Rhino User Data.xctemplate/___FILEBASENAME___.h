//___FILEHEADER___

#pragma once

// Custom data attached to an object's attributes or geometry, saved in .3dm files.
// Attach with:  attributes.AttachUserData(new ___FILEBASENAMEASIDENTIFIER___());
// Find with:    ___FILEBASENAMEASIDENTIFIER___::Cast(attributes.GetUserData(ON_CLASS_ID(___FILEBASENAMEASIDENTIFIER___)));
class ___FILEBASENAMEASIDENTIFIER___ : public ON_UserData
{
  ON_OBJECT_DECLARE(___FILEBASENAMEASIDENTIFIER___);

public:
  ___FILEBASENAMEASIDENTIFIER___();
  ~___FILEBASENAMEASIDENTIFIER___() = default;
  ___FILEBASENAMEASIDENTIFIER___(const ___FILEBASENAMEASIDENTIFIER___& src);
  ___FILEBASENAMEASIDENTIFIER___& operator=(const ___FILEBASENAMEASIDENTIFIER___& src);

  // ON_UserData overrides
  bool GetDescription(ON_wString& description) override;
  bool Archive() const override;
  bool Transform(const ON_Xform& xform) override;

  // ON_Object overrides
  bool Write(ON_BinaryArchive& archive) const override;
  bool Read(ON_BinaryArchive& archive) override;

  // TODO: Add your data here, and read and write it below.
  ON_wString m_notes;
};
