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

namespace DataDictionary
{
    public abstract class ModelElement : Generated.BaseModelElement
    {
        /// <summary>
        /// Provides the EFS System in which this element belongs
        /// </summary>
        public EFSSystem EFSSystem
        {
            get
            {
                return EFSSystem.INSTANCE;
            }
        }

        /// <summary>
        /// Provides the Dictionary in which this element belongs
        /// </summary>
        public Dictionary Dictionary
        {
            get
            {
                return Utils.EnclosingFinder<Dictionary>.find(this);
            }
        }

        /// <summary>
        /// Adds a new element log attached to this model element
        /// </summary>
        /// <param name="log"></param>
        protected override void AddElementLog(Utils.ElementLog log)
        {
            Parameter enclosingParameter = Utils.EnclosingFinder<Parameter>.find(this);
            if (enclosingParameter != null)
            {
                log.Log = "In " + FullName + ":" + log.Log;
                enclosingParameter.AddElementLog(log);
            }
            else
            {
                base.AddElementLog(log);
            }
        }
    }

    public interface TextualExplain
    {
        /// <summary>
        /// The explanation of the element
        /// </summary>
        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        string getExplain(bool explainSubElements);
    }

    /// <summary>
    /// Utilities for RTF explain boxes
    /// </summary>
    public class TextualExplainUtilities
    {
        /// <summary>
        /// Left pads the string provided
        /// </summary>
        /// <param name="data">the data to pad</param>
        /// <param name="padlen">the size of the pad</param>
        /// <returns></returns>
        public static string Pad(string data, int padlen)
        {
            return "".PadLeft(padlen) + data;
        }

        /// <summary>
        /// Provides the expression
        /// </summary>
        /// <param name="element"></param>
        /// <param name="padlen"></param>
        /// <returns></returns>
        public static string Expression(ModelElement element, int padlen)
        {
            string retVal = "";

            IExpressionable expressionable = element as IExpressionable;
            if (expressionable != null)
            {
                if (string.IsNullOrEmpty(expressionable.ExpressionText))
                {
                    retVal = TextualExplainUtilities.Pad("<Undefined expression or statement>", padlen);
                }
                else
                {
                    retVal = TextualExplainUtilities.Pad(expressionable.ExpressionText, padlen);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Comments a section of text
        /// </summary>
        /// <param name="data">the data to pad</param>
        /// <param name="padlen">the size of the pad</param>
        /// <returns></returns>
        public static string Comment(string data, int padlen)
        {
            string retVal = "";

            foreach (string line in data.Split('\n'))
            {
                retVal = retVal + Pad("{\\cf11//" + line + "}\\cf1\\par", padlen);
            }

            return retVal;
        }

        /// <summary>
        /// Comments an Icommentable
        /// </summary>
        /// <param name="element"></param>
        /// <param name="padlen"></param>
        /// <returns></returns>
        public static string Comment(ModelElement element, int padlen)
        {
            string retVal = "";

            ICommentable commentable = element as ICommentable;
            if (commentable != null && !string.IsNullOrEmpty(commentable.Comment))
            {
                retVal = Comment(commentable.Comment, padlen);
            }

            return retVal;
        }

        /// <summary>
        /// The name of the element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="padlen"></param>
        /// <returns></returns>
        public static string Name(ModelElement element, int padlen)
        {
            string retVal = "";

            Namable namable = element as Namable;
            if (namable != null)
            {
                retVal = TextualExplainUtilities.Pad("{\\cf11 // " + namable.Name + "}\\cf1\\par", padlen);
            }

            return retVal;
        }

        /// <summary>
        /// Provides the header of the element 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="padlen"></param>
        /// <returns></returns>
        public static string Header(ModelElement element, int padlen)
        {
            string retVal = "";

            retVal += Comment(element, padlen);
            retVal += Name(element, padlen);

            return retVal;
        }

        /// <summary>
        /// Iterates the same character a given number of times
        /// </summary>
        /// <param name="c">the character to iterate</param>
        /// <param name="padlen">the size of the expected result</param>
        /// <returns></returns>
        public static string Iterate(char c, int len)
        {
            string retVal = "";
            for (int i = 0; i < len; i++)
            {
                retVal = retVal + c;
            }
            return retVal;
        }

        /// <summary>
        /// Adds RTF prefixes and postfixes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encapsule(string data)
        {
            string retVal = data;

            if (!retVal.StartsWith("\\rtf"))
            {
                // Replaces all end of lines with \par
                retVal = retVal.Replace("\n", "\\par ");

                // This is used to ensure that the right style is used for the text
                retVal = "{\\cf11}\\cf1" + retVal;

                // Common prefix to handle the colors
                retVal = "{\\rtf1\\ansi{\\colortbl;\\red0\\green0\\blue0;\\red0\\green0\\blue255;\\red0\\green255\\blue255;\\red0\\green255\\blue0;\\red255\\green0\\blue255;\\red255\\green0\\blue0;\\red255\\green255\\blue0;\\red255\\green255\\blue255;\\red0\\green0\\blue128;\\red0\\green128\\blue128;\\red0\\green128\\blue0;\\red128\\green0\\blue128;\\red128\\green0\\blue0;\\red128\\green128\\blue0;\\red128\\green128\\blue128;\\red192\\green192\\blue192;}" + retVal + "}";
            }

            return retVal;
        }
    }
}
