function GameInput(window) {

    var gameInput = {};

    gameInput.key = {
        LEFT: 37,
        UP: 38,
        RIGHT: 39,
        DOWN: 40,
        SPACE: 32
};

    gameInput.keys = {};

    gameInput.startCapturing = function() {
        window.addEventListener("keydown",
            function (e) {
                gameInput.keys[e.keyCode] = true;
                switch (e.keyCode) {
                    case 37: case 39: case 38: case 40: // Arrow keys
                    case 32: e.preventDefault(); break; // Space
                    default: break; // do not block other keys
                }
            },
            false);

        window.addEventListener('keyup',
            function (e) {
                gameInput.keys[e.keyCode] = false;
            },
            false);
    };


    return gameInput;

}