//___FILEHEADER___

#pragma once

// Receives Rhino events once registered and enabled, e.g. in OnLoadPlugIn():
//   m_watcher.Register();
//   m_watcher.Enable(TRUE);
// Override any other CRhinoEventWatcher functions you need.
class ___FILEBASENAMEASIDENTIFIER___ : public CRhinoEventWatcher
{
public:
  ___FILEBASENAMEASIDENTIFIER___() = default;

  void OnNewDocument(CRhinoDoc& doc) override;
  void OnCloseDocument(CRhinoDoc& doc) override;
  void OnEndOpenDocument(CRhinoDoc& doc, const wchar_t* filename, BOOL32 bMerge, BOOL32 bReference) override;
  void OnAddObject(CRhinoDoc& doc, CRhinoObject& object) override;
  void OnDeleteObject(CRhinoDoc& doc, CRhinoObject& object) override;
  void OnReplaceObject(CRhinoDoc& doc, CRhinoObject& old_object, CRhinoObject& new_object) override;
};
