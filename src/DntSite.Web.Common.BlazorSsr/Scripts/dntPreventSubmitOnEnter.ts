namespace DntBlazorSsr {
    export class DntPreventSubmitOnEnter {
        static enable(): void {
            document.querySelectorAll<HTMLFormElement>("form").forEach(element => {
                element.onkeydown = (event: KeyboardEvent) => {
                    if ((event.target as HTMLElement)?.nodeName === "TEXTAREA") {
                        return;
                    }
                    if (event.key === "Enter") {
                        event.preventDefault();
                    }
                };
            });
        }
    }
}
