namespace DntBlazorSsr {
    export class DntChangeInputDirectionDependOnLanguage {
        static containsRtlRegExp() {
            const faAlphabet = "ابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی";
            const faNumber = "۰۱۲۳۴۵۶۷۸۹";
            const faShortVowels = "َُِ";
            const faOthers = "‌آاً";
            const faMixedWithArabic = "ًٌٍَُِّْٰٔءک‌ةۀأإیـئؤ،";
            const faText = faAlphabet + faNumber + faShortVowels + faOthers;
            const faComplexText = faText + faMixedWithArabic;
            return new RegExp(`[${faComplexText}]`);
        }

        static containsRtlText(text: string | undefined) {
            return !text ? false : DntChangeInputDirectionDependOnLanguage.containsRtlRegExp().test(text);
        }

        static getDirection(text: string | undefined) {
            return DntChangeInputDirectionDependOnLanguage.containsRtlText(text) ? "rtl" : "ltr";
        }

        static setDirection(element: HTMLInputElement) {
            if (element.value) {
                element.style.direction = DntChangeInputDirectionDependOnLanguage.getDirection(element.value);
            }
        }

        static enable() {
            document.querySelectorAll<HTMLInputElement>("input[type=text],input[type=search]").forEach(element => {
                DntChangeInputDirectionDependOnLanguage.setDirection(element);

                element.onkeyup = () => {
                    DntChangeInputDirectionDependOnLanguage.setDirection(element);
                };
            });
        }
    }
}
