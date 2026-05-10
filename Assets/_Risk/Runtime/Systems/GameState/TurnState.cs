using System;

namespace Risk.Runtime.GameState
{
    [Serializable]
    public class TurnState
    {
        public string CurrentPlayer;
        public string CurrentPhase;
    }
}