$(document).ready(
    $.ajax( function() {
        url: "/Pizza/Success",
        type: "GET",
        success: function (result) {
            console.log(result.message)
        },
        error: function () {
            console.log("errore")
        }
    })
)