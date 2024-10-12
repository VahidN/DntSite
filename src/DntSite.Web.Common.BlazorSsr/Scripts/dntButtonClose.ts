namespace DntBlazorSsr {
    export class DntButtonClose {
        static enable(): void {
            const dntButton = 'data-dnt-btn-close';
            document.querySelectorAll<HTMLButtonElement>(`[${dntButton}]`).forEach(element => {
                element.onclick = () => {
                    const divId = element.getAttribute(dntButton);
                    if (divId) {
                        const parentDiv = document.getElementById(divId);
                        if (parentDiv) {
                            parentDiv.style.display = 'none';
                        }
                    }

                    document.querySelectorAll('.modal-backdrop').forEach(element => {
                        element.classList.remove('modal-backdrop', 'fade', 'show');
                    });

                    document.querySelectorAll('.modal').forEach(element => {
                        element.classList.remove('modal', 'fade', 'show', 'd-block');
                    });
                };
            });
        }
    }
}
