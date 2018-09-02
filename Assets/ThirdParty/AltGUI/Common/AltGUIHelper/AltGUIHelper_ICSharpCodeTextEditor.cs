//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI
{
	static partial class AltGUIHelper
	{
		internal static Alt.GUI.ICSharpCode.TextEditor.TextEditorControl Create_ICSharpCodeTextEditorControl()
		{
			Alt.GUI.ICSharpCode.TextEditor.TextEditorControl textEditorControl = new Alt.GUI.ICSharpCode.TextEditor.TextEditorControl();

			textEditorControl.BorderStyle = BorderStyle.None;
			
			textEditorControl.IsReadOnly = false;
			textEditorControl.Document.DocumentChanged += 
				new Alt.GUI.ICSharpCode.TextEditor.Document.DocumentEventHandler((sender, e) => { //SetModifiedFlag(editor, true);
				});
			
			// Alt.GUI.ICSharpCode.TextEditor doesn't have any built-in code folding
			// strategies, so I've included a simple one. Apparently, the
			// foldings are not updated automatically, so in this demo the user
			// cannot add or remove folding regions after loading the file.
			textEditorControl.Document.FoldingManager.FoldingStrategy = new RegionFoldingStrategy();
			textEditorControl.Document.FoldingManager.UpdateFoldings(null, null);

			return textEditorControl;
		}

		
		
		/// <summary>
		/// The class to generate the foldings, it implements Alt.GUI.ICSharpCode.TextEditor.Document.IFoldingStrategy
		/// </summary>
		public class RegionFoldingStrategy : Alt.GUI.ICSharpCode.TextEditor.Document.IFoldingStrategy
		{
			/// <summary>
			/// Generates the foldings for our document.
			/// </summary>
			/// <param name="document">The current document.</param>
			/// <param name="fileName">The filename of the document.</param>
			/// <param name="parseInformation">Extra parse information, not used in this sample.</param>
			/// <returns>A list of FoldMarkers.</returns>
			public List<Alt.GUI.ICSharpCode.TextEditor.Document.FoldMarker> GenerateFoldMarkers(Alt.GUI.ICSharpCode.TextEditor.Document.IDocument document, string fileName, object parseInformation)
			{
				List<Alt.GUI.ICSharpCode.TextEditor.Document.FoldMarker> list = new List<Alt.GUI.ICSharpCode.TextEditor.Document.FoldMarker>();
				
				Stack<int> startLines = new Stack<int>();
				
				// Create foldmarkers for the whole document, enumerate through every line.
				for (int i = 0; i < document.TotalNumberOfLines; i++)
				{
					var seg = document.GetLineSegment(i);
					int offs, end = document.TextLength;
					char c;
					for (offs = seg.Offset; offs < end && ((c = document.GetCharAt(offs)) == ' ' || c == '\t'); offs++)
					{
					}
					if (offs == end)
					{
						break;
					}
					
					int spaceCount = offs - seg.Offset;
					
					// now offs points to the first non-whitespace char on the line
					if (document.GetCharAt(offs) == '#')
					{
						string text = document.GetText(offs, seg.Length - spaceCount);
						if (text.StartsWith("#region"))
						{
							startLines.Push(i);
						}
						
						if (text.StartsWith("#endregion") && startLines.Count > 0)
						{
							// Add a new FoldMarker to the list.
							int start = startLines.Pop();
							list.Add(new Alt.GUI.ICSharpCode.TextEditor.Document.FoldMarker(document, start, 
							                                                                document.GetLineSegment(start).Length, 
							                                                                i, spaceCount + "#endregion".Length));
						}
					}
				}
				
				return list;
			}
		}
	}
}
