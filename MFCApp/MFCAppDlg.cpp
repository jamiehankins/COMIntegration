
// MFCAppDlg.cpp : implementation file
//

#include "stdafx.h"
#include "MFCApp.h"
#include "MFCAppDlg.h"
#include "afxdialogex.h"
#include "ComClassEventSink.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CComModule _Module;
extern __declspec(selectany) CAtlModule* _pAtlModule = &_Module;

CString ErrorDescription(HRESULT hr)
{
    TCHAR* szErrMsg;
    CString err;
    if (FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER |
        FORMAT_MESSAGE_FROM_SYSTEM,
        NULL, hr, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR)&szErrMsg, 0, NULL) != 0)
    {
        err = szErrMsg;
        LocalFree(szErrMsg);
    }
    else
    {
        err.Format(_T("Unable to look up message (0x%x)."), hr);
    }
    return err;
}

// CMFCAppDlg dialog
CMFCAppDlg::CMFCAppDlg(CWnd* pParent /*=nullptr*/)
    : CDialogEx(IDD_MFCAPP_DIALOG, pParent)
{
    m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
    m_events = NULL;
}

CMFCAppDlg::~CMFCAppDlg()
{
    if (NULL != m_events)
    {
        m_events->Unadvise(m_ptr);
        m_events->Release();
    }
}

void CMFCAppDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CMFCAppDlg, CDialogEx)
    ON_WM_PAINT()
    ON_WM_QUERYDRAGICON()
END_MESSAGE_MAP()


// CMFCAppDlg message handlers

BOOL CMFCAppDlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();

    // Set the icon for this dialog.  The framework does this automatically
    //  when the application's main window is not a dialog
    SetIcon(m_hIcon, TRUE);			// Set big icon
    SetIcon(m_hIcon, FALSE);		// Set small icon

    // Start up COM.
    CoInitializeEx(0, COINIT_MULTITHREADED);

    // Get our class.
    HRESULT hr = m_ptr.CreateInstance(CLSID_ComClass);
    if (!SUCCEEDED(hr))
    {
        CString err = _T("Failed to create .NET COM object.\r\n");
        err += ErrorDescription(hr);
        MessageBox(err, _T("Error"));
        OnCancel();
    }

    // Connect the event source to our handler.
    m_events = new ComClassEventSink(this);
    hr = m_events->DispEventAdvise(m_ptr, &(m_events->m_iid));
    if (!SUCCEEDED(hr))
    {
        CString err = _T("Failed to register for .NET events.\r\n");
        err += ErrorDescription(hr);
        MessageBox(err, _T("Error"));
        OnCancel();
    }

    m_ptr->DoSomething("Try this");

    return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMFCAppDlg::OnPaint()
{
    if (IsIconic())
    {
        CPaintDC dc(this); // device context for painting

        SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

        // Center icon in client rectangle
        int cxIcon = GetSystemMetrics(SM_CXICON);
        int cyIcon = GetSystemMetrics(SM_CYICON);
        CRect rect;
        GetClientRect(&rect);
        int x = (rect.Width() - cxIcon + 1) / 2;
        int y = (rect.Height() - cyIcon + 1) / 2;

        // Draw the icon
        dc.DrawIcon(x, y, m_hIcon);
    }
    else
    {
        CDialogEx::OnPaint();
    }
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CMFCAppDlg::OnQueryDragIcon()
{
    return static_cast<HCURSOR>(m_hIcon);
}

