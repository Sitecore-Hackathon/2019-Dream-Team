namespace DreamTeam.Foundation.AccessibilityChecker.RenderingContext
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Sitecore.Mvc.Extensions;
    using Sitecore.Mvc.Pipelines;
    using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
    using Sitecore.Mvc.Pipelines.Response.RenderRendering;
    using Sitecore.Mvc.Presentation;

    public class PerformItemRendering : PerformRendering
    {
        public static readonly string ItemRenderingKey = Guid.NewGuid().ToString();

        /// <summary>
        /// Render step, except it temporarily abandons the placeholder context to render a seperate item, after which it puts the context back
        /// </summary>
        /// <param name="placeholderName">Placeholder to render</param>
        /// <param name="writer">writer to render to</param>
        /// <param name="args"></param>
        protected override void Render(string placeholderName, TextWriter writer, RenderPlaceholderArgs args)
        {
            if (PageRenderItemDefinitionContext.CurrentOrNull != null)
                args.PageContext.PageDefinition = PageRenderItemDefinitionContext.Current.Definition;

            if (placeholderName != ItemRenderingKey)
            {
                base.Render(placeholderName, writer, args);
                return;
            }

            Stack<PlaceholderContext> previousContext = new Stack<PlaceholderContext>();
            while (PlaceholderContext.CurrentOrNull != null)
            {
                previousContext.Push(PlaceholderContext.Current);
                PlaceholderContext.Exit();
            }

            try
            {
                PipelineService.Get().RunPipeline("mvc.renderRendering", new RenderRenderingArgs(args.PageContext.PageDefinition.Renderings.First(x => x.Placeholder.IsWhiteSpaceOrNull()), writer));
            }
            finally
            {
                while (PlaceholderContext.CurrentOrNull != null)
                {
                    PlaceholderContext.Exit();
                }

                while (previousContext.Any())
                {
                    PlaceholderContext.Enter(previousContext.Pop());
                }
            }
        }
    }
}