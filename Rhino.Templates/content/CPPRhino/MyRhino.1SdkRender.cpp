// MyRhino.1SdkRender.cpp
//

#include "stdafx.h"
#include "MyRhino.1SdkRender.h"
#include "MyRhino.1PlugIn.h"

CMyRhino__1SdkRender::CMyRhino__1SdkRender(
	const CRhinoCommandContext& context,
	CRhinoRenderPlugIn& plugin,
	const ON_wString& sCaption,
	UINT id,
	bool bPreview
	)
	: CRhRdkSdkRender(context, plugin, sCaption, id)
{
	m_bRenderQuick = bPreview;
	m_bCancel = false;
	m_bContinueModal = true;
	m_hRenderThread = NULL;
	GetRenderWindow().ClearChannels();

	// TODO: Add any extra channels before the rendering starts.

	// e.g., This adds a Z_buffer channel which allows things like Fog and DOF post effects to work.
	GetRenderWindow().AddChannel(IRhRdkRenderWindow::chanDistanceFromCamera, sizeof(float));
}

CMyRhino__1SdkRender::~CMyRhino__1SdkRender()
{
	if (m_hRenderThread != NULL)
	{
		::WaitForSingleObject(m_hRenderThread, INFINITE);
		::CloseHandle(m_hRenderThread);
		m_hRenderThread = NULL;
	}
}

CRhinoSdkRender::RenderReturnCodes CMyRhino__1SdkRender::Render(const ON_2iSize& sizeRender)
{
	if (!::RhRdkIsAvailable())
		return CRhinoSdkRender::render_error_starting_render;

	const ON_Viewport& vp = RhinoApp().ActiveView()->ActiveViewport().VP();

	// Force render meshes to be created on the main thread.
	IRhRdkSdkRenderMeshIterator* pIterator = NewRenderMeshIterator(vp);
	pIterator->EnsureRenderMeshesCreated();

	// You can now use this Iterator to get all of the meshes in the scene.
	// While the iterator is around, all meshes are guaranteed to be available
	// which means you don't need to copy them during the rendering process.

	CRhRdkRenderMesh rm;
	pIterator->Reset();
	while (pIterator->Next(rm))
	{
		// TODO: Use the mesh.
	}

	CRhinoSdkRender::RenderReturnCodes rc = CRhRdkSdkRender::Render(sizeRender);

	delete pIterator;

	return rc;
}

CRhinoSdkRender::RenderReturnCodes CMyRhino__1SdkRender::RenderWindow(CRhinoView* pView, const LPRECT pRect, bool bInPopupWindow)
{
	if (!::RhRdkIsAvailable())
		return CRhinoSdkRender::render_error_starting_render;

	CRhinoDoc* pDocument = CommandContext().Document();
	if (nullptr == pDocument)
		return CRhinoSdkRender::render_error_starting_render;

	const auto sizeRender = RenderSize(*pDocument, true);

	const ON_Viewport& vp = pView->ActiveViewport().VP();

	// Force render meshes to be created on the main thread.
	IRhRdkSdkRenderMeshIterator* pIterator = NewRenderMeshIterator(vp);
	pIterator->EnsureRenderMeshesCreated();

	// You can now use this Iterator to get all of the meshes in the scene.
	// While the iterator is around, all meshes are guaranteed to be available
	// which means you don't need to copy them during the rendering process.

	CRhRdkRenderMesh rm;
	pIterator->Reset();
	while (pIterator->Next(rm))
	{
		// TODO: Use the mesh.  This might be the point you create your acceleration structure
		// or, if you are writing a renderer that uses its own mesh representation, you might do the
		// conversion here.
		// One thing to remember - the IRhRdkSdkRenderMeshIterator::Next function is not, at this time,
		// thread safe, so please don't pass the iterator's pointer into multiple render threads and use
		// it to query the mesh list.  In any case, it's not really optimized for in-render access.
	}

	CRhinoSdkRender::RenderReturnCodes rc;

	if (bInPopupWindow)
	{
		// Rendering the specified region in a normal popup window.

		// This method gives roughly a region-sized frame.
		const ON_4iRect rect(pRect->left, pRect->top, pRect->right, pRect->bottom);
		rc = __super::Render(rect.Size());

		// This method gives a normal-sized frame with a region-sized rendered area inside it.
//		rc = CRhRdkSdkRender::Render(sizeRender);
	}
	else
	{
		// Rendering directly into the viewport.
		rc = __super::RenderWindow(pView, pRect, bInPopupWindow);
	}

	delete pIterator;

	return rc;
}

BOOL CMyRhino__1SdkRender::NeedToProcessGeometryTable()
{
	return ::MyRhino__1PlugIn().SceneChanged();
}

BOOL CMyRhino__1SdkRender::NeedToProcessLightTable()
{
	return ::MyRhino__1PlugIn().LightingChanged();
}

BOOL CMyRhino__1SdkRender::RenderPreCreateWindow()
{
	::MyRhino__1PlugIn().SetSceneChanged(FALSE);
	::MyRhino__1PlugIn().SetLightingChanged(FALSE);

	return TRUE;
}

