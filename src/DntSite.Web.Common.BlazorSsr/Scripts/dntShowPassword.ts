namespace DntBlazorSsr {
    export class DntShowPassword {
        static enable(): void {
            document.querySelectorAll<HTMLInputElement>("input[type=password]").forEach(passwordElement => {

                const wrapper = passwordElement.parentElement;
                if (!wrapper) {
                    return;
                }

                const showPasswordButton = document.createElement("button");
                showPasswordButton.type = "button";
                showPasswordButton.classList.add("btn", "btn-outline-secondary", "btn-sm", "bi", "bi-eye");
                showPasswordButton.setAttribute("title", "نمایش کلمه عبور");

                wrapper.append(showPasswordButton);

                showPasswordButton.onclick = () => {
                    const type = passwordElement.getAttribute('type');
                    if (type === 'password') {
                        passwordElement.setAttribute('type', 'text');

                        showPasswordButton.classList.remove("bi-eye");
                        showPasswordButton.classList.add("bi-eye-slash");
                        showPasswordButton.setAttribute("title", "پوشاندن کلمه عبور");
                    } else {
                        passwordElement.setAttribute('type', 'password');

                        showPasswordButton.classList.remove("bi-eye-slash");
                        showPasswordButton.classList.add("bi-eye");
                        showPasswordButton.setAttribute("title", "نمایش کلمه عبور");
                    }
                };
            });
        }
    }
}
