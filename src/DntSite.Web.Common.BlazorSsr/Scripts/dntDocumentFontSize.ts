namespace DntBlazorSsr {
    export class DntDocumentFontSize {
        static enable(): void {
            const mainBodies = document.querySelectorAll("div.main");
            if (!mainBodies) {
                return;
            }

            const btnPlus = document.querySelector<HTMLButtonElement>('button#btn_plus');
            if (!btnPlus) {
                return;
            }

            const btnMinus = document.querySelector<HTMLButtonElement>('button#btn_minus');
            if (!btnMinus) {
                return;
            }

            const btnReset = document.querySelector<HTMLButtonElement>('button#btn_reset');
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

            const setMainFontSize = (size: string) => {
                mainBodies.forEach(mainBody => {
                    fontSizes.forEach(item => mainBody.classList.remove(item));
                    mainBody.classList.add(size);
                    localStorage.setItem(cacheKey, size);
                });
            };

            setMainFontSize(fontSize);

            btnPlus.onclick = () => {
                fontSizeIndex++;
                if (fontSizeIndex < fontSizes.length) {
                    setMainFontSize(fontSizes[fontSizeIndex]);
                } else {
                    fontSizeIndex = fontSizes.length - 1;
                }
            };

            btnMinus.onclick = () => {
                fontSizeIndex--;
                if (fontSizeIndex >= 0) {
                    setMainFontSize(fontSizes[fontSizeIndex]);
                } else {
                    fontSizeIndex = 0;
                }
            };

            btnReset.onclick = () => {
                fontSizeIndex = 0;
                setMainFontSize(fontSizes[fontSizeIndex]);
            };
        }
    }
}
