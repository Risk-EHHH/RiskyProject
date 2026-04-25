using System;
using System.Collections.Generic;
using UnityEngine;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// This class is responsible for local mirroring of the current Game Data from the web Python backend.
    /// This class is the Model of MVC pattern to sync the UI with the Game State.
    /// The data present in this class needs to be updated and synced with the backend every time the backend gets updated 
    /// </summary>
    public class GameStateModel : MonoBehaviour
    {
        public event Action NewGameStarted;
        public event Action<BoardInfo> BoardInfoUpdated;
        public event Action<Dictionary<string, TerritoryInfo>> TerritoriesStatesUpdated;
        public event Action<Dictionary<string, PlayerInfo>> PlayersInfoUpdated;
        
        
        [SerializeField] private NewGameMetadata _newGameMetadata;
        [SerializeField] private BoardInfo _boardInfo;
        
        private Dictionary<string, TerritoryInfo> _territoriesInfo = new();
        private Dictionary<string, PlayerInfo> _playersInfo = new();
        
        public NewGameMetadata NewGameMetadata
        {
            get => _newGameMetadata;
            set
            {
                _newGameMetadata = value;
                NewGameStarted?.Invoke();
                Debug.Log("New Game Started");
            }
        }
        
        public BoardInfo BoardInfo
        {
            get => _boardInfo;
            set
            {
                _boardInfo = value;
                BoardInfoUpdated?.Invoke(_boardInfo);
            }
        }

        public Dictionary<string, TerritoryInfo> TerritoriesInfo
        {
            get => _territoriesInfo;
            set
            {
                _territoriesInfo = value;
                TerritoriesStatesUpdated?.Invoke(_territoriesInfo);
            }
        }
        
        public Dictionary<string, PlayerInfo> PlayersInfo
        {
            get => _playersInfo;
            set
            {
                _playersInfo = value;
                PlayersInfoUpdated?.Invoke(_playersInfo);
            }
        }
    }
    
}
