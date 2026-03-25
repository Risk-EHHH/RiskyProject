using System.Collections.Generic;
using MyUtils.DependencyValidator;
using Risk.Runtime.BackendCommunication;
using UnityEngine;

namespace Risk.Runtime.HUD
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] GameStateModel _gameStateModel;
        [SerializeField] TopBarManager _topBarManager;

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

        private void OnPlayersUpdated(List<PlayerInfo> playerInfos)
        {
            foreach (var playerInfo in playerInfos)
            {
                _topBarManager.AddPlayerInfo(playerInfo);
            }
        }
    }
}