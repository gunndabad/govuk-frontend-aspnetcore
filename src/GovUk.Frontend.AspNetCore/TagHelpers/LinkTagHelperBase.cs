using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    public abstract class LinkTagHelperBase : TagHelper
    {
        protected const string ActionAttributeName = "asp-action";
        protected const string ControllerAttributeName = "asp-controller";
        protected const string AreaAttributeName = "asp-area";
        protected const string PageAttributeName = "asp-page";
        protected const string PageHandlerAttributeName = "asp-page-handler";
        protected const string FragmentAttributeName = "asp-fragment";
        protected const string HostAttributeName = "asp-host";
        protected const string ProtocolAttributeName = "asp-protocol";
        protected const string RouteAttributeName = "asp-route";
        protected const string RouteValuesDictionaryName = "asp-all-route-data";
        protected const string RouteValuesPrefix = "asp-route-";
        protected const string HrefAttributeName = "href";

        private IDictionary<string, string> _routeValues;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public LinkTagHelperBase(
            IGovUkHtmlGenerator htmlGenerator,
            IUrlHelperFactory urlHelperFactory)
        {
            Generator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
            _urlHelperFactory = urlHelperFactory;
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

        protected IGovUkHtmlGenerator Generator { get; }

        protected bool HasLinkAttributes =>
            Action != null ||
            Area != null ||
            Controller != null ||
            Fragment != null ||
            Host != null ||
            Href != null ||
            Page != null ||
            PageHandler != null ||
            Protocol != null ||
            Route != null ||
            RouteValues.Count > 0;

        protected string ResolveHref()
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

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            string href = null;
            if (hrefLink)
            {
                href = Href;
            }
            else if (pageLink)
            {
                href = urlHelper.Page(Page, PageHandler, RouteValues, Protocol, Host, Fragment);
            }
            else if (routeLink)
            {
                href = urlHelper.RouteUrl(Route, RouteValues, Protocol, Host, Fragment);
            }
            else // if (actionLink)
            {
                href = urlHelper.Action(Action, Controller, RouteValues, Protocol, Host, Fragment);
            }

            if (href == null)
            {
                throw new InvalidOperationException("Cannot determine the 'href' attribute for <a>.");
            }

            return href;
        }
    }
}
