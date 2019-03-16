#include "stdafx.h"
#include "MFCAppDlg.h"
#include "ComClassEventSink.h"


ComClassEventSink::ComClassEventSink(CMFCAppDlg *parent)
{
    m_parent = parent;
}


ComClassEventSink::~ComClassEventSink()
{
}

STDMETHODIMP ComClassEventSink::SomethingWasDone(BSTR something)
{
    HRESULT hr = S_OK;

    return hr;
}

STDMETHODIMP ComClassEventSink::ParamsWereSent(ParamStruct *paramArray, int count)
{
    HRESULT hr = S_OK;


    return hr;
}

