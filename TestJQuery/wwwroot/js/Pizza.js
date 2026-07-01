$(document).ready(functioon(){
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
        url: "/PizzaController/PizzaMethod",
        data:
        {
            search: searchString
        },
        success:
        function (data) {
            renderPizzas(data)
            },
        error:

    })
}

function renderPizzas(pizzas) {

}