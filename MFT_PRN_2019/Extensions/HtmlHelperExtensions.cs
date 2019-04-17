//using Byu.Fhss.Sce.Authorization;
//using Byu.Fhss.Sce.Config;
//using Byu.Fhss.Sce.Navigation;
//using Byu.Fhss.Sce.PageManagement;
//using Byu.Fhss.Sce.Plugins;
//using Byu.Fhss.Sce.Util.Navigation;
//using Byu.Fhss.Sce.Util.PageManagement;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;

//namespace Byu.Fhss.Sce
//{
//    /// <summary>
//    /// Extensions for the HtmlHelper object in Razor used by SCE.
//    /// </summary>
//    public static class HtmlHelperExtensions
//    {
//        private static string _LinkTemplate;
//        private static string LinkTemplate
//        {
//            get
//            {
//                if (string.IsNullOrWhiteSpace(_LinkTemplate))
//                {
//                    _LinkTemplate = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Plugins/SCE/Templates/Navigation/LI_Template.html"));
//                }
//                return _LinkTemplate;
//            }
//        }
//        private static string NavLink(string Title, string Link)
//        {
//            return LinkTemplate.Replace("{{REPLACE_TITLE}}", Title).Replace("{{REPLACE_LINK}}", Link);
//        }

//        private static string _NewWindowLinkTemplate;
//        private static string NewWindowLinkTemplate
//        {
//            get
//            {
//                if (string.IsNullOrWhiteSpace(_NewWindowLinkTemplate))
//                {
//                    _NewWindowLinkTemplate = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Plugins/SCE/Templates/Navigation/LI_NewWindowTemplate.html"));
//                }
//                return _NewWindowLinkTemplate;
//            }
//        }
//        private static string NavNewWindowLink(string Title, string Link)
//        {
//            return NewWindowLinkTemplate.Replace("{{REPLACE_TITLE}}", Title).Replace("{{REPLACE_LINK}}", Link);
//        }

//        /// <summary>
//        /// Displays the navigation with the given name from the store in location given by fileLocation.
//        /// </summary>
//        /// <param name="html">The object the extension attaches to.</param>
//        /// <param name="navigationId">The id of the navigation to render.</param>
//        /// <returns></returns>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "html")]
//        public static MvcHtmlString DisplayNavigationMenu(this HtmlHelper html, int navigationId)
//        {
//            NavigationModel RequestedNav = NavigationInterface.Default.LoadNavigation(navigationId);

//            if (RequestedNav != null)
//            {
//                string responseBuilder = "";
//                foreach (NavEntry navItem in RequestedNav.Entries.OrderBy(n => n.DisplayOrder))
//                {
//                    switch (navItem.MenuType)
//                    {
//                        case "Link to New Window":
//                            responseBuilder += NavNewWindowLink(navItem.Title, navItem.Link);
//                            break;
//                        case "Link":
//                        default:
//                            responseBuilder += NavLink(navItem.Title, navItem.Link);
//                            break;
//                    }
//                }

//                return new MvcHtmlString(responseBuilder);
//            }
//            else
//            {
//                return new MvcHtmlString("");
//            }
//        }

//        /// <summary>
//        /// This will return only the navigation which is predefined in ~/Plugins/SCE/Templates/Navigation/DropDown/
//        /// </summary>
//        /// <param name="html">The object the extension attaches to.</param>
//        /// <returns></returns>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "html")]
//        public static MvcHtmlString DisplayUserNavigation(this HtmlHelper html)
//        {
//            string response;
//            List<AdminMenuItem> menuItems = PluginManager.AdminMenus;
//            string editorItems = string.Join("", menuItems.Where(i => i.Section == MenuSections.EditorSection).Select(i => i.ToHtml()));
//            string adminItems = string.Join("", menuItems.Where(i => i.Section == MenuSections.AdminSection).Select(i => i.ToHtml()));
//            if (GroupsManager.Default.CheckGroupMembership("Admins", SceUser.Current.NetID))
//            {
//                response = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Plugins/SCE/Templates/Navigation/DropDown/ADMIN_DropDown.html"))
//                    .Replace("{{AdminPluginItems}}", adminItems)
//                    .Replace("{{EditorPluginItems}}", editorItems);
//            }
//            else if (GroupsManager.Default.CheckGroupMembership("Editors", SceUser.Current.NetID))
//            {
//                response = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Plugins/SCE/Templates/Navigation/DropDown/EDITOR_DropDown.html"))
//                    .Replace("{{EditorPluginItems}}", editorItems);
//            }
//            else
//            {
//                response = "";
//            }

