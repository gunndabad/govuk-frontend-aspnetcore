using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    public abstract class LinkTagHelperBase : TagHelper
    {
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string PageAttributeName = "asp-page";
        private const string PageHandlerAttributeName = "asp-page-handler";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private const string HrefAttributeName = "href";

        private IDictionary<string, string> _routeValues;

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public LinkTagHelperBase(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        [HtmlAttributeName(HostAttributeName)]
        public string Host { get; set; }

        [HtmlAttributeName(HrefAttributeName)]
        public string Href { get; set; }

        [HtmlAttributeName(PageAttributeName)]
        public string Page { get; set; }

        [HtmlAttributeName(PageHandlerAttributeName)]
        public string PageHandler { get; set; }

        [HtmlAttributeName(ProtocolAttributeName)]
        public string Protocol { get; set; }

        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected TagBuilder CreateAnchorTagBuilder()
        {
            var hrefLink = Href != null;
            var routeLink = Route != null;
            var actionLink = Controller != null || Action != null;
            var pageLink = Page != null || PageHandler != null;

            if (new[] { hrefLink, routeLink, actionLink, pageLink }.Count(l => l) > 1)
            {
                throw new InvalidOperationException(
                    $"Cannot determine the 'href' attribute for <a>. The following attributes are mutually exclusive:\n" +
                    $"{HrefAttributeName}\n" +
                    $"{RouteAttributeName}\n" +
                    $"{ControllerAttributeName}, {ActionAttributeName}\n" +
                    $"{PageAttributeName}, {PageHandlerAttributeName}");
            }

            RouteValueDictionary routeValues = null;
            if (_routeValues != null && _routeValues.Count > 0)
            {
                routeValues = new RouteValueDictionary(_routeValues);
            }

            if (Area != null)
            {
                // Unconditionally replace any value from asp-route-area.
                if (routeValues == null)
                {
                    routeValues = new RouteValueDictionary();
                }
                routeValues["area"] = Area;
            }

            string href = null;
            if (hrefLink)
            {
                href = Href;
            }
            else if (pageLink)
            {
                href = _htmlGenerator.GetPageLinkHref(ViewContext, Page, PageHandler, RouteValues, Protocol, Host, Fragment);
            }
            else if (routeLink)
            {
                href = _htmlGenerator.GetRouteLinkHref(ViewContext, Route, RouteValues, Protocol, Host, Fragment);
            }
            else // if (actionLink)
            {
                href = _htmlGenerator.GetActionLinkHref(ViewContext, Action, Controller, RouteValues, Protocol, Host, Fragment);
            }

            return _htmlGenerator.GenerateAnchor(href);
        }
    }
}
