using System.Collections.Generic;
using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using UnityEngine;

namespace Risk.Runtime.HUD
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private GameStateModel _gameStateModel;
        [SerializeField] private TopBarManager _topBarManager;

        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_gameStateModel, this);
            DependencyValidator.NotNull(_topBarManager, this);
        }

        private void OnEnable()
        {
            _gameStateModel.PlayersInitialized += OnPlayersInitialized;
            _gameStateModel.PlayersUpdated += OnPlayersUpdated;
            _gameStateModel.TurnStateUpdated += OnTurnStateUpdated;
        }

        private void OnDisable()
        {
            _gameStateModel.PlayersInitialized -= OnPlayersInitialized;
            _gameStateModel.PlayersUpdated -= OnPlayersUpdated;
            _gameStateModel.TurnStateUpdated -= OnTurnStateUpdated;
            
        }

        #endregion

        private void OnPlayersInitialized(List<PlayerState> players) =>
            _topBarManager.InitializePlayers(players);

        private void OnPlayersUpdated(List<PlayerState> players) =>
            _topBarManager.UpdatePlayers(players);
        
        private void OnTurnStateUpdated(TurnState turn)
        {
            _topBarManager.HighlightPlayer(turn.CurrentPlayer);
        }
    }
}