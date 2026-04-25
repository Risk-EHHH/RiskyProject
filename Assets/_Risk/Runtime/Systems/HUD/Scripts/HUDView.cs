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
            _gameStateModel.PlayersInfoUpdated += OnPlayersInfoUpdated;
        }
        
        private void OnDisable()
        {
            _gameStateModel.PlayersInfoUpdated -= OnPlayersInfoUpdated;
        }

        #endregion

        private void OnPlayersInfoUpdated(Dictionary<string, PlayerInfo> playerInfos)
        {
            // foreach (var playerInfo in playerInfos)
            // {
            //     _topBarManager.AddPlayerInfo(playerInfo);
            // }
            
            foreach (var playerInfo in playerInfos.Values)
            {
                _topBarManager.AddPlayerInfo(playerInfo);
            }
        }
    }
}