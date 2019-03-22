using System;
using System.Runtime.InteropServices;

namespace ComDll
{
    [StructLayout(LayoutKind.Sequential)]
    [Guid(ClassId), ComVisible(true)]
    public struct ParamStruct
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Key;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Value;
        [MarshalAs(UnmanagedType.I4)]
        public int Number;

        internal const string ClassId = "8163B82F-E0AD-47BC-8F82-EFA324DCFB95";
    }

    /*
    [StructLayout(LayoutKind.Sequential)]
    [Guid(ClassId), ComVisible(true)]
    public struct ParamArray
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint Count;
        [MarshalAs(UnmanagedType.)]
        public ParamStruct[] Params;

        internal const string ClassId = "8409ECD7-D38C-4B24-91F4-6BBF133226DF";
    }
    */

}
