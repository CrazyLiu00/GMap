//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using UnityEngine;
using UnityEngine.UI;

using Alt.GUI.QuickFont;
using Alt.Sketch;
using Alt.Threading;


[AddComponentMenu("AltGUI/Examples/Common/QuickFont Demo")]
public abstract class AltGUIQuickFontDemo : MonoBehaviour
{	
	protected abstract Alt.GUI.QuickFont.QuickFontControl QuickFontControl
	{
		get;
	}

	protected abstract double ClientWidth
	{
		get;
	}
	
	protected abstract double ClientHeight
	{
		get;
	}
	
	
	static QFont heading1;
	static QFont heading2;
	static QFont mainText;
	static QFont codeText;
	static QFont controlsText;
	static QFont monoSpaced;
	
	
	//string constants
	
	
	String introduction = @"Welcome to the AltGUI QuickFont tutorial. All text in this tutorial (including headings!) is drawn with QuickFont, so it is also intended to showcase the library. :) If you want to get started immediately, you can skip the rest of this introduction by pressing [Right]. You can also press [Left] to go back to previous pages at any point" + Environment.NewLine + Environment.NewLine +
		"Why QuickFont? QuickFont is intended as a replacement (and improvement upon) OpenTK's TextPrinter library. My primary motivation for writing it was for practical reasons: I'm using OpenTK to write a game, and currently the most annoying bugs are all being caused by TextPrinter: it is slow, it is buggy, and no one wants to maintain it." + Environment.NewLine + Environment.NewLine +
			"I did consider simply fixing it, but then decided that it would be easier and more fun to write my own library from scratch. That is exactly what I've done." + Environment.NewLine + Environment.NewLine +
			"In fact it's turned out to be well worth it. It has only taken me a few days to write the library, and already it has quite a few really cool features which I will be using in my game.";
	
	String usingQuickFontIsSuperEasy = @"Using QuickFont is super easy. To load a font: ";
	String loadingAFont1 = "myFont = new QFont(\"HappySans.ttf\", 16);";
	String andPrintWithIt = @"...and to print with it: ";
	String printWithFont1 = "QFont.Begin();" + Environment.NewLine + "myFont.Print(\"Hello World!\")" + Environment.NewLine + "QFont.End();";
	String itIsAlsoEasyToMeasure = "It is also very easy to measure text: ";
	String measureText1 = "var bounds = myFont.Measure(\"Hello World\"); ";
	
	String oneOfTheFirstGotchas = "One of the first \"gotchas\" that I experienced with the old TextPrinter was having to manage a private font collection. Unlike TextPrinter, QuickFont does not need the private font collection (or Font object for that matter) to exist after construction. QuickFont works out everything it needs at load time, hence you can just pass it a file name, it will load the pfc internally and then chuck it away immediately. If you still prefer to manage a font collection yourself, and you simply want to create a QuickFont from a font object, that's fine: QuickFont has a constructor for this:  ";
	String loadingAFont2 = "myFont = new QFont(fontObject);";
	
	String whenPrintingText = "When printing text, you can specify" + Environment.NewLine +
		"an alignment. Unbounded text can" + Environment.NewLine +
			"be left-aligned, right-aligned " + Environment.NewLine +
			"or centered. You specify the " + Environment.NewLine +
			"alignment as follows: ";
	
	
	String printWithFont2 = "myFont.Print(\"Hello World!\", QFontAlignment.Right)";
	
