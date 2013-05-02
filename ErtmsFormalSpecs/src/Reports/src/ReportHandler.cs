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
using System.Diagnostics;
using System.IO;
using DataDictionary;

namespace Report
{
    /// <summary>
    /// Class contain the base information for report configs
    /// (Name of the report, the path of the generated .pdf and
    /// the dictionary)
    /// </summary>
    public abstract class ReportHandler : Utils.ProgressHandler
    {
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates the full file name from a given title
        /// </summary>
        /// <param name="fileName">Name of the report</param>
        protected void createFileName(string title)
        {
            string reportPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string dateString = String.Format("{0:D4}-{1:D2}-{2:D2}",
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    DateTime.Now.Day);

            FileName = Path.Combine(reportPath, dateString + "_" + title + ".pdf");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportHandler(Dictionary dictionary)
        {
            Name = "Report";
            createFileName("Report");
            Dictionary = dictionary;
        }

        /// <summary>
        /// Name of the report
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// The name and the path of the final .pdf document
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// The dictionary representing the model
        /// </summary>
        public Dictionary Dictionary { set; get; }

        /// The system for which the report should be created
        public virtual EFSSystem EFSSystem { get { return Dictionary.EFSSystem; } }

        /// <summary>
        /// Displays the generated report
        /// </summary>
        public void displayReport()
        {
            Process.Start(FileName);
        }
    }
}

