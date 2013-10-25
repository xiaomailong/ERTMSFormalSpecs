// ------------------------------------------------------------------------------
// -- Copyright ERTMS Solutions
// -- Licensed under the EUPL V.1.1
// -- http://joinup.ec.europa.eu/software/page/eupl/licence-eupl
// --
// -- This file is part of ERTMSFormalSpec software and documentation
// --
// --  ERTMSFormalSpec is free software: you can redistribute it and/or modify
// --  it under the terms of the EUPL General Public License, v.1.1
// --
// -- ERTMSFormalSpec is distributed in the hope that it will be useful,
// -- but WITHOUT ANY WARRANTY; without even the implied warranty of
// -- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// --
// ------------------------------------------------------------------------------
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace ERTMSFormalSpecs
{
    static class ErtmsFormalSpecGui
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                log4net.Config.XmlConfigurator.Configure(new FileInfo("logconfig.xml"));

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings != null && config.AppSettings.Settings != null)
                {
                    foreach (KeyValueConfigurationElement keyValue in config.AppSettings.Settings)
                    {
                        if (keyValue.Key == "LockOpenedFiles")
                        {
                            DataDictionary.Util.PleaseLockFiles = !(keyValue.Value.CompareTo("false") == 0);
                        }
                    }
                }

                GUI.MainWindow window = new GUI.MainWindow();
                Application.Run(window);
            }
            finally
            {
                DataDictionary.Util.UnlockAllFiles();
            }


            DataDictionary.EFSSystem.INSTANCE.Stop();
            GUI.SynchronizerList.Stop();
        }
    }
}
