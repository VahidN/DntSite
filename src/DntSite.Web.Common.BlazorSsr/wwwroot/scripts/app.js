﻿window.DntCardAccordion = {
    enable: () => {
        document.querySelectorAll("button[data-dnt-collapse]").forEach(element => {
            element.title = "بستن محتوا";
            element.onclick = () => {
                const card = element.parentElement?.parentElement;
                if (!card) {
                    return;
                }

                card.querySelectorAll(".card-body, .card-footer, .list-group").forEach(cardChild => {
                    if (cardChild.classList.contains("collapse")) {
                        cardChild.classList.remove("accordion-collapse", "collapse");

                        element.firstElementChild.classList.remove("bi-chevron-left");
                        element.firstElementChild.classList.add("bi-chevron-down");
                        element.title = "بستن محتوا";

                    } else {
                        cardChild.classList.add("accordion-collapse", "collapse");

                        element.firstElementChild.classList.remove("bi-chevron-down");
                        element.firstElementChild.classList.add("bi-chevron-left");
                        element.title = "گشودن محتوا";
                    }
                });
            };
        });
    }
};

window.DntStickySidebar = {
    enable: () => {
        const sideBarMenu = document.querySelector("div#sidebar-menu > ul");
        if (sideBarMenu) {
            sideBarMenu.classList.add("sticky-top");
            sideBarMenu.style.zIndex = "1010";
            const header = document.getElementById("header");
            if (header) {
                const top = header.clientHeight + 10;
                sideBarMenu.style.top = `${top}px`;
            }
        }
    }
};

window.DntFillSearchBox = {
    enable: () => {
        document.querySelectorAll("button[data-dnt-search-text]").forEach(element => {
            const searchText = element.getAttribute("data-dnt-search-text");
            if (!searchText) {
                return;
            }

            element.onclick = () => {
                document.querySelectorAll("input[type=search]").forEach(searchBox => {
                    searchBox.value = searchText;
                    searchBox.click();
                    searchBox.focus();
                });
            };
        });
    }
};

