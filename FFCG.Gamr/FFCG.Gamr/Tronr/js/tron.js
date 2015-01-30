$(function () {
    // Declare a proxy to reference the hub.
    var tron = $.connection.tronHub;
    // Create a function that the hub can call to broadcast messages.
    tron.client.hello = function (data) {
        $("#content").append(data);
    };

    $.connection.hub.start().done(function () {
        $('#lab').click(function () {
            // Call the Send method on the hub.
            tron.server.send();
            // Clear text box and reset focus for next comment.

        });
    });

});