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
namespace GUI.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing.Design;
    using System.ComponentModel;
    using System.Windows.Forms.Design;
    using DataDictionary;
    using System.Windows.Forms;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DefaultValueUITypedEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Sets the string value into the right property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        private class TextChangeHandler : EditorForm.HandleTextChange
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instance"></param>
            public TextChangeHandler(ModelElement instance)
                : base(instance)
            {
            }

            /// <summary>
            /// The way text is retrieved from the instance
            /// </summary>
            /// <returns></returns>
            public override string GetText()
            {
                string retVal = "";
                DataDictionary.Types.IDefaultValueElement defaultValueElement = Instance as DataDictionary.Types.IDefaultValueElement;

                if (defaultValueElement != null)
                {
                    retVal = defaultValueElement.Default;
                }
                return retVal;
            }

            /// <summary>
            /// The way text is set back in the instance
            /// </summary>
            /// <returns></returns>
            public override void SetText(string text)
            {
                DataDictionary.Types.IDefaultValueElement defaultValueElement = Instance as DataDictionary.Types.IDefaultValueElement;

                if (defaultValueElement != null)
                {
                    defaultValueElement.Default = text;
                }
            }
        }

        /// <summary>
        /// Sets the string value into the right property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        private void HandleTextChange(ModelElement instance, string value)
        {
            DataDictionary.Types.IDefaultValueElement defaultValueElement = instance as DataDictionary.Types.IDefaultValueElement;

            if (defaultValueElement != null)
            {
                defaultValueElement.Default = value;
            }
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (svc != null)
            {
                DataDictionary.Types.IDefaultValueElement defaultValueElement = value as DataDictionary.Types.IDefaultValueElement;
                if (defaultValueElement != null)
                {
                    EditorForm form = new EditorForm();
                    form.AutoComplete = true;
                    TextChangeHandler handler = new TextChangeHandler(defaultValueElement as ModelElement);
                    form.setChangeHandler(handler);
                    GUIUtils.MDIWindow.AddChildWindow(form);
                }
            }

            return value;
        }
    }
}
