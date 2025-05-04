namespace DntBlazorSsr {
    export class DntApplyBootstrapTable {
        static enable(): void {
            DntApplyBootstrapTable.applyBootstrapStyles(document, "rtl");
        }

        static applyBootstrapStyles(htmlElement: ParentNode, direction: string): void {
            htmlElement.querySelectorAll<HTMLTableElement>("table:not([class~='table'])").forEach(element => {
                element.classList.add('table', 'table-striped', 'table-hover', 'table-bordered', 'table-condensed');
                element.style.maxWidth = "100%";
                element.style.width = "auto";
                element.style.marginLeft = "auto";
                element.style.marginRight = "auto";
                element.style.direction = direction;
            });
        }

        static centerAlignAllTableCells(editorElement: HTMLElement) {
            const tables = editorElement.querySelectorAll<HTMLTableElement>('table');
            tables.forEach(table => {
                const cells = table.querySelectorAll<HTMLElement>('th, td');
                cells.forEach(cell => {
                    cell.style.textAlign = 'center';
                    cell.style.verticalAlign = 'middle';
                });
            });
        }
    }
}
