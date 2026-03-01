using System;
using System.Collections.Generic;
using Risk.Runtime.BackendCommunication;
using Risk.Runtime.Input;
using Risk.Runtime.Utils;
using UnityEngine;


namespace Risk.Runtime.GameBoard
{
    /// <summary>
    /// Represents the visual component of the game board in the Risk game runtime.
    /// This class is responsible for handling the rendering and display-related logic
    /// of the game board elements.
    /// </summary>
    public class GameBoardView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoardInputManager _boardInputManager;
        [SerializeField] private GameStateModel _gameStateModel;
        [SerializeField] private List<BoardContinent> _boardContinents;

        private Dictionary<string, BoardTerritory> _territoryViews = new();

        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_gameStateModel, this);
            DependencyValidator.ListNotNull(_boardContinents, this);
        }

        private void OnEnable()
        {
            _gameStateModel.GameInfoUpdated += OnGameInfoUpdated;
            _gameStateModel.PlayersUpdated += OnPlayersUpdated;
            
            _boardInputManager.BoardTerritoryClicked += OnBoardTerritoryClicked;
        }
        
        private void OnDisable()
        {
            _gameStateModel.GameInfoUpdated -= OnGameInfoUpdated;
            _gameStateModel.PlayersUpdated -= OnPlayersUpdated;
            
            _boardInputManager.BoardTerritoryClicked -= OnBoardTerritoryClicked;
        }

        private void Start()
        {
            PopulateTerritories();   
        }
        #endregion

        /// <summary>
        /// Initializes and populates the dictionary of territories (_territoryViews)
        /// with normalized territory names as keys and corresponding
        /// <see cref="BoardTerritory"/> instances as values.
        /// </summary>
        /// <remarks>
        /// This method iterates over all <see cref="BoardContinent"/> instances
        /// in the _boardContinents list and processes each <see cref="BoardTerritory"/>.
        /// The territory names are normalized to ensure consistent key formatting.
        /// </remarks>
        private void PopulateTerritories()
        {
            foreach (var boardContinent in _boardContinents)
            {
                foreach (var boardTerritory in boardContinent.BoardTerritories)
                {
                    string normalizedName = NormalizeTerritoryName(boardTerritory.name);
                    _territoryViews.Add(normalizedName, boardTerritory);
                }
            }
        }

        /// <summary>
        /// Handles updates to the game state by processing the updated game information.
        /// </summary>
        /// <param name="gameInfo">The updated <see cref="GameInfo"/> instance containing
        /// the current game state, including territory and player data.</param>
        private void OnGameInfoUpdated(GameInfo gameInfo)
        {
            UpdateTerritoriesTroops(gameInfo.Territories);
        }

        /// <summary>
        /// Handles updates to the list of players and refreshes the related UI components to
        /// reflect the current game state.
        /// </summary>
        /// <param name="players">A list of <see cref="PlayerInfo"/> objects representing
        /// the updated set of players, including their names, territories, army pools,
        /// and other relevant data.</param>
        private void OnPlayersUpdated(List<PlayerInfo> players)
        {
            //TODO Update players UI visualization
        }

        /// <summary>
        /// Updates the troop count of each territory in the game board view based on the provided game information.
        /// </summary>
        /// <param name="gameInfoTerritories">A list of <see cref="Territory"/> objects representing the current state
        /// of territories, including their troop counts and other data.</param>
        /// <remarks>
        /// This method iterates through the provided list of territories, normalizes their names using
        /// <see cref="NormalizeTerritoryName(string)"/>, and updates the corresponding troop counts in
        /// the <see cref="BoardTerritory"/> views managed by the game board. Only territories that have
        /// matching keys in the local dictionary are updated.
        /// </remarks>
        private void UpdateTerritoriesTroops(List<Territory> gameInfoTerritories)
        {
            foreach (Territory territory in gameInfoTerritories)
            {
                string backendName = NormalizeTerritoryName(territory.Name);
                if (_territoryViews.TryGetValue(backendName, out var territoryView))
                {
                    territoryView.TroopCount = territory.Armies;
                }
            }
        }

        /// <summary>
        /// Normalizes the given territory name by removing all spaces and converting it to lowercase.
        /// </summary>
        /// <param name="territoryName"></param>
        /// <returns></returns>
        private static string NormalizeTerritoryName(string territoryName)
        {
            return territoryName.Replace(" ", "").ToLower();
        }
        
        private void OnBoardTerritoryHovered(BoardTerritory hoveredTerritory)
        {
            hoveredTerritory.HoverTerritory(true);
            foreach (var boardContinent in _boardContinents)
            {
                foreach (var boardTerritory in boardContinent.BoardTerritories)
                {
                    if (boardTerritory != hoveredTerritory)
                        boardTerritory.HoverTerritory(false);
                }
            }
        }
        
        private void OnBoardTerritoryClicked(BoardTerritory selectedTerritory)
        {
            selectedTerritory.SelectTerritory(true);
            foreach (var boardContinent in _boardContinents)
            {
                foreach (var boardTerritory in boardContinent.BoardTerritories)
                {
                    if (boardTerritory != selectedTerritory)
                        boardTerritory.SelectTerritory(false);
                }
            }
        }
        
    }
}
