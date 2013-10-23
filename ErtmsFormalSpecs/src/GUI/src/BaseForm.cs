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
using System.Windows.Forms;
using System.Threading;
using System;

namespace GUI
{
    public interface IBaseForm
    {
        /// <summary>
        /// The property grid used to edit elements properties
        /// </summary>
        MyPropertyGrid Properties { get; }

        /// <summary>
        /// The text editor for messages
        /// </summary>
        RichTextBox MessagesTextBox { get; }

        /// <summary>
        /// The requirements text box used to display the associated requirements
        /// </summary>
        EditorTextBox RequirementsTextBox { get; }

        /// <summary>
        /// The text box used to edit expression
        /// </summary>
        EditorTextBox ExpressionEditorTextBox { get; }

        /// <summary>
        /// The enclosing MDI Window
        /// </summary>
        MainWindow MDIWindow { get; }

        /// <summary>
        /// The main tree view of the form
        /// </summary>
        BaseTreeView TreeView { get; }

        /// <summary>
        /// The sub tree view of the form
        /// </summary>
        BaseTreeView subTreeView { get; }

        /// <summary>
        /// The explain text box
        /// </summary>
        ExplainTextBox ExplainTextBox { get; }

        /// <summary>
        /// Refreshed the view, while no structural model occurred
        /// </summary>
        void Refresh();

        /// <summary>
        /// Allows to refresh the view, according to the fact that the structure for the model could change
        /// </summary>
        void RefreshModel();

        /// <summary>
        /// Provides the model element currently selected in this IBaseForm
        /// </summary>
        Utils.IModelElement Selected { get; }
    }

    public abstract class BaseForm : Form, IBaseForm
    {
        /// <summary>
        /// The property grid used to edit elements properties
        /// </summary>
        public abstract MyPropertyGrid Properties { get; }

        /// <summary>
        /// The text editor for messages
        /// </summary>
        public abstract RichTextBox MessagesTextBox { get; }

        /// <summary>
        /// The requirements text box used to display the associated requirements
        /// </summary>
        public abstract EditorTextBox RequirementsTextBox { get; }

        /// <summary>
        /// The text box used to edit expression
        /// </summary>
        public abstract EditorTextBox ExpressionEditorTextBox { get; }

        /// <summary>
        /// The enclosing MDI Window
        /// </summary>
        public MainWindow MDIWindow
        {
            get { return GUI.FormsUtils.EnclosingForm(this.Parent) as MainWindow; }
        }

        /// <summary>
        /// The main tree view of the form
        /// </summary>
        public abstract BaseTreeView TreeView { get; }

        /// <summary>
        /// The sub tree view of the form
        /// </summary>
        public abstract BaseTreeView subTreeView { get; }

        /// <summary>
        /// The explain text box
        /// </summary>
        public abstract ExplainTextBox ExplainTextBox { get; }

        /// <summary>
        /// Allows to refresh the view, according to the fact that the structure for the model could change
        /// </summary>
        public abstract void RefreshModel();

        /// <summary>
        /// Provides the model element currently selected in this IBaseForm
        /// </summary>
        public virtual Utils.IModelElement Selected
        {
            get
            {
                Utils.IModelElement retVal = null;

                if (TreeView != null && TreeView.Selected != null)
                {
                    retVal = TreeView.Selected.Model;
                }

                return retVal;
            }
        }

        /// <summary>
        /// The thread used to synchronize node names with their model
        /// </summary>
        private class Synchronizer : GenericSynchronizationHandler<BaseForm>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instance"></param>
            public Synchronizer(BaseForm instance, int cycleTime)
                : base(instance, cycleTime)
            {
            }

            /// <summary>
            /// Synchronization
            /// </summary>
            /// <param name="instance"></param>
            public override void HandleSynchronization(BaseForm instance)
            {
                instance.Invoke((MethodInvoker)delegate
                {
                    instance.SynchronizeForm();
                });
            }
        }

        /// <summary>
        /// Indicates that synchronization is required
        /// </summary>
        private Synchronizer FormSynchronizer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseForm()
            : base()
        {
            FormSynchronizer = new Synchronizer(this, 300);
        }

        /// <summary>
        /// Stop the synchronizer when the form is closed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            FormSynchronizer.Synchronize = false;
        }

        /// <summary>
        /// Synchronizes the form with its model
        /// </summary>
        public virtual void SynchronizeForm()
        {
            if (Properties != null)
            {
                if (!Properties.ContainsFocus && !(Properties.ActiveControl is System.Windows.Forms.Button))
                {
                    Properties.Refresh();
                }
            }

            if (TreeView != null && TreeView.SelectedNode != null)
            {
                ((BaseTreeNode)TreeView.SelectedNode).RefreshViewAccordingToModel(this, true);
            }
        }
    }

    public class FormsUtils
    {
        public static Form EnclosingForm(Control control)
        {
            while (control != null && !(control is Form))
            {
                control = control.Parent;
            }
            return control as Form;
        }

        public static IBaseForm EnclosingIBaseForm(Control control)
        {
            while (control != null && !(control is IBaseForm))
            {
                control = control.Parent;
            }
            return control as IBaseForm;
        }
    }
}
