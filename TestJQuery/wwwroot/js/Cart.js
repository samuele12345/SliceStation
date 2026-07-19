

$(document).ready(function () {

    var totPrice;
    // La chiave Stripe viene passata dall'HTML (vedi Cart.cshtml)
    var stripeKey = document.getElementById('stripe-publishable-key').value;
    var stripe = Stripe(stripeKey);

    $.ajax({
        url: "/Pizza/CartData",
        type: "GET",
        success: function (result) {
            console.log("pizzas: ", result)
            totPrice = renderOrders(result);
        },
        error: function () {
            console.log("errore");
        }
    })

    $(".btn-ck").on("click", function () {
        console.log("Totale da inviare (centesimi):", totPrice);

        $.ajax({
            url: "/Pizza/StripeCheckout",
            type: "POST",
            data: {
                total: totPrice
            },
            success: function (result) {
                console.log("Risposta backend:", result);

                if (result.sessionId) {
                    // Successo: reindirizza a Stripe
                    stripe.redirectToCheckout({ sessionId: result.sessionId })
                        .then(function (error) {
                            if (error) {
                                console.error("Errore Stripe:", error);
                                alert("Errore reindirizzamento: " + error.message);
                            }
                        });
                } else {
                    // Errore logico dal backend
                    alert(result.message || "Errore durante la creazione della sessione");
                }
            },
            error: function (xhr, status, error) {
                console.error("Errore AJAX:", error);
                alert("Errore di connessione al server");
            }
        })
    })

    $(document).on("click", ".x-icon", function () {
        const $cartItem = $(this).closest(".cart-item");
        const pizzaId = $cartItem.data("id");  // ← Usa l'ID reale dal DB
        const priceSnapshot = parseFloat($cartItem.data("price"));  // ← Prezzo in centesimi
        const quantity = $cartItem.data("quant");

        $.ajax({
            url: "/Pizza/DeleteItem",
            type: "POST",
            data: {
                num: pizzaId  // ← Invia l'ID reale
            },
            success: function (result) {
                console.log(result.message);
                if (result.success === true) {
                    // 1. Rimuovi l'elemento dal DOM
                    $cartItem.remove();
                    

                    // 2. Ricalcola il totale
                    let newTotal = 0;
                    $(".cart-item").each(function () {
                        newTotal += parseFloat($(this).data("price"));
                    });

                    // 3. Aggiorna il totale visualizzato
                    $(".tot-price").html(`<strong>Totale: €${(newTotal / 100).toFixed(2)}</strong>`);

                    // 4. Aggiorna la variabile globale
                    totPrice = newTotal;

                    console.log("Nuovo totale:", newTotal);
                }
            },
            error: function () {
                console.log("errore");
            }
        })
    })
})

function renderOrders(orders) {
    const ords = $(".ordini");
    var html = "";
    var tot = 0;

    if (orders.length == 0) {
        ords.html(`<h2 style="color: white">The cart is empty!</h2>`);
        ords.css({ "min-height": "500px", "display": "flex", "flex-direction": "column", "justify-content": "center", "align-items": "center" });
        return 0;
    }

    orders.forEach(ord => {
        html += `
            <div class="cart-item" data-id="${ord.id}" data-price="${ord.priceSnapshot} data-quant="${ord.quantity}">
                <div class="nam-quant">
                    <h3>${ord.quantity}</h3>
                    <h3>${ord.pizzaName}</h3>

                </div>

                <div class="price-x">
                    <h3>€${(ord.priceSnapshot) / 100}</h3>
                    <!-- aggiungere - e + -->
                    <div><h1 class="x-icon">✕</h1></div>
                </div>

            </div>
        `
        tot += ord.priceSnapshot  // ← MANTIENI IN CENTESIMI!
    })

    html += `<h4 class="tot-price"><strong>Totale: €${(tot / 100).toFixed(2)}</strong></h4>`

    ords.html(html);

    return tot;  // ← Ritorna centesimi (es. 2550)
}

