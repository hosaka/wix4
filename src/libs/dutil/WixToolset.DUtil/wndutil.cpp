// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

#include "precomp.h"


// Exit macros
#define WnduExitOnLastError(x, s, ...) ExitOnLastErrorSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitOnLastErrorDebugTrace(x, s, ...) ExitOnLastErrorDebugTraceSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitWithLastError(x, s, ...) ExitWithLastErrorSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitOnFailure(x, s, ...) ExitOnFailureSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitOnRootFailure(x, s, ...) ExitOnRootFailureSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitWithRootFailure(x, e, s, ...) ExitWithRootFailureSource(DUTIL_SOURCE_WNDUTIL, x, e, s, __VA_ARGS__)
#define WnduExitOnFailureDebugTrace(x, s, ...) ExitOnFailureDebugTraceSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitOnNull(p, x, e, s, ...) ExitOnNullSource(DUTIL_SOURCE_WNDUTIL, p, x, e, s, __VA_ARGS__)
#define WnduExitOnNullWithLastError(p, x, s, ...) ExitOnNullWithLastErrorSource(DUTIL_SOURCE_WNDUTIL, p, x, s, __VA_ARGS__)
#define WnduExitOnNullDebugTrace(p, x, e, s, ...)  ExitOnNullDebugTraceSource(DUTIL_SOURCE_WNDUTIL, p, x, e, s, __VA_ARGS__)
#define WnduExitOnInvalidHandleWithLastError(p, x, s, ...) ExitOnInvalidHandleWithLastErrorSource(DUTIL_SOURCE_WNDUTIL, p, x, s, __VA_ARGS__)
#define WnduExitOnWin32Error(e, x, s, ...) ExitOnWin32ErrorSource(DUTIL_SOURCE_WNDUTIL, e, x, s, __VA_ARGS__)
#define WnduExitOnOptionalXmlQueryFailure(x, b, s, ...) ExitOnOptionalXmlQueryFailureSource(DUTIL_SOURCE_WNDUTIL, x, b, s, __VA_ARGS__)
#define WnduExitOnRequiredXmlQueryFailure(x, s, ...) ExitOnRequiredXmlQueryFailureSource(DUTIL_SOURCE_WNDUTIL, x, s, __VA_ARGS__)
#define WnduExitOnGdipFailure(g, x, s, ...) ExitOnGdipFailureSource(DUTIL_SOURCE_WNDUTIL, g, x, s, __VA_ARGS__)

struct MEMBUFFER_FOR_RICHEDIT
{
    BYTE* rgbData;
    DWORD cbData;

    DWORD iData;
};

const DWORD GROW_WINDOW_TEXT = 250;


// prototypes
static DWORD CALLBACK RichEditStreamFromFileHandleCallback(
    __in DWORD_PTR dwCookie,
    __in_bcount(cb) LPBYTE pbBuff,
    __in LONG cb,
    __in LONG *pcb
    );
static DWORD CALLBACK RichEditStreamFromMemoryCallback(
    __in DWORD_PTR dwCookie,
    __in_bcount(cb) LPBYTE pbBuff,
    __in LONG cb,
    __in LONG *pcb
    );


DAPI_(HRESULT) WnduLoadRichEditFromFile(
    __in HWND hWnd,
    __in_z LPCWSTR wzFileName,
    __in HMODULE hModule
    )
{
    HRESULT hr = S_OK;
    LPWSTR sczFile = NULL;
    HANDLE hFile = INVALID_HANDLE_VALUE;

    hr = PathRelativeToModule(&sczFile, wzFileName, hModule);
    WnduExitOnFailure(hr, "Failed to read resource data.");

    hFile = ::CreateFileW(sczFile, GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, NULL);
    if (INVALID_HANDLE_VALUE == hFile)
    {
        WnduExitWithLastError(hr, "Failed to open RTF file.");
    }
    else
    {
        LONGLONG llRtfSize;
        hr = FileSizeByHandle(hFile, &llRtfSize);
        if (SUCCEEDED(hr))
        {
            ::SendMessageW(hWnd, EM_EXLIMITTEXT, 0, static_cast<LPARAM>(llRtfSize));
        }

        EDITSTREAM es = { };
        es.pfnCallback = RichEditStreamFromFileHandleCallback;
        es.dwCookie = reinterpret_cast<DWORD_PTR>(hFile);

        ::SendMessageW(hWnd, EM_STREAMIN, SF_RTF, reinterpret_cast<LPARAM>(&es));
        hr = es.dwError;
        WnduExitOnFailure(hr, "Failed to update RTF stream.");
    }

LExit:
    ReleaseStr(sczFile);
    ReleaseFile(hFile);

    return hr;
}

