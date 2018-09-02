//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI;
using Alt.Sketch;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltGUI PictureBox.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIPictureBox.EditorName, AltGUIPictureBox.EditorID)]
	public class AltGUIPictureBox
		: AltGUIControlHost
    {
		new public const string EditorName = "PictureBox";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif

		
		public Alt.GUI.PictureBox PictureBox
		{
			get
			{
				return Child as Alt.GUI.PictureBox;
			}
		}


		public ImageSource Image
		{
			get
			{
				return InternalImage;
			}
			set
			{
				InternalImage = value;
			}
		}
		
		protected virtual ImageSource InternalImage
		{
			get
			{
				Alt.GUI.PictureBox pictureBox = PictureBox;
				if (pictureBox != null)
				{
					return pictureBox.Image;
				}
				
				return null;
			}
			set
			{
				Alt.GUI.PictureBox pictureBox = PictureBox;
				if (pictureBox != null)
				{
					pictureBox.Image = value;
				}
			}
		}


		public PictureBoxSizeMode SizeMode
		{
			get
			{
				Alt.GUI.PictureBox pictureBox = PictureBox;
				if (pictureBox != null)
				{
					return pictureBox.SizeMode;
				}
				
				return PictureBoxSizeMode.Normal;
			}
			set
			{
				Alt.GUI.PictureBox pictureBox = PictureBox;
				if (pictureBox != null)
				{
					pictureBox.SizeMode = value;
				}
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			Child = AltGUIHelper.Create_PictureBox();
		}
	}
}
