﻿$(function () {
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

    function sendUserActionToServer(actionName, userAction) {
        // TODO
    }

    function animateGameObject(object) {
        object.Position.X += object.Velocity.X;
        object.Position.Y += object.Velocity.Y;
    }

    function animateWorld(world) {
        _.each(world.GameObjects, function (object) {
            animateGameObject(object);
        });
    }

    function applyUserActionOnClient(actionName, userAction) {
        console.log(actionName);
        var ship = gameState.World.GameObjects[0];
        if (actionName === 'Rotate') {
            ship.Rotation = (ship.Rotation + userAction.Direction * rotateSpeed) % 360;
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
            };
            gameState.World.GameObjects.push(bullet);
        }
    }

    function handleUserAction(actionName, userAction) {
        sendUserActionToServer(actionName, userAction);
        applyUserActionOnClient(actionName, userAction);
    }

    function handleInput() {
        if (gameInput.keys[gameInput.key.LEFT]) {
            handleUserAction('Rotate', { Direction: -1 });
        }
        if (gameInput.keys[gameInput.key.RIGHT]) {
            handleUserAction('Rotate', { Direction: 1 });
        }
        if (gameInput.keys[gameInput.key.UP]) {
            handleUserAction('Thrust', {});
        }
        if (gameInput.keys[gameInput.key.SPACE]) {
            handleUserAction('Fire', {});
        }
    }


    function receiveInput() {
        //console.log('hej');
        handleInput();
        animateWorld(gameState.World);

        gameRenderer.render(gameState);
        setTimeout(function () {
            receiveInput();
        }, 10);
    }

    gameInput.startCapturing();
    gameRenderer.render(gameState);
    receiveInput();


});