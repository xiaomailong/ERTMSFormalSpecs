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
            AddRanges = false;
            AddRangesDetails = false;
            AddEnumerations = false;
            AddEnumerationsDetails = false;
            AddStructures = false;
            AddStructuresDetails = false;
            AddCollections = false;
            AddCollectionsDetails = false;
            AddFunctions = false;
            AddFunctionsDetails = false;
            AddProcedures = false;
            AddProceduresDetails = false;
            AddVariables = false;
            AddVariablesDetails = false;
            AddRules = false;
            AddRulesDetails = false;
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

            if (!aNameSpace.FullName.StartsWith("Messages"))
            {
                report.AddSubParagraph("Namespace " + aNameSpace.FullName);

                if (AddRanges)
                {
                    report.CreateRangesSection(aNameSpace, AddRangesDetails);
                }
                if (AddEnumerations)
                {
                    report.CreateEnumerationsSection(aNameSpace, AddEnumerationsDetails);
                }
                if (AddStructures)
                {
                    report.CreateStructuresSection(aNameSpace, AddStructuresDetails);
                }
                if (AddCollections)
                {
                    report.CreateCollectionsSection(aNameSpace, AddCollectionsDetails);
                }
                if (AddFunctions)
                {
                    report.CreateFunctionsSection(aNameSpace, AddFunctionsDetails);
                }
                if (AddProcedures)
                {
                    report.CreateProceduresSection(aNameSpace, AddProceduresDetails);
                }
                if (AddVariables)
                {
                    report.CreateVariablesSection(aNameSpace, AddVariablesDetails, InOutFilter);
                }
                if (AddRules)
                {
                    report.CreateRulesSection(aNameSpace, AddRulesDetails);
                }
                report.CloseSubParagraph();

                foreach (DataDictionary.Types.NameSpace nameSpace in aNameSpace.SubNameSpaces)
                {
                    CreateNamespaceSection(report, nameSpace);
                }
            }
        }

        public bool AddRanges { set; get; }
        public bool AddRangesDetails { set; get; }

        public bool AddEnumerations { set; get; }
        public bool AddEnumerationsDetails { set; get; }

        public bool AddStructures { set; get; }
        public bool AddStructuresDetails { set; get; }

        public bool AddCollections { set; get; }
        public bool AddCollectionsDetails { set; get; }

        public bool AddFunctions { set; get; }
        public bool AddFunctionsDetails { set; get; }

        public bool AddProcedures { set; get; }
        public bool AddProceduresDetails { set; get; }

        public bool AddVariables { set; get; }
        public bool AddVariablesDetails { set; get; }
        public bool InOutFilter { set; get; }

        public bool AddRules { set; get; }
        public bool AddRulesDetails { set; get; }
    }
}
