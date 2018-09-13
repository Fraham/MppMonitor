"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/arQueueSizeHub").build();

connection.on("UpdateArQueueSize", function (size, time) {
    document.getElementById("ArQueueSizeSize").textContent = size;
    document.getElementById("ArQueueSizeTime").textContent = time;

    var level = "good";

    if (size > 10000) {
        level = "caution";
    }
    if (size > 30000) {
        level = "alert";
    }

    document.getElementById("ArQueueSizeContainer").className = level + " fullBody";
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});