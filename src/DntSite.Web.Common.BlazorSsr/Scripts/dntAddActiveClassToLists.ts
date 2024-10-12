namespace DntBlazorSsr {
    export class DntAddActiveClassToLists {
        static enable(): void {
            const listItemsLinks = document.querySelectorAll<HTMLAnchorElement>("ul.list-group > a");
            const path = location.href.toLowerCase();
            listItemsLinks.forEach(link => {
                if (link.href && path.startsWith(`${link.href.toLowerCase()}`)) {
                    link.classList.add('active');
                } else {
                    link.classList.remove('active');
                }
            });
        }
    }
}
