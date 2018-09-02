using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class Win32MessageBox {

    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);
}
