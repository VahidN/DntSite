namespace DntBlazorSsr {
    export class DntStatusCheck {
        static updateOnlineStatus(): void {
            document.querySelectorAll<HTMLInputElement>('[id^="dnt-online-status-report"]').forEach(statusBarElement => {
                if (navigator.onLine) {
                    statusBarElement.classList.add('d-none');
                    statusBarElement.innerHTML = '';
                } else {
                    statusBarElement.classList.remove('d-none');
                    statusBarElement.classList.add('badge', 'text-bg-danger', 'fs-6', 'mt-1', 'mb-2', 'show');
                    statusBarElement.innerHTML = 'مرورگر در این لحظه آنلاین نیست. لطفا از اتصال خود به شبکه مطمئن شوید.';
                }
            });
        }

        static enable(): void {
            window.addEventListener('online', DntStatusCheck.updateOnlineStatus);
            window.addEventListener('offline', DntStatusCheck.updateOnlineStatus);
        }
    }
}
