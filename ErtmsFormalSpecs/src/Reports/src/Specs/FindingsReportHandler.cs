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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports.Specs
{
    using MigraDoc.DocumentObjectModel;

    public class FindingsReportHandler : ReportHandler
    {
        public bool addQuestions { set; get; }
        public bool addComments { set; get; }
        public bool addBugs { set; get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary"></param>
        public FindingsReportHandler(DataDictionary.Dictionary dictionary)
            :base(dictionary)
        {
            createFileName("FindingsReport");
            addQuestions = false;
            addComments = false;
            addBugs = false;
        }


        public override Document  BuildDocument()
        {
            Document retVal = new Document();

            Log.Info("Creating findings report");
            retVal.Info.Title = "EFS Subset-076 Findings report";
            retVal.Info.Author = "ERTMS Solutions";
            retVal.Info.Subject = "Subset-076 findings report";

            FindingsReport report = new FindingsReport(retVal);
            if (addBugs)
            {
                Log.Info("..generating bugs");
                report.CreateBugsArticle(this);
            }

            if (addComments)
            {
                Log.Info("..generating remarks");
                report.CreateCommentsArticle(this);
            }
            
            if (addQuestions)
            {
                Log.Info("..generating questions");
                report.CreateQuestionsArticle(this);
            }

            return retVal;
        }

    }
}
