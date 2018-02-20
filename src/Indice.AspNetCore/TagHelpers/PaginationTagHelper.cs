﻿using Indice.AspNetCore.Types;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indice.AspNetCore.TagHelpers
{
    /// <summary>
    /// Pagination helper.
    /// </summary>
    [HtmlTargetElement("ul", Attributes = "[class^=" + PaginationClassName + "], [list-options]")]
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        const string PaginationClassName = "pagination";
        const int MINIMUM_WINDOW = 5;
        const int MINUMUM_PAGES = MINIMUM_WINDOW + 3;

        /// <summary>
        /// The virew context.
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        /// <summary>
        /// the html generator
        /// </summary>
        protected readonly IHtmlGenerator Generator;

        /// <summary>
        /// All the current <see cref="ListOptions"/>.
        /// </summary>
        [HtmlAttributeName("list-options")]
        public ListOptions Options { get; set; }

        /// <summary>
        /// The total <see cref="Count"/> of records.
        /// </summary>
        [HtmlAttributeName("count")]
        public int Count { get; set; }

        /// <summary>
        /// The number of always visible pages.
        /// </summary>
        [HtmlAttributeName("window")]
        public int Window { get; set; } = MINIMUM_WINDOW;

        /// <summary>
        /// The total number of <see cref="Pages"/>
        /// </summary>
        protected int Pages => Options.GetPagesFor(Count);

        public PaginationTagHelper(IHtmlGenerator generator) {
            Generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            if (Options == null) {
                throw new InvalidOperationException($"The list options must be provided.");
            }
            output.TagName = "ul";

            var childContent = await output.GetChildContentAsync();
            if (childContent.IsEmptyOrWhiteSpace) {

                output.Content.AppendHtml(PageLink(Options.Page != 1, "«", Options.Page - 1, "previous"));
                output.Content.AppendHtml(PageLink(Options.Page != 1, $"{1}", 1, "first"));

                var window = AdjustWindow(Window, Pages);
                var windowFactor = ((window - 2) / 2);

                IEnumerable<int> pages;

                var nearStart = Options.Page - windowFactor <= 3;
                var nearEnd = Pages - Options.Page - windowFactor < 3;

                if (Pages < MINUMUM_PAGES) {
                    pages = Enumerable.Range(2, Pages - 2);

                    foreach (var page in pages) {
                        output.Content.AppendHtml(PageLink(Options.Page != page, $"{page}", page, $"{page}"));
                    }
                } else {
                    if (nearStart) {
                        pages = Enumerable.Range(2, window - 1);

                        foreach (var page in pages) {
                            output.Content.AppendHtml(PageLink(Options.Page != page, $"{page}", page, $"{page}"));
                        }

                        if (pages.Any()) {
                            output.Content.AppendHtml(PageLink(false, $"...", Options.Page, $"more pages"));
                        }
                    }

                    if (nearEnd) {
                        pages = Enumerable.Range(Pages - window + 1, window - 1);

                        if (pages.Any()) {
                            output.Content.AppendHtml(PageLink(false, $"...", Options.Page, $"more pages"));
                        }

                        foreach (var page in pages) {
                            output.Content.AppendHtml(PageLink(Options.Page != page, $"{page}", page, $"{page}"));
                        }
                    }

                    if (!nearStart && !nearEnd) {
                        pages = Enumerable.Range(Options.Page - windowFactor, window - 2);
                        output.Content.AppendHtml(PageLink(false, $"...", Options.Page, $"more pages"));

                        foreach (var page in pages) {
                            output.Content.AppendHtml(PageLink(Options.Page != page, $"{page}", page, $"{page}"));
                        }

                        output.Content.AppendHtml(PageLink(false, $"...", Options.Page, $"more pages"));
                    }
                }

                output.Content.AppendHtml(PageLink(Options.Page != Pages, $"{Pages}", Pages, "last"));
                output.Content.AppendHtml(PageLink(Options.Page != Pages, "»", Options.Page + 1, "next"));

                // append summary if needed.
            } 
        }

        private IHtmlContent PageLink(bool enabled, string content, int page, string cssClass, string title = null) {
            var active = Options.Page == page;
            //if (!enabled)
            //    return HtmlString.Empty;
            var routeValues = Options.ToDictionary(new { Page = page }).AsRouteValues();
            var container = new TagBuilder("li");
            container.AddCssClass("page-item");
            if (null != cssClass)
                container.AddCssClass(cssClass);
            if (null != title)
                container.MergeAttribute("title", title);
            if (!enabled) { 
                container.AddCssClass("disabled");
            }
            var anchorTag = Generator.GenerateActionLink(ViewContext,
                                                         string.Empty, null, null, null, null, null,
                                                         routeValues.ToRouteValueDictionary(),
                                                         null);
            anchorTag.AddCssClass("page-link");
            if (active) {
                container.AddCssClass("active");
                anchorTag.MergeAttribute("href", "", true);
            } 
            var spanTag = new TagBuilder("span");
            spanTag.InnerHtml.SetContent(content);
            anchorTag.InnerHtml.AppendHtml(spanTag);
            container.InnerHtml.AppendHtml(anchorTag);
            return container;
        }

        private static int AdjustWindow(int window, int totalPages) {
            if (window < MINIMUM_WINDOW || window > totalPages - 3) {
                return MINIMUM_WINDOW;
            }
            if (window % 2 == 0) {
                window = window - 1;
            }
            return window;
        }
    }
}