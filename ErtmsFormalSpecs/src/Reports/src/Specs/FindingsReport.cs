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
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;

namespace Reports.Specs
{
    class FindingsReport : ReportTools
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary"></param>
        public FindingsReport(Document document)
            : base(document)
        {
        }

        // For each element in this report, have a Create method that does the paragraphing stuff and a 
        // Generate method, that involves loops and stuff

        /// <summary>
        /// Provides the first-level paragraph under a chapter with the provided name
        /// </summary>
        /// <param name="aSectionName"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private DataDictionary.Specification.Paragraph getSection(string aSectionName, DataDictionary.Dictionary dictionary)
        {
            DataDictionary.Specification.Paragraph retVal = new DataDictionary.Specification.Paragraph();
            foreach (DataDictionary.Specification.Specification specification in dictionary.Specifications)
            {
                foreach (DataDictionary.Specification.Chapter chapter in specification.Chapters)
                {
                    foreach (DataDictionary.Specification.Paragraph paragraph in chapter.Paragraphs)
                    {
                        if (paragraph.ExpressionText == aSectionName)
                        {
                            retVal = paragraph;
                        }
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Creates an article for the questions
        /// </summary>
        /// <param name="aReportConfig"></param>
        public void CreateQuestionsArticle(FindingsReportHandler aReportConfig)
        {
            AddSubParagraph("Questions related to Subset-076");
            AddParagraph("This section contains the " + getSection("Questions", aReportConfig.Dictionary).SubParagraphs.Count +
                " questions raised in translating the Subset-076 test sequences.");
            GenerateQuestions(aReportConfig.Dictionary);
            CloseSubParagraph();
        }

        /// <summary>
        /// Creates an article for the remarks
        /// </summary>
        /// <param name="aReportConfig"></param>
        public void CreateCommentsArticle(FindingsReportHandler aReportConfig)
        {
            AddSubParagraph("Comments for Subset-076");
            AddParagraph("This section contains the " + getSection("Comments", aReportConfig.Dictionary).SubParagraphs.Count +
                " comments for the Subset-076 test sequences. " + "Comments are findings that are minor bugs, that could be assumptions the ERTMS Solutions team is not aware of, or information we feel is worth sharing in the present document.");
            GenerateComments(aReportConfig.Dictionary);
            CloseSubParagraph();
        }

        /// <summary>
        /// Creates an article for the bugs
        /// </summary>
        /// <param name="aReportConfig"></param>
        public void CreateBugsArticle(FindingsReportHandler aReportConfig)
        {
            AddSubParagraph("Bugs for Subset-076");
            AddParagraph("This section contains the " + getSection("Bugs", aReportConfig.Dictionary).SubParagraphs.Count +
                " discovered bugs for the Subset-076 test sequences");
            AddParagraph("These are the findings that are errors in the test sequences. They prevent execution of the test sequence.");
            GenerateBugs(aReportConfig.Dictionary);
            CloseSubParagraph();
        }


        /// <summary>
        /// Creates a table for the questions
        /// </summary>
        /// <param name="aDictionary"></param>
        public void GenerateQuestions(DataDictionary.Dictionary aDictionary)
        {
            AddSubParagraph("");
            DataDictionary.Specification.Paragraph questions = getSection("Questions", aDictionary);
            foreach (DataDictionary.Specification.Paragraph subparagraph in questions.SubParagraphs)
            {
                AddSubParagraph("Question " + subparagraph.FullId);
                AddParagraph(subparagraph.ExpressionText);
                
                // provide the translations the paragraph references
                AddSteps(subparagraph);

                CloseSubParagraph();
            }
            CloseSubParagraph();
        }

        private void AddImplementations(DataDictionary.Specification.Paragraph subparagraph)
        {
            bool first = true;
            foreach (DataDictionary.ReqRef reference in subparagraph.Implementations)
            {
                DataDictionary.Tests.Translations.Translation translation = reference.Model as DataDictionary.Tests.Translations.Translation;
                if (translation != null)
                {
                    if (first)
                    {
                        AddTable(new string[] { "Related translations" }, new int[] { 100 });
                        first = false;
                    }
                    foreach (DataDictionary.Tests.Translations.SourceText sourceText in translation.SourceTexts)
                    {
                        AddRow(sourceText.ExpressionText);
                        foreach (DataDictionary.Tests.Translations.SourceTextComment comment in sourceText.Comments)
                        {
                            AddRow(comment.ExpressionText);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds the table of implementations of a paragraph
        /// </summary>
        /// <param name="subparagraph"></param>
        private void AddSteps(DataDictionary.Specification.Paragraph subparagraph)
        {
            AddTable(new string[] { "Test case" , "Sequence" }, new int[] { 40, 90 });

            Dictionary<string, string> TestFeatures = findSteps(subparagraph);

            foreach (KeyValuePair<string, string> testFT in TestFeatures)
            {
                AddRow(new string[] { testFT.Key, testFT.Value });
            }
        }

        /// <summary>
        /// Builds a dictionary of sequences containing a given test case, from the implementations of a paragraph
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        private Dictionary<string, string> findSteps(DataDictionary.Specification.Paragraph paragraph)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            foreach (DataDictionary.ReqRef reference in paragraph.Implementations)
            {
                DataDictionary.Tests.Step step = reference.Model as DataDictionary.Tests.Step;
                if (step != null)
                {
                    if (retVal.ContainsKey(step.TestCase.Name))
                    {
                        // Only add the subsequence if it is not already in the string
                        if (retVal[step.TestCase.Name].IndexOf(step.SubSequence.Name) == -1)
                        {
                            retVal[step.TestCase.Name] = retVal[step.TestCase.Name] + "\n" + step.SubSequence.Name;
                        }
                    }
                    else
                    {
                        retVal[step.TestCase.Name] = step.SubSequence.Name;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Creates a table for the remarks
        /// </summary>
        /// <param name="aDictionary"></param>
        public void GenerateComments(DataDictionary.Dictionary aDictionary)
        {
            AddSubParagraph("");
            DataDictionary.Specification.Paragraph questions = getSection("Comments", aDictionary);
            foreach (DataDictionary.Specification.Paragraph subparagraph in questions.SubParagraphs)
            {
                AddSubParagraph("Comment " + subparagraph.FullId);
                AddParagraph(subparagraph.ExpressionText);

                // provide the translations the paragraph references
                AddSteps(subparagraph);

                CloseSubParagraph();
            }
            CloseSubParagraph();
        }

        /// <summary>
        /// Creates a table for the bugs
        /// </summary>
        /// <param name="aDictionary"></param>
        public void GenerateBugs(DataDictionary.Dictionary aDictionary)
        {
            AddSubParagraph("");
            DataDictionary.Specification.Paragraph questions = getSection("Bugs", aDictionary);
            foreach (DataDictionary.Specification.Paragraph subparagraph in questions.SubParagraphs)
            {
                AddSubParagraph("Bug " + subparagraph.FullId);
                AddParagraph(subparagraph.ExpressionText);

                // provide the translations the paragraph references
                AddSteps(subparagraph);

                CloseSubParagraph();
            }
        }
    }
}
