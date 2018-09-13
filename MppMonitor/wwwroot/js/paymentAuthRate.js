"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/paymentAuthRateHub").build();

connection.on("UpdatePaymentAuthRate-Realex", function (authRate) {
    update(authRate, "Realex");
});

connection.on("UpdatePaymentAuthRate-SecPayVpn", function (authRate) {
    update(authRate, "SecPay");
});

connection.on("UpdatePaymentAuthRate-Overall", function (authRate) {
    update(authRate, "Overall");
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function update(authRate, paymentProvider) {
    document.getElementById("PaymentAuthRateRate-" + paymentProvider).textContent = authRate;
    document.getElementById("PaymentAuthRatePaymentProvider-" + paymentProvider).textContent = paymentProvider;

    var level = "good";
    if (authRate < 40) {
        level = "caution";
    }
    if (authRate < 30) {
        level = "alert";
    }

    document.getElementById("PaymentAuthRateContainer-" + paymentProvider).className = level + " fullBody";
}
