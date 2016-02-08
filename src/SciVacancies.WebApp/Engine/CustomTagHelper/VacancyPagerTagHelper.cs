using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Razor.TagHelpers;
using SciVacancies.ReadModel.Pager;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp
{
    [HtmlTargetElement("div", Attributes = "pagedlist")]
    public class VacancypagerTagHelper : TagHelper
    {
        private IDictionary<string, string[]> _queryDictionary;
        private const string ParameterNamePageNumber = "CurrentPage";
        private const string ParameterNamePageSize = "PageSize";

        [HtmlAttributeName("request")]
        public HttpRequest Request { get; set; }
        [HtmlAttributeName("pagedlist")]
        public PagedList PagedList { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _queryDictionary = Request.ToQueryStringDictionary();//Microsoft.AspNet.WebUtilities.QueryHelpers.ParseQuery(Request.QueryString.Value);

            if (PagedList == null)
                return;

            //проверка минимальных допустимых значений
            PagedList.CurrentPage = PagedList.CurrentPage < 0 ? 1 : PagedList.CurrentPage;
            PagedList.PageSize = PagedList.PageSize < 0 ? 10 : PagedList.PageSize;

            if (!_queryDictionary.ContainsKey(ParameterNamePageNumber))
                _queryDictionary.Add(ParameterNamePageNumber, new[] { "1" });
            if (!_queryDictionary.ContainsKey(ParameterNamePageSize))
                _queryDictionary.Add(ParameterNamePageSize, new[] { "10" });

            var divParent = new TagBuilder("div");
            divParent.AddCssClass("bottom-nav");
            {
                var divLeft = new TagBuilder("div");
                divLeft.AddCssClass("bottom-nav-left");
                {
                    {
                        var div = new TagBuilder("div");
                        div.AddCssClass("count-news");
                        var sb = new StringBuilder();
                        sb.Append(PagedList.FirstRowIndexOnPage);
                        sb.Append("-");
                        sb.Append(PagedList.LastRowIndexOnPage);
                        sb.Append(" из ");
                        sb.Append(PagedList.TotalItems);

                        div.InnerHtml.Append(sb.ToString());
                        divLeft.InnerHtml.Append(div);
                    }

                    {
                        var div = new TagBuilder("div");
                        div.AddCssClass("count-line-page");

                        var form = new TagBuilder("form");
                        form.Attributes.Add("method", "GET");
                        SetPageNumberInQuery(1);
                        form.Attributes.Add("action", GetNewUri());
                        form.Attributes.Add("style", "display:inline;");

                        {

                            var input = new TagBuilder("input");
                            input.Attributes.Add("type", "text");
                            input.Attributes.Add("name", ParameterNamePageSize);
                            input.Attributes.Add("id", ParameterNamePageSize);
                            input.Attributes.Add("value", PagedList.PageSize.ToString());
                            input.Attributes.Add("changeaction", "true");

                            var inputHidden = new TagBuilder("input");
                            inputHidden.Attributes.Add("type", "hidden");
                            inputHidden.Attributes.Add("name", ParameterNamePageNumber);
                            inputHidden.Attributes.Add("id", ParameterNamePageNumber);
                            inputHidden.Attributes.Add("value", PagedList.CurrentPage.ToString());

                            form.InnerHtml.Append(input).Append(inputHidden) /*+ submit.ToString(TagRenderMode.SelfClosing)*/;
                        }

                        div.InnerHtml.Append(form).Append(" записей на странице");
                        divLeft.InnerHtml.Append(div);
                    }

                }

                var divRight = new TagBuilder("div");
                divRight.AddCssClass("bottom-nav-right");
                {
                    var ul = new TagBuilder("ul");

                    {
                        var li1 = new TagBuilder("li");
                        {
                            if (PagedList.CurrentPage <= 1)
                            {
                                var span = new TagBuilder("span");
                                span.AddCssClass("prev");
                                span.AddCssClass("disable");

                                span.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Первая"));
                                li1.InnerHtml.Append(span);
                            }
                            else
                            {
                                var a = new TagBuilder("a");
                                a.AddCssClass("prev");
                                SetPageNumberInQuery(1);
                                a.Attributes.Add("href", GetNewUri());
                                a.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Первая"));
                                li1.InnerHtml.Append(a);
                            }
                        }

                        var li2 = new TagBuilder("li");
                        {
                            if (PagedList.CurrentPage <= 1)
                            {
                                var span = new TagBuilder("span");
                                span.AddCssClass("first");
                                span.AddCssClass("disable");
                                span.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Предыдущая"));
                                li2.InnerHtml.Append(span);
                            }
                            else
                            {
                                var a = new TagBuilder("a");
                                a.AddCssClass("first");
                                SetPageNumberInQuery(PagedList.CurrentPage <= PagedList.TotalPages ? PagedList.CurrentPage - 1 : PagedList.TotalPages);
                                a.Attributes.Add("href", GetNewUri());
                                a.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Предыдущая"));
                                li2.InnerHtml.Append(a);
                            }
                        }

                        var li3 = new TagBuilder("li");
                        {
                            if (1 == PagedList.TotalPages)
                            {
                                var span = new TagBuilder("span");
                                span.AddCssClass("count-page");
                                span.AddCssClass("disable");
                                span.InnerHtml.Append(PagedList.CurrentPage + " из " + PagedList.TotalPages);
                                li3.InnerHtml.Append(span);
                            }
                            else
                            {
                                var divMiddlePage = new TagBuilder("div");
                                divMiddlePage.AddCssClass("count-page");

                                var form = new TagBuilder("form");
                                form.Attributes.Add("method", "GET");
                                SetPageNumberInQuery(PagedList.CurrentPage);
                                form.Attributes.Add("action", GetNewUri());
                                form.Attributes.Add("style", "display:inline;");

                                {

                                    var input = new TagBuilder("input");
                                    input.Attributes.Add("type", "text");
                                    input.Attributes.Add("name", ParameterNamePageNumber);
                                    input.Attributes.Add("id", ParameterNamePageNumber);
                                    if (PagedList.TotalPages == 0)
                                    {
                                        input.Attributes.Add("value", "0");
                                    }
                                    else
                                    {
                                        input.Attributes.Add("value", PagedList.CurrentPage.ToString());
                                    }

                                    input.Attributes.Add("changeaction", "true");

                                    var inputHidden = new TagBuilder("input");
                                    inputHidden.Attributes.Add("type", "hidden");
                                    inputHidden.Attributes.Add("name", ParameterNamePageSize);
                                    inputHidden.Attributes.Add("id", ParameterNamePageSize);
                                    inputHidden.Attributes.Add("value", PagedList.PageSize.ToString());

                                    form.InnerHtml.Append(input).Append(inputHidden) /*+ submit.ToString(TagRenderMode.SelfClosing)*/;
                                }

                                divMiddlePage.InnerHtml.Append(form).Append(" из " + PagedList.TotalPages);
                                li3.InnerHtml.Append(divMiddlePage);
                            }
                        }

                        var li4 = new TagBuilder("li");
                        {
                            if (PagedList.CurrentPage >= PagedList.TotalPages)
                            {
                                var span = new TagBuilder("span");
                                span.AddCssClass("next");
                                span.AddCssClass("disable");
                                span.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Следующая"));
                                li4.InnerHtml.Append(span);
                            }
                            else
                            {
                                var a = new TagBuilder("a");
                                a.AddCssClass("next");
                                SetPageNumberInQuery(PagedList.CurrentPage < 1 ? 1 : PagedList.CurrentPage + 1);
                                a.Attributes.Add("href", GetNewUri());
                                a.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Следующая"));
                                li4.InnerHtml.Append(a);
                            }
                        }

                        var li5 = new TagBuilder("li");
                        {
                            if (PagedList.CurrentPage >= PagedList.TotalPages)
                            {
                                var span = new TagBuilder("span");
                                span.AddCssClass("latest");
                                span.AddCssClass("disable");
                                span.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Последняя"));
                                li5.InnerHtml.Append(span);
                            }
                            else
                            {
                                var a = new TagBuilder("a");
                                a.AddCssClass("latest");
                                SetPageNumberInQuery(PagedList.TotalPages);
                                a.Attributes.Add("href", GetNewUri());
                                a.InnerHtml.Append(new TagBuilder("span").InnerHtml.Append("Последняя"));
                                li5.InnerHtml.Append(a);
                            }
                        }

                        ul.InnerHtml.Append(li1).Append(li2).Append(li3).Append(li4).Append(li5);
                    }

                    divRight.InnerHtml.Append(ul);
                }


                divParent.InnerHtml.Append(divLeft).Append(divRight);
            }


            output.Content.Append(divParent);
        }

        private string GetNewUri()
        {
            return $"{Request.Path.Value}?{GetNewQueryString()}";
        }

        private string GetNewQueryString()
        {
            return _queryDictionary.Aggregate(string.Empty, (current1, item) => current1 + item.Value.Aggregate(string.Empty, (current, value) => current + $"&{item.Key}={value}")).Trim('&').Trim();
        }

        private void RemovePageNumberInQuery()
        {
            if (_queryDictionary.ContainsKey(ParameterNamePageNumber))
                _queryDictionary.Remove(ParameterNamePageNumber);
        }

        /// <summary>
        /// Задать новое значение Номер Страницы
        /// </summary>
        /// <param name="pageNumber"></param>
        private void SetPageNumberInQuery(int pageNumber)
        {
            RemovePageNumberInQuery();
            _queryDictionary.Add(ParameterNamePageNumber, new[] { pageNumber.ToString() });
        }
    }

}
