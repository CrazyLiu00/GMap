//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_Button : GUnit
    {
		Alt.GUI.Temporary.Gwen.Control.Button buttonA, buttonB, buttonC, buttonD, buttonE, buttonF, buttonG, buttonH;


        public GUnit_Button(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			buttonA = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonA.Text = "Event tester";
            buttonA.SetBounds(200, 30, 300, 200);
            buttonA.Pressed += onButtonAp;
            buttonA.Clicked += onButtonAc;
            buttonA.Released += onButtonAr;

			buttonB = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonB.Text = "\u0417\u0430\u043C\u0435\u0436\u043D\u0430\u044F \u043C\u043E\u0432\u0430";
            buttonB.SetPosition(0, 20);

			buttonC = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonC.Text = "Image button";
            buttonC.SetImage("AltData/Gwen/test16.png");
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(buttonC, buttonB, 10);

			buttonD = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonD.SetImage("AltData/Gwen/test16.png");
            buttonD.SetSize(20, 20);
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(buttonD, buttonC, 10);

			buttonE = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonE.Text = "Toggle me";
            buttonE.IsToggle = true;
            buttonE.Toggled += onToggle;
            buttonE.ToggledOn += onToggleOn;
            buttonE.ToggledOff += onToggleOff;
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(buttonE, buttonD, 10);

			buttonF = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonF.Text = "Disabled :D";
            buttonF.IsDisabled = true;
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(buttonF, buttonE, 10);

			buttonG = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonG.Text = "With Tooltip";
            buttonG.SetToolTipText("This is tooltip");
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(buttonG, buttonF, 10);

			buttonH = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            buttonH.Text = "I'm autosized";
            buttonH.SizeToContents();
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(buttonH, buttonG, 10);
        }

        void onButtonAc(Base control)
        {
            UnitPrint("Button: Clicked");
        }

        void onButtonAp(Base control)
        {
            UnitPrint("Button: Pressed");
        }

        void onButtonAr(Base control)
        {
            UnitPrint("Button: Released");
        }

        void onToggle(Base control)
        {
            UnitPrint("Button: Toggled");
        }

        void onToggleOn(Base control)
        {
            UnitPrint("Button: OnToggleOn");
        }

        void onToggleOff(Base control)
        {
            UnitPrint("Button: ToggledOff");
        }
    }
}
