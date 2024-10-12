namespace DntBlazorSsr {
    export class DntNavLinkMenu {
        static enable(): void {
            document.querySelectorAll<HTMLAnchorElement>('a.nav-link.dropdown-toggle').forEach(element => {
                element.onclick = (event) => {
                    const nextElementSibling = element.nextElementSibling as HTMLElement;
                    if (!nextElementSibling) {
                        return;
                    }

                    event.preventDefault();

                    nextElementSibling.classList.remove('slideOut');
                    nextElementSibling.classList.add('slideIn');
                    nextElementSibling.style.display = 'block';
                };
                element.onblur = () => {
                    setTimeout(() => {
                        const nextElementSibling = element.nextElementSibling as HTMLElement;
                        if (!nextElementSibling) {
                            return;
                        }

                        nextElementSibling.classList.remove('slideIn');
                        nextElementSibling.classList.add('slideOut');
                        nextElementSibling.style.display = 'none';
                    }, 250);
                };
            });
        }
    }
}
