﻿define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.AccessibilityChecker = {
        canExecute: function (context) {
            return true;
        },

        execute: function (context) {
            ExperienceEditor.PipelinesUtil.generateRequestProcessor("DreamTeam.CheckAccessibility", function (response) {
                var dialogFeatures = "dialogMinWidth: 800px;";
                ExperienceEditor.Dialogs.showModalDialog(response.responseValue.value, "", dialogFeatures, null);

                var parent = ExperienceEditor.getPageEditingWindow();

                console.log(window.axe);

                window.axe.run(parent.document, {
                    //exclude: ['iframe','.ui-dialog-normal']
                }, function (data) {
                    window.WCAG2.generateTable(data, document.getElementById('accessibility-result'));
                });
            }).execute(ExperienceEditor.generatePageContext(context, ExperienceEditor.getPageEditingWindow().document));
        }
    };
});
