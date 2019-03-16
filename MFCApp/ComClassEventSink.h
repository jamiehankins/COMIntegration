#pragma once

// Forward declaration of the dialog class.
class CMFCAppDlg;

/*
class EngageEventSink : public IDispEventImpl<
    1967,
    EngageEventSink,
    &DIID_IEngageEngineEvents,
    &LIBID_TrecerdoEngageProcessHost, 1, 0>
{*/

class ComClassEventSink : public IDispEventImpl<
    1967,
    ComClassEventSink,
    &DIID_IComClassEvents,
    &LIBID_ComDll, 1, 0>
{
public:
    ComClassEventSink(CMFCAppDlg *parent);
    ~ComClassEventSink();

    BEGIN_SINK_MAP(ComClassEventSink)
        SINK_ENTRY_EX(1967, DIID_IComClassEvents, 1, SomethingWasDone)
        SINK_ENTRY_EX(1967, DIID_IComClassEvents, 2, ParamsWereSent)
    END_SINK_MAP()

    STDMETHODIMP SomethingWasDone(BSTR message);
    STDMETHODIMP ParamsWereSent(ParamStruct *paramArray, int count);

private:
    CMFCAppDlg *m_parent;
};

