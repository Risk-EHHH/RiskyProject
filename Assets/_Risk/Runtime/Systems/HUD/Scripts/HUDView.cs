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
            _gameStateModel.PlayersUpdated += OnPlayersUpdated;
        }
        
        private void OnDisable()
        {
            _gameStateModel.PlayersUpdated -= OnPlayersUpdated;
        }

        #endregion

        private void OnPlayersUpdated(List<PlayerState> players)
        {
            foreach (PlayerState player in players)
            {
                _topBarManager.AddPlayerInfo(player);
            }
        }
    }
}