using Microsoft.AspNet.Http;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class ResearcherEditPhotoViewModel: PageViewModelBase
    {
        public IFormFile PhotoFile { get; set; }

        public string ImageName { get; set; }
        public long? ImageSize { get; set; }
        public string ImageExtension { get; set; }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl ?? string.Empty; }
            set { _imageUrl = value; }
        }
    }
}