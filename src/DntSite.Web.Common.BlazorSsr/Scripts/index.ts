(() => {
    'use strict';

    DntBlazorSsr.DntUtilities.enable();

    window.addEventListener('DOMContentLoaded', () => {
        // @ts-ignore
        Blazor.addEventListener('enhancedload', () => {
            DntBlazorSsr.DntUtilities.enable();
        });
    });
})();