	String righAlignedText = "Right-aligned text will appear" + Environment.NewLine +
		"to the left of the original" + Environment.NewLine +
			"position, given by this red line.";
	
	
	String centredTextAsYou = "Centred text, as you would expect, is centred" + Environment.NewLine +
		"around the current position. The default alignment" + Environment.NewLine +
			"is Left. As you can see, you can include " + Environment.NewLine +
			"line-breaks in unbounded text.";
	
	
	String ofCourseItsNot = "Of course, it's not much fun having to insert your own line-breaks. A much better option is to simply specify the bounds of your text, and then let QuickFont decide where the line-breaks should go for you. You do this by specifying maxWidth. " + Environment.NewLine + Environment.NewLine +
		"You can still specify line-breaks for new paragraphs. For example, this is all written using a single print. QuickFont is also clever enough to spot where it might have accidentally inserted a line-break just before you have explicitly included one in the text. In this case, it will make sure that it does not insert a redundant line-break. :)" + Environment.NewLine + Environment.NewLine +
			"Another really cool feature of QuickFont, as you may have guessed already, is that it supports justified bounded text. It was quite tricky to get it all working pixel-perfectly, but as you can see, the results are pretty good. The precise justification settings are configurable in myFont.Options." + Environment.NewLine + Environment.NewLine +
			"You can press the [Up] and [Down] arrow keys to change the alignment on this block of bounded text. You can also press the [Enter] key to test some serious bounding! Note that the bound Height is always ignored.";
	
	
	
	String anotherCoolFeature = "QuickFont works by using the AltSketch to render to a bitmap, and then measuring and targeting each glyph before packing them into another bitmap which is then turned into an OpenGL texture. So essentially all fonts are \"texture\" fonts. However, QuickFont also allows you to get at the bitmaps before they are turned into OpenGL textures, save them to png file(s), modify them and then load (and retarget/remeasure) them again as QFonts. Sound complicated? - Don't worry, it's really easy. :)" + Environment.NewLine + Environment.NewLine +
		"Firstly, you need to create your new silhouette files from an existing font. You only want to call this code once, as calling it again will overwrite your modified .png, so take care. :) ";
	
	String textureFontCode1 = "QFont.CreateTextureFontFiles(\"HappySans.ttf\", 16, \"myTextureFont\");";
	
	
	String thisWillHaveCreated = "This will have created two files: \"myTextureFont.qfont\" and \"myTextureFont.png\" (or possibly multiple png files if your font is very large - I will explain how to configure this later). The next step is to actually texture your font. The png file(s) contain packed font silhouettes, perfect for layer effects in programs such as photoshop or GIMP. I suggest locking the alpha channel first, because QuickFont will complain if you merge two glyphs. You can enlarge glyphs at this stage, and QuickFont will automatically retarget each glyph when you next load the texture; however, it will fail if glyphs are merged...    ";
	
	String ifYouDoIntend = "...if you do intend to increase the size of the glyphs, then you can configure the silhouette texture to be generated with larger glyph margins to avoid glyphs merging. Here, I've also configured the texture sheet size a bit larger because the font is large and I want it all on one sheet for convenience: ";
	
	String textureFontCode2 = "QFontBuilderConfiguration config = new QFontBuilderConfiguration();" + Environment.NewLine +
		"config.GlyphMargin = 6;" + Environment.NewLine +
			"config.PageWidth = 1024;" + Environment.NewLine +
			"config.PageHeight = 1024;" + Environment.NewLine +
			"QFont.CreateTextureFontFiles(\"HappySans.ttf\", 48, config, \"myTextureFont\");";
	
	
	String actuallyTexturing = "Actually texturing the glyphs is really going to come down to how skilled you are in photoshop, and how good the original font was that you chose as a silhouette. To give you an idea: this very cool looking font I'm using for headings only took me 3 minutes to texture in photoshop because I did it with layer affects that did all glyphs at once. :)" + Environment.NewLine + Environment.NewLine +
		"Anyway, once you've finished texturing your font, save the png file. Now you can load the font and write with it just like any other font!";
	
	String textureFontCode3 = "myTexureFont = QFont.FromQFontFile(\"myTextureFont.qfont\");";
	