BOOL CMyRhino__1SdkRender::RenderContinueModal()
{
	return m_bContinueModal;
}

void CMyRhino__1SdkRender::SetContinueModal(bool b)
{
	m_bContinueModal = b;
}

void CMyRhino__1SdkRender::RenderThread(void* pv) // Static.
{
	reinterpret_cast<CMyRhino__1SdkRender*>(pv)->ThreadedRender();
}

void CMyRhino__1SdkRender::StartRendering()
{
	m_hRenderThread = ::CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)RenderThread, this, 0, NULL);
}

BOOL CMyRhino__1SdkRender::StartRenderingInWindow(CRhinoView*, const LPCRECT)
{
	m_hRenderThread = ::CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)RenderThread, this, 0, NULL);

	return TRUE;
}

void CMyRhino__1SdkRender::StopRendering()
{
	// If rendering is in progress, cancel it and wait for it to stop.
	if (NULL != m_hRenderThread)
	{
		m_bCancel = true;

		::WaitForSingleObject(m_hRenderThread, INFINITE);
		::CloseHandle(m_hRenderThread);
		m_hRenderThread = NULL;
	}
}

static void CalculatePixel(int x, int y, const ON_2iSize& sizeRender, CRhRdkColor& colOut, float& zOut, bool bFast);

int CMyRhino__1SdkRender::ThreadedRender(void)
{
	// TODO: Replace this with your own rendering code.

	m_bCancel = false;

	CRhinoDoc* pDocument = CommandContext().Document();
	if (nullptr == pDocument)
		return -1;

	const auto sizeRender = RenderSize(*pDocument, true);

	ON_4iRect rect(0, 0, sizeRender.cx, sizeRender.cy);

	IRhRdkRenderWindow& renderWnd = GetRenderWindow();

	// Set the render window size.
	renderWnd.SetSize(sizeRender);

	// Open the RGBA channel. This may be all you need.
	IRhRdkRenderWindow::IChannel* pChanRGBA = renderWnd.OpenChannel(IRhRdkRenderWindow::chanRGBA);
	if (nullptr != pChanRGBA)
	{
		// Optionally open another channel; in this case the Z buffer or distance from camera.
		IRhRdkRenderWindow::IChannel* pChanZ = renderWnd.OpenChannel(IRhRdkRenderWindow::chanDistanceFromCamera);
		if (nullptr != pChanZ)
		{
			float z = 0.0f;
			CRhRdkColor col;

			for (int y = 0; y < sizeRender.cy; y++)
			{
				for (int x = 0; x < sizeRender.cx; x++)
				{
					CalculatePixel(x, y, sizeRender, col, z, m_bRenderQuick);

					if (m_bCancel)
						break;

					pChanRGBA->SetValue(x, y, ComponentOrder::RGBA, col.FloatArray());

					pChanZ->SetValue(x, y, ComponentOrder::Irrelevant, &z);
				}

				rect.top = y;
				rect.bottom = y + 1;
				renderWnd.InvalidateArea(rect);
			}

			pChanZ->Close();
		}

		pChanRGBA->Close();
	}

	// This must be the last thing the thread does.
	SetContinueModal(false);

	return 0;
}

// Fake shading functions for demo purposes.

void CalcCore(int x, int y, float w, float h, double x1, double y1, float& r, float& g, float& b, float& a, float& z)
{
	const float k = min(w, h) * 0.5f, radius = k * 0.8f;
	float rr = 0.05f, gg = 0.25f, bb = 0.6f, aa = 0.0f, zz = 0.0f;
	const float dist = (float)sqrt((double)(x1 * x1) + (double)(y1 * y1));
	if (dist < radius)
	{
		bb = x / w; bb *= bb;
		rr = y / h; rr *= rr;
		zz = 1.0f - (dist / k);
		aa = 1.0f;
		gg = zz * 0.68f;
	}

	r += rr; g += gg; b += bb; a += aa; z += zz;
}

static void CalculatePixel(int x, int y, const ON_2iSize& sizeRender, CRhRdkColor& colOut, float& zOut, bool bFast)
{
	const float w = (float)sizeRender.cx;
	const float h = (float)sizeRender.cy;

	const double midX = w * 0.5f;
	const double midY = h * 0.5f;

	int samples = 0;

	float r = 0.0f, g = 0.0f, b = 0.0f, a = 0.0f, z = 0.0f;

	if (bFast)
	{
		const double x1 = midX - x;
		const double y1 = midY - y;
		CalcCore(x, y, w, h, x1, y1, r, g, b, a, z);
		samples++;
	}
	else
	for (float s = -0.5f; s < 0.5001f; s += 0.0625f)
	{
		for (float t = -0.5f; t < 0.5001f; t += 0.0625f)
		{
			const double x1 = midX - x + s;
			const double y1 = midY - y + t;
			CalcCore(x, y, w, h, x1, y1, r, g, b, a, z);
			samples++;
		}
	}

	float* f = colOut.FloatArray();
	f[0] = r / samples;
	f[1] = g / samples;
	f[2] = b / samples;
	f[3] = a / samples;

	zOut = z;
}
