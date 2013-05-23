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
    /// <summary>
    /// Stores data about the paragraphs found in the RTF document
    ///   - the paragraph ID
    ///   - the paragraph Text
    ///   - if there are changes in that paragraph
    /// </summary>
    public class Paragraph
    {
        /// <summary>
        /// The paragraph Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The paragraph text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The text before the change
        /// </summary>
        public string OriginalText { get; set; }

        /// <summary>
        /// The state of the paragraph
        ///  - NoChange : there are no changes between this paragraph and the original one
        ///  - Changed : the paragraph text has changed
        ///  - Inserted : the paragraph was not present in the original
        ///  - Deleted : the paragraph is no more present in the document
        /// </summary>
        public enum ParagraphState { NoChange, Changed, Inserted, Deleted };
        public ParagraphState State { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        public Paragraph(string id)
        {
            Id = id.Trim();
            Text = "";
            State = ParagraphState.NoChange;
        }
    }
}
