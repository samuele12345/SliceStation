const xBut = document.querySelector(".esc");
const pop = document.querySelector(".pop-up");

// Controlla se gli elementi esistono prima di aggiungere eventi
if (xBut && pop) {
    xBut.addEventListener("click", () => {
        pop.classList.add("hidden");
    });
}