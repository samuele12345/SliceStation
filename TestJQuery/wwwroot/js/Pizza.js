// Carica pizze immediatamente
loadPizzas();

$(document).ready(function () {
    const searchInput = $("#searchInput");

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
        html += `
            <div class="card">
                <div class="cont-img-piz">
                    <img id="img-piz" src="${pizza.image}" loading="lazy" alt="${pizza.name}"/>
                </div>
                <div class="cont-info-piz">
                    <h3>${pizza.name}</h3>
                    <p>${pizza.description}</p>
                    <p><strong>€${pizza.price}</strong></p>
                </div>
                <div class="add-but-cont">
                    <h4 class="add-but">Add To Cart<h4>
                </div>
            </div>
        `
    })

    contPizze.html(html);
}


// Event Delegation con jQuery
$(document).on("click", ".add-but-cont", function() {
    $(".notif").removeClass("active").addClass("active");
});

// Chiudi notifica al click fuori
$(document).on("click", function(e) {
    const notif = $(".notif");
    if (notif.hasClass("active") && !$(e.target).closest(".notif").length) {
        notif.removeClass("active");
    }
});