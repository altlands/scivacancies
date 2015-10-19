using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class VacancyInfo: ViewComponent
    {
        private readonly IOptions<AttachmentSettings> _attachmentSettings;
        public VacancyInfo(IOptions<AttachmentSettings> attachmentSettings)
        {
            _attachmentSettings = attachmentSettings;

        }
        public IViewComponentResult Invoke(VacancyDetailsViewModel model)
        {
            model.FolderVacanciesAttachmentsUrl = _attachmentSettings.Value.Vacancy.UrlPathPart;
            return View("/Views/Partials/_VacancyInfo", model);
        }
    }
}
