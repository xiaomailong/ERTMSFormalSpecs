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


namespace DataDictionary.Tests
{
    public class SubStep : Generated.SubStep, TextualExplain, ICommentable
    {
        /// <summary>
        /// This step changes
        /// </summary>
        public System.Collections.ArrayList Actions
        {
            get
            {
                if (allActions() == null)
                {
                    setAllActions(new System.Collections.ArrayList());
                }
                return allActions();
            }
        }


        /// <summary>
        /// This step expectations
        /// </summary>
        public System.Collections.ArrayList Expectations
        {
            get
            {
                if (allExpectations() == null)
                {
                    setAllExpectations(new System.Collections.ArrayList());
                }
                return allExpectations();
            }
        }

        /// <summary>
        /// The enclosing step, if any
        /// </summary>
        public Step Step
        {
            get { return Enclosing as Step; }
        }

        /// <summary>
        /// The enclosing translation, if any
        /// </summary>
        public Translations.Translation Translation
        {
            get { return Enclosing as Translations.Translation; }
        }

        public override System.Collections.ArrayList EnclosingCollection
        {
            get
            {
                System.Collections.ArrayList retVal = null;

                if (Step != null)
                {
                    retVal = Step.SubSteps;
                }
                else if (Translation != null)
                {
                    retVal = Translation.SubSteps;
                }

                return retVal;
            }
        }

        /// <summary>
        /// Adds a model element in this model element
        /// </summary>
        /// <param name="copy"></param>
        public override void AddModelElement(Utils.IModelElement element)
        {
            if (element is Rules.Action)
            {
                Rules.Action item = element as Rules.Action;
                if (item != null)
                {
                    appendActions(item);
                }
            }
            else if (element is Expectation)
            {
                Tests.Expectation item = element as Tests.Expectation;
                if (item != null)
                {
                    appendExpectations(item);
                }
            }
        }

        /// <summary>
        /// Indicates if this step contains some actions or expectations
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            bool retVal = true;
            if (Actions.Count != 0 || Expectations.Count != 0)
            {
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// The explanation of this step, as RTF pseudo code
        /// </summary>
        /// <returns></returns>
        public override string getExplain()
        {
            string retVal = "";

            foreach (Rules.Action action in Actions)
            {
                retVal = retVal + action.Name + "\n";
            }

            if (Expectations.Count > 0)
            {
                retVal = retVal + "implies\n";
                foreach (Expectation expectation in Expectations)
                {
                    retVal = retVal + "  " + expectation.getExplain() + "\n";
                }
            }

            return retVal;
        }

        /// <summary>
        /// Creates a sub step and sets its default values
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SubStep createDefault(string name)
        {
            SubStep retVal = (SubStep)DataDictionary.Generated.acceptor.getFactory().createSubStep();
            retVal.Name = name;

            return retVal;
        }


        /// <summary>
        /// Provides an explanation of the step's behaviour
        /// </summary>
        /// <param name="indentLevel">the number of white spaces to add at the beginning of each line</param>
        /// <returns></returns>
        public string getExplain(int indentLevel, bool explainSubElements)
        {
            string retVal = TextualExplainUtilities.Pad("{\\cf11 // " + Name + "}\\cf1\\par", indentLevel);

            foreach (Rules.Action action in Actions)
            {
                retVal += action.getExplain(indentLevel + 2, explainSubElements) + "\\par";
            }
            retVal += TextualExplainUtilities.Pad("{\\b IMPLIES}\\par", indentLevel);
            foreach (Expectation expectation in Expectations)
            {
                retVal += expectation.getExplain(indentLevel + 2, explainSubElements) + "\\par";
            }

            return retVal;
        }

        /// <summary>
        /// Provides an explanation of the step's behaviour
        /// </summary>

        /// <param name="explainSubElements">Precises if we need to explain the sub elements (if any)</param>
        /// <returns></returns>
        public string getExplain(bool explainSubElements)
        {
            string retVal = "";

            retVal = getExplain(0, explainSubElements);

            return retVal;
        }

        /// <summary>
        /// The comment related to this element
        /// </summary>
        public string Comment
        {
            get { return getComment(); }
            set { setComment(value); }
        }

    }
}
