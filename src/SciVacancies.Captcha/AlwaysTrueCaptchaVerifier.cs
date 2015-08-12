namespace SciVacancies.Captcha
{
    public class AlwaysTrueCaptchaVerifier : ICaptchaVerifier
    {
        public bool IsValid(string captchaText)
        {
            return true;
        }
    }
}