//            return new MvcHtmlString(response);
//        }

//        /// <summary>
//        /// Returns the MVC html string with the content for the given SCE page.
//        /// </summary>
//        /// <param name="html">The object the extension attaches to.</param>
//        /// <param name="page">The SCE page whose content is to be rendered.</param>
//        /// <param name="id">The index number of which of the contents pages of the SCE page is to be displayed.</param>
//        /// <param name="classes">Any classes to be included when rendering the content.</param>
//        /// <returns>Fully formatted MVC html string of the content file.</returns>
//        public static MvcHtmlString DisplaySceContent(this HtmlHelper html, ScePage page, int id, string classes)
//        {
//            return DisplaySceContent(html, page, id, classes, "full");
//        }

//        /// <summary>
//        /// Returns the MVC html string with the content for the given SCE page.
//        /// </summary>
//        /// <param name="html">The object the extension attaches to.</param>
//        /// <param name="page">The SCE page whose content is to be rendered.</param>
//        /// <param name="id">The index number of which of the contents pages of the SCE page is to be displayed.</param>
//        /// <param name="classes">Any classes to be included when rendering the content.</param>
//        /// <param name="editorType">I CAN'T REMEMBER</param>
//        /// <returns>Fully formatted MVC html string of the content file.</returns>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "html")]
//        public static MvcHtmlString DisplaySceContent(this HtmlHelper html, ScePage page, int id, string classes, string editorType)
//        {
//            if (page == null)
//            {
//                throw new ArgumentNullException(nameof(page), "Page cannot be null.");
//            }

//            string content = page.GetContentFile(id);

//            string editorPath = HttpContext.Current.Server.MapPath("~/Plugins/SCE/Templates/Content/Editor.html");
//            string editor = System.IO.File.ReadAllText(editorPath);

//            string dotWrapperPath = HttpContext.Current.Server.MapPath("~/Plugins/SCE/Templates/Content/DotWrapper.html");
//            string dotWrapper = System.IO.File.ReadAllText(dotWrapperPath);

//            string finalContent = content;

//            if (GroupsManager.Default.CheckGroupMembership(new List<string> { "Editors", "Admins" }, SceUser.Current.NetID))
//            {
//                finalContent = dotWrapper
//                    .Replace("{{REPLACE_ID}}", id.ToString(CultureInfo.CurrentCulture))
//                    .Replace("{{REPLACE_CLASS}}", classes)
//                    .Replace("{{REPLACE_CONTENT}}", content)
//                    .Replace("{{EDITOR_TYPE}}", editorType)

//                    + editor
//                        .Replace("{{REPLACE_ID}}", id.ToString(CultureInfo.CurrentCulture))
//                        .Replace("{{REPLACE_PAGE_TITLE}}", page.UriName)
//                        .Replace("{{REPLACE_URI_PATH}}", page.UriPath)
//                        .Replace("{{REPLACE_HTML}}", content);
//            }

//            MvcHtmlString htmlContent = new MvcHtmlString(finalContent);
//            return htmlContent;
//        }

//        /// <summary>
//        /// Helper method to render the CDN style resources specified in the Web.config.
//        /// </summary>
//        /// <param name="html">The object the extension attaches to.</param>
//        /// <returns>The css formatted and rendered as a MVC html string, within its appropriate link tag</returns>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "html")]
//        public static MvcHtmlString RenderCdnStyles(this HtmlHelper html)
//        {
//            StringBuilder builder = new StringBuilder();
//            foreach (var file in CssFiles.Files.Cdn.All)
//            {
//                builder.AppendFormat(CultureInfo.InvariantCulture,"<link rel='stylesheet' href='{0}' />", file.Url);
//            }
//            return new MvcHtmlString(builder.ToString());
//        }

//        /// <summary>
//        /// Helper method to render the CDN script resources specified in the Web.config.
//        /// </summary>
//        /// <param name="html">The object the extension attaches to.</param>
//        /// <returns>The js formatted and rendered as a MVC html string, within its appropriate script tag.</returns>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "html")]
//        public static MvcHtmlString RenderCdnScripts(this HtmlHelper html)
//        {
//            StringBuilder builder = new StringBuilder();
//            foreach (var file in JsFiles.Files.Cdn.All)
//            {
//                builder.AppendFormat(CultureInfo.InvariantCulture, "<script type='text/javascript' src='{0}'></script>", file.Url, CultureInfo.InvariantCulture);
//            }
//            return new MvcHtmlString(builder.ToString());
//        }
//    }
//}