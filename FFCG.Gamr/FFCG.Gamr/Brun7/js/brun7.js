$(function () {

   


    //var name = prompt('Enter your name:', '');
    var name = 'Jens och David';
   // $('#display_name').text(name);

    // Declare a proxy to reference the hub.
    var hub = $.connection.brun7Hub;
    // Create a function that the hub can call to broadcast messages.

    var gameId;
    function refreshGameRoom(players) {
        for (var i = 0; i < players.length; i++) {
            var player = players[i];

            var rows = player.Rows;
            var table = document.createElement('table');

            //var headerRow = document.createElement('tr');
            //var headerCell = document.createElement('td');
            //headerCell.colSpan = 5;
            //headerCell.createTextNode(player.Name);

            //headerRow.appendChild(headerCell);
            //table.appendChild(headerRow);

            for (var r = 0, tr; r < rows.length; r++) {
                var row = rows[r];
                tr = document.createElement('tr');

                for (var c = 0; c < row.Squares.length; c++) {
                    var square = row.Squares[c];
                    var td = document.createElement('td');
                    td.appendChild(document.createTextNode(square.Number));

                    if (square.Checked) {
                        td.className = "checked";
                    }

                    tr.appendChild(td);
                }

                table.appendChild(tr);
            }
            
            $("#cards").html($(table));

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

        refreshGameRoom(players);
    }

    hub.client.refreshCurrentGameState = function (currentNumber, players) {
        $("#currentNumber").append(currentNumber + ', ');
        refreshGameRoom(players);
    }

    hub.client.announceBingoWinner = function(winner) {
        $("#winner").text(winner + " has bingo!!!");
        $("#winner").fadeIn(500);
    }

    $.connection.hub.start().done(function () {

        hub.server.hello();

        $("#create_game").click(function() { 
            hub.server.createGame(name);
        });

        $("#start-game").click(function() {
            hub.server.startGame(gameId);
        });
    });

});