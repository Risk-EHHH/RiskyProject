using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Risk.Runtime.BackendCommunication
{
    public class GameController : MonoBehaviour
    {
        private BackendManager _backendManager;
        private GameStateModel _gameStateModel;

        private void Awake()
        {
            _backendManager = GetComponent<BackendManager>();
            _gameStateModel = GetComponent<GameStateModel>();
            
            Debug.Assert(_backendManager != null, "BackendManager component not found!");
            Debug.Assert(_gameStateModel != null, "GameStateModel component not found!");
        }

        private async void Start()
        {
            await InitializeGameAsync();
        }

        private async Task InitializeGameAsync()
        {
            Debug.Log("Initializing game...");
        
            (GameInfo gameInfo, var players) = await _backendManager.GetInitialDataAsync();
        
            if (gameInfo != null && players != null)
            {
                _gameStateModel.GameInfo = gameInfo;
                _gameStateModel.Players = players;
                Debug.Log($"Game initialized! {gameInfo.Territories.Count} territories, {players.Count} players");
            }
        }
    }
}


