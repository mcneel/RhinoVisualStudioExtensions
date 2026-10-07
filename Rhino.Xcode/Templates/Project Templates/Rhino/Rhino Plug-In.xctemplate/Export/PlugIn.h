//___FILEHEADER___

#pragma once

// C___PACKAGENAMEASIDENTIFIER___PlugIn
// See ___PACKAGENAMEASIDENTIFIER___PlugIn.cpp for the implementation of this class.

class C___PACKAGENAMEASIDENTIFIER___PlugIn : public CRhinoFileExportPlugIn
{
public:
  // Called when the plug-in is loaded and "thePlugIn" is constructed. Keep it
  // simple and solid; do anything that might fail in OnLoadPlugIn().
  C___PACKAGENAMEASIDENTIFIER___PlugIn();

  // Called to destroy "thePlugIn" when the plug-in is unloaded. Clean up any memory
  // you have allocated with onmalloc(), onrealloc(), oncalloc(), or onstrdup().
  ~C___PACKAGENAMEASIDENTIFIER___PlugIn() = default;

  // Required overrides

  // Plug-in name display string. This name is displayed by Rhino when
  // loading the plug-in, in the plug-in help menu, and in the Rhino
  // interface for managing plug-ins.
  const wchar_t* PlugInName() const override;

  // Plug-in version display string. This name is displayed by Rhino
  // when loading the plug-in and in the Rhino interface for
  // managing plug-ins.
  const wchar_t* PlugInVersion() const override;

  // Plug-in unique identifier. The identifier is used by Rhino for
  // managing plug-ins.
  GUID PlugInID() const override;

  // Additional overrides

  // Called after the plug-in is loaded and the constructor has been
  // run. This is a good place to perform any significant initialization,
  // license checking, and so on. This function must return TRUE for
  // the plug-in to continue to load.
  int OnLoadPlugIn() override;

  // Called one time when plug-in is about to be unloaded. By this time,
  // some of the SDK managers have been deleted and there is no active
  // document or view, so only manipulate your own objects here.
  void OnUnloadPlugIn() override;

  // File export overrides

  // Called by Rhino when displaying the save file dialog.
  // Add supported file type extensions here.
  void AddFileType(ON_ClassArray<CRhinoFileType>& extensions, const CRhinoFileWriteOptions& options) override;

  // Called by Rhino to write document geometry to an external file.
  int WriteFile(const wchar_t* filename, int index, CRhinoDoc& doc, const CRhinoFileWriteOptions& options) override;

private:
  ON_wString m_plugin_version;

  // TODO: Add additional class information here
};

// Return a reference to the one and only C___PACKAGENAMEASIDENTIFIER___PlugIn object
C___PACKAGENAMEASIDENTIFIER___PlugIn& ___PACKAGENAMEASIDENTIFIER___PlugIn();