	String asIhaveleant = "As I have learnt, trying to create drop-shadows as part of the glyphs themselves gives very poor results because the glyphs become larger than usual and the font looks poor when printed. To do drop-shadows properly, they need to be rendered separately underneath each glyph. This is what QuickFont does. In fact it does a lot more: it will generate the drop-shadow textures for you. It's super-easy to create a font with a drop-shadow: ";
	String dropShadowCode1 = "myFont = new QFont(\"HappySans.ttf\", 16, new QFontBuilderConfiguration(true));";
	String thisWorksFine = "This works fine for texture fonts too: ";
	String dropShadowCode2 = "myTexureFont = QFont.FromQFontFile(\"myTextureFont.qfont\", new QFontLoaderConfiguration(true));";
	String onceAFont = "Once a font has been loaded with a drop-shadow, it will automatically be rendered with a shadow. However, you can turn this off or customise the drop-shadow in myFont.options when rendering (I am rotating the drop shadow here, which looks kind of cool but is now giving me a headache xD). I've turned drop-shadows on for this font on this page; however, they are very subtle because the font is so tiny. If you want the shadow to be more visible for tiny fonts like this, you could modify the DropShadowConfiguration object passed into the font constructor to blur the shadow less severely during creation. ";
	
	String thereAreActually = "There are actually a lot more interesting config values and neat things that QuickFont does. Now that I look back at it, it's a bit crazy that I got this all done in a few days, but this tutorial is getting a bit tedious to write and I'm dying to get on with making my game, so I'm going to leave it at this. " + Environment.NewLine + Environment.NewLine +
		"I suppose I should also mention that there are almost certainly going to be a few bugs. Let me know if you find any and I will get them fixed asap. :) " + Environment.NewLine + Environment.NewLine +
			"I should probably also say something about the code: it's not unit tested and it probably would need a good few hours of refactoring before it would be clean enough to be respectable. I will do this at some point. Also, feel free to berate me if I'm severely breaking any conventions. I'm a programmer by profession and really should know better. ;)" + Environment.NewLine + Environment.NewLine +
			"With regard to features: I'm probably not going to add many more to this library. It really is intended for rendering cool-looking text quickly for things like games. If you want highly formatted text, for example, then it probably isn't the right tool. I hope you find it useful; I know I already do! :P" + Environment.NewLine + Environment.NewLine +
			"A tiny disclaimer: all of QuickFont is written from scratch apart from ~100 lines I stole from TextPrinter for setting the correct perspective. Obviously the example itself is just a hacked around version of the original example that comes with OpenTK.";
	
	String hereIsSomeMono = "Here is some mononspaced text.  Monospaced fonts will automatically be rendered in monospace mode; however, you can render monospaced fonts ordinarily " +
		"or ordinary fonts in monospace mode using the render option:";
	String monoCode1 = " myFont.Options.Monospacing = QFontMonospacing.Yes; ";
	String theDefaultMono = "The default value for this is QFontMonospacing.Natural which simply means that if the underlying font was monospaced, then use monospacing. ";
	
	
	String mono = " **   **   **   *  *   **  " + Environment.NewLine +
		" * * * *  *  *  ** *  *  * " + Environment.NewLine +
			" *  *  *  *  *  * **  *  * " + Environment.NewLine +
			" *     *   **   *  *   **  ";
	
	
	int currentDemoPage = 1;
	int lastPage = 9;
	
	QFontAlignment cycleAlignment = QFontAlignment.Left;
	
	
	Pen m_LinesPen;


	protected virtual void Start()
	{
		m_LinesPen = new Pen(Alt.Sketch.Color.LimeGreen * 1.2);
		
		//	Initialization
		if (!m_Initialized)
		{
			if (!m_InitializationStarted)
			{
				InitializeInternal();
			}
			
			while (!m_Initialized)
			{
				Thread.Sleep(1);
			}
		}
	}


	public static void Initialize()
	{
#if !UNITY_WEBPLAYER
		InitializeInternal();
#endif
	}


