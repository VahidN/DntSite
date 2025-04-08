namespace DntBlazorSsr {
    export class DntHtmlEditor {
        static getUniqueId(outerDivElement: HTMLElement): string | null {
            return outerDivElement.getAttribute('data-dnt-html-editor-id');
        }

        static setEditorElementHeight(editorElement: HTMLElement, toolbar: HTMLElement) {
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

        static scrollToCursor(editorElement: HTMLElement) {
            editorElement.scrollIntoView({
                behavior: "instant",
                block: "center",
                inline: "nearest"
            });
        }

        static setTextDirectionOnPaste(quill: any, editorElement: HTMLElement) {
            editorElement.addEventListener('paste', function (ce) {
                const initialText = DntHtmlEditor.getEditorTextContent(quill);
                if (initialText === "") {
                    const pastedText = ce.clipboardData?.getData('text/plain');
                    const dir = DntChangeInputDirectionDependOnLanguage.getDirection(pastedText);
                    if (dir === 'ltr') {
                        DntHtmlEditor.setLtrDir(quill);
                    } else {
                        DntHtmlEditor.setRtlDir(quill);
                    }
                }
            }, true);
        }

        static setRtlDir(quill: any) {
            // @ts-ignore
            quill.format('align', 'right', Quill.sources.USER);
            // @ts-ignore
            quill.format('direction', 'rtl', Quill.sources.USER);
        }

        static setLtrDir(quill: any) {
            // @ts-ignore
            quill.format('align', 'left', Quill.sources.USER);
            // @ts-ignore
            quill.format('direction', 'ltr', Quill.sources.USER);
        }

        static displayFullyLoadedEditor(outerDivElement: HTMLElement) {
            outerDivElement.classList.remove('d-none');
        }

        static getEditorOptions(editorElement: HTMLElement) {
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

            let maximumUploadImageSizeInBytes: number | null = null;
            const maximumUploadImageSize = editorElement.getAttribute('data-dnt-html-editor-maximum-upload-image-size-in-bytes');
            if (maximumUploadImageSize) {
                maximumUploadImageSizeInBytes = parseInt(maximumUploadImageSize, 10);
            }

            let maximumUploadFileSizeInBytes: number | null = null;
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

        static hideTextArea(textAreaElement: HTMLElement) {
            textAreaElement.style.display = 'none';
        }

        static cleanWhiteSpaces(text: string | null): string | undefined {
            return text?.replace(/&nbsp;/g, " ");
        }

        static getEditorHtmlContent(quill: any): string {
            // @ts-ignore
            return DntHtmlEditor.cleanWhiteSpaces(quill.getSemanticHTML()) ?? "";
        }

        static getEditorTextContent(quill: any): string {
            // @ts-ignore
            return new DOMParser().parseFromString(DntHtmlEditor.getEditorHtmlContent(quill), 'text/html').body.textContent?.trim() || "";
        }

        static synchronizeQuillAndTextArea(quill: any, textAreaElement: HTMLTextAreaElement) {
            // @ts-ignore
            quill.on('editor-change', (eventName, ...args) => {
                DntHtmlEditor.addDirectionToParagraphs();
                textAreaElement.value = DntHtmlEditor.getEditorHtmlContent(quill);
            });
        }

        static handleInsertImageUrl(quill: any, insertImageUrlLabel: string | null) {
            if (!insertImageUrlLabel) {
                return;
            }

            const url = prompt(insertImageUrlLabel);
            if (url) {
                const range = quill.getSelection();
                // @ts-ignore
                quill.insertEmbed(range.index, 'image', url, Quill.sources.USER);
            }
        }

        static showProgress(uniqueId: string, percent: number) {
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

        static hideProgress(uniqueId: string) {
            const progressDiv = document.getElementById(`progress-${uniqueId}`);
            if (!progressDiv) {
                return;
            }

            progressDiv.classList.add('d-none');
        }

        static showErrorMessage(uniqueId: string, message: string | null, direction: string | undefined) {
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

        static hideErrorMessage(uniqueId: string) {
            const errorDiv = document.getElementById(`alert-${uniqueId}`);
            if (!errorDiv) {
                return;
            }

            errorDiv.classList.add('d-none');
        }

        static humanFileSize(size: number): string {
            const i = size === 0 ? 0 : Math.floor(Math.log(size) / Math.log(1024));
            return (size / Math.pow(1024, i)).toFixed(2) + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][i];
        }

        static addMoreAlignAndDirections() {
            // @ts-ignore
            Quill.imports['formats/align'].whitelist.push('left');
            // @ts-ignore
            Quill.imports['formats/direction'].whitelist.push('ltr');
        }

        static addDirectionToParagraphs() {
            document.querySelectorAll<HTMLElement>('.ql-editor > .ql-direction-ltr').forEach(element => {
                element.style.textAlign = 'left';
                element.dir = 'ltr';
            });
            document.querySelectorAll<HTMLElement>('.ql-editor > .ql-direction-rtl').forEach(element => {
                element.style.textAlign = 'right';
                element.dir = 'rtl';
            });
        }

        static cleanAllStyles(editorElement: HTMLElement) {
            editorElement.querySelectorAll('*').forEach(element => {
                element.removeAttribute('style');
            });

            editorElement.querySelectorAll("li.ql-direction-ltr").forEach(element => {
                element.removeAttribute('class');
                element.removeAttribute('style');
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

        static makeInlineCode(quill: any) {
            // @ts-ignore
            const range = quill.getSelection();
            if (range) {
                quill.formatText(range.index, range.length, {
                    'code': true,
                    'direction': 'ltr'
                });
                quill.formatText(range.index, range.length, 'dir', 'ltr');
            }
        }

        static handleDirection(quill: any, value: any) {
            // @ts-ignore
            const {align} = quill.getFormat();

            if (align === 'right') {
                // adds class="ql-direction-ltr ql-align-left"
                DntHtmlEditor.setLtrDir(quill);
            } else if (value === 'rtl') {
                // adds class="ql-align-right ql-direction-rtl"
                DntHtmlEditor.setRtlDir(quill);
            }
        }

        static uploadFile(uniqueId: string,
                          quill: any,
                          accept: string | null,
                          isImage: boolean,
                          apiUrl: string | null,
                          uploadOnlyImageFileErrorMessage: string | null,
                          additionalJsonData: string | null,
                          maximumFileSizeInBytes: number | null,
                          maximumUploadFileSizeErrorMessage: string | null) {

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
                            // @ts-ignore
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
                        // @ts-ignore
                        dntHtmlEditor.showErrorMessage(uniqueId, e.message, 'ltr');
                        throw e;
                    }
                    Object.entries(jsonObject).forEach((entry) => {
                        const [key, value] = entry;
                        formData.append(key, value as string);
                    });
                }

                xhr.send(formData);
            };
        }

        static setDirection(quill: any, direction: string) {
            const initialText = DntHtmlEditor.getEditorTextContent(quill);
            if (initialText === "") {
                const isRtl = direction && direction === 'rtl';
                quill.format('direction', isRtl ? 'rtl' : 'ltr');
                quill.format('align', isRtl ? 'right' : 'left');
            }
        }

        static addMoreLanguages() {
            // @ts-ignore
            const Syntax = Quill.imports["modules/syntax"];
            Syntax.DEFAULTS.languages = [
                {key: 'plain', label: 'Plain'},
                {key: 'bash', label: 'Bash'},
                {key: 'cpp', label: 'C++'},
                {key: 'cs', label: 'C#'},
                {key: 'css', label: 'CSS'},
                {key: 'diff', label: 'Diff'},
                {key: 'xml', label: 'HTML/XML'},
                {key: 'java', label: 'Java'},
                {key: 'javascript', label: 'JavaScript'},
                {key: 'markdown', label: 'Markdown'},
                {key: 'php', label: 'PHP'},
                {key: 'python', label: 'Python'},
                {key: 'ruby', label: 'Ruby'},
                {key: 'sql', label: 'SQL'},
                {key: "typescript", label: "TypeScript"},
                {key: "vbnet", label: "VB.NET"},
                {key: "rust", label: "Rust"},
                {key: 'go', label: "Go"},
                {key: 'swift', label: 'Swift'},
                {key: 'objectivec', label: 'Objective C'},
                {key: 'yaml', label: "Yaml"},
                {key: 'json', label: 'JSON'},
                {key: 'kotlin', label: 'Kotlin'},
                {key: 'perl', label: 'Perl'},
                {key: 'ini', label: 'Ini'},
                {key: 'less', label: 'Less'},
                {key: 'scss', label: 'Scss'},
                {key: 'shell', label: 'Shell'}
            ];
            Syntax.DEFAULTS.languages.sort((a: { label: string; }, b: { label: string; }) => {
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
            // @ts-ignore
            if (typeof Quill === 'undefined') {
                console.error('Please include the `quill/dist/quill.js` file first!');
                return;
            }

            const dntHtmlEditorIdentifier = 'data-dnt-html-editor';
            document.querySelectorAll<HTMLElement>(`[${dntHtmlEditorIdentifier}]`).forEach(outerDivElement => {
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
                DntHtmlEditor.addMoreLanguages();
                DntHtmlEditor.addMoreAlignAndDirections();
                // @ts-ignore
                const quill = new Quill(editorElement, {
                    debug: 'warn',
                    readOnly: isReadOnly,
                    modules: {
                        syntax: true,
                        toolbar: {
                            container: `#${toolbarId}`,
                            handlers: {
                                'inline-code': (value: any) => dntHtmlEditor.makeInlineCode(quill),
                                'clean-styles': (value: any) => {
                                    dntHtmlEditor.cleanAllStyles(editorElement);
                                    dntHtmlEditor.convertMonoSpaceSpansToCode();
                                },
                                direction: (value: any) => {
                                    dntHtmlEditor.handleDirection(quill, value);
                                    dntHtmlEditor.addDirectionToParagraphs();
                                },
                                uploadImageFile: (value: any) => dntHtmlEditor.uploadFile(uniqueId,
                                    quill,
                                    acceptedUploadImageFormats,
                                    true,
                                    uploadImageFileApiPath,
                                    uploadOnlyImageFileErrorMessage,
                                    additionalJsonDataDuringImageFileUpload,
                                    maximumUploadImageSizeInBytes,
                                    maximumUploadImageSizeErrorMessage),
                                insertImageUrl: (value: any) => dntHtmlEditor.handleInsertImageUrl(quill, insertImageUrlLabel),
                                uploadFile: (value: any) => dntHtmlEditor.uploadFile(uniqueId,
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
                dntHtmlEditor.scrollToCursor(editorElement);
                dntHtmlEditor.setTextDirectionOnPaste(quill, editorElement);
            });
        }
    }
}
