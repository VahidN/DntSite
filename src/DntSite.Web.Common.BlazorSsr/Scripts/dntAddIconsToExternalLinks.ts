namespace DntBlazorSsr {
    interface CustomHTMLAnchorElement extends HTMLAnchorElement {
        itemprop?: string;
    }

    export class DntAddIconsToExternalLinks {
        static enable(): void {
            if (!window.navigator.onLine) {
                return;
            }

            const mySite = window.location.host;
            const googleFavIco = "https://www.google.com/s2/favicons?domain=";
            document.querySelectorAll<CustomHTMLAnchorElement>("a").forEach(link => {
                if (!link || !link.href) {
                    return;
                }

                if (document.querySelector('[contenteditable]')?.contains(link)) {
                    return;
                }

                if (link.title && link.title.toLowerCase().startsWith("share")) {
                    return;
                }

                if (link.classList && link.classList.contains('navbar-brand')) {
                    return;
                }

                if (link.itemprop && link.itemprop.toLowerCase().startsWith("social")) {
                    return;
                }

                if (link && link.href &&
                    link.href.match("^https?://") &&
                    !link.href.match(mySite) &&
                    link.getAttribute("itemprop") !== "social") {
                    const domain = link.href.replace(/<\S[^><]*>/g, "").split('/')[2];
                    link.style.background = `url(${googleFavIco}${domain}) center right no-repeat`;
                    link.style.paddingRight = "20px";
                    link.style.backgroundSize = "16px 16px";
                    link.target = "_blank";
                }
            });
        }
    }
}
