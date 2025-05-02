namespace DntBlazorSsr {
    export class DntApplyBootstrapTable {
        static enable(): void {
            DntApplyBootstrapTable.applyTo(document, "rtl");
        }

        static applyTo(htmlElement: ParentNode, direction: string): void {
            htmlElement.querySelectorAll<HTMLTableElement>("table:not([class~='table'])").forEach(element => {
                element.classList.add('table', 'table-striped', 'table-hover', 'table-bordered', 'table-condensed');
                element.style.maxWidth = "100%";
                element.style.width = "auto";
                element.style.marginLeft = "auto";
                element.style.marginRight = "auto";
                element.style.direction = direction;
            });
        }
    }
}
