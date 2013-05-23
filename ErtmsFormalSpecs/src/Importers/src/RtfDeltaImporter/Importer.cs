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
namespace Importers.RtfDeltaImporter
{
    using DataDictionary.Specification;

    /// <summary>
    /// This class is used to import a delta in the specifications based on a delta operation 
    /// performed in Word and saved in Rtf file format. 
    /// It detects the paragraphs that have been modified and updates the specification 
    /// according to the changes, that is
    ///   - save the current paragraph text, to ease the revision
    ///   - update the paragraph text with the new paragraph version 
    ///   - sets the paragraph as needing a manual review 
    ///   - invalidates the models to take this change into consideration
    /// </summary>
    public class Importer : Utils.ProgressHandler
    {
        /// <summary>
        /// The file path of the original file
        /// </summary>
        private string OriginalFilePath { get; set; }

        /// <summary>
        /// The document which corresponds to the original file path
        /// </summary>
        public Document OriginalDocument { get; private set; }

        /// <summary>
        /// The file path of the new file
        /// </summary>
        private string NewFilePath { get; set; }

        /// <summary>
        /// The document which corresponds to the new file path
        /// </summary>
        public Document NewDocument { get; private set; }

        /// <summary>
        /// The specification to be udated
        /// </summary>
        private Specification Specifications { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="newFilePath"></param>
        /// <param name="specifications"></param>
        public Importer(string originalFilePath, string newFilePath, Specification specifications)
        {
            OriginalFilePath = originalFilePath;
            NewFilePath = newFilePath;
            Specifications = specifications;
        }

        /// <summary>
        /// Performs the delta on the specification provided
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="specifications"></param>
        private void PerformDelta(DataDictionary.Specification.Specification specifications)
        {
            foreach (Paragraph p in NewDocument.ChangedParagraphs)
            {
                DataDictionary.Specification.Paragraph par = specifications.FindParagraph(p.Id);

                if (par != null)
                {
                    par.Text = p.Text;
                    par.AddInfo("Paragraph has been changed");
                }
                else
                {
                    specifications.AddError("Cannot find paragraph " + p.Id + " for modification");
                }
            }

            foreach (Paragraph p in NewDocument.NewParagraphs)
            {
                DataDictionary.Specification.Paragraph par = specifications.FindParagraph(p.Id);

                if (par != null)
                {
                    specifications.AddError("Paragraph " + p.Id + " already exists, whereas it has been detected as a new paragraph in the release");
                }
                else
                {
                    par = specifications.FindParagraph(p.Id, true);
                    par.setText(p.Text);
                    par.AddInfo("New paragraph");
                }
            }

            foreach (Paragraph p in NewDocument.RemovedParagraphs)
            {
                DataDictionary.Specification.Paragraph par = specifications.FindParagraph(p.Id);

                if (par != null)
                {
                    par.Text = "<Removed in current release>";
                    par.AddInfo("Paragraph has been removed");
                }
                else
                {
                    specifications.AddError("Cannot find paragraph " + p.Id + " for removal");
                }
            }
        }

        /// <summary>
        /// Executes the work in the background task
        /// </summary>
        public override void ExecuteWork()
        {
            OriginalDocument = new Document(OriginalFilePath);
            NewDocument = new Document(NewFilePath);

            NewDocument.UpdateState(OriginalDocument);

            PerformDelta(Specifications);
        }
    }
}
