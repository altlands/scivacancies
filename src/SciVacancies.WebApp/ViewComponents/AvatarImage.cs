using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;

namespace SciVacancies.WebApp.ViewComponents
{
    public class AvatarImage : ViewComponent
    {
        private readonly IOptions<AttachmentSettings> _attachmentSettings;
        private readonly IOptions<SiteFileSettings> _siteFileSettings;

        public AvatarImage(IOptions<AttachmentSettings> attachmentSettings,IOptions<SiteFileSettings> siteFileSettings)
        {
            _attachmentSettings = attachmentSettings;
            _siteFileSettings = siteFileSettings;
        }

        public IViewComponentResult Invoke(string imageUrl)
        {
            var model =  string.IsNullOrWhiteSpace(imageUrl)
            ? _siteFileSettings.Options.PathToBlankAvatar
            : (_attachmentSettings.Options.Researcher.UrlPathPart + imageUrl);

            return View("/Views/Partials/_AvatarImage", model);
        }
    }
}
