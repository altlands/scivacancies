namespace SciVacancies.Captcha.TextGenerators
{
    /// <summary>
    /// generate random text for the CAPTCHA
    /// </summary>
    /// <returns></returns>
    public interface ICaptchaTextGenerator
    {
        string GenerateText();
    }
}
