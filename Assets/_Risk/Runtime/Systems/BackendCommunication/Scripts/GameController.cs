using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyUtils.DependencyValidator;
using UnityEngine;

namespace Risk.Runtime.BackendCommunication
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<string> _playerNames = new List<string>();
        
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
            {
                MockChangeTurn();
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                MockChangeTurnPhase();
            }
            
        }

        private async Task InitializeGameAsync()
        {
            Debug.Log("Initializing game...");
            
            
            NewGameMetadata newGameMetadata = await _backendManager.PostNewGame(_playerNames);
            _gameStateModel.NewGameMetadata = newGameMetadata;
            _gameStateModel.BoardInfo = await _backendManager.GetBoardInfo();
            _gameStateModel.PlayersInfo = await _backendManager.GetPlayersInfo();
        }

        private void MockChangeTurn()
        {
            //TODO Mock change turn
            
        }

        private async void MockChangeTurnPhase()
        {
            _gameStateModel.TerritoriesInfo = await _backendManager.GetTerritoriesInfo();
            //TODO Mock change turn phase
            
            // (at some point the change of phase will trigger a change turn)
        }
    }
}


