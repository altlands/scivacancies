using System.Linq;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Razor.TagHelpers;

namespace SciVacancies.WebApp
{
    [HtmlTargetElement("a", Attributes = "filelink")]
    public class FileLinkTagHelper : TagHelper
    {
        [HtmlAttributeName("filelink")]
        public string FileLink { get; set; }

        [HtmlAttributeName("pathtocatalog")]
        public string PathToCatalog { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(FileLink))
                if (output.Attributes != null && output.Attributes.Any())
                {
                    var href = output.Attributes.FirstOrDefault(c => c.Name.ToUpper().Equals("HREF"));
                    if (href != null)
                    {
                        string find;
                        int place;
                        if (!string.IsNullOrWhiteSpace(PathToCatalog))
                        {
                            find = "~";
                            place = PathToCatalog.IndexOf(find);
                            if (place != -1)
                                PathToCatalog = PathToCatalog.Remove(place, find.Length);
                        }
                        
                        find = ".";
                        place = FileLink.LastIndexOf(find);
                        if (place > -1)
                        {
                            FileLink = FileLink
                                .Remove(place, find.Length).Insert(place, "/");
                        }
                        
                        href.Value = PathToCatalog + FileLink;
                    }
                }
        }

    }

}
