(function (window) {
    window.WCAG2 = {
        /**
         *
         * @param {Array} violations
         */
        generateRows: function (violations) {
            var rows = '';
            violations.forEach(function (item) {
                rows += '<tr>' +
                    '<td><img src="/sitecore/shell/themes/standard/images/bullet_square_red.png" width="16" height="16" align="middle"\n' +
                    '             class="" alt="" border="0"></td>' +
                    '<td><div><b>' + item.id + '</b> - ' + item.impact + '</div></td>' +
                    '<td class="scValidatorResult">' + item.description + '</td>' +
                    '</tr>';
            });

            return rows;
        },
        /**
         *
         * @param {{violations: Array}} data
         * @param {HTMLElement} target
         */
        generateTable: function (data, target) {
            var fragment = '<table class="scListControl" width="100%" cellpadding="0" border="0">' +
                '<tbody>' +
                this.generateRows(data.violations) +
                '</tbody>' +
                '</table>';

            target.innerHTML = fragment;
        }
    };
})(window);