window.DntRemoteAutoComplete = {
    canonicalize: (url) => {
        let div = document.createElement('div');
        div.innerHTML = "<a></a>";
        div.firstChild.href = url;
        div.innerHTML = div.innerHTML;
        return div.firstChild.href;
    },
    insertAfterElement: (elem, refElem) => {
        return refElem.parentNode.insertBefore(elem, refElem.nextSibling);
    },
    createElement: (html) => {
        let div = document.createElement('div');
        div.innerHTML = html;
        return div.firstChild;
    },
    enable: () => {
        document.querySelectorAll("[data-dnt-auto-complete]").forEach(element => {
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

            element.parentNode.classList.add('dropdown');
            const dropdown = DntRemoteAutoComplete.createElement(`<div class="dropdown-menu shadow-sm"></div>`);
            DntRemoteAutoComplete.insertAfterElement(dropdown, element);

            const hideDropdown = () => {
                dropdown.classList.remove('slideIn');
                dropdown.classList.add('slideOut');
                setTimeout(() => {
                    dropdown.innerHTML = '';
                }, 50);
            };

            const hideDropdownOnEscKeyPress = (evt) => {
                evt = evt || window.event;
                let isEscape = false;
                if ("key" in evt) {
                    isEscape = (evt.key === "Escape" || evt.key === "Esc");
                } else {
                    isEscape = (evt.keyCode === 27);
                }
                if (isEscape) {
                    hideDropdown();
                }
            };

            document.onkeydown = (evt) => {
                hideDropdownOnEscKeyPress(evt);
            };

            document.onclick = (evt) => {
                if (!dropdown.contains(evt.target)) {
                    // Clicked outside the dropdown
                    hideDropdown();
                }
            };

            const showDropdown = () => {
                dropdown.classList.remove('slideOut');
                dropdown.classList.add('slideIn');
                dropdown.style.display = 'block';
            };

            const showData = (data) => {
                const items = element.nextSibling;
                items.innerHTML = '';

                for (let i = 0; i < data.length; i++) {
                    items.appendChild(DntRemoteAutoComplete.createElement(data[i]));
                }

                if (data.length > 0) {
                    showDropdown();
                } else {
                    hideDropdown();
                }
            };

            const searchedItems = [];
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

                element.nextSibling.querySelectorAll('.dropdown-item').forEach((item) => {
                    item.onclick = (event) => {
                        logSearchedData();
                        hideDropdown();
                    };
                    item.onmousedown = (event) => {
                        const keycode = (event.keyCode ? event.keyCode : event.which);
                        if (keycode === 3) { // right click
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

            element.onkeydown = (event) => {
                if (event.key === 'Escape') {
                    hideDropdown();
                } else if (event.key === 'Enter') {
                    window.location.href = `${redirectUrl}/${encodeURIComponent(element.value)}`;
                }
            };
        });
    }
};

window.DntPersianDatePicker = {
    showDatePicker: (inputElement) => {
        const _faNums = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
        const _arNums = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
        const _faDigitsRegex = /[۰۱۲۳۴۵۶۷۸۹]/g;
        const _arDigitsRegex = /[٠١٢٣٤٥٦٧٨٩]/g;
        const _enDigitsRegex = /[0-9]/g;
        const _dayNames = ["شنبه", `یکشنبه`, `دوشنبه`, `سه‌شنبه`, `چهارشنبه`, `پنج‌شنبه`, 'جمعه'];
        const _monthNames = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'];

        let _datePicker = null;
        let _textBox = null;
        let _datePickerStyle = null;
        let _isClickInsideDatePickerDiv = false;

        const toEnglishNumbers = (inputNumber) => {
            if (!inputNumber) return '';
            let value = inputNumber.toString().trim();
            if (!value) return '';

            value = value.replace(_faDigitsRegex, (char) => `${_faNums.indexOf(char)}`);
            return value.replace(_arDigitsRegex, (char) => `${_arNums.indexOf(char)}`);
        };

        const toPersianNumbers = (inputNumber) => {
            if (!inputNumber) return '';
            let value = inputNumber.toString().trim();
            if (!value) return '';

            return value.replace(_enDigitsRegex, (char) => `${_faNums[Number(char)]}`);
        };

        const todayPersianStr = toEnglishNumbers(new Intl.DateTimeFormat("fa", {
            year: "numeric",
            month: "2-digit",
            day: "2-digit"
        }).format(Date.now()));

        const init = () => {
            _datePicker = createElement("div", document.body);
            _datePickerStyle = _datePicker.style;
            _datePickerStyle.position = "absolute";
            _datePickerStyle.zIndex = "10000000";
            _datePicker.onmousedown = () => {
                _isClickInsideDatePickerDiv = true;
            };
            _datePicker.onclick = () => {
                _textBox.focus();
            };
        };

        const hideCalendar = () => {
            _datePickerStyle.visibility = "hidden";
        };

        const showCalendar = (textBox) => {
            if (_datePicker == null) {
                init();
            }
            _textBox = textBox;
            _textBox.value = toEnglishNumbers(_textBox.value);
            _datePicker.tabIndex = 0; // to accept focus
            _isClickInsideDatePickerDiv = false;
            _textBox.onblur = () => {
                if (!_isClickInsideDatePickerDiv) {
                    hideCalendar();
                }
            };

            let left = 0;
            let top = 0;
            let parent = _textBox;
            while (parent.offsetParent) {
                left += parent.offsetLeft;
                top += parent.offsetTop;
                parent = parent.offsetParent;
            }
            _datePickerStyle.left = left + "px";
            _datePickerStyle.top = top + _textBox.offsetHeight + "px";
            _datePickerStyle.visibility = "visible";

            drawCalendar(_textBox.value);
        };

        const toInt = (text) => {
            return parseInt(text, 10);
        };

        const getPersianDateParts = (persianDateStr) => {
            if (!persianDateStr) {
                persianDateStr = todayPersianStr;
            }

            let dateParts = persianDateStr.split(/[\/\-.]/, 3);
            if (dateParts.length !== 3) {
                return getPersianDateParts(todayPersianStr);
            }
            return [toInt(dateParts[0]), toInt(dateParts[1]), toInt(dateParts[2])];
        };

        const persianDatePartsToStr = (parts) => {
            return !parts ? "" : `${parts[0]}/${String(parts[1]).padStart(2, '0')}/${String(parts[2]).padStart(2, '0')}`;
        }

        const nextYear = (date) => {
            const parts = getPersianDateParts(date);
            return persianDatePartsToStr([parts[0] + 1, parts[1], parts[2]]);
        };

        const previousYear = (date) => {
            const parts = getPersianDateParts(date);
            return persianDatePartsToStr([parts[0] - 1, parts[1], parts[2]]);
        };

        const nextMonth = (date) => {
            const parts = getPersianDateParts(date);
            return parts[1] < 12 ?
                persianDatePartsToStr([parts[0], parts[1] + 1, parts[2]]) :
                persianDatePartsToStr([parts[0] + 1, 1, parts[2]]);
        };

        const previousMonth = (date) => {
            const parts = getPersianDateParts(date);
            return parts[1] > 1 ?
                persianDatePartsToStr([parts[0], parts[1] - 1, parts[2]]) :
                persianDatePartsToStr([parts[0] - 1, 12, parts[2]]);
        };

        const isLeapYear = (year) => {
            return (((((year - 474) % 2820) + 512) * 682) % 2816) < 682;
        };

        const mod = (a, b) => {
            return Math.abs(a - (b * Math.floor(a / b)));
        };

        const getWeekDay = (date) => {
            return mod(getDiffDays('1392/03/25', date), 7);
        };

        const getDays = (date) => {
            const parts = getPersianDateParts(date);
            return parts[1] < 8 ?
                (parts[1] - 1) * 31 + parts[2] :
                6 * 31 + (parts[1] - 7) * 30 + parts[2];
        };

        const getMonthDays = (date) => {
            const parts = getPersianDateParts(date);
            if (parts[1] < 7)
                return 31;
            if (parts[1] < 12)
                return 30;
            return isLeapYear(parts[0]) ? 30 : 29;
        };

        const getDiffDays = (date1, date2) => {
            let diffDays = getDays(date2) - getDays(date1);
            let dateArray1 = getPersianDateParts(date1);
            let dateArray2 = getPersianDateParts(date2);
            let y1 = (dateArray1[0] < dateArray2[0]) ? dateArray1[0] : dateArray2[0];
            let y2 = (dateArray1[0] < dateArray2[0]) ? dateArray2[0] : dateArray1[0];
            for (let y = y1; y < y2; y++)
                if (isLeapYear(y))
                    diffDays += (dateArray1[0] < dateArray2[0]) ? 366 : -366;
                else
                    diffDays += (dateArray1[0] < dateArray2[0]) ? 365 : -365;
            return toInt(diffDays);
        };

        const changeDay = (date, day) => {
            const parts = getPersianDateParts(date);
            return persianDatePartsToStr([parts[0], parts[1], day]);
        };

        const setValue = (date) => {
            _textBox.value = date;
            _textBox.focus();
            hideCalendar();
        };

        const createElement = (tag, parent) => {
            const element = document.createElement(tag);
            parent.appendChild(element);
            return element;
        };

        const drawCalendarFooter = (table) => {
            const tr = table.insertRow(8);
            setClassName(tr, "table-secondary");

            let td = createElement("td", tr)
            td.colSpan = 2;

            let button = createElement("button", td);
            button.classList.add('btn', 'btn-success', 'btn-sm');
            setInnerHTML(button, "امروز");
            button.onclick = () => {
                setValue(todayPersianStr);
            };

            td = createElement("td", tr)
            td.colSpan = 2;
            td.style.textAlign = "center";

            button = createElement("button", td);
            button.classList.add('btn', 'btn-secondary', 'btn-sm');
            setInnerHTML(button, "بستن");
            button.onclick = () => {
                hideCalendar();
            };

            td = createElement("td", tr)
            td.colSpan = 3;
            td.style.textAlign = "left";

            button = createElement("button", td);
            button.classList.add('btn', 'btn-danger', 'btn-sm');
            setInnerHTML(button, "خالی");
            button.onclick = () => {
                setValue('');
            };
        }

        const drawCalendarRows = (table, date) => {
            const weekDay = getWeekDay(changeDay(date, 1));

            for (let row = 0; row < 7; row++) {
                let tr = table.insertRow(row + 1);

                if (row === 6)
                    setClassName(tr, "table-danger");
                else if (mod(row, 2) !== 1)
                    setClassName(tr, "datePickerRow");

                const th = createElement("th", tr)
                setInnerHTML(th, _dayNames[row]);
                setClassName(th, "text-center");

                for (let col = 0; col < 6; col++) {
                    const cellValue = col * 7 + row - weekDay + 1;

                    let td = createElement("td", tr);

                    if (cellValue > 0 && cellValue <= getMonthDays(date)) {
                        setInnerHTML(td, `<button class="btn btn-outline-secondary btn-sm">${toPersianNumbers(cellValue)}</button>`);

                        let cellDate = changeDay(date, cellValue);
                        let cellClassName = "text-center";

                        if (cellDate === _textBox.value)
                            cellClassName = "text-center bg-info";
                        else if (cellDate === todayPersianStr)
                            cellClassName = "text-center bg-warning";

                        setClassName(td, cellClassName);

                        td.onclick = () => {
                            setValue(changeDay(date, cellValue));
                        };
                    }
                }
            }
        }

        const drawCalendarHeader = (table, date) => {
            let tr = table.insertRow(0);
            setClassName(tr, "table-primary");

            let td = createElement("td", tr)
            td.colSpan = 3;

            let button = createElement("button", td);
            button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
            setInnerHTML(button, "❰");
            button.onclick = () => {
                drawCalendar(previousMonth(date));
            };

            let span = createElement("strong", td);
            const parts = getPersianDateParts(date);
            setInnerHTML(span, _monthNames[parts[1] - 1]);
            setClassName(span, "datePickerMonth");

            button = createElement("button", td);
            button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
            setInnerHTML(button, "❱");
            button.onclick = () => {
                drawCalendar(nextMonth(date));
            };

            td = createElement("td", tr)
            td.colSpan = 4;
            td.style.textAlign = "left";

            button = createElement("button", td);
            button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
            setInnerHTML(button, "❰");
            button.onclick = () => {
                drawCalendar(previousYear(date));
            };

            span = createElement("strong", td)
            setInnerHTML(span, toPersianNumbers(String(parts[0])));
            setClassName(span, "datePickerYear");

            button = createElement("button", td);
            button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
            setInnerHTML(button, "❱");
            button.onclick = () => {
                drawCalendar(nextYear(date));
            };
        }

        const createCalendarTable = () => {
            let table = createElement("table", _datePicker);
            setClassName(table, "table table-borderless table-striped table-striped-columns shadow-sm rounded-3");
            table.cellSpacing = 0;
            return table;
        }

        const drawCalendar = (date) => {
            _textBox.focus();

            setInnerHTML(_datePicker, "");

            const table = createCalendarTable();
            drawCalendarHeader(table, date);
            drawCalendarRows(table, date);
            drawCalendarFooter(table);
        };

        const setInnerHTML = (element, html) => {
            element.innerHTML = html;
        };

        const setClassName = (element, className) => {
            element.className = className;
        };

        showCalendar(inputElement);
    },
    enable: () => {
        document.querySelectorAll("[data-dnt-date-picker]").forEach(element => {
            element.onclick = () => {
                DntPersianDatePicker.showDatePicker(element);
            };
        });
    }
};

window.DntWindowLocationChangeWatcher = {
    showLoadingSpinner: () => {
        const spinnerId = `dnt-loading-spinner`;

        const existingSpinner = document.getElementById(spinnerId);
        existingSpinner?.remove();

        const spinnerDiv = document.createElement('div');
        spinnerDiv.classList.add('modal', 'fade', 'show', 'slideIn', 'd-none', 'bg-dark', 'bg-opacity-25');
        spinnerDiv.id = spinnerId;
        spinnerDiv.innerHTML = `
                    <div id="app" class="d-flex flex-column min-vh-100">
                        <div class="d-flex vh-100">
                            <div class="d-flex w-100 justify-content-center align-self-center">
                              <div class="spinner-border text-primary m-5" style="width: 5rem; height: 5rem" role="status">
                                <span class="visually-hidden">Loading ... </span>
                              </div>
                            </div>
                        </div>
                    </div>
        `;
        document.body.prepend(spinnerDiv);

        setTimeout(() => {
            const loadingElement = document.getElementById(spinnerId)
            if (loadingElement) {
                loadingElement.classList.remove('d-none');
                loadingElement.classList.add('d-block');
                document.body.prepend(loadingElement);
            }
        }, 300);

        setTimeout(() => {
            const loadingElement = document.getElementById(spinnerId)
            loadingElement?.remove();
        }, 3000);
    },
    locationChanged: () => {
        DntWindowLocationChangeWatcher.showLoadingSpinner();
    },
    enable: () => {
        const {pushState, replaceState} = window.history;

        window.history.pushState = function (...args) {
            pushState.apply(window.history, args);
            window.dispatchEvent(new Event('pushState'));
        };

        window.history.replaceState = function (...args) {
            replaceState.apply(window.history, args);
            window.dispatchEvent(new Event('replaceState'));
        };

        window.addEventListener('popstate', () => DntWindowLocationChangeWatcher.locationChanged());
        window.addEventListener('replaceState', () => DntWindowLocationChangeWatcher.locationChanged());
        window.addEventListener('pushState', () => DntWindowLocationChangeWatcher.locationChanged());
    }
};


window.DntConfirmWhenLinkClicked = {
    enable: () => {
        const dataCancelConfirmAttribute = "data-cancel-confirm-message";
        document.querySelectorAll(`a[${dataCancelConfirmAttribute}]`).forEach(element => {
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

                document.getElementById(hideButtonId).onclick = () => {
                    modalDiv.style.display = 'none';
                    document.body.removeChild(modalDiv);
                };

                document.getElementById(confirmButtonId).onclick = () => {
                    element.removeAttribute(dataCancelConfirmAttribute);
                    element.click();
                };
            };
        });
    }
};

window.DntInputTag = {
    manageRemoveButtons: () => {
        const dntButton = 'data-dnt-tag-id';
        document.querySelectorAll(`[${dntButton}]`).forEach((element, key, parent) => {
            element.onclick = (event) => {
                const divId = element.getAttribute(dntButton);
                if (divId) {
                    const parentDiv = document.getElementById(divId);
                    if (parentDiv) {
                        parentDiv.remove();
                    }
                }
            };
        });
    },
    manageAddButton: function () {
        document.querySelectorAll('[id^="dnt-add-tag"]').forEach(element => {
            element.onclick = () => {
                const dataListId = element.getAttribute("data-dnt-data-list-id");
                if (!dataListId) {
                    return;
                }

                const containerDiv = document.getElementById(`tags-list-${dataListId}`);
                if (!containerDiv) {
                    return;
                }

                const input = document.querySelector(`input[list='${dataListId}']`);
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
    },
    manageAddTagOnEnter: () => {
        document.querySelectorAll('[id^="dnt-add-tag"]').forEach(element => {
            const dataListId = element.getAttribute("data-dnt-data-list-id");
            if (!dataListId) {
                return;
            }

            const input = document.querySelector(`input[list='${dataListId}']`);
            if (!input) {
                return;
            }

            input.onkeydown = (event) => {
                if (event.key === "Enter") {
                    element.click();
                }
            };
        });
    },
    enable: () => {
        DntInputTag.manageAddTagOnEnter();
        DntInputTag.manageAddButton();
        DntInputTag.manageRemoveButtons();
    }
};

window.DntChangeInputDirectionDependOnLanguage = {
    containsRtlRegExp: () => {
        const faAlphabet = "ابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی";
        const faNumber = "۰۱۲۳۴۵۶۷۸۹";
        const faShortVowels = "َُِ";
        const faOthers = "‌آاً";
        const faMixedWithArabic = "ًٌٍَُِّْٰٔءک‌ةۀأإیـئؤ،";
        const faText = faAlphabet + faNumber + faShortVowels + faOthers;
        const faComplexText = faText + faMixedWithArabic;
        return new RegExp(`[${faComplexText}]`);
    },
    containsRtlText: (text) => {
        return !text ? false : DntChangeInputDirectionDependOnLanguage.containsRtlRegExp().test(text);
    },
    getDirection: (text) => {
        return DntChangeInputDirectionDependOnLanguage.containsRtlText(text) ? "rtl" : "ltr";
    },
    enable: () => {
        document.querySelectorAll("input[type=text]").forEach(element => {
            element.onkeyup = () => {
                element.style.direction = DntChangeInputDirectionDependOnLanguage.getDirection(element.value);
            };
        });
    }
};

window.DntPreventSubmitOnEnter = {
    enable: () => {
        document.querySelectorAll("form").forEach(element => {
            element.onkeydown = (event) => {
                if (event.target.nodeName === "TEXTAREA") {
                    return;
                }
                if (event.key === "Enter") {
                    event.preventDefault();
                }
            };
        });
    }
};

window.DntStyleValidationMessages = {
    enable: () => {
        const validationMessages = document.querySelectorAll("div.validation-message");
        validationMessages.forEach(element => {
            element.classList.add('badge', 'text-bg-danger', 'fs-6', 'mt-2');
        });

        validationMessages[0]?.scrollIntoView({
            behavior: "smooth",
            block: "center",
            inline: "nearest"
        });
    }
};

window.DntPreventDoubleClick = {
    enable: () => {
        document.querySelectorAll("button[type=submit]").forEach(element => {
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

                    document.getElementById(hideButtonId).onclick = () => {
                        modalDiv.style.display = 'none';
                        document.body.removeChild(modalDiv);

                        element.type = "submit";
                        element.disabled = false;
                        element.setAttribute(dataCancelConfirmAttribute, confirmMessage);
                    };

                    document.getElementById(confirmButtonId).onclick = () => {
                        element.removeAttribute(dataCancelConfirmAttribute);
                        element.type = "submit";
                        element.disabled = false;
                        element.click();
                    };
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
};

window.DntApplyBootstarpTable = {
    enable: () => {
        document.querySelectorAll("table:not([class~='table'])").forEach(element => {
            element.classList.add('table', 'table-striped', 'table-hover', 'table-bordered', 'table-condensed');
            element.style.maxWidth = "100%";
            element.style.width = "auto";
            element.style.marginLeft = "auto";
            element.style.marginRight = "auto";
        });
    }
};

window.DntDocumentFontSize = {
    enable: () => {
        const mainBodies = document.querySelectorAll("div.main");
        if (!mainBodies) {
            return;
        }

        const btnPlus = document.querySelector('button#btn_plus');
        if (!btnPlus) {
            return;
        }

        const btnMinus = document.querySelector('button#btn_minus');
        if (!btnMinus) {
            return;
        }

        const btnReset = document.querySelector('button#btn_reset');
        if (!btnReset) {
            return;
        }

        const fontSizes = ['fs-6', 'fs-5', 'fs-4', 'fs-3', 'fs-2', 'fs-1'];
        let fontSizeIndex = 0;
        let cacheKey = "body-font-size";
        let fontSize = localStorage.getItem(cacheKey);
        if (!fontSize) {
            fontSize = fontSizes[fontSizeIndex];
        } else {
            fontSizeIndex = fontSizes.indexOf(fontSize);
        }

        const setMainFontSize = size => {
            mainBodies.forEach(mainBody => {
                fontSizes.forEach(item => mainBody.classList.remove(item));
                mainBody.classList.add(size);
                localStorage.setItem(cacheKey, size);
            });
        };

        setMainFontSize(fontSize);

        btnPlus.onclick = (event) => {
            fontSizeIndex++;
            if (fontSizeIndex < fontSizes.length) {
                setMainFontSize(fontSizes[fontSizeIndex]);
            } else {
                fontSizeIndex = fontSizes.length - 1;
            }
        };

        btnMinus.onclick = (event) => {
            fontSizeIndex--;
            if (fontSizeIndex >= 0) {
                setMainFontSize(fontSizes[fontSizeIndex]);
            } else {
                fontSizeIndex = 0;
            }
        };

        btnReset.onclick = (event) => {
            fontSizeIndex = 0;
            setMainFontSize(fontSizes[fontSizeIndex]);
        };
    }
};

window.DntReactionsPopOver = {
    enable: () => {
        document.querySelectorAll('[data-bs-toggle="reaction"]').forEach((element, key, parent) => {
            element.onclick = (event) => {
                const upsDivId = element.getAttribute('data-bs-toggle-up');
                const upsDiv = document.getElementById(upsDivId);
                if (upsDiv) {
                    upsDiv.classList.remove('d-none');
                    upsDiv.style.display = "block";
                }

                const downsDivId = element.getAttribute('data-bs-toggle-down');
                const downsDiv = document.getElementById(downsDivId);
                if (downsDiv) {
                    downsDiv.classList.remove('d-none');
                    downsDiv.style.display = "block";
                }

                DntButtonClose.enable();
            };
        });
    }
};


window.DntInputFile = {
    enable: () => {
        document.querySelectorAll('.form-control[type=file].d-none').forEach((fileInput, key, parent) => {
            fileInput.onchange = (event) => {
                const fileInputElement = fileInput;
                const nextElementSibling = fileInputElement.nextElementSibling;
                if (fileInputElement.files == null || fileInputElement.files[0] == null || !nextElementSibling) {
                    return;
                }

                nextElementSibling.innerHTML = [...fileInputElement.files].map(f => f.name).join(', ');
                nextElementSibling.classList.remove('d-none');
            };
        });
    }
};


window.DntNavLinkMenu = {
    enable: () => {
        document.querySelectorAll('a.nav-link.dropdown-toggle').forEach((element, key, parent) => {
            element.onclick = (event) => {
                const nextElementSibling = element.nextElementSibling;
                if (!nextElementSibling) {
                    return;
                }

                event.preventDefault();

                nextElementSibling.classList.remove('slideOut');
                nextElementSibling.classList.add('slideIn');
                nextElementSibling.style.display = 'block';
            };
            element.onblur = () => {
                setTimeout(() => {
                    const nextElementSibling = element.nextElementSibling;
                    if (!nextElementSibling) {
                        return;
                    }

                    nextElementSibling.classList.remove('slideIn');
                    nextElementSibling.classList.add('slideOut');
                    nextElementSibling.style.display = 'none';
                }, 250);
            };
        });

    }
};

window.DntButtonSpinner = {
    enable: () => {
        const dntButton = 'data-dnt-button-spinner';
        document.querySelectorAll(`[${dntButton}]`).forEach((element, key, parent) => {
            element.onclick = (event) => {
                const buttonElement = element;
                buttonElement.style.display = 'none';

                const divId = buttonElement.getAttribute(dntButton);
                if (divId) {
                    document.getElementById(divId)?.classList.remove('d-none');
                }
            };
        });
    }
};

window.DntButtonClose = {
    enable: () => {
        const dntButton = 'data-dnt-btn-close';
        document.querySelectorAll(`[${dntButton}]`).forEach((element, key, parent) => {
            element.onclick = (event) => {
                const divId = element.getAttribute(dntButton);
                if (divId) {
                    const parentDiv = document.getElementById(divId);
                    if (parentDiv) {
                        parentDiv.style.display = 'none';
                    }
                }

                document.querySelectorAll('.modal-backdrop').forEach((element, key, parent) => {
                    element.classList.remove('modal-backdrop', 'fade', 'show');
                });

                document.querySelectorAll('.modal').forEach((element, key, parent) => {
                    element.classList.remove('modal', 'fade', 'show', 'd-block');
                });
            };
        });
    }
};


window.DntBootstrapDarkMode = {
    enable: () => {
        const lightSwitch = document.getElementById("lightSwitch");
        const lightSwitchIcon = document.getElementById("lightSwitch-icon");
        if (!lightSwitch || !lightSwitchIcon) {
            return;
        }

        const setTheme = isDark => {
            const mode = isDark ? "dark" : "light";
            if (!lightSwitch.checked) {
                lightSwitch.checked = !!isDark;
            }
            localStorage.setItem("lightSwitch", mode);
            document.documentElement.setAttribute("data-bs-theme", mode);
            lightSwitchIcon.setAttribute("class", isDark ? "bi-moon me-1" : "bi-sun me-1");
        };

        const onToggleMode = () => setTheme(lightSwitch.checked);

        const isSystemDefaultThemeDark = () =>
            window.matchMedia("(prefers-color-scheme: dark)").matches;

        const setup = () => {
            let settings = localStorage.getItem("lightSwitch");
            if (!settings) {
                settings = isSystemDefaultThemeDark() ? "dark" : "light";
            }

            lightSwitch.checked = settings === "dark";
            lightSwitch.addEventListener("change", onToggleMode);
            onToggleMode();
        };

        setup();
    }
};

window.DntBackToTop = {
    enable: () => {
        document.querySelectorAll('[id^="back-to-top"]').forEach(element => {
            element.onclick = () => {
                window.scrollTo({top: 0, behavior: "smooth"});
            };
        });
    }
};

window.DntAddIconsToExternalLinks = {
    enable: () => {

        if (!window.navigator.onLine) {
            return;
        }

        const mySite = window.location.host;
        const googleFavIco = "https://www.google.com/s2/favicons?domain=";
        document.querySelectorAll("a").forEach(link => {
            if (!link || !link.href) {
                return;
            }

            if (document.querySelector('[contenteditable]')?.contains(link)) {
                return;
            }

            if (link.title && link.title.toLowerCase().startsWith("share")) {
                return;
            }

            if (link.classList && link.classList.contains('navbar-brand')) {
                return;
            }

            if (link.itemprop && link.itemprop.toLowerCase().startsWith("social")) {
                return;
            }

            if (link && link.href &&
                link.href.match("^https?://") &&
                !link.href.match(mySite) &&
                link.getAttribute("itemprop") !== "social") {
                const domain = link.href.replace(/<\S[^><]*>/g, "").split('/')[2];
                link.style.background = `url(${googleFavIco}${domain}) center right no-repeat`;
                link.style.paddingRight = "20px";
                link.style.backgroundSize = "16px 16px";
                link.target = "_blank";
            }
        });
    }
};

window.DntHtmlEditor = {
    getUniqueId: outerDivElement => outerDivElement.getAttribute('data-dnt-html-editor-id'),
    setEditorElementHeight: (editorElement, toolbar) => {
        toolbar.classList.add('sticky-top', 'bg-light-subtle');
        const header = document.getElementById("header");
        if (header) {
            toolbar.style.top = `${header.clientHeight}px`;
            toolbar.style.zIndex = "1010";
        }

        editorElement.style.minHeight = editorElement.getAttribute('data-dnt-html-editor-height');
        editorElement.style.height = "auto";
    },
    displayFullyLoadedEditor: outerDivElement => {
        outerDivElement.classList.remove('d-none');
    },
    getEditorOptions: editorElement => {
        const isReadOnly = editorElement.getAttribute('data-dnt-html-editor-is-read-only') === 'true';
        const placeholder = editorElement.getAttribute('data-dnt-html-editor-placeholder');
        const theme = editorElement.getAttribute('data-dnt-html-editor-theme');
        const insertImageUrlLabel = editorElement.getAttribute('data-dnt-html-editor-insert-image-url-label');
        const uploadFileApiPath = editorElement.getAttribute('data-dnt-html-editor-upload-file-api-path');
        const uploadImageFileApiPath = editorElement.getAttribute('data-dnt-html-editor-upload-image-file-api-path');
        const uploadOnlyImageFileErrorMessage = editorElement.getAttribute('data-dnt-html-editor-upload-only-image-error-message');
        const additionalJsonDataDuringImageFileUpload = editorElement.getAttribute('data-dnt-html-editor-additional-json-data-during-image-file-upload');
        const additionalJsonDataDuringFileUpload = editorElement.getAttribute('data-dnt-html-editor-additional-json-data-during-file-upload');
        const acceptedUploadImageFormats = editorElement.getAttribute('data-dnt-html-editor-accepted-upload-image-formats');
        const acceptedUploadFileFormats = editorElement.getAttribute('data-dnt-html-editor-accepted-upload-file-formats');
        const maximumUploadImageSizeInBytes = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-image-size-in-bytes');
        const maximumUploadFileSizeInBytes = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-file-size-in-bytes');
        const maximumUploadImageSizeErrorMessage = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-image-size-error-message');
        const maximumUploadFileSizeErrorMessage = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-file-size-error-message');
        return {
            isReadOnly,
            placeholder,
            theme,
            insertImageUrlLabel,
            uploadFileApiPath,
            uploadImageFileApiPath,
            uploadOnlyImageFileErrorMessage,
            additionalJsonDataDuringImageFileUpload,
            additionalJsonDataDuringFileUpload,
            acceptedUploadImageFormats,
            acceptedUploadFileFormats,
            maximumUploadImageSizeInBytes,
            maximumUploadFileSizeInBytes,
            maximumUploadImageSizeErrorMessage,
            maximumUploadFileSizeErrorMessage
        };
    },
    hideTextArea: textAreaElement => {
        textAreaElement.style.display = 'none';
    },
    synchronizeQuillAndTextArea: (quill, textAreaElement) => {
        quill.on('editor-change', (eventName, ...args) => {
            textAreaElement.value = quill.getSemanticHTML();
        });
    },
    handleInsertImageUrl: (quill, insertImageUrlLabel) => {
        const url = prompt(insertImageUrlLabel);
        if (url) {
            const range = quill.getSelection();
            quill.insertEmbed(range.index, 'image', url, Quill.sources.USER);
        }
    },
    showProgress: (uniqueId, percent) => {
        const progressDiv = document.getElementById(`progress-${uniqueId}`);
        if (!progressDiv) {
            return;
        }

        const progressPercentDiv = document.getElementById(`progress-div-${uniqueId}`);
        if (!progressPercentDiv) {
            return;
        }

        progressDiv.classList.remove('d-none');
        progressPercentDiv.innerHTML = `${percent} %`;
        progressPercentDiv.style.width = `${percent}%`;
    },
    hideProgress: uniqueId => {
        const progressDiv = document.getElementById(`progress-${uniqueId}`);
        if (!progressDiv) {
            return;
        }

        progressDiv.classList.add('d-none');
    },
    showErrorMessage: (uniqueId, message, direction) => {
        const errorDiv = document.getElementById(`alert-${uniqueId}`);
        if (!errorDiv) {
            return;
        }

        if (direction) {
            errorDiv.setAttribute('dir', direction);
        }
        errorDiv.classList.remove('d-none');
        errorDiv.innerHTML = message;
    },
    hideErrorMessage: uniqueId => {
        const errorDiv = document.getElementById(`alert-${uniqueId}`);
        if (!errorDiv) {
            return;
        }

        errorDiv.classList.add('d-none');
    },
    humanFileSize: size => {
        const i = size === 0 ? 0 : Math.floor(Math.log(size) / Math.log(1024));
        return (size / Math.pow(1024, i)).toFixed(2) * 1 + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][i];
    },
    isNumeric: number => (typeof (number) === 'number' || typeof (number) === "string" && number.trim() !== '') && !isNaN(number),
    uploadFile: (uniqueId, quill, accept, isImage, apiUrl, uploadOnlyImageFileErrorMessage, additionalJsonData, maximumFileSizeInBytes, maximumUploadFileSizeErrorMessage) => {

        let dntHtmlEditor = DntHtmlEditor;
        dntHtmlEditor.hideErrorMessage(uniqueId);
        dntHtmlEditor.hideProgress(uniqueId);

        const fileInput = document.createElement('input');
        fileInput.setAttribute('type', 'file');
        fileInput.setAttribute('accept', accept);
        fileInput.click();
        fileInput.onchange = () => {
            if (fileInput.files == null || fileInput.files[0] == null) {
                return;
            }

            const firstFile = fileInput.files[0];
            const hasImageType = /^image\//.test(firstFile.type);
            if (isImage && !hasImageType) {
                dntHtmlEditor.showErrorMessage(uniqueId, uploadOnlyImageFileErrorMessage);
                return;
            }

            if (dntHtmlEditor.isNumeric(maximumFileSizeInBytes)) {
                if (firstFile.size >= maximumFileSizeInBytes) {
                    const maxSize = dntHtmlEditor.humanFileSize(maximumFileSizeInBytes);
                    dntHtmlEditor.showErrorMessage(uniqueId, `${maximumUploadFileSizeErrorMessage} ${maxSize}`);
                    return;
                }
            }

            const xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", evt => {
                if (evt.lengthComputable) {
                    const uploadPercent = Math.round((evt.loaded / evt.total) * 100);
                    dntHtmlEditor.showProgress(uniqueId, uploadPercent);
                }
            }, false);
            xhr.open('POST', apiUrl, true);
            xhr.onload = () => {
                if (xhr.status === 200) {
                    const data = JSON.parse(xhr.responseText);
                    if (data.error) {
                        dntHtmlEditor.showErrorMessage(uniqueId, data.error);
                    }
                    const url = data.url;
                    const range = quill.getSelection();
                    if (isImage || hasImageType) {
                        quill.insertEmbed(range.index, 'image', url, Quill.sources.USER);
                    } else {
                        const fileName = data.fileName;
                        quill.insertText(range.index, fileName, 'link', url)
                    }
                } else {
                    dntHtmlEditor.showErrorMessage(uniqueId, `${xhr.statusText}: ${xhr.responseText}`, 'ltr');
                }
                dntHtmlEditor.hideProgress(uniqueId);
            };

            const formData = new FormData();
            formData.append('file', firstFile);

            if (additionalJsonData && additionalJsonData.length !== 0) {
                let jsonObject;
                try {
                    jsonObject = JSON.parse(additionalJsonData);
                } catch (e) {
                    dntHtmlEditor.showErrorMessage(uniqueId, e.message, 'ltr');
                    throw e;
                }
                Object.entries(jsonObject).forEach((entry) => {
                    const [key, value] = entry;
                    formData.append(key, value);
                });
            }

            xhr.send(formData);
        };
    },
    setDirection: (quill, direction) => {
        const initialContent = quill.getSemanticHTML();
        const initialText = new DOMParser().parseFromString(initialContent, 'text/html').body.textContent || "";
        if (initialText === "") {
            const isRtl = direction && direction === 'rtl';
            quill.format('direction', isRtl ? 'rtl' : 'ltr');
            quill.format('align', isRtl ? 'right' : 'left');
        }
    },
    enable: () => {
        const dntHtmlEditorIdentifier = 'data-dnt-html-editor';
        document.querySelectorAll(`[${dntHtmlEditorIdentifier}]`)
            .forEach((outerDivElement, key, parent) => {
                let dntHtmlEditor = DntHtmlEditor;
                const uniqueId = dntHtmlEditor.getUniqueId(outerDivElement);

                const toolbarId = `quill-toolbar-container-${uniqueId}`;
                const toolbar = document.getElementById(toolbarId);
                if (!toolbar) {
                    return;
                }
                const direction = toolbar.dir;

                const editorElement = document.getElementById(`quill-editor-${uniqueId}`);
                if (!editorElement) {
                    return;
                }

                const textAreaElement = outerDivElement.querySelector("textarea");
                if (!textAreaElement) {
                    return;
                }
                dntHtmlEditor.hideTextArea(textAreaElement);

                const {
                    isReadOnly,
                    placeholder,
                    theme,
                    insertImageUrlLabel,
                    uploadFileApiPath,
                    uploadImageFileApiPath,
                    uploadOnlyImageFileErrorMessage,
                    additionalJsonDataDuringImageFileUpload,
                    additionalJsonDataDuringFileUpload,
                    acceptedUploadImageFormats,
                    acceptedUploadFileFormats,
                    maximumUploadImageSizeInBytes,
                    maximumUploadFileSizeInBytes,
                    maximumUploadImageSizeErrorMessage,
                    maximumUploadFileSizeErrorMessage
                } = dntHtmlEditor.getEditorOptions(editorElement);

                dntHtmlEditor.setEditorElementHeight(editorElement, toolbar);
                const quill = new Quill(editorElement, {
                    debug: 'warn',
                    readOnly: isReadOnly,
                    modules: {
                        syntax: true,
                        toolbar: {
                            container: `#${toolbarId}`,
                            handlers: {
                                uploadImageFile: value => dntHtmlEditor.uploadFile(uniqueId,
                                    quill,
                                    acceptedUploadImageFormats,
                                    true,
                                    uploadImageFileApiPath,
                                    uploadOnlyImageFileErrorMessage,
                                    additionalJsonDataDuringImageFileUpload,
                                    maximumUploadImageSizeInBytes,
                                    maximumUploadImageSizeErrorMessage),
                                insertImageUrl: value => dntHtmlEditor.handleInsertImageUrl(quill, insertImageUrlLabel),
                                uploadFile: value => dntHtmlEditor.uploadFile(uniqueId,
                                    quill,
                                    acceptedUploadFileFormats,
                                    false,
                                    uploadFileApiPath,
                                    uploadOnlyImageFileErrorMessage,
                                    additionalJsonDataDuringFileUpload,
                                    maximumUploadFileSizeInBytes,
                                    maximumUploadFileSizeErrorMessage)
                            }
                        }
                    },
                    placeholder: placeholder,
                    theme: theme
                });
                dntHtmlEditor.setDirection(quill, direction);
                dntHtmlEditor.displayFullyLoadedEditor(outerDivElement);
                dntHtmlEditor.synchronizeQuillAndTextArea(quill, textAreaElement);
            });
    }
};

window.DntSyntaxHighlighter = {
    enable: () => {
        document.querySelectorAll('pre').forEach((element) => {
            let language = element.getAttribute("language") || element.getAttribute("data-language");
            if (!language) {
                return;
            }

            let preHtml = element.innerHTML.trim();
            if (!preHtml.toLowerCase().startsWith("<code")) {
                language = language.toLowerCase();
                switch (language) {
                    case "cs":
                    case "csharp":
                        language = "language-csharp";
                        break;
                    case "vb":
                    case "vbnet":
                        language = "language-vbnet";
                        break;
                    case "jscript":
                    case "javascript":
                        language = "language-javascript";
                        break;
                    case "sql":
                        language = "language-sql";
                        break;
                    case "xml":
                        language = "language-xml";
                        break;
                    case "css":
                        language = "language-css";
                        break;
                    case "java":
                        language = "language-java";
                        break;
                    case "delphi":
                    case "pas":
                        language = "language-pas";
                        break;
                    case "fsharp":
                        language = "language-fsharp";
                        break;
                    case "typescript":
                        language = "language-typescript";
                        break;
                    case "rust":
                        language = "language-rust";
                        break;
                    case "powershell":
                        language = "language-powershell";
                        break;
                    case "bash":
                        language = "language-bash";
                        break;
                    case "php":
                        language = "language-php";
                        break;
                    case "git":
                        language = "language-git";
                        break;
                    default:
                        language = "language-csharp";
                        break;
                }

                // Blogger mode flag
                const br = /<br\s*\/?>/gi;
                preHtml = preHtml.replace(br, '\n');
                preHtml = `<div align='left' style='direction:ltr;text-align:left;' dir='ltr'><pre class='line-numbers'><code class='${language}'>${preHtml}</code></pre></div>`;
                element.innerHTML = preHtml;
            }
        });

        hljs.highlightAll();
    }
};

window.DntHljsCopyToClipboardPlugin = {
    enable: () => {
        hljs.addPlugin({
            "after:highlightElement": ({el, text}) => {
                const wrapper = el.parentElement;
                if (!wrapper) {
                    return;
                }

                const title = "كپى كدها در حافظه";
                if (wrapper.innerHTML.includes(title)) {
                    return;
                }

                const copyButton = document.createElement("button");
                copyButton.classList.add("btn", "btn-sm", "btn-dark");
                copyButton.setAttribute("title", title);

                copyButton.onclick = () => {
                    navigator.clipboard.writeText(text);

                    copyButton.classList.remove("btn-dark");
                    copyButton.classList.add("btn-success");
                    copyButton.innerHTML = "<i class='bi bi-clipboard-check'></i>";

                    setTimeout(() => {
                        copyButton.classList.remove("btn-success");
                        copyButton.classList.add("btn-dark");
                        copyButton.innerHTML = "<i class='bi bi-clipboard'></i>";
                    }, 500);
                };

                copyButton.innerHTML = "<i class='bi bi-clipboard'></i>";

                const copyButtonDiv = document.createElement("div");
                copyButtonDiv.classList.add("position-absolute", "d-block");
                copyButtonDiv.style.right = "0";
                copyButtonDiv.append(copyButton);

                wrapper.classList.add("position-relative");
                wrapper.prepend(copyButtonDiv);
            },
        });
    }
};

window.DntReportErrors = {
    postError: errorMessage => {
        if (!errorMessage || errorMessage.toLowerCase().includes("s2/favicons")) {
            return;
        }
        fetch("/api/JavaScriptErrorsReport/Log", {
            method: "POST",
            body: JSON.stringify(errorMessage),
            headers: {
                'Accept': 'application/json; charset=utf-8',
                'Content-Type': 'application/json; charset=utf-8',
                'Pragma': 'no-cache'
            }
        });
    }
};

window.onerror = (message, url, lineNo, columnNo, error) => {
    DntReportErrors.postError(`JavaScript error: ${message} on line ${lineNo} and column ${columnNo} for ${url} \n ${error?.stack}`);
};

window.DntCatchImageErrors = {
    hideImage: function (imageElement) {
        imageElement.style.visibility = 'hidden';
        imageElement.width = 0;
        imageElement.height = 0;
    },
    handleImageError: imageElement => {
        DntCatchImageErrors.hideImage(imageElement);
        DntReportErrors.postError(`Image error: Image ${imageElement.src} is missing @ ${window.location.href}.`);
    },
    enable: () => {
        document.querySelectorAll('img').forEach((el) => {
            el.addEventListener('error', () => {
                DntCatchImageErrors.handleImageError(el);
            });
        });
    }
};

window.DntAddActiveClassToLists = {
    enable: () => {
        const listItemsLinks = document.querySelectorAll("ul.list-group > a");
        const path = location.href.toLowerCase();
        listItemsLinks.forEach(link => {
            if (link.href && path.startsWith(`${link.href.toLowerCase()}`)) {
                link.classList.add('active');
            } else {
                link.classList.remove('active');
            }
        });
    }
};

window.DntUtilities = {
    enable: () => {
        DntCardAccordion.enable();
        DntHljsCopyToClipboardPlugin.enable();
        DntAddActiveClassToLists.enable();
        DntPersianDatePicker.enable();
        DntRemoteAutoComplete.enable();
        DntFillSearchBox.enable();
        DntStickySidebar.enable();
        DntPreventSubmitOnEnter.enable();
        DntNavLinkMenu.enable();
        DntBootstrapDarkMode.enable();
        DntBackToTop.enable();
        DntButtonSpinner.enable();
        DntButtonClose.enable();
        DntHtmlEditor.enable();
        DntSyntaxHighlighter.enable();
        DntCatchImageErrors.enable();
        DntInputFile.enable();
        DntReactionsPopOver.enable();
        DntDocumentFontSize.enable();
        DntAddIconsToExternalLinks.enable();
        DntApplyBootstarpTable.enable();
        DntPreventDoubleClick.enable();
        DntChangeInputDirectionDependOnLanguage.enable();
        DntInputTag.enable();
        DntStyleValidationMessages.enable();
        DntConfirmWhenLinkClicked.enable();
        DntWindowLocationChangeWatcher.enable();
    }
};

(() => {
    'use strict';

    DntUtilities.enable();

    window.addEventListener('DOMContentLoaded', () => {
        Blazor.addEventListener('enhancedload', () => {
            DntUtilities.enable();
        });
    });
})();
