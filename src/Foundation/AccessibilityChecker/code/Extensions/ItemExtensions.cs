namespace DreamTeam.Foundation.AccessibilityChecker.Extensions
{
    using DreamTeam.Foundation.AccessibilityChecker.Renderer;
    using Sitecore.Data.Items;

    public static class ItemExtensions
    {
        /// <summary>
        /// Renders an item with a layout definition to a string
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Rendered output for the item</returns>
        public static string RenderToString(this Item item)
        {
            return new ItemRenderer(item).Render();
        }
    }
}