function GameRenderer(canvas)
{
    var context = canvas.getContext("2d");

    function renderShape(points, position, rotation) {

        // first save the untranslated/unrotated context
        context.save();
        context.translate(position.X, position.Y);
        context.rotate(rotation * Math.PI / 180);
        context.beginPath();
        context.moveTo(points[0].X, points[0].Y);
        for (var i = 1; i < points.length; i++) {
            context.lineTo(points[i].X, points[i].Y);
        }
        context.closePath();
        context.fillStyle = "rgb(72,72,72)";
        context.stroke();
        //context.fill();


        // restore the context to its untranslated/unrotated state
        context.restore();
    }


    function renderGameObject(object) {

        if (object.Name) {
            context.font = "20px Georgia";
            context.fillText(object.Name, object.Position.X, object.Position.Y - 10);
        }

        renderShape(object.Shape, object.Position, object.Rotation);
    }

    function renderWorld(world) {
        context.clearRect(0, 0, canvas.width, canvas.height);
        _.each(world.GameObjects, function (object) {
            renderGameObject(object);
        });
    }

    var gameRenderer = {};

    gameRenderer.render = function (gameState) {
        renderWorld(gameState.World);
    }

    return gameRenderer;

};