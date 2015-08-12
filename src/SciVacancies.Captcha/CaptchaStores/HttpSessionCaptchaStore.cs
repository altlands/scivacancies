using System;
using Microsoft.AspNet.Http;

namespace SciVacancies.Captcha.CaptchaStores
{
    public class HttpSessionCaptchaStore : ICaptchaStore
    {
        public readonly HttpContext Context;

        public HttpSessionCaptchaStore(HttpContext context)
        {
            Context = context;
        }

        public void DeleteCaptcha(string captchaId)
        {
            Session[captchaId] = null;
        }

        public ISessionCollection Session
        {
            get
            {
                if (Context?.Session == null) throw new InvalidOperationException("Http Sessions not available in this context");
                return Context.Session;
            }
        } 

        public string GetCaptcha(string captchaId)
        {
            var result = Session.GetString(captchaId);
            if (string.IsNullOrEmpty(result)) throw new InvalidOperationException("Captcha not found for key");
            return result;
        }

        public void PutCaptcha(string captchaId, string captchaText)
        {            
            Session.SetString(captchaId,captchaText);
        }
    }
}
