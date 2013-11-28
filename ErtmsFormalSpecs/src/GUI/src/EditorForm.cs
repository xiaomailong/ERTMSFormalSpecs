using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataDictionary;
using WeifenLuo.WinFormsUI.Docking;

namespace GUI
{
    public partial class EditorForm : DockContent
    {
        /// <summary>
        /// Indicates the actions to be performed to get the text from the instance and to set it into the instance
        /// </summary>
        public abstract class HandleTextChange
        {
            /// <summary>
            /// The instance that is currently handled
            /// </summary>
            public ModelElement Instance { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instance"></param>
            public HandleTextChange(ModelElement instance)
            {
                Instance = instance;
            }

            /// <summary>
            /// The way text is retrieved from the instance
            /// </summary>
            /// <returns></returns>
            public abstract string GetText();

            /// <summary>
            /// The way text is set back in the instance
            /// </summary>
            /// <returns></returns>
            public abstract void SetText(string text);
        }

        /// <summary>
        /// Indicates that only types should be considered
        /// </summary>
        public bool ConsiderOnlyTypes
        {
            set { editorTextBox.ConsiderOnlyTypes = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditorForm()
        {
            InitializeComponent();

            editorTextBox.TextBox.TextChanged += new EventHandler(TextChangedHandler);
            editorTextBox.TextBox.KeyUp += new KeyEventHandler(TextBox_KeyUp);
            FormClosed += new FormClosedEventHandler(Window_FormClosed);

            DockState = WeifenLuo.WinFormsUI.Docking.DockState.Float;
        }

        void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Handles the close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            GUIUtils.MDIWindow.HandleSubWindowClosed(this);
        }

        /// <summary>
        /// Called when the text has been changed in the inner text box
        /// This updates the instance according to the __textChangeHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChangedHandler(object sender, EventArgs e)
        {
            if (__textChangeHandler != null)
            {
                __textChangeHandler.SetText(editorTextBox.TextBox.Text);
            }
        }

        /// <summary>
        /// Indicates whether auto completion is available
        /// </summary>
        public bool AutoComplete
        {
            set
            {
                editorTextBox.AutoComplete = value;
            }
        }

        /// <summary>
        /// The delegate method that need to be called when the text of the text box has been changed
        /// </summary>
        private HandleTextChange __textChangeHandler = null;

        /// <summary>
        /// The element on which this editor is built
        /// </summary>
        public void setChangeHandler(HandleTextChange handleTextChange)
        {
            __textChangeHandler = handleTextChange;
            Text = "Editor for " + __textChangeHandler.Instance.FullName;
            editorTextBox.Instance = __textChangeHandler.Instance;
            RefreshText();
        }

        /// <summary>
        /// Refreshes the text of the text box
        /// </summary>
        public void RefreshText()
        {
            int start = editorTextBox.TextBox.SelectionStart;
            Value = __textChangeHandler.GetText();
            editorTextBox.TextBox.SelectionStart = start;
        }

        /// <summary>
        /// Indicates that the editor text box has the focus
        /// </summary>
        /// <returns></returns>
        public bool EditorTextBoxHasFocus()
        {
            return editorTextBox.TextBox.Focused;
        }

        /// <summary>
        /// The textual value to edit
        /// </summary>
        public string Value
        {
            get { return editorTextBox.TextBox.Text; }
            set
            {
                editorTextBox.TextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                editorTextBox.TextBox.Text = value;
            }
        }

        /// <summary>
        /// The instance currently edited by the editor
        /// </summary>
        public DataDictionary.Generated.BaseModelElement Instance
        {
            get { return __textChangeHandler.Instance; }
        }
    }
}
