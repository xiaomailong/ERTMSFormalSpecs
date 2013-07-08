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
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;


namespace Reports.ERTMSAcademy
{
    public class ERTMSAcademyReport : ReportTools
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="document"></param>
        public ERTMSAcademyReport(Document document)
            : base(document)
        {
        }


        public void Fill(string path, string pattern)
        {
            string[] lines = File.ReadAllLines(path);
            string author = "", date = "", comment = "", stats = "";
            bool addEntry = false;
            foreach (string line in lines)
            {
                if (line.StartsWith("Author")) /* Name of the author */
                {
                    if (date != "" && comment != "" && stats != "")
                    {
                        AddTable(new string[] { "Added on " + date }, new int[] { 20, 120 });
                        AddRow("Author",     author );
                        AddRow("Comment",    comment);
                        AddRow("Statistics", stats  );
                        AddParagraph("");
                    }
                    /* This is a new entry => all the previous information is deleted */
                    author = line.Remove(0, 7); /* Removing "Author " */
                    date = "";
                    comment = "";
                    stats = "";
                    addEntry = false;
                }
                else if (line.StartsWith("Date")) /* Date of the commit */
                {
                    date = line.Remove(0, 5); /* Removing "Date " */
                }
                else if (line.Contains(pattern)) /* Commit message */
                {
                    /* If the commit line contains the pattern, the entry will be added */
                    comment = line;
                    addEntry = true;
                }
                else if (!line.Equals("")) /* Statistics */
                {
                    if (addEntry)
                    {
                        stats += line + '\n';
                    }
                    else /* This entry will not be added => previous information is deleted */
                    {
                        stats = "";
                    }
                }
            }

            /* Adding the last entry */
            if (date != "" && comment != "" && stats != "")
            {
                AddTable(new string[] { "Added on " + date }, new int[] { 20, 120 });
                AddRow("Author",     author );
                AddRow("Comment",    comment);
                AddRow("Statistics", stats  );
            }
        }
    }
}
