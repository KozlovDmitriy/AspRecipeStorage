using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace AspRecipeStorage.Models
{
    public static class AuthorizeAuthorizeActionLinkExtention
    {
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues)
        {
            return helper.AuthorizeActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return helper.AuthorizeActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return helper.AuthorizeActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (HasActionPermission(helper, actionName, controllerName))
            {
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            else
            {
                var routeValuesRvd = new RouteValueDictionary(routeValues);
                var htmlAttributesRvd = new RouteValueDictionary(htmlAttributes);
                var @class = htmlAttributesRvd.ContainsKey("class") ? htmlAttributesRvd["class"] + " disabled" : null;
                if (@class != null)
                {
                    htmlAttributesRvd["class"] = @class;
                }
                return helper.ActionLink(linkText, actionName, controllerName, routeValuesRvd, htmlAttributesRvd);
            }
            
        }

        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, object routeValues)
        {
            return helper.AuthorizeActionLink(linkText, actionName, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return helper.AuthorizeActionLink(linkText, actionName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }
        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName)
        {
            return helper.AuthorizeActionLink(linkText, actionName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString AuthorizeActionLink(this HtmlHelper helper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string controllerName = (string)helper.ViewContext.RouteData.GetRequiredString("controller");
            return helper.AuthorizeActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);

        }

        static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            ControllerBase controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                ? htmlHelper.ViewContext.Controller
                : GetControllerByName(htmlHelper, controllerName);

            ControllerContext controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }

        static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
                return false;

            AuthorizationContext authContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (Filter authFilter in FilterProviders.Providers.GetFilters(authContext, actionDescriptor))
            {
                if (authFilter.Instance is System.Web.Mvc.AuthorizeAttribute)
                {


                    ((IAuthorizationFilter)authFilter.Instance).OnAuthorization(authContext);

                    if (authContext.Result != null)
                        return false;
                }
            }

            return true;
        }

        static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            IController controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));
            }

            return (ControllerBase)controller;
        }

    }
}