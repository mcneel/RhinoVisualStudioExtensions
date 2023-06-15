// MyRhino.1PlugIn.cpp : defines the initialization routines for the plug-in.
//

#include "stdafx.h"
#include "rhinoSdkPlugInDeclare.h"
#include "MyRhino.1PlugIn.h"
#include "Resource.h"
#if TypeRender
#include "MyRhino.1RdkPlugIn.h"
#include "MyRhino.1SdkRender.h"
#endif

// The plug-in object must be constructed before any plug-in classes derived
// from CRhinoCommand. The #pragma init_seg(lib) ensures that this happens.
#pragma warning(push)
#pragma warning(disable : 4073)
#pragma init_seg(lib)
#pragma warning(pop)

// Rhino plug-in declaration
RHINO_PLUG_IN_DECLARE

// Rhino plug-in name
// Provide a short, friendly name for this plug-in.
RHINO_PLUG_IN_NAME(L"MyRhino.1");

// Rhino plug-in id
// Provide a unique uuid for this plug-in.
RHINO_PLUG_IN_ID(L"4FFB7B14-A8B1-4E8B-B431-671248EA137B");

// Rhino plug-in version
// Provide a version number string for this plug-in.
RHINO_PLUG_IN_VERSION(__DATE__ "  " __TIME__)

// Rhino plug-in description
// Provide a description of this plug-in.
RHINO_PLUG_IN_DESCRIPTION(L"MyRhino.1 plug-in for Rhinoceros®");

// Rhino plug-in icon resource id
// Provide an icon resource this plug-in.
// Icon resource should contain 16, 24, 32, 48, and 256-pixel image sizes.
RHINO_PLUG_IN_ICON_RESOURCE_ID(IDI_ICON);

// Rhino plug-in developer declarations
// TODO: fill in the following developer declarations with
// your company information. Note, all of these declarations
// must be present or your plug-in will not load.
//
// When completed, delete the following #error directive.
// #error Developer declarations block is incomplete!
RHINO_PLUG_IN_DEVELOPER_ORGANIZATION(L"My Company Name");
RHINO_PLUG_IN_DEVELOPER_ADDRESS(L"123 Developer Street\r\nCity State 12345-6789");
RHINO_PLUG_IN_DEVELOPER_COUNTRY(L"My Country");
RHINO_PLUG_IN_DEVELOPER_PHONE(L"123.456.7890");
RHINO_PLUG_IN_DEVELOPER_FAX(L"123.456.7891");
RHINO_PLUG_IN_DEVELOPER_EMAIL(L"support@mycompany.com");
RHINO_PLUG_IN_DEVELOPER_WEBSITE(L"http://www.mycompany.com");
RHINO_PLUG_IN_UPDATE_URL(L"http://www.mycompany.com/support");

// The one and only CMyRhino__1PlugIn object
static class CMyRhino__1PlugIn thePlugIn;

/////////////////////////////////////////////////////////////////////////////
// CMyRhino__1PlugIn definition

CMyRhino__1PlugIn& MyRhino__1PlugIn()
{
	// Return a reference to the one and only CMyRhino__1PlugIn object
	return thePlugIn;
}

CMyRhino__1PlugIn::CMyRhino__1PlugIn()
{
	// Description:
	//   CMyRhino__1PlugIn constructor. The constructor is called when the
	//   plug-in is loaded and "thePlugIn" is constructed. Once the plug-in
	//   is loaded, CMyRhino__1PlugIn::OnLoadPlugIn() is called. The
	//   constructor should be simple and solid. Do anything that might fail in
	//   CMyRhino__1PlugIn::OnLoadPlugIn().

	// TODO: Add construction code here
	m_plugin_version = RhinoPlugInVersion();
#if RDK || TypeRender

	m_pRdkPlugIn = nullptr;
#endif
}

/////////////////////////////////////////////////////////////////////////////
// Required overrides

const wchar_t* CMyRhino__1PlugIn::PlugInName() const
{
	// Description:
	//   Plug-in name display string.  This name is displayed by Rhino when
	//   loading the plug-in, in the plug-in help menu, and in the Rhino
	//   interface for managing plug-ins.

	// TODO: Return a short, friendly name for the plug-in.
	return RhinoPlugInName();
}

