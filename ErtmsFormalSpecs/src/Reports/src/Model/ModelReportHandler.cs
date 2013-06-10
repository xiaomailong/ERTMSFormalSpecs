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
namespace Reports.Model
{
    using MigraDoc.DocumentObjectModel;

    public class ModelReportHandler : ReportHandler
    {
        public ModelReportHandler(DataDictionary.Dictionary aDictionary)
            : base(aDictionary)
        {
            createFileName("ModelReport");
            AddRanges              = false;
            AddRangesDetails       = false;
            AddEnumerations        = false;
            AddEnumerationsDetails = false;
            AddStructures          = false;
            AddStructuresDetails   = false;
            AddCollections         = false;
            AddCollectionsDetails  = false;
            AddFunctions           = false;
            AddFunctionsDetails    = false;
            AddProcedures          = false;
            AddProceduresDetails   = false;
            AddVariables           = false;
            AddVariablesDetails    = false;
            AddRules               = false;
            AddRulesDetails        = false;
            ImplementedOnly        = true;
        }

        /// <summary>
        /// Creates a report on the model, according to user's choices
        /// </summary>
        /// <returns>The document created, or null</returns>
        public override Document BuildDocument()
        {
            Document retVal = new Document();

            Log.Info("Generating model report");
            retVal.Info.Title = "EFS Model report";
            retVal.Info.Author = "ERTMS Solutions";
            retVal.Info.Subject = "Model report";

            ModelReport report = new ModelReport(retVal);
            foreach (DataDictionary.Types.NameSpace nameSpace in Dictionary.NameSpaces)
            {
                CreateNamespaceSection(report, nameSpace);
            }

            return retVal;
        }

        public void CreateNamespaceSection(ModelReport report, DataDictionary.Types.NameSpace aNameSpace)
        {
            Log.Info("..generating name space " + aNameSpace.Name);
            bool informationAdded = false;

            if (!aNameSpace.FullName.StartsWith("Messages"))
            {
                if (AddRanges)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Ranges, ImplementedOnly) > 0)
                    {
                        report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                        report.CreateRangesSection(aNameSpace, AddRangesDetails, ImplementedOnly);
                        informationAdded = true;
                    }
                }
                if (AddEnumerations)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Enumerations, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateEnumerationsSection(aNameSpace, AddEnumerationsDetails, ImplementedOnly);
                    }
                }
                if (AddStructures)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Structures, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateStructuresSection(aNameSpace, AddStructuresDetails, ImplementedOnly);
                    }
                }
                if (AddCollections)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Collections, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateCollectionsSection(aNameSpace, AddCollectionsDetails, ImplementedOnly);
                    }
                }
                if (AddStateMachines)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.StateMachines, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateStateMachinesSection(aNameSpace, AddStateMachinesDetails, ImplementedOnly);
                    }
                }
                if (AddFunctions)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Functions, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateFunctionsSection(aNameSpace, AddFunctionsDetails, ImplementedOnly);
                    }
                }
                if (AddProcedures)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Procedures, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateProceduresSection(aNameSpace, AddProceduresDetails, ImplementedOnly);
                    }
                }
                if (AddVariables)
                {
                    if (report.CountDisplayedVariables(aNameSpace.Variables, ImplementedOnly, InOutFilter) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateVariablesSection(aNameSpace, AddVariablesDetails, ImplementedOnly, InOutFilter);
                    }
                }
                if (AddRules)
                {
                    if (report.CountDisplayedReqRelated(aNameSpace.Rules, ImplementedOnly) > 0)
                    {
                        if (!informationAdded)
                        {
                            report.AddSubParagraph("Namespace " + aNameSpace.FullName);
                            informationAdded = true;
                        }
                        report.CreateRulesSection(aNameSpace, AddRulesDetails, ImplementedOnly);
                    }
                }

                if (informationAdded)
                {
                    report.CloseSubParagraph();
                }

                foreach (DataDictionary.Types.NameSpace nameSpace in aNameSpace.SubNameSpaces)
                {
                    CreateNamespaceSection(report, nameSpace);
                }
            }
        }

        public bool AddRanges               { set; get; }
        public bool AddRangesDetails        { set; get; }

        public bool AddEnumerations         { set; get; }
        public bool AddEnumerationsDetails  { set; get; }

        public bool AddStructures           { set; get; }
        public bool AddStructuresDetails    { set; get; }

        public bool AddCollections          { set; get; }
        public bool AddCollectionsDetails   { set; get; }

        public bool AddStateMachines        { set; get; }
        public bool AddStateMachinesDetails { set; get; }

        public bool AddFunctions            { set; get; }
        public bool AddFunctionsDetails     { set; get; }

        public bool AddProcedures           { set; get; }
        public bool AddProceduresDetails    { set; get; }

        public bool AddVariables            { set; get; }
        public bool AddVariablesDetails     { set; get; }
        public bool InOutFilter             { set; get; }

        public bool AddRules                { set; get; }
        public bool AddRulesDetails         { set; get; }

        public bool ImplementedOnly         { set; get; }
    }
}
