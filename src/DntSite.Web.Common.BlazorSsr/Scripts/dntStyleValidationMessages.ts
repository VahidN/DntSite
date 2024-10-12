namespace DntBlazorSsr {
    export class DntStyleValidationMessages {
        static enable(): void {
            const validationMessages = document.querySelectorAll("div.validation-message");
            validationMessages.forEach(element => {
                element.classList.add('badge', 'text-bg-danger', 'fs-6', 'mt-2');
            });

            validationMessages[0]?.scrollIntoView({
                behavior: "smooth",
                block: "center",
                inline: "nearest"
            });
        }
    }
}
