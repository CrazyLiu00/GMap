//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.InteractiveGeometry;


namespace Alt.GUI.Temporary.Gwen.Control
{
    /// <summary>
    /// InteractiveGeometryContainer control.
    /// </summary>
    public class InteractiveGeometryContainer : DoubleBufferedControl
    {
        InteractiveGeometryElementsContainer m_ElementsContainer = new InteractiveGeometryElementsContainer();

        
        /// <summary>
        /// InteractiveGeometryContainer constructor.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public InteractiveGeometryContainer(Base parent)
            : base(parent)
        {
            SetSize(100, 100);
            MouseInputEnabled = true;
            KeyboardInputEnabled = true;

            DoubleBuffered = false;
        }


        public void AddElement(InteractiveGeometryElement element)
        {
            m_ElementsContainer.AddElement(element);
        }

        public void RemoveElement(InteractiveGeometryElement element)
        {
            m_ElementsContainer.RemoveElement(element);
        }



        IntervalTimer m_TickTimer;
        protected override void Render(Skin.Base skin)
        {
            if (m_TickTimer == null)
            {
                m_TickTimer = new IntervalTimer(10);
                m_TickTimer.Start();
            }

            double delta = m_TickTimer.ElapsedTime;
            if (m_TickTimer.IsTimeOver)
            {
                OnTick(delta);
            }


            base.Render(skin);
        }



        protected virtual void OnTick(double delta)
        {
            m_ElementsContainer.DoTick(delta);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            m_ElementsContainer.DoPaint(e);
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            m_ElementsContainer.Resize(ClientRectangle.Size);

            Refresh();
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            m_ElementsContainer.InjectMouseMove(e);

            Refresh();
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            m_ElementsContainer.InjectMouseDown(e);

            Refresh();
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            m_ElementsContainer.InjectMouseUp(e);

            Refresh();
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            m_ElementsContainer.InjectMouseWheel(e);

            Refresh();
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            m_ElementsContainer.InjectMouseEnter(e);

            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            m_ElementsContainer.InjectMouseLeave(e);

            Refresh();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            m_ElementsContainer.InjectKeyDown(e);

            Refresh();
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            m_ElementsContainer.InjectKeyUp(e);

            Refresh();
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            m_ElementsContainer.InjectKeyPress(e);

            Refresh();
        }
    }
}
