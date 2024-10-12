namespace DntBlazorSsr {
    export class DntStickySidebar {
        static enable(): void {
            const sideBarMenu = document.querySelector<HTMLUListElement>("div#sidebar-menu > ul");
            if (sideBarMenu) {
                sideBarMenu.classList.add("sticky-top");
                sideBarMenu.style.zIndex = "1010";
                const header = document.getElementById("header");
                if (header) {
                    const top = header.clientHeight + 10;
                    sideBarMenu.style.top = `${top}px`;
                }
            }
        }
    }
}
