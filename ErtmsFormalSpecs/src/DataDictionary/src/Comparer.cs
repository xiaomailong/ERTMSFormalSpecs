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
                obj.AddInfo ("CHANGED Name, Previously was [" + other.getName() + "]");
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

            if ( obj.allRequirements() != null )
            {
                if ( other.allRequirements() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRequirements() != null ) 
                {
                    foreach ( Generated.ReqRef otherElement in other.allRequirements() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("CHANGED Comment, Previously was [" + other.getComment() + "]");
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
                obj.AddInfo ("CHANGED Implemented, Previously was [" + other.getImplemented() + "]");
            }
            if ( obj.getVerified() != other.getVerified() )
            {
                obj.AddInfo ("CHANGED Verified, Previously was [" + other.getVerified() + "]");
            }
            if ( obj.getNeedsRequirement() != other.getNeedsRequirement() )
            {
                obj.AddInfo ("CHANGED NeedsRequirement, Previously was [" + other.getNeedsRequirement() + "]");
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
                    obj.AddInfo ("REMOVED : Specification");
                }
            }
            else
            {
                compareSpecification ( obj.getSpecification(), other.getSpecification() );
            }
            if ( obj.allRuleDisablings() != null )
            {
                if ( other.allRuleDisablings() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.RuleDisabling subElement in obj.allRuleDisablings() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRuleDisablings() != null ) 
                {
                    foreach ( Generated.RuleDisabling otherElement in other.allRuleDisablings() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allNameSpaces() != null )
            {
                if ( other.allNameSpaces() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaces() != null ) 
                {
                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allNameSpaceRefs() != null )
            {
                if ( other.allNameSpaceRefs() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaceRefs() != null ) 
                {
                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allTests() != null )
            {
                if ( other.allTests() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Frame subElement in obj.allTests() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allTests() != null ) 
                {
                    foreach ( Generated.Frame otherElement in other.allTests() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allTestRefs() != null )
            {
                if ( other.allTestRefs() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.FrameRef subElement in obj.allTestRefs() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allTestRefs() != null ) 
                {
                    foreach ( Generated.FrameRef otherElement in other.allTestRefs() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.getTranslationDictionary() == null )
            {
                if ( other.getTranslationDictionary() != null )
                {
                    obj.AddInfo ("REMOVED : TranslationDictionary");
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
                    obj.AddInfo ("REMOVED : ShortcutDictionary");
                }
            }
            else
            {
                compareShortcutDictionary ( obj.getShortcutDictionary(), other.getShortcutDictionary() );
            }
            if ( !canonicalStringEquality(obj.getXsi(), other.getXsi()) )
            {
                obj.AddInfo ("CHANGED Xsi, Previously was [" + other.getXsi() + "]");
            }
            if ( !canonicalStringEquality(obj.getXsiLocation(), other.getXsiLocation()) )
            {
                obj.AddInfo ("CHANGED XsiLocation, Previously was [" + other.getXsiLocation() + "]");
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
                obj.AddInfo ("CHANGED Rule, Previously was [" + other.getRule() + "]");
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

            if ( obj.allNameSpaces() != null )
            {
                if ( other.allNameSpaces() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaces() != null ) 
                {
                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allNameSpaceRefs() != null )
            {
                if ( other.allNameSpaceRefs() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaceRefs() != null ) 
                {
                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allRanges() != null )
            {
                if ( other.allRanges() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Range subElement in obj.allRanges() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRanges() != null ) 
                {
                    foreach ( Generated.Range otherElement in other.allRanges() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allEnumerations() != null )
            {
                if ( other.allEnumerations() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Enum subElement in obj.allEnumerations() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allEnumerations() != null ) 
                {
                    foreach ( Generated.Enum otherElement in other.allEnumerations() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allStructures() != null )
            {
                if ( other.allStructures() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Structure subElement in obj.allStructures() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allStructures() != null ) 
                {
                    foreach ( Generated.Structure otherElement in other.allStructures() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allCollections() != null )
            {
                if ( other.allCollections() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Collection subElement in obj.allCollections() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allCollections() != null ) 
                {
                    foreach ( Generated.Collection otherElement in other.allCollections() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allStateMachines() != null )
            {
                if ( other.allStateMachines() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allStateMachines() != null ) 
                {
                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allFunctions() != null )
            {
                if ( other.allFunctions() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Function subElement in obj.allFunctions() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFunctions() != null ) 
                {
                    foreach ( Generated.Function otherElement in other.allFunctions() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allProcedures() != null )
            {
                if ( other.allProcedures() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Procedure subElement in obj.allProcedures() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allProcedures() != null ) 
                {
                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allVariables() != null )
            {
                if ( other.allVariables() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Variable subElement in obj.allVariables() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allVariables() != null ) 
                {
                    foreach ( Generated.Variable otherElement in other.allVariables() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allRules() != null )
            {
                if ( other.allRules() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Id, Previously was [" + other.getId() + "]");
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("CHANGED Comment, Previously was [" + other.getComment() + "]");
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
                obj.AddInfo ("CHANGED Default, Previously was [" + other.getDefault() + "]");
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

            if ( obj.allValues() != null )
            {
                if ( other.allValues() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.EnumValue subElement in obj.allValues() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allValues() != null ) 
                {
                    foreach ( Generated.EnumValue otherElement in other.allValues() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allSubEnums() != null )
            {
                if ( other.allSubEnums() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Enum subElement in obj.allSubEnums() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSubEnums() != null ) 
                {
                    foreach ( Generated.Enum otherElement in other.allSubEnums() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Value, Previously was [" + other.getValue() + "]");
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
                obj.AddInfo ("CHANGED MinValue, Previously was [" + other.getMinValue() + "]");
            }
            if ( !canonicalStringEquality(obj.getMaxValue(), other.getMaxValue()) )
            {
                obj.AddInfo ("CHANGED MaxValue, Previously was [" + other.getMaxValue() + "]");
            }
            if ( obj.allSpecialValues() != null )
            {
                if ( other.allSpecialValues() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.EnumValue subElement in obj.allSpecialValues() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSpecialValues() != null ) 
                {
                    foreach ( Generated.EnumValue otherElement in other.allSpecialValues() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.getPrecision() != other.getPrecision() )
            {
                obj.AddInfo ("CHANGED Precision, Previously was [" + other.getPrecision() + "]");
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

            if ( obj.allElements() != null )
            {
                if ( other.allElements() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.StructureElement subElement in obj.allElements() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allElements() != null ) 
                {
                    foreach ( Generated.StructureElement otherElement in other.allElements() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allProcedures() != null )
            {
                if ( other.allProcedures() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Procedure subElement in obj.allProcedures() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allProcedures() != null ) 
                {
                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allStateMachines() != null )
            {
                if ( other.allStateMachines() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allStateMachines() != null ) 
                {
                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allRules() != null )
            {
                if ( other.allRules() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED TypeName, Previously was [" + other.getTypeName() + "]");
            }
            if ( !canonicalStringEquality(obj.getDefault(), other.getDefault()) )
            {
                obj.AddInfo ("CHANGED Default, Previously was [" + other.getDefault() + "]");
            }
            if ( obj.getMode() != other.getMode() )
            {
                obj.AddInfo ("CHANGED Mode, Previously was [" + other.getMode() + "]");
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
                obj.AddInfo ("CHANGED TypeName, Previously was [" + other.getTypeName() + "]");
            }
            if ( obj.getMaxSize() != other.getMaxSize() )
            {
                obj.AddInfo ("CHANGED MaxSize, Previously was [" + other.getMaxSize() + "]");
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

            if ( obj.allParameters() != null )
            {
                if ( other.allParameters() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allParameters() != null ) 
                {
                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allCases() != null )
            {
                if ( other.allCases() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Case subElement in obj.allCases() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allCases() != null ) 
                {
                    foreach ( Generated.Case otherElement in other.allCases() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( !canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                obj.AddInfo ("CHANGED TypeName, Previously was [" + other.getTypeName() + "]");
            }
            if ( obj.getCacheable() != other.getCacheable() )
            {
                obj.AddInfo ("CHANGED Cacheable, Previously was [" + other.getCacheable() + "]");
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
                obj.AddInfo ("CHANGED TypeName, Previously was [" + other.getTypeName() + "]");
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

            if ( obj.allPreConditions() != null )
            {
                if ( other.allPreConditions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countPreConditions() && i < other.countPreConditions() )
                    {
                        ModelElement element = (ModelElement) obj.getPreConditions( i );
                        ModelElement otherElement = (ModelElement) other.getPreConditions( i );
                        if ( !canonicalStringEquality(element.Name, otherElement.Name) )
                        {
                            element.AddInfo ("CHANGED, Previously was [" + otherElement.Name + "]");
                        }
                        i += 1;
                    }
                    while ( i < obj.countPreConditions() )
                    {
                        obj.AddInfo("ADDED : " + obj.getPreConditions( i ).Name );                   
                        i += 1;
                    }
                    while ( i < other.countPreConditions() )
                    {
                        obj.AddInfo("REMOVED : " + other.getPreConditions( i ).Name );                   
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                    {
                        obj.AddInfo("ADDED : " + subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allPreConditions() != null ) 
                {
                    foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( !canonicalStringEquality(obj.getExpression(), other.getExpression()) )
            {
                obj.AddInfo ("CHANGED Expression, Previously was [" + other.getExpression() + "]");
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
                    obj.AddInfo ("REMOVED : StateMachine");
                }
            }
            else
            {
                compareStateMachine ( obj.getStateMachine(), other.getStateMachine() );
            }
            if ( obj.allRules() != null )
            {
                if ( other.allRules() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allParameters() != null )
            {
                if ( other.allParameters() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allParameters() != null ) 
                {
                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED InitialState, Previously was [" + other.getInitialState() + "]");
            }
            if ( obj.allStates() != null )
            {
                if ( other.allStates() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.State subElement in obj.allStates() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allStates() != null ) 
                {
                    foreach ( Generated.State otherElement in other.allStates() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allRules() != null )
            {
                if ( other.allRules() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                    obj.AddInfo ("REMOVED : StateMachine");
                }
            }
            else
            {
                compareStateMachine ( obj.getStateMachine(), other.getStateMachine() );
            }
            if ( obj.getWidth() != other.getWidth() )
            {
                obj.AddInfo ("CHANGED Width, Previously was [" + other.getWidth() + "]");
            }
            if ( obj.getHeight() != other.getHeight() )
            {
                obj.AddInfo ("CHANGED Height, Previously was [" + other.getHeight() + "]");
            }
            if ( obj.getX() != other.getX() )
            {
                obj.AddInfo ("CHANGED X, Previously was [" + other.getX() + "]");
            }
            if ( obj.getY() != other.getY() )
            {
                obj.AddInfo ("CHANGED Y, Previously was [" + other.getY() + "]");
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
                obj.AddInfo ("CHANGED TypeName, Previously was [" + other.getTypeName() + "]");
            }
            if ( !canonicalStringEquality(obj.getDefaultValue(), other.getDefaultValue()) )
            {
                obj.AddInfo ("CHANGED DefaultValue, Previously was [" + other.getDefaultValue() + "]");
            }
            if ( obj.getVariableMode() != other.getVariableMode() )
            {
                obj.AddInfo ("CHANGED VariableMode, Previously was [" + other.getVariableMode() + "]");
            }
            if ( obj.allSubVariables() != null )
            {
                if ( other.allSubVariables() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Variable subElement in obj.allSubVariables() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSubVariables() != null ) 
                {
                    foreach ( Generated.Variable otherElement in other.allSubVariables() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Priority, Previously was [" + other.getPriority() + "]");
            }
            if ( obj.allConditions() != null )
            {
                if ( other.allConditions() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.RuleCondition subElement in obj.allConditions() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allConditions() != null ) 
                {
                    foreach ( Generated.RuleCondition otherElement in other.allConditions() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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

            if ( obj.allPreConditions() != null )
            {
                if ( other.allPreConditions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countPreConditions() && i < other.countPreConditions() )
                    {
                        ModelElement element = (ModelElement) obj.getPreConditions( i );
                        ModelElement otherElement = (ModelElement) other.getPreConditions( i );
                        if ( !canonicalStringEquality(element.Name, otherElement.Name) )
                        {
                            element.AddInfo ("CHANGED, Previously was [" + otherElement.Name + "]");
                        }
                        i += 1;
                    }
                    while ( i < obj.countPreConditions() )
                    {
                        obj.AddInfo("ADDED : " + obj.getPreConditions( i ).Name );                   
                        i += 1;
                    }
                    while ( i < other.countPreConditions() )
                    {
                        obj.AddInfo("REMOVED : " + other.getPreConditions( i ).Name );                   
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                    {
                        obj.AddInfo("ADDED : " + subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allPreConditions() != null ) 
                {
                    foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allActions() != null )
            {
                if ( other.allActions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countActions() && i < other.countActions() )
                    {
                        ModelElement element = (ModelElement) obj.getActions( i );
                        ModelElement otherElement = (ModelElement) other.getActions( i );
                        if ( !canonicalStringEquality(element.Name, otherElement.Name) )
                        {
                            element.AddInfo ("CHANGED, Previously was [" + otherElement.Name + "]");
                        }
                        i += 1;
                    }
                    while ( i < obj.countActions() )
                    {
                        obj.AddInfo("ADDED : " + obj.getActions( i ).Name );                   
                        i += 1;
                    }
                    while ( i < other.countActions() )
                    {
                        obj.AddInfo("REMOVED : " + other.getActions( i ).Name );                   
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Action subElement in obj.allActions() )
                    {
                        obj.AddInfo("ADDED : " + subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allActions() != null ) 
                {
                    foreach ( Generated.Action otherElement in other.allActions() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allSubRules() != null )
            {
                if ( other.allSubRules() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allSubRules() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSubRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allSubRules() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Condition, Previously was [" + other.getCondition() + "]");
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
                obj.AddInfo ("CHANGED Expression, Previously was [" + other.getExpression() + "]");
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
                obj.AddInfo ("CHANGED CycleDuration, Previously was [" + other.getCycleDuration() + "]");
            }
            if ( obj.allSubSequences() != null )
            {
                if ( other.allSubSequences() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SubSequence subElement in obj.allSubSequences() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSubSequences() != null ) 
                {
                    foreach ( Generated.SubSequence otherElement in other.allSubSequences() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED D_LRBG, Previously was [" + other.getD_LRBG() + "]");
            }
            if ( !canonicalStringEquality(obj.getLevel(), other.getLevel()) )
            {
                obj.AddInfo ("CHANGED Level, Previously was [" + other.getLevel() + "]");
            }
            if ( !canonicalStringEquality(obj.getMode(), other.getMode()) )
            {
                obj.AddInfo ("CHANGED Mode, Previously was [" + other.getMode() + "]");
            }
            if ( !canonicalStringEquality(obj.getNID_LRBG(), other.getNID_LRBG()) )
            {
                obj.AddInfo ("CHANGED NID_LRBG, Previously was [" + other.getNID_LRBG() + "]");
            }
            if ( !canonicalStringEquality(obj.getQ_DIRLRBG(), other.getQ_DIRLRBG()) )
            {
                obj.AddInfo ("CHANGED Q_DIRLRBG, Previously was [" + other.getQ_DIRLRBG() + "]");
            }
            if ( !canonicalStringEquality(obj.getQ_DIRTRAIN(), other.getQ_DIRTRAIN()) )
            {
                obj.AddInfo ("CHANGED Q_DIRTRAIN, Previously was [" + other.getQ_DIRTRAIN() + "]");
            }
            if ( !canonicalStringEquality(obj.getQ_DLRBG(), other.getQ_DLRBG()) )
            {
                obj.AddInfo ("CHANGED Q_DLRBG, Previously was [" + other.getQ_DLRBG() + "]");
            }
            if ( !canonicalStringEquality(obj.getRBC_ID(), other.getRBC_ID()) )
            {
                obj.AddInfo ("CHANGED RBC_ID, Previously was [" + other.getRBC_ID() + "]");
            }
            if ( !canonicalStringEquality(obj.getRBCPhone(), other.getRBCPhone()) )
            {
                obj.AddInfo ("CHANGED RBCPhone, Previously was [" + other.getRBCPhone() + "]");
            }
            if ( obj.allTestCases() != null )
            {
                if ( other.allTestCases() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.TestCase subElement in obj.allTestCases() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allTestCases() != null ) 
                {
                    foreach ( Generated.TestCase otherElement in other.allTestCases() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Feature, Previously was [" + other.getFeature() + "]");
            }
            if ( obj.getCase() != other.getCase() )
            {
                obj.AddInfo ("CHANGED Case, Previously was [" + other.getCase() + "]");
            }
            if ( obj.allSteps() != null )
            {
                if ( other.allSteps() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Step subElement in obj.allSteps() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSteps() != null ) 
                {
                    foreach ( Generated.Step otherElement in other.allSteps() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED TCS_Order, Previously was [" + other.getTCS_Order() + "]");
            }
            if ( obj.getDistance() != other.getDistance() )
            {
                obj.AddInfo ("CHANGED Distance, Previously was [" + other.getDistance() + "]");
            }
            if ( !canonicalStringEquality(obj.getDescription(), other.getDescription()) )
            {
                obj.AddInfo ("CHANGED Description, Previously was [" + other.getDescription() + "]");
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("CHANGED Comment, Previously was [" + other.getComment() + "]");
            }
            if ( !canonicalStringEquality(obj.getUserComment(), other.getUserComment()) )
            {
                obj.AddInfo ("CHANGED UserComment, Previously was [" + other.getUserComment() + "]");
            }
            if ( obj.getIO() != other.getIO() )
            {
                obj.AddInfo ("CHANGED IO, Previously was [" + other.getIO() + "]");
            }
            if ( obj.getInterface() != other.getInterface() )
            {
                obj.AddInfo ("CHANGED Interface, Previously was [" + other.getInterface() + "]");
            }
            if ( obj.getLevelIN() != other.getLevelIN() )
            {
                obj.AddInfo ("CHANGED LevelIN, Previously was [" + other.getLevelIN() + "]");
            }
            if ( obj.getLevelOUT() != other.getLevelOUT() )
            {
                obj.AddInfo ("CHANGED LevelOUT, Previously was [" + other.getLevelOUT() + "]");
            }
            if ( obj.getModeIN() != other.getModeIN() )
            {
                obj.AddInfo ("CHANGED ModeIN, Previously was [" + other.getModeIN() + "]");
            }
            if ( obj.getModeOUT() != other.getModeOUT() )
            {
                obj.AddInfo ("CHANGED ModeOUT, Previously was [" + other.getModeOUT() + "]");
            }
            if ( obj.getTranslationRequired() != other.getTranslationRequired() )
            {
                obj.AddInfo ("CHANGED TranslationRequired, Previously was [" + other.getTranslationRequired() + "]");
            }
            if ( obj.getTranslated() != other.getTranslated() )
            {
                obj.AddInfo ("CHANGED Translated, Previously was [" + other.getTranslated() + "]");
            }
            if ( obj.allSubSteps() != null )
            {
                if ( other.allSubSteps() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSubSteps() != null ) 
                {
                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allMessages() != null )
            {
                if ( other.allMessages() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBMessage subElement in obj.allMessages() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allMessages() != null ) 
                {
                    foreach ( Generated.DBMessage otherElement in other.allMessages() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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

            if ( obj.allActions() != null )
            {
                if ( other.allActions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countActions() && i < other.countActions() )
                    {
                        ModelElement element = (ModelElement) obj.getActions( i );
                        ModelElement otherElement = (ModelElement) other.getActions( i );
                        if ( !canonicalStringEquality(element.Name, otherElement.Name) )
                        {
                            element.AddInfo ("CHANGED, Previously was [" + otherElement.Name + "]");
                        }
                        i += 1;
                    }
                    while ( i < obj.countActions() )
                    {
                        obj.AddInfo("ADDED : " + obj.getActions( i ).Name );                   
                        i += 1;
                    }
                    while ( i < other.countActions() )
                    {
                        obj.AddInfo("REMOVED : " + other.getActions( i ).Name );                   
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Action subElement in obj.allActions() )
                    {
                        obj.AddInfo("ADDED : " + subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allActions() != null ) 
                {
                    foreach ( Generated.Action otherElement in other.allActions() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allExpectations() != null )
            {
                if ( other.allExpectations() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countExpectations() && i < other.countExpectations() )
                    {
                        ModelElement element = (ModelElement) obj.getExpectations( i );
                        ModelElement otherElement = (ModelElement) other.getExpectations( i );
                        if ( !canonicalStringEquality(element.Name, otherElement.Name) )
                        {
                            element.AddInfo ("CHANGED, Previously was [" + otherElement.Name + "]");
                        }
                        i += 1;
                    }
                    while ( i < obj.countExpectations() )
                    {
                        obj.AddInfo("ADDED : " + obj.getExpectations( i ).Name );                   
                        i += 1;
                    }
                    while ( i < other.countExpectations() )
                    {
                        obj.AddInfo("REMOVED : " + other.getExpectations( i ).Name );                   
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Expectation subElement in obj.allExpectations() )
                    {
                        obj.AddInfo("ADDED : " + subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allExpectations() != null ) 
                {
                    foreach ( Generated.Expectation otherElement in other.allExpectations() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.getSkipEngine() != other.getSkipEngine() )
            {
                obj.AddInfo ("CHANGED SkipEngine, Previously was [" + other.getSkipEngine() + "]");
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
                obj.AddInfo ("CHANGED Value, Previously was [" + other.getValue() + "]");
            }
            if ( obj.getBlocking() != other.getBlocking() )
            {
                obj.AddInfo ("CHANGED Blocking, Previously was [" + other.getBlocking() + "]");
            }
            if ( obj.getKind() != other.getKind() )
            {
                obj.AddInfo ("CHANGED Kind, Previously was [" + other.getKind() + "]");
            }
            if ( obj.getDeadLine() != other.getDeadLine() )
            {
                obj.AddInfo ("CHANGED DeadLine, Previously was [" + other.getDeadLine() + "]");
            }
            if ( !canonicalStringEquality(obj.getCondition(), other.getCondition()) )
            {
                obj.AddInfo ("CHANGED Condition, Previously was [" + other.getCondition() + "]");
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
                obj.AddInfo ("CHANGED MessageOrder, Previously was [" + other.getMessageOrder() + "]");
            }
            if ( obj.getMessageType() != other.getMessageType() )
            {
                obj.AddInfo ("CHANGED MessageType, Previously was [" + other.getMessageType() + "]");
            }
            if ( obj.allFields() != null )
            {
                if ( other.allFields() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFields() != null ) 
                {
                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allPackets() != null )
            {
                if ( other.allPackets() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBPacket subElement in obj.allPackets() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allPackets() != null ) 
                {
                    foreach ( Generated.DBPacket otherElement in other.allPackets() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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

            if ( obj.allFields() != null )
            {
                if ( other.allFields() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFields() != null ) 
                {
                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Variable, Previously was [" + other.getVariable() + "]");
            }
            if ( obj.getValue() != other.getValue() )
            {
                obj.AddInfo ("CHANGED Value, Previously was [" + other.getValue() + "]");
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

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allTranslations() != null )
            {
                if ( other.allTranslations() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Translation subElement in obj.allTranslations() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allTranslations() != null ) 
                {
                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allTranslations() != null )
            {
                if ( other.allTranslations() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Translation subElement in obj.allTranslations() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allTranslations() != null ) 
                {
                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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

            if ( obj.allSourceTexts() != null )
            {
                if ( other.allSourceTexts() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSourceTexts() != null ) 
                {
                    foreach ( Generated.SourceText otherElement in other.allSourceTexts() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.getImplemented() != other.getImplemented() )
            {
                obj.AddInfo ("CHANGED Implemented, Previously was [" + other.getImplemented() + "]");
            }
            if ( obj.allSubSteps() != null )
            {
                if ( other.allSubSteps() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allSubSteps() != null ) 
                {
                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( !canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                obj.AddInfo ("CHANGED Comment, Previously was [" + other.getComment() + "]");
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

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allShortcuts() != null )
            {
                if ( other.allShortcuts() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allShortcuts() != null ) 
                {
                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allShortcuts() != null )
            {
                if ( other.allShortcuts() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allShortcuts() != null ) 
                {
                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED ShortcutName, Previously was [" + other.getShortcutName() + "]");
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
                obj.AddInfo ("CHANGED Version, Previously was [" + other.getVersion() + "]");
            }
            if ( obj.allChapters() != null )
            {
                if ( other.allChapters() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Chapter subElement in obj.allChapters() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allChapters() != null ) 
                {
                    foreach ( Generated.Chapter otherElement in other.allChapters() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.allChapterRefs() != null )
            {
                if ( other.allChapterRefs() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ChapterRef subElement in obj.allChapterRefs() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allChapterRefs() != null ) 
                {
                    foreach ( Generated.ChapterRef otherElement in other.allChapterRefs() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Id, Previously was [" + other.getId() + "]");
            }
            if ( obj.allParagraphs() != null )
            {
                if ( other.allParagraphs() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allParagraphs() != null ) 
                {
                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
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
                obj.AddInfo ("CHANGED Id, Previously was [" + other.getId() + "]");
            }
            if ( obj.getType() != other.getType() )
            {
                obj.AddInfo ("CHANGED Type, Previously was [" + other.getType() + "]");
            }
            if ( obj.getScope() != other.getScope() )
            {
                obj.AddInfo ("CHANGED Scope, Previously was [" + other.getScope() + "]");
            }
            if ( !canonicalStringEquality(obj.getBl(), other.getBl()) )
            {
                obj.AddInfo ("CHANGED Bl, Previously was [" + other.getBl() + "]");
            }
            if ( obj.getOptional() != other.getOptional() )
            {
                obj.AddInfo ("CHANGED Optional, Previously was [" + other.getOptional() + "]");
            }
            if ( !canonicalStringEquality(obj.getText(), other.getText()) )
            {
                obj.AddInfo ("CHANGED Text, Previously was [" + other.getText() + "]");
            }
            if ( !canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                obj.AddInfo ("CHANGED Version, Previously was [" + other.getVersion() + "]");
            }
            if ( obj.getReviewed() != other.getReviewed() )
            {
                obj.AddInfo ("CHANGED Reviewed, Previously was [" + other.getReviewed() + "]");
            }
            if ( obj.getImplementationStatus() != other.getImplementationStatus() )
            {
                obj.AddInfo ("CHANGED ImplementationStatus, Previously was [" + other.getImplementationStatus() + "]");
            }
            if ( obj.allParagraphs() != null )
            {
                if ( other.allParagraphs() != null ) 
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
                            obj.AddInfo("ADDED : "+ subElement.Name );                   
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
                            obj.AddInfo("REMOVED : " + otherElement.Name );                   
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                    {
                        obj.AddInfo("ADDED : "+ subElement.Name );                   
                    }
                }
            }
            else 
            {
                if ( other.allParagraphs() != null ) 
                {
                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        obj.AddInfo("REMOVED : " + otherElement.Name );                   
                    }
                }
            }
            if ( obj.getRevision() == null )
            {
                if ( other.getRevision() != null )
                {
                    obj.AddInfo ("REMOVED : Revision");
                }
            }
            else
            {
                compareParagraphRevision ( obj.getRevision(), other.getRevision() );
            }
            if ( obj.getMoreInfoRequired() != other.getMoreInfoRequired() )
            {
                obj.AddInfo ("CHANGED MoreInfoRequired, Previously was [" + other.getMoreInfoRequired() + "]");
            }
            if ( obj.getSpecIssue() != other.getSpecIssue() )
            {
                obj.AddInfo ("CHANGED SpecIssue, Previously was [" + other.getSpecIssue() + "]");
            }
            if ( obj.getFunctionalBlock() != other.getFunctionalBlock() )
            {
                obj.AddInfo ("CHANGED FunctionalBlock, Previously was [" + other.getFunctionalBlock() + "]");
            }
            if ( !canonicalStringEquality(obj.getFunctionalBlockName(), other.getFunctionalBlockName()) )
            {
                obj.AddInfo ("CHANGED FunctionalBlockName, Previously was [" + other.getFunctionalBlockName() + "]");
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
                obj.AddInfo ("CHANGED Text, Previously was [" + other.getText() + "]");
            }
            if ( !canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                obj.AddInfo ("CHANGED Version, Previously was [" + other.getVersion() + "]");
            }
        }

    }
}
