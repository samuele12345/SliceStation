

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
})

function renderOrders(orders) {
    const ords = $(".ordini");
    var html = "";
    var tot = 0;

    orders.forEach(ord => {
        html += `
            <div class="cart-item">
                <div class="nam-quant">
                    <h3>${ord.quantity}</h3>
                    <h3>${ord.pizzaName}</h3>

                </div>

                <h3>Price: €${(ord.priceSnapshot) / 100}</h3>
            </div>
        `
        tot += ord.priceSnapshot  // ← MANTIENI IN CENTESIMI!
    })

    html += `<h4><strong>Totale: €${(tot / 100).toFixed(2)}</strong></h4>`

    ords.html(html);

    return tot;  // ← Ritorna centesimi (es. 2550)
}

