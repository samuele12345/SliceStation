$(document).ready(function () {
    const modBut = $(".modifica");
    const contDati = $(".box-container");

    let inModifica = false;

    modBut.click(function () {
        if (inModifica) {

            const newEmail = $("#inp-email").val().trim();
            const newName = $("#inp-name").val().trim();
            const newAddress = $("#inp-address").val().trim();

            $.ajax({
                url: "/Account/CurrentUserData",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    email: newEmail,
                    name: newName,
                    address: newAddress
                }),
                success: function (response) {
                    if (response.success) {

                        contDati.html(
                            `
                                

                                <div class="campo">
                                    <p class="campo-mail">Email: ${response.email}</p>

                                </div>

                                <div class="campo">
                                    <p class="campo-name">Name: ${response.name}</p>
                                </div>

                                <div class="campo">
                                    <p class="campo-address">Address: ${response.address}</p>
                                </div>
                                
                            `
                        )
                        modBut.text("Modifica");
                        inModifica = false;

                        alert("Profilo aggiornato con successo");
                    }
                    else
                    {
                        alert("Errore:" + response.message);
                        if (response.errors) {
                            console.log(response.errors);
                        }
                    }
                },
                error: function () {
                    console.log("errore nel caricamento dei dati")
                }
            })

            
        }
        else
        {
            $.ajax({
                url: "/Account/CurrentUserData",
                type: "GET",
                success: function (data) {
                    if (data.success) {
                        contDati.html(
                            `
                                <div>
                                    <div class="campo">
                                        <p>Email: </p>
                                        <input id="inp-email" type="text" value="${data.email}">
                                    </div>
                        

                                </div>

                                <div>
                                    <div class="campo">
                                        <p>Name: </p>
                                        <input id="inp-name" type="text" value="${data.name}">
                                    </div>

                        
                                </div>

                                <div>
                                    <div class="campo">
                                        <p>Address: </p>
                                        <input id="inp-address" type="text" value="${data.address}">
                                    </div>

                        
                                </div>
                            `
                        )
                    }
                    modBut.text("Salva");
                    inModifica = true;
                },
                error: function () {
                    console.log("error in saving data")
                }
            })
            
        }
    })
})