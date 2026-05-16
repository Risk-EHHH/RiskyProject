using System;
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
        
        public event Action<string> ActionPerformed;

        #region MonoBehaviour
        
        private void Awake()
        {
            _backendManager = GetComponent<BackendManager>();
            DependencyValidator.ComponentExist(_backendManager, this);

            _gameStateModel = GetComponent<GameStateModel>();
            DependencyValidator.ComponentExist(_gameStateModel, this);
            
            _turnManager = GetComponent<TurnManager>();
            DependencyValidator.ComponentExist(_turnManager, this);
        }

        private void OnEnable()
        {
            ActionPerformed += OnActionPerformed;
        }

        private void OnDisable()
        {
            ActionPerformed -= OnActionPerformed;
        }

        private async void Start()
        {
            await InitializeGameAsync();
        }

        private void Update()
        {
            if (!_isGameStarted) return;
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))  
                Reinforce(_gameStateModel.TurnState.CurrentPlayer, "Alaska", 3);
        }

        #endregion

        private async Task InitializeGameAsync()
        {
            Debug.Log("Initializing game...");

            await SyncNewGame();
            await SyncBoard();
            await SyncPlayers();
            await SyncSecretPlayer(_gameStateModel.Players[0].Id);
            
            _isGameStarted = await _backendManager.PostStartGame();
            
            await SyncTurnState();
        }

        private async void OnActionPerformed(string actionId)
        {
            ActionResult actionResult = await _backendManager.GetActionResult(actionId);
            if (actionResult.ActionStatus != "SUCCESS") return;
            
            Debug.Log($"Action {actionId} succeeded!");
            await SyncTurnState();
            await SyncTurnPhase();
        }

        // --------------- Sync methods ---------------

        private async Task SyncNewGame()
        {
            Game gameDto = await _backendManager.PostNewGame(_playerNames);
            _gameStateModel.Game = GameStateMapper.ToGameState(gameDto);
        }

        private async Task SyncBoard()
        {
            BoardInfo boardDto = await _backendManager.GetBoardInfo();
            _gameStateModel.Board = GameStateMapper.ToBoardState(boardDto);
        }

        private async Task SyncPlayers()
        {
            Dictionary<string, PlayerInfo> playersDto = await _backendManager.GetPlayersInfo();
            _gameStateModel.Players = GameStateMapper.ToPlayerStates(playersDto);
        }

        private async Task SyncTerritories()
        {
            Dictionary<string, TerritoryInfo> territoriesDto = await _backendManager.GetTerritoriesInfo();
            _gameStateModel.Territories = GameStateMapper.ToTerritoryStates(territoriesDto);
        }

        private async Task SyncSecretPlayer(string playerId)
        {
            SecretPlayerInfo secretPlayerDto = await _backendManager.GetSecretPlayerInfo(playerId);
            _gameStateModel.SecretPlayer = GameStateMapper.ToSecretPlayerState(secretPlayerDto);
        }

        private async Task SyncTurnState()
        {
            TurnInfo turnDto = await _backendManager.GetTurnInfo();
            _gameStateModel.TurnState = GameStateMapper.ToTurnState(turnDto);
        }

        /// <summary>
        /// Syncs all state that can change each turn phase: territories, players, and the local player's secret info.
        /// </summary>
        private async Task SyncTurnPhase()
        {
            await SyncTerritories();
            await SyncPlayers();
            await SyncSecretPlayer(_gameStateModel.TurnState.CurrentPlayer);
        }

        // --------------- Actions ---------------

        public async void Reinforce(string playerId, string territoryName, int armies)
        {
            PlayerAction playerAction = await _backendManager.PostReinforce(playerId, territoryName, armies);
            ActionPerformed?.Invoke(playerAction.ActionId);
        }
    }
}