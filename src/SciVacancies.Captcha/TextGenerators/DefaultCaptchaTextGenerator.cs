using System;
using System.Text;

namespace SciVacancies.Captcha.TextGenerators
{
    public class DefaultCaptchaTextGenerator : ICaptchaTextGenerator
    {
        private static readonly Random _rand = new Random();

        static DefaultCaptchaTextGenerator()
        {
            TextLength = CaptchaConfiguration.TextLength;
            TextChars = CaptchaConfiguration.Letters;
        }


        /// <summary>
        /// Gets or sets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public static int TextLength { get; set; }

        /// <summary>
        /// Gets or sets a string of available text characters for the generator to use.
        /// </summary>
        /// <value>The text chars.</value>
        public static string TextChars
        {
            get;
            set;
        }

        public string GenerateText()
        {
            var sb = new StringBuilder(TextLength);
            int maxLength = TextChars.Length;
            for (int n = 0; n <= TextLength - 1; n++)
                sb.Append(TextChars.Substring(_rand.Next(maxLength), 1));

            return sb.ToString();
        }
    }
}
