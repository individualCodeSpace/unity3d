/******************************************************************************************************************************************************
 江西联创精密机电有限公司对本代码拥有全部权限，未经许可不得引用本代码任何内容，也不得用于非江西联创精密机电有限公司之外的任何商业和非商业项目。

 项目编号：
 软件名称：

 开发环境：
 运行环境：
 功能描述：保存文件时打开文件夹选择保存路径； 打开文件时选择指定路径下目标文件；

 原作者：邓通福
 完成日期：
 特别说明：

 修改者：
 完成日期：
 修改记录：
 如经过多次修改，则继续往下添加。
 **********************************************************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
//自动识别托管结构（文件路径）的长度
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

/// <summary>
/// 打开文件夹
/// </summary>
public class OpenFileName
{
    public int StructSize = 0;
    public IntPtr DlgOwner = IntPtr.Zero;
    public IntPtr Instance = IntPtr.Zero;
    public String Filter = null;
    public String CustomFilter = null;
    public int MaxCustFilter = 0;
    public int FilterIndex = 0;
    public String File = null;
    public int MaxFile = 0;
    public String FileTitle = null;
    public int MaxFileTitle = 0;
    public String InitialDir = null;
    public String Title = null;
    public int Flags = 0;
    public short FileOffset = 0;
    public short FileExtension = 0;
    public String DefExt = null;
    public IntPtr CustData = IntPtr.Zero;
    public IntPtr Hook = IntPtr.Zero;
    public String TemplateName = null;
    public IntPtr ReservedPtr = IntPtr.Zero;
    public int ReservedInt = 0;
    public int FlagsEx = 0;
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenDialogFile
{
    public int StructSize = 0;
    public IntPtr DlgOwner = IntPtr.Zero;
    public IntPtr Instance = IntPtr.Zero;
    public String Filter = null;
    public String CustomFilter = null;
    public int MaxCustFilter = 0;
    public int FilterIndex = 0;
    public String File = null;
    public int MaxFile = 0;
    public String FileTitle = null;
    public int MaxFileTitle = 0;
    public String InitialDir = null;
    public String Title = null;
    public int Flags = 0;
    public short FileOffset = 0;
    public short FileExtension = 0;
    public String DefExt = null;
    public IntPtr CustData = IntPtr.Zero;
    public IntPtr Hook = IntPtr.Zero;
    public String TemplateName = null;
    public IntPtr ReservedPtr = IntPtr.Zero;
    public int ReservedInt = 0;
    public int FlagsEx = 0;
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenDialogDir
{
    public IntPtr hwndOwner = IntPtr.Zero;
    public IntPtr pidlRoot = IntPtr.Zero;
    public string pszDisplayName = null;
    public string lpszTitle = null;
    public UInt32 ulFlags = 0;
    public IntPtr lpfn = IntPtr.Zero;
    public IntPtr lPzram = IntPtr.Zero;
    public int iImage = 0;
}
public class WindowDll
{
    #region Window
    //链接指定系统函数  打开文件对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
   
    //另存为对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
   
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenDialogFile ofn);
    
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenDialogFile ofn);
   
    [DllImport("Shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

    [DllImport("Shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);
    #endregion
    public const string IMAGEFILTER = "图片文件(*.jpg;*.png)\0*.jpg;*.png";
    public const string ALLFILTER = "所有文件(*.*)\0*.*";
    /// <summary>
    /// 选择文件
    /// </summary>
    /// <param name="callback">返回选择文件夹的路径</param>
    /// <param name="filter">文件类型筛选器</param>
    public static  void SelectFile(Action<string> callback, string filter = ALLFILTER)
    {
        try
        {

            OpenFileName ofn = new OpenFileName();

            ofn.StructSize = Marshal.SizeOf(ofn);

            ofn.Filter = filter;
            //文件
            ofn.File = new string(new char[256]);

            ofn.MaxFile = ofn.File.Length;
            //标题
            ofn.FileTitle = new string(new char[64]);

            ofn.MaxFileTitle = ofn.FileTitle.Length;
            
            //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  
            ofn.Title = "选择文件";

            //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
            ofn.Flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

            if (GetOpenFileName(ofn))
            {
                string filepath = ofn.File;
                if(File.Exists(filepath))
                {
                    if (callback != null)
                        callback(filepath);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        if (callback != null)
            callback(string.Empty);
    }
    /// <summary>
    /// 调用windowsExploer并返回所选文件夹路径
    /// </summary>
    /// <param name="dialogtitle">打开对话框的标题</param>
    /// <return>所选文件夹路径</return>
    public static string GetPathFromWindowsExplorer(string dialogtitle = "请选择下载路径")
    {
        try
        {
            OpenDialogDir ofn2 = new OpenDialogDir();
            ofn2.pszDisplayName = new string(new char[2048]);
            ;
            ofn2.lpszTitle = dialogtitle;
            ofn2.ulFlags = 0x00000040;
            IntPtr pidlPtr = SHBrowseForFolder(ofn2);
            char[] charArray = new char[2048];
            for(int i = 0; i<2048; i++)
            {
                charArray[i] = '\0';
            }

            SHGetPathFromIDList(pidlPtr, charArray);
            string res = new string(charArray);
            res = res.Substring(0, res.IndexOf('\0'));
            return res;
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        return string.Empty;
    }
    public static void OpenFolder(string path)
    {
        System.Diagnostics.Process.Start("explorer.exe", path);
    }

}
public class OpenFile : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenFileWin(string path)
    {
        ////打开文件选择窗口

        OpenFileName ofn = new OpenFileName();

        ofn.StructSize = Marshal.SizeOf(ofn);

        ofn.Filter = "All Files (*.*)|*.*";
        //文件
        ofn.File = new string(new char[256]);

        ofn.MaxFile = ofn.File.Length;
        //标题
        ofn.FileTitle = new string(new char[64]);

        ofn.MaxFileTitle = ofn.FileTitle.Length;
        //默认路径  
        ofn.InitialDir = path.Replace('/', '\\');
        //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  
        ofn.Title = "选择文件";
        //显示文件的类型
        ofn.DefExt = "xml";
        
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.Flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        if (WindowDll.GetOpenFileName(ofn))
        {

            //判断文件名称
            Debug.Log("Selected file with full path: {0}" + ofn.File);
        }


        //GameObject ob = GameObject.Find(ofn.file);
        //GameObject oo = Instantiate(ob);
        //oo.transform.SetParent(cube.transform);




    }
   

}





