namespace DntBlazorSsr {
    export class DntSyntaxHighlighter {
        static enable(): void {
            // @ts-ignore
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

                    // Blogger mode flag
                    const br = /<br\s*\/?>/gi;
                    preHtml = preHtml.replace(br, '\n');
                    preHtml = `<div align='left' style='direction:ltr;text-align:left;' dir='ltr'><pre class='line-numbers'><code class='${language}'>${preHtml}</code></pre></div>`;
                    element.innerHTML = preHtml;
                }
            });

            // @ts-ignore
            hljs.highlightAll();
        }
    }
}
