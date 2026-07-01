$(document).ready(function(){
    const searchInput = $("#searchInput");

    loadPizzas();

    searchInput.keyup(function () {
        let search = searchInput.val();
        loadPizzas(search);
    });
});

function loadPizzas(searchString = "") {
    $.ajax({
        type: "GET",
        url: "/Pizza/PizzaMethod",
        data:
        {
            search: searchString
        },
        success:
        function (data) {
            console.log("Dati ricevuti:", data); // ← DEBUG
            renderPizzas(data)
        },
        error:
        function () {
            $("#elencoPizze").html("<p>Errore nel caricamento delle pizze</p>"); 
        }
    })
}

function renderPizzas(pizzas) {
    let html = ""; 
    let contPizze = $("#elencoPizze");

    if (pizzas.length === 0) {
        contPizze.html("<p>Nessuna pizza trovata</p>")
        return;
    }

    pizzas.forEach(function (pizza) {
        console.log("Pizza singola:", pizza); // ← DEBUG
        html += `
            <div class="card">
                <p>Nome: ${pizza.name}</p>
                <p>Descrizione: ${pizza.description}</p>
                <p>Prezzo: €${pizza.price}</p>
            </div>
        `
    })

    contPizze.html(html);
}