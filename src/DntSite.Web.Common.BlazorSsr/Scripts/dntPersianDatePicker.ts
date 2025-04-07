namespace DntBlazorSsr {
    export class DntPersianDatePicker {
        static showDatePicker(inputElement: HTMLInputElement): void {

            let _datePicker: HTMLDivElement | null = null;
            let _textBox: HTMLInputElement | null = null;
            let _datePickerStyle: CSSStyleDeclaration | null = null;
            let _isClickInsideDatePickerDiv = false;

            const init = () => {
                _datePicker = createElement("div", document.body) as HTMLDivElement;
                _datePickerStyle = _datePicker.style;
                _datePickerStyle.position = "absolute";
                _datePickerStyle.zIndex = "10000000";
                _datePicker.onmousedown = () => {
                    _isClickInsideDatePickerDiv = true;
                };
                _datePicker.onclick = () => {
                    _textBox?.focus();
                };
            };

            const hideCalendar = () => {
                if (!_datePickerStyle) {
                    return;
                }
                _datePickerStyle.visibility = "hidden";
            };

            const showCalendar = (textBox: HTMLInputElement) => {
                if (!_datePicker) {
                    init();
                }
                _textBox = textBox;
                _textBox.value = DntPersianCalendar.toEnglishNumbers(_textBox.value);
                if (_datePicker) {
                    _datePicker.tabIndex = 0; // to accept focus
                }
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
                    parent = parent.offsetParent as HTMLInputElement;
                }
                if (_datePickerStyle) {
                    _datePickerStyle.left = left + "px";
                    _datePickerStyle.top = top + _textBox.offsetHeight + "px";
                    _datePickerStyle.visibility = "visible";
                }

                drawCalendar(_textBox.value);
            };

            const setValue = (date: string) => {
                if (!_textBox) {
                    return;
                }
                _textBox.value = date;
                _textBox.focus();
                hideCalendar();
            };

            const createElement = (tag: string, parent: HTMLElement): HTMLElement => {
                const element = document.createElement(tag);
                parent.appendChild(element);
                return element;
            };

            const drawCalendarFooter = (table: HTMLTableElement) => {
                const tr = table.insertRow(8);
                setClassName(tr, "table-secondary");

                let td = createElement("td", tr) as HTMLTableCellElement;
                td.colSpan = 2;

                let button = createElement("button", td);
                button.classList.add('btn', 'btn-success', 'btn-sm');
                setInnerHTML(button, "امروز");
                button.onclick = () => {
                    setValue(DntPersianCalendar.getTodayPersianStr());
                };

                td = createElement("td", tr) as HTMLTableCellElement;
                td.colSpan = 2;
                td.style.textAlign = "center";

                button = createElement("button", td);
                button.classList.add('btn', 'btn-secondary', 'btn-sm');
                setInnerHTML(button, "بستن");
                button.onclick = () => {
                    hideCalendar();
                };

                td = createElement("td", tr) as HTMLTableCellElement;
                td.colSpan = 3;
                td.style.textAlign = "left";

                button = createElement("button", td);
                button.classList.add('btn', 'btn-danger', 'btn-sm');
                setInnerHTML(button, "خالی");
                button.onclick = () => {
                    setValue('');
                };
            }

            const drawCalendarRows = (table: HTMLTableElement, date: string) => {
                const weekDay = DntPersianCalendar.getWeekDay(DntPersianCalendar.changeDay(date, 1));

                for (let row = 0; row < 7; row++) {
                    let tr = table.insertRow(row + 1);

                    if (row === 6)
                        setClassName(tr, "table-danger");
                    else if (DntPersianCalendar.mod(row, 2) !== 1)
                        setClassName(tr, "datePickerRow");

                    const th = createElement("th", tr)
                    setInnerHTML(th, DntPersianCalendar.persianDayNames[row]);
                    setClassName(th, "text-center");

                    for (let col = 0; col < 6; col++) {
                        const cellValue = col * 7 + row - weekDay + 1;

                        let td = createElement("td", tr);

                        if (cellValue > 0 && cellValue <= DntPersianCalendar.getMonthDays(date)) {
                            setInnerHTML(td, `<button class="btn btn-outline-secondary btn-sm">${DntPersianCalendar.toPersianNumbers(cellValue.toString())}</button>`);

                            let cellDate = DntPersianCalendar.changeDay(date, cellValue);
                            let cellClassName = "text-center";

                            if (cellDate === _textBox?.value)
                                cellClassName = "text-center bg-info";
                            else if (cellDate === DntPersianCalendar.getTodayPersianStr())
                                cellClassName = "text-center bg-warning";

                            setClassName(td, cellClassName);

                            td.onclick = () => {
                                setValue(DntPersianCalendar.changeDay(date, cellValue));
                            };
                        }
                    }
                }
            }

            const drawCalendarHeader = (table: HTMLTableElement, date: string) => {
                let tr = table.insertRow(0);
                setClassName(tr, "table-primary");

                let td = createElement("td", tr) as HTMLTableCellElement;
                td.colSpan = 3;

                let button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
                setInnerHTML(button, "❰");
                button.onclick = () => {
                    drawCalendar(DntPersianCalendar.getPreviousMonth(date));
                };

                let span = createElement("strong", td);
                const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
                setInnerHTML(span, DntPersianCalendar.persianMonthNames[pMonth - 1]);
                setClassName(span, "datePickerMonth");

                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
                setInnerHTML(button, "❱");
                button.onclick = () => {
                    drawCalendar(DntPersianCalendar.getNextMonth(date));
                };

                td = createElement("td", tr) as HTMLTableCellElement;
                td.colSpan = 4;
                td.style.textAlign = "left";

                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
                setInnerHTML(button, "❰");
                button.onclick = () => {
                    drawCalendar(DntPersianCalendar.getPreviousYear(date));
                };

                span = createElement("strong", td);
                setInnerHTML(span, DntPersianCalendar.toPersianNumbers(String(pYear)));
                setClassName(span, "datePickerYear");

                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
                setInnerHTML(button, "❱");
                button.onclick = () => {
                    drawCalendar(DntPersianCalendar.getNextYear(date));
                };
            }

            const createCalendarTable = (): HTMLTableElement | null => {
                if (!_datePicker) {
                    return null;
                }
                let table = createElement("table", _datePicker) as HTMLTableElement;
                setClassName(table, "table table-borderless table-striped table-striped-columns shadow-sm rounded-3");
                table.cellSpacing = '0';
                return table;
            }

            const drawCalendar = (date: string) => {
                if (!_textBox) {
                    return;
                }
                _textBox.focus();

                if (!_datePicker) {
                    return;
                }
                setInnerHTML(_datePicker, "");

                const table = createCalendarTable();
                if (!table) {
                    return;
                }
                drawCalendarHeader(table, date);
                drawCalendarRows(table, date);
                drawCalendarFooter(table);
            };

            const setInnerHTML = (element: HTMLElement, html: string) => {
                element.innerHTML = html;
            };

            const setClassName = (element: HTMLElement, className: string) => {
                element.className = className;
            };

            showCalendar(inputElement);
        }

        static enable(): void {
            document.querySelectorAll<HTMLInputElement>("[data-dnt-date-picker]").forEach(element => {
                element.onclick = () => {
                    DntPersianDatePicker.showDatePicker(element);
                };
            });
        }
    }
}
