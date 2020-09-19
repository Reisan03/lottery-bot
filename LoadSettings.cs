using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Config
{
    /// <summary>
    /// default.iniからtokenを取得するやつ
    /// </summary>
    class Settings
    {
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public string Path { get; set; } //取得するやつ
        public void IniFile(string _path) // インスタンス初期化するやつ
        {
            Path = _path;
        }

        public string GetToken() //token取るやつ
        {
            var sb = new StringBuilder(1024);
            var get = GetPrivateProfileString("Data", "token", "Unknown", sb, (uint)sb.Capacity, Path);
            return sb.ToString();
        }
    }
}
