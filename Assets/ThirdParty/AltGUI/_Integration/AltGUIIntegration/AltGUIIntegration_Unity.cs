//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI.Integration;
using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI
{
    public static partial class AltGUIIntegration
    {
		public const string EditorComponentMenuPath = "AltGUI/Unity UI/";
		public const string EditorComponentMenuPathNGUI = "AltGUI/NGUI/";


		static bool IsInitialized
		{
			get
			{
				return UnityApplication.IsInitialized;
			}
		}


        static void InitializePlatformSpecificTools()
        {
            Alt.Sketch.Config.Font_NoAntiAliasMaxSize = 5;

#if UNITY_5
			Alt.Backend.BackendManager.Clipboard = new UnityClipboard();

			UnityEngine.TextAsset bindata = UnityEngine.Resources.Load("AltData.zip", typeof(UnityEngine.TextAsset)) as UnityEngine.TextAsset;
			if (bindata != null)
			{
				System.IO.MemoryStream stream = new System.IO.MemoryStream(bindata.bytes); 

				VirtualFileSystem.AddZipStorage(
					"AltData.zip",
					stream);
			}
#else
            VirtualFileSystem.AddZipStorage(
                "AltData.zip",
                VirtualFile.OpenRead("Assets/AltData.zip"));
#endif

			Alt.Sketch.Config.AppendLogo(Bitmap.FromFile("AltData/Logo/Unity.jpg"));

			UnityApplication.GetOrCreateInstance();
		}


        static void DoTick()
        {
            UnityApplication.DoEvents();
        }



		internal class UnityApplication : Alt.GUI.Application
		{
			class UnityPlatformDriver : Alt.GUI.Integration.ApplicationOSPlatformDefaultDriver
			{
			}

			static UnityApplication m_Instance;
			internal static UnityApplication Instance
			{
				get
				{
					return GetOrCreateInstance();
				}
			}

			internal static UnityApplication GetOrCreateInstance()
			{
				if (m_Instance == null)
				{
					ApplicationOSPlatformDriver.ApplicationInitPlatform = new UnityPlatformDriver();
					new UnityApplication();
				}

				return m_Instance;
			}


			internal static bool IsInitialized
			{
				get
				{
					return m_Instance != null;
				}
			}


			UnityApplication()
			{
				m_Instance = this;
			}
		}



#if UNITY_5
		sealed class UnityClipboard : Alt.Backend.Clipboard.VirtualClipboard
		{
			public override void Clear()
			{
				//TEST
				UnityEngine.TextEditor te = new UnityEngine.TextEditor();
				te.Paste();
			}
			
			
			public override bool ContainsText()
			{
				UnityEngine.TextEditor te = new UnityEngine.TextEditor();
				return te.CanPaste();
			}
			
			
			public override bool ContainsText(TextDataFormat format)
			{
				return base.ContainsText(format);//TEST	(format == TextDataFormat.Text || format == TextDataFormat.UnicodeText) ? 
			}
			
			
			public override string GetText()
			{
				UnityEngine.TextEditor te = new UnityEngine.TextEditor();
				te.Paste();
				return te.content.text;
			}
			
			
			public override string GetText(TextDataFormat format)
			{
				return base.GetText(format);//TEST	
			}
			
			
			public override void SetText(string text)
			{
				UnityEngine.TextEditor te = new UnityEngine.TextEditor();
				te.content = new UnityEngine.GUIContent(text);
				te.OnFocus();
				te.Copy();
			}
			
			
			public override void SetText(string text, TextDataFormat format)
			{
				base.SetText(text, format);//TEST
			}
		}
#endif
    }
}
