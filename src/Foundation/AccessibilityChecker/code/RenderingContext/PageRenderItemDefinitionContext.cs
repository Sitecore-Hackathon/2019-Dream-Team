namespace DreamTeam.Foundation.AccessibilityChecker.RenderingContext
{
    using System;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Mvc.Common;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Sites;

    //<summary>
    /// A context stack for the current page defintion.  By default many of the MVC pipeline
    /// processes use a static singular page context PageContext.  This is problematic if
    /// you want to render from a completely seperate context.  This is set up as a context
    /// stack to be used on top of the already used PageContext.
    /// </summary> 
    public class PageRenderItemDefinitionContext
    {
        public static PageRenderItemDefinitionContext Current => ContextService.Get().GetCurrent<PageRenderItemDefinitionContext>();

        public static PageRenderItemDefinitionContext CurrentOrNull => ContextService.Get().GetCurrentOrDefault<PageRenderItemDefinitionContext>();

        public PageDefinition Definition { get; private set; }
        public Item Item { get; private set; }
        public DisplayMode PageMode { get; set; }

        public PageRenderItemDefinitionContext(PageDefinition pageDefinition, Item item, DisplayMode exteriorDisplayMode)
        {
            Assert.ArgumentNotNull(pageDefinition, nameof(pageDefinition));
            Assert.ArgumentNotNull(item, nameof(item));

            Definition = pageDefinition;
            Item = item;
            PageMode = exteriorDisplayMode;
        }

        public static IDisposable Enter(PageRenderItemDefinitionContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            return ContextService.Get().Push(context);
        }
    }
}