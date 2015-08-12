using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;

namespace SciVacancies.Captcha
{
    /// <summary>
    /// CAPTCHA Image
    /// </summary>
    /// <seealso href="http://www.codinghorror.com">Original By Jeff Atwood</seealso>
    public class CaptchaImage
    {
        #region Static

        private static Color[] _colors;
        private static string[] _fonts;

        private static readonly Random _rand = new Random();

        /// <summary>
        /// Initializes the <see cref="CaptchaImage"/> class.
        /// </summary>
        static CaptchaImage()
        {
            FontWarp = CaptchaConfiguration.FontFactor;
            BackgroundNoise = CaptchaConfiguration.BackgroundNoise;
            LineNoise = CaptchaConfiguration.LineNoise;           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaImage"/> class.
        /// </summary>
        public CaptchaImage(string text)
        {
            Width = 180;
            Height = 50;
            Text = text;
        }

        

        /// <summary>
        /// Gets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public int TextLength
        {
            get
            {
                return Text.Length;
            }
        }

        /// <summary>
        /// Gets and sets amount of random warping to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The font warp.</value>
        public static FontWarpFactor FontWarp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets amount of background noise to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The background noise.</value>
        public static BackgroundNoiseLevel BackgroundNoise
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets amount of line noise to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The line noise.</value>
        public static LineNoiseLevel LineNoise
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cache time out.
        /// </summary>
        /// <value>The cache time out.</value>
        public static double CacheTimeOut
        {
            get;
            set;
        }

        #endregion

        private int _height;
        private int _width;

        #region Public Properties

        /// <summary>
        /// Gets the randomly generated Captcha text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// Width of Captcha image to generate, in pixels
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return _width; }
            set
            {
                if ((value > 60)) _width = value;
            }
        }

