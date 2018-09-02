//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_MenuStrip : GUnit
    {
        public GUnit_MenuStrip(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.MenuStrip menu = new Alt.GUI.Temporary.Gwen.Control.MenuStrip(this);
            menu.Margin = Alt.GUI.Temporary.Gwen.Margin.Two;

            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem root = menu.AddItem("File");
                root.Menu.AddItem("New", "AltData/Gwen/test16.png", "Ctrl+N").SetAction(MenuItemSelect);
                root.Menu.AddItem("Load", "AltData/Gwen/test16.png", "Ctrl+L").SetAction(MenuItemSelect);
                root.Menu.AddItem("Save", String.Empty, "Ctrl+S").SetAction(MenuItemSelect);
                root.Menu.AddItem("Save As..", String.Empty, "Ctrl+A").SetAction(MenuItemSelect);
                root.Menu.AddItem("Quit", String.Empty, "Ctrl+Q").SetAction(MenuItemSelect);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem pRoot = menu.AddItem("\u043F\u0438\u0440\u0430\u0442\u0441\u0442\u0432\u043E");
                pRoot.Menu.AddItem("\u5355\u5143\u6D4B\u8BD5").SetAction(MenuItemSelect);
                pRoot.Menu.AddItem("\u0111\u01A1n v\u1ECB th\u1EED nghi\u1EC7m", "AltData/Gwen/test16.png").SetAction(MenuItemSelect);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem pRoot = menu.AddItem("Submenu");

				Alt.GUI.Temporary.Gwen.Control.MenuItem pCheckable = pRoot.Menu.AddItem("Checkable");
                pCheckable.IsCheckable = true;
                pCheckable.IsCheckable = true;

                {
					Alt.GUI.Temporary.Gwen.Control.MenuItem pRootB = pRoot.Menu.AddItem("Two");
                    pRootB.Menu.AddItem("Two.One");
                    pRootB.Menu.AddItem("Two.Two");
                    pRootB.Menu.AddItem("Two.Three");
                    pRootB.Menu.AddItem("Two.Four");
                    pRootB.Menu.AddItem("Two.Five");
                    pRootB.Menu.AddItem("Two.Six");
                    pRootB.Menu.AddItem("Two.Seven");
                    pRootB.Menu.AddItem("Two.Eight");
                    pRootB.Menu.AddItem("Two.Nine", "AltData/Gwen/test16.png");
                }

                pRoot.Menu.AddItem("Three");
                pRoot.Menu.AddItem("Four");
                pRoot.Menu.AddItem("Five");

                {
					Alt.GUI.Temporary.Gwen.Control.MenuItem pRootB = pRoot.Menu.AddItem("Six");
                    pRootB.Menu.AddItem("Six.One");
                    pRootB.Menu.AddItem("Six.Two");
                    pRootB.Menu.AddItem("Six.Three");
                    pRootB.Menu.AddItem("Six.Four");
                    pRootB.Menu.AddItem("Six.Five", "AltData/Gwen/test16.png");

                    {
						Alt.GUI.Temporary.Gwen.Control.MenuItem pRootC = pRootB.Menu.AddItem("Six.Six");
                        pRootC.Menu.AddItem("Sheep");
                        pRootC.Menu.AddItem("Goose");
                        {
							Alt.GUI.Temporary.Gwen.Control.MenuItem pRootD = pRootC.Menu.AddItem("Camel");
                            pRootD.Menu.AddItem("Eyes");
                            pRootD.Menu.AddItem("Nose");
                            {
								Alt.GUI.Temporary.Gwen.Control.MenuItem pRootE = pRootD.Menu.AddItem("Hair");
                                pRootE.Menu.AddItem("Blonde");
                                pRootE.Menu.AddItem("Black");
                                {
									Alt.GUI.Temporary.Gwen.Control.MenuItem pRootF = pRootE.Menu.AddItem("Red");
                                    pRootF.Menu.AddItem("Light");
                                    pRootF.Menu.AddItem("Medium");
                                    pRootF.Menu.AddItem("Dark");
                                }
                                pRootE.Menu.AddItem("Brown");
                            }
                            pRootD.Menu.AddItem("Ears");
                        }
                        pRootC.Menu.AddItem("Duck");
                    }

                    pRootB.Menu.AddItem("Six.Seven");
                    pRootB.Menu.AddItem("Six.Eight");
                    pRootB.Menu.AddItem("Six.Nine");
                }

                pRoot.Menu.AddItem("Seven");
            }
        }

		void MenuItemSelect(Alt.GUI.Temporary.Gwen.Control.Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.MenuItem item = control as Alt.GUI.Temporary.Gwen.Control.MenuItem;
            UnitPrint(String.Format("Menu item selected: {0}", item.Text));
        }
    }
}
