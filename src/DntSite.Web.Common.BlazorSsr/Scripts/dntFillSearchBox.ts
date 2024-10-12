namespace DntBlazorSsr {
    export class DntFillSearchBox {
        static enable(): void {
            document.querySelectorAll<HTMLButtonElement>("button[data-dnt-search-text]").forEach(element => {
                const searchText = element.getAttribute("data-dnt-search-text");
                if (!searchText) {
                    return;
                }

                element.onclick = () => {
                    document.querySelectorAll<HTMLInputElement>("input[type=search]").forEach(searchBox => {
                        searchBox.value = searchText;
                        searchBox.click();
                        searchBox.focus();
                    });
                };
            });
        }
    }
}
