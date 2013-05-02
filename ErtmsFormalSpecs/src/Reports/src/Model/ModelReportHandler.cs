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
namespace Report.Model
{
    using DataDictionary;

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
        /// Generates the file in the background task
        /// </summary>
        /// <param name="arg"></param>
        public override void ExecuteWork()
        {
            ReportBuilder builder = new ReportBuilder(EFSSystem);
            if (!builder.BuildModelReport(this))
            {
                Log.ErrorFormat("Report creation failed");
            }
            else
            {
                displayReport();
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
