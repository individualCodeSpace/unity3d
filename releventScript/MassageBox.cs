/******************************************************************************************************************************************************
author:laibaolin。
 **********************************************************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using UnityEngine;


public class MassageBox : MonoBehaviour
{
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);
    public void WindowDisplay()
    {
        MassageBox.print("hello world");
    }
}


