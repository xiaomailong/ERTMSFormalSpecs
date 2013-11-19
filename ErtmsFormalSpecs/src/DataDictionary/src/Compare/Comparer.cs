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

namespace DataDictionary.Compare
{
    using System.Collections.Generic;
    public static class Comparer 
    {
        /// <summary>
        /// Compares two Namable and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareNamable(Generated.Namable obj, Generated.Namable other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            if ( !CompareUtil.canonicalStringEquality(obj.getName(), other.getName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Name", "Previously was [" + other.getName() + "]") );
            }
        }

        /// <summary>
        /// Compares two ReferencesParagraph and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareReferencesParagraph(Generated.ReferencesParagraph obj, Generated.ReferencesParagraph other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allRequirements() != null )
            {
                if ( other.allRequirements() != null ) 
                {
                    foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                    {
                        bool compared = false;
                        foreach ( Generated.ReqRef otherElement in other.allRequirements() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareReqRef ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.ReqRef otherElement in other.allRequirements() )
                    {
                        bool found = false;
                        foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRequirements() != null ) 
                {
                    foreach ( Generated.ReqRef otherElement in other.allRequirements() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Comment", "Previously was [" + other.getComment() + "]") );
            }
        }

        /// <summary>
        /// Compares two ReqRelated and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareReqRelated(Generated.ReqRelated obj, Generated.ReqRelated other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReferencesParagraph (obj, other, diff);

            if ( obj.getImplemented() != other.getImplemented() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Implemented", "Previously was [" + other.getImplemented() + "]") );
            }
            if ( obj.getVerified() != other.getVerified() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Verified", "Previously was [" + other.getVerified() + "]") );
            }
            if ( obj.getNeedsRequirement() != other.getNeedsRequirement() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "NeedsRequirement", "Previously was [" + other.getNeedsRequirement() + "]") );
            }
        }

        /// <summary>
        /// Compares two Dictionary and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDictionary(Generated.Dictionary obj, Generated.Dictionary other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            if ( obj.getSpecification() == null )
            {
                if ( other.getSpecification() != null )
                {
                    diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Specification", "" ) );
                }
            }
            else
            {
                compareSpecification ( obj.getSpecification(), other.getSpecification(), diff );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRuleDisabling ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.RuleDisabling otherElement in other.allRuleDisablings() )
                    {
                        bool found = false;
                        foreach ( Generated.RuleDisabling subElement in obj.allRuleDisablings() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.RuleDisabling subElement in obj.allRuleDisablings() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRuleDisablings() != null ) 
                {
                    foreach ( Generated.RuleDisabling otherElement in other.allRuleDisablings() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareNameSpace ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        bool found = false;
                        foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaces() != null ) 
                {
                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareNameSpaceRef ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        bool found = false;
                        foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaceRefs() != null ) 
                {
                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareFrame ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Frame otherElement in other.allTests() )
                    {
                        bool found = false;
                        foreach ( Generated.Frame subElement in obj.allTests() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Frame subElement in obj.allTests() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allTests() != null ) 
                {
                    foreach ( Generated.Frame otherElement in other.allTests() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareFrameRef ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.FrameRef otherElement in other.allTestRefs() )
                    {
                        bool found = false;
                        foreach ( Generated.FrameRef subElement in obj.allTestRefs() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.FrameRef subElement in obj.allTestRefs() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allTestRefs() != null ) 
                {
                    foreach ( Generated.FrameRef otherElement in other.allTestRefs() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.getTranslationDictionary() == null )
            {
                if ( other.getTranslationDictionary() != null )
                {
                    diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TranslationDictionary", "" ) );
                }
            }
            else
            {
                compareTranslationDictionary ( obj.getTranslationDictionary(), other.getTranslationDictionary(), diff );
            }
            if ( obj.getShortcutDictionary() == null )
            {
                if ( other.getShortcutDictionary() != null )
                {
                    diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "ShortcutDictionary", "" ) );
                }
            }
            else
            {
                compareShortcutDictionary ( obj.getShortcutDictionary(), other.getShortcutDictionary(), diff );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getXsi(), other.getXsi()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Xsi", "Previously was [" + other.getXsi() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getXsiLocation(), other.getXsiLocation()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "XsiLocation", "Previously was [" + other.getXsiLocation() + "]") );
            }
        }

        /// <summary>
        /// Compares two RuleDisabling and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRuleDisabling(Generated.RuleDisabling obj, Generated.RuleDisabling other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getRule(), other.getRule()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Rule", "Previously was [" + other.getRule() + "]") );
            }
        }

        /// <summary>
        /// Compares two NameSpaceRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareNameSpaceRef(Generated.NameSpaceRef obj, Generated.NameSpaceRef other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

        }

        /// <summary>
        /// Compares two NameSpace and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareNameSpace(Generated.NameSpace obj, Generated.NameSpace other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allNameSpaces() != null )
            {
                if ( other.allNameSpaces() != null ) 
                {
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        bool compared = false;
                        foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareNameSpace ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        bool found = false;
                        foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaces() != null ) 
                {
                    foreach ( Generated.NameSpace otherElement in other.allNameSpaces() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareNameSpaceRef ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        bool found = false;
                        foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allNameSpaceRefs() != null ) 
                {
                    foreach ( Generated.NameSpaceRef otherElement in other.allNameSpaceRefs() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRange ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Range otherElement in other.allRanges() )
                    {
                        bool found = false;
                        foreach ( Generated.Range subElement in obj.allRanges() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Range subElement in obj.allRanges() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRanges() != null ) 
                {
                    foreach ( Generated.Range otherElement in other.allRanges() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareEnum ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Enum otherElement in other.allEnumerations() )
                    {
                        bool found = false;
                        foreach ( Generated.Enum subElement in obj.allEnumerations() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Enum subElement in obj.allEnumerations() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allEnumerations() != null ) 
                {
                    foreach ( Generated.Enum otherElement in other.allEnumerations() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareStructure ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Structure otherElement in other.allStructures() )
                    {
                        bool found = false;
                        foreach ( Generated.Structure subElement in obj.allStructures() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Structure subElement in obj.allStructures() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allStructures() != null ) 
                {
                    foreach ( Generated.Structure otherElement in other.allStructures() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareCollection ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Collection otherElement in other.allCollections() )
                    {
                        bool found = false;
                        foreach ( Generated.Collection subElement in obj.allCollections() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Collection subElement in obj.allCollections() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allCollections() != null ) 
                {
                    foreach ( Generated.Collection otherElement in other.allCollections() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareStateMachine ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        bool found = false;
                        foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allStateMachines() != null ) 
                {
                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareFunction ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Function otherElement in other.allFunctions() )
                    {
                        bool found = false;
                        foreach ( Generated.Function subElement in obj.allFunctions() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Function subElement in obj.allFunctions() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFunctions() != null ) 
                {
                    foreach ( Generated.Function otherElement in other.allFunctions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareProcedure ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        bool found = false;
                        foreach ( Generated.Procedure subElement in obj.allProcedures() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Procedure subElement in obj.allProcedures() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allProcedures() != null ) 
                {
                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareVariable ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Variable otherElement in other.allVariables() )
                    {
                        bool found = false;
                        foreach ( Generated.Variable subElement in obj.allVariables() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Variable subElement in obj.allVariables() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allVariables() != null ) 
                {
                    foreach ( Generated.Variable otherElement in other.allVariables() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRule ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        bool found = false;
                        foreach ( Generated.Rule subElement in obj.allRules() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.getWidth() != other.getWidth() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Width", "Previously was [" + other.getWidth() + "]") );
            }
            if ( obj.getHeight() != other.getHeight() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Height", "Previously was [" + other.getHeight() + "]") );
            }
            if ( obj.getX() != other.getX() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "X", "Previously was [" + other.getX() + "]") );
            }
            if ( obj.getY() != other.getY() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Y", "Previously was [" + other.getY() + "]") );
            }
            if ( obj.getHidden() != other.getHidden() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Hidden", "Previously was [" + other.getHidden() + "]") );
            }
        }

        /// <summary>
        /// Compares two ReqRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareReqRef(Generated.ReqRef obj, Generated.ReqRef other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            if ( !CompareUtil.canonicalStringEquality(obj.getId(), other.getId()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Id", "Previously was [" + other.getId() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Comment", "Previously was [" + other.getComment() + "]") );
            }
        }

        /// <summary>
        /// Compares two Type and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareType(Generated.Type obj, Generated.Type other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getDefault(), other.getDefault()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Default", "Previously was [" + other.getDefault() + "]") );
            }
        }

        /// <summary>
        /// Compares two Enum and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareEnum(Generated.Enum obj, Generated.Enum other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareType (obj, other, diff);

            if ( obj.allValues() != null )
            {
                if ( other.allValues() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countValues() && i < other.countValues() )
                    {
                        Generated.EnumValue element = obj.getValues( i );
                        Generated.EnumValue otherElement = other.getValues( i );
                        compareEnumValue ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countValues() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getValues(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countValues() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getValues( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.EnumValue subElement in obj.allValues() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allValues() != null ) 
                {
                    foreach ( Generated.EnumValue otherElement in other.allValues() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareEnum ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Enum otherElement in other.allSubEnums() )
                    {
                        bool found = false;
                        foreach ( Generated.Enum subElement in obj.allSubEnums() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Enum subElement in obj.allSubEnums() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSubEnums() != null ) 
                {
                    foreach ( Generated.Enum otherElement in other.allSubEnums() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two EnumValue and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareEnumValue(Generated.EnumValue obj, Generated.EnumValue other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getValue(), other.getValue()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Value", "Previously was [" + other.getValue() + "]") );
            }
        }

        /// <summary>
        /// Compares two Range and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRange(Generated.Range obj, Generated.Range other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareType (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getMinValue(), other.getMinValue()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "MinValue", "Previously was [" + other.getMinValue() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getMaxValue(), other.getMaxValue()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "MaxValue", "Previously was [" + other.getMaxValue() + "]") );
            }
            if ( obj.allSpecialValues() != null )
            {
                if ( other.allSpecialValues() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countSpecialValues() && i < other.countSpecialValues() )
                    {
                        Generated.EnumValue element = obj.getSpecialValues( i );
                        Generated.EnumValue otherElement = other.getSpecialValues( i );
                        compareEnumValue ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countSpecialValues() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getSpecialValues(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countSpecialValues() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getSpecialValues( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.EnumValue subElement in obj.allSpecialValues() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSpecialValues() != null ) 
                {
                    foreach ( Generated.EnumValue otherElement in other.allSpecialValues() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.getPrecision() != other.getPrecision() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Precision", "Previously was [" + other.getPrecision() + "]") );
            }
        }

        /// <summary>
        /// Compares two Structure and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStructure(Generated.Structure obj, Generated.Structure other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareType (obj, other, diff);

            if ( obj.allElements() != null )
            {
                if ( other.allElements() != null ) 
                {
                    foreach ( Generated.StructureElement subElement in obj.allElements() )
                    {
                        bool compared = false;
                        foreach ( Generated.StructureElement otherElement in other.allElements() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareStructureElement ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.StructureElement otherElement in other.allElements() )
                    {
                        bool found = false;
                        foreach ( Generated.StructureElement subElement in obj.allElements() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.StructureElement subElement in obj.allElements() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allElements() != null ) 
                {
                    foreach ( Generated.StructureElement otherElement in other.allElements() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareProcedure ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        bool found = false;
                        foreach ( Generated.Procedure subElement in obj.allProcedures() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Procedure subElement in obj.allProcedures() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allProcedures() != null ) 
                {
                    foreach ( Generated.Procedure otherElement in other.allProcedures() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareStateMachine ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        bool found = false;
                        foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allStateMachines() != null ) 
                {
                    foreach ( Generated.StateMachine otherElement in other.allStateMachines() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRule ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        bool found = false;
                        foreach ( Generated.Rule subElement in obj.allRules() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two StructureElement and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStructureElement(Generated.StructureElement obj, Generated.StructureElement other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TypeName", "Previously was [" + other.getTypeName() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getDefault(), other.getDefault()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Default", "Previously was [" + other.getDefault() + "]") );
            }
            if ( obj.getMode() != other.getMode() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Mode", "Previously was [" + other.getMode() + "]") );
            }
        }

        /// <summary>
        /// Compares two Collection and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareCollection(Generated.Collection obj, Generated.Collection other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareType (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TypeName", "Previously was [" + other.getTypeName() + "]") );
            }
            if ( obj.getMaxSize() != other.getMaxSize() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "MaxSize", "Previously was [" + other.getMaxSize() + "]") );
            }
        }

        /// <summary>
        /// Compares two Function and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFunction(Generated.Function obj, Generated.Function other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareType (obj, other, diff);

            if ( obj.allParameters() != null )
            {
                if ( other.allParameters() != null ) 
                {
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        bool compared = false;
                        foreach ( Generated.Parameter otherElement in other.allParameters() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareParameter ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        bool found = false;
                        foreach ( Generated.Parameter subElement in obj.allParameters() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allParameters() != null ) 
                {
                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.allCases() != null )
            {
                if ( other.allCases() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countCases() && i < other.countCases() )
                    {
                        Generated.Case element = obj.getCases( i );
                        Generated.Case otherElement = other.getCases( i );
                        compareCase ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countCases() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getCases(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countCases() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getCases( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Case subElement in obj.allCases() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allCases() != null ) 
                {
                    foreach ( Generated.Case otherElement in other.allCases() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TypeName", "Previously was [" + other.getTypeName() + "]") );
            }
            if ( obj.getCacheable() != other.getCacheable() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Cacheable", "Previously was [" + other.getCacheable() + "]") );
            }
        }

        /// <summary>
        /// Compares two Parameter and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareParameter(Generated.Parameter obj, Generated.Parameter other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TypeName", "Previously was [" + other.getTypeName() + "]") );
            }
        }

        /// <summary>
        /// Compares two Case and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareCase(Generated.Case obj, Generated.Case other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allPreConditions() != null )
            {
                if ( other.allPreConditions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countPreConditions() && i < other.countPreConditions() )
                    {
                        Generated.PreCondition element = obj.getPreConditions( i );
                        Generated.PreCondition otherElement = other.getPreConditions( i );
                        comparePreCondition ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getPreConditions(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getPreConditions( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allPreConditions() != null ) 
                {
                    foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getExpression(), other.getExpression()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Expression", "Previously was [" + other.getExpression() + "]") );
            }
        }

        /// <summary>
        /// Compares two Procedure and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareProcedure(Generated.Procedure obj, Generated.Procedure other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( obj.getStateMachine() == null )
            {
                if ( other.getStateMachine() != null )
                {
                    diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "StateMachine", "" ) );
                }
            }
            else
            {
                compareStateMachine ( obj.getStateMachine(), other.getStateMachine(), diff );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRule ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        bool found = false;
                        foreach ( Generated.Rule subElement in obj.allRules() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareParameter ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        bool found = false;
                        foreach ( Generated.Parameter subElement in obj.allParameters() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Parameter subElement in obj.allParameters() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allParameters() != null ) 
                {
                    foreach ( Generated.Parameter otherElement in other.allParameters() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two StateMachine and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStateMachine(Generated.StateMachine obj, Generated.StateMachine other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareType (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getInitialState(), other.getInitialState()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "InitialState", "Previously was [" + other.getInitialState() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareState ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.State otherElement in other.allStates() )
                    {
                        bool found = false;
                        foreach ( Generated.State subElement in obj.allStates() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.State subElement in obj.allStates() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allStates() != null ) 
                {
                    foreach ( Generated.State otherElement in other.allStates() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRule ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        bool found = false;
                        foreach ( Generated.Rule subElement in obj.allRules() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allRules() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two State and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareState(Generated.State obj, Generated.State other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( obj.getStateMachine() == null )
            {
                if ( other.getStateMachine() != null )
                {
                    diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "StateMachine", "" ) );
                }
            }
            else
            {
                compareStateMachine ( obj.getStateMachine(), other.getStateMachine(), diff );
            }
            if ( obj.getWidth() != other.getWidth() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Width", "Previously was [" + other.getWidth() + "]") );
            }
            if ( obj.getHeight() != other.getHeight() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Height", "Previously was [" + other.getHeight() + "]") );
            }
            if ( obj.getX() != other.getX() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "X", "Previously was [" + other.getX() + "]") );
            }
            if ( obj.getY() != other.getY() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Y", "Previously was [" + other.getY() + "]") );
            }
        }

        /// <summary>
        /// Compares two Variable and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareVariable(Generated.Variable obj, Generated.Variable other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getTypeName(), other.getTypeName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TypeName", "Previously was [" + other.getTypeName() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getDefaultValue(), other.getDefaultValue()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "DefaultValue", "Previously was [" + other.getDefaultValue() + "]") );
            }
            if ( obj.getVariableMode() != other.getVariableMode() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "VariableMode", "Previously was [" + other.getVariableMode() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareVariable ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Variable otherElement in other.allSubVariables() )
                    {
                        bool found = false;
                        foreach ( Generated.Variable subElement in obj.allSubVariables() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Variable subElement in obj.allSubVariables() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSubVariables() != null ) 
                {
                    foreach ( Generated.Variable otherElement in other.allSubVariables() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two Rule and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRule(Generated.Rule obj, Generated.Rule other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( obj.getPriority() != other.getPriority() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Priority", "Previously was [" + other.getPriority() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRuleCondition ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.RuleCondition otherElement in other.allConditions() )
                    {
                        bool found = false;
                        foreach ( Generated.RuleCondition subElement in obj.allConditions() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.RuleCondition subElement in obj.allConditions() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allConditions() != null ) 
                {
                    foreach ( Generated.RuleCondition otherElement in other.allConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two RuleCondition and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareRuleCondition(Generated.RuleCondition obj, Generated.RuleCondition other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( obj.allPreConditions() != null )
            {
                if ( other.allPreConditions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countPreConditions() && i < other.countPreConditions() )
                    {
                        Generated.PreCondition element = obj.getPreConditions( i );
                        Generated.PreCondition otherElement = other.getPreConditions( i );
                        comparePreCondition ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getPreConditions(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getPreConditions( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allPreConditions() != null ) 
                {
                    foreach ( Generated.PreCondition otherElement in other.allPreConditions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                        Generated.Action element = obj.getActions( i );
                        Generated.Action otherElement = other.getActions( i );
                        compareAction ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countActions() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getActions(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countActions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getActions( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Action subElement in obj.allActions() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allActions() != null ) 
                {
                    foreach ( Generated.Action otherElement in other.allActions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareRule ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Rule otherElement in other.allSubRules() )
                    {
                        bool found = false;
                        foreach ( Generated.Rule subElement in obj.allSubRules() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Rule subElement in obj.allSubRules() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSubRules() != null ) 
                {
                    foreach ( Generated.Rule otherElement in other.allSubRules() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two PreCondition and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void comparePreCondition(Generated.PreCondition obj, Generated.PreCondition other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            if ( !CompareUtil.canonicalStringEquality(obj.getCondition(), other.getCondition()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Condition", "Previously was [" + other.getCondition() + "]") );
            }
        }

        /// <summary>
        /// Compares two Action and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareAction(Generated.Action obj, Generated.Action other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            if ( !CompareUtil.canonicalStringEquality(obj.getExpression(), other.getExpression()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Expression", "Previously was [" + other.getExpression() + "]") );
            }
        }

        /// <summary>
        /// Compares two FrameRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFrameRef(Generated.FrameRef obj, Generated.FrameRef other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

        }

        /// <summary>
        /// Compares two Frame and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFrame(Generated.Frame obj, Generated.Frame other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getCycleDuration(), other.getCycleDuration()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "CycleDuration", "Previously was [" + other.getCycleDuration() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareSubSequence ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.SubSequence otherElement in other.allSubSequences() )
                    {
                        bool found = false;
                        foreach ( Generated.SubSequence subElement in obj.allSubSequences() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SubSequence subElement in obj.allSubSequences() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSubSequences() != null ) 
                {
                    foreach ( Generated.SubSequence otherElement in other.allSubSequences() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two SubSequence and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSubSequence(Generated.SubSequence obj, Generated.SubSequence other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getD_LRBG(), other.getD_LRBG()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "D_LRBG", "Previously was [" + other.getD_LRBG() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getLevel(), other.getLevel()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Level", "Previously was [" + other.getLevel() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getMode(), other.getMode()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Mode", "Previously was [" + other.getMode() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getNID_LRBG(), other.getNID_LRBG()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "NID_LRBG", "Previously was [" + other.getNID_LRBG() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getQ_DIRLRBG(), other.getQ_DIRLRBG()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Q_DIRLRBG", "Previously was [" + other.getQ_DIRLRBG() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getQ_DIRTRAIN(), other.getQ_DIRTRAIN()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Q_DIRTRAIN", "Previously was [" + other.getQ_DIRTRAIN() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getQ_DLRBG(), other.getQ_DLRBG()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Q_DLRBG", "Previously was [" + other.getQ_DLRBG() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getRBC_ID(), other.getRBC_ID()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "RBC_ID", "Previously was [" + other.getRBC_ID() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getRBCPhone(), other.getRBCPhone()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "RBCPhone", "Previously was [" + other.getRBCPhone() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareTestCase ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.TestCase otherElement in other.allTestCases() )
                    {
                        bool found = false;
                        foreach ( Generated.TestCase subElement in obj.allTestCases() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.TestCase subElement in obj.allTestCases() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allTestCases() != null ) 
                {
                    foreach ( Generated.TestCase otherElement in other.allTestCases() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two TestCase and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareTestCase(Generated.TestCase obj, Generated.TestCase other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReqRelated (obj, other, diff);

            if ( obj.getFeature() != other.getFeature() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Feature", "Previously was [" + other.getFeature() + "]") );
            }
            if ( obj.getCase() != other.getCase() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Case", "Previously was [" + other.getCase() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareStep ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Step otherElement in other.allSteps() )
                    {
                        bool found = false;
                        foreach ( Generated.Step subElement in obj.allSteps() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Step subElement in obj.allSteps() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSteps() != null ) 
                {
                    foreach ( Generated.Step otherElement in other.allSteps() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two Step and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareStep(Generated.Step obj, Generated.Step other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.getTCS_Order() != other.getTCS_Order() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TCS_Order", "Previously was [" + other.getTCS_Order() + "]") );
            }
            if ( obj.getDistance() != other.getDistance() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Distance", "Previously was [" + other.getDistance() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getDescription(), other.getDescription()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Description", "Previously was [" + other.getDescription() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Comment", "Previously was [" + other.getComment() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getUserComment(), other.getUserComment()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "UserComment", "Previously was [" + other.getUserComment() + "]") );
            }
            if ( obj.getIO() != other.getIO() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "IO", "Previously was [" + other.getIO() + "]") );
            }
            if ( obj.getInterface() != other.getInterface() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Interface", "Previously was [" + other.getInterface() + "]") );
            }
            if ( obj.getLevelIN() != other.getLevelIN() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "LevelIN", "Previously was [" + other.getLevelIN() + "]") );
            }
            if ( obj.getLevelOUT() != other.getLevelOUT() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "LevelOUT", "Previously was [" + other.getLevelOUT() + "]") );
            }
            if ( obj.getModeIN() != other.getModeIN() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "ModeIN", "Previously was [" + other.getModeIN() + "]") );
            }
            if ( obj.getModeOUT() != other.getModeOUT() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "ModeOUT", "Previously was [" + other.getModeOUT() + "]") );
            }
            if ( obj.getTranslationRequired() != other.getTranslationRequired() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "TranslationRequired", "Previously was [" + other.getTranslationRequired() + "]") );
            }
            if ( obj.getTranslated() != other.getTranslated() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Translated", "Previously was [" + other.getTranslated() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareSubStep ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        bool found = false;
                        foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSubSteps() != null ) 
                {
                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareDBMessage ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.DBMessage otherElement in other.allMessages() )
                    {
                        bool found = false;
                        foreach ( Generated.DBMessage subElement in obj.allMessages() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBMessage subElement in obj.allMessages() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allMessages() != null ) 
                {
                    foreach ( Generated.DBMessage otherElement in other.allMessages() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two SubStep and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSubStep(Generated.SubStep obj, Generated.SubStep other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allActions() != null )
            {
                if ( other.allActions() != null ) 
                {
                    int i = 0;
                    while ( i < obj.countActions() && i < other.countActions() )
                    {
                        Generated.Action element = obj.getActions( i );
                        Generated.Action otherElement = other.getActions( i );
                        compareAction ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countActions() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getActions(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countActions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getActions( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Action subElement in obj.allActions() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allActions() != null ) 
                {
                    foreach ( Generated.Action otherElement in other.allActions() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                        Generated.Expectation element = obj.getExpectations( i );
                        Generated.Expectation otherElement = other.getExpectations( i );
                        compareExpectation ( element, otherElement, diff );
                        i += 1;
                    }
                    while ( i < obj.countExpectations() )
                    {
                        diff.Diffs.Add ( new Diff(obj.getExpectations(i), Diff.ActionEnum.Add) );
                        i += 1;
                    }
                    while ( i < other.countExpectations() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", other.getExpectations( i ).Name) );
                        i += 1;
                    }
                }
                else 
                {
                    foreach ( Generated.Expectation subElement in obj.allExpectations() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allExpectations() != null ) 
                {
                    foreach ( Generated.Expectation otherElement in other.allExpectations() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.getSkipEngine() != other.getSkipEngine() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "SkipEngine", "Previously was [" + other.getSkipEngine() + "]") );
            }
        }

        /// <summary>
        /// Compares two Expectation and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareExpectation(Generated.Expectation obj, Generated.Expectation other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getValue(), other.getValue()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Value", "Previously was [" + other.getValue() + "]") );
            }
            if ( obj.getBlocking() != other.getBlocking() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Blocking", "Previously was [" + other.getBlocking() + "]") );
            }
            if ( obj.getKind() != other.getKind() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Kind", "Previously was [" + other.getKind() + "]") );
            }
            if ( obj.getDeadLine() != other.getDeadLine() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "DeadLine", "Previously was [" + other.getDeadLine() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getCondition(), other.getCondition()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Condition", "Previously was [" + other.getCondition() + "]") );
            }
        }

        /// <summary>
        /// Compares two DBMessage and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDBMessage(Generated.DBMessage obj, Generated.DBMessage other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.getMessageOrder() != other.getMessageOrder() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "MessageOrder", "Previously was [" + other.getMessageOrder() + "]") );
            }
            if ( obj.getMessageType() != other.getMessageType() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "MessageType", "Previously was [" + other.getMessageType() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareDBField ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        bool found = false;
                        foreach ( Generated.DBField subElement in obj.allFields() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFields() != null ) 
                {
                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareDBPacket ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.DBPacket otherElement in other.allPackets() )
                    {
                        bool found = false;
                        foreach ( Generated.DBPacket subElement in obj.allPackets() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBPacket subElement in obj.allPackets() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allPackets() != null ) 
                {
                    foreach ( Generated.DBPacket otherElement in other.allPackets() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two DBPacket and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDBPacket(Generated.DBPacket obj, Generated.DBPacket other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allFields() != null )
            {
                if ( other.allFields() != null ) 
                {
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        bool compared = false;
                        foreach ( Generated.DBField otherElement in other.allFields() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareDBField ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        bool found = false;
                        foreach ( Generated.DBField subElement in obj.allFields() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.DBField subElement in obj.allFields() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFields() != null ) 
                {
                    foreach ( Generated.DBField otherElement in other.allFields() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two DBField and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareDBField(Generated.DBField obj, Generated.DBField other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getVariable(), other.getVariable()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Variable", "Previously was [" + other.getVariable() + "]") );
            }
            if ( obj.getValue() != other.getValue() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Value", "Previously was [" + other.getValue() + "]") );
            }
        }

        /// <summary>
        /// Compares two TranslationDictionary and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareTranslationDictionary(Generated.TranslationDictionary obj, Generated.TranslationDictionary other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        bool compared = false;
                        foreach ( Generated.Folder otherElement in other.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareFolder ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        bool found = false;
                        foreach ( Generated.Folder subElement in obj.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareTranslation ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        bool found = false;
                        foreach ( Generated.Translation subElement in obj.allTranslations() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Translation subElement in obj.allTranslations() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allTranslations() != null ) 
                {
                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two Folder and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareFolder(Generated.Folder obj, Generated.Folder other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        bool compared = false;
                        foreach ( Generated.Folder otherElement in other.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareFolder ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        bool found = false;
                        foreach ( Generated.Folder subElement in obj.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Folder subElement in obj.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.Folder otherElement in other.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareTranslation ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        bool found = false;
                        foreach ( Generated.Translation subElement in obj.allTranslations() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Translation subElement in obj.allTranslations() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allTranslations() != null ) 
                {
                    foreach ( Generated.Translation otherElement in other.allTranslations() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two Translation and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareTranslation(Generated.Translation obj, Generated.Translation other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allSourceTexts() != null )
            {
                if ( other.allSourceTexts() != null ) 
                {
                    foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                    {
                        bool compared = false;
                        foreach ( Generated.SourceText otherElement in other.allSourceTexts() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareSourceText ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.SourceText otherElement in other.allSourceTexts() )
                    {
                        bool found = false;
                        foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSourceTexts() != null ) 
                {
                    foreach ( Generated.SourceText otherElement in other.allSourceTexts() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.getImplemented() != other.getImplemented() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Implemented", "Previously was [" + other.getImplemented() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareSubStep ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        bool found = false;
                        foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allSubSteps() != null ) 
                {
                    foreach ( Generated.SubStep otherElement in other.allSubSteps() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getComment(), other.getComment()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Comment", "Previously was [" + other.getComment() + "]") );
            }
        }

        /// <summary>
        /// Compares two SourceText and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSourceText(Generated.SourceText obj, Generated.SourceText other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

        }

        /// <summary>
        /// Compares two ShortcutDictionary and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareShortcutDictionary(Generated.ShortcutDictionary obj, Generated.ShortcutDictionary other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        bool compared = false;
                        foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareShortcutFolder ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        bool found = false;
                        foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareShortcut ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        bool found = false;
                        foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allShortcuts() != null ) 
                {
                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two ShortcutFolder and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareShortcutFolder(Generated.ShortcutFolder obj, Generated.ShortcutFolder other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( obj.allFolders() != null )
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        bool compared = false;
                        foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareShortcutFolder ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        bool found = false;
                        foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allFolders() != null ) 
                {
                    foreach ( Generated.ShortcutFolder otherElement in other.allFolders() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareShortcut ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        bool found = false;
                        foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allShortcuts() != null ) 
                {
                    foreach ( Generated.Shortcut otherElement in other.allShortcuts() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two Shortcut and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareShortcut(Generated.Shortcut obj, Generated.Shortcut other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getShortcutName(), other.getShortcutName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "ShortcutName", "Previously was [" + other.getShortcutName() + "]") );
            }
        }

        /// <summary>
        /// Compares two Specification and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareSpecification(Generated.Specification obj, Generated.Specification other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Version", "Previously was [" + other.getVersion() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.getId(), otherElement.getId()) )
                            {
                                compareChapter ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Chapter otherElement in other.allChapters() )
                    {
                        bool found = false;
                        foreach ( Generated.Chapter subElement in obj.allChapters() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.getId(), otherElement.getId()) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Chapter subElement in obj.allChapters() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allChapters() != null ) 
                {
                    foreach ( Generated.Chapter otherElement in other.allChapters() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                compareChapterRef ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.ChapterRef otherElement in other.allChapterRefs() )
                    {
                        bool found = false;
                        foreach ( Generated.ChapterRef subElement in obj.allChapterRefs() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.Name, otherElement.Name) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.ChapterRef subElement in obj.allChapterRefs() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allChapterRefs() != null ) 
                {
                    foreach ( Generated.ChapterRef otherElement in other.allChapterRefs() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two ChapterRef and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareChapterRef(Generated.ChapterRef obj, Generated.ChapterRef other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

        }

        /// <summary>
        /// Compares two Chapter and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareChapter(Generated.Chapter obj, Generated.Chapter other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareNamable (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getId(), other.getId()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Id", "Previously was [" + other.getId() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.getId(), otherElement.getId()) )
                            {
                                compareParagraph ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        bool found = false;
                        foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.getId(), otherElement.getId()) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allParagraphs() != null ) 
                {
                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
        }

        /// <summary>
        /// Compares two Paragraph and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareParagraph(Generated.Paragraph obj, Generated.Paragraph other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            compareReferencesParagraph (obj, other, diff);

            if ( !CompareUtil.canonicalStringEquality(obj.getId(), other.getId()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Id", "Previously was [" + other.getId() + "]") );
            }
            if ( obj.getType() != other.getType() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Type", "Previously was [" + other.getType() + "]") );
            }
            if ( obj.getScope() != other.getScope() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Scope", "Previously was [" + other.getScope() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getBl(), other.getBl()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Bl", "Previously was [" + other.getBl() + "]") );
            }
            if ( obj.getOptional() != other.getOptional() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Optional", "Previously was [" + other.getOptional() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getText(), other.getText()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Text", "Previously was [" + other.getText() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Version", "Previously was [" + other.getVersion() + "]") );
            }
            if ( obj.getReviewed() != other.getReviewed() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Reviewed", "Previously was [" + other.getReviewed() + "]") );
            }
            if ( obj.getImplementationStatus() != other.getImplementationStatus() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "ImplementationStatus", "Previously was [" + other.getImplementationStatus() + "]") );
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
                            if ( CompareUtil.canonicalStringEquality(subElement.getId(), otherElement.getId()) )
                            {
                                compareParagraph ( subElement, otherElement, diff );
                                compared = true;
                            break;
                            }
                        }

                        if ( !compared ) 
                        {
                            diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                        }
                    }

                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        bool found = false;
                        foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                        {
                            if ( CompareUtil.canonicalStringEquality(subElement.getId(), otherElement.getId()) )
                            {
                                found = true;
                                break;
                            }
                        }

                        if ( !found )
                        {
                            diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                        }
                    }
                }
                else 
                {
                    foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                    {
                        diff.Diffs.Add ( new Diff(subElement, Diff.ActionEnum.Add ) );
                    }
                }
            }
            else 
            {
                if ( other.allParagraphs() != null ) 
                {
                    foreach ( Generated.Paragraph otherElement in other.allParagraphs() )
                    {
                        diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Remove, "", otherElement.Name) );
                    }
                }
            }
            if ( obj.getRevision() == null )
            {
                if ( other.getRevision() != null )
                {
                    diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Revision", "" ) );
                }
            }
            else
            {
                compareParagraphRevision ( obj.getRevision(), other.getRevision(), diff );
            }
            if ( obj.getMoreInfoRequired() != other.getMoreInfoRequired() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "MoreInfoRequired", "Previously was [" + other.getMoreInfoRequired() + "]") );
            }
            if ( obj.getSpecIssue() != other.getSpecIssue() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "SpecIssue", "Previously was [" + other.getSpecIssue() + "]") );
            }
            if ( obj.getFunctionalBlock() != other.getFunctionalBlock() )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "FunctionalBlock", "Previously was [" + other.getFunctionalBlock() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getFunctionalBlockName(), other.getFunctionalBlockName()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "FunctionalBlockName", "Previously was [" + other.getFunctionalBlockName() + "]") );
            }
        }

        /// <summary>
        /// Compares two ParagraphRevision and annotates the differences on the first one
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="other"></param>
        public static void compareParagraphRevision(Generated.ParagraphRevision obj, Generated.ParagraphRevision other, VersionDiff diff)
        {
            if ( other == null )
            { 
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Add) );
                return;
            }

            if ( !CompareUtil.canonicalStringEquality(obj.getText(), other.getText()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Text", "Previously was [" + other.getText() + "]") );
            }
            if ( !CompareUtil.canonicalStringEquality(obj.getVersion(), other.getVersion()) )
            {
                diff.Diffs.Add ( new Diff(obj, Diff.ActionEnum.Change, "Version", "Previously was [" + other.getVersion() + "]") );
            }
        }

        /// <summary>
        /// Searches a specific string in Namable and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchNamable(Generated.Namable obj, string searchString, List<ModelElement> occurences)
        {
            if ( obj.getName() != null && obj.getName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in ReferencesParagraph and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchReferencesParagraph(Generated.ReferencesParagraph obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allRequirements() != null )
            {
                foreach ( Generated.ReqRef subElement in obj.allRequirements() )
                {
                    searchReqRef ( subElement, searchString, occurences );
                }
            }
            if ( obj.getComment() != null && obj.getComment().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in ReqRelated and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchReqRelated(Generated.ReqRelated obj, string searchString, List<ModelElement> occurences)
        {
            searchReferencesParagraph (obj, searchString, occurences);

        }

        /// <summary>
        /// Searches a specific string in Dictionary and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchDictionary(Generated.Dictionary obj, string searchString, List<ModelElement> occurences)
        {
            if ( obj.getSpecification() != null )
            {
                searchSpecification ( obj.getSpecification(), searchString, occurences );
            }
            if ( obj.allRuleDisablings() != null )
            {
                foreach ( Generated.RuleDisabling subElement in obj.allRuleDisablings() )
                {
                    searchRuleDisabling ( subElement, searchString, occurences );
                }
            }
            if ( obj.allNameSpaces() != null )
            {
                foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                {
                    searchNameSpace ( subElement, searchString, occurences );
                }
            }
            if ( obj.allNameSpaceRefs() != null )
            {
                foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                {
                    searchNameSpaceRef ( subElement, searchString, occurences );
                }
            }
            if ( obj.allTests() != null )
            {
                foreach ( Generated.Frame subElement in obj.allTests() )
                {
                    searchFrame ( subElement, searchString, occurences );
                }
            }
            if ( obj.allTestRefs() != null )
            {
                foreach ( Generated.FrameRef subElement in obj.allTestRefs() )
                {
                    searchFrameRef ( subElement, searchString, occurences );
                }
            }
            if ( obj.getTranslationDictionary() != null )
            {
                searchTranslationDictionary ( obj.getTranslationDictionary(), searchString, occurences );
            }
            if ( obj.getShortcutDictionary() != null )
            {
                searchShortcutDictionary ( obj.getShortcutDictionary(), searchString, occurences );
            }
            if ( obj.getXsi() != null && obj.getXsi().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getXsiLocation() != null && obj.getXsiLocation().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in RuleDisabling and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchRuleDisabling(Generated.RuleDisabling obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.getRule() != null && obj.getRule().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in NameSpaceRef and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchNameSpaceRef(Generated.NameSpaceRef obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

        }

        /// <summary>
        /// Searches a specific string in NameSpace and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchNameSpace(Generated.NameSpace obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allNameSpaces() != null )
            {
                foreach ( Generated.NameSpace subElement in obj.allNameSpaces() )
                {
                    searchNameSpace ( subElement, searchString, occurences );
                }
            }
            if ( obj.allNameSpaceRefs() != null )
            {
                foreach ( Generated.NameSpaceRef subElement in obj.allNameSpaceRefs() )
                {
                    searchNameSpaceRef ( subElement, searchString, occurences );
                }
            }
            if ( obj.allRanges() != null )
            {
                foreach ( Generated.Range subElement in obj.allRanges() )
                {
                    searchRange ( subElement, searchString, occurences );
                }
            }
            if ( obj.allEnumerations() != null )
            {
                foreach ( Generated.Enum subElement in obj.allEnumerations() )
                {
                    searchEnum ( subElement, searchString, occurences );
                }
            }
            if ( obj.allStructures() != null )
            {
                foreach ( Generated.Structure subElement in obj.allStructures() )
                {
                    searchStructure ( subElement, searchString, occurences );
                }
            }
            if ( obj.allCollections() != null )
            {
                foreach ( Generated.Collection subElement in obj.allCollections() )
                {
                    searchCollection ( subElement, searchString, occurences );
                }
            }
            if ( obj.allStateMachines() != null )
            {
                foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                {
                    searchStateMachine ( subElement, searchString, occurences );
                }
            }
            if ( obj.allFunctions() != null )
            {
                foreach ( Generated.Function subElement in obj.allFunctions() )
                {
                    searchFunction ( subElement, searchString, occurences );
                }
            }
            if ( obj.allProcedures() != null )
            {
                foreach ( Generated.Procedure subElement in obj.allProcedures() )
                {
                    searchProcedure ( subElement, searchString, occurences );
                }
            }
            if ( obj.allVariables() != null )
            {
                foreach ( Generated.Variable subElement in obj.allVariables() )
                {
                    searchVariable ( subElement, searchString, occurences );
                }
            }
            if ( obj.allRules() != null )
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    searchRule ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in ReqRef and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchReqRef(Generated.ReqRef obj, string searchString, List<ModelElement> occurences)
        {
            if ( obj.getId() != null && obj.getId().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getComment() != null && obj.getComment().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Type and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchType(Generated.Type obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.getDefault() != null && obj.getDefault().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Enum and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchEnum(Generated.Enum obj, string searchString, List<ModelElement> occurences)
        {
            searchType (obj, searchString, occurences);

            if ( obj.allValues() != null )
            {
                foreach ( Generated.EnumValue subElement in obj.allValues() )
                {
                    searchEnumValue ( subElement, searchString, occurences );
                }
            }
            if ( obj.allSubEnums() != null )
            {
                foreach ( Generated.Enum subElement in obj.allSubEnums() )
                {
                    searchEnum ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in EnumValue and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchEnumValue(Generated.EnumValue obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getValue() != null && obj.getValue().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Range and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchRange(Generated.Range obj, string searchString, List<ModelElement> occurences)
        {
            searchType (obj, searchString, occurences);

            if ( obj.getMinValue() != null && obj.getMinValue().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getMaxValue() != null && obj.getMaxValue().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allSpecialValues() != null )
            {
                foreach ( Generated.EnumValue subElement in obj.allSpecialValues() )
                {
                    searchEnumValue ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Structure and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchStructure(Generated.Structure obj, string searchString, List<ModelElement> occurences)
        {
            searchType (obj, searchString, occurences);

            if ( obj.allElements() != null )
            {
                foreach ( Generated.StructureElement subElement in obj.allElements() )
                {
                    searchStructureElement ( subElement, searchString, occurences );
                }
            }
            if ( obj.allProcedures() != null )
            {
                foreach ( Generated.Procedure subElement in obj.allProcedures() )
                {
                    searchProcedure ( subElement, searchString, occurences );
                }
            }
            if ( obj.allStateMachines() != null )
            {
                foreach ( Generated.StateMachine subElement in obj.allStateMachines() )
                {
                    searchStateMachine ( subElement, searchString, occurences );
                }
            }
            if ( obj.allRules() != null )
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    searchRule ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in StructureElement and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchStructureElement(Generated.StructureElement obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.getTypeName() != null && obj.getTypeName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getDefault() != null && obj.getDefault().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Collection and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchCollection(Generated.Collection obj, string searchString, List<ModelElement> occurences)
        {
            searchType (obj, searchString, occurences);

            if ( obj.getTypeName() != null && obj.getTypeName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Function and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchFunction(Generated.Function obj, string searchString, List<ModelElement> occurences)
        {
            searchType (obj, searchString, occurences);

            if ( obj.allParameters() != null )
            {
                foreach ( Generated.Parameter subElement in obj.allParameters() )
                {
                    searchParameter ( subElement, searchString, occurences );
                }
            }
            if ( obj.allCases() != null )
            {
                foreach ( Generated.Case subElement in obj.allCases() )
                {
                    searchCase ( subElement, searchString, occurences );
                }
            }
            if ( obj.getTypeName() != null && obj.getTypeName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Parameter and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchParameter(Generated.Parameter obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getTypeName() != null && obj.getTypeName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Case and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchCase(Generated.Case obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allPreConditions() != null )
            {
                foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                {
                    searchPreCondition ( subElement, searchString, occurences );
                }
            }
            if ( obj.getExpression() != null && obj.getExpression().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Procedure and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchProcedure(Generated.Procedure obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.getStateMachine() != null )
            {
                searchStateMachine ( obj.getStateMachine(), searchString, occurences );
            }
            if ( obj.allRules() != null )
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    searchRule ( subElement, searchString, occurences );
                }
            }
            if ( obj.allParameters() != null )
            {
                foreach ( Generated.Parameter subElement in obj.allParameters() )
                {
                    searchParameter ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in StateMachine and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchStateMachine(Generated.StateMachine obj, string searchString, List<ModelElement> occurences)
        {
            searchType (obj, searchString, occurences);

            if ( obj.getInitialState() != null && obj.getInitialState().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allStates() != null )
            {
                foreach ( Generated.State subElement in obj.allStates() )
                {
                    searchState ( subElement, searchString, occurences );
                }
            }
            if ( obj.allRules() != null )
            {
                foreach ( Generated.Rule subElement in obj.allRules() )
                {
                    searchRule ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in State and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchState(Generated.State obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.getStateMachine() != null )
            {
                searchStateMachine ( obj.getStateMachine(), searchString, occurences );
            }
        }

        /// <summary>
        /// Searches a specific string in Variable and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchVariable(Generated.Variable obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.getTypeName() != null && obj.getTypeName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getDefaultValue() != null && obj.getDefaultValue().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allSubVariables() != null )
            {
                foreach ( Generated.Variable subElement in obj.allSubVariables() )
                {
                    searchVariable ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Rule and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchRule(Generated.Rule obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.allConditions() != null )
            {
                foreach ( Generated.RuleCondition subElement in obj.allConditions() )
                {
                    searchRuleCondition ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in RuleCondition and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchRuleCondition(Generated.RuleCondition obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.allPreConditions() != null )
            {
                foreach ( Generated.PreCondition subElement in obj.allPreConditions() )
                {
                    searchPreCondition ( subElement, searchString, occurences );
                }
            }
            if ( obj.allActions() != null )
            {
                foreach ( Generated.Action subElement in obj.allActions() )
                {
                    searchAction ( subElement, searchString, occurences );
                }
            }
            if ( obj.allSubRules() != null )
            {
                foreach ( Generated.Rule subElement in obj.allSubRules() )
                {
                    searchRule ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in PreCondition and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchPreCondition(Generated.PreCondition obj, string searchString, List<ModelElement> occurences)
        {
            if ( obj.getCondition() != null && obj.getCondition().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Action and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchAction(Generated.Action obj, string searchString, List<ModelElement> occurences)
        {
            if ( obj.getExpression() != null && obj.getExpression().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in FrameRef and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchFrameRef(Generated.FrameRef obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

        }

        /// <summary>
        /// Searches a specific string in Frame and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchFrame(Generated.Frame obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getCycleDuration() != null && obj.getCycleDuration().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allSubSequences() != null )
            {
                foreach ( Generated.SubSequence subElement in obj.allSubSequences() )
                {
                    searchSubSequence ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in SubSequence and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchSubSequence(Generated.SubSequence obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getD_LRBG() != null && obj.getD_LRBG().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getLevel() != null && obj.getLevel().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getMode() != null && obj.getMode().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getNID_LRBG() != null && obj.getNID_LRBG().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getQ_DIRLRBG() != null && obj.getQ_DIRLRBG().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getQ_DIRTRAIN() != null && obj.getQ_DIRTRAIN().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getQ_DLRBG() != null && obj.getQ_DLRBG().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getRBC_ID() != null && obj.getRBC_ID().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getRBCPhone() != null && obj.getRBCPhone().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allTestCases() != null )
            {
                foreach ( Generated.TestCase subElement in obj.allTestCases() )
                {
                    searchTestCase ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in TestCase and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchTestCase(Generated.TestCase obj, string searchString, List<ModelElement> occurences)
        {
            searchReqRelated (obj, searchString, occurences);

            if ( obj.allSteps() != null )
            {
                foreach ( Generated.Step subElement in obj.allSteps() )
                {
                    searchStep ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Step and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchStep(Generated.Step obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getDescription() != null && obj.getDescription().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getComment() != null && obj.getComment().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getUserComment() != null && obj.getUserComment().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allSubSteps() != null )
            {
                foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                {
                    searchSubStep ( subElement, searchString, occurences );
                }
            }
            if ( obj.allMessages() != null )
            {
                foreach ( Generated.DBMessage subElement in obj.allMessages() )
                {
                    searchDBMessage ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in SubStep and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchSubStep(Generated.SubStep obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allActions() != null )
            {
                foreach ( Generated.Action subElement in obj.allActions() )
                {
                    searchAction ( subElement, searchString, occurences );
                }
            }
            if ( obj.allExpectations() != null )
            {
                foreach ( Generated.Expectation subElement in obj.allExpectations() )
                {
                    searchExpectation ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Expectation and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchExpectation(Generated.Expectation obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getValue() != null && obj.getValue().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getCondition() != null && obj.getCondition().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in DBMessage and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchDBMessage(Generated.DBMessage obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allFields() != null )
            {
                foreach ( Generated.DBField subElement in obj.allFields() )
                {
                    searchDBField ( subElement, searchString, occurences );
                }
            }
            if ( obj.allPackets() != null )
            {
                foreach ( Generated.DBPacket subElement in obj.allPackets() )
                {
                    searchDBPacket ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in DBPacket and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchDBPacket(Generated.DBPacket obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allFields() != null )
            {
                foreach ( Generated.DBField subElement in obj.allFields() )
                {
                    searchDBField ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in DBField and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchDBField(Generated.DBField obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getVariable() != null && obj.getVariable().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in TranslationDictionary and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchTranslationDictionary(Generated.TranslationDictionary obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allFolders() != null )
            {
                foreach ( Generated.Folder subElement in obj.allFolders() )
                {
                    searchFolder ( subElement, searchString, occurences );
                }
            }
            if ( obj.allTranslations() != null )
            {
                foreach ( Generated.Translation subElement in obj.allTranslations() )
                {
                    searchTranslation ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Folder and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchFolder(Generated.Folder obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allFolders() != null )
            {
                foreach ( Generated.Folder subElement in obj.allFolders() )
                {
                    searchFolder ( subElement, searchString, occurences );
                }
            }
            if ( obj.allTranslations() != null )
            {
                foreach ( Generated.Translation subElement in obj.allTranslations() )
                {
                    searchTranslation ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Translation and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchTranslation(Generated.Translation obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allSourceTexts() != null )
            {
                foreach ( Generated.SourceText subElement in obj.allSourceTexts() )
                {
                    searchSourceText ( subElement, searchString, occurences );
                }
            }
            if ( obj.allSubSteps() != null )
            {
                foreach ( Generated.SubStep subElement in obj.allSubSteps() )
                {
                    searchSubStep ( subElement, searchString, occurences );
                }
            }
            if ( obj.getComment() != null && obj.getComment().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in SourceText and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchSourceText(Generated.SourceText obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

        }

        /// <summary>
        /// Searches a specific string in ShortcutDictionary and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchShortcutDictionary(Generated.ShortcutDictionary obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allFolders() != null )
            {
                foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                {
                    searchShortcutFolder ( subElement, searchString, occurences );
                }
            }
            if ( obj.allShortcuts() != null )
            {
                foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                {
                    searchShortcut ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in ShortcutFolder and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchShortcutFolder(Generated.ShortcutFolder obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.allFolders() != null )
            {
                foreach ( Generated.ShortcutFolder subElement in obj.allFolders() )
                {
                    searchShortcutFolder ( subElement, searchString, occurences );
                }
            }
            if ( obj.allShortcuts() != null )
            {
                foreach ( Generated.Shortcut subElement in obj.allShortcuts() )
                {
                    searchShortcut ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Shortcut and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchShortcut(Generated.Shortcut obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getShortcutName() != null && obj.getShortcutName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in Specification and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchSpecification(Generated.Specification obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getVersion() != null && obj.getVersion().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allChapters() != null )
            {
                foreach ( Generated.Chapter subElement in obj.allChapters() )
                {
                    searchChapter ( subElement, searchString, occurences );
                }
            }
            if ( obj.allChapterRefs() != null )
            {
                foreach ( Generated.ChapterRef subElement in obj.allChapterRefs() )
                {
                    searchChapterRef ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in ChapterRef and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchChapterRef(Generated.ChapterRef obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

        }

        /// <summary>
        /// Searches a specific string in Chapter and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchChapter(Generated.Chapter obj, string searchString, List<ModelElement> occurences)
        {
            searchNamable (obj, searchString, occurences);

            if ( obj.getId() != null && obj.getId().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allParagraphs() != null )
            {
                foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                {
                    searchParagraph ( subElement, searchString, occurences );
                }
            }
        }

        /// <summary>
        /// Searches a specific string in Paragraph and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchParagraph(Generated.Paragraph obj, string searchString, List<ModelElement> occurences)
        {
            searchReferencesParagraph (obj, searchString, occurences);

            if ( obj.getId() != null && obj.getId().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getBl() != null && obj.getBl().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getText() != null && obj.getText().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getVersion() != null && obj.getVersion().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.allParagraphs() != null )
            {
                foreach ( Generated.Paragraph subElement in obj.allParagraphs() )
                {
                    searchParagraph ( subElement, searchString, occurences );
                }
            }
            if ( obj.getRevision() != null )
            {
                searchParagraphRevision ( obj.getRevision(), searchString, occurences );
            }
            if ( obj.getFunctionalBlockName() != null && obj.getFunctionalBlockName().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

        /// <summary>
        /// Searches a specific string in ParagraphRevision and updates the list 
        /// of model element with all the elements in which that string is found
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj">The string to search for</param>
        /// <param name="occurences">The list of model elements which hold the searched string</param>
        public static void searchParagraphRevision(Generated.ParagraphRevision obj, string searchString, List<ModelElement> occurences)
        {
            if ( obj.getText() != null && obj.getText().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
            if ( obj.getVersion() != null && obj.getVersion().Contains (searchString) ) 
            {
                occurences.Add ( obj );
            }
        }

    }
}
