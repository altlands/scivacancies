using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class VacancyApplicationInfo: ViewComponent
    {
        private readonly IOptions<AttachmentSettings> _attachmentSettings;
        public VacancyApplicationInfo(IOptions<AttachmentSettings> attachmentSettings)
        {
            _attachmentSettings = attachmentSettings;

        }
        public IViewComponentResult Invoke(VacancyApplicationDetailsViewModel model)
        {
            model.FolderApplicationsAttachmentsUrl= _attachmentSettings.Options.VacancyApplication.UrlPathPart;
            return View("/Views/Partials/_VacancyApplicationInfo", model);
        }
    }
}
