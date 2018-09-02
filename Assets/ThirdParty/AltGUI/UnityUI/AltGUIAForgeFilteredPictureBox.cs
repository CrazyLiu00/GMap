//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI;
using Alt.GUI.AForge;
using Alt.Sketch;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltGUI AForge Filtered PictureBox.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIAForgeFilteredPictureBox.EditorName, AltGUIAForgeFilteredPictureBox.EditorID)]
	public class AltGUIAForgeFilteredPictureBox
		: AltGUIPictureBox
    {
		new public const string EditorName = "AForge Filtered PictureBox";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif


		ImageSource m_SrcImage;
		protected override ImageSource InternalImage
		{
			get
			{
				return m_SrcImage;
			}
			set
			{
				if (m_SrcImage == value)
				{
					return;
				}

				m_SrcImage = value;
				// make sure the image has 24 bpp format
				if (m_SrcImage.PixelFormat != PixelFormat.Format24bppRgb)
				{
					m_SrcImage = global::AForge.Imaging.Image.Clone(m_SrcImage as Bitmap, PixelFormat.Format24bppRgb);
				}

				UpdateImage();
			}
		}


		ImageFilterType m_ImageFilter = ImageFilterType.None;
		public ImageFilterType ImageFilter
		{
			get
			{
				return m_ImageFilter;
			}
			set
			{
				if (m_ImageFilter == value)
				{
					return;
				}

				m_ImageFilter = value;
				
				UpdateImage();
			}
		}
		

		void UpdateImage()
		{
			Alt.GUI.PictureBox pictureBox = PictureBox;
			if (pictureBox == null)
			{
				return;
			}

			pictureBox.Image = Alt.GUI.AForge.ImageFilter.Apply(m_SrcImage as Bitmap, m_ImageFilter);
		}
	}
}
