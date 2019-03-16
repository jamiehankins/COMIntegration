
// MFCAppDlg.h : header file
//

#pragma once

// Forward declaration.
class ComClassEventSink;

// CMFCAppDlg dialog
class CMFCAppDlg : public CDialogEx
{
    // Construction
public:
    CMFCAppDlg(CWnd* pParent = nullptr);
    ~CMFCAppDlg();

// Dialog Data
#ifdef AFX_DESIGN_TIME
    enum { IDD = IDD_MFCAPP_DIALOG };
#endif

protected:
    virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
    HICON m_hIcon;

    // Generated message map functions
    virtual BOOL OnInitDialog();
    afx_msg void OnPaint();
    afx_msg HCURSOR OnQueryDragIcon();
    DECLARE_MESSAGE_MAP()

private:
    IComClassPtr m_ptr;
    ComClassEventSink *m_events;
};
