namespace DntBlazorSsr {
    export class DntStyleSiteImages {
        static enable(): void {
            document.querySelectorAll<HTMLImageElement>('img').forEach(el => {
                if (el.src && el.src.toLowerCase().includes("/file/")) {
                    el.classList.add("rounded", "border", "shadow-sm", "border-secondary-subtle");
                }
            });
        }
    }
}
