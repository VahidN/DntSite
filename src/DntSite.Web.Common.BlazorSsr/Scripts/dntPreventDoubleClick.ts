namespace DntBlazorSsr {
    export class DntPreventDoubleClick {
        static enable(): void {
            document.querySelectorAll<HTMLButtonElement>("button[type=submit]").forEach(element => {
                element.onclick = () => {

                    const dataCancelConfirmAttribute = "data-cancel-confirm-message";
                    const hideButtonId = "data-cancel-button-hide";
                    const confirmButtonId = "data-cancel-button-confirm";

                    const confirmMessage = element.getAttribute(dataCancelConfirmAttribute);
                    if (confirmMessage) {
                        element.type = "button";
                        element.disabled = true;

                        const modalDiv = document.createElement('div');
                        modalDiv.innerHTML = '<div class="modal fade show d-block" tabindex="-1">' +
                            '<div class="modal-dialog modal-dialog-centered modal-dialog-scrollable" dir="rtl">' +
                            '     <div class="modal-content">' +
                            '          <div class="modal-body">' +
                            confirmMessage +
                            '          </div>' +
                            '          <div class="modal-footer">' +
                            `              <button id="${hideButtonId}" type="button" class="btn btn-success btn-sm">لغو</button>` +
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

                                element.type = "submit";
                                element.disabled = false;
                                element.setAttribute(dataCancelConfirmAttribute, confirmMessage);
                            };
                        }

                        const confirmButton = document.getElementById(confirmButtonId);
                        if (confirmButton) {
                            confirmButton.onclick = () => {
                                element.removeAttribute(dataCancelConfirmAttribute);
                                element.type = "submit";
                                element.disabled = false;
                                element.click();
                            };
                        }
                    } else {
                        if (!element.classList.contains('is-submitting')) {
                            element.classList.add('is-submitting');

                            const newSpanElement = document.createElement("span");
                            newSpanElement.classList.add('ms-2', 'spinner-grow', 'spinner-grow-sm', 'text-light');
                            element.append(newSpanElement);
                        } else {
                            element.type = "button";
                            element.disabled = true;
                        }
                    }
                };
            });
        }
    }
}
