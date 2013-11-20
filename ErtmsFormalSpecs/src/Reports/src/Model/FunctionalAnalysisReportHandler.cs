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
    using DataDictionary;
    using DataDictionary.Interpreter;
    using DataDictionary.Types;
    using DataDictionary.Variables;
    using DataDictionary.Types.AccessMode;
    using System.Collections.Generic;

    public class FunctionalAnalysisReportHandler : ReportHandler
    {
        public FunctionalAnalysisReportHandler(DataDictionary.Dictionary aDictionary)
            : base(aDictionary)
        {
            createFileName("FunctionalAnalysisReport");
        }

        /// <summary>
        /// Creates a report on the model, according to user's choices
        /// </summary>
        /// <returns>The document created, or null</returns>
        public override Document BuildDocument()
        {
            Document retVal = new Document();

            Log.Info("Generating functional analysis report");
            retVal.Info.Title = "EFS Functional Analysis report";
            retVal.Info.Author = "ERTMS Solutions";
            retVal.Info.Subject = "Functional Analysis report";

            FunctionalAnalysisReport report = new FunctionalAnalysisReport(retVal);
            List<AccessMode> accesses = IEnclosesNameSpacesUtils.getAccesses(EFSSystem, null);
            foreach (DataDictionary.Types.NameSpace nameSpace in Dictionary.NameSpaces)
            {
                CreateNamespaceSection(report, nameSpace, accesses);
            }

            return retVal;
        }

        public void CreateNamespaceSection(FunctionalAnalysisReport report, DataDictionary.Types.NameSpace nameSpace, List<AccessMode> accesses)
        {
            Log.Info("..generating name space " + nameSpace.Name);

            report.CreateNameSpaceSection(nameSpace, accesses);
            foreach (DataDictionary.Types.NameSpace subNameSpace in nameSpace.NameSpaces)
            {
                CreateNamespaceSection(report, subNameSpace, accesses);
            }
        }
    }
}