	static void InitializeInternal()
	{
		m_InitializationStarted = true;

		try
		{
#if !UNITY_WEBPLAYER
			m_InitializeThread = new Thread(new ThreadStart(InitializeWorker));
			#if !SILVERLIGHT
			m_InitializeThread.SetApartmentState(ApartmentState.STA);
			#endif
			m_InitializeThread.Start();
#else
			InitializeWorker();
#endif
		}
		catch
		{
			m_Initialized = true;
		}
	}

#if !UNITY_WEBPLAYER
	static Thread m_InitializeThread;
#endif
	static bool m_InitializationStarted = false;
	static volatile bool m_Initialized = false;
	static void InitializeWorker()
	{
		try
		{
			heading2 = QFont.FromQFontFile("AltData/QuickFont/woodenFont.qfont", 1.0f, new QFontLoaderConfiguration(true));
			
			var builderConfig = new QFontBuilderConfiguration(true);
			builderConfig.ShadowConfig.blurRadius = 1; //reduce blur radius because font is very small
			builderConfig.TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit; //best render hint for this font
			
			//mainText = new QFont("AltData/QuickFont/Fonts/times.ttf", 14, builderConfig);
			mainText = new QFont("AltData/Fonts/Open-Sans/OpenSans-Regular.ttf", 14, builderConfig);
			
			
			heading1 = new QFont("AltData/QuickFont/Fonts/HappySans.ttf", 72, new QFontBuilderConfiguration(true));
			
			
			controlsText = new QFont("AltData/QuickFont/Fonts/HappySans.ttf", 32, new QFontBuilderConfiguration(true));
			
			
			codeText = new QFont("AltData/QuickFont/Fonts/Comfortaa-Regular.ttf", 12, Alt.Sketch.FontStyle.Regular);
			
			heading1.Options.Colour = Alt.Sketch.Color.LimeGreen * 1.2;
			mainText.Options.Colour = Alt.Sketch.Color.White;
			mainText.Options.DropShadowActive = false;
			codeText.Options.Colour = ColorR.Yellow;
			
			QFontBuilderConfiguration config2 = new QFontBuilderConfiguration();
			config2.SuperSampleLevels = 1;
			//   font = new QFont("Fonts/times.ttf", 16,config2);
			//   font.Options.Colour = new Color4(0.1, 0.1, 0.1, 1.0f);
			//   font.Options.CharacterSpacing = 0.1f;
			
			
			monoSpaced = new QFont("AltData/QuickFont/Fonts/Anonymous.ttf", 10);
			monoSpaced.Options.Colour = Alt.Sketch.Color.WhiteSmoke;// new ColorR(0.1, 0.1, 0.1);
			
			//Console.WriteLine(" Monospaced : " + monoSpaced.IsMonospacingActive);
		}
		catch
		{
		}
		finally
		{
			m_Initialized = true;
#if !UNITY_WEBPLAYER
			m_InitializeThread = null;
#endif
		}
	}


	double cnt;
	double boundsAnimationCnt = 1.0f;
	Alt.IntervalTimer m_Timer = new Alt.IntervalTimer();
	protected virtual void Update()
	{
		double delta = m_Timer.ElapsedTime;
		m_Timer.Reset();
		
		cnt += delta;

		if (boundsAnimationCnt < 1.0f)
		{
			boundsAnimationCnt += delta * 0.2f;
		}
		else
		{
			boundsAnimationCnt = 1.0f;
		}

		Alt.GUI.QuickFont.QuickFontControl quickFontControl = QuickFontControl;
		if (quickFontControl != null)
		{
			quickFontControl.Invalidate();
		}
	}


	protected void QuickFont_onKeyDown(Alt.GUI.KeyEventArgs e)
	{
		switch (e.KeyCode)
		{
		case Alt.GUI.Keys.Space:
		case Alt.GUI.Keys.Right:
			currentDemoPage++;
			break;
			
		case Alt.GUI.Keys.Backspace:
		case Alt.GUI.Keys.Left:
			currentDemoPage--;
			break;
			
		case Alt.GUI.Keys.Enter:
		{
			if (currentDemoPage == 4)
				boundsAnimationCnt = 0f;
			
		}
			break;
			
		case Alt.GUI.Keys.Up:
		{
			if (currentDemoPage == 4)
			{
				if (cycleAlignment == QFontAlignment.Justify)
					cycleAlignment = QFontAlignment.Left;
				else
					cycleAlignment++;
			}
			
			
		}
			break;
			
		case Alt.GUI.Keys.Down:
		{
			if (currentDemoPage == 4)
			{
				if (cycleAlignment == QFontAlignment.Left)
					cycleAlignment = QFontAlignment.Justify;
				else
					cycleAlignment--;
			}
			
			
		}
			break;
			
		case Alt.GUI.Keys.F9:
			
			break;
		}
		
		if (currentDemoPage > lastPage)
			currentDemoPage = lastPage;
		
		if (currentDemoPage < 1)
			currentDemoPage = 1;
	}

	
	
