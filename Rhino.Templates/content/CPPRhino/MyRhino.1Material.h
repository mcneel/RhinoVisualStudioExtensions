// MyRhino.1Material.h
//

#pragma once

// CMyRhino__1MaterialFactory
// See MyRhino.1Material.cpp for the implementation of this class.
//

class CMyRhino__1MaterialFactory : public CRhRdkMaterialFactory
{
public:
	CMyRhino__1MaterialFactory() = default;

protected:
	virtual CRhRdkMaterial* NewMaterial(void) const override;
};

// CMyRhino__1Material
// See MyRhino.1Material.cpp for the implementation of this class.
//

class CMyRhino__1Material : public CRhRdkMaterial
{
public:
	CMyRhino__1Material() = default;

	// CRhRdkContent overrides.
	virtual UUID TypeId(void) const override;
	virtual ON_wString TypeName(void) const override;
	virtual ON_wString TypeDescription(void) const override;
	virtual ON_wString InternalName(void) const override;
	virtual void AddUISections(IRhRdkExpandableContentUI& ui) override;
	virtual UUID RenderEngineId(void) const override;
	virtual UUID PlugInId(void) const override;
	virtual void* GetShader(const UUID& uuidRenderEngine, void* pvData) const override;
	virtual bool IsFactoryProductAcceptableAsChild(const CRhRdkContentFactory& factory, const wchar_t* wszChildSlotName) const override;
	virtual unsigned int BitFlags(void) const override;

	// CRhRdkMaterial overrides.
	virtual void SimulateMaterial(ON_Material& matOut, CRhRdkTexture::TextureGeneration tg = CRhRdkTexture::TextureGeneration::Allow,
	                              int iSimulatedTextureSize = -1, const CRhinoObject* pObject = nullptr) const override;
};
