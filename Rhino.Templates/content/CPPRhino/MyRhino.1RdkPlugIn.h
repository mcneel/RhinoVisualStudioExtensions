// MyRhino.1RdkPlugIn.h
//

#pragma once

#if TypeRender
// CMyRhino__1RdkRenderPlugIn
// See MyRhino.1RdkPlugIn.cpp for the implementation of this class.
//

class CMyRhino__1RdkPlugIn : public CRhRdkRenderPlugIn
#else
// CMyRhino__1CRhRdkPlugIn
// See MyRhino__1RdkPlugIn.cpp for the implementation of this class.
//

class CMyRhino__1RdkPlugIn : public CRhRdkPlugIn
#endif
{
public:
	CMyRhino__1RdkPlugIn() = default;

	virtual bool Initialize() override;
	virtual void Uninitialize() override;

	virtual UUID PlugInId() const override;

#if TypeRender
	virtual void AbortRender() override;
#endif

	CRhinoPlugIn& RhinoPlugIn() const override;

protected:
#if TypeRender
	virtual void RegisterExtensions() const override;

	// Preview renderers
	virtual bool CreatePreview(const ON_2iSize& sizeImage, RhRdkPreviewQuality quality, const IRhRdkPreviewSceneServer* pSceneServer, IRhRdkPreviewCallbacks* pNotify, CRhinoDib& dibOut) override;
	virtual bool CreatePreview(const ON_2iSize& sizeImage, const CRhRdkTexture& texture, CRhinoDib& dibOut) override;

#endif
	virtual bool SupportsFeature(const UUID& uuidFeature) const override;
};