DAPI_(HRESULT) WnduLoadRichEditFromResource(
    __in HWND hWnd,
    __in_z LPCSTR szResourceName,
    __in HMODULE hModule
    )
{
    HRESULT hr = S_OK;
    MEMBUFFER_FOR_RICHEDIT buffer = { };
    EDITSTREAM es = { };

    hr = ResReadData(hModule, szResourceName, reinterpret_cast<LPVOID*>(&buffer.rgbData), &buffer.cbData);
    WnduExitOnFailure(hr, "Failed to read resource data.");

    es.pfnCallback = RichEditStreamFromMemoryCallback;
    es.dwCookie = reinterpret_cast<DWORD_PTR>(&buffer);

    ::SendMessageW(hWnd, EM_STREAMIN, SF_RTF, reinterpret_cast<LPARAM>(&es));
    hr = es.dwError;
    WnduExitOnFailure(hr, "Failed to update RTF stream.");

LExit:
    return hr;
}


DAPI_(HRESULT) WnduGetControlText(
    __in HWND hWnd,
    __inout_z LPWSTR* psczText
    )
{
    HRESULT hr = S_OK;
    SIZE_T cbSize = 0;
    DWORD cchText = 0;
    DWORD cchTextRead = 0;

    // Ensure the string has room for at least one character.
    hr = StrMaxLength(*psczText, &cbSize);
    WnduExitOnFailure(hr, "Failed to get text buffer length.");

    cchText = (DWORD)min(DWORD_MAX, cbSize);

    if (!cchText)
    {
        cchText = GROW_WINDOW_TEXT;

        hr = StrAlloc(psczText, cchText);
        WnduExitOnFailure(hr, "Failed to grow text buffer.");
    }

    // Read (and keep growing buffer) until we finally read less than there
    // is room in the buffer.
    for (;;)
    {
        cchTextRead = ::GetWindowTextW(hWnd, *psczText, cchText);
        if (cchTextRead + 1 < cchText)
        {
            break;
        }
        else
        {
            cchText = cchTextRead + GROW_WINDOW_TEXT;

            hr = StrAlloc(psczText, cchText);
            WnduExitOnFailure(hr, "Failed to grow text buffer again.");
        }
    }

LExit:
    return hr;
}


DAPI_(HRESULT) WnduShowOpenFileDialog(
    __in_opt HWND hwndParent,
    __in BOOL fForcePathExists,
    __in BOOL fForceFileExists,
    __in_opt LPCWSTR wzTitle,
    __in COMDLG_FILTERSPEC* rgFilters,
    __in DWORD cFilters,
    __in DWORD dwDefaultFilter,
    __in_opt LPCWSTR wzDefaultPath,
    __inout LPWSTR* psczPath
    )
{
    HRESULT hr = S_OK;
    IFileOpenDialog* pFileOpenDialog = NULL;
    FILEOPENDIALOGOPTIONS fos = 0;
    FILEOPENDIALOGOPTIONS additionalFlags = FOS_NOCHANGEDIR;
    IShellItem* psi = NULL;
    LPWSTR sczDefaultDirectory = NULL;
    LPWSTR sczPath = NULL;

    hr = CoCreateInstance(CLSID_FileOpenDialog, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pFileOpenDialog));
    WnduExitOnFailure(hr, "Failed to create FileOpenDialog object.");

    hr = pFileOpenDialog->SetTitle(wzTitle);
    WnduExitOnFailure(hr, "Failed to set FileOpenDialog title.");

    hr = pFileOpenDialog->GetOptions(&fos);
    WnduExitOnFailure(hr, "Failed to get FileOpenDialog options.");

    if (fForcePathExists)
    {
        additionalFlags |= FOS_PATHMUSTEXIST;
    }

    if (fForceFileExists)
    {
        additionalFlags |= FOS_FILEMUSTEXIST;
    }

    hr = pFileOpenDialog->SetOptions(fos | additionalFlags);
    WnduExitOnFailure(hr, "Failed to set FileOpenDialog options.");

    hr = pFileOpenDialog->SetFileTypes(cFilters, rgFilters);
    WnduExitOnFailure(hr, "Failed to set FileOpenDialog file types.");

    hr = pFileOpenDialog->SetFileTypeIndex(dwDefaultFilter);
    WnduExitOnFailure(hr, "Failed to set FileOpenDialog default file type.");

    if (wzDefaultPath && *wzDefaultPath)
    {
        hr = PathGetDirectory(wzDefaultPath, &sczDefaultDirectory);
        if (SUCCEEDED(hr))
        {
            hr = ::SHCreateItemFromParsingName(sczDefaultDirectory, NULL, IID_PPV_ARGS(&psi));
            if (SUCCEEDED(hr))
            {
                hr = pFileOpenDialog->SetFolder(psi);
                WnduExitOnFailure(hr, "Failed to set FileOpenDialog folder.");
            }

            ReleaseNullObject(psi);
        }

        hr = pFileOpenDialog->SetFileName(wzDefaultPath);
        WnduExitOnFailure(hr, "Failed to set FileOpenDialog file name.");
    }

    hr = pFileOpenDialog->Show(hwndParent);
    if (HRESULT_FROM_WIN32(ERROR_CANCELLED) == hr)
    {
        ExitFunction();
    }
    WnduExitOnFailure(hr, "Failed to show FileOpenDialog for file.");

    hr = pFileOpenDialog->GetResult(&psi);
    WnduExitOnFailure(hr, "Failed to get FileOpenDialog result.");

    hr = psi->GetDisplayName(SIGDN_FILESYSPATH, &sczPath);
    WnduExitOnFailure(hr, "Failed to get FileOpenDialog result path.");

    hr = StrAllocString(psczPath, sczPath, 0);
    WnduExitOnFailure(hr, "Failed to copy FileOpenDialog result path.");

