namespace DntBlazorSsr {
    export class DntBackToTop {
        static enable(): void {
            document.querySelectorAll<HTMLElement>('[id^="back-to-top"]').forEach(element => {
                element.onclick = () => {
                    window.scrollTo({top: 0, behavior: "smooth"});
                };
            });
        }
    }
}
