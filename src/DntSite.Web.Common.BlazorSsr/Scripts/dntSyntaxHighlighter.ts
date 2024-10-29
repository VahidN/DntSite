namespace DntBlazorSsr {
    export class DntSyntaxHighlighter {
        static enable(): void {
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
                        case "sql":
                            language = "language-sql";
                            break;
                        case "xml":
                            language = "language-xml";
                            break;
                        case "css":
                            language = "language-css";
                            break;
                        case "java":
                            language = "language-java";
                            break;
                        case "delphi":
                        case "pas":
                            language = "language-pas";
                            break;
                        case "fsharp":
                            language = "language-fsharp";
                            break;
                        case "typescript":
                            language = "language-typescript";
                            break;
                        case "rust":
                            language = "language-rust";
                            break;
                        case "powershell":
                            language = "language-powershell";
                            break;
                        case "bash":
                            language = "language-bash";
                            break;
                        case "php":
                            language = "language-php";
                            break;
                        case "git":
                            language = "language-git";
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
            hljs?.highlightAll();
        }
    }
}
