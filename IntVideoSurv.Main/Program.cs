﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CameraViewer.Forms;


namespace CameraViewer
{
    class Program
    {
        public static bool FullScreen;
        public static bool CameraRelay;
        public static string RelayHostIpPort;

        [STAThread]
        static void Main()
        {
            Boolean createdNew; //返回是否赋予了使用线程的互斥体初始所属权
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(Properties.Settings.Default.DefaultSkinName);

            var iniData = new IniParser.FileIniDataParser().LoadFile("Config.ini");

            var ftp = "Ftp";
            FtpService.HostIp = iniData[ftp]["Server"];
            FtpService.UserName = iniData[ftp]["UserName"];
            FtpService.Password = iniData[ftp]["Password"];

            var db = "Database";
            var server = iniData[db]["Server"];
            var user = iniData[db]["UserName"];
            var pwd = iniData[db]["Password"];

            FullScreen = iniData["UI"]["FullScreen"] == "1";

            CameraRelay = iniData["RelayCamera"]["Enabled"] == "1";
            RelayHostIpPort = iniData["RelayCamera"]["Host"];

            System.Threading.Mutex instance = new System.Threading.Mutex(true, "ArresterSerialPort", out createdNew); //同步基元变量
            if (createdNew)
            {


                var conn = DevExpress.Xpo.DB.OracleConnectionProvider.GetConnectionString(server, user, pwd);
                DevExpress.Xpo.Session.DefaultSession.ConnectionString = conn;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                instance.ReleaseMutex();
            }

        }
    }
}
