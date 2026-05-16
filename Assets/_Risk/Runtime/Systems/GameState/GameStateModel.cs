using System;
using System.Collections.Generic;
using UnityEngine;

namespace Risk.Runtime.GameState
{
    public class GameStateModel : MonoBehaviour
    {
        public event Action<GameState> GameStarted;
        public event Action<BoardState> BoardUpdated;
        public event Action<List<TerritoryState>> TerritoriesUpdated;
        public event Action<List<PlayerState>> PlayersInitialized;
        public event Action<List<PlayerState>> PlayersUpdated;
        public event Action<SecretPlayerState> SecretPlayerUpdated;
        public event Action<TurnState> TurnStateUpdated;
        
        [SerializeField] private GameState _game;
        [SerializeField] private BoardState _board;
        [SerializeField] private List<TerritoryState> _territories = new();
        [SerializeField] private List<PlayerState> _players = new();
        [SerializeField] private SecretPlayerState _secretPlayer;
        [SerializeField] private TurnState _turnState;
        
        private bool _playersInitialized;
        
        public GameState Game
        {
            get => _game;
            set
            {
                _game = value;
                GameStarted?.Invoke(_game);
            }
        }

        public BoardState Board
        {
            get => _board;
            set
            {
                _board = value;
                BoardUpdated?.Invoke(_board);
            }
        }

        public List<TerritoryState> Territories
        {
            get => _territories;
            set
            {
                _territories = value;
                TerritoriesUpdated?.Invoke(_territories);
            }
        }

        public List<PlayerState> Players
        {
            get => _players;
            set
            {
                _players = value;
                if (!_playersInitialized)
                {
                    _playersInitialized = true;
                    PlayersInitialized?.Invoke(_players);
                }
                else
                {
                    PlayersUpdated?.Invoke(_players);
                }
            }
        }

        public SecretPlayerState SecretPlayer
        {
            get => _secretPlayer;
            set
            {
                _secretPlayer = value;
                SecretPlayerUpdated?.Invoke(_secretPlayer);
            }
        }
        
        public TurnState TurnState
        {
            get => _turnState;
            set
            {
                _turnState = value;
                TurnStateUpdated?.Invoke(_turnState);
            }
        }
    }
}