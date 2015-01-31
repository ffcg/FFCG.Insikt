$(function () {
    // Declare a proxy to reference the hub.
    var hub = $.connection.destroyrHub;
    // Create a function that the hub can call to broadcast messages.
    
    hub.client.handshake = function(tst) {
        
    }

    $.connection.hub.start().done(function () {
        hub.server.handshake();
    });

    var canvas = document.getElementById("gameCanvas");

    var gameInput = GameInput(window);
    var gameRenderer = GameRenderer(canvas);

    var gameState = {
        World: {
            Width: canvas.scrollWidth,
            Height: canvas.scrollHeight,
            GameObjects: [
                {
                    Name: 'Test1',
                    Position: { X: 100.0, Y: 200.0 },
                    Shape: [
                        { X: -10, Y: -10 }, { X: 20, Y: 0 }, { X: -10, Y: 10 }
                    ],
                    Rotation: 0,
                    Velocity: { X: 0, Y: 0 }
                },
                {
                    Name: 'Test2',
                    Position: { X: 300.0, Y: 400.0 },
                    Shape: [
                        { X: -10, Y: -10 }, { X: 20, Y: 0 }, { X: -10, Y: 10 }
                    ],
                    Rotation: 90,
                    Velocity: { X: 0, Y: 0 }
    }
            ]
        }
    }

    var rotateSpeed = 2.0;
    var acceleration = 0.2;
    var bulletSpeed = 1.2;

    function toDegrees(angle) {
        return angle * (180 / Math.PI);
    }

    function toRadians(angle) {
        return angle * (Math.PI / 180);
    }

    function animateGameObject(world, object) {
        object.Position.X += object.Velocity.X;
        object.Position.Y += object.Velocity.Y;

        if (object.Position.X > world.Width) {
            object.Position.X -= world.Width;
        }
        else if (object.Position.X < 0) {
            object.Position.X = world.Width + object.Position.X;
        }
        if (object.Position.Y > world.Width) {
            object.Position.Y -= world.Width;
        }
        else if (object.Position.Y < 0) {
            object.Position.Y = world.Width + object.Position.Y;
        }
        if (object.Local && object.Local.update) {
            object.Local.update();
        }

    }

    function animateWorld(world) {
        _.each(world.GameObjects, function (object) {
            animateGameObject(world, object);
        });
        world.GameObjects = _.reject(world.GameObjects, function(object) { return object.RemoveMe; });
    }

    function applyUserActionOnClient(actionName) {
        //console.log(actionName);
        var ship = gameState.World.GameObjects[0];
        if (actionName === 'RotateLeft') {
            ship.Rotation = (ship.Rotation - rotateSpeed) % 360;
        }
        else if (actionName === 'RotateRight') {
            ship.Rotation = (ship.Rotation + rotateSpeed) % 360;
        }
        else if (actionName === 'Thrust') {
            var rotation = toRadians(ship.Rotation);
            ship.Velocity.X += Math.cos(rotation) * acceleration;
            ship.Velocity.Y += Math.sin(rotation) * acceleration;
        }
        else if (actionName === 'Fire') {
            var rotation = toRadians(ship.Rotation);
            var bullet = {
                Position: { X: ship.Position.X + Math.cos(rotation) * ship.Shape[1].X, Y: ship.Position.Y + Math.sin(rotation) * ship.Shape[1].Y },
                Shape: [
                    { X: -2, Y: -2 }, { X: 2, Y: 0 }, { X: -2, Y: 2 }
                ],
                Rotation: 0,
                Velocity: { X: ship.Velocity.X + Math.cos(rotation) * bulletSpeed, Y: ship.Velocity.Y + Math.sin(rotation) * bulletSpeed },
                Local: {
                    age: 0,
                    update: function() {
                        bullet.Local.age++;
                        if (bullet.Local.age > 100) {
                            bullet.RemoveMe = true;
                        }
                    }
                }
            };
            gameState.World.GameObjects.push(bullet);
        }
    }

    function handleUserAction(actionName) {
        //hub.server.userAction(actionName);
        applyUserActionOnClient(actionName);
    }

    function handleInput() {
        if (gameInput.keys[gameInput.key.LEFT]) {
            handleUserAction('RotateLeft');
        }
        if (gameInput.keys[gameInput.key.RIGHT]) {
            handleUserAction('RotateRight');
        }
        if (gameInput.keys[gameInput.key.UP]) {
            handleUserAction('Thrust');
        }
        if (gameInput.keys[gameInput.key.SPACE]) {
            handleUserAction('Fire');
        }
    }


    function receiveInput() {
        handleInput();
        animateWorld(gameState.World);

        gameRenderer.render(gameState);
        setTimeout(function () {
            receiveInput();
        }, 10);
    }

    function joinGame() {
        $("#joinDiv").hide();
        $("#gameCanvas").show();
        var playerName = $("#playerName").val();
        //hub.server.playerJoined(playerName);
        gameInput.startCapturing();
        gameRenderer.render(gameState);
        receiveInput();
    }

    // fulkod
    $("#playerName").bind("keydown", function (event) {
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) {
            joinGame();
            return false;
        } else {
            return true;
        }
    });

    $("#gameCanvas").hide();
    $("#playerName").focus();

    $("#joinButton").click(function () {
        joinGame();
    });



});