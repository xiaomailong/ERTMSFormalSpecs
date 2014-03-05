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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DataDictionary.Constants;
using DataDictionary.Variables;
using GUI.BoxArrowDiagram;
using DataDictionary.Rules;
using DataDictionary.Specification;
using DataDictionary;
using System.Collections.Generic;

namespace GUI.RequirementSetDiagram
{
    public partial class RequirementSetControl : BoxControl<RequirementSet, RequirementSetDependancy>
    {
        /// <summary>
        /// The metrics associates to this requirements set
        /// </summary>
        private GUI.SpecificationView.ParagraphTreeNode.ParagraphSetMetrics Metrics { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RequirementSetControl()
            : base()
        {
            MouseDoubleClick += new MouseEventHandler(HandleMouseDoubleClick);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public RequirementSetControl(IContainer container)
            : base(container)
        {
            MouseDoubleClick += new MouseEventHandler(HandleMouseDoubleClick);
        }

        public override RequirementSet Model
        {
            set
            {
                base.Model = value;
                List<Paragraph> paragraphs = new List<Paragraph>();
                Model.GetParagraphs(paragraphs);
                Metrics = GUI.SpecificationView.ParagraphTreeNode.CreateParagraphSetMetrics(EFSSystem.INSTANCE, paragraphs);
            }
        }

        public override void AcceptDrop(Utils.ModelElement element)
        {
            base.AcceptDrop(element);

            // Allows to allocate paragraphs in requirement sets
            Paragraph paragraph = element as Paragraph;
            if (paragraph != null)
            {
                if (!paragraph.BelongsToRequirementSet(Model.Name))
                {
                    RequirementSetReference reference = (RequirementSetReference)DataDictionary.Generated.acceptor.getFactory().createRequirementSetReference();
                    reference.Name = Model.FullName;
                    paragraph.appendRequirementSets(reference);
                }
            }
        }

        public override void SelectBox()
        {
            base.SelectBox();

            GUIUtils.MDIWindow.SetCoverageStatus(Model);
        }

        /// <summary>
        /// Handles a double click event on the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleMouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectBox();

            RequirementSetPanel panel = (RequirementSetPanel)Panel;
            if (panel != null)
            {
                RequirementSetDiagramWindow window = new RequirementSetDiagramWindow();
                GUIUtils.MDIWindow.AddChildWindow(window);
                window.SetEnclosing(Model);
                window.Text = Model.Name;
            }
        }

        /// <summary>
        /// Implemented color
        /// </summary>
        public static Color IMPLEMENTED_COLOR = Color.Green;
        public static Pen IMPLEMENTED_PEN = new Pen(IMPLEMENTED_COLOR);

        public override void PaintInBoxArrowPanel(PaintEventArgs e)
        {
            SetColor(Color.Transparent);
            e.Graphics.FillRectangle(new SolidBrush(NORMAL_COLOR), Location.X, Location.Y, Width, Height);
            e.Graphics.DrawRectangle(NORMAL_PEN, Location.X, Location.Y, Width, Height);

            double ratio = 1;
            if (Metrics.implementableCount > 0)
            {
                ratio = (double)Metrics.implementedCount / (double)Metrics.implementableCount;
            }

            int length = (int)((Width - 20) * ratio);
            e.Graphics.DrawRectangle(NORMAL_PEN, Location.X + 10, Location.Y + Height - 20, Width - 20, 10);
            e.Graphics.FillRectangle(new SolidBrush(IMPLEMENTED_COLOR), Location.X + 10, Location.Y + Height - 20, length, 10);
        }
    }
}