LExit:
    if (sczPath)
    {
        ::CoTaskMemFree(sczPath);
    }

    ReleaseStr(sczDefaultDirectory);
    ReleaseObject(psi);
    ReleaseObject(pFileOpenDialog);

    return hr;
}


DAPI_(HRESULT) WnduShowOpenFolderDialog(
    __in_opt HWND hwndParent,
    __in BOOL fForceFileSystem,
    __in_opt LPCWSTR wzTitle,
    __inout LPWSTR* psczPath
    )
{
    HRESULT hr = S_OK;
    IFileOpenDialog* pFileOpenDialog = NULL;
    FILEOPENDIALOGOPTIONS fos = 0;
    FILEOPENDIALOGOPTIONS additionalFlags = FOS_PICKFOLDERS | FOS_NOCHANGEDIR;
    IShellItem* psi = NULL;
    LPWSTR sczPath = NULL;

    hr = CoCreateInstance(CLSID_FileOpenDialog, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pFileOpenDialog));
    WnduExitOnFailure(hr, "Failed to create FileOpenDialog object.");

    hr = pFileOpenDialog->SetTitle(wzTitle);
    WnduExitOnFailure(hr, "Failed to set FileOpenDialog title.");

    hr = pFileOpenDialog->GetOptions(&fos);
    WnduExitOnFailure(hr, "Failed to get FileOpenDialog options.");

    if (fForceFileSystem)
    {
        additionalFlags |= FOS_FORCEFILESYSTEM;
    }

    hr = pFileOpenDialog->SetOptions(fos | additionalFlags);
    WnduExitOnFailure(hr, "Failed to set FileOpenDialog options.");

    hr = pFileOpenDialog->Show(hwndParent);
    if (HRESULT_FROM_WIN32(ERROR_CANCELLED) == hr)
    {
        ExitFunction();
    }
    WnduExitOnFailure(hr, "Failed to show FileOpenDialog for folder.");

    hr = pFileOpenDialog->GetResult(&psi);
    WnduExitOnFailure(hr, "Failed to get FileOpenDialog result.");

    hr = psi->GetDisplayName(SIGDN_FILESYSPATH, &sczPath);
    WnduExitOnFailure(hr, "Failed to get FileOpenDialog result path.");

    hr = StrAllocString(psczPath, sczPath, 0);
    WnduExitOnFailure(hr, "Failed to copy FileOpenDialog result path.");

LExit:
    if (sczPath)
    {
        ::CoTaskMemFree(sczPath);
    }

    ReleaseObject(psi);
    ReleaseObject(pFileOpenDialog);

    return hr;
}


static DWORD CALLBACK RichEditStreamFromFileHandleCallback(
    __in DWORD_PTR dwCookie,
    __in_bcount(cb) LPBYTE pbBuff,
    __in LONG cb,
    __in LONG* pcb
    )
{
    HRESULT hr = S_OK;
    HANDLE hFile = reinterpret_cast<HANDLE>(dwCookie);

    if (!::ReadFile(hFile, pbBuff, cb, reinterpret_cast<DWORD*>(pcb), NULL))
    {
        WnduExitWithLastError(hr, "Failed to read file");
    }

LExit:
    return hr;
}


static DWORD CALLBACK RichEditStreamFromMemoryCallback(
    __in DWORD_PTR dwCookie,
    __in_bcount(cb) LPBYTE pbBuff,
    __in LONG cb,
    __in LONG* pcb
    )
{
    HRESULT hr = S_OK;
    MEMBUFFER_FOR_RICHEDIT* pBuffer = reinterpret_cast<MEMBUFFER_FOR_RICHEDIT*>(dwCookie);
    DWORD cbCopy = 0;

    if (pBuffer->iData < pBuffer->cbData)
    {
        cbCopy = min(static_cast<DWORD>(cb), pBuffer->cbData - pBuffer->iData);
        memcpy(pbBuff, pBuffer->rgbData + pBuffer->iData, cbCopy);

        pBuffer->iData += cbCopy;
        Assert(pBuffer->iData <= pBuffer->cbData);
    }

    *pcb = cbCopy;
    return hr;
}
