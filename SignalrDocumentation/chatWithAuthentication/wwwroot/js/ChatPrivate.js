var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatter")
    .build();


connection.on("NewMsg", function (content,date) {
    var p = document.createElement("p");
    p.textContent = `${content}         at ${date}`;
    document.getElementsByClassName("messages-chat")[0].appendChild(p);
});

document.getElementById("clickS").addEventListener("click", async (event) => {
    var msg= document.getElementById("write-message").value;
    try {
        var n = document.getElementsByClassName("name")[0].innerHTML
        await connection.invoke("SendMessagePrivate",n, msg );
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
});

(async () => {
    try {
        await connection.start();
    }
    catch (e) {
        console.error(e.toString());
    }
})();