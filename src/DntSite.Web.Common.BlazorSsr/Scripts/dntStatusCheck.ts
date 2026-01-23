namespace DntBlazorSsr {
    export class DntStatusCheck {
        static updateOnlineStatus(): void {
            document.querySelectorAll<HTMLInputElement>('[id^="dnt-online-status-report"]').forEach(statusBarElement => {
                if (navigator.onLine) {
                    DntStatusCheck.hideStatusBarElement(statusBarElement);
                } else {
                    if (DntStatusCheck.isLocalEnvironment()) {
                        DntStatusCheck.hideStatusBarElement(statusBarElement);
                        return;
                    }

                    DntStatusCheck.onlineCheck("/").then(() => {
                        DntStatusCheck.hideStatusBarElement(statusBarElement);
                    }).catch(() => {
                        DntStatusCheck.showStatusBarElement(statusBarElement);
                    });
                }
            });
        }

        static showStatusBarElement(statusBarElement: HTMLInputElement): void {
            statusBarElement.classList.remove('d-none');
            statusBarElement.classList.add('badge', 'text-bg-danger', 'fs-6', 'mt-1', 'mb-2', 'show');
            statusBarElement.innerHTML = 'مرورگر در این لحظه آنلاین نیست. لطفا از اتصال خود به شبکه مطمئن شوید.';
        }

        static hideStatusBarElement(statusBarElement: HTMLInputElement): void {
            statusBarElement.classList.add('d-none');
            statusBarElement.innerHTML = '';
        }

        static enable(): void {
            window.addEventListener('online', DntStatusCheck.updateOnlineStatus);
            window.addEventListener('offline', DntStatusCheck.updateOnlineStatus);

            // Initial check
            DntStatusCheck.updateOnlineStatus();
        }

        static onlineCheck(baseUrl: string): Promise<boolean> {
            let xhr = new XMLHttpRequest();
            return new Promise((resolve, reject) => {
                xhr.onload = () => {
                    resolve(true);
                };
                xhr.onerror = () => {
                    reject(false);
                };
                xhr.open('GET', baseUrl, true);
                xhr.send();
            });
        }

        static isLocalEnvironment(): boolean {
            const {protocol, hostname} = window.location;

            const isLocalhost = hostname === 'localhost' || hostname === '127.0.0.1' || hostname === '[::1]';
            const isPrivateIP = /^10\./.test(hostname) || /^192\.168\./.test(hostname) || /^172\.(1[6-9]|2\d|3[0-1])\./.test(hostname);

            return protocol === 'file:' || isLocalhost || isPrivateIP;
        }
    }
}
