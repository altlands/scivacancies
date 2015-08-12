using System;
using Microsoft.AspNet.Http;

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
            if (captchaCookie == null) throw new InvalidOperationException("captcha key is not found");
            return captchaCookie;
        }

        public void WriteCaptchaCookie(string captchaId)
        {
            _context.Response.Cookies.Append(captchaKey, captchaId, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddSeconds(180) //TODO: вынести в конфиг время действия Капчи
            });
        }

    }
}