        /// <summary>
        /// Height of Captcha image to generate, in pixels
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return _height; }
            set
            {
                if (value > 30) _height = value;
            }
        }

        #endregion

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <returns></returns>
        public Bitmap RenderImage()
        {
            return GenerateImagePrivate();
        }

        /// <summary>
        /// Returns a random font family from the font whitelist
        /// </summary>
        /// <returns></returns>
        private static string GetRandomFontFamily()
        {
            return FontFamilies[_rand.Next(0, FontFamilies.Length)];
        }

        /// <summary>
        /// Randoms the color.
        /// </summary>
        /// <returns></returns>
        private static Color GetRandomColor()
        {
            return Colors[_rand.Next(0, Colors.Length)];
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        /// <param name="xmin">The xmin.</param>
        /// <param name="xmax">The xmax.</param>
        /// <param name="ymin">The ymin.</param>
        /// <param name="ymax">The ymax.</param>
        /// <returns></returns>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(_rand.Next(xmin, xmax), _rand.Next(ymin, ymax));
        }

        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="f">The f.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);
            return gp;
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            string fname = GetRandomFontFamily();
            int minFont = _height * CaptchaConfiguration.FontSizeMin / 100;
            int maxFont = _height * CaptchaConfiguration.FontSizeMax / 100;
            float fsize = _rand.Next(minFont, maxFont);

            return new Font(fname, fsize, _rand.Next(0, 2) == 1 ? FontStyle.Bold : FontStyle.Regular);
        }

        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        /// <returns></returns>
        private Bitmap GenerateImagePrivate()
        {
            var bmp = new Bitmap(_width, _height, PixelFormat.Format24bppRgb);

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.Clear(Color.White);

                int charOffset = 0;
                double charWidth = _width / TextLength;

                var rect = new Rectangle(new Point(0, 0), bmp.Size);
                AddNoise(gr, rect);

                double angle = _rand.Next(CaptchaConfiguration.MinAngle, CaptchaConfiguration.MaxAngle);
                double angle2 = _rand.Next(CaptchaConfiguration.MinAngle, CaptchaConfiguration.MaxAngle);
                if (angle * angle2 > 0) angle2 = -angle2;
                angle2 = (angle2 - angle) / (TextLength - 1);

                AddLine(gr, rect);
                foreach (char c in Text)
                {
                    // establish font and draw area
                    // _rand.Next(CaptchaConfiguration.minAngle, CaptchaConfiguration.maxAngle);
                    using (Font fnt = GetFont())
                    {
                        using (Brush fontBrush = new SolidBrush(GetRandomColor()))
                        {
                            int charShift = _rand.Next(CaptchaConfiguration.MinShift, CaptchaConfiguration.MaxShift);
                            int sX = Convert.ToInt32(charOffset * charWidth) - charShift;
                            var rectChar = new Rectangle(sX, 0, Convert.ToInt32(charWidth), _height);

                            // warp the character
                            GraphicsPath gp = TextPath(c.ToString(CultureInfo.InvariantCulture), fnt, rectChar);
                            int ssx = Convert.ToInt32(Math.Floor(WarpText(gp, rectChar, angle)));

                            // draw the character
                            gr.FillPath(fontBrush, gp);

                            charOffset += 1;

                        }
                    }
                    angle += angle2;
                }


            }

            return bmp;
        }

        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount
        /// </summary>
        /// <param name="textPath">The text path.</param>
        /// <param name="rect">The rect.</param>
        private float WarpText(GraphicsPath textPath, Rectangle rect, double angle)
        {
            float warpDivisor;
            float rangeModifier;

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    warpDivisor = 6F;
                    rangeModifier = 1F;
                    break;
                case FontWarpFactor.Medium:
                    warpDivisor = 5F;
                    rangeModifier = 1.3F;
                    break;
                case FontWarpFactor.High:
                    warpDivisor = 4.5F;
                    rangeModifier = 1.4F;
                    break;
                case FontWarpFactor.Extreme:
                    warpDivisor = 4F;
                    rangeModifier = 1.5F;
                    break;
                default:
                    return textPath.GetBounds().Width;
            }

            var rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / warpDivisor);
            int wrange = Convert.ToInt32(rect.Width / warpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * rangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * rangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wrange * rangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hrange * rangeModifier);

            if (left < 0)
                left = 0;
            if (top < 0)
                top = 0;
            if (width > Width)
                width = Width;
            if (height > Height)
                height = Height;

            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            PointF rightBottom = RandomPoint(width - wrange, width, height - hrange, height);

            var points = new PointF[] { leftTop, rightTop, leftBottom, rightBottom };
            var m = new Matrix();
            m.Translate(0, 0);

            m.RotateAt((float)angle, new PointF(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2), MatrixOrder.Append);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
            return textPath.GetBounds().Width;
        }

        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        /// <param name="g">The graphics.</param>
        /// <param name="rect">The rect.</param>
        private static void AddNoise(Graphics g, Rectangle rect)
        {
            int density;
            int size;
            int min;

            switch (BackgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    goto default;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 1;
                    min = 1;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 2;
                    min = 1;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 3;
                    min = 1;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 14;
                    size = 4;
                    min = 1;
                    break;
                default:
                    return;
            }

            using (var br = new SolidBrush(GetRandomColor()))
            {
                int max = size * Convert.ToInt32(Math.Min(rect.Width, rect.Height) / 20);

                for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / (density * max)); i++)
                    g.FillEllipse(br, _rand.Next(rect.Width), _rand.Next(rect.Height), _rand.Next(min, max),
                        _rand.Next(min, max));
            }
        }

        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>
        /// <param name="g">The graphics.</param>
        /// <param name="rect">The rect.</param>
        private void AddLine(Graphics g, Rectangle rect)
        {
            int length;
            float width;
            int linecount;

            switch (LineNoise)
            {
                case LineNoiseLevel.None:
                    goto default;
                case LineNoiseLevel.Low:
                    length = 1;
                    width = 1;
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 2;
                    width = 1;
                    linecount = 2;
                    break;
                case LineNoiseLevel.High:
                    length = 2;
                    width = 2;
                    linecount = 3;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = 3;
                    linecount = 3;
                    break;
                default:
                    return;
            }

            var pf = new PointF[length + 1];
            using (var p = new Pen(GetRandomColor(), width))
            {
                for (int l = 1; l <= linecount; l++)
                {
                    for (int i = 0; i <= length; i++) pf[i] = RandomPoint(rect);
                    g.DrawCurve(p, pf, 0.0F);
                }
            }
        }

        private static string[] FontFamilies
        {
            get
            {
                if (_fonts == null) _fonts = CaptchaConfiguration.Fonts.Split(new char[] { ';' });
                for (int i = 0; i < _fonts.Length; i++)
                {
                    _fonts[i] = _fonts[i].Trim();
                }
                return _fonts;
            }
        }

        private static Color[] Colors
        {
            get
            {
                if (_colors == null)
                {
                    string[] scolors = CaptchaConfiguration.Colors.Split(new char[] { ';' });

                    _colors = new Color[scolors.Length];
                    for (int i = 0; i < scolors.Length; i++)
                    {
                        _colors[i] = GetColorFromString(scolors[i].Trim());
                    }
                }
                return _colors;
            }
        }

        private static Color GetColorFromString(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException("str cannot be null or empty");

            string[] scolors = str.Split(new char[] { '.' });
            int red = Int32.Parse(scolors[0]);
            int green = Int32.Parse(scolors[1]);
            int blue = Int32.Parse(scolors[2]);

            return Color.FromArgb(red, green, blue);
        }
    }

}
