namespace DreamTeam.Foundation.AccessibilityChecker.WebEdit.Commands
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Layouts;
    using Sitecore.Shell.Applications.Dialogs.Testing;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Web;
    using Sitecore.Web.UI.Sheer;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Sitecore.Shell.Applications.WebEdit.Commands;
    using Sitecore;
    using Sitecore.Web.UI.WebControls;
    using Sitecore.Text;
    using Sitecore.StringExtensions;
    using DreamTeam.Foundation.AccessibilityChecker.Extensions;

    [Serializable]
    public class ComponentAccessibilityCheckerCommand : WebEditCommand
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            Item item = context.Items[0];
            using (new DatabaseSwitcher(Context.ContentDatabase))
            {
                var htmlOutputString = item.RenderToString();
            }
            NameValueCollection expr_40 = new NameValueCollection();
            expr_40["id"] = item.ID.ToString();
            expr_40["language"] = ((Context.Language == null) ? item.Language.ToString() : Context.Language.ToString());
            expr_40["version"] = item.Version.ToString();
            expr_40["database"] = item.Database.Name;
            NameValueCollection parameters = expr_40;

            Context.ClientPage.Start(this, "Run", parameters);
        }

        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (Sitecore.Context.PageMode.IsPreview)
            {
                return CommandState.Disabled;
            }

            return CommandState.Enabled;
        }

        protected void Run(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (!SheerResponse.CheckModified())
            {
                return;
            }

            UrlString urlString2 = new UrlString("/sitecore/shell/Applications/Item browser.aspx");
            urlString2.Append("ro", "/sitecore");
            urlString2.Append("sc_content", Context.ContentDatabase.Name);
            urlString2.Append("filter", "AddTagDialog");

            SheerResponse.ShowModalDialog(urlString2.ToString(), "1000", "600", string.Empty, true);
            AjaxScriptManager.Current.Dispatch("item:refresh(id={0})".FormatWith(new object[]
                        {
                            args.Parameters["id"]
                        }));
                        return;

            //if (args.IsPostBack)
            //{
            //    if (Context.Page != null && Context.Page.Page != null && Context.Page.Page.Session["TrackingFieldModified"] as string == "1")
            //    {
            //        Context.Page.Page.Session["TrackingFieldModified"] = null;
            //        if (AjaxScriptManager.Current != null)
            //        {
            //            AjaxScriptManager.Current.Dispatch("analytics:trackingchanged");
            //            AjaxScriptManager.Current.Dispatch("item:refresh(id={0})".FormatWith(new object[]
            //            {
            //                args.Parameters["id"]
            //            }));
            //            return;
            //        }
            //        Context.ClientPage.SendMessage(this, "analytics:trackingchanged");
            //        Context.ClientPage.SendMessage(this, "item:refresh(id={0})".FormatWith(new object[]
            //        {
            //            args.Parameters["id"]
            //        }));
            //        return;
            //    }
            //}
            //else
            //{
            //    UrlString urlString = new UrlString("/sitecore/shell/~/xaml/Sitecore.Shell.Applications.Analytics.Personalization.ProfileCardsForm.aspx");
            //    UrlHandle expr_113 = new UrlHandle();
            //    expr_113["itemid"] = args.Parameters["id"];
            //    expr_113["databasename"] = args.Parameters["database"];
            //    expr_113["la"] = args.Parameters["language"];
            //    UrlHandle urlHandle = expr_113;
            //    urlHandle.Add(urlString);
            //    SheerResponse.ShowModalDialog(urlString.ToString(), "1000", "600", string.Empty, true);
            //    args.WaitForPostBack();
            //}
        }
    }
}