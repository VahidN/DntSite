namespace DntBlazorSsr {
    export class DntReportErrors {
        static postError(errorMessage: string) {
            if (!errorMessage || errorMessage.toLowerCase().includes("s2/favicons")) {
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

        static enable(): void {
            window.onerror = (message, url, lineNo, columnNo, error) => {
                DntReportErrors.postError(`JavaScript error: ${message} on line ${lineNo} and column ${columnNo} for ${url} \n ${error?.stack}`);
            };
        }
    }
}
