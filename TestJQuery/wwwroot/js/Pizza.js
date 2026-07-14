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

function pizObs() {
    const pizzaDesc = document.querySelectorAll(".infos");

    if (pizzaDesc.length === 0) return; 

    const observer = new IntersectionObserver((items) => {
        items.forEach(item => {
            if (item.isIntersecting) {
                item.target.classList.add("active");

            }
        })
    }, { threshold: 0.3 });

    pizzaDesc.forEach(el => {
        observer.observe(el);
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
            <div class="card" data-pizza-id="${pizza.id}" data-pizza-name="${pizza.name}" data-pizza-price="${pizza.price}">
                <div class="cont-img-p">
                    <div class="cont-img-piz">
                        <img id="img-piz" src="${pizza.image}" loading="lazy" alt="${pizza.name}"/>
                    </div>
                    <div class="cont-info-piz">
                        <h3 class="infos piz-name">${pizza.name}</h3>
                        <p class="infos">${pizza.description}</p>
                        <p class="infos"><strong>€${pizza.price}</strong></p>
                    </div>
                </div>
                <div class="add-but-cont">
                    <h4 class="add-but">Add To Cart</h4>
                </div>
            </div>
        `
    })

    contPizze.html(html); 

    pizObs();

}


// Event Delegation con jQuery - ascolta sia il div che il bottone
$(document).on("click", ".add-but-cont, .add-but", function(e) {
    e.stopPropagation();

    const card = $(this).closest(".card");
    const pizzaId = card.data("pizza-id");
    const pizzaName = card.data("pizza-name");
    const pizzaPrice = card.data("pizza-price");

    console.log("Pizza ID:", pizzaId, "Nome:", pizzaName, "Prezzo:", pizzaPrice); // ← Debug

    $.ajax({
        url: "/Pizza/PostOrd",
        type: "POST",
        data: {
            pizzaId: pizzaId,
            quantity: 1,
            pizzaName: pizzaName,
            pizzaPrice: pizzaPrice
        },
        success: function (response) {
            console.log("Risposta server:", response); // ← Debug
            $(".notif-overlay").addClass("active").css("display", "block");
            $(".notif").addClass("active").css("display", "flex");
        },
        error: function (xhr) {
            console.error("Errore:", xhr.responseJSON); // ← Debug
            alert("Error adding pizza");
        }
    })
});

$(document).on("click", ".but-continue", function () {
    $(".notif-overlay").removeClass("active").css("display", "none");
    $(".notif").removeClass("active").css("display", "none");
});

// Chiudi notifica al click fuori
$(".notif-overlay").on("click", function() {
    const notif = $(".notif");
    const overlay = $(".notif-overlay");

    if (notif.hasClass("active")) {
        notif.removeClass("active").css("display", "none");
        overlay.removeClass("active").css("display", "none");
    }
});



