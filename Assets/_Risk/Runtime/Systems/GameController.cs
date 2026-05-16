using System.Collections.Generic;
using System.Threading.Tasks;
using MyUtils.DependencyValidator;
using Risk.Runtime.BackendCommunication;
using Risk.Runtime.GameState;
using UnityEngine;

namespace Risk.Runtime
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<string> _playerNames = new();

        private BackendManager _backendManager;
        private GameStateModel _gameStateModel;
        private TurnManager _turnManager;
        
        private bool _isGameStarted;
        
        private void Awake()
        {
            _backendManager = GetComponent<BackendManager>();
            DependencyValidator.ComponentExist(_backendManager, this);

            _gameStateModel = GetComponent<GameStateModel>();
            DependencyValidator.ComponentExist(_gameStateModel, this);
            
            _turnManager = GetComponent<TurnManager>();
            DependencyValidator.ComponentExist(_turnManager, this);
        }

        private async void Start()
        {
            await InitializeGameAsync();
        }

        private void Update()
        {
            if (!_isGameStarted) return;
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.N)) // in the future this should happen after the player finishes a turn phase (e.g., hooked to a UI button the plater presses when the turn is over)  
            {
                FetchTurnState();
                if (_turnManager.HasPhaseChanged)
                {
                    //OnTurnPhaseChanged(); //placeholder, turn logic needs to be well thought out when integrating also backend actions
                }
            }
        }

        // done only at the beginning
        private async Task InitializeGameAsync()
        {
            Debug.Log("Initializing game...");

            Game gameDto = await _backendManager.PostNewGame(_playerNames);
            _gameStateModel.Game = GameStateMapper.ToGameState(gameDto);
            
            _isGameStarted = await _backendManager.PostStartGame();

            BoardInfo boardDto = await _backendManager.GetBoardInfo();
            _gameStateModel.Board = GameStateMapper.ToBoardState(boardDto);

            Dictionary<string, PlayerInfo> playersDto = await _backendManager.GetPlayersInfo();
            _gameStateModel.Players = GameStateMapper.ToPlayerStates(playersDto);

            SecretPlayerInfo secretPlayerDto = await _backendManager.GetSecretPlayerInfo(_gameStateModel.Players[0].Id); // for now just the first player
            _gameStateModel.SecretPlayer = GameStateMapper.ToSecretPlayerState(secretPlayerDto);
        }

        // done every time the turn phase changes
        private async void OnTurnPhaseChanged()
        {
            Dictionary<string, TerritoryInfo> territoriesDto = await _backendManager.GetTerritoriesInfo();
            _gameStateModel.Territories = GameStateMapper.ToTerritoryStates(territoriesDto);
            
            Dictionary<string, PlayerInfo> playersDto = await _backendManager.GetPlayersInfo();
            _gameStateModel.Players = GameStateMapper.ToPlayerStates(playersDto);
            
            SecretPlayerInfo secretPlayerDto = await _backendManager.GetSecretPlayerInfo(_gameStateModel.TurnState.CurrentPlayer); // for now just the first player
            _gameStateModel.SecretPlayer = GameStateMapper.ToSecretPlayerState(secretPlayerDto);
        }

        // done every time after the player makes an action
        private async void FetchTurnState()
        {
            TurnInfo turnDto = await _backendManager.GetTurnInfo();
            _gameStateModel.TurnState = GameStateMapper.ToTurnState(turnDto);
        }
    }
}