namespace DntBlazorSsr {
    export class DntInputFile {
        static enable(): void {
            document.querySelectorAll<HTMLInputElement>('.form-control[type=file].d-none').forEach((fileInput, key, parent) => {
                fileInput.onchange = () => {
                    const fileInputElement = fileInput;
                    const nextElementSibling = fileInputElement.nextElementSibling;
                    if (fileInputElement.files == null || fileInputElement.files[0] == null || !nextElementSibling) {
                        return;
                    }

                    nextElementSibling.innerHTML = [...fileInputElement.files].map(f => f.name).join(', ');
                    nextElementSibling.classList.remove('d-none');
                };
            });
        }
    }
}
