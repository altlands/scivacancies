using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace SciVacancies.WebApp
{
    [TargetElement("checkboxing", Attributes = "items, values, property")]
    public class CheckboxingTagHelper : TagHelper
    {
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; }
        [HtmlAttributeName("values")]
        public IEnumerable<string> Values { get; set; }
        [HtmlAttributeName("property")]
        public string Property { get; set; }
        [HtmlAttributeName("showcount")]
        public int Showcount { get; set; }
        [HtmlAttributeName("labelforindex")]
        public int LabelForIndex { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Items == null) return;

            /*
                <li>
                    <span class="checkbox">
                        <input type="checkbox" />
                    </span><label>Владивосток (12)</label>
                </li>
            */

            var i = LabelForIndex;
            if (Values != null)
            {
                var selectedCount = Items.Count(c => Values.Contains(c.Value));
                Showcount = selectedCount > Showcount ? selectedCount : Showcount;
                foreach (var item in Items.Where(c => Values.Contains(c.Value)))
                {
                    i++;
                    AppendItem(output, i, item, true);
                }
                foreach (var item in Items.Where(c => !Values.Contains(c.Value)))
                {
                    i++;
                    AppendItem(output, i, item, false);
                }
            }
            else
            {
                foreach (var item in Items)
                {
                    i++;
                    AppendItem(output, i, item, false);
                }
            }

        }

        private void AppendItem(TagHelperOutput output, int i, SelectListItem item, bool itemIsChecked)
        {
            var input = new TagBuilder("input");
            var span = new TagBuilder("span");

            input.Attributes.Add("type", "checkbox");
            input.Attributes.Add("name", Property);
            input.Attributes.Add("id", Property + i);
            input.Attributes.Add("value", item.Value);

            if (itemIsChecked)
            {
                input.Attributes.Add("checked", string.Empty);
                span.AddCssClass("checked");
            }

            span.AddCssClass("checkbox");
            span.InnerHtml = input.ToString(TagRenderMode.SelfClosing);

            var label = new TagBuilder("label");
            label.Attributes.Add("for", input.Attributes["id"]);
            label.SetInnerText(item.Text);

            var li = new TagBuilder("li")
            {
                InnerHtml = span.ToString() + label
            };
            if (Showcount > 0 && i > Showcount)
            {
                li.Attributes.Add("style", "display: none;");
            }
            var t = new StringBuilder();

            output.Content.Append(li.ToString());
        }
    }

}
