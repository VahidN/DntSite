namespace DntBlazorSsr {
    export class DntStyleSiteImages {
        static enable(): void {
            document.querySelectorAll<HTMLImageElement>('img').forEach(el => {
                if (!el.src) {
                    return;
                }

                const src = el.src.toLowerCase();
                if (src.includes("/file/")) {
                    el.classList.add("rounded", "border", "shadow-sm", "border-secondary-subtle");
                    if (!src.includes("avatar")) {
                        el.classList.add("mt-2", "mb-2");
                    }
                }
            });
        }
    }
}
