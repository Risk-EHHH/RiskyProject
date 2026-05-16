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
            
            //reinforce mock, in the future there will be an ActionManager deciding which actions to take based on player interaction with UI
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))  
            {
                Reinforce(_gameStateModel.TurnState.CurrentPlayer, "Alaska", 3);
            }
        }

        #endregion
        // done only at the beginning
        private async Task InitializeGameAsync()
        {
            Debug.Log("Initializing game...");

            // initialize game on the backend
            Game gameDto = await _backendManager.PostNewGame(_playerNames);
            _gameStateModel.Game = GameStateMapper.ToGameState(gameDto);
            
            // retrieve initial game state from the backend
            BoardInfo boardDto = await _backendManager.GetBoardInfo();
            _gameStateModel.Board = GameStateMapper.ToBoardState(boardDto);

            Dictionary<string, PlayerInfo> playersDto = await _backendManager.GetPlayersInfo();
            _gameStateModel.Players = GameStateMapper.ToPlayerStates(playersDto);

            SecretPlayerInfo secretPlayerDto = await _backendManager.GetSecretPlayerInfo(_gameStateModel.Players[0].Id); // for now just the first player
            _gameStateModel.SecretPlayer = GameStateMapper.ToSecretPlayerState(secretPlayerDto);
            
            // trigger backend to start the game with the phase loop
            _isGameStarted = await _backendManager.PostStartGame();
            
            FetchTurnState();
        }

        /// <summary>
        /// Handles the action performed event by fetching the result of the specified action and updating the game state if the action succeeds.
        /// </summary>
        /// <param name="actionId">The unique identifier of the action that was performed.</param>
        private async void OnActionPerformed(string actionId)
        {
            ActionResult actionResult = await _backendManager.GetActionResult(actionId);
            Debug.Log($"Action {actionId} result: {actionResult.ActionStatus}");
            if (actionResult.ActionStatus != "SUCCESS") return;
            Debug.Log($"Action {actionId} succeeded!");
            FetchTurnState();
            OnTurnPhaseChanged();
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


        /// <summary>
        /// Fetches the current turn state from the backend, updating the model and consequently the <see cref="TurnManager"/>.
        /// </summary>
        private async void FetchTurnState()
        {
            TurnInfo turnDto = await _backendManager.GetTurnInfo();
            _gameStateModel.TurnState = GameStateMapper.ToTurnState(turnDto);
        }

        // --------------- Actions --------------- (in the future these could be done by the ActionManager)
        
        public async void Reinforce(string playerId, string territoryName, int armies)
        {
            PlayerAction playerAction = await _backendManager.PostReinforce(playerId, territoryName, armies);
            ActionPerformed?.Invoke(playerAction.ActionId);
        }
    }
}