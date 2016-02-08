using System;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace SciVacancies.Captcha
{
    public class CaptchaContext
    {
        private readonly HttpContext _context;
        private const string captchaKey = "captchakey";

        public CaptchaContext(HttpContext context)
        {
            _context = context;
        }

        public string GetCaptchaKey()
        {
            var captchaCookie = _context.Request.Cookies[captchaKey];
            if (captchaCookie == StringValues.Empty) throw new InvalidOperationException("captcha key is not found");
            return captchaCookie;
        }

        [Obsolete("указывать период актуальности кода из конфига приложения")]
        public void WriteCaptchaCookie(string captchaId)
        {
            WriteCaptchaCookie(captchaId, 180);
        }

        public void WriteCaptchaCookie(string captchaId, int captchaDurationSeconds)
        {
            _context.Response.Cookies.Append(captchaKey, captchaId, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddSeconds(captchaDurationSeconds) 
            });
        }



    }
}
