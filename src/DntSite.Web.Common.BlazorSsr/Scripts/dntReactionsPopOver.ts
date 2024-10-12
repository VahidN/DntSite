namespace DntBlazorSsr {
    export class DntReactionsPopOver {
        static enable(): void {
            document.querySelectorAll<HTMLElement>('[data-bs-toggle="reaction"]').forEach(element => {
                element.onclick = () => {
                    const upsDivId = element.getAttribute('data-bs-toggle-up');
                    if (upsDivId) {
                        const upsDiv = document.getElementById(upsDivId);
                        if (upsDiv) {
                            upsDiv.classList.remove('d-none');
                            upsDiv.style.display = "block";
                        }
                    }

                    const downsDivId = element.getAttribute('data-bs-toggle-down');
                    if (downsDivId) {
                        const downsDiv = document.getElementById(downsDivId);
                        if (downsDiv) {
                            downsDiv.classList.remove('d-none');
                            downsDiv.style.display = "block";
                        }
                    }

                    DntButtonClose.enable();
                };
            });
        }
    }
}
