$(function () {

   


    //var name = prompt('Enter your name:', '');
    var name = 'Jens och David';
   // $('#display_name').text(name);

    // Declare a proxy to reference the hub.
    var hub = $.connection.brun7Hub;
    // Create a function that the hub can call to broadcast messages.

    var gameId;
    var gamePlayers;

    function refreshGameRoom() {
        for (var i = 0; i < gamePlayers.length; i++) {
            var player = gamePlayers[i];

            var rows = player.Rows;
            var table = document.createElement('table');

            for (var r = 0, tr; r < rows.length; r++) {
                var row = rows[r];
                tr = document.createElement('tr');

                for (var c = 0; c < row.Squares.length; c++) {
                    var square = row.Squares[c];
                    var td = document.createElement('td');
                    td.appendChild(document.createTextNode(square.Number + ' ' + square.Checked));
                    tr.appendChild(td);
                }

                table.appendChild(tr);
            }
            
            $("#cards").append($(table));

        }
    };

    hub.client.listGames = function (games) {

    }

    hub.client.roomCreated = function(id, players) {
        //hide create view
        $("#create-area").hide();

        //show game view
        $("#game-view").show();

        gameId = id;
        gamePlayers = players;

        refreshGameRoom();
    }

    

    $.connection.hub.start().done(function () {

        hub.server.hello();

        $("#create_game").click(function() { 
            hub.server.createGame(name);
        });

        $("#start-game").click(function() {
            //hub.server.startGame(gameId);
        });
    });

});