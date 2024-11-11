namespace DntBlazorSsr {
    export class DntReportErrors {
        static postError(errorMessage: string) {
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

        static enable(): void {
            window.onerror = (message, url, lineNo, columnNo, error) => {
                DntReportErrors.postError(`JavaScript error: ${message} on line ${lineNo} and column ${columnNo} for ${url} \n ${error?.stack}`);
            };
        }

        private static ignoreMessage(message: string) {
            return message.includes("s2/favicons") || message.includes("chrome-extension");
        }
    }
}
