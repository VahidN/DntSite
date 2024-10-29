namespace DntBlazorSsr {
    export class DntHljsCopyToClipboardPlugin {
        static enable(): void {
            // @ts-ignore
            hljs?.addPlugin({
                // @ts-ignore
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
    }
}
