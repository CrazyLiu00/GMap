//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.Sketch;


namespace Alt.GUI
{
    static partial class UnityHelper
    {		
		static Dictionary<UnityEngine.KeyCode, Alt.GUI.Keys> m_TranslationKeyCodeDict;
		static Dictionary<UnityEngine.KeyCode, Alt.GUI.Keys> TranslationKeyCodeDict
		{
			get
			{
				return m_TranslationKeyCodeDict;
			}
		}
		public static Alt.GUI.Keys TranslateKeyCode(UnityEngine.KeyCode key)
		{
			if (m_TranslationKeyCodeDict == null)
			{
				m_TranslationKeyCodeDict = new Dictionary<UnityEngine.KeyCode, Alt.GUI.Keys>();
				
				//     A key outside the known keys.
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.None, Alt.GUI.Keys.Unknown);
				
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Backspace, Alt.GUI.Keys.Backspace);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Delete, Alt.GUI.Keys.Delete);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Tab, Alt.GUI.Keys.Tab);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Clear, Alt.GUI.Keys.Clear);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Return, Alt.GUI.Keys.Return);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Pause, Alt.GUI.Keys.Pause);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Escape, Alt.GUI.Keys.Escape);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Space, Alt.GUI.Keys.Space);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad0, Alt.GUI.Keys.NumPad0);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad1, Alt.GUI.Keys.NumPad1);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad2, Alt.GUI.Keys.NumPad2);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad3, Alt.GUI.Keys.NumPad3);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad4, Alt.GUI.Keys.NumPad4);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad5, Alt.GUI.Keys.NumPad5);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad6, Alt.GUI.Keys.NumPad6);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad7, Alt.GUI.Keys.NumPad7);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad8, Alt.GUI.Keys.NumPad8);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Keypad9, Alt.GUI.Keys.NumPad9);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadPeriod, Alt.GUI.Keys.OemPeriod);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadDivide, Alt.GUI.Keys.OemBackslash);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadMultiply, Alt.GUI.Keys.Multiply);//???
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadMinus, Alt.GUI.Keys.OemMinus);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadPlus, Alt.GUI.Keys.OemPlus);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadEnter, Alt.GUI.Keys.Enter);//???
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.KeypadEquals, Alt.GUI.Keys.);//???
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.UpArrow, Alt.GUI.Keys.Up);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.DownArrow, Alt.GUI.Keys.Down);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightArrow, Alt.GUI.Keys.Right);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftArrow, Alt.GUI.Keys.Left);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Insert, Alt.GUI.Keys.Insert);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Home, Alt.GUI.Keys.Home);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.End, Alt.GUI.Keys.End);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.PageUp, Alt.GUI.Keys.PageUp);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.PageDown, Alt.GUI.Keys.PageDown);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F1, Alt.GUI.Keys.F1);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F2, Alt.GUI.Keys.F2);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F3, Alt.GUI.Keys.F3);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F4, Alt.GUI.Keys.F4);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F5, Alt.GUI.Keys.F5);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F6, Alt.GUI.Keys.F6);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F7, Alt.GUI.Keys.F7);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F8, Alt.GUI.Keys.F8);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F9, Alt.GUI.Keys.F9);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F10, Alt.GUI.Keys.F10);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F11, Alt.GUI.Keys.F11);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F12, Alt.GUI.Keys.F12);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F13, Alt.GUI.Keys.F13);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F14, Alt.GUI.Keys.F14);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F15, Alt.GUI.Keys.F15);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha0, Alt.GUI.Keys.D0);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha1, Alt.GUI.Keys.D1);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha2, Alt.GUI.Keys.D2);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha3, Alt.GUI.Keys.D3);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha4, Alt.GUI.Keys.D4);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha5, Alt.GUI.Keys.D5);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha6, Alt.GUI.Keys.D6);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha7, Alt.GUI.Keys.D7);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha8, Alt.GUI.Keys.D8);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Alpha9, Alt.GUI.Keys.D9);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Exclaim, Alt.GUI.Keys.);//"!"
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.DoubleQuote, Alt.GUI.Keys.Quotes);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Hash, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Dollar, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Ampersand, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Quote, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftParen, Alt.GUI.Keys.OpenBracket);//???
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightParen, Alt.GUI.Keys.CloseBracket);//???
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Asterisk, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Plus, Alt.GUI.Keys.Plus);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Comma, Alt.GUI.Keys.Comma);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Minus, Alt.GUI.Keys.OemMinus);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Period, Alt.GUI.Keys.Period);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Slash, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Colon, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Semicolon, Alt.GUI.Keys.Semicolon);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Less, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Equals, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Greater, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Question, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.At, Alt.GUI.Keys.Attn);//???
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftBracket, Alt.GUI.Keys.OpenBracket);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Backslash, Alt.GUI.Keys.Backslash);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightBracket, Alt.GUI.Keys.CloseBracket);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Caret, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Underscore, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.BackQuote, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.A, Alt.GUI.Keys.A);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.B, Alt.GUI.Keys.B);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.C, Alt.GUI.Keys.C);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.D, Alt.GUI.Keys.D);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.E, Alt.GUI.Keys.E);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.F, Alt.GUI.Keys.F);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.G, Alt.GUI.Keys.G);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.H, Alt.GUI.Keys.H);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.I, Alt.GUI.Keys.I);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.J, Alt.GUI.Keys.J);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.K, Alt.GUI.Keys.K);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.L, Alt.GUI.Keys.L);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.M, Alt.GUI.Keys.M);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.N, Alt.GUI.Keys.N);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.O, Alt.GUI.Keys.O);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.P, Alt.GUI.Keys.P);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Q, Alt.GUI.Keys.Q);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.R, Alt.GUI.Keys.R);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.S, Alt.GUI.Keys.S);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.T, Alt.GUI.Keys.T);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.U, Alt.GUI.Keys.U);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.V, Alt.GUI.Keys.V);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.W, Alt.GUI.Keys.W);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.X, Alt.GUI.Keys.X);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Y, Alt.GUI.Keys.Y);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Z, Alt.GUI.Keys.Z);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Numlock, Alt.GUI.Keys.NumLock);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.CapsLock, Alt.GUI.Keys.CapsLock);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.ScrollLock, Alt.GUI.Keys.ScrollLock);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightShift, Alt.GUI.Keys.RightShift);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftShift, Alt.GUI.Keys.LeftShift);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightControl, Alt.GUI.Keys.RightControl);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftControl, Alt.GUI.Keys.LeftControl);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightAlt, Alt.GUI.Keys.RightAlt);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftAlt, Alt.GUI.Keys.LeftAlt);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftCommand, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftApple, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.LeftWindows, Alt.GUI.Keys.LWin);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightCommand, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightApple, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.RightWindows, Alt.GUI.Keys.RWin);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.AltGr, Alt.GUI.Keys.Alt);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Help, Alt.GUI.Keys.Help);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Print, Alt.GUI.Keys.Print);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.SysReq, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Break, Alt.GUI.Keys.);
				m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Menu, Alt.GUI.Keys.Menu);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse0, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse1, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse2, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse3, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse4, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse5, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Mouse6, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton0, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton1, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton2, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton3, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton4, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton5, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton6, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton7, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton8, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton9, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton10, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton11, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton12, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton13, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton14, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton15, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton16, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton17, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton18, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.JoystickButton19, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button0, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button1, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button2, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button3, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button4, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button5, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button6, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button7, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button8, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button9, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button10, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button11, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button12, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button13, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button14, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button15, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button16, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button17, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button18, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick1Button19, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button0, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button1, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button2, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button3, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button4, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button5, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button6, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button7, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button8, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button9, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button10, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button11, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button12, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button13, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button14, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button15, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button16, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button17, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button18, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick2Button19, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button0, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button1, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button2, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button3, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button4, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button5, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button6, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button7, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button8, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button9, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button10, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button11, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button12, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button13, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button14, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button15, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button16, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button17, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button18, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick3Button19, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button0, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button1, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button2, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button3, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button4, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button5, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button6, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button7, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button8, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button9, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button10, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button11, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button12, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button13, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button14, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button15, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button16, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button17, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button18, Alt.GUI.Keys.);
				//m_TranslationKeyCodeDict.Add(UnityEngine.KeyCode.Joystick4Button19, Alt.GUI.Keys.);
			}
			
			Alt.GUI.Keys keys;
			if (m_TranslationKeyCodeDict.TryGetValue(key, out keys))
			{
				return keys;
			}
			
			return (Alt.GUI.Keys)key;
		}



		
		public static string ToCursorFileName(Alt.GUI.Cursor cursor)
		{
			StdCursor stdCursor = StdCursor.Unknown;//TEMP cursor.StdCursor;

			switch (stdCursor)
			{
			case Alt.GUI.StdCursor.Unknown:
			{
				//return "";
				return "AltCursors\\arrow_m.cur";
			}
			case Alt.GUI.StdCursor.Default:
			{
				return "";
			}
			case Alt.GUI.StdCursor.AppStarting:
			{
				return "";
			}
			case Alt.GUI.StdCursor.Arrow:
			{
				return "";
			}
			case Alt.GUI.StdCursor.Cross:
			{
				return "";
			}
			case Alt.GUI.StdCursor.Hand:
			{
				return "";
			}
			case Alt.GUI.StdCursor.Help:
			{
				return "";
			}
			case Alt.GUI.StdCursor.HSplit:
			{
				return "";
			}
			case Alt.GUI.StdCursor.IBeam:
			{
				return "";
			}
			case Alt.GUI.StdCursor.No:
			{
				return "";
			}
			case Alt.GUI.StdCursor.NoMove2D:
			{
				return "";
			}
			case Alt.GUI.StdCursor.NoMoveHoriz:
			{
				return "";
			}
			case Alt.GUI.StdCursor.NoMoveVert:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanEast:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanNE:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanNorth:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanNW:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanSE:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanSouth:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanSW:
			{
				return "";
			}
			case Alt.GUI.StdCursor.PanWest:
			{
				return "";
			}
			case Alt.GUI.StdCursor.SizeAll:
			{
				return "";
			}
			case Alt.GUI.StdCursor.SizeNESW:
			{
				return "";
			}
			case Alt.GUI.StdCursor.SizeNS:
			{
				return "";
			}
			case Alt.GUI.StdCursor.SizeNWSE:
			{
				return "";
			}
			case Alt.GUI.StdCursor.SizeWE:
			{
				return "";
			}
			case Alt.GUI.StdCursor.UpArrow:
			{
				return "";
			}
			case Alt.GUI.StdCursor.VSplit:
			{
				return "";
			}
			case Alt.GUI.StdCursor.WaitCursor:
			{
				return "";
			}
			}
			
			return "GUI/Cursors/DefaultSystem.cur";
		}
	}
}
