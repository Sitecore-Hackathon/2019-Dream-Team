define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.AccessibilityChecker = {
        canExecute: function (context) {
            return true;
        },

        execute: function (context) {
            console.warn("AccessibilityChecker command is not implemented yet!");
        }
    };
});