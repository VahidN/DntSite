namespace DntBlazorSsr {
    export class DntBootstrapDarkMode {
        static enable(): void {
            const lightSwitch = document.getElementById("lightSwitch") as HTMLInputElement;
            const lightSwitchIcon = document.getElementById("lightSwitch-icon");
            if (!lightSwitch || !lightSwitchIcon) {
                return;
            }

            const setTheme = (isDark: boolean) => {
                const mode = isDark ? "dark" : "light";
                if (!lightSwitch.checked) {
                    lightSwitch.checked = isDark;
                }
                DntStorageProvider.setItem("lightSwitch", mode);
                document.documentElement.setAttribute("data-bs-theme", mode);
                lightSwitchIcon.setAttribute("class", isDark ? "bi-moon me-1" : "bi-sun me-1");
            };

            const onToggleMode = () => setTheme(lightSwitch.checked);

            const isSystemDefaultThemeDark = () =>
                window.matchMedia("(prefers-color-scheme: dark)").matches;

            const setup = () => {
                let settings = DntStorageProvider.getItem("lightSwitch");
                if (!settings) {
                    settings = isSystemDefaultThemeDark() ? "dark" : "light";
                }

                lightSwitch.checked = settings === "dark";
                lightSwitch.addEventListener("change", onToggleMode);
                onToggleMode();
            };

            setup();
        }
    }
}
