$(function () {
    // Declare a proxy to reference the hub.
    var hub = $.connection.destroyrHub;
    // Create a function that the hub can call to broadcast messages.
    
    hub.client.handshake = function(tst) {
        
    }

    $.connection.hub.start().done(function () {
        hub.server.handshake();
    });

});