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

        static synchronizeQuillAndTextArea(quill: any, textAreaElement: HTMLTextAreaElement) {
            // @ts-ignore
            quill.on('editor-change', (eventName, ...args) => {
                textAreaElement.value = quill.getSemanticHTML();
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
            const initialContent = quill.getSemanticHTML();
            const initialText = new DOMParser().parseFromString(initialContent, 'text/html').body.textContent || "";
            if (initialText === "") {
                const isRtl = direction && direction === 'rtl';
                quill.format('direction', isRtl ? 'rtl' : 'ltr');
                quill.format('align', isRtl ? 'right' : 'left');
            }
        }

        static enable() {
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
                // @ts-ignore
                const quill = new Quill(editorElement, {
                    debug: 'warn',
                    readOnly: isReadOnly,
                    modules: {
                        syntax: true,
                        toolbar: {
                            container: `#${toolbarId}`,
                            handlers: {
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
            });
        }
    }
}
