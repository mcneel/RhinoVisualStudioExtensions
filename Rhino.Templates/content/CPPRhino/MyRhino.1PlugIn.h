// MyRhino.1PlugIn.h : main header file for the MyRhino.1 plug-in.
//

#pragma once
#if TypeRender

#include "MyRhino.1EventWatcher.h"

class CMyRhino__1RdkPlugIn;
#endif

// CMyRhino__1PlugIn
// See MyRhino.1PlugIn.cpp for the implementation of this class
//

#if TypeUtility
class CMyRhino__1PlugIn : public CRhinoUtilityPlugIn
#elif TypeRender
class CMyRhino__1PlugIn : public CRhinoRenderPlugIn
#elif TypeDigitize
class CMyRhino__1PlugIn : public CRhinoDigitizerPlugIn
#elif TypeImport
class CMyRhino__1PlugIn : public CRhinoFileImportPlugIn
#elif TypeExport
class CMyRhino__1PlugIn : public CRhinoFileExportPlugIn
#endif
{
public:
  // CMyRhino__1PlugIn constructor. The constructor is called when the
  // plug-in is loaded and "thePlugIn" is constructed. Once the plug-in
  // is loaded, CMyRhino__1PlugIn::OnLoadPlugIn() is called. The
  // constructor should be simple and solid. Do anything that might fail in
  // CMyRhino__1PlugIn::OnLoadPlugIn().
  CMyRhino__1PlugIn();
  
  // CMyRhino__1PlugIn destructor. The destructor is called to destroy
  // "thePlugIn" when the plug-in is unloaded. Immediately before the
  // DLL is unloaded, CMyRhino__1PlugIn::OnUnloadPlugin() is called. Do
  // not do too much here. Be sure to clean up any memory you have allocated
  // with onmalloc(), onrealloc(), oncalloc(), or onstrdup().
  ~CMyRhino__1PlugIn() = default;

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
  // license checking, and so on.  This function must return TRUE for
  // the plug-in to continue to load.  
  BOOL OnLoadPlugIn() override;
  
  // Called one time when plug-in is about to be unloaded. By this time,
  // Rhino's mainframe window has been destroyed, and some of the SDK
  // managers have been deleted. There is also no active document or active
  // view at this time. Thus, you should only be manipulating your own objects.
  // or tools here.  
  void OnUnloadPlugIn() override;
#if TypeDigitize

  // Digitizer overrides
  
  // Called by Rhino to enable or disable input from the digitizer.
  bool EnableDigitizer(bool bEnable) override;
  
  // Return the length unit system the digitizer is using. 
  // Rhino uses this value when it calibrates a digitizer.
  ON::LengthUnitSystem UnitSystem() const override;
  
  // Return the point tolerance of the digitizer.
  double PointTolerance() const override;
#endif
#if TypeImport

  // File import overrides
  
  // Called by Rhino when displaying the open file dialog.
  // Add supported file type extensions here.
  void AddFileType(ON_ClassArray<CRhinoFileType>& extensions, const CRhinoFileReadOptions& options) override;
  
  // Called by Rhino to read document geometry from an external file.
  BOOL ReadFile(const wchar_t* filename, int index, CRhinoDoc& doc, const CRhinoFileReadOptions& options) override;
#endif
#if TypeExport

  // File export overrides
  
  // Called by Rhino when displaying the open file dialog
  // Add supported file type extensions here.  
  void AddFileType(ON_ClassArray<CRhinoFileType>& extensions, const CRhinoFileWriteOptions& options) override;
  
  // Called by Rhino to write document geometry to an external file.
  BOOL WriteFile(const wchar_t* filename, int index, CRhinoDoc& doc, const CRhinoFileWriteOptions& options) override;
#endif
#if TypeRender

  // Render overrides
  CRhinoCommand::result Render(const CRhinoCommandContext& context, bool bPreview) override;
  CRhinoCommand::result RenderWindow(
    const CRhinoCommandContext& context, 
    bool render_preview, 
    CRhinoView* view, 
    const LPRECT rect, 
    bool bInWindow, 
    bool bBlowUp
    ) override;

  BOOL SaveRenderedImage(ON_wString filename) override;
  BOOL CloseRenderWindow() override;

  // Render methods
  CRhinoCommand::result RenderQuiet( const CRhinoCommandContext& context, bool bPreview);
  BOOL SceneChanged() const;
  void SetSceneChanged(BOOL bChanged);
  BOOL LightingChanged() const;
  void SetLightingChanged(BOOL bChanged);
  UINT MainFrameResourceID() const;
#endif

private:
  ON_wString m_plugin_version;
#if TypeRender
  CMyRhino__1EventWatcher m_event_watcher;
  CMyRhino__1RdkPlugIn* m_pRdkPlugIn;
#endif

  // TODO: Add additional class information here
};

// Return a reference to the one and only CMyRhino__1PlugIn object
CMyRhino__1PlugIn& MyRhino__1PlugIn();



