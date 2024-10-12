namespace DntBlazorSsr {
    export class DntConfirmWhenLinkClicked {
        static enable(): void {
            const dataCancelConfirmAttribute = "data-cancel-confirm-message";
            document.querySelectorAll<HTMLAnchorElement>(`a[${dataCancelConfirmAttribute}]`).forEach(element => {
                element.onclick = (event) => {

                    const confirmMessage = element.getAttribute(dataCancelConfirmAttribute);
                    if (!confirmMessage) {
                        return;
                    }

                    event.preventDefault();

                    const hideButtonId = "data-cancel-button-hide";
                    const confirmButtonId = "data-cancel-button-confirm";

                    const modalDiv = document.createElement('div');
                    modalDiv.innerHTML = '<div class="modal fade show d-block" tabindex="-1">' +
                        '<div class="modal-dialog modal-dialog-centered modal-dialog-scrollable" dir="rtl">' +
                        '     <div class="modal-content">' +
                        '          <div class="modal-body">' +
                        confirmMessage +
                        '          </div>' +
                        '          <div class="modal-footer">' +
                        `              <button id="${hideButtonId}" type="button" class="btn btn-success btn-sm">بستن</button>` +
                        `              <button id="${confirmButtonId}" type="button" class="btn btn-danger btn-sm">ادامه</button>` +
                        '          </div>' +
                        '     </div>' +
                        '</div>' +
                        '</div>' +
                        '<div class="modal-backdrop fade show"></div>';
                    document.body.appendChild(modalDiv);

                    const hideButton = document.getElementById(hideButtonId);
                    if (hideButton) {
                        hideButton.onclick = () => {
                            modalDiv.style.display = 'none';
                            document.body.removeChild(modalDiv);
                        };
                    }

                    const confirmButton = document.getElementById(confirmButtonId);
                    if (confirmButton) {
                        confirmButton.onclick = () => {
                            element.removeAttribute(dataCancelConfirmAttribute);
                            element.click();
                        };
                    }
                };
            });
        }
    }
}
