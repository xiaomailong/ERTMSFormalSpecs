using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataDictionary;
using DataDictionary.Interpreter;
using Utils;
namespace GUI
{
    public partial class EditorTextBox : UserControl
    {
        /// <summary>
        /// Indicates whether there is a pending selection in the combo box
        /// </summary>
        private bool PendingSelection { get; set; }

        /// <summary>
        /// The enclosing IBaseForm
        /// </summary>
        IBaseForm EnclosingForm
        {
            get
            {
                Control current = Parent;
                while (current != null && !(current is IBaseForm))
                {
                    current = current.Parent;
                }

                return current as IBaseForm;
            }
        }

        /// <summary>
        /// Provides the instance on which this editor is based
        /// </summary>
        private DataDictionary.ModelElement Instance { get { return (DataDictionary.ModelElement)EnclosingForm.Selected; } }

        /// <summary>
        /// Provides the EFSSystem 
        /// </summary>
        private EFSSystem EFSSystem { get { return Instance.EFSSystem; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditorTextBox()
        {
            InitializeComponent();

            EditionTextBox.AllowDrop = true;
            EditionTextBox.DragDrop += new DragEventHandler(Editor_DragDropHandler);
            EditionTextBox.KeyUp += new KeyEventHandler(Editor_KeyUp);

            EditionTextBox.ShortcutsEnabled = true;

            EditionTextBox.GotFocus += new EventHandler(Editor_GotFocus);
            EditionTextBox.LostFocus += new EventHandler(Editor_LostFocus);

            SelectionComboBox.LostFocus += new EventHandler(SelectionComboBox_LostFocus);
            SelectionComboBox.KeyUp += new KeyEventHandler(SelectionComboBox_KeyUp);
        }

        public void Copy()
        {
            EditionTextBox.Copy();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        public void Cut()
        {
            EditionTextBox.Cut();
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        public void Paste()
        {
            EditionTextBox.Paste();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        public void Undo()
        {
            EditionTextBox.Undo();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        public void Redo()
        {
            EditionTextBox.Redo();
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        /// <summary>
        /// Provides the list of available elements which match the prefix provided
        /// </summary>
        /// <param name="element">The element in which the possibilities should be found</param>
        /// <param name="prefix">The prefix of the element to find</param>
        /// <param name="enclosingName">The name of the enclosing structure</param>
        /// <returns></returns>
        private HashSet<string> getPossibilities(IModelElement element, string prefix, string enclosingName)
        {
            HashSet<string> retVal = new HashSet<string>();

            while (element != null)
            {
                ISubDeclarator subDeclarator = element as ISubDeclarator;
                if (subDeclarator != null)
                {
                    subDeclarator.InitDeclaredElements();
                    foreach (string subElem in subDeclarator.DeclaredElements.Keys)
                    {
                        if (subElem.StartsWith(prefix))
                        {
                            if (enclosingName != null)
                            {
                                foreach (Utils.INamable namable in subDeclarator.DeclaredElements[subElem])
                                {
                                    if (namable.FullName.EndsWith(enclosingName + "." + subElem))
                                    {
                                        retVal.Add(subElem);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                retVal.Add(subElem);
                            }
                        }
                    }
                }

                element = element.Enclosing as IModelElement;
            }

            return retVal;
        }

        /// <summary>
        /// Provides the current prefix, according to the selection position
        /// </summary>
        /// <returns></returns>
        private string CurrentPrefix()
        {
            string retVal = "";

            int i = EditionTextBox.SelectionStart;
            while (i >= EditionTextBox.Text.Length)
            {
                i = i - 1;
            }
            while (i >= 0 && !Char.IsSeparator(EditionTextBox.Text[i]))
            {
                retVal = EditionTextBox.Text[i] + retVal;
                i = i - 1;
            }

            return retVal;
        }

        /// <summary>
        /// Provides the list of model elements which correspond to the prefix given
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private List<string> AllChoices(out string prefix)
        {
            List<string> retVal = new List<string>();

            // Also use the default namespace
            List<Utils.INamable> possibleInstances = new List<Utils.INamable>();
            string currentText = CurrentPrefix().Trim();

            string enclosingName;
            int lastDot = currentText.LastIndexOf('.');
            if (lastDot > 0)
            {
                enclosingName = currentText.Substring(0, lastDot);
                Expression expression = EFSSystem.Parser.Expression(Instance, enclosingName, Filter.AllMatches);
                if (expression != null)
                {
                    if (expression.Ref != null)
                    {
                        possibleInstances.Add(expression.Ref);
                    }
                    else
                    {
                        foreach (ReturnValueElement element in expression.getReferences(null, Filter.AllMatches, false).Values)
                        {
                            possibleInstances.Add(element.Value);
                        }
                    }
                }
                else
                {
                    possibleInstances.Add(Instance);
                }
                prefix = currentText.Substring(lastDot + 1);
            }
            else
            {
                possibleInstances.Add(Instance);
                enclosingName = null;
                prefix = currentText;
            }

            foreach (Utils.INamable namable in possibleInstances)
            {
                retVal.AddRange(getPossibilities((IModelElement)namable, prefix, enclosingName));
            }


            retVal.Sort();
            return retVal;
        }

        /// <summary>
        /// Displays the combo box if required and updates the edotor's text
        /// </summary>
        private void DisplayComboBox()
        {
            string prefix;
            List<string> allChoices = AllChoices(out prefix);

            if (prefix.Length <= EditionTextBox.SelectionStart)
            {
                EditionTextBox.Select(EditionTextBox.SelectionStart - prefix.Length, prefix.Length);
                if (allChoices.Count == 1)
                {
                    EditionTextBox.SelectedText = allChoices[0];
                }
                else if (allChoices.Count > 1)
                {
                    SelectionComboBox.Items.Clear();
                    foreach (string choice in allChoices)
                    {
                        SelectionComboBox.Items.Add(choice);
                    }
                    if (prefix != null && prefix.Length > 0)
                    {
                        SelectionComboBox.Text = prefix;
                    }
                    else
                    {
                        SelectionComboBox.Text = allChoices[0];
                    }

                    // Try to compute the combo box location
                    // TODO : Hypothesis. The first displayed line is the first line of the text
                    int line = 1;
                    string lineData = "";
                    for (int i = 0; i < EditionTextBox.SelectionStart; i++)
                    {
                        switch (EditionTextBox.Text[i])
                        {
                            case '\n':
                                line += 1;
                                lineData = "";
                                break;

                            default:
                                lineData += EditionTextBox.Text[i];
                                break;
                        }
                    }

                    // TODO : How to get a Graphics without having to create it.
                    Graphics g = EnclosingForm.MDIWindow.CreateGraphics();
                    SizeF size = g.MeasureString(lineData, EditionTextBox.Font);
                    g.Dispose();

                    Point comboBoxLocation = new Point((int)size.Width, (line - 1) * EditionTextBox.Font.Height + 5);
                    SelectionComboBox.Location = comboBoxLocation;
                    PendingSelection = true;
                    SelectionComboBox.Show();
                    SelectionComboBox.Focus();
                }
            }
        }

        void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                        // Remove the space that has just been added
                        EditionTextBox.Select(EditionTextBox.SelectionStart - 1, 1);
                        EditionTextBox.SelectedText = "";
                        DisplayComboBox();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.OemPeriod:
                        DisplayComboBox();
                        break;

                    default:
                        break;
                }
            }
        }

        private void ConfirmComboBoxSelection()
        {
            if (PendingSelection)
            {
                PendingSelection = false;
                EditionTextBox.SelectedText = SelectionComboBox.Text;
                EditionTextBox.SelectionStart = EditionTextBox.SelectionStart + SelectionComboBox.Text.Length;
                SelectionComboBox.Text = "";
                SelectionComboBox.Items.Clear();
                SelectionComboBox.Hide();
            }
        }

        private void AbordComboBoxSelection()
        {
            if (PendingSelection)
            {
                PendingSelection = false;
                SelectionComboBox.Text = "";
                SelectionComboBox.Items.Clear();
                SelectionComboBox.Hide();
            }
        }

        void SelectionComboBox_LostFocus(object sender, EventArgs e)
        {
            ConfirmComboBoxSelection();
        }

        void SelectionComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    ConfirmComboBoxSelection();
                    break;

                case Keys.Escape:
                    AbordComboBoxSelection();
                    break;

                default:
                    break;
            }
        }

        void Editor_LostFocus(object sender, EventArgs e)
        {
            if (EnclosingForm != null && EnclosingForm.MDIWindow != null)
            {
                EnclosingForm.MDIWindow.SelectedRichTextBox = null;
            }
        }

        void Editor_GotFocus(object sender, EventArgs e)
        {
            if (EnclosingForm != null && EnclosingForm.MDIWindow != null)
            {
                EnclosingForm.MDIWindow.SelectedRichTextBox = this;
            }
        }

        /// <summary>
        /// Called when the drop operation is performed on this text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Editor_DragDropHandler(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("WindowsForms10PersistentObject", false))
            {
                BaseTreeNode SourceNode = (BaseTreeNode)e.Data.GetData("WindowsForms10PersistentObject");
                if (SourceNode != null)
                {
                    DataDictionaryView.VariableTreeNode variableNode = SourceNode as DataDictionaryView.VariableTreeNode;
                    if (variableNode != null)
                    {
                        StringBuilder text = new StringBuilder();
                        text.Append(StripUseless(SourceNode.Model.FullName, EnclosingForm.Selected) + " <- ");

                        DataDictionary.Variables.Variable variable = variableNode.Item;
                        DataDictionary.Types.Structure structure = variable.Type as DataDictionary.Types.Structure;
                        if (structure != null)
                        {
                            text.Append(StripUseless(structure.FullName, EnclosingForm.Selected) + "{\n");
                            bool first = true;
                            foreach (DataDictionary.Types.StructureElement element in structure.Elements)
                            {
                                if (!first)
                                {
                                    text.Append(",\n");
                                }
                                insertElement(element, text, 4);
                                first = false;
                            }
                            text.Append("}\n");
                        }
                        else
                        {
                            text.Append(variable.DefaultValue.FullName);
                        }
                        EditionTextBox.SelectedText = text.ToString();
                    }
                    else
                    {
                        EditionTextBox.SelectedText = StripUseless(SourceNode.Model.FullName, EnclosingForm.Selected);
                    }
                }
            }
        }

        private void insertElement(DataDictionary.Types.ITypedElement element, StringBuilder text, int indent)
        {
            text.Append(TextualExplainUtilities.Pad(element.Name + " => ", indent));
            DataDictionary.Types.Structure structure = element.Type as DataDictionary.Types.Structure;
            if (structure != null)
            {
                indent = indent + 4;
                text.Append(StripUseless(structure.FullName, EnclosingForm.Selected) + "{\n");
                bool first = true;
                foreach (DataDictionary.Types.StructureElement subElement in structure.Elements)
                {
                    if (!first)
                    {
                        text.Append(",\n");
                    }
                    insertElement(subElement, text, indent);
                    first = false;
                }
                indent -= 4;
                text.Append("\n" + TextualExplainUtilities.Pad("}", indent));
            }
            else
            {
                text.Append(element.Default);
            }
        }

        /// <summary>
        /// The prefix for the Default namespace
        /// </summary>
        private static string DEFAULT_PREFIX = "Default.";

        /// <summary>
        /// Removes useless prefixes from the string provided
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private string StripUseless(string fullName, Utils.IModelElement model)
        {
            string retVal = fullName;

            if (model != null)
            {
                char[] tmp = fullName.ToArray();
                char[] modelName = model.FullName.ToArray();

                int i = 0;
                while (i < tmp.Length && i < modelName.Length)
                {
                    if (tmp[i] != modelName[i])
                    {
                        break;
                    }
                    i += 1;
                }

                retVal = retVal.Substring(i);
                if (Utils.Utils.isEmpty(retVal))
                {
                    retVal = model.Name;
                }
            }

            if (retVal.StartsWith(DEFAULT_PREFIX))
            {
                retVal = retVal.Substring(DEFAULT_PREFIX.Length);
            }

            return retVal;
        }

        public string Rtf { get { return EditionTextBox.Rtf; } set { EditionTextBox.Rtf = value; } }
        public bool ReadOnly { get { return EditionTextBox.ReadOnly; } set { EditionTextBox.ReadOnly = value; } }
        public string[] Lines { get { return EditionTextBox.Lines; } set { EditionTextBox.Lines = value; } }
    }
}
