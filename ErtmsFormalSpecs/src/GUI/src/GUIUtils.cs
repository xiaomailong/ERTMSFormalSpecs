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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public class GUIUtils
    {
        /// <summary>
        /// The main window of the application
        /// </summary>
        public static MainWindow MDIWindow { get; set; }

        /// <summary>
        /// Refreshes the view according to the model element that has been changed
        /// </summary>
        /// <param name="model"></param>
        public static void RefreshViewAccordingToModel(DataDictionary.Generated.BaseModelElement model)
        {
            MDIWindow.Invoke((MethodInvoker)delegate
            {
                // Refresh the node which corresponds to the model that has been changed
                foreach (IBaseForm form in MDIWindow.SubWindows)
                {
                    if (form.TreeView != null)
                    {
                        BaseTreeNode node = form.TreeView.FindNode(model);
                        if (node != null)
                        {
                            node.RefreshNode();
                            if (form.Properties != null)
                            {
                                form.Properties.Refresh();
                            }
                        }
                    }
                }

                foreach (EditorForm editor in MDIWindow.Editors)
                {
                    if (editor.Instance == model)
                    {
                        editor.RefreshText();
                    }
                }
            });
        }

        /// <summary>
        /// Access to a graphics item
        /// </summary>
        public static Graphics Graphics { get; set; }

        /// --------------------------------------------------------------------
        ///   Enclosing finder
        /// --------------------------------------------------------------------
        /// <summary>
        /// Finds in an enclosing element the element whose type matches T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EnclosingFinder<T> : Utils.IFinder
            where T : class
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public EnclosingFinder()
            {
                Utils.FinderRepository.INSTANCE.Register(this);
            }

            /// <summary>
            /// Finds an enclosing element whose type is T
            /// </summary>
            /// <param name="el"></param>
            /// <returns></returns>
            public static T find(System.Windows.Forms.Control el)
            {
                while (el != null && !(el is T))
                {
                    el = el.Parent;
                }
                return el as T;
            }

            /// <summary>
            /// Clears the cache
            /// </summary>
            public void ClearCache()
            {
                // No cache
            }
        }
    }
}
