namespace DntBlazorSsr {
    export class DntCatchImageErrors {
        static hideImage(imageElement: HTMLImageElement) {
            imageElement.style.visibility = 'hidden';
            imageElement.width = 0;
            imageElement.height = 0;
        }

        static handleImageError(imageElement: HTMLImageElement) {
            DntCatchImageErrors.hideImage(imageElement);
            DntReportErrors.postError(`Image error: Image ${imageElement.src} is missing @ ${window.location.href}.`);
        }

        static enable() {
            document.querySelectorAll<HTMLImageElement>('img').forEach((el) => {
                el.addEventListener('error', () => {
                    DntCatchImageErrors.handleImageError(el);
                });
            });
        }
    }
}
