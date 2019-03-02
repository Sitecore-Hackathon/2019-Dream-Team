define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.AccessibilityChecker = {
        canExecute: function (context) {
            return true;
        },

        execute: function (context) {
            ExperienceEditor.PipelinesUtil.generateRequestProcessor("DreamTeam.CheckAccessibility", function (response) {
                var dialogFeatures = "dialogMinWidth: 800px;";
                ExperienceEditor.Dialogs.showModalDialog(response.responseValue.value, "", dialogFeatures, null);
                axe.run(document, {
                    exclude: ['iframe','.ui-dialog-normal']
                }, function (data) {
                    WCAG2.generateTable(data, document.getElementById('accessibility-result'));
                });
            }).execute(ExperienceEditor.generatePageContext(context, ExperienceEditor.getPageEditingWindow().document));
        }
    };
});
