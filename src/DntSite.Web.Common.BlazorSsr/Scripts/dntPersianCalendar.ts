namespace DntBlazorSsr {
    export class DntPersianCalendar {
        static arabicNumbers = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
        static farsiDigitsRegex = /[۰۱۲۳۴۵۶۷۸۹]/g;
        static arabicDigitsRegex = /[٠١٢٣٤٥٦٧٨٩]/g;
        static farsiNumbers = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
        static englishDigitsRegex = /[0-9]/g;
        static persianDayNames = ["شنبه", `یکشنبه`, `دوشنبه`, `سه‌شنبه`, `چهارشنبه`, `پنج‌شنبه`, 'جمعه'];
        static persianMonthNames = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'];

        static toEnglishNumbers(inputNumber: string | null): string {
            if (!inputNumber) return '';
            let value = inputNumber.toString().trim();
            if (!value) return '';

            value = value.replace(DntPersianCalendar.farsiDigitsRegex,
                (char) => `${DntPersianCalendar.farsiNumbers.indexOf(char)}`);
            return value.replace(DntPersianCalendar.arabicDigitsRegex,
                (char) => `${DntPersianCalendar.arabicNumbers.indexOf(char)}`);
        }

        static toPersianNumbers(inputNumber: string | null): string {
            if (!inputNumber) return '';
            let value = inputNumber.toString().trim();
            if (!value) return '';

            return value.replace(DntPersianCalendar.englishDigitsRegex,
                (char) => `${DntPersianCalendar.farsiNumbers[Number(char)]}`);
        }

        static toInt(text: string): number {
            return parseInt(text, 10);
        }

        static getTodayPersianStr() {
            return DntPersianCalendar.toEnglishNumbers(new Intl.DateTimeFormat("fa", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit"
            }).format(Date.now()));
        }

        static getPersianDateParts(persianDateStr: string): number[] {
            if (!persianDateStr) {
                persianDateStr = DntPersianCalendar.getTodayPersianStr();
            }

            let dateParts = persianDateStr.split(/[\/\-.]/, 3);
            if (dateParts.length !== 3) {
                return DntPersianCalendar.getPersianDateParts(DntPersianCalendar.getTodayPersianStr());
            }
            return [DntPersianCalendar.toInt(dateParts[0]), DntPersianCalendar.toInt(dateParts[1]), DntPersianCalendar.toInt(dateParts[2])];
        }

        static persianDatePartsToStr(parts: number[]): string {
            return !parts ? "" : `${parts[0]}/${String(parts[1]).padStart(2, '0')}/${String(parts[2]).padStart(2, '0')}`;
        }

        static getNextYear(date: string): string {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return DntPersianCalendar.persianDatePartsToStr([pYear + 1, pMonth, pDay]);
        }

        static getPreviousYear(date: string): string {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return DntPersianCalendar.persianDatePartsToStr([pYear - 1, pMonth, pDay]);
        }

        static getNextMonth(date: string): string {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return pMonth < 12 ?
                DntPersianCalendar.persianDatePartsToStr([pYear, pMonth + 1, pDay]) :
                DntPersianCalendar.persianDatePartsToStr([pYear + 1, 1, pDay]);
        }

        static getPreviousMonth(date: string): string {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return pMonth > 1 ?
                DntPersianCalendar.persianDatePartsToStr([pYear, pMonth - 1, pDay]) :
                DntPersianCalendar.persianDatePartsToStr([pYear - 1, 12, pDay]);
        }

        static isPersianLeapYear(pYear: number): boolean {
            // Persian leap year follows a 33-year cycle pattern
            // Using the algorithm: year % 33, if remainder is one of [1, 5, 9, 13, 17, 21, 25, 29], it's a leap year
            const remainder = pYear % 33;
            const leapYears = [1, 5, 9, 13, 17, 21, 25, 29];
            return leapYears.includes(remainder);
        }

        static mod(a: number, b: number): number {
            return Math.abs(a - (b * Math.floor(a / b)));
        }

        static getWeekDay(date: string): number {
            return DntPersianCalendar.mod(DntPersianCalendar.getDiffDays('1392/03/25', date), 7);
        }

        static getDays(date: string): number {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return pMonth < 8 ? (pMonth - 1) * 31 + pDay : 6 * 31 + (pMonth - 7) * 30 + pDay;
        }

        static getMonthDays(date: string): number {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            if (pMonth < 7)
                return 31;
            if (pMonth < 12)
                return 30;
            return DntPersianCalendar.isPersianLeapYear(pYear) ? 30 : 29;
        }

        static getDiffDays(date1: string, date2: string): number {
            let diffDays = DntPersianCalendar.getDays(date2) - DntPersianCalendar.getDays(date1);
            const [pYear1, pMonth1, pDay1] = DntPersianCalendar.getPersianDateParts(date1);
            const [pYear2, pMonth2, pDay2] = DntPersianCalendar.getPersianDateParts(date2);
            let y1 = (pYear1 < pYear2) ? pYear1 : pYear2;
            let y2 = (pYear1 < pYear2) ? pYear2 : pYear1;
            for (let y = y1; y < y2; y++)
                if (DntPersianCalendar.isPersianLeapYear(y))
                    diffDays += (pYear1 < pYear2) ? 366 : -366;
                else
                    diffDays += (pYear1 < pYear2) ? 365 : -365;
            return diffDays;
        }

        static changeDay(date: string, day: number): string {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return DntPersianCalendar.persianDatePartsToStr([pYear, pMonth, day]);
        }
    }
}
