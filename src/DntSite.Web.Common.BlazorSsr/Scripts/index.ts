(() => {
    'use strict';

    DntBlazorSsr.DntUtilities.enable();

    window.addEventListener('DOMContentLoaded', () => {
        // @ts-ignore
        if (typeof Blazor !== 'undefined') {
            // @ts-ignore
            Blazor.addEventListener('enhancedload', () => {
                DntBlazorSsr.DntUtilities.enable();
            });
        } else {
            console.error('Please include the `_framework/blazor.web.js` file first!');
        }
    });
})();