	void PrintWithBounds(Alt.Sketch.Graphics graphics, QFont font, string text, Alt.Sketch.Rect bounds, QFontAlignment alignment, ref double yOffset)
	{
		double maxWidth = bounds.Width;
		
		double height = font.Measure(text, maxWidth, alignment).Height;
		
		bounds.Height = height;
		graphics.DrawRectangle(m_LinesPen, bounds - new Point(1, 0) + new Size(0, 1));
		
		font.Print(text, maxWidth, alignment, new Alt.Sketch.Vector2(bounds.X, bounds.Y));
		
		yOffset += height;
	}
	
	
	//some helpers
	
	
	void PrintComment(Alt.Sketch.Graphics graphics, string comment, ref double yOffset)
	{
		PrintComment(graphics, mainText, comment, QFontAlignment.Justify, ref yOffset);
	}
	
	
	void PrintComment(Alt.Sketch.Graphics graphics, QFont font, string comment, QFontAlignment alignment, ref double yOffset)
	{
		GraphicsState state = graphics.Save();
		
		yOffset += 20;
		graphics.TranslateTransform(30, yOffset);
		
		font.Print(comment, ClientWidth - 60, alignment);
		
		yOffset += font.Measure(comment, ClientWidth - 60, alignment).Height;
		
		graphics.Restore(state);
	}
	
	
	void PrintCommentWithLine(Alt.Sketch.Graphics graphics, string comment, QFontAlignment alignment, double xOffset, ref double yOffset)
	{
		PrintCommentWithLine(graphics, mainText, comment, alignment, xOffset, ref yOffset);
	}
	
	
	void PrintCommentWithLine(Alt.Sketch.Graphics graphics, QFont font, string comment, QFontAlignment alignment, double xOffset, ref double yOffset)
	{
		GraphicsState state = graphics.Save();
		
		yOffset += 20;
		graphics.TranslateTransform((int)xOffset, yOffset);
		
		font.Print(comment, alignment);
		
		Size bounds = font.Measure(comment, ClientWidth - 60, alignment);
		graphics.DrawLine(m_LinesPen, 0, 0, 0, bounds.Height + 20);
		
		yOffset += bounds.Height;
		
		graphics.Restore(state);
	}
	
	
	void PrintCode(Alt.Sketch.Graphics graphics, string code, ref double yOffset)
	{
		GraphicsState state = graphics.Save();
		
		yOffset += 20;
		graphics.TranslateTransform(50f, yOffset);
		
		codeText.Print(code, ClientWidth - 50, QFontAlignment.Left);
		
		yOffset += codeText.Measure(code, ClientWidth - 50, QFontAlignment.Left).Height;
		
		graphics.Restore(state);
	}
	

