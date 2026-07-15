

$(document).ready(
    $.ajax({
        url: "/Pizza/CartData",
        type: "GET",
        success: function (result) {
            console.log("pizzas: ", result)
            renderOrders(result);
        },
        error: function () {
            console.log("errore");
        }
    })
)

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
        tot += ord.priceSnapshot / 100
    })

    html += `<h4><strong>Totale: €${tot.toFixed(2)}</strong></h4>`

    ords.html(html);
}