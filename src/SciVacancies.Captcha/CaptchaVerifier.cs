using Microsoft.AspNet.Http;
using SciVacancies.Captcha.CaptchaStores;

namespace SciVacancies.Captcha
{
    public class CaptchaVerifier : ICaptchaVerifier
    {
        private readonly ICaptchaStore _store;
        private readonly HttpContext _context;

        public CaptchaVerifier(ICaptchaStore store, HttpContext context)
        {
            _store = store;
            _context = context;
        }
        
        public bool IsValid(string captchaText)
        {
            var key = new CaptchaContext(_context).GetCaptchaKey();
            try
            {
                var storedValue = _store.GetCaptcha(key);
                return string.Equals(storedValue,captchaText);
            }
            finally
            {
                _store.DeleteCaptcha(key);
            }

        }
    }
}
