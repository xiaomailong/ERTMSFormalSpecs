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
using MigraDoc.DocumentObjectModel;


namespace Reports.ERTMSAcademy
{
    public class ERTMSAcademyReportHandler : ReportHandler
    {
        public ERTMSAcademyReportHandler(DataDictionary.Dictionary aDictionary)
            :base (aDictionary)
        {
            UserName = "";
            createFileName("ERTMSAcademyReport");
        }

        public string UserName;


        /// <summary>
        /// Creates a report on the model, according to user's choices
        /// </summary>
        /// <returns>The document created, or null</returns>
        public override Document BuildDocument()
        {
            Document retVal = new Document();

            Log.Info("Generating ERTMS Academy report report");
            retVal.Info.Title   = "ERTMS Academy report";
            retVal.Info.Author  = "ERTMS Solutions";
            retVal.Info.Subject = "ERTMS Academy report";

            ERTMSAcademyReport report = new ERTMSAcademyReport(retVal);

            report.AddSubParagraph("Implementation activity by " + UserName);
            report.Fill("ERTMSAcademyReport.txt", "DEV");

            report.AddSubParagraph("Testing activity by " + UserName);
            report.Fill("ERTMSAcademyReport.txt", "Added");

            return retVal;
        }
    }
}
