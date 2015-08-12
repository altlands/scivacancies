namespace SciVacancies.Captcha
{
    public interface ICaptchaVerifier
    {
        bool IsValid(string captchaText);
    }
}
