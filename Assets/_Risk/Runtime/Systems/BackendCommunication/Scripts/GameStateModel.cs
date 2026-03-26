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
        public event Action<BoardInfoMetadata> BoardInfoUpdated;
        public event Action<Dictionary<string, TerritoryState>> TerritoriesStatesUpdated;
        
        [SerializeField] private NewGameMetadata _newGameMetadata;
        [SerializeField] private BoardInfoMetadata _boardInfoMetadata;
        
        private Dictionary<string, TerritoryState> _territoriesStates = new();
        
        
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
        
        public BoardInfoMetadata BoardInfoMetadata
        {
            get => _boardInfoMetadata;
            set
            {
                _boardInfoMetadata = value;
                BoardInfoUpdated?.Invoke(_boardInfoMetadata);
            }
        }

        public Dictionary<string, TerritoryState> TerritoryStates
        {
            get => _territoriesStates;
            set
            {
                _territoriesStates = value;
                TerritoriesStatesUpdated?.Invoke(_territoriesStates);
            }
        }
        
        
        //----------- OBSOLETE ---------
        // public event Action<GameInfo> GameInfoUpdated;
        // public event Action<List<PlayerInfo>> PlayersUpdated;
        //
        // [SerializeField] private GameInfo _gameInfo;
        // [SerializeField] private List<PlayerInfo> _players;
        //
        //
        // public GameInfo GameInfo
        // {
        //     get => _gameInfo;
        //     set
        //     {
        //         _gameInfo = value;
        //         GameInfoUpdated?.Invoke(_gameInfo);
        //     }
        // }
        //
        //
        // public List<PlayerInfo> Players { 
        //     get => _players;
        //     set
        //     {
        //         _players = value;
        //         PlayersUpdated?.Invoke(_players);
        //     }
        // }
    }
    
}
