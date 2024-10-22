namespace DntBlazorSsr {
    export class DntWindowLocationChangeWatcher {

        private static _isInitialized: boolean = false;

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
                const loadingElement = document.getElementById(spinnerId)
                if (loadingElement) {
                    loadingElement.classList.remove('d-none');
                    loadingElement.classList.add('d-block');
                    document.body.prepend(loadingElement);
                }
            }, 300);

            setTimeout(() => {
                const loadingElement = document.getElementById(spinnerId)
                loadingElement?.remove();
            }, 3000);
        }

        static locationChanged() {
            DntWindowLocationChangeWatcher.showLoadingSpinner();
        }

        static enable(): void {
            if (DntWindowLocationChangeWatcher._isInitialized) {
                return;
            }

            const {pushState, replaceState} = window.history;

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
    }
}
