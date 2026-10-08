//___FILEHEADER___

#include "___FILEBASENAME___.h"

// Keep these fast: they run on every matching event, often many times per command.

void ___FILEBASENAMEASIDENTIFIER___::OnNewDocument(CRhinoDoc& doc)
{
  UNREFERENCED_PARAMETER(doc);
  // TODO: Called when a new document is created.
}

void ___FILEBASENAMEASIDENTIFIER___::OnCloseDocument(CRhinoDoc& doc)
{
  UNREFERENCED_PARAMETER(doc);
  // TODO: Called when a document is about to be closed.
}

void ___FILEBASENAMEASIDENTIFIER___::OnEndOpenDocument(CRhinoDoc& doc, const wchar_t* filename, BOOL32 bMerge, BOOL32 bReference)
{
  UNREFERENCED_PARAMETER(doc);
  UNREFERENCED_PARAMETER(filename);
  UNREFERENCED_PARAMETER(bMerge);
  UNREFERENCED_PARAMETER(bReference);
  // TODO: Called after a file has been read into a document.
}

void ___FILEBASENAMEASIDENTIFIER___::OnAddObject(CRhinoDoc& doc, CRhinoObject& object)
{
  UNREFERENCED_PARAMETER(doc);
  UNREFERENCED_PARAMETER(object);
  // TODO: Called when an object is added to a document.
}

void ___FILEBASENAMEASIDENTIFIER___::OnDeleteObject(CRhinoDoc& doc, CRhinoObject& object)
{
  UNREFERENCED_PARAMETER(doc);
  UNREFERENCED_PARAMETER(object);
  // TODO: Called when an object is deleted from a document.
}

void ___FILEBASENAMEASIDENTIFIER___::OnReplaceObject(CRhinoDoc& doc, CRhinoObject& old_object, CRhinoObject& new_object)
{
  UNREFERENCED_PARAMETER(doc);
  UNREFERENCED_PARAMETER(old_object);
  UNREFERENCED_PARAMETER(new_object);
  // TODO: Called when an object is replaced, e.g. after it is moved or edited.
  // OnDeleteObject and OnAddObject are also called for the old and new objects.
}
