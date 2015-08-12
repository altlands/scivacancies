namespace SciVacancies.Captcha.CaptchaStores
{
    public interface ICaptchaStore
    {
        void DeleteCaptcha(string captchaId);
        string GetCaptcha(string captchaId);
        void PutCaptcha(string captchaId, string captchaText);
    }
}
