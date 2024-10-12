namespace DntBlazorSsr {
    export class DntInputTag {
        static manageRemoveButtons() {
            const dntButton = 'data-dnt-tag-id';
            document.querySelectorAll<HTMLButtonElement>(`[${dntButton}]`).forEach(element => {
                element.onclick = () => {
                    const divId = element.getAttribute(dntButton);
                    if (divId) {
                        const parentDiv = document.getElementById(divId);
                        if (parentDiv) {
                            parentDiv.remove();
                        }
                    }
                };
            });
        }

        static manageAddButton() {
            document.querySelectorAll<HTMLInputElement>('[id^="dnt-add-tag"]').forEach(element => {
                element.onclick = () => {
                    const dataListId = element.getAttribute("data-dnt-data-list-id");
                    if (!dataListId) {
                        return;
                    }

                    const containerDiv = document.getElementById(`tags-list-${dataListId}`);
                    if (!containerDiv) {
                        return;
                    }

                    const input = document.querySelector<HTMLInputElement>(`input[list='${dataListId}']`);
                    if (!input) {
                        return;
                    }

                    const tagValue = input.value;
                    if (!tagValue) {
                        return;
                    }

                    const dir = DntChangeInputDirectionDependOnLanguage.getDirection(tagValue);

                    const tagDiv = document.createElement('div');
                    tagDiv.classList.add('badge', 'bg-secondary', 'me-2');
                    tagDiv.id = `dnt-tag-${tagValue}`;
                    tagDiv.innerHTML =
                        ` <span class="me-1" dir="${dir}">${tagValue}</span>` +
                        ` <input type="hidden" name="EnteredTags" value="${tagValue}"/>` +
                        ' <button type="button"' +
                        '        class="btn-close btn-close-white"' +
                        `        data-dnt-tag-id="dnt-tag-${tagValue}"` +
                        '        title="حذف تگ از لیست">' +
                        ' </button>';
                    containerDiv.appendChild(tagDiv);

                    input.value = '';
                    input.focus();

                    DntInputTag.manageRemoveButtons();
                };
            });
        }

        static manageAddTagOnEnter() {
            document.querySelectorAll<HTMLElement>('[id^="dnt-add-tag"]').forEach(element => {
                const dataListId = element.getAttribute("data-dnt-data-list-id");
                if (!dataListId) {
                    return;
                }

                const input = document.querySelector<HTMLInputElement>(`input[list='${dataListId}']`);
                if (!input) {
                    return;
                }

                input.onkeydown = (event) => {
                    if (event.key === "Enter") {
                        element.click();
                    }
                };
            });
        }

        static enable(): void {
            DntInputTag.manageAddTagOnEnter();
            DntInputTag.manageAddButton();
            DntInputTag.manageRemoveButtons();
        }
    }
}
