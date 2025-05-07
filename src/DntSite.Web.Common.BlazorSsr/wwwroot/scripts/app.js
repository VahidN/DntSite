"use strict";
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntAddActiveClassToLists {
        static enable() {
            const listItemsLinks = document.querySelectorAll("ul.list-group > a");
            const path = location.href.toLowerCase();
            listItemsLinks.forEach(link => {
                if (link.href && path.startsWith(`${link.href.toLowerCase()}`)) {
                    link.classList.add('active');
                }
                else {
                    link.classList.remove('active');
                }
            });
        }
    }
    DntBlazorSsr.DntAddActiveClassToLists = DntAddActiveClassToLists;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntAddIconsToExternalLinks {
        static enable() {
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
    }
    DntBlazorSsr.DntAddIconsToExternalLinks = DntAddIconsToExternalLinks;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntApplyBootstrapTable {
        static enable() {
            DntApplyBootstrapTable.applyBootstrapStyles(document, "rtl");
        }
        static applyBootstrapStyles(htmlElement, direction) {
            htmlElement.querySelectorAll("table:not([class~='table'])").forEach(element => {
                element.classList.add('table', 'table-striped', 'table-hover', 'table-bordered', 'table-condensed');
                element.style.maxWidth = "100%";
                element.style.width = "auto";
                element.style.marginLeft = "auto";
                element.style.marginRight = "auto";
                element.style.direction = direction;
            });
        }
        static centerAlignAllTableCells(editorElement) {
            const tables = editorElement.querySelectorAll('table');
            tables.forEach(table => {
                const cells = table.querySelectorAll('th, td');
                cells.forEach(cell => {
                    cell.style.textAlign = 'center';
                    cell.style.verticalAlign = 'middle';
                });
            });
        }
    }
    DntBlazorSsr.DntApplyBootstrapTable = DntApplyBootstrapTable;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntBackToTop {
        static enable() {
            document.querySelectorAll('[id^="back-to-top"]').forEach(element => {
                element.onclick = () => {
                    window.scrollTo({ top: 0, behavior: "smooth" });
                };
            });
        }
    }
    DntBlazorSsr.DntBackToTop = DntBackToTop;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntBootstrapDarkMode {
        static enable() {
            const lightSwitch = document.getElementById("lightSwitch");
            const lightSwitchIcon = document.getElementById("lightSwitch-icon");
            if (!lightSwitch || !lightSwitchIcon) {
                return;
            }
            const setTheme = (isDark) => {
                const mode = isDark ? "dark" : "light";
                if (!lightSwitch.checked) {
                    lightSwitch.checked = isDark;
                }
                DntBlazorSsr.DntStorageProvider.setItem("lightSwitch", mode);
                document.documentElement.setAttribute("data-bs-theme", mode);
                lightSwitchIcon.setAttribute("class", isDark ? "bi-moon me-1" : "bi-sun me-1");
            };
            const onToggleMode = () => setTheme(lightSwitch.checked);
            const isSystemDefaultThemeDark = () => window.matchMedia("(prefers-color-scheme: dark)").matches;
            const setup = () => {
                let settings = DntBlazorSsr.DntStorageProvider.getItem("lightSwitch");
                if (!settings) {
                    settings = isSystemDefaultThemeDark() ? "dark" : "light";
                }
                lightSwitch.checked = settings === "dark";
                lightSwitch.addEventListener("change", onToggleMode);
                onToggleMode();
            };
            setup();
        }
    }
    DntBlazorSsr.DntBootstrapDarkMode = DntBootstrapDarkMode;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntButtonClose {
        static enable() {
            const dntButton = 'data-dnt-btn-close';
            document.querySelectorAll(`[${dntButton}]`).forEach(element => {
                element.onclick = () => {
                    const divId = element.getAttribute(dntButton);
                    if (divId) {
                        const parentDiv = document.getElementById(divId);
                        if (parentDiv) {
                            parentDiv.style.display = 'none';
                        }
                    }
                    document.querySelectorAll('.modal-backdrop').forEach(element => {
                        element.classList.remove('modal-backdrop', 'fade', 'show');
                    });
                    document.querySelectorAll('.modal').forEach(element => {
                        element.classList.remove('modal', 'fade', 'show', 'd-block');
                    });
                };
            });
        }
    }
    DntBlazorSsr.DntButtonClose = DntButtonClose;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntButtonSpinner {
        static enable() {
            const dntButton = 'data-dnt-button-spinner';
            document.querySelectorAll(`[${dntButton}]`).forEach((element, key, parent) => {
                element.onclick = () => {
                    const buttonElement = element;
                    buttonElement.style.display = 'none';
                    const divId = buttonElement.getAttribute(dntButton);
                    if (divId) {
                        document.getElementById(divId)?.classList.remove('d-none');
                    }
                };
            });
        }
    }
    DntBlazorSsr.DntButtonSpinner = DntButtonSpinner;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntCardAccordion {
        static enable() {
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
                            element.firstElementChild?.classList.remove("bi-chevron-left");
                            element.firstElementChild?.classList.add("bi-chevron-down");
                            element.title = "بستن محتوا";
                        }
                        else {
                            cardChild.classList.add("accordion-collapse", "collapse");
                            element.firstElementChild?.classList.remove("bi-chevron-down");
                            element.firstElementChild?.classList.add("bi-chevron-left");
                            element.title = "گشودن محتوا";
                        }
                    });
                };
            });
        }
    }
    DntBlazorSsr.DntCardAccordion = DntCardAccordion;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntCatchImageErrors {
        static hideImage(imageElement) {
            imageElement.style.visibility = 'hidden';
            imageElement.width = 0;
            imageElement.height = 0;
        }
        static handleImageError(imageElement) {
            DntCatchImageErrors.hideImage(imageElement);
            DntBlazorSsr.DntReportErrors.postError(`Image error: Image ${imageElement.src} is missing @ ${window.location.href}.`);
        }
        static enable() {
            document.querySelectorAll('img').forEach((el) => {
                el.addEventListener('error', () => {
                    DntCatchImageErrors.handleImageError(el);
                });
            });
        }
    }
    DntBlazorSsr.DntCatchImageErrors = DntCatchImageErrors;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntChangeInputDirectionDependOnLanguage {
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
        static containsRtlText(text) {
            return !text ? false : DntChangeInputDirectionDependOnLanguage.containsRtlRegExp().test(text);
        }
        static getDirection(text) {
            return DntChangeInputDirectionDependOnLanguage.containsRtlText(text) ? "rtl" : "ltr";
        }
        static setDirection(element) {
            if (element.value) {
                element.style.direction = DntChangeInputDirectionDependOnLanguage.getDirection(element.value);
            }
        }
        static enable() {
            document.querySelectorAll("input[type=text],input[type=search]").forEach(element => {
                DntChangeInputDirectionDependOnLanguage.setDirection(element);
                element.onkeyup = () => {
                    DntChangeInputDirectionDependOnLanguage.setDirection(element);
                };
            });
        }
    }
    DntBlazorSsr.DntChangeInputDirectionDependOnLanguage = DntChangeInputDirectionDependOnLanguage;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntConfirmWhenLinkClicked {
        static enable() {
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
                    const hideButton = document.getElementById(hideButtonId);
                    if (hideButton) {
                        hideButton.onclick = () => {
                            modalDiv.style.display = 'none';
                            document.body.removeChild(modalDiv);
                        };
                    }
                    const confirmButton = document.getElementById(confirmButtonId);
                    if (confirmButton) {
                        confirmButton.onclick = () => {
                            element.removeAttribute(dataCancelConfirmAttribute);
                            element.click();
                        };
                    }
                };
            });
        }
    }
    DntBlazorSsr.DntConfirmWhenLinkClicked = DntConfirmWhenLinkClicked;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntDocumentFontSize {
        static enable() {
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
            let fontSize = DntBlazorSsr.DntStorageProvider.getItem(cacheKey);
            if (!fontSize) {
                fontSize = fontSizes[fontSizeIndex];
            }
            else {
                fontSizeIndex = fontSizes.indexOf(fontSize);
            }
            const setMainFontSize = (size) => {
                mainBodies.forEach(mainBody => {
                    fontSizes.forEach(item => mainBody.classList.remove(item));
                    mainBody.classList.add(size);
                    DntBlazorSsr.DntStorageProvider.setItem(cacheKey, size);
                });
            };
            setMainFontSize(fontSize);
            btnPlus.onclick = () => {
                fontSizeIndex++;
                if (fontSizeIndex < fontSizes.length) {
                    setMainFontSize(fontSizes[fontSizeIndex]);
                }
                else {
                    fontSizeIndex = fontSizes.length - 1;
                }
            };
            btnMinus.onclick = () => {
                fontSizeIndex--;
                if (fontSizeIndex >= 0) {
                    setMainFontSize(fontSizes[fontSizeIndex]);
                }
                else {
                    fontSizeIndex = 0;
                }
            };
            btnReset.onclick = () => {
                fontSizeIndex = 0;
                setMainFontSize(fontSizes[fontSizeIndex]);
            };
        }
    }
    DntBlazorSsr.DntDocumentFontSize = DntDocumentFontSize;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntFillSearchBox {
        static enable() {
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
    }
    DntBlazorSsr.DntFillSearchBox = DntFillSearchBox;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntHljsCopyToClipboardPlugin {
        static enable() {
            if (typeof hljs === 'undefined') {
                console.error('Please include the `highlightjs/cdn-assets/highlight.min.js` file first!');
                return;
            }
            hljs.addPlugin({
                "after:highlightElement": ({ el, text }) => {
                    const wrapper = el.parentElement;
                    if (!wrapper) {
                        return;
                    }
                    const title = "کپى کدها در حافظه";
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
    }
    DntBlazorSsr.DntHljsCopyToClipboardPlugin = DntHljsCopyToClipboardPlugin;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntHtmlEditor {
        static getUniqueId(outerDivElement) {
            return outerDivElement.getAttribute('data-dnt-html-editor-id');
        }
        static setEditorElementHeight(editorElement, toolbar) {
            toolbar.classList.add('sticky-top', 'bg-light-subtle');
            const header = document.getElementById("header");
            if (header) {
                toolbar.style.top = `${header.clientHeight}px`;
                toolbar.style.zIndex = "1010";
            }
            const minHeight = editorElement.getAttribute('data-dnt-html-editor-height');
            if (minHeight) {
                editorElement.style.minHeight = minHeight;
            }
            editorElement.style.height = "auto";
        }
        static scrollToCursor(editorElement) {
            editorElement.scrollIntoView({
                behavior: "instant",
                block: "center",
                inline: "nearest"
            });
        }
        static setTextDirectionOnPaste(quill, editorElement) {
            editorElement.addEventListener('paste', function (ce) {
                const initialText = DntHtmlEditor.getEditorTextContent(quill);
                if (initialText === "") {
                    const pastedText = ce.clipboardData?.getData('text/plain');
                    const dir = DntBlazorSsr.DntChangeInputDirectionDependOnLanguage.getDirection(pastedText);
                    if (dir === 'ltr') {
                        DntHtmlEditor.setLtrDir(quill);
                    }
                    else {
                        DntHtmlEditor.setRtlDir(quill);
                    }
                }
            }, true);
        }
        static setRtlDir(quill) {
            quill.format('align', 'right', Quill.sources.USER);
            quill.format('direction', 'rtl', Quill.sources.USER);
        }
        static setLtrDir(quill) {
            quill.format('align', 'left', Quill.sources.USER);
            quill.format('direction', 'ltr', Quill.sources.USER);
        }
        static displayFullyLoadedEditor(outerDivElement) {
            outerDivElement.classList.remove('d-none');
        }
        static getEditorOptions(editorElement) {
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
            let maximumUploadImageSizeInBytes = null;
            const maximumUploadImageSize = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-image-size-in-bytes');
            if (maximumUploadImageSize) {
                maximumUploadImageSizeInBytes = parseInt(maximumUploadImageSize, 10);
            }
            let maximumUploadFileSizeInBytes = null;
            const maximumUploadFileSize = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-file-size-in-bytes');
            if (maximumUploadFileSize) {
                maximumUploadFileSizeInBytes = parseInt(maximumUploadFileSize, 10);
            }
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
        }
        static hideTextArea(textAreaElement) {
            textAreaElement.style.display = 'none';
        }
        static cleanWhiteSpaces(text) {
            return text?.replace(/&nbsp;/g, " ");
        }
        static getEditorHtmlContent(quill) {
            return DntHtmlEditor.cleanWhiteSpaces(quill.getSemanticHTML()) ?? "";
        }
        static getEditorTextContent(quill) {
            return new DOMParser().parseFromString(DntHtmlEditor.getEditorHtmlContent(quill), 'text/html').body.textContent?.trim() || "";
        }
        static synchronizeQuillAndTextArea(quill, textAreaElement) {
            quill.on('editor-change', (eventName, ...args) => {
                DntHtmlEditor.addDirectionToParagraphs();
                textAreaElement.value = DntHtmlEditor.getEditorHtmlContent(quill);
            });
        }
        static handleInsertImageUrl(quill, insertImageUrlLabel) {
            if (!insertImageUrlLabel) {
                return;
            }
            const url = prompt(insertImageUrlLabel);
            if (url) {
                const range = quill.getSelection();
                quill.insertEmbed(range.index, 'image', url, Quill.sources.USER);
            }
        }
        static showProgress(uniqueId, percent) {
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
        }
        static hideProgress(uniqueId) {
            const progressDiv = document.getElementById(`progress-${uniqueId}`);
            if (!progressDiv) {
                return;
            }
            progressDiv.classList.add('d-none');
        }
        static showErrorMessage(uniqueId, message, direction) {
            const errorDiv = document.getElementById(`alert-${uniqueId}`);
            if (!errorDiv) {
                return;
            }
            if (direction) {
                errorDiv.setAttribute('dir', direction);
            }
            errorDiv.classList.remove('d-none');
            if (message) {
                errorDiv.innerHTML = message;
            }
        }
        static hideErrorMessage(uniqueId) {
            const errorDiv = document.getElementById(`alert-${uniqueId}`);
            if (!errorDiv) {
                return;
            }
            errorDiv.classList.add('d-none');
        }
        static humanFileSize(size) {
            const i = size === 0 ? 0 : Math.floor(Math.log(size) / Math.log(1024));
            return (size / Math.pow(1024, i)).toFixed(2) + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][i];
        }
        static addMoreAlignAndDirections() {
            Quill.imports['formats/align'].whitelist.push('left');
            Quill.imports['formats/direction'].whitelist.push('ltr');
        }
        static addDirectionToParagraphs() {
            document.querySelectorAll('.ql-editor > .ql-direction-ltr').forEach(element => {
                element.style.textAlign = 'left';
                element.dir = 'ltr';
            });
            document.querySelectorAll('.ql-editor > .ql-direction-rtl').forEach(element => {
                element.style.textAlign = 'right';
                element.dir = 'rtl';
            });
        }
        static cleanAllStyles(editorElement) {
            DntHtmlEditor.removeAllStyles(editorElement);
            DntBlazorSsr.DntApplyBootstrapTable.applyBootstrapStyles(editorElement, "rtl");
            DntBlazorSsr.DntApplyBootstrapTable.centerAlignAllTableCells(editorElement);
            DntHtmlEditor.normalizeAllHeaders(editorElement);
        }
        static removeAllStyles(editorElement) {
            editorElement.querySelectorAll('*').forEach(element => {
                element.removeAttribute('style');
            });
            editorElement.querySelectorAll("li.ql-direction-ltr,p.ql-direction-ltr").forEach(element => {
                element.removeAttribute('class');
                element.removeAttribute('style');
                element.removeAttribute('dir');
            });
            editorElement.querySelectorAll("div.ql-code-block").forEach(element => {
                element.classList.remove('ql-direction-rtl', 'ql-align-right');
                element.removeAttribute('style');
                element.removeAttribute('dir');
            });
            editorElement.querySelectorAll("li").forEach(element => {
                if (element.hasAttribute("data-list")) {
                    element.setAttribute("data-list", "bullet");
                }
                element.classList.remove("ql-indent-1", "ql-indent-2", "ql-indent-3");
            });
        }
        static normalizeAllHeaders(editorElement) {
            editorElement.querySelectorAll('h1, h2, h3, h4, h5, h6').forEach(headerElement => {
                const strong = document.createElement('strong');
                strong.innerHTML = headerElement.innerHTML;
                strong.dir = "rtl";
                strong.style.textAlign = "right";
                strong.classList.add("ql-align-right", "ql-direction-rtl");
                headerElement.replaceWith(strong);
            });
            editorElement.querySelectorAll('p').forEach(element => {
                element.removeAttribute('style');
                element.style.textAlign = "right";
                element.setAttribute('dir', "rtl");
                element.classList.add("ql-align-right", "ql-direction-rtl");
            });
        }
        static convertMonoSpaceSpansToCode() {
            document.querySelectorAll("span.ql-font-monospace").forEach(element => {
                const code = document.createElement('code');
                code.dir = "ltr";
                code.innerHTML = element.innerHTML;
                element.replaceWith(code);
            });
        }
        static makeInlineCode(quill) {
            const range = quill.getSelection();
            if (range) {
                quill.formatText(range.index, range.length, {
                    'code': true,
                    'direction': 'ltr'
                });
                quill.formatText(range.index, range.length, 'dir', 'ltr');
            }
        }
        static handleDirection(quill, value) {
            const { align } = quill.getFormat();
            if (align === 'right') {
                DntHtmlEditor.setLtrDir(quill);
            }
            else if (value === 'rtl') {
                DntHtmlEditor.setRtlDir(quill);
            }
        }
        static uploadFile(uniqueId, quill, accept, isImage, apiUrl, uploadOnlyImageFileErrorMessage, additionalJsonData, maximumFileSizeInBytes, maximumUploadFileSizeErrorMessage) {
            let dntHtmlEditor = DntHtmlEditor;
            dntHtmlEditor.hideErrorMessage(uniqueId);
            dntHtmlEditor.hideProgress(uniqueId);
            const fileInput = document.createElement('input');
            fileInput.setAttribute('type', 'file');
            if (accept) {
                fileInput.setAttribute('accept', accept);
            }
            fileInput.click();
            fileInput.onchange = () => {
                if (fileInput.files == null || fileInput.files[0] == null) {
                    return;
                }
                const firstFile = fileInput.files[0];
                const hasImageType = /^image\//.test(firstFile.type);
                if (isImage && !hasImageType) {
                    dntHtmlEditor.showErrorMessage(uniqueId, uploadOnlyImageFileErrorMessage, undefined);
                    return;
                }
                if (maximumFileSizeInBytes) {
                    if (firstFile.size >= maximumFileSizeInBytes) {
                        const maxSize = dntHtmlEditor.humanFileSize(maximumFileSizeInBytes);
                        dntHtmlEditor.showErrorMessage(uniqueId, `${maximumUploadFileSizeErrorMessage} ${maxSize}`, undefined);
                        return;
                    }
                }
                if (!apiUrl) {
                    return;
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
                            dntHtmlEditor.showErrorMessage(uniqueId, data.error, undefined);
                        }
                        const url = data.url;
                        const range = quill.getSelection();
                        if (isImage || hasImageType) {
                            quill.insertEmbed(range.index, 'image', url, Quill.sources.USER);
                        }
                        else {
                            const fileName = data.fileName;
                            quill.insertText(range.index, fileName, 'link', url);
                        }
                    }
                    else {
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
                    }
                    catch (e) {
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
        }
        static setDirection(quill, direction) {
            const initialText = DntHtmlEditor.getEditorTextContent(quill);
            if (initialText === "") {
                const isRtl = direction && direction === 'rtl';
                quill.format('direction', isRtl ? 'rtl' : 'ltr');
                quill.format('align', isRtl ? 'right' : 'left');
            }
        }
        static addMoreLanguages() {
            const Syntax = Quill.imports["modules/syntax"];
            Syntax.DEFAULTS.languages = [
                { key: 'plain', label: 'Plain' },
                { key: 'bash', label: 'Bash' },
                { key: 'cpp', label: 'C++' },
                { key: 'cs', label: 'C#' },
                { key: 'css', label: 'CSS' },
                { key: 'diff', label: 'Diff' },
                { key: 'xml', label: 'HTML/XML' },
                { key: 'java', label: 'Java' },
                { key: 'javascript', label: 'JavaScript' },
                { key: 'markdown', label: 'Markdown' },
                { key: 'php', label: 'PHP' },
                { key: 'python', label: 'Python' },
                { key: 'ruby', label: 'Ruby' },
                { key: 'sql', label: 'SQL' },
                { key: "typescript", label: "TypeScript" },
                { key: "vbnet", label: "VB.NET" },
                { key: "rust", label: "Rust" },
                { key: 'go', label: "Go" },
                { key: 'swift', label: 'Swift' },
                { key: 'objectivec', label: 'Objective C' },
                { key: 'yaml', label: "Yaml" },
                { key: 'json', label: 'JSON' },
                { key: 'kotlin', label: 'Kotlin' },
                { key: 'perl', label: 'Perl' },
                { key: 'ini', label: 'Ini' },
                { key: 'less', label: 'Less' },
                { key: 'scss', label: 'Scss' },
                { key: 'shell', label: 'Shell' }
            ];
            Syntax.DEFAULTS.languages.sort((a, b) => {
                const nameA = a.label.toUpperCase();
                const nameB = b.label.toUpperCase();
                if (nameA < nameB) {
                    return -1;
                }
                if (nameA > nameB) {
                    return 1;
                }
                return 0;
            });
        }
        static enable() {
            if (typeof Quill === 'undefined') {
                console.error('Please include the `quill/dist/quill.js` file first!');
                return;
            }
            const dntHtmlEditorIdentifier = 'data-dnt-html-editor';
            document.querySelectorAll(`[${dntHtmlEditorIdentifier}]`).forEach(outerDivElement => {
                let dntHtmlEditor = DntHtmlEditor;
                const uniqueId = dntHtmlEditor.getUniqueId(outerDivElement);
                if (!uniqueId) {
                    return;
                }
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
                const { isReadOnly, placeholder, theme, insertImageUrlLabel, uploadFileApiPath, uploadImageFileApiPath, uploadOnlyImageFileErrorMessage, additionalJsonDataDuringImageFileUpload, additionalJsonDataDuringFileUpload, acceptedUploadImageFormats, acceptedUploadFileFormats, maximumUploadImageSizeInBytes, maximumUploadFileSizeInBytes, maximumUploadImageSizeErrorMessage, maximumUploadFileSizeErrorMessage } = dntHtmlEditor.getEditorOptions(editorElement);
                dntHtmlEditor.setEditorElementHeight(editorElement, toolbar);
                DntHtmlEditor.addMoreLanguages();
                DntHtmlEditor.addMoreAlignAndDirections();
                const quill = new Quill(editorElement, {
                    debug: 'warn',
                    readOnly: isReadOnly,
                    modules: {
                        syntax: true,
                        toolbar: {
                            container: `#${toolbarId}`,
                            handlers: {
                                'inline-code': (value) => dntHtmlEditor.makeInlineCode(quill),
                                'clean-styles': (value) => {
                                    dntHtmlEditor.cleanAllStyles(editorElement);
                                    dntHtmlEditor.convertMonoSpaceSpansToCode();
                                },
                                direction: (value) => {
                                    dntHtmlEditor.handleDirection(quill, value);
                                    dntHtmlEditor.addDirectionToParagraphs();
                                },
                                uploadImageFile: (value) => dntHtmlEditor.uploadFile(uniqueId, quill, acceptedUploadImageFormats, true, uploadImageFileApiPath, uploadOnlyImageFileErrorMessage, additionalJsonDataDuringImageFileUpload, maximumUploadImageSizeInBytes, maximumUploadImageSizeErrorMessage),
                                insertImageUrl: (value) => dntHtmlEditor.handleInsertImageUrl(quill, insertImageUrlLabel),
                                uploadFile: (value) => dntHtmlEditor.uploadFile(uniqueId, quill, acceptedUploadFileFormats, false, uploadFileApiPath, uploadOnlyImageFileErrorMessage, additionalJsonDataDuringFileUpload, maximumUploadFileSizeInBytes, maximumUploadFileSizeErrorMessage)
                            }
                        }
                    },
                    placeholder: placeholder,
                    theme: theme
                });
                dntHtmlEditor.setDirection(quill, direction);
                dntHtmlEditor.displayFullyLoadedEditor(outerDivElement);
                dntHtmlEditor.synchronizeQuillAndTextArea(quill, textAreaElement);
                dntHtmlEditor.scrollToCursor(editorElement);
                dntHtmlEditor.setTextDirectionOnPaste(quill, editorElement);
            });
        }
    }
    DntBlazorSsr.DntHtmlEditor = DntHtmlEditor;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntInputFile {
        static enable() {
            document.querySelectorAll('.form-control[type=file].d-none').forEach((fileInput, key, parent) => {
                fileInput.onchange = () => {
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
    }
    DntBlazorSsr.DntInputFile = DntInputFile;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntInputTag {
        static manageRemoveButtons() {
            const dntButton = 'data-dnt-tag-id';
            document.querySelectorAll(`[${dntButton}]`).forEach(element => {
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
                    const enteredTags = [];
                    document.querySelectorAll("input[type=hidden]").forEach(element => {
                        enteredTags.push(element.value);
                    });
                    if (enteredTags.includes(tagValue)) {
                        input.value = '';
                        input.focus();
                        return;
                    }
                    const dir = DntBlazorSsr.DntChangeInputDirectionDependOnLanguage.getDirection(tagValue);
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
        }
        static enable() {
            DntInputTag.manageAddTagOnEnter();
            DntInputTag.manageAddButton();
            DntInputTag.manageRemoveButtons();
        }
    }
    DntBlazorSsr.DntInputTag = DntInputTag;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntNavLinkMenu {
        static enable() {
            document.querySelectorAll('a.nav-link.dropdown-toggle').forEach(element => {
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
    }
    DntBlazorSsr.DntNavLinkMenu = DntNavLinkMenu;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntPersianCalendar {
        static toEnglishNumbers(inputNumber) {
            if (!inputNumber)
                return '';
            let value = inputNumber.toString().trim();
            if (!value)
                return '';
            value = value.replace(DntPersianCalendar.farsiDigitsRegex, (char) => `${DntPersianCalendar.farsiNumbers.indexOf(char)}`);
            return value.replace(DntPersianCalendar.arabicDigitsRegex, (char) => `${DntPersianCalendar.arabicNumbers.indexOf(char)}`);
        }
        static toPersianNumbers(inputNumber) {
            if (!inputNumber)
                return '';
            let value = inputNumber.toString().trim();
            if (!value)
                return '';
            return value.replace(DntPersianCalendar.englishDigitsRegex, (char) => `${DntPersianCalendar.farsiNumbers[Number(char)]}`);
        }
        static toInt(text) {
            return parseInt(text, 10);
        }
        static getTodayPersianStr() {
            return DntPersianCalendar.toEnglishNumbers(new Intl.DateTimeFormat("fa", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit"
            }).format(Date.now()));
        }
        static getPersianDateParts(persianDateStr) {
            if (!persianDateStr) {
                persianDateStr = DntPersianCalendar.getTodayPersianStr();
            }
            let dateParts = persianDateStr.split(/[\/\-.]/, 3);
            if (dateParts.length !== 3) {
                return DntPersianCalendar.getPersianDateParts(DntPersianCalendar.getTodayPersianStr());
            }
            return [DntPersianCalendar.toInt(dateParts[0]), DntPersianCalendar.toInt(dateParts[1]), DntPersianCalendar.toInt(dateParts[2])];
        }
        static persianDatePartsToStr(parts) {
            return !parts ? "" : `${parts[0]}/${String(parts[1]).padStart(2, '0')}/${String(parts[2]).padStart(2, '0')}`;
        }
        static getNextYear(date) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return DntPersianCalendar.persianDatePartsToStr([pYear + 1, pMonth, pDay]);
        }
        static getPreviousYear(date) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return DntPersianCalendar.persianDatePartsToStr([pYear - 1, pMonth, pDay]);
        }
        static getNextMonth(date) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return pMonth < 12 ?
                DntPersianCalendar.persianDatePartsToStr([pYear, pMonth + 1, pDay]) :
                DntPersianCalendar.persianDatePartsToStr([pYear + 1, 1, pDay]);
        }
        static getPreviousMonth(date) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return pMonth > 1 ?
                DntPersianCalendar.persianDatePartsToStr([pYear, pMonth - 1, pDay]) :
                DntPersianCalendar.persianDatePartsToStr([pYear - 1, 12, pDay]);
        }
        static isPersianLeapYear(pYear) {
            const remainder = pYear % 33;
            const leapYears = [1, 5, 9, 13, 17, 21, 25, 29];
            return leapYears.includes(remainder);
        }
        static mod(a, b) {
            return Math.abs(a - (b * Math.floor(a / b)));
        }
        static getWeekDay(date) {
            return DntPersianCalendar.mod(DntPersianCalendar.getDiffDays('1392/03/25', date), 7);
        }
        static getDays(date) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return pMonth < 8 ? (pMonth - 1) * 31 + pDay : 6 * 31 + (pMonth - 7) * 30 + pDay;
        }
        static getMonthDays(date) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            if (pMonth < 7)
                return 31;
            if (pMonth < 12)
                return 30;
            return DntPersianCalendar.isPersianLeapYear(pYear) ? 30 : 29;
        }
        static getDiffDays(date1, date2) {
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
        static changeDay(date, day) {
            const [pYear, pMonth, pDay] = DntPersianCalendar.getPersianDateParts(date);
            return DntPersianCalendar.persianDatePartsToStr([pYear, pMonth, day]);
        }
    }
    DntPersianCalendar.arabicNumbers = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
    DntPersianCalendar.farsiDigitsRegex = /[۰۱۲۳۴۵۶۷۸۹]/g;
    DntPersianCalendar.arabicDigitsRegex = /[٠١٢٣٤٥٦٧٨٩]/g;
    DntPersianCalendar.farsiNumbers = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
    DntPersianCalendar.englishDigitsRegex = /[0-9]/g;
    DntPersianCalendar.persianDayNames = ["شنبه", `یکشنبه`, `دوشنبه`, `سه‌شنبه`, `چهارشنبه`, `پنج‌شنبه`, 'جمعه'];
    DntPersianCalendar.persianMonthNames = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'];
    DntBlazorSsr.DntPersianCalendar = DntPersianCalendar;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntPersianDatePicker {
        static showDatePicker(inputElement) {
            let _datePicker = null;
            let _textBox = null;
            let _datePickerStyle = null;
            let _isClickInsideDatePickerDiv = false;
            const init = () => {
                _datePicker = createElement("div", document.body);
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
            const showCalendar = (textBox) => {
                if (!_datePicker) {
                    init();
                }
                _textBox = textBox;
                _textBox.value = DntBlazorSsr.DntPersianCalendar.toEnglishNumbers(_textBox.value);
                if (_datePicker) {
                    _datePicker.tabIndex = 0;
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
                    parent = parent.offsetParent;
                }
                if (_datePickerStyle) {
                    _datePickerStyle.left = left + "px";
                    _datePickerStyle.top = top + _textBox.offsetHeight + "px";
                    _datePickerStyle.visibility = "visible";
                }
                drawCalendar(_textBox.value);
            };
            const setValue = (date) => {
                if (!_textBox) {
                    return;
                }
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
                let td = createElement("td", tr);
                td.colSpan = 2;
                let button = createElement("button", td);
                button.classList.add('btn', 'btn-success', 'btn-sm');
                setInnerHTML(button, "امروز");
                button.onclick = () => {
                    setValue(DntBlazorSsr.DntPersianCalendar.getTodayPersianStr());
                };
                td = createElement("td", tr);
                td.colSpan = 2;
                td.style.textAlign = "center";
                button = createElement("button", td);
                button.classList.add('btn', 'btn-secondary', 'btn-sm');
                setInnerHTML(button, "بستن");
                button.onclick = () => {
                    hideCalendar();
                };
                td = createElement("td", tr);
                td.colSpan = 3;
                td.style.textAlign = "left";
                button = createElement("button", td);
                button.classList.add('btn', 'btn-danger', 'btn-sm');
                setInnerHTML(button, "خالی");
                button.onclick = () => {
                    setValue('');
                };
            };
            const drawCalendarRows = (table, date) => {
                const weekDay = DntBlazorSsr.DntPersianCalendar.getWeekDay(DntBlazorSsr.DntPersianCalendar.changeDay(date, 1));
                for (let row = 0; row < 7; row++) {
                    let tr = table.insertRow(row + 1);
                    if (row === 6)
                        setClassName(tr, "table-danger");
                    else if (DntBlazorSsr.DntPersianCalendar.mod(row, 2) !== 1)
                        setClassName(tr, "datePickerRow");
                    const th = createElement("th", tr);
                    setInnerHTML(th, DntBlazorSsr.DntPersianCalendar.persianDayNames[row]);
                    setClassName(th, "text-center");
                    for (let col = 0; col < 6; col++) {
                        const cellValue = col * 7 + row - weekDay + 1;
                        let td = createElement("td", tr);
                        if (cellValue > 0 && cellValue <= DntBlazorSsr.DntPersianCalendar.getMonthDays(date)) {
                            setInnerHTML(td, `<button class="btn btn-outline-secondary btn-sm">${DntBlazorSsr.DntPersianCalendar.toPersianNumbers(cellValue.toString())}</button>`);
                            let cellDate = DntBlazorSsr.DntPersianCalendar.changeDay(date, cellValue);
                            let cellClassName = "text-center";
                            if (cellDate === _textBox?.value)
                                cellClassName = "text-center bg-info";
                            else if (cellDate === DntBlazorSsr.DntPersianCalendar.getTodayPersianStr())
                                cellClassName = "text-center bg-warning";
                            setClassName(td, cellClassName);
                            td.onclick = () => {
                                setValue(DntBlazorSsr.DntPersianCalendar.changeDay(date, cellValue));
                            };
                        }
                    }
                }
            };
            const drawCalendarHeader = (table, date) => {
                let tr = table.insertRow(0);
                setClassName(tr, "table-primary");
                let td = createElement("td", tr);
                td.colSpan = 3;
                let button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
                setInnerHTML(button, "❰");
                button.onclick = () => {
                    drawCalendar(DntBlazorSsr.DntPersianCalendar.getPreviousMonth(date));
                };
                let span = createElement("strong", td);
                const [pYear, pMonth, pDay] = DntBlazorSsr.DntPersianCalendar.getPersianDateParts(date);
                setInnerHTML(span, DntBlazorSsr.DntPersianCalendar.persianMonthNames[pMonth - 1]);
                setClassName(span, "datePickerMonth");
                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
                setInnerHTML(button, "❱");
                button.onclick = () => {
                    drawCalendar(DntBlazorSsr.DntPersianCalendar.getNextMonth(date));
                };
                td = createElement("td", tr);
                td.colSpan = 4;
                td.style.textAlign = "left";
                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'me-1');
                setInnerHTML(button, "❰");
                button.onclick = () => {
                    drawCalendar(DntBlazorSsr.DntPersianCalendar.getPreviousYear(date));
                };
                span = createElement("strong", td);
                setInnerHTML(span, DntBlazorSsr.DntPersianCalendar.toPersianNumbers(String(pYear)));
                setClassName(span, "datePickerYear");
                button = createElement("button", td);
                button.classList.add('btn', 'btn-primary', 'btn-sm', 'ms-1');
                setInnerHTML(button, "❱");
                button.onclick = () => {
                    drawCalendar(DntBlazorSsr.DntPersianCalendar.getNextYear(date));
                };
            };
            const createCalendarTable = () => {
                if (!_datePicker) {
                    return null;
                }
                let table = createElement("table", _datePicker);
                setClassName(table, "table table-borderless table-striped table-striped-columns shadow-sm rounded-3");
                table.cellSpacing = '0';
                return table;
            };
            const drawCalendar = (date) => {
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
            const setInnerHTML = (element, html) => {
                element.innerHTML = html;
            };
            const setClassName = (element, className) => {
                element.className = className;
            };
            showCalendar(inputElement);
        }
        static enable() {
            document.querySelectorAll("[data-dnt-date-picker]").forEach(element => {
                element.onclick = () => {
                    DntPersianDatePicker.showDatePicker(element);
                };
            });
        }
    }
    DntBlazorSsr.DntPersianDatePicker = DntPersianDatePicker;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntPreventDoubleClick {
        static enable() {
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
                    }
                    else {
                        if (!element.classList.contains('is-submitting')) {
                            element.classList.add('is-submitting');
                            const newSpanElement = document.createElement("span");
                            newSpanElement.classList.add('ms-2', 'spinner-grow', 'spinner-grow-sm', 'text-light');
                            element.append(newSpanElement);
                        }
                        else {
                            element.type = "button";
                            element.disabled = true;
                        }
                    }
                };
            });
        }
    }
    DntBlazorSsr.DntPreventDoubleClick = DntPreventDoubleClick;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntPreventSubmitOnEnter {
        static enable() {
            document.querySelectorAll("form").forEach(element => {
                element.onkeydown = (event) => {
                    if (event.target?.nodeName === "TEXTAREA") {
                        return;
                    }
                    if (event.key === "Enter") {
                        event.preventDefault();
                    }
                };
            });
        }
    }
    DntBlazorSsr.DntPreventSubmitOnEnter = DntPreventSubmitOnEnter;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntReactionsPopOver {
        static enable() {
            document.querySelectorAll('[data-bs-toggle="reaction"]').forEach(element => {
                element.onclick = () => {
                    const upsDivId = element.getAttribute('data-bs-toggle-up');
                    if (upsDivId) {
                        const upsDiv = document.getElementById(upsDivId);
                        if (upsDiv) {
                            upsDiv.classList.remove('d-none');
                            upsDiv.style.display = "block";
                        }
                    }
                    const downsDivId = element.getAttribute('data-bs-toggle-down');
                    if (downsDivId) {
                        const downsDiv = document.getElementById(downsDivId);
                        if (downsDiv) {
                            downsDiv.classList.remove('d-none');
                            downsDiv.style.display = "block";
                        }
                    }
                    DntBlazorSsr.DntButtonClose.enable();
                };
            });
        }
    }
    DntBlazorSsr.DntReactionsPopOver = DntReactionsPopOver;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntRemoteAutoComplete {
        static canonicalize(url) {
            let div = document.createElement('div');
            div.innerHTML = "<a></a>";
            const child = div.firstChild;
            child.href = url;
            div.innerHTML = div.innerHTML;
            return child.href;
        }
        static insertAfterElement(elem, refElem) {
            return elem ? refElem?.parentNode?.insertBefore(elem, refElem.nextSibling) : undefined;
        }
        static createElement(html) {
            let div = document.createElement('div');
            div.innerHTML = html;
            return div.firstChild;
        }
        static enable() {
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
                element.parentNode?.classList.add('dropdown');
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
                const hideDropdownOnEscKeyPress = (evt) => {
                    evt = evt || window.event;
                    if (evt.key === "Escape" || evt.key === "Esc") {
                        hideDropdown();
                    }
                };
                document.onkeydown = (evt) => {
                    hideDropdownOnEscKeyPress(evt);
                };
                document.onclick = (evt) => {
                    if (!dropdown.contains(evt.target)) {
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
                        const node = DntRemoteAutoComplete.createElement(data[i]);
                        if (node) {
                            items.appendChild(node);
                        }
                    }
                    if (data.length > 0) {
                        showDropdown();
                    }
                    else {
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
                    element.nextSibling?.querySelectorAll('.dropdown-item').forEach(item => {
                        item.onclick = (event) => {
                            logSearchedData();
                            hideDropdown();
                        };
                        item.onmousedown = (event) => {
                            if (event.button === 2) {
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
                    })
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
                    }
                    else if (event.key === 'Enter') {
                        window.location.href = `${redirectUrl}/${encodeURIComponent(element.value)}`;
                    }
                };
            });
        }
    }
    DntBlazorSsr.DntRemoteAutoComplete = DntRemoteAutoComplete;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntReportErrors {
        static postError(errorMessage) {
            if (!errorMessage) {
                return;
            }
            const message = errorMessage.toLowerCase();
            if (DntReportErrors.ignoreMessage(message)) {
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
        static enable() {
            window.onerror = (message, url, lineNo, columnNo, error) => {
                DntReportErrors.postError(`JavaScript error: ${message} on line ${lineNo} and column ${columnNo} for ${url} \n ${error?.stack}`);
            };
        }
        static ignoreMessage(message) {
            return message.includes("s2/favicons") || message.includes("chrome-extension");
        }
    }
    DntBlazorSsr.DntReportErrors = DntReportErrors;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntShowPassword {
        static enable() {
            document.querySelectorAll("input[type=password]").forEach(passwordElement => {
                const wrapper = passwordElement.parentElement;
                if (!wrapper) {
                    return;
                }
                const showPasswordButton = document.createElement("button");
                showPasswordButton.type = "button";
                showPasswordButton.classList.add("btn", "btn-outline-secondary", "btn-sm", "bi", "bi-eye");
                showPasswordButton.setAttribute("title", "نمایش کلمه عبور");
                wrapper.append(showPasswordButton);
                showPasswordButton.onclick = () => {
                    const type = passwordElement.getAttribute('type');
                    if (type === 'password') {
                        passwordElement.setAttribute('type', 'text');
                        showPasswordButton.classList.remove("bi-eye");
                        showPasswordButton.classList.add("bi-eye-slash");
                        showPasswordButton.setAttribute("title", "پوشاندن کلمه عبور");
                    }
                    else {
                        passwordElement.setAttribute('type', 'password');
                        showPasswordButton.classList.remove("bi-eye-slash");
                        showPasswordButton.classList.add("bi-eye");
                        showPasswordButton.setAttribute("title", "نمایش کلمه عبور");
                    }
                };
            });
        }
    }
    DntBlazorSsr.DntShowPassword = DntShowPassword;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntStatusCheck {
        static updateOnlineStatus() {
            document.querySelectorAll('[id^="dnt-online-status-report"]').forEach(statusBarElement => {
                if (navigator.onLine) {
                    statusBarElement.classList.add('d-none');
                    statusBarElement.innerHTML = '';
                }
                else {
                    statusBarElement.classList.remove('d-none');
                    statusBarElement.classList.add('badge', 'text-bg-danger', 'fs-6', 'mt-1', 'mb-2', 'show');
                    statusBarElement.innerHTML = 'مرورگر در این لحظه آنلاین نیست. لطفا از اتصال خود به شبکه مطمئن شوید.';
                }
            });
        }
        static enable() {
            window.addEventListener('online', DntStatusCheck.updateOnlineStatus);
            window.addEventListener('offline', DntStatusCheck.updateOnlineStatus);
            DntStatusCheck.updateOnlineStatus();
        }
    }
    DntBlazorSsr.DntStatusCheck = DntStatusCheck;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntStickySidebar {
        static enable() {
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
    }
    DntBlazorSsr.DntStickySidebar = DntStickySidebar;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntStorageProvider {
        static isSupported() {
            try {
                const testKey = "__DntStorageProvider__";
                localStorage.setItem(testKey, testKey);
                localStorage.removeItem(testKey);
                return true;
            }
            catch (e) {
                return false;
            }
        }
        static clear() {
            if (DntStorageProvider.isSupported()) {
                localStorage.clear();
            }
            else {
                DntStorageProvider.inMemoryStorage = {};
            }
        }
        static getItem(name) {
            if (DntStorageProvider.isSupported()) {
                return localStorage.getItem(name);
            }
            if (DntStorageProvider.inMemoryStorage.hasOwnProperty(name)) {
                return DntStorageProvider.inMemoryStorage[name];
            }
            return null;
        }
        static key(index) {
            if (DntStorageProvider.isSupported()) {
                return localStorage.key(index);
            }
            else {
                return Object.keys(DntStorageProvider.inMemoryStorage)[index] || null;
            }
        }
        static removeItem(name) {
            if (DntStorageProvider.isSupported()) {
                localStorage.removeItem(name);
            }
            else {
                delete DntStorageProvider.inMemoryStorage[name];
            }
        }
        static setItem(name, value) {
            if (DntStorageProvider.isSupported()) {
                localStorage.setItem(name, value);
            }
            else {
                DntStorageProvider.inMemoryStorage[name] = String(value);
            }
        }
        static count() {
            if (DntStorageProvider.isSupported()) {
                return localStorage.length;
            }
            else {
                return Object.keys(DntStorageProvider.inMemoryStorage).length;
            }
        }
    }
    DntStorageProvider.inMemoryStorage = {};
    DntBlazorSsr.DntStorageProvider = DntStorageProvider;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntStyleSiteImages {
        static enable() {
            document.querySelectorAll('img').forEach(el => {
                if (!el.src) {
                    return;
                }
                const src = el.src.toLowerCase();
                if (src.includes("/file/")) {
                    el.classList.add("rounded", "border", "shadow-sm", "border-secondary-subtle");
                    if (!src.includes("avatar")) {
                        el.classList.add("mt-2", "mb-2");
                    }
                }
            });
        }
    }
    DntBlazorSsr.DntStyleSiteImages = DntStyleSiteImages;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntStyleValidationMessages {
        static enable() {
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
    }
    DntBlazorSsr.DntStyleValidationMessages = DntStyleValidationMessages;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntSyntaxHighlighter {
        static enable() {
            if (typeof hljs === 'undefined') {
                console.error('Please include the `highlightjs/cdn-assets/highlight.min.js` file first!');
                return;
            }
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
                        case "delphi":
                        case "pas":
                            language = "language-pas";
                            break;
                        case "plain":
                            language = "language-plaintext";
                            break;
                        case "css":
                        case "xml":
                        case "sql":
                        case "java":
                        case "fsharp":
                        case "typescript":
                        case "rust":
                        case "powershell":
                        case "bash":
                        case "cmd":
                        case "dockerfile":
                        case "kotlin":
                        case "php":
                        case "perl":
                        case "shell":
                        case "git":
                        case "pgsql":
                        case "go":
                        case "swift":
                        case "objectivec":
                        case "yaml":
                        case "json":
                        case "accesslog":
                        case "nginx":
                        case "ini":
                        case "less":
                        case "scss":
                            language = `language-${language}`;
                            break;
                        default:
                            language = "language-csharp";
                            break;
                    }
                    const br = /<br\s*\/?>/gi;
                    preHtml = preHtml.replace(br, '\n');
                    preHtml = `<div align='left' style='direction:ltr;text-align:left;' dir='ltr'><pre class='line-numbers'><code class='${language}'>${preHtml}</code></pre></div>`;
                    element.innerHTML = preHtml;
                }
            });
            hljs.highlightAll();
        }
    }
    DntBlazorSsr.DntSyntaxHighlighter = DntSyntaxHighlighter;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntUtilities {
        static enable() {
            DntBlazorSsr.DntStatusCheck.enable();
            DntBlazorSsr.DntShowPassword.enable();
            DntBlazorSsr.DntReportErrors.enable();
            DntBlazorSsr.DntStyleSiteImages.enable();
            DntBlazorSsr.DntCardAccordion.enable();
            DntBlazorSsr.DntHljsCopyToClipboardPlugin.enable();
            DntBlazorSsr.DntAddActiveClassToLists.enable();
            DntBlazorSsr.DntPersianDatePicker.enable();
            DntBlazorSsr.DntRemoteAutoComplete.enable();
            DntBlazorSsr.DntFillSearchBox.enable();
            DntBlazorSsr.DntStickySidebar.enable();
            DntBlazorSsr.DntPreventSubmitOnEnter.enable();
            DntBlazorSsr.DntNavLinkMenu.enable();
            DntBlazorSsr.DntBootstrapDarkMode.enable();
            DntBlazorSsr.DntBackToTop.enable();
            DntBlazorSsr.DntButtonSpinner.enable();
            DntBlazorSsr.DntButtonClose.enable();
            DntBlazorSsr.DntHtmlEditor.enable();
            DntBlazorSsr.DntSyntaxHighlighter.enable();
            DntBlazorSsr.DntCatchImageErrors.enable();
            DntBlazorSsr.DntInputFile.enable();
            DntBlazorSsr.DntReactionsPopOver.enable();
            DntBlazorSsr.DntDocumentFontSize.enable();
            DntBlazorSsr.DntAddIconsToExternalLinks.enable();
            DntBlazorSsr.DntApplyBootstrapTable.enable();
            DntBlazorSsr.DntPreventDoubleClick.enable();
            DntBlazorSsr.DntChangeInputDirectionDependOnLanguage.enable();
            DntBlazorSsr.DntInputTag.enable();
            DntBlazorSsr.DntStyleValidationMessages.enable();
            DntBlazorSsr.DntConfirmWhenLinkClicked.enable();
            DntBlazorSsr.DntWindowLocationChangeWatcher.enable();
        }
    }
    DntBlazorSsr.DntUtilities = DntUtilities;
})(DntBlazorSsr || (DntBlazorSsr = {}));
var DntBlazorSsr;
(function (DntBlazorSsr) {
    class DntWindowLocationChangeWatcher {
        static getCurrentUrlWithoutHash() {
            const url = new URL(window.location.href);
            url.hash = '';
            return url.toString();
        }
        static scrollToTopIfPageUrlHasChanged() {
            const newUrl = DntWindowLocationChangeWatcher.getCurrentUrlWithoutHash();
            if (DntWindowLocationChangeWatcher._previousPageUrl != newUrl) {
                window.scrollTo({ top: 0, left: 0, behavior: 'instant' });
            }
            DntWindowLocationChangeWatcher._previousPageUrl = newUrl;
        }
        static showLoadingSpinner() {
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
                const loadingElement = document.getElementById(spinnerId);
                if (loadingElement) {
                    loadingElement.classList.remove('d-none');
                    loadingElement.classList.add('d-block');
                    document.body.prepend(loadingElement);
                }
            }, 300);
            setTimeout(() => {
                const loadingElement = document.getElementById(spinnerId);
                loadingElement?.remove();
            }, 3000);
        }
        static locationChanged() {
            DntWindowLocationChangeWatcher.showLoadingSpinner();
            DntWindowLocationChangeWatcher.scrollToTopIfPageUrlHasChanged();
        }
        static startNavigationInterception() {
            if (DntWindowLocationChangeWatcher._isInitialized) {
                return;
            }
            const { pushState, replaceState } = window.history;
            window.history.pushState = function (...args) {
                pushState.apply(window.history, args);
                window.dispatchEvent(new Event('pushState'));
            };
            window.history.replaceState = function (...args) {
                replaceState.apply(window.history, args);
                window.dispatchEvent(new Event('replaceState'));
            };
            window.addEventListener('popstate', DntWindowLocationChangeWatcher.locationChanged);
            window.addEventListener('replaceState', DntWindowLocationChangeWatcher.locationChanged);
            window.addEventListener('pushState', DntWindowLocationChangeWatcher.locationChanged);
            DntWindowLocationChangeWatcher._isInitialized = true;
        }
        static enable() {
            DntWindowLocationChangeWatcher.startNavigationInterception();
        }
    }
    DntWindowLocationChangeWatcher._isInitialized = false;
    DntWindowLocationChangeWatcher._previousPageUrl = DntWindowLocationChangeWatcher.getCurrentUrlWithoutHash();
    DntBlazorSsr.DntWindowLocationChangeWatcher = DntWindowLocationChangeWatcher;
})(DntBlazorSsr || (DntBlazorSsr = {}));
(() => {
    'use strict';
    DntBlazorSsr.DntUtilities.enable();
    window.addEventListener('DOMContentLoaded', () => {
        if (typeof Blazor !== 'undefined') {
            Blazor.addEventListener('enhancedload', () => {
                DntBlazorSsr.DntUtilities.enable();
            });
        }
        else {
            console.error('Please include the `_framework/blazor.web.js` file first!');
        }
    });
})();
