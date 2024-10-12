namespace DntBlazorSsr {
    export class DntPersianDatePicker {
        static showDatePicker(inputElement: HTMLInputElement): void {
            const _faNums = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
            const _arNums = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
            const _faDigitsRegex = /[۰۱۲۳۴۵۶۷۸۹]/g;
            const _arDigitsRegex = /[٠١٢٣٤٥٦٧٨٩]/g;
            const _enDigitsRegex = /[0-9]/g;
            const _dayNames = ["شنبه", `یکشنبه`, `دوشنبه`, `سه‌شنبه`, `چهارشنبه`, `پنج‌شنبه`, 'جمعه'];
            const _monthNames = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'];

            let _datePicker: HTMLDivElement | null = null;
            let _textBox: HTMLInputElement | null = null;
            let _datePickerStyle: CSSStyleDeclaration | null = null;
            let _isClickInsideDatePickerDiv = false;

            const toEnglishNumbers = (inputNumber: string | null): string => {
                if (!inputNumber) return '';
                let value = inputNumber.toString().trim();
                if (!value) return '';

                value = value.replace(_faDigitsRegex, (char) => `${_faNums.indexOf(char)}`);
                return value.replace(_arDigitsRegex, (char) => `${_arNums.indexOf(char)}`);
            };

            const toPersianNumbers = (inputNumber: string | null): string => {
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
                _textBox.value = toEnglishNumbers(_textBox.value);
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

            const toInt = (text: string): number => {
                return parseInt(text, 10);
            };

            const getPersianDateParts = (persianDateStr: string): number[] => {
                if (!persianDateStr) {
                    persianDateStr = todayPersianStr;
                }

                let dateParts = persianDateStr.split(/[\/\-.]/, 3);
                if (dateParts.length !== 3) {
                    return getPersianDateParts(todayPersianStr);
                }
                return [toInt(dateParts[0]), toInt(dateParts[1]), toInt(dateParts[2])];
            };

            const persianDatePartsToStr = (parts: number[]): string => {
                return !parts ? "" : `${parts[0]}/${String(parts[1]).padStart(2, '0')}/${String(parts[2]).padStart(2, '0')}`;
            }

            const nextYear = (date: string): string => {
                const parts = getPersianDateParts(date);
                return persianDatePartsToStr([parts[0] + 1, parts[1], parts[2]]);
            };

            const previousYear = (date: string): string => {
                const parts = getPersianDateParts(date);
                return persianDatePartsToStr([parts[0] - 1, parts[1], parts[2]]);
            };

            const nextMonth = (date: string): string => {
                const parts = getPersianDateParts(date);
                return parts[1] < 12 ?
                    persianDatePartsToStr([parts[0], parts[1] + 1, parts[2]]) :
                    persianDatePartsToStr([parts[0] + 1, 1, parts[2]]);
            };

            const previousMonth = (date: string): string => {
                const parts = getPersianDateParts(date);
                return parts[1] > 1 ?
                    persianDatePartsToStr([parts[0], parts[1] - 1, parts[2]]) :
                    persianDatePartsToStr([parts[0] - 1, 12, parts[2]]);
            };

            const isLeapYear = (year: number): boolean => {
                return (((((year - 474) % 2820) + 512) * 682) % 2816) < 682;
            };

            const mod = (a: number, b: number): number => {
                return Math.abs(a - (b * Math.floor(a / b)));
            };

            const getWeekDay = (date: string): number => {
                return mod(getDiffDays('1392/03/25', date), 7);
            };

            const getDays = (date: string): number => {
                const parts = getPersianDateParts(date);
                return parts[1] < 8 ?
                    (parts[1] - 1) * 31 + parts[2] :
                    6 * 31 + (parts[1] - 7) * 30 + parts[2];
            };

            const getMonthDays = (date: string): number => {
                const parts = getPersianDateParts(date);
                if (parts[1] < 7)
                    return 31;
                if (parts[1] < 12)
                    return 30;
                return isLeapYear(parts[0]) ? 30 : 29;
            };

            const getDiffDays = (date1: string, date2: string): number => {
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
                return diffDays;
            };

            const changeDay = (date: string, day: number): string => {
                const parts = getPersianDateParts(date);
                return persianDatePartsToStr([parts[0], parts[1], day]);
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
                    setValue(todayPersianStr);
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
                            setInnerHTML(td, `<button class="btn btn-outline-secondary btn-sm">${toPersianNumbers(cellValue.toString())}</button>`);

                            let cellDate = changeDay(date, cellValue);
                            let cellClassName = "text-center";

                            if (cellDate === _textBox?.value)
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

            const drawCalendarHeader = (table: HTMLTableElement, date: string) => {
                let tr = table.insertRow(0);
                setClassName(tr, "table-primary");

                let td = createElement("td", tr) as HTMLTableCellElement;
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

                td = createElement("td", tr) as HTMLTableCellElement;
                td.colSpan = 4;
                td.style.textAlign = "left";

                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
                setInnerHTML(button, "❰");
                button.onclick = () => {
                    drawCalendar(previousYear(date));
                };

                span = createElement("strong", td);
                setInnerHTML(span, toPersianNumbers(String(parts[0])));
                setClassName(span, "datePickerYear");

                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
                setInnerHTML(button, "❱");
                button.onclick = () => {
                    drawCalendar(nextYear(date));
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
