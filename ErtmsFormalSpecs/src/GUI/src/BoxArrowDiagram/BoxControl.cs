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

namespace GUI.BoxArrowDiagram
{
    public partial class BoxControl<BoxModel, ArrowModel> : Label
        where BoxModel : class, DataDictionary.IGraphicalDisplay
        where ArrowModel : class, DataDictionary.IGraphicalArrow<BoxModel>
    {
        /// <summary>
        /// The mode of displaying boxes
        /// </summary>
        protected enum BoxModeEnum { Rectangle, RoundedCorners };

        /// <summary>
        /// The mode of displaying boxes
        /// </summary>
        protected BoxModeEnum BoxMode { get; set; }

        /// <summary>
        /// The size of an box control button
        /// </summary>
        public static Size DEFAULT_SIZE = new Size(100, 50);

        /// <summary>
        /// The grid size used to place boxes
        /// </summary>
        public static int GRID_SIZE = 10;

        /// <summary>
        /// Provides the enclosing box-arrow panel
        /// </summary>
        public BoxArrowPanel<BoxModel, ArrowModel> Panel
        {
            get { return GUIUtils.EnclosingFinder<BoxArrowPanel<BoxModel, ArrowModel>>.find(this); }
        }

        /// <summary>
        /// Provides the enclosing form
        /// </summary>
        public Form EnclosingForm
        {
            get { return GUIUtils.EnclosingFinder<Form>.find(this); }
        }

        /// <summary>
        /// The model for this control
        /// </summary>
        private BoxModel __model;
        public BoxModel Model
        {
            get { return __model; }
            set
            {
                __model = value;
            }
        }

        /// <summary>
        /// Refreshes the control according to the related model
        /// </summary>
        public void RefreshControl()
        {
            if (Model.Width == 0 || Model.Height == 0)
            {
                Model.Width = DEFAULT_SIZE.Width;
                Model.Height = DEFAULT_SIZE.Height;
            }
            Size = new Size(Model.Width, Model.Height);

            if (Model.X == 0 || Model.Y == 0)
            {
                Point p = Panel.GetNextPosition();
                Model.X = p.X;
                Model.Y = p.Y;
            }
            SetPosition(Model.X, Model.Y);

            this.TextAlign = ContentAlignment.MiddleCenter;
            Text = Model.GraphicalName;
        }

        /// <summary>
        /// Sets the color of the control
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(Color color)
        {
            if (color != BackColor)
            {
                BackColor = color;
            }
        }

        /// <summary>
        /// A normal pen
        /// </summary>
        public static Color NORMAL_COLOR = Color.LightGray;
        public static Pen NORMAL_PEN = new Pen(Color.Black);

        /// <summary>
        /// A activated pen
        /// </summary>
        public static Color ACTIVATED_COLOR = Color.Blue;
        public static Pen ACTIVATED_PEN = new Pen(Color.Black, 4);

        /// <summary>
        /// Indicates that the box should be displayed in the ACTIVE color
        /// </summary>
        /// <returns></returns>
        public virtual bool IsActive()
        {
            return false;
        }

        private const int ROUND_SIZE = 10;

        /// <summary>
        /// Draws the box within the box-arrow panel
        /// </summary>
        /// <param name="e"></param>
        public void PaintInBoxArrowPanel(PaintEventArgs e)
        {
            Pen pen = NORMAL_PEN;
            switch (BoxMode)
            {
                case BoxModeEnum.Rectangle:
                    e.Graphics.DrawRectangle(pen, Location.X, Location.Y, Width, Height);
                    if (IsActive())
                    {
                        pen = ACTIVATED_PEN;
                        SetColor(ACTIVATED_COLOR);
                    }
                    else
                    {
                        SetColor(NORMAL_COLOR);
                    }

                    break;

                case BoxModeEnum.RoundedCorners:
                    Point[] points = new Point[] {
                        new Point (Location.X + ROUND_SIZE , Location.Y),
                        new Point (Location.X + Width - ROUND_SIZE , Location.Y),
                        new Point (Location.X + Width, Location.Y + ROUND_SIZE ),
                        new Point (Location.X + Width, Location.Y + Height - ROUND_SIZE ),
                        new Point (Location.X + Width - ROUND_SIZE , Location.Y + Height),
                        new Point (Location.X + ROUND_SIZE , Location.Y + Height),
                        new Point (Location.X, Location.Y + Height - ROUND_SIZE ),
                        new Point (Location.X , Location.Y + ROUND_SIZE ),
                    };

                    Brush innerBrush = new SolidBrush(NORMAL_COLOR);
                    e.Graphics.FillRectangle(innerBrush, new Rectangle(points[0], new Size(points[4].X - points[0].X, points[4].Y - points[0].Y)));
                    e.Graphics.FillRectangle(innerBrush, new Rectangle(points[7], new Size(points[3].X - points[7].X, points[3].Y - points[7].Y)));

                    e.Graphics.DrawLine(pen, points[0], points[1]);
                    e.Graphics.DrawLine(pen, points[2], points[3]);
                    e.Graphics.DrawLine(pen, points[4], points[5]);
                    e.Graphics.DrawLine(pen, points[6], points[7]);

                    Size rectangleSize = new Size(2 * ROUND_SIZE, 2 * ROUND_SIZE);
                    Rectangle rectangle;
                    rectangle = new Rectangle(new Point(points[0].X - ROUND_SIZE, points[0].Y), rectangleSize);
                    e.Graphics.FillPie(innerBrush, rectangle, 180.0f, 90.0f);
                    e.Graphics.DrawArc(pen, rectangle, 180.0f, 90.0f);

                    rectangle = new Rectangle(new Point(points[2].X - 2 * ROUND_SIZE, points[2].Y - ROUND_SIZE), rectangleSize);
                    e.Graphics.FillPie(innerBrush, rectangle, 270.0f, 90.0f);
                    e.Graphics.DrawArc(pen, rectangle, 270.0f, 90.0f);

                    rectangle = new Rectangle(new Point(points[4].X - ROUND_SIZE, points[4].Y - 2 * ROUND_SIZE), rectangleSize);
                    e.Graphics.FillPie(innerBrush, rectangle, 0.0f, 90.0f);
                    e.Graphics.DrawArc(pen, rectangle, 0.0f, 90.0f);

                    rectangle = new Rectangle(new Point(points[6].X, points[6].Y - ROUND_SIZE), rectangleSize);
                    e.Graphics.FillPie(innerBrush, rectangle, 90.0f, 90.0f);
                    e.Graphics.DrawArc(pen, rectangle, 90.0f, 90.0f);

                    break;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BoxControl()
        {
            InitializeComponent();

            BoxMode = BoxModeEnum.Rectangle;
            MouseDown += new MouseEventHandler(HandleMouseDown);
            MouseUp += new MouseEventHandler(HandleMouseUp);
            MouseMove += new MouseEventHandler(HandleMouseMove);
            MouseClick += new MouseEventHandler(HandleMouseClick);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public BoxControl(IContainer container)
        {
            container.Add(this);

            BoxMode = BoxModeEnum.Rectangle;
            InitializeComponent();
            MouseDown += new MouseEventHandler(HandleMouseDown);
            MouseUp += new MouseEventHandler(HandleMouseUp);
            MouseMove += new MouseEventHandler(HandleMouseMove);
            MouseClick += new MouseEventHandler(HandleMouseClick);
        }

        /// <summary>
        /// Selects the current box 
        /// </summary>
        public void SelectBox()
        {
            Panel.Select(this);
        }

        /// <summary>
        /// The location where the mouse down occured
        /// </summary>
        private Point moveStartLocation;

        /// <summary>
        /// The control location where the mouse down occured
        /// </summary>
        private Point positionBeforeMove;

        /// <summary>
        /// In a move operation ? 
        /// </summary>
        private bool moving = false;

        /// <summary>
        /// Handles a mouse down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            moveStartLocation = e.Location;
            positionBeforeMove = new Point(Model.X, Model.Y);

        }

        /// <summary>
        /// Handles a mouse up event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            if (Model.X != positionBeforeMove.X || Model.Y != positionBeforeMove.Y)
            {
                Panel.ControlHasMoved();
            }
        }

        /// <summary>
        /// Handles a mouse move event, when 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                Point mouseMoveLocation = e.Location;

                int deltaX = mouseMoveLocation.X - moveStartLocation.X;
                int deltaY = mouseMoveLocation.Y - moveStartLocation.Y;

                if (Math.Abs(deltaX) > 5 || Math.Abs(deltaY) > 5)
                {
                    int newX = Model.X + deltaX;
                    int newY = Model.Y + deltaY;
                    if (Panel.Location.X <= newX && Panel.Location.Y <= newY)
                    {
                        SetPosition(newX, newY);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the position of the control, according to the X & Y provided
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetPosition(int x, int y)
        {
            int posX = x / GRID_SIZE;
            posX = posX * GRID_SIZE;

            int posY = y / GRID_SIZE;
            posY = posY * GRID_SIZE;

            Model.X = posX;
            Model.Y = posY;

            Location = new Point(Model.X, Model.Y);
        }

        /// <summary>
        /// Provides the center of the box control
        /// </summary>
        public Point Center
        {
            get
            {
                Point retVal = Location;

                retVal.X = retVal.X + Width / 2;
                retVal.Y = retVal.Y + Height / 2;

                return retVal;
            }
        }


        /// <summary>
        /// Handles a mouse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleMouseClick(object sender, MouseEventArgs e)
        {
            SelectBox();
        }

        /// <summary>
        /// Provides the span of this control, over the X axis
        /// </summary>
        public Span XSpan
        {
            get { return new Span(Location.X, Location.X + Width); }
        }

        /// <summary>
        /// Provides the span of this control, over the Y axis
        /// </summary>
        public Span YSpan
        {
            get { return new Span(Location.Y, Location.Y + Height); }
        }
    }
}
