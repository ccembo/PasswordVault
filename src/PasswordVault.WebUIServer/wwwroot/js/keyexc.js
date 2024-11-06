"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/KeyExchangeHub").build();

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

connection.on("KeyExchangeReceived", function (user, message) {

    console.log( `${user} received the key: ${message}`);
    location.reload();
});

connection.start().then(function () {
    document.getElementById("send").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("send").addEventListener("click", function (event) {
    var user = "user";//document.getElementById("userInput").value;
    var message = document.getElementById("key").value;
    document.getElementById("loading").hidden = false;
    connection.invoke("RequestKey", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});