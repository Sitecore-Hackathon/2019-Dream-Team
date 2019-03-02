namespace DreamTeam.Foundation.AccessibilityChecker.Renderer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using DreamTeam.Foundation.AccessibilityChecker.RenderingContext;
    using Sitecore;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.Mvc.Configuration;
    using Sitecore.Mvc.Extensions;
    using Sitecore.Mvc.Pipelines;
    using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
    using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Sites;

    /// <summary>
    /// Renders an item's layout to a string or TextWriter.
    /// </summary>
    public class ItemRenderer
    {
        public Item Item { get; set; }

        public ItemRenderer(Item item)
        {
            this.Item = item;
        }

        /// <summary>
        /// Renders an item with a layout defined to a string for MVC
        /// </summary>
        /// <returns>HTML of item</returns>
        public virtual string Render()
        {
            using (TextWriter tw = new StringWriter())
            {
                Render(tw);
                return tw.ToString();
            }
        }

        /// <summary>
        /// Renders an item with a layout defined to a string for MVC
        /// </summary>
        /// <returns>HTML of item</returns>
        public virtual void Render(TextWriter writer)
        {
            var originalDisplayMode = Context.Site.DisplayMode;

            // keep a copy of the renderings we start with.
            // running the renderPlaceholder pipeline (which runs renderRendering) will overwrite these
            // and we need to set them back how they were when we're done rendering the xBlock
            var originalRenderingDefinitionContext = RenderingContext.CurrentOrNull?.PageContext?.PageDefinition;

            try
            {
                // prevents editing the snippet in context, so you cannot mistakenly change something shared by mistake
                if (Context.PageMode.IsExperienceEditorEditing)
                    Context.Site.SetDisplayMode(DisplayMode.Preview, DisplayModeDuration.Temporary);

                var pageDef = new PageDefinition
                {
                    Renderings = new List<Rendering>()
                };

                //Extracts the item's layout XML, then parses all of the renderings out of it.
                pageDef.Renderings.AddRange(GetRenderings(GetLayoutFromItem()));

                // Uncovers the main layout rendering
                var pageRenderingArgs = new GetPageRenderingArgs(pageDef);
                PipelineService.Get().RunPipeline("mvc.getPageRendering", pageRenderingArgs);

                //Renders all placeholders for the layout rendering, which would be the entire page
                var renderPlaceholderArgs = new RenderPlaceholderArgs(PerformItemRendering.ItemRenderingKey, writer, pageRenderingArgs.Result)
                {
                    PageContext = new PageContext
                    {
                        PageDefinition = pageDef
                    }
                };

                using (PageRenderItemDefinitionContext.Enter(new PageRenderItemDefinitionContext(pageDef, Item, originalDisplayMode)))
                {
                    PipelineService.Get().RunPipeline("mvc.renderPlaceholder", renderPlaceholderArgs);
                }
            }
            catch (Exception e)
            {
                Log.Error("There was a problem rendering an item to string", e, this);
                if (originalDisplayMode == DisplayMode.Edit || originalDisplayMode == DisplayMode.Preview)
                {
                    writer.Write($"<p class=\"edit-only\">Error occurred while rendering {this.Item.Paths.FullPath}: {e.Message}<br>For error details, <a href=\"{LinkManager.GetItemUrl(this.Item)}\" onclick=\"window.open(this.href); return false;\">visit the target page</a></p>");
                }
            }
            finally
            {
                // replace the renderings in the current context with the ones that existed before we ran our sideline renderPlaceholder
                // because they have been overwritten with the xBlock's renderings at this point
                if (originalRenderingDefinitionContext != null)
                {
                    RenderingContext.CurrentOrNull.PageContext.PageDefinition = originalRenderingDefinitionContext;
                }

                Context.Site.SetDisplayMode(originalDisplayMode, DisplayModeDuration.Temporary);
            }
        }

        /// <summary>
        /// Gets the layout XML from the item
        /// </summary>
        /// <returns>xml of the layout definition</returns>
        protected virtual XElement GetLayoutFromItem()
        {
            Field innerField = new LayoutField(this.Item).InnerField;

            if (innerField == null)
            {
                return null;
            }

            string fieldValue = LayoutField.GetFieldValue(innerField);

            if (fieldValue.IsWhiteSpaceOrNull())
            {
                return null;
            }

            return XDocument.Parse(fieldValue).Root;
        }

        /// <summary>
        /// Gets the rendering out of the xml node and injects some values in
        /// </summary>
        /// <param name="renderingNode"></param>
        /// <param name="deviceId"></param>
        /// <param name="layoutId"></param>
        /// <param name="renderingType"></param>
        /// <param name="parser"></param>
        /// <returns>MVC rendering</returns>
        protected virtual Rendering GetRendering(XElement renderingNode, Guid deviceId, Guid layoutId, string renderingType, XmlBasedRenderingParser parser)
        {
            Rendering rendering = parser.Parse(renderingNode, false);
            rendering.DeviceId = deviceId;
            rendering.LayoutId = layoutId;
            if (renderingType != null)
            {
                rendering.RenderingType = renderingType;
            }

            // if the xBlock is rendering in the context of another page, renderings with no data source should be repointed to the xBlock page item
            // as opposed to the context page item
            if (string.IsNullOrWhiteSpace(rendering.DataSource))
            {
                rendering.DataSource = this.Item.ID.ToString();
            }

            return rendering;
        }

        /// <summary>
        /// Get all renderings out of the layout definition
        /// </summary>
        /// <param name="layoutDefinition">xml of the layout definition</param>
        /// <returns>list of renderings</returns>
        protected virtual IEnumerable<Rendering> GetRenderings(XElement layoutDefinition)
        {
            XmlBasedRenderingParser parser = MvcSettings.GetRegisteredObject<XmlBasedRenderingParser>();
            foreach (XElement xelement in layoutDefinition.Elements("d"))
            {
                Guid deviceId = xelement.GetAttributeValueOrEmpty("id").ToGuid();
                Guid layoutId = xelement.GetAttributeValueOrEmpty("l").ToGuid();

                yield return this.GetRendering(xelement, deviceId, layoutId, "Layout", parser);

                foreach (XElement renderingNode in xelement.Elements("r"))
                    yield return GetRendering(renderingNode, deviceId, layoutId, renderingNode.Name.LocalName, parser);
            }
        }
    }
}