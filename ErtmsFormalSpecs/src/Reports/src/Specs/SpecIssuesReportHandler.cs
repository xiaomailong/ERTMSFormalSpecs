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
    using DataDictionary;

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
        /// Generates the file in the background thread
        /// </summary>
        /// <param name="arg"></param>
        public override void ExecuteWork()
        {
            ReportBuilder builder = new ReportBuilder(EFSSystem);
            if (!builder.BuildSpecIssuesReport(this))
            {
                Log.ErrorFormat("Report creation failed");
            }
            else
            {
                displayReport();
            }
        }

        public bool AddSpecIssues { set; get; }
        public bool AddDesignChoices { set; get; }
    }
}

