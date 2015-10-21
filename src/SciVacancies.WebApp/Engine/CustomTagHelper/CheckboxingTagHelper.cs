using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.ViewComponents;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace SciVacancies.WebApp
{
    [HtmlTargetElement("checkboxing", Attributes = "items, values, property")]
    public class CheckboxingTagHelper : TagHelper
    {
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; }
        [HtmlAttributeName("values")]
        public IEnumerable<int> Values { get; set; }
        [HtmlAttributeName("property")]
        public string Property { get; set; }
        /// <summary>
        /// количество отображаемых значений для выбора (список ComboBox'ов), где (-1) - показывать все значения, (0) - значение по-умолчанию, (n) - показать минимум 'n'-значений, или все выбранные значения.
        /// </summary>
        [HtmlAttributeName("showcount")]
        public int Showcount { get; set; }
        [HtmlAttributeName("labelforindex")]
        public int LabelForIndex { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Items == null) return;

            var i = LabelForIndex;
            if (Values != null)
            {
                var valuesString = Values.Select(c => c.ToString());

                var selectedCount = Items.Count(c => valuesString.Contains(c.Value));

                if (Showcount != -1) //если не указано "показывать все значения"
                    Showcount = selectedCount > Showcount ? selectedCount : Showcount; //показать минимум Showcount значений, или все выбранные значения

                foreach (var item in Items.Where(c => valuesString.Contains(c.Value)))
                {
                    i++;
                    AppendItem(output, i, item, true);
                }
                foreach (var item in Items.Where(c => !valuesString.Contains(c.Value)))
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
            span.InnerHtml.Append(input);

            var label = new TagBuilder("label");
            label.Attributes.Add("for", input.Attributes["id"]);
            label.InnerHtml.AppendEncoded(item.Text);

            var li = new TagBuilder("li");

            li.InnerHtml.Append(span).Append(label);
            li.AddCssClass("li-checkbox");

            if (Showcount > 0 && i > Showcount)
            {
                li.Attributes.Add("style", "display: none;");
            }
            var t = new StringBuilder();

            output.Content.Append(li);
        }
    }

}