	protected void QuickFont_onPaint(Alt.GUI.PaintEventArgs e)
	{
		Alt.Sketch.Graphics graphics = e.Graphics;
		GraphicsState state;
		
		switch (currentDemoPage)
		{
		case 1:
		{
			double yOffset = 0;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(ClientWidth * 0.5f, yOffset + 10);
			heading1.Print("AltGUI QuickFont", QFontAlignment.Centre);
			yOffset += heading1.Measure("QuickFont").Height;
			graphics.Restore(state);
			
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Introduction", QFontAlignment.Left);
			yOffset += heading2.Measure("Introduction").Height;
			graphics.Restore(state);
			
			
			state = graphics.Save();
			graphics.TranslateTransform(30f, yOffset + 20);
			mainText.Print(introduction, ClientWidth - 60, QFontAlignment.Justify);
			graphics.Restore(state);
			
			QFont.End();
		}
			break;
			
		case 2:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Easy as ABC!", QFontAlignment.Left);
			yOffset += heading2.Measure("Easy as ABC!").Height;
			graphics.Restore(state);
			
			
			PrintComment(graphics, usingQuickFontIsSuperEasy, ref yOffset);
			PrintCode(graphics, loadingAFont1, ref yOffset);
			
			PrintComment(graphics, andPrintWithIt, ref yOffset);
			PrintCode(graphics, printWithFont1, ref yOffset);
			
			PrintComment(graphics, itIsAlsoEasyToMeasure, ref yOffset);
			PrintCode(graphics, measureText1, ref yOffset);
			
			PrintComment(graphics, oneOfTheFirstGotchas, ref yOffset);
			PrintCode(graphics, loadingAFont2, ref yOffset);
			
			QFont.End();
		}
			break;
			
		case 3:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Alignment", QFontAlignment.Left);
			yOffset += heading2.Measure("Easy as ABC!").Height;
			graphics.Restore(state);
			
			PrintCommentWithLine(graphics, whenPrintingText, QFontAlignment.Left, ClientWidth * 0.5f, ref yOffset);
			PrintCode(graphics, printWithFont2, ref yOffset);
			
			
			PrintCommentWithLine(graphics, righAlignedText, QFontAlignment.Right, ClientWidth * 0.5f, ref yOffset);
			yOffset += 10f;
			
			PrintCommentWithLine(graphics, centredTextAsYou, QFontAlignment.Centre, ClientWidth * 0.5f, ref yOffset);
			
			QFont.End();
		}
			break;
			
		case 4:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Bounds and Justify", QFontAlignment.Left);
			yOffset += heading2.Measure("Easy as ABC!").Height;
			graphics.Restore(state);
			
			state = graphics.Save();
			yOffset += 20;
			graphics.TranslateTransform((int)(ClientWidth * 0.5), yOffset);
			controlsText.Options.Colour = Alt.Sketch.Color.Yellow;
			controlsText.Print("Press [Up], [Down] or [Enter]!", QFontAlignment.Centre);
			yOffset += controlsText.Measure("[]").Height;
			graphics.Restore(state);
			
			double boundShrink = (int)(350 * (1 - Math.Cos(boundsAnimationCnt * Math.PI * 2)));
			
			yOffset += 15;
			PrintWithBounds(graphics, mainText, ofCourseItsNot, new Alt.Sketch.Rect(30f + boundShrink * 0.5f, yOffset, ClientWidth - 60 - boundShrink, 350f), cycleAlignment, ref yOffset);
			
			string printWithBounds = "myFont.Print(text, 400f, QFontAlignment." + cycleAlignment + ");";
			yOffset += 15f;
			PrintCode(graphics, printWithBounds, ref yOffset);
			
