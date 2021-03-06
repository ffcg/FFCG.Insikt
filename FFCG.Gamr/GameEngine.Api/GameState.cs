﻿using System.Collections.Generic;

namespace GameEngine.Messages
{
    public class GameState
    {
        public List<PlayerState> Players { get; set; }
        public WorldState World { get; set; }
    }

    public class WorldState
    {
        public List<GameObject> GameObjects { get; set; }
    }
}