const wchar_t* CMyRhino__1PlugIn::PlugInVersion() const
{
	// Description:
	//   Plug-in version display string. This name is displayed by Rhino
	//   when loading the plug-in and in the Rhino interface for managing
	//   plug-ins.

	// TODO: Return the version number of the plug-in.
	return m_plugin_version;
}

GUID CMyRhino__1PlugIn::PlugInID() const
{
	// Description:
	//   Plug-in unique identifier. The identifier is used by Rhino to
	//   manage the plug-ins.

	// TODO: Return a unique identifier for the plug-in.
	// {4FFB7B14-A8B1-4E8B-B431-671248EA137B}
	return ON_UuidFromString(RhinoPlugInId());
}

/////////////////////////////////////////////////////////////////////////////
// Additional overrides

BOOL CMyRhino__1PlugIn::OnLoadPlugIn()
{
	// Description:
	//   Called after the plug-in is loaded and the constructor has been
	//   run. This is a good place to perform any significant initialization,
	//   license checking, and so on.  This function must return TRUE for
	//   the plug-in to continue to load.

	// Remarks:
	//    Plug-ins are not loaded until after Rhino is started and a default document
	//    is created.  Because the default document already exists
	//    CRhinoEventWatcher::On????Document() functions are not called for the default
	//    document.  If you need to do any document initialization/synchronization then
	//    override this function and do it here.  It is not necessary to call
	//    CPlugIn::OnLoadPlugIn() from your derived class.
#if TypeRender

	// If this assert fires, it's likely that the RDK has not yet been loaded by Rhino.
	// This can happen if you load your plug-in first, before the debug RDK and for some
	// reason it actually manages to find rdk.rhp on the search path. If this happens,
	// load protect your plug-in, restart Rhino and and load rdk.rhp using drag and drop
	// or the plug-in manager. Then un-load protect your plug-in.
	ASSERT(RhRdkIsAvailable());

	// TODO: Add render plug-in initialization code here.

	m_pRdkPlugIn = new CMyRhino__1RdkPlugIn;
	ON_wString str;
	if (!m_pRdkPlugIn->Initialize())
	{
		delete m_pRdkPlugIn;
		m_pRdkPlugIn = nullptr;
		str.Format(L"Failed to load %s, version %s. RDK initialization failed\n", PlugInName(), PlugInVersion());
		RhinoApp().Print(str);
		return FALSE;
	}

	str.Format(L"Loading %s, version %s\n", PlugInName(), PlugInVersion());
	RhinoApp().Print(str);
#elif TypeDigitizer
	// NOTE: DO NOT enable your digitizer here!
#elif TypeRender
	// TODO: Add render plug-in initialization code here.

	str.Format(L"Loading %s, version %s\n", PlugInName(), PlugInVersion());
	RhinoApp().Print(str);

	m_event_watcher.Register();
	m_event_watcher.Enable(TRUE);

	CRhinoDoc* doc = RhinoApp().ActiveDoc();
	if (doc)
		m_event_watcher.OnNewDocument(*doc);
#else
	// TODO: Add plug-in initialization code here.
#endif

	return TRUE;
}

void CMyRhino__1PlugIn::OnUnloadPlugIn()
{
	// Description:
	//    Called one time when plug-in is about to be unloaded. By this time,
	//    Rhino's mainframe window has been destroyed, and some of the SDK
	//    managers have been deleted. There is also no active document or active
	//    view at this time. Thus, you should only be manipulating your own objects.
	//    or tools here.

#if TypeRender
	// TODO: Add render plug-in clean-up code here.
	m_event_watcher.Enable(FALSE);
	m_event_watcher.UnRegister();
#elif PLUGIN_TYPE_RENDER
	if (nullptr != m_pRdkPlugIn)
	{
		m_pRdkPlugIn->Uninitialize();
		delete m_pRdkPlugIn;
		m_pRdkPlugIn = nullptr;
	}
#else
	// TODO: Add plug-in cleanup code here.
#endif
}
#if TypeDigitizer

/////////////////////////////////////////////////////////////////////////////
// Digitizer overrides

bool CMyRhino__1PlugIn::EnableDigitizer(bool bEnable)
{
	// Description:
	//   Called by Rhino to enable/disable input from the digitizer.
	//   If bEnable is true and EnableDigitizer() returns false,
	//   then Rhino will not calibrate the digitizer.

	if (bEnable)
	{
		// TODO: Set up your digitizer.
		// Use CRhinoPlugIn::SendPoint() to stream points to Rhino.
	}
	else
	{
		// TODO: Stop using CRhinoPlugIn::SendPoint().
		// Shut down the connection with the digitizer.
	}
	return true;
}

