namespace DntBlazorSsr {
    export class DntRemoteAutoComplete {

        static canonicalize(url: string): string {
            let div = document.createElement('div');
            div.innerHTML = "<a></a>";
            const child = div.firstChild as HTMLAnchorElement;
            child.href = url;
            div.innerHTML = div.innerHTML;
            return child.href;
        }

        static insertAfterElement(elem: Node | null, refElem: Node | null): Node | undefined {
            return elem ? refElem?.parentNode?.insertBefore(elem, refElem.nextSibling) : undefined;
        }

        static createElement(html: string): HTMLElement | null {
            let div = document.createElement('div');
            div.innerHTML = html;
            return div.firstChild as HTMLElement;
        }

        static enable(): void {
            document.querySelectorAll<HTMLInputElement>("[data-dnt-auto-complete]").forEach(element => {
                let queryUrl = element.getAttribute("data-dnt-auto-complete-remote-query-url");
                if (!queryUrl) {
                    return;
                }
                queryUrl = DntRemoteAutoComplete.canonicalize(queryUrl);

                const queryString = element.getAttribute("data-dnt-auto-complete-remote-query-string");
                if (!queryString) {
                    return;
                }

                const logUrl = element.getAttribute("data-dnt-auto-complete-remote-log-url");
                if (!logUrl) {
                    return;
                }

                const redirectUrl = element.getAttribute("data-dnt-auto-complete-redirect-url");
                if (!redirectUrl) {
                    return;
                }

                (element.parentNode as HTMLElement)?.classList.add('dropdown');
                const dropdown = DntRemoteAutoComplete.createElement(`<div class="dropdown-menu shadow-sm"></div>`);
                if (!dropdown) {
                    return;
                }
                DntRemoteAutoComplete.insertAfterElement(dropdown, element);

                const hideDropdown = () => {
                    dropdown.classList.remove('slideIn');
                    dropdown.classList.add('slideOut');
                    setTimeout(() => {
                        dropdown.innerHTML = '';
                    }, 50);
                };

                const hideDropdownOnEscKeyPress = (evt: KeyboardEvent) => {
                    evt = evt || window.event;
                    if (evt.key === "Escape" || evt.key === "Esc") {
                        hideDropdown();
                    }
                };

                document.onkeydown = (evt: KeyboardEvent) => {
                    hideDropdownOnEscKeyPress(evt);
                };

                document.onclick = (evt) => {
                    if (!dropdown.contains(evt.target as HTMLElement)) {
                        // Clicked outside the dropdown
                        hideDropdown();
                    }
                };

                const showDropdown = () => {
                    dropdown.classList.remove('slideOut');
                    dropdown.classList.add('slideIn');
                    dropdown.style.display = 'block';
                };

                const showData = (data: string[]) => {
                    const items = element.nextSibling as HTMLElement;
                    items.innerHTML = '';

                    for (let i = 0; i < data.length; i++) {
                        const node = DntRemoteAutoComplete.createElement(data[i]);
                        if (node) {
                            items.appendChild(node);
                        }
                    }

                    if (data.length > 0) {
                        showDropdown();
                    } else {
                        hideDropdown();
                    }
                };

                const searchedItems: string[] = [];
                const logSearchedValues = () => {
                    const logSearchedData = () => {
                        const searchValue = element.value;

                        if (!searchValue || searchedItems.includes(searchValue)) {
                            return;
                        }

                        searchedItems.push(searchValue);
                        fetch(logUrl, {
                            method: "POST",
                            body: JSON.stringify(searchValue),
                            headers: {
                                'Accept': 'application/json; charset=utf-8',
                                'Content-Type': 'application/json; charset=utf-8',
                                'Pragma': 'no-cache'
                            }
                        });
                    };

                    (element.nextSibling as HTMLElement)?.querySelectorAll<HTMLElement>('.dropdown-item').forEach(item => {
                        item.onclick = (event: MouseEvent) => {
                            logSearchedData();
                            hideDropdown();
                        };
                        item.onmousedown = (event: MouseEvent) => {
                            if (event.button === 2) { // right click
                                logSearchedData();
                            }
                        };
                    });
                };

                let controller = new AbortController();
                let signal = controller.signal;

                const doFetchRemoteData = () => {
                    controller.abort();
                    controller = new AbortController();
                    signal = controller.signal;

                    if (!element.value) {
                        hideDropdown();
                        return;
                    }

                    const url = new URL(queryUrl);
                    url.searchParams.append(queryString, element.value);
                    fetch(url, {
                            method: "GET",
                            signal: signal,
                            headers: {
                                'Pragma': 'no-cache'
                            }
                        }
                    )
                        .then(response => {
                            if (response.ok) {
                                return response;
                            }
                            return Promise.reject(response);
                        })
                        .then(response => response.json())
                        .then(data => {
                            showData(data);
                            logSearchedValues();
                        })
                        .catch(error => {
                            hideDropdown();
                        });
                };

                element.oninput = () => {
                    doFetchRemoteData();
                };

                element.onclick = () => {
                    doFetchRemoteData();
                };

                element.onkeydown = (event: KeyboardEvent) => {
                    if (event.key === 'Escape') {
                        hideDropdown();
                    } else if (event.key === 'Enter') {
                        window.location.href = `${redirectUrl}/${encodeURIComponent(element.value)}`;
                    }
                };
            });
        }
    }
}
