using System.Threading.Tasks;
using MyUtils.DependencyValidator;
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
            DependencyValidator.ComponentExist(_backendManager, this);
            
            _gameStateModel = GetComponent<GameStateModel>();
            DependencyValidator.ComponentExist(_gameStateModel, this);
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


