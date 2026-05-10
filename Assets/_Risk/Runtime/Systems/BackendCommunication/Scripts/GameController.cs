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
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                MockChangeTurnPhase();
        }

        private async Task InitializeGameAsync()
        {
            Debug.Log("Initializing game...");

            Game gameDto = await _backendManager.PostNewGame(_playerNames);
            _gameStateModel.Game = GameStateMapper.ToGameState(gameDto);

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
    }
}