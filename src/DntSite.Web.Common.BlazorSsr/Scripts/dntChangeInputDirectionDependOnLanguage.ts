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

        static containsRtlText(text: string) {
            return !text ? false : DntChangeInputDirectionDependOnLanguage.containsRtlRegExp().test(text);
        }

        static getDirection(text: string) {
            return DntChangeInputDirectionDependOnLanguage.containsRtlText(text) ? "rtl" : "ltr";
        }

        static enable() {
            document.querySelectorAll<HTMLInputElement>("input[type=text]").forEach(element => {
                element.onkeyup = () => {
                    element.style.direction = DntChangeInputDirectionDependOnLanguage.getDirection(element.value);
                };
            });
        }
    }
}
