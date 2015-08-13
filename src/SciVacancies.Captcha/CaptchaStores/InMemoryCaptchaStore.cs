using System;
using System.Collections.Generic;

namespace SciVacancies.Captcha.CaptchaStores
{
    public class InMemoryCaptchaStore : ICaptchaStore
    {
        private static readonly Dictionary<string, string> _captchaDictionary;

        static InMemoryCaptchaStore()
        {
            _captchaDictionary = new Dictionary<string, string>();
        }

        public void DeleteCaptcha(string captchaId)
        {
            _captchaDictionary.Remove(captchaId);
        }

        public string GetCaptcha(string captchaId)
        {
            string value;
            if (!_captchaDictionary.TryGetValue(captchaId, out value))
                throw new InvalidOperationException("captcha not found by key");
            return value;
        }

        public void PutCaptcha(string captchaId, string captchaText)
        {
            _captchaDictionary[captchaId] = captchaText;
        }
    }
}