			QFont.End();
		}
			break;
			
		case 5:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Your own Texture Fonts", QFontAlignment.Left);
			yOffset += heading2.Measure("T").Height;
			graphics.Restore(state);
			
			PrintComment(graphics, anotherCoolFeature, ref yOffset);
			PrintCode(graphics, textureFontCode1, ref yOffset);
			PrintComment(graphics, thisWillHaveCreated, ref yOffset);
			
			QFont.End();
		}
			break;
			
		case 6:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Your own Texture Fonts", QFontAlignment.Left);
			yOffset += heading2.Measure("T").Height;
			graphics.Restore(state);
			
			PrintComment(graphics, ifYouDoIntend, ref yOffset);
			PrintCode(graphics, textureFontCode2, ref yOffset);
			PrintComment(graphics, actuallyTexturing, ref yOffset);
			PrintCode(graphics, textureFontCode3, ref yOffset);
			
			QFont.End();
		}
			break;
			
		case 7:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			heading2.Options.DropShadowOffset = new Alt.Sketch.Vector2(0.1f + 0.2f * (float)Math.Sin(cnt), 0.1f + 0.2f * (float)Math.Cos(cnt));
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Drop Shadows", QFontAlignment.Left);
			yOffset += heading2.Measure("T").Height;
			graphics.Restore(state);
			
			heading2.Options.DropShadowOffset = new Alt.Sketch.Vector2(0.16f, 0.16f); //back to default
			
			mainText.Options.DropShadowActive = true;
			mainText.Options.DropShadowOpacity = 0.7f;
			mainText.Options.DropShadowOffset = new Alt.Sketch.Vector2(0.1f + 0.2f * (float)Math.Sin(cnt), 0.1f + 0.2f * (float)Math.Cos(cnt));
			
			PrintComment(graphics, asIhaveleant, ref yOffset);
			PrintCode(graphics, dropShadowCode1, ref yOffset);
			PrintComment(graphics, thisWorksFine, ref yOffset);
			PrintCode(graphics, dropShadowCode2, ref yOffset);
			PrintComment(graphics, onceAFont, ref yOffset);
			
			mainText.Options.DropShadowActive = false;
			
			QFont.End();
		}
			break;
			
		case 8:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			monoSpaced.Options.CharacterSpacing = 0.05f;
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("Monospaced Fonts", QFontAlignment.Left);
			yOffset += heading2.Measure("T").Height;
			graphics.Restore(state);
			
			
			PrintComment(graphics, monoSpaced, hereIsSomeMono, QFontAlignment.Left, ref yOffset);
			PrintCode(graphics, monoCode1, ref yOffset);
			PrintComment(graphics, monoSpaced, theDefaultMono, QFontAlignment.Left, ref yOffset);
			
			PrintCommentWithLine(graphics, monoSpaced, mono, QFontAlignment.Left, ClientWidth * 0.5f, ref yOffset);
			yOffset += 2f;
			PrintCommentWithLine(graphics, monoSpaced, mono, QFontAlignment.Right, ClientWidth * 0.5f, ref yOffset);
			yOffset += 2f;
			PrintCommentWithLine(graphics, monoSpaced, mono, QFontAlignment.Centre, ClientWidth * 0.5f, ref yOffset);
			yOffset += 2f;
			
			monoSpaced.Options.CharacterSpacing = 0.5f;
			PrintComment(graphics, monoSpaced, "As usual, you can adjust character spacing with myFont.Options.CharacterSpacing.", QFontAlignment.Left, ref yOffset);
			
			QFont.End();
		}
			break;
			
		case 9:
		{
			double yOffset = 20;
			
			QFont.Begin();
			
			state = graphics.Save();
			graphics.TranslateTransform(20f, yOffset);
			heading2.Print("In Conclusion", QFontAlignment.Left);
			yOffset += heading2.Measure("T").Height;
			graphics.Restore(state);
			
			PrintComment(graphics, thereAreActually, ref yOffset);
			
			QFont.End();
		}
			break;
		}
		
		
		QFont.Begin();
		
		Alt.Sketch.Color pressColor = new ColorR(0.8, 0.2, 0.2);// new ColorR(0.8, 0.1, 0.1);
		
		if (currentDemoPage != lastPage)
		{
			state = graphics.Save();
			graphics.TranslateTransform(ClientWidth - 15 - 16 * (float)(1 + Math.Sin(cnt * 4)), ClientHeight - controlsText.Measure("P").Height - 17);
			controlsText.Options.Colour = pressColor;
			controlsText.Print("Press [Right] ->", QFontAlignment.Right);
			graphics.Restore(state);
		}
		
		if (currentDemoPage != 1)
		{
			state = graphics.Save();
			graphics.TranslateTransform(15 + 16 * (float)(1 + Math.Sin(cnt * 4)), ClientHeight - controlsText.Measure("P").Height - 17);
			controlsText.Options.Colour = pressColor; 
			controlsText.Print("<- Press [Left]", QFontAlignment.Left);
			graphics.Restore(state);
		}
		
		QFont.End();
	}
}
