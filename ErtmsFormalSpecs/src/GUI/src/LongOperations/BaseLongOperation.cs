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
    using Utils;

    /// <summary>
    /// The base class used to handle long operations
    /// </summary>
    public abstract class BaseLongOperation : ProgressHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseLongOperation()
            : base()
        {
        }

        /// <summary>
        /// Executes the operation in background using a progress handler
        /// </summary>
        /// <param name="message">The message to display on the dialog window</param>
        /// <param name="allowCancel">Indicates that the opeation can be canceled</param>
        public void ExecuteUsingProgressDialog(string message, bool allowCancel = true)
        {
            ProgressDialog dialog = new ProgressDialog(message, this, allowCancel);
            dialog.ShowDialog();
        }
    }
}
