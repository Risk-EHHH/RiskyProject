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

        private bool _isGameStarted;
        
        private void Awake()
        {
            _backendManager = GetComponent<BackendManager>();
            DependencyValidator.ComponentExist(_backendManager, this);

            _gameStateModel = GetComponent<GameStateModel>();
            DependencyValidator.ComponentExist(_gameStateModel, this);
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
            }
        }

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

        private async void MockChangeTurnPhase()
        {
            Dictionary<string, TerritoryInfo> territoriesDto = await _backendManager.GetTerritoriesInfo();
            _gameStateModel.Territories = GameStateMapper.ToTerritoryStates(territoriesDto);
        }
        
        private async void FetchTurnState()
        {
            TurnInfo turnDto = await _backendManager.GetTurnInfo();
            _gameStateModel.TurnState = GameStateMapper.ToTurnState(turnDto);
        }
    }
}