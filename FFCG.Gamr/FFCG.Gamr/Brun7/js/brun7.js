$(function () {
    var name = prompt('Enter your name:', '');
    //var name = 'Jens och David';
    // $('#display_name').text(name);

    // Declare a proxy to reference the hub.
    var hub = $.connection.brun7Hub;
    // Create a function that the hub can call to broadcast messages.

    var gameId;
    var isCreator = false;
    function refreshGameRoom(players) {
        var tables = [];

        for (var i = 0; i < players.length; i++) {
            var player = players[i];

            var rows = player.Rows;
            var table = document.createElement('table');
            table.className = "floatleft";
            var headerRow = document.createElement('tr');
            var headerCell = document.createElement('td');
            headerCell.colSpan = 5;
            headerCell.appendChild(document.createTextNode(player.Name));

            headerRow.appendChild(headerCell);
            table.appendChild(headerRow);

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


            tables.push(table);
        }

        document.getElementById("cards").innerHTML = "";
        //$("#cards").html();

        for (var t = 0; t < tables.length; t++) {
            document.getElementById("cards").appendChild(tables[t]);
            //$("#cards").append($(tables[t]));
        }

    };

    hub.client.roomCreated = function (id, players) {
        //hide create view
        $("#create-area").hide();
        $("#join-area").hide();

        //show game view
        $("#game-view").show();
        $("#start-game").show();

        gameId = id;
        isCreator = true;
        refreshGameRoom(players);
    }

    hub.client.refreshCurrentGameState = function (currentNumber, players) {
        $("#currentNumber").append(currentNumber + ', ');
        refreshGameRoom(players);
    }

    hub.client.playerJoined = function (id, players) {
        $("#create-area").hide();
        $("#join-area").hide();

        //show game view
        $("#game-view").show();

        gameId = id;

        refreshGameRoom(players);
    }



    hub.client.announceBingoWinner = function (winner) {
        $("#winner").text(winner + " has bingo!!!");
        $("#winner").show();

        if (isCreator)
            $("#reset-game").show();
    }

    hub.client.gameReady = function () {
        $("#create-area").hide();

        if(!isCreator)
            $("#join-area").show();
    }

    hub.client.gameResetted = function() {
        $("#reset-game").hide();
        $("#winner").hide();
        $("#currentNumber").html("");
    }

    $.connection.hub.start().done(function () {

        hub.server.hello();

        $("#create_game").click(function () {
            hub.server.createGame(name);
        });

        $("#start-game").click(function () {
            hub.server.startGame();
        });

        $("#join_game").click(function () {
            hub.server.joinGame(name);
        });

        $("#reset-game").click(function() {
            hub.server.resetGame();
            $("#reset-game").hide();
        });
    });

});