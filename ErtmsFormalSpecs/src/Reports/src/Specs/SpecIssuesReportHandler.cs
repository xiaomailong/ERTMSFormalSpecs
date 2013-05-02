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
namespace Report.Specs
{
    using MigraDoc.DocumentObjectModel;

    public class SpecIssuesReportHandler : ReportHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary"></param>
        public SpecIssuesReportHandler(DataDictionary.Dictionary dictionary)
            : base(dictionary)
        {
            createFileName("SpecificationIssuesReport");
            AddSpecIssues = false;
            AddDesignChoices = false;
        }

        /// <summary>
        /// Creates a report on specs issues, according to user's choices
        /// </summary>
        /// <returns>The document created, or null</returns>
        public override Document BuildDocument()
        {
            Document retVal = new Document();

            Log.Info("Creating spec issues report");
            retVal.Info.Title = "EFS Specification issues report";
            retVal.Info.Author = "ERTMS Solutions";
            retVal.Info.Subject = "Specification issues report";

            SpecIssuesReport report = new SpecIssuesReport(retVal);
            if (AddSpecIssues)
            {
                Log.Info("..generating spec issues");
                report.CreateSpecIssuesArticle(this);
            }
            if (AddDesignChoices)
            {
                Log.Info("..generating design choices");
                report.CreateDesignChoicesArticle(this);
            }

            return retVal;
        }

        public bool AddSpecIssues { set; get; }
        public bool AddDesignChoices { set; get; }
    }
}