ON::LengthUnitSystem CMyRhino__1PlugIn::UnitSystem() const
{
	// TODO: Return the digitizer's unit system.
	return ON::LengthUnitSystem::Millimeters;
}

double CMyRhino__1PlugIn::PointTolerance() const
{
	// Description:
	//   Precision of the digitizer in the unit system
	//   returned by CMyRhino__1PlugIn::UnitSystem().
	//   If this number is too small, digitizer noise will
	//   cause GetPoint() to jitter.

	// TODO: Return the digitizer's point tolerance.
	return 0.01;
}

#endif
#if TypeExport

/////////////////////////////////////////////////////////////////////////////
// File export overrides

void CMyRhino__1PlugIn::AddFileType(ON_ClassArray<CRhinoFileType>& extensions, const CRhinoFileWriteOptions& options)
{
	UNREFERENCED_PARAMETER(extensions);
	UNREFERENCED_PARAMETER(options);

	// Description:
	//   When Rhino gets ready to display either the save or export file dialog,
	//   it calls AddFileType() once for each loaded file export plug-in.
	// Parameters:
	//   extensions [in] Append your supported file type extensions to this list.
	//   options [in] File write options.
	// Example:
	//   If your plug-in exports "Geometry Files" that have a ".geo" extension,
	//   then your AddToFileType(....) would look like the following:
	//
	//   CExportPlugIn::AddToFileType(ON_ClassArray<CRhinoFileType>&  extensions, const CRhinoFileWriteOptions& options)
	//   {
	//      CRhinoFileType ft(PlugInID(), L"Geometry Files (*.geo)", L"geo");
	//      extensions.Append(ft);
	//   }

    CRhinoFileType ft(PlugInID(), L"MyFileDescription (*.myext)", L"myext");
    extensions.Append(ft);

	// TODO: Add supported file extensions here.
}

BOOL CMyRhino__1PlugIn::WriteFile(const wchar_t* filename, int index, CRhinoDoc& doc, const CRhinoFileWriteOptions& options)
{
	UNREFERENCED_PARAMETER(filename);
	UNREFERENCED_PARAMETER(index);
	UNREFERENCED_PARAMETER(doc);
	UNREFERENCED_PARAMETER(options);

	// Description:
	//   Rhino calls WriteFile() to write document geometry to an external file.
	// Parameters:
	//   filename [in] The name of file to write.
	//   index [in] The index of file extension added to list in AddToFileType().
	//   doc [in] The current Rhino document.
	//   options [in] File write options.
	// Remarks:
	//   The plug-in is responsible for opening the file and writing to it.
	// Return TRUE if successful, otherwise return FALSE.

	// TODO: Add file export code here.
	return TRUE;
}
#endif
#if TypeImport

/////////////////////////////////////////////////////////////////////////////
// File import overrides

void CMyRhino__1PlugIn::AddFileType(ON_ClassArray<CRhinoFileType>& extensions, const CRhinoFileReadOptions& options)
{
	UNREFERENCED_PARAMETER(extensions);
	UNREFERENCED_PARAMETER(options);

	// Description:
	//   When Rhino gets ready to display either the open or import file dialog,
	//   it calls AddFileType() once for each loaded file import plug-in.
	// Parameters:
	//   extensions [in] Append your supported file type extensions to this list.
	//   options [in] File write options.
	// Example:
	//   If your plug-in imports "Geometry Files" that have a ".geo" extension,
	//   then your AddToFileType(....) would look like the following:
	//
	//   CImportPlugIn::AddToFileType(ON_ClassArray<CRhinoFileType>& extensions, const CRhinoFileReadOptions& options)
	//   {
	//      CRhinoFileType ft(PlugInID(), L"Geometry Files (*.geo)", L"geo");
	//      extensions.Append(ft);
	//   }

    CRhinoFileType ft(PlugInID(), L"MyFileDescription (*.myext)", L"myext");
    extensions.Append(ft);

	// TODO: Add supported file extensions here.
}

