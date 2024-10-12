namespace DntBlazorSsr {
    export class DntButtonSpinner {
        static enable(): void {
            const dntButton = 'data-dnt-button-spinner';
            document.querySelectorAll<HTMLButtonElement>(`[${dntButton}]`).forEach((element, key, parent) => {
                element.onclick = () => {
                    const buttonElement = element;
                    buttonElement.style.display = 'none';

                    const divId = buttonElement.getAttribute(dntButton);
                    if (divId) {
                        document.getElementById(divId)?.classList.remove('d-none');
                    }
                };
            });
        }
    }
}
