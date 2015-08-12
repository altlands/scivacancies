namespace SciVacancies.Captcha.TextGenerators
{
    public class FixedTextCaptchaTextGenerator : ICaptchaTextGenerator
    {
        private readonly string _text;

        public FixedTextCaptchaTextGenerator(string text)
        {
            _text = text;
        }

        public string GenerateText()
        {
            return _text;
        }
    }
}
