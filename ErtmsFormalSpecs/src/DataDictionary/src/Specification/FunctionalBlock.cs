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

namespace DataDictionary.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// Represents a functional block
    /// </summary>
    public class FunctionalBlock : Generated.FunctionalBlock, IGraphicalDisplay, IHoldsParagraphs
    {
        /// <summary>
        /// Provides all the dependances related to this functional block
        /// </summary>
        public ArrayList Dependances
        {
            get
            {
                if (allDependances() == null)
                {
                    setAllDependances(new ArrayList());
                }

                return allDependances();
            }
        }

        /// <summary>
        /// The X position
        /// </summary>
        public int X { get { return getX(); } set { setX(value); } }

        /// <summary>
        /// The Y position
        /// </summary>
        public int Y { get { return getY(); } set { setY(value); } }

        /// <summary>
        /// The width
        /// </summary>
        public int Width { get { return getWidth(); } set { setWidth(value); } }

        /// <summary>
        /// The height
        /// </summary>
        public int Height { get { return getHeight(); } set { setHeight(value); } }

        /// <summary>
        /// The name to be displayed
        /// </summary>
        public string GraphicalName { get { return Name; } }

        /// <summary>
        /// Indicates that the element is hiddent
        /// </summary>
        public bool Hidden { get { return false; } set { } }

        /// <summary>
        /// The explanation of the element
        /// </summary>
        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        public string getExplain(bool explainSubElements)
        {
            return Name;
        }

        private class ParagraphForFunctionalBlock : Generated.Visitor
        {
            /// <summary>
            /// The functional block for which the paragraphs should be found
            /// </summary>
            private FunctionalBlock FunctionalBlock { get; set; }

            /// <summary>
            /// The list of paragraphs to be filled
            /// </summary>
            public List<Paragraph> Paragraphs { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="functionalBlock"></param>
            public ParagraphForFunctionalBlock(FunctionalBlock functionalBlock, List<Paragraph> paragraphs)
            {
                FunctionalBlock = functionalBlock;
                Paragraphs = paragraphs;
            }

            public override void visit(Generated.Paragraph obj, bool visitSubNodes)
            {
                Paragraph paragraph = (Paragraph)obj;

                if (paragraph.BelongsToFunctionalBlock(FunctionalBlock.Name))
                {
                    Paragraphs.Add(paragraph);

                    // Avoid visiting sub paragraphs
                    paragraph.GetParagraphs(Paragraphs);
                }
                else
                {
                    base.visit(obj, visitSubNodes);
                }
            }
        }

        /// <summary>
        /// Provides the paragraphs related to this functional block 
        /// </summary>
        /// <param name="paragraphs"></param>
        public void GetParagraphs(List<Paragraph> paragraphs)
        {
            ParagraphForFunctionalBlock gatherer = new ParagraphForFunctionalBlock(this, paragraphs);
            foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
            {
                gatherer.visit(dictionary);
            }
        }
    }
}
