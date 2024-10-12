namespace DntBlazorSsr {
    export class DntCardAccordion {
        static enable(): void {
            document.querySelectorAll<HTMLButtonElement>("button[data-dnt-collapse]").forEach(element => {
                element.title = "بستن محتوا";
                element.onclick = () => {
                    const card = element.parentElement?.parentElement;
                    if (!card) {
                        return;
                    }

                    card.querySelectorAll(".card-body, .card-footer, .list-group").forEach(cardChild => {
                        if (cardChild.classList.contains("collapse")) {
                            cardChild.classList.remove("accordion-collapse", "collapse");

                            element.firstElementChild?.classList.remove("bi-chevron-left");
                            element.firstElementChild?.classList.add("bi-chevron-down");
                            element.title = "بستن محتوا";

                        } else {
                            cardChild.classList.add("accordion-collapse", "collapse");

                            element.firstElementChild?.classList.remove("bi-chevron-down");
                            element.firstElementChild?.classList.add("bi-chevron-left");
                            element.title = "گشودن محتوا";
                        }
                    });
                };
            });
        }
    }
}
