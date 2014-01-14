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
namespace GUI.LongOperations
{
    using DataDictionary;

    public class CheckModelOperation : BaseLongOperation
    {
        /// <summary>
        /// The system on which the check is performed
        /// </summary>
        private EFSSystem EFSSystem { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="system"></param>
        public CheckModelOperation(EFSSystem system)
        {
            EFSSystem = system;
        }

        /// <summary>
        /// Generates the file in the background thread
        /// </summary>
        /// <param name="arg"></param>
        public override void ExecuteWork()
        {
            DataDictionary.Generated.ControllersManager.DesactivateAllNotifications();
            try
            {
                foreach (DataDictionary.Dictionary dictionary in EFSSystem.Dictionaries)
                {
                    dictionary.CheckRules();
                }
            }
            finally
            {
                DataDictionary.Generated.ControllersManager.ActivateAllNotifications();
            }
        }
    }
}
