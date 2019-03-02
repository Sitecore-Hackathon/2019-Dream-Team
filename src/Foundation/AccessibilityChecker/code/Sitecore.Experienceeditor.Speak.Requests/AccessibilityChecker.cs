namespace DreamTeam.Foundation.AccessibilityChecker.Sitecore.Experienceeditor.Speak.Requests
{
    using global::Sitecore.ExperienceEditor.Speak.Server.Contexts;
    using global::Sitecore.ExperienceEditor.Speak.Server.Requests;
    using global::Sitecore.ExperienceEditor.Speak.Server.Responses;
    using global::Sitecore.Text;

    public class AccessibilityChecker : PipelineProcessorRequest<PageContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            var item = this.RequestContext.Item;

            var url = string.Format(
                //"/sitecore/client/AccessibilityChecker/AccessibilityChecker?sc_lang={0}&itemId={1}",
                "/axe",
                this.RequestContext.Language,
                item.ID);

            var pipelineProcessorResponseValue = new PipelineProcessorResponseValue()
            {
                Value = new UrlString(url).ToString()
            };

            return pipelineProcessorResponseValue;
        }
    }
}