BOOL CMyRhino__1PlugIn::ReadFile(const wchar_t* filename, int index, CRhinoDoc& doc, const CRhinoFileReadOptions& options)
{
	UNREFERENCED_PARAMETER(filename);
	UNREFERENCED_PARAMETER(index);
	UNREFERENCED_PARAMETER(doc);
	UNREFERENCED_PARAMETER(options);

	// Description:
	//   Rhino calls ReadFile() to create document geometry from an external file.
	// Parameters:
	//   filename [in] The name of file to read.
	//   index [in] The index of file extension added to list in AddToFileType().
	//   doc [in] If importing, then the current Rhino document. Otherwise, an empty Rhino document.
	//   options [in] File read options.
	// Remarks:
	//   The plug-in is responsible for opening the file and writing to it.
	// Return TRUE if successful, otherwise return FALSE.

	// TODO: Add file import code here.
	return TRUE;
}
#endif
#if TypeRender

/////////////////////////////////////////////////////////////////////////////
// Render overrides

CRhinoCommand::result CMyRhino__1PlugIn::Render(const CRhinoCommandContext& context, bool bPreview)
{
	// Description:
	//   Called by the Render and RenderPreview commands if this application is both
	//   a Render plug-in and is set as the default render engine.
	// Parameters:
	//   context [in] Command parameters passed to the render command.
	//   bPreview [in] If true, a faster, lower quality rendering is expected.

	const auto pDoc = context.Document();
	if (nullptr == pDoc)
		return CRhinoCommand::failure; 

	CMyRhino__1SdkRender render(context, *this, L"[MyRhino.1", IDI_RENDER, bPreview);
	const auto size = render.RenderSize(*pDoc, true);
	if (CRhinoSdkRender::render_ok != render.Render(size))
		return CRhinoCommand::failure;

	return CRhinoCommand::success;
}

CRhinoCommand::result CMyRhino__1PlugIn::RenderWindow(
	const CRhinoCommandContext& context,
	bool render_preview,
	CRhinoView* view,
	const LPRECT rect,
	bool bInWindow,
	bool bBlowUp
)
{
	UNREFERENCED_PARAMETER(bBlowUp);

	// Description:
	//   Called by Render command if this application is both
	//   a Render plug-in and is set as the default render engine.
	// Parameters:
	//   context [in] Command parameters passed to the render command.
	//   bPreview [in] If true, a faster, lower quality rendering is expected.
	//   view [in] View to render.
	//   rect [in] Rectangle to render in viewport window coordinates.

	CMyRhino__1SdkRender render(context, *this, L"MyRhino.1", IDI_RENDER, render_preview);
	if (CRhinoSdkRender::render_ok == render.RenderWindow(view, rect, bInWindow))
		return CRhinoCommand::success;

	return CRhinoCommand::failure;
}

CRhinoCommand::result CMyRhino__1PlugIn::RenderQuiet(const CRhinoCommandContext& context, bool bPreview)
{
	UNREFERENCED_PARAMETER(context);
	UNREFERENCED_PARAMETER(bPreview);

	return CRhinoCommand::failure;
}

BOOL CMyRhino__1PlugIn::SaveRenderedImage(ON_wString filename)
{
	// Description:
	//   Message sent from a script to save the rendering to a file.
	// Parameters:
	//   filename [in] The name of file to save.

	// TODO: Add file saving code here.
	return FALSE;
}

BOOL CMyRhino__1PlugIn::CloseRenderWindow()
{
	// Description:
	//   Close render window notification. Called when rendering is done and render window
	//   is no longer modal. (When RenderCommand returns if you leave the window up)

	return FALSE;
}

// Render methods

UINT CMyRhino__1PlugIn::MainFrameResourceID() const
{
	return IDR_RENDER;
}

BOOL CMyRhino__1PlugIn::SceneChanged() const
{
	return m_event_watcher.RenderSceneModified();
}

void CMyRhino__1PlugIn::SetSceneChanged(BOOL bChanged)
{
	m_event_watcher.SetRenderMeshFlags(bChanged);
	m_event_watcher.SetMaterialFlags(bChanged);
}

BOOL CMyRhino__1PlugIn::LightingChanged() const
{
	return m_event_watcher.RenderLightingModified();
}

void CMyRhino__1PlugIn::SetLightingChanged(BOOL bChanged)
{
	m_event_watcher.SetLightFlags(bChanged);
}
#endif



