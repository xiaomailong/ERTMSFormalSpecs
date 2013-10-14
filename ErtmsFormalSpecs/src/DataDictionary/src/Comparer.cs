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
    /// <summary>
    /// This class is used to compare two dictionaries 
    /// by annotating the first dictionary with the differences with the second
    /// </summary>
    public static class Comparer
    {

        /// <summary>
        /// Computes a canonic form of a string
        /// </summary>
        /// <param name="s1"></param>
        /// <returns></returns>
        public static string canonicString(string s1)
        {
            string retVal = s1;

            if (retVal == null)
            {
                retVal = "";
            }

            retVal = retVal.Replace('\n', ' ');
            retVal = retVal.Replace('\t', ' ');
            retVal = retVal.Replace('\r', ' ');

            while (retVal.IndexOf("  ") >= 0)
            {
                retVal = retVal.Replace("  ", " ");
            }

            return retVal;
        }

        /// <summary>
        /// Compares two strings
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool canonicalStringEquality(string s1, string s2)
        {
            bool retVal;

            s1 = canonicString(s1);
            s2 = canonicString(s2);
            retVal = s1 == s2;

            return retVal;
        }

        /// <summary>
        /// Compares two Namable and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareNamable(Generated.Namable obj, Generated.Namable other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            if ( !canonicalStringEquality(obj.getName(), other.getName()) )
            {
                obj.AddInfo ("Value of Name changed. Previous value was " + other.getName());
            }
        }

        /// <summary>
        /// Compares two ReferencesParagraph and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareReferencesParagraph(Generated.ReferencesParagraph obj, Generated.ReferencesParagraph other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allRequirements() != null && other.allRequirements() != null ) 
            {
                foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                {
                    bool compared = false;
                    foreach ( Generated.ReqRef otherElement in other.allRequirements() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareReqRef ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.ReqRef otherElement in other.allRequirements() )
                {
                    bool found = false;
                    foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("Value of Comment changed. Previous value was " + other.getComment());
            }
        }

        /// <summary>
        /// Compares two ReqRelated and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareReqRelated(Generated.ReqRelated obj, Generated.ReqRelated other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReferencesParagraph (obj, other);

            if ( obj.getImplemented() != other.getImplemented() )
            {
                obj.AddInfo ("Value of Implemented changed. Previous value was " + other.getImplemented());
            }
            if ( obj.getVerified() != other.getVerified() )
            {
                obj.AddInfo ("Value of Verified changed. Previous value was " + other.getVerified());
            }
            if ( obj.getNeedsRequirement() != other.getNeedsRequirement() )
            {
                obj.AddInfo ("Value of NeedsRequirement changed. Previous value was " + other.getNeedsRequirement());
            }
        }

        /// <summary>
        /// Compares two Dictionary and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDictionary(Generated.Dictionary obj, Generated.Dictionary other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            if ( obj.getSpecification() == null )
            {
                if ( other.getSpecification() != null )
                {
                    obj.AddInfo ("Element Specification has been removed");
                }
            }
            else
            {
                compareSpecification ( obj.getSpecification(), other.getSpecification() );
            }
            if ( obj.allRuleDisablings() != null && other.allRuleDisablings() != null ) 
            {
                foreach ( Generated.RuleDisabling subElement in obj.allRuleDisablings() )
                {
                    bool compared = false;
                    foreach ( Generated.RuleDisabling otherElement in other.allRuleDisablings() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRuleDisabling ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.RuleDisabling otherElement in other.allRuleDisablings() )
                {
                    bool found = false;
                    foreach ( Generated.RuleDisabling subElement in obj.allRuleDisablings() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allNameSpaces() != null && other.allNameSpaces() != null ) 
            {
                foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                {
                    bool compared = false;
                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareNameSpace ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                {
                    bool found = false;
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allNameSpaceRefs() != null && other.allNameSpaceRefs() != null ) 
            {
                foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                {
                    bool compared = false;
                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareNameSpaceRef ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                {
                    bool found = false;
                    foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allTests() != null && other.allTests() != null ) 
            {
                foreach ( Generated.Frame subElement in obj.allTests() )
                {
                    bool compared = false;
                    foreach ( Generated.Frame otherElement in other.allTests() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareFrame ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Frame otherElement in other.allTests() )
                {
                    bool found = false;
                    foreach ( Generated.Frame subElement in obj.allTests() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allTestRefs() != null && other.allTestRefs() != null ) 
            {
                foreach ( Generated.FrameRef subElement in obj.allTestRefs() )
                {
                    bool compared = false;
                    foreach ( Generated.FrameRef otherElement in other.allTestRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareFrameRef ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.FrameRef otherElement in other.allTestRefs() )
                {
                    bool found = false;
                    foreach ( Generated.FrameRef subElement in obj.allTestRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.getTranslationDictionary() == null )
            {
                if ( other.getTranslationDictionary() != null )
                {
                    obj.AddInfo ("Element TranslationDictionary has been removed");
                }
            }
            else
            {
                compareTranslationDictionary ( obj.getTranslationDictionary(), other.getTranslationDictionary() );
            }
            if ( obj.getShortcutDictionary() == null )
            {
                if ( other.getShortcutDictionary() != null )
                {
                    obj.AddInfo ("Element ShortcutDictionary has been removed");
                }
            }
            else
            {
                compareShortcutDictionary ( obj.getShortcutDictionary(), other.getShortcutDictionary() );
            }
            if ( !canonicalStringEquality(obj.getXsi(), other.getXsi()) )
            {
                obj.AddInfo ("Value of Xsi changed. Previous value was " + other.getXsi());
            }
            if ( !canonicalStringEquality(obj.getXsiLocation(), other.getXsiLocation()) )
            {
                obj.AddInfo ("Value of XsiLocation changed. Previous value was " + other.getXsiLocation());
            }
        }

        /// <summary>
        /// Compares two RuleDisabling and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRuleDisabling(Generated.RuleDisabling obj, Generated.RuleDisabling other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( !canonicalStringEquality(obj.getRule(), other.getRule()) )
            {
                obj.AddInfo ("Value of Rule changed. Previous value was " + other.getRule());
            }
        }

        /// <summary>
        /// Compares two NameSpaceRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareNameSpaceRef(Generated.NameSpaceRef obj, Generated.NameSpaceRef other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

        }

        /// <summary>
        /// Compares two NameSpace and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareNameSpace(Generated.NameSpace obj, Generated.NameSpace other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allNameSpaces() != null && other.allNameSpaces() != null ) 
            {
                foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                {
                    bool compared = false;
                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareNameSpace ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                {
                    bool found = false;
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allNameSpaceRefs() != null && other.allNameSpaceRefs() != null ) 
            {
                foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                {
                    bool compared = false;
                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareNameSpaceRef ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                {
                    bool found = false;
                    foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allRanges() != null && other.allRanges() != null ) 
            {
                foreach ( Generated.Range subElement in obj.allRanges() )
                {
                    bool compared = false;
                    foreach ( Generated.Range otherElement in other.allRanges() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRange ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Range otherElement in other.allRanges() )
                {
                    bool found = false;
                    foreach ( Generated.Range subElement in obj.allRanges() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allEnumerations() != null && other.allEnumerations() != null ) 
            {
                foreach ( Generated.Enum subElement in obj.allEnumerations() )
                {
                    bool compared = false;
                    foreach ( Generated.Enum otherElement in other.allEnumerations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareEnum ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Enum otherElement in other.allEnumerations() )
                {
                    bool found = false;
                    foreach ( Generated.Enum subElement in obj.allEnumerations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allStructures() != null && other.allStructures() != null ) 
            {
                foreach ( Generated.Structure subElement in obj.allStructures() )
                {
                    bool compared = false;
                    foreach ( Generated.Structure otherElement in other.allStructures() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareStructure ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Structure otherElement in other.allStructures() )
                {
                    bool found = false;
                    foreach ( Generated.Structure subElement in obj.allStructures() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allCollections() != null && other.allCollections() != null ) 
            {
                foreach ( Generated.Collection subElement in obj.allCollections() )
                {
                    bool compared = false;
                    foreach ( Generated.Collection otherElement in other.allCollections() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareCollection ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Collection otherElement in other.allCollections() )
                {
                    bool found = false;
                    foreach ( Generated.Collection subElement in obj.allCollections() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allStateMachines() != null && other.allStateMachines() != null ) 
            {
                foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                {
                    bool compared = false;
                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareStateMachine ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                {
                    bool found = false;
                    foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allFunctions() != null && other.allFunctions() != null ) 
            {
                foreach ( Generated.Function subElement in obj.allFunctions() )
                {
                    bool compared = false;
                    foreach ( Generated.Function otherElement in other.allFunctions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareFunction ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Function otherElement in other.allFunctions() )
                {
                    bool found = false;
                    foreach ( Generated.Function subElement in obj.allFunctions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allProcedures() != null && other.allProcedures() != null ) 
            {
                foreach ( Generated.Procedure subElement in obj.allProcedures() )
                {
                    bool compared = false;
                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareProcedure ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Procedure otherElement in other.allProcedures() )
                {
                    bool found = false;
                    foreach ( Generated.Procedure subElement in obj.allProcedures() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allVariables() != null && other.allVariables() != null ) 
            {
                foreach ( Generated.Variable subElement in obj.allVariables() )
                {
                    bool compared = false;
                    foreach ( Generated.Variable otherElement in other.allVariables() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareVariable ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Variable otherElement in other.allVariables() )
                {
                    bool found = false;
                    foreach ( Generated.Variable subElement in obj.allVariables() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allRules() != null && other.allRules() != null ) 
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    bool compared = false;
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRule ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Rule otherElement in other.allRules() )
                {
                    bool found = false;
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two ReqRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareReqRef(Generated.ReqRef obj, Generated.ReqRef other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            if ( !canonicalStringEquality(obj.getId(), other.getId()) )
            {
                obj.AddInfo ("Value of Id changed. Previous value was " + other.getId());
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("Value of Comment changed. Previous value was " + other.getComment());
            }
        }

        /// <summary>
        /// Compares two Type and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareType(Generated.Type obj, Generated.Type other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( !canonicalStringEquality(obj.getDefault(), other.getDefault()) )
            {
                obj.AddInfo ("Value of Default changed. Previous value was " + other.getDefault());
            }
        }

        /// <summary>
        /// Compares two Enum and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareEnum(Generated.Enum obj, Generated.Enum other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareType (obj, other);

            if ( obj.allValues() != null && other.allValues() != null ) 
            {
                foreach ( Generated.EnumValue subElement in obj.allValues() )
                {
                    bool compared = false;
                    foreach ( Generated.EnumValue otherElement in other.allValues() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareEnumValue ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.EnumValue otherElement in other.allValues() )
                {
                    bool found = false;
                    foreach ( Generated.EnumValue subElement in obj.allValues() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allSubEnums() != null && other.allSubEnums() != null ) 
            {
                foreach ( Generated.Enum subElement in obj.allSubEnums() )
                {
                    bool compared = false;
                    foreach ( Generated.Enum otherElement in other.allSubEnums() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareEnum ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Enum otherElement in other.allSubEnums() )
                {
                    bool found = false;
                    foreach ( Generated.Enum subElement in obj.allSubEnums() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two EnumValue and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareEnumValue(Generated.EnumValue obj, Generated.EnumValue other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getValue(), other.getValue()) )
            {
                obj.AddInfo ("Value of Value changed. Previous value was " + other.getValue());
            }
        }

        /// <summary>
        /// Compares two Range and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRange(Generated.Range obj, Generated.Range other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareType (obj, other);

            if ( !canonicalStringEquality(obj.getMinValue(), other.getMinValue()) )
            {
                obj.AddInfo ("Value of MinValue changed. Previous value was " + other.getMinValue());
            }
            if ( !canonicalStringEquality(obj.getMaxValue(), other.getMaxValue()) )
            {
                obj.AddInfo ("Value of MaxValue changed. Previous value was " + other.getMaxValue());
            }
            if ( obj.allSpecialValues() != null && other.allSpecialValues() != null ) 
            {
                foreach ( Generated.EnumValue subElement in obj.allSpecialValues() )
                {
                    bool compared = false;
                    foreach ( Generated.EnumValue otherElement in other.allSpecialValues() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareEnumValue ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.EnumValue otherElement in other.allSpecialValues() )
                {
                    bool found = false;
                    foreach ( Generated.EnumValue subElement in obj.allSpecialValues() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.getPrecision() != other.getPrecision() )
            {
                obj.AddInfo ("Value of Precision changed. Previous value was " + other.getPrecision());
            }
        }

        /// <summary>
        /// Compares two Structure and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStructure(Generated.Structure obj, Generated.Structure other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareType (obj, other);

            if ( obj.allElements() != null && other.allElements() != null ) 
            {
                foreach ( Generated.StructureElement subElement in obj.allElements() )
                {
                    bool compared = false;
                    foreach ( Generated.StructureElement otherElement in other.allElements() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareStructureElement ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.StructureElement otherElement in other.allElements() )
                {
                    bool found = false;
                    foreach ( Generated.StructureElement subElement in obj.allElements() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allProcedures() != null && other.allProcedures() != null ) 
            {
                foreach ( Generated.Procedure subElement in obj.allProcedures() )
                {
                    bool compared = false;
                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareProcedure ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Procedure otherElement in other.allProcedures() )
                {
                    bool found = false;
                    foreach ( Generated.Procedure subElement in obj.allProcedures() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allStateMachines() != null && other.allStateMachines() != null ) 
            {
                foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                {
                    bool compared = false;
                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareStateMachine ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                {
                    bool found = false;
                    foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allRules() != null && other.allRules() != null ) 
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    bool compared = false;
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRule ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Rule otherElement in other.allRules() )
                {
                    bool found = false;
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two StructureElement and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStructureElement(Generated.StructureElement obj, Generated.StructureElement other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( !canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                obj.AddInfo ("Value of TypeName changed. Previous value was " + other.getTypeName());
            }
            if ( !canonicalStringEquality(obj.getDefault(), other.getDefault()) )
            {
                obj.AddInfo ("Value of Default changed. Previous value was " + other.getDefault());
            }
            if ( obj.getMode() != other.getMode() )
            {
                obj.AddInfo ("Value of Mode changed. Previous value was " + other.getMode());
            }
        }

        /// <summary>
        /// Compares two Collection and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareCollection(Generated.Collection obj, Generated.Collection other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareType (obj, other);

            if ( !canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                obj.AddInfo ("Value of TypeName changed. Previous value was " + other.getTypeName());
            }
            if ( obj.getMaxSize() != other.getMaxSize() )
            {
                obj.AddInfo ("Value of MaxSize changed. Previous value was " + other.getMaxSize());
            }
        }

        /// <summary>
        /// Compares two Function and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFunction(Generated.Function obj, Generated.Function other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareType (obj, other);

            if ( obj.allParameters() != null && other.allParameters() != null ) 
            {
                foreach ( Generated.Parameter subElement in obj.allParameters() )
                {
                    bool compared = false;
                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareParameter ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Parameter otherElement in other.allParameters() )
                {
                    bool found = false;
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allCases() != null && other.allCases() != null ) 
            {
                foreach ( Generated.Case subElement in obj.allCases() )
                {
                    bool compared = false;
                    foreach ( Generated.Case otherElement in other.allCases() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareCase ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Case otherElement in other.allCases() )
                {
                    bool found = false;
                    foreach ( Generated.Case subElement in obj.allCases() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( !canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                obj.AddInfo ("Value of TypeName changed. Previous value was " + other.getTypeName());
            }
            if ( obj.getCacheable() != other.getCacheable() )
            {
                obj.AddInfo ("Value of Cacheable changed. Previous value was " + other.getCacheable());
            }
        }

        /// <summary>
        /// Compares two Parameter and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareParameter(Generated.Parameter obj, Generated.Parameter other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                obj.AddInfo ("Value of TypeName changed. Previous value was " + other.getTypeName());
            }
        }

        /// <summary>
        /// Compares two Case and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareCase(Generated.Case obj, Generated.Case other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allPreConditions() != null && other.allPreConditions() != null ) 
            {
                foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                {
                    bool compared = false;
                    foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            comparePreCondition ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                {
                    bool found = false;
                    foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( !canonicalStringEquality(obj.getExpression(), other.getExpression()) )
            {
                obj.AddInfo ("Value of Expression changed. Previous value was " + other.getExpression());
            }
        }

        /// <summary>
        /// Compares two Procedure and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareProcedure(Generated.Procedure obj, Generated.Procedure other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( obj.getStateMachine() == null )
            {
                if ( other.getStateMachine() != null )
                {
                    obj.AddInfo ("Element StateMachine has been removed");
                }
            }
            else
            {
                compareStateMachine ( obj.getStateMachine(), other.getStateMachine() );
            }
            if ( obj.allRules() != null && other.allRules() != null ) 
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    bool compared = false;
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRule ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Rule otherElement in other.allRules() )
                {
                    bool found = false;
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allParameters() != null && other.allParameters() != null ) 
            {
                foreach ( Generated.Parameter subElement in obj.allParameters() )
                {
                    bool compared = false;
                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareParameter ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Parameter otherElement in other.allParameters() )
                {
                    bool found = false;
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two StateMachine and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStateMachine(Generated.StateMachine obj, Generated.StateMachine other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareType (obj, other);

            if ( !canonicalStringEquality(obj.getInitialState(), other.getInitialState()) )
            {
                obj.AddInfo ("Value of InitialState changed. Previous value was " + other.getInitialState());
            }
            if ( obj.allStates() != null && other.allStates() != null ) 
            {
                foreach ( Generated.State subElement in obj.allStates() )
                {
                    bool compared = false;
                    foreach ( Generated.State otherElement in other.allStates() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareState ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.State otherElement in other.allStates() )
                {
                    bool found = false;
                    foreach ( Generated.State subElement in obj.allStates() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allRules() != null && other.allRules() != null ) 
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    bool compared = false;
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRule ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Rule otherElement in other.allRules() )
                {
                    bool found = false;
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two State and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareState(Generated.State obj, Generated.State other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( obj.getStateMachine() == null )
            {
                if ( other.getStateMachine() != null )
                {
                    obj.AddInfo ("Element StateMachine has been removed");
                }
            }
            else
            {
                compareStateMachine ( obj.getStateMachine(), other.getStateMachine() );
            }
            if ( obj.getWidth() != other.getWidth() )
            {
                obj.AddInfo ("Value of Width changed. Previous value was " + other.getWidth());
            }
            if ( obj.getHeight() != other.getHeight() )
            {
                obj.AddInfo ("Value of Height changed. Previous value was " + other.getHeight());
            }
            if ( obj.getX() != other.getX() )
            {
                obj.AddInfo ("Value of X changed. Previous value was " + other.getX());
            }
            if ( obj.getY() != other.getY() )
            {
                obj.AddInfo ("Value of Y changed. Previous value was " + other.getY());
            }
        }

        /// <summary>
        /// Compares two Variable and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareVariable(Generated.Variable obj, Generated.Variable other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( !canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                obj.AddInfo ("Value of TypeName changed. Previous value was " + other.getTypeName());
            }
            if ( !canonicalStringEquality(obj.getDefaultValue(), other.getDefaultValue()) )
            {
                obj.AddInfo ("Value of DefaultValue changed. Previous value was " + other.getDefaultValue());
            }
            if ( obj.getVariableMode() != other.getVariableMode() )
            {
                obj.AddInfo ("Value of VariableMode changed. Previous value was " + other.getVariableMode());
            }
            if ( obj.allSubVariables() != null && other.allSubVariables() != null ) 
            {
                foreach ( Generated.Variable subElement in obj.allSubVariables() )
                {
                    bool compared = false;
                    foreach ( Generated.Variable otherElement in other.allSubVariables() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareVariable ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Variable otherElement in other.allSubVariables() )
                {
                    bool found = false;
                    foreach ( Generated.Variable subElement in obj.allSubVariables() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two Rule and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRule(Generated.Rule obj, Generated.Rule other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( obj.getPriority() != other.getPriority() )
            {
                obj.AddInfo ("Value of Priority changed. Previous value was " + other.getPriority());
            }
            if ( obj.allConditions() != null && other.allConditions() != null ) 
            {
                foreach ( Generated.RuleCondition subElement in obj.allConditions() )
                {
                    bool compared = false;
                    foreach ( Generated.RuleCondition otherElement in other.allConditions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRuleCondition ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.RuleCondition otherElement in other.allConditions() )
                {
                    bool found = false;
                    foreach ( Generated.RuleCondition subElement in obj.allConditions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two RuleCondition and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRuleCondition(Generated.RuleCondition obj, Generated.RuleCondition other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( obj.allPreConditions() != null && other.allPreConditions() != null ) 
            {
                foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                {
                    bool compared = false;
                    foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            comparePreCondition ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                {
                    bool found = false;
                    foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allActions() != null && other.allActions() != null ) 
            {
                foreach ( Generated.Action subElement in obj.allActions() )
                {
                    bool compared = false;
                    foreach ( Generated.Action otherElement in other.allActions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareAction ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Action otherElement in other.allActions() )
                {
                    bool found = false;
                    foreach ( Generated.Action subElement in obj.allActions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allSubRules() != null && other.allSubRules() != null ) 
            {
                foreach ( Generated.Rule subElement in obj.allSubRules() )
                {
                    bool compared = false;
                    foreach ( Generated.Rule otherElement in other.allSubRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareRule ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Rule otherElement in other.allSubRules() )
                {
                    bool found = false;
                    foreach ( Generated.Rule subElement in obj.allSubRules() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two PreCondition and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void comparePreCondition(Generated.PreCondition obj, Generated.PreCondition other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            if ( !canonicalStringEquality(obj.getCondition(), other.getCondition()) )
            {
                obj.AddInfo ("Value of Condition changed. Previous value was " + other.getCondition());
            }
        }

        /// <summary>
        /// Compares two Action and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareAction(Generated.Action obj, Generated.Action other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            if ( !canonicalStringEquality(obj.getExpression(), other.getExpression()) )
            {
                obj.AddInfo ("Value of Expression changed. Previous value was " + other.getExpression());
            }
        }

        /// <summary>
        /// Compares two FrameRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFrameRef(Generated.FrameRef obj, Generated.FrameRef other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

        }

        /// <summary>
        /// Compares two Frame and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFrame(Generated.Frame obj, Generated.Frame other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getCycleDuration(), other.getCycleDuration()) )
            {
                obj.AddInfo ("Value of CycleDuration changed. Previous value was " + other.getCycleDuration());
            }
            if ( obj.allSubSequences() != null && other.allSubSequences() != null ) 
            {
                foreach ( Generated.SubSequence subElement in obj.allSubSequences() )
                {
                    bool compared = false;
                    foreach ( Generated.SubSequence otherElement in other.allSubSequences() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareSubSequence ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.SubSequence otherElement in other.allSubSequences() )
                {
                    bool found = false;
                    foreach ( Generated.SubSequence subElement in obj.allSubSequences() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two SubSequence and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSubSequence(Generated.SubSequence obj, Generated.SubSequence other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getD_LRBG(), other.getD_LRBG()) )
            {
                obj.AddInfo ("Value of D_LRBG changed. Previous value was " + other.getD_LRBG());
            }
            if ( !canonicalStringEquality(obj.getLevel(), other.getLevel()) )
            {
                obj.AddInfo ("Value of Level changed. Previous value was " + other.getLevel());
            }
            if ( !canonicalStringEquality(obj.getMode(), other.getMode()) )
            {
                obj.AddInfo ("Value of Mode changed. Previous value was " + other.getMode());
            }
            if ( !canonicalStringEquality(obj.getNID_LRBG(), other.getNID_LRBG()) )
            {
                obj.AddInfo ("Value of NID_LRBG changed. Previous value was " + other.getNID_LRBG());
            }
            if ( !canonicalStringEquality(obj.getQ_DIRLRBG(), other.getQ_DIRLRBG()) )
            {
                obj.AddInfo ("Value of Q_DIRLRBG changed. Previous value was " + other.getQ_DIRLRBG());
            }
            if ( !canonicalStringEquality(obj.getQ_DIRTRAIN(), other.getQ_DIRTRAIN()) )
            {
                obj.AddInfo ("Value of Q_DIRTRAIN changed. Previous value was " + other.getQ_DIRTRAIN());
            }
            if ( !canonicalStringEquality(obj.getQ_DLRBG(), other.getQ_DLRBG()) )
            {
                obj.AddInfo ("Value of Q_DLRBG changed. Previous value was " + other.getQ_DLRBG());
            }
            if ( !canonicalStringEquality(obj.getRBC_ID(), other.getRBC_ID()) )
            {
                obj.AddInfo ("Value of RBC_ID changed. Previous value was " + other.getRBC_ID());
            }
            if ( !canonicalStringEquality(obj.getRBCPhone(), other.getRBCPhone()) )
            {
                obj.AddInfo ("Value of RBCPhone changed. Previous value was " + other.getRBCPhone());
            }
            if ( obj.allTestCases() != null && other.allTestCases() != null ) 
            {
                foreach ( Generated.TestCase subElement in obj.allTestCases() )
                {
                    bool compared = false;
                    foreach ( Generated.TestCase otherElement in other.allTestCases() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareTestCase ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.TestCase otherElement in other.allTestCases() )
                {
                    bool found = false;
                    foreach ( Generated.TestCase subElement in obj.allTestCases() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two TestCase and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareTestCase(Generated.TestCase obj, Generated.TestCase other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReqRelated (obj, other);

            if ( obj.getFeature() != other.getFeature() )
            {
                obj.AddInfo ("Value of Feature changed. Previous value was " + other.getFeature());
            }
            if ( obj.getCase() != other.getCase() )
            {
                obj.AddInfo ("Value of Case changed. Previous value was " + other.getCase());
            }
            if ( obj.allSteps() != null && other.allSteps() != null ) 
            {
                foreach ( Generated.Step subElement in obj.allSteps() )
                {
                    bool compared = false;
                    foreach ( Generated.Step otherElement in other.allSteps() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareStep ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Step otherElement in other.allSteps() )
                {
                    bool found = false;
                    foreach ( Generated.Step subElement in obj.allSteps() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two Step and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStep(Generated.Step obj, Generated.Step other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.getTCS_Order() != other.getTCS_Order() )
            {
                obj.AddInfo ("Value of TCS_Order changed. Previous value was " + other.getTCS_Order());
            }
            if ( obj.getDistance() != other.getDistance() )
            {
                obj.AddInfo ("Value of Distance changed. Previous value was " + other.getDistance());
            }
            if ( !canonicalStringEquality(obj.getDescription(), other.getDescription()) )
            {
                obj.AddInfo ("Value of Description changed. Previous value was " + other.getDescription());
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("Value of Comment changed. Previous value was " + other.getComment());
            }
            if ( !canonicalStringEquality(obj.getUserComment(), other.getUserComment()) )
            {
                obj.AddInfo ("Value of UserComment changed. Previous value was " + other.getUserComment());
            }
            if ( obj.getIO() != other.getIO() )
            {
                obj.AddInfo ("Value of IO changed. Previous value was " + other.getIO());
            }
            if ( obj.getInterface() != other.getInterface() )
            {
                obj.AddInfo ("Value of Interface changed. Previous value was " + other.getInterface());
            }
            if ( obj.getLevelIN() != other.getLevelIN() )
            {
                obj.AddInfo ("Value of LevelIN changed. Previous value was " + other.getLevelIN());
            }
            if ( obj.getLevelOUT() != other.getLevelOUT() )
            {
                obj.AddInfo ("Value of LevelOUT changed. Previous value was " + other.getLevelOUT());
            }
            if ( obj.getModeIN() != other.getModeIN() )
            {
                obj.AddInfo ("Value of ModeIN changed. Previous value was " + other.getModeIN());
            }
            if ( obj.getModeOUT() != other.getModeOUT() )
            {
                obj.AddInfo ("Value of ModeOUT changed. Previous value was " + other.getModeOUT());
            }
            if ( obj.getTranslationRequired() != other.getTranslationRequired() )
            {
                obj.AddInfo ("Value of TranslationRequired changed. Previous value was " + other.getTranslationRequired());
            }
            if ( obj.getTranslated() != other.getTranslated() )
            {
                obj.AddInfo ("Value of Translated changed. Previous value was " + other.getTranslated());
            }
            if ( obj.allSubSteps() != null && other.allSubSteps() != null ) 
            {
                foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                {
                    bool compared = false;
                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareSubStep ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                {
                    bool found = false;
                    foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allMessages() != null && other.allMessages() != null ) 
            {
                foreach ( Generated.DBMessage subElement in obj.allMessages() )
                {
                    bool compared = false;
                    foreach ( Generated.DBMessage otherElement in other.allMessages() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareDBMessage ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.DBMessage otherElement in other.allMessages() )
                {
                    bool found = false;
                    foreach ( Generated.DBMessage subElement in obj.allMessages() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two SubStep and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSubStep(Generated.SubStep obj, Generated.SubStep other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allActions() != null && other.allActions() != null ) 
            {
                foreach ( Generated.Action subElement in obj.allActions() )
                {
                    bool compared = false;
                    foreach ( Generated.Action otherElement in other.allActions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareAction ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Action otherElement in other.allActions() )
                {
                    bool found = false;
                    foreach ( Generated.Action subElement in obj.allActions() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allExpectations() != null && other.allExpectations() != null ) 
            {
                foreach ( Generated.Expectation subElement in obj.allExpectations() )
                {
                    bool compared = false;
                    foreach ( Generated.Expectation otherElement in other.allExpectations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareExpectation ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Expectation otherElement in other.allExpectations() )
                {
                    bool found = false;
                    foreach ( Generated.Expectation subElement in obj.allExpectations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.getSkipEngine() != other.getSkipEngine() )
            {
                obj.AddInfo ("Value of SkipEngine changed. Previous value was " + other.getSkipEngine());
            }
        }

        /// <summary>
        /// Compares two Expectation and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareExpectation(Generated.Expectation obj, Generated.Expectation other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getValue(), other.getValue()) )
            {
                obj.AddInfo ("Value of Value changed. Previous value was " + other.getValue());
            }
            if ( obj.getBlocking() != other.getBlocking() )
            {
                obj.AddInfo ("Value of Blocking changed. Previous value was " + other.getBlocking());
            }
            if ( obj.getKind() != other.getKind() )
            {
                obj.AddInfo ("Value of Kind changed. Previous value was " + other.getKind());
            }
            if ( obj.getDeadLine() != other.getDeadLine() )
            {
                obj.AddInfo ("Value of DeadLine changed. Previous value was " + other.getDeadLine());
            }
            if ( !canonicalStringEquality(obj.getCondition(), other.getCondition()) )
            {
                obj.AddInfo ("Value of Condition changed. Previous value was " + other.getCondition());
            }
        }

        /// <summary>
        /// Compares two DBMessage and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDBMessage(Generated.DBMessage obj, Generated.DBMessage other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.getMessageOrder() != other.getMessageOrder() )
            {
                obj.AddInfo ("Value of MessageOrder changed. Previous value was " + other.getMessageOrder());
            }
            if ( obj.getMessageType() != other.getMessageType() )
            {
                obj.AddInfo ("Value of MessageType changed. Previous value was " + other.getMessageType());
            }
            if ( obj.allFields() != null && other.allFields() != null ) 
            {
                foreach ( Generated.DBField subElement in obj.allFields() )
                {
                    bool compared = false;
                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareDBField ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.DBField otherElement in other.allFields() )
                {
                    bool found = false;
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allPackets() != null && other.allPackets() != null ) 
            {
                foreach ( Generated.DBPacket subElement in obj.allPackets() )
                {
                    bool compared = false;
                    foreach ( Generated.DBPacket otherElement in other.allPackets() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareDBPacket ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.DBPacket otherElement in other.allPackets() )
                {
                    bool found = false;
                    foreach ( Generated.DBPacket subElement in obj.allPackets() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two DBPacket and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDBPacket(Generated.DBPacket obj, Generated.DBPacket other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allFields() != null && other.allFields() != null ) 
            {
                foreach ( Generated.DBField subElement in obj.allFields() )
                {
                    bool compared = false;
                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareDBField ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.DBField otherElement in other.allFields() )
                {
                    bool found = false;
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two DBField and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDBField(Generated.DBField obj, Generated.DBField other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getVariable(), other.getVariable()) )
            {
                obj.AddInfo ("Value of Variable changed. Previous value was " + other.getVariable());
            }
            if ( obj.getValue() != other.getValue() )
            {
                obj.AddInfo ("Value of Value changed. Previous value was " + other.getValue());
            }
        }

        /// <summary>
        /// Compares two TranslationDictionary and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareTranslationDictionary(Generated.TranslationDictionary obj, Generated.TranslationDictionary other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allFolders() != null && other.allFolders() != null ) 
            {
                foreach ( Generated.Folder subElement in obj.allFolders() )
                {
                    bool compared = false;
                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareFolder ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Folder otherElement in other.allFolders() )
                {
                    bool found = false;
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allTranslations() != null && other.allTranslations() != null ) 
            {
                foreach ( Generated.Translation subElement in obj.allTranslations() )
                {
                    bool compared = false;
                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareTranslation ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Translation otherElement in other.allTranslations() )
                {
                    bool found = false;
                    foreach ( Generated.Translation subElement in obj.allTranslations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two Folder and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFolder(Generated.Folder obj, Generated.Folder other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allFolders() != null && other.allFolders() != null ) 
            {
                foreach ( Generated.Folder subElement in obj.allFolders() )
                {
                    bool compared = false;
                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareFolder ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Folder otherElement in other.allFolders() )
                {
                    bool found = false;
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allTranslations() != null && other.allTranslations() != null ) 
            {
                foreach ( Generated.Translation subElement in obj.allTranslations() )
                {
                    bool compared = false;
                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareTranslation ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Translation otherElement in other.allTranslations() )
                {
                    bool found = false;
                    foreach ( Generated.Translation subElement in obj.allTranslations() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two Translation and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareTranslation(Generated.Translation obj, Generated.Translation other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allSourceTexts() != null && other.allSourceTexts() != null ) 
            {
                foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                {
                    bool compared = false;
                    foreach ( Generated.SourceText otherElement in other.allSourceTexts() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareSourceText ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.SourceText otherElement in other.allSourceTexts() )
                {
                    bool found = false;
                    foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.getImplemented() != other.getImplemented() )
            {
                obj.AddInfo ("Value of Implemented changed. Previous value was " + other.getImplemented());
            }
            if ( obj.allSubSteps() != null && other.allSubSteps() != null ) 
            {
                foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                {
                    bool compared = false;
                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareSubStep ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                {
                    bool found = false;
                    foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("Value of Comment changed. Previous value was " + other.getComment());
            }
        }

        /// <summary>
        /// Compares two SourceText and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSourceText(Generated.SourceText obj, Generated.SourceText other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

        }

        /// <summary>
        /// Compares two ShortcutDictionary and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareShortcutDictionary(Generated.ShortcutDictionary obj, Generated.ShortcutDictionary other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allFolders() != null && other.allFolders() != null ) 
            {
                foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                {
                    bool compared = false;
                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareShortcutFolder ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                {
                    bool found = false;
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allShortcuts() != null && other.allShortcuts() != null ) 
            {
                foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                {
                    bool compared = false;
                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareShortcut ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                {
                    bool found = false;
                    foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two ShortcutFolder and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareShortcutFolder(Generated.ShortcutFolder obj, Generated.ShortcutFolder other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( obj.allFolders() != null && other.allFolders() != null ) 
            {
                foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                {
                    bool compared = false;
                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareShortcutFolder ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                {
                    bool found = false;
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allShortcuts() != null && other.allShortcuts() != null ) 
            {
                foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                {
                    bool compared = false;
                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareShortcut ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                {
                    bool found = false;
                    foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two Shortcut and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareShortcut(Generated.Shortcut obj, Generated.Shortcut other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getShortcutName(), other.getShortcutName()) )
            {
                obj.AddInfo ("Value of ShortcutName changed. Previous value was " + other.getShortcutName());
            }
        }

        /// <summary>
        /// Compares two Specification and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSpecification(Generated.Specification obj, Generated.Specification other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                obj.AddInfo ("Value of Version changed. Previous value was " + other.getVersion());
            }
            if ( obj.allChapters() != null && other.allChapters() != null ) 
            {
                foreach ( Generated.Chapter subElement in obj.allChapters() )
                {
                    bool compared = false;
                    foreach ( Generated.Chapter otherElement in other.allChapters() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareChapter ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Chapter otherElement in other.allChapters() )
                {
                    bool found = false;
                    foreach ( Generated.Chapter subElement in obj.allChapters() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.allChapterRefs() != null && other.allChapterRefs() != null ) 
            {
                foreach ( Generated.ChapterRef subElement in obj.allChapterRefs() )
                {
                    bool compared = false;
                    foreach ( Generated.ChapterRef otherElement in other.allChapterRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareChapterRef ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.ChapterRef otherElement in other.allChapterRefs() )
                {
                    bool found = false;
                    foreach ( Generated.ChapterRef subElement in obj.allChapterRefs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two ChapterRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareChapterRef(Generated.ChapterRef obj, Generated.ChapterRef other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

        }

        /// <summary>
        /// Compares two Chapter and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareChapter(Generated.Chapter obj, Generated.Chapter other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareNamable (obj, other);

            if ( !canonicalStringEquality(obj.getId(), other.getId()) )
            {
                obj.AddInfo ("Value of Id changed. Previous value was " + other.getId());
            }
            if ( obj.allParagraphs() != null && other.allParagraphs() != null ) 
            {
                foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                {
                    bool compared = false;
                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareParagraph ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                {
                    bool found = false;
                    foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
        }

        /// <summary>
        /// Compares two Paragraph and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareParagraph(Generated.Paragraph obj, Generated.Paragraph other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            compareReferencesParagraph (obj, other);

            if ( !canonicalStringEquality(obj.getId(), other.getId()) )
            {
                obj.AddInfo ("Value of Id changed. Previous value was " + other.getId());
            }
            if ( obj.getType() != other.getType() )
            {
                obj.AddInfo ("Value of Type changed. Previous value was " + other.getType());
            }
            if ( obj.getScope() != other.getScope() )
            {
                obj.AddInfo ("Value of Scope changed. Previous value was " + other.getScope());
            }
            if ( !canonicalStringEquality(obj.getBl(), other.getBl()) )
            {
                obj.AddInfo ("Value of Bl changed. Previous value was " + other.getBl());
            }
            if ( obj.getOptional() != other.getOptional() )
            {
                obj.AddInfo ("Value of Optional changed. Previous value was " + other.getOptional());
            }
            if ( !canonicalStringEquality(obj.getText(), other.getText()) )
            {
                obj.AddInfo ("Value of Text changed. Previous value was " + other.getText());
            }
            if ( !canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                obj.AddInfo ("Value of Version changed. Previous value was " + other.getVersion());
            }
            if ( obj.getReviewed() != other.getReviewed() )
            {
                obj.AddInfo ("Value of Reviewed changed. Previous value was " + other.getReviewed());
            }
            if ( obj.getImplementationStatus() != other.getImplementationStatus() )
            {
                obj.AddInfo ("Value of ImplementationStatus changed. Previous value was " + other.getImplementationStatus());
            }
            if ( obj.allParagraphs() != null && other.allParagraphs() != null ) 
            {
                foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                {
                    bool compared = false;
                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            compareParagraph ( subElement, otherElement );
                            compared = true;
                            break;
                        }
                    }

                    if ( !compared ) 
                    {
                        subElement.AddInfo ("Element added");
                    }
                }
                foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                {
                    bool found = false;
                    foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                    {
                        if ( canonicalStringEquality(subElement.Name, otherElement.Name) )
                        {
                            found = true;
                            break;
                        }
                   }

                   if ( !found )
                   {
                      obj.AddInfo("Element " + otherElement.Name + " has been removed");                   
                   }
               }
            }
            if ( obj.getRevision() == null )
            {
                if ( other.getRevision() != null )
                {
                    obj.AddInfo ("Element Revision has been removed");
                }
            }
            else
            {
                compareParagraphRevision ( obj.getRevision(), other.getRevision() );
            }
            if ( obj.getMoreInfoRequired() != other.getMoreInfoRequired() )
            {
                obj.AddInfo ("Value of MoreInfoRequired changed. Previous value was " + other.getMoreInfoRequired());
            }
            if ( obj.getSpecIssue() != other.getSpecIssue() )
            {
                obj.AddInfo ("Value of SpecIssue changed. Previous value was " + other.getSpecIssue());
            }
            if ( obj.getFunctionalBlock() != other.getFunctionalBlock() )
            {
                obj.AddInfo ("Value of FunctionalBlock changed. Previous value was " + other.getFunctionalBlock());
            }
            if ( !canonicalStringEquality(obj.getFunctionalBlockName(), other.getFunctionalBlockName()) )
            {
                obj.AddInfo ("Value of FunctionalBlockName changed. Previous value was " + other.getFunctionalBlockName());
            }
        }

        /// <summary>
        /// Compares two ParagraphRevision and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareParagraphRevision(Generated.ParagraphRevision obj, Generated.ParagraphRevision other)
        {
            if ( other == null )
            { 
                obj.AddInfo ("Element has been added");
                return;
            }

            if ( !canonicalStringEquality(obj.getText(), other.getText()) )
            {
                obj.AddInfo ("Value of Text changed. Previous value was " + other.getText());
            }
            if ( !canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                obj.AddInfo ("Value of Version changed. Previous value was " + other.getVersion());
            }
        }

    }
}
