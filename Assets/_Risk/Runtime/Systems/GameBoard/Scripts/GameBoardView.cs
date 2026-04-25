using System.Collections.Generic;
using MyUtils.DependencyValidator;
using Risk.Runtime.BackendCommunication;
using Risk.Runtime.HUD;
using Risk.Runtime.Input;
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
        [SerializeField] private TerritoryContextCard _selectedTerritoryContextCard;
        [SerializeField] private TerritoryContextCard _hoveredTerritoryContextCard;
        
        private readonly Dictionary<string, BoardTerritory> _territoryViews = new();
        private BoardTerritory _lastSelectedTerritory;
        private BoardTerritory _lastHoveredTerritory;
        
        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_gameStateModel, this);
            DependencyValidator.ListNotNull(_boardContinents, this);
        }

        private void OnEnable()
        {
            _gameStateModel.BoardInfoUpdated += OnBoardInfoUpdated;
            _gameStateModel.TerritoriesStatesUpdated += OnTerritoriesStatesUpdated;
            
            _boardInputManager.BoardTerritoryClicked += OnBoardTerritoryClicked;
            _boardInputManager.BoardTerritoryHovered += OnBoardTerritoryHovered;
            _boardInputManager.BoardTerritoryHoverExited += OnBoardTerritoryHoverExited;
        }
        
        private void OnDisable()
        {
            _gameStateModel.BoardInfoUpdated -= OnBoardInfoUpdated;
            _gameStateModel.TerritoriesStatesUpdated -= OnTerritoriesStatesUpdated;
            
            
            _boardInputManager.BoardTerritoryClicked -= OnBoardTerritoryClicked;
            _boardInputManager.BoardTerritoryHovered -= OnBoardTerritoryHovered;
            _boardInputManager.BoardTerritoryHoverExited -= OnBoardTerritoryHoverExited;
            
        }

        private void Start()
        {
            PopulateTerritories();   
        }
        #endregion

        /// <summary>
        /// Initializes and populates the dictionary of board territories (_territoryViews)
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

        // Called only at the beginning of the game
        private void OnBoardInfoUpdated(BoardInfo boardInfo)
        {
            foreach (Continent continent in boardInfo.Continents)
            {
                foreach (Territory territory in continent.Territories)
                {
                    string normalizedName = NormalizeTerritoryName(territory.Name);
                    if (_territoryViews.TryGetValue(normalizedName, out BoardTerritory territoryView))
                    {
                        territoryView.TroopCount = territory.Armies;
                        territoryView.OwnerId = territory.Owner;
                    }
                }
            }
        }
        
        // Called every turn
        private void OnTerritoriesStatesUpdated(Dictionary<string, TerritoryInfo> territoriesStates)
        {
            foreach ((string territoryName, TerritoryInfo state) in territoriesStates)
            {
                string normalizedName = NormalizeTerritoryName(territoryName);
                if (_territoryViews.TryGetValue(normalizedName, out BoardTerritory territoryView))
                {
                    territoryView.TroopCount = state.Armies;
                    territoryView.OwnerId = state.Owner;
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
            hoveredTerritory.IsHovered = true;
            if (_lastHoveredTerritory != null && _lastHoveredTerritory != hoveredTerritory)
            {
                _lastHoveredTerritory.IsHovered = false;
            }
            _lastHoveredTerritory = hoveredTerritory;
            
            if (hoveredTerritory == _lastSelectedTerritory)
            {
                _hoveredTerritoryContextCard.ToggleCard(false);
                return;
            }

            TerritoryContextCard.TerritoryDisplayData data = new()
            {
                Name = hoveredTerritory.TerritoryName,
                OwnerName = hoveredTerritory.OwnerId,
                TroopCount = hoveredTerritory.TroopCount,
                ScreenPosition = Camera.main.WorldToScreenPoint(hoveredTerritory.transform.position)
            };
            _hoveredTerritoryContextCard.Set(data);
        }

        private void OnBoardTerritoryHoverExited()
        {
            //if (_lastSelectedTerritory != null) return;
            
            if (_lastHoveredTerritory != null)
            {
                _lastHoveredTerritory.IsHovered = false;
                _lastHoveredTerritory = null;
            }
            _hoveredTerritoryContextCard.ToggleCard(false);
        }

        /// <summary>
        /// Handles the logic that occurs when a territory on the game board is clicked by the player.
        /// This method updates the selection state of the clicked territory while ensuring that any previously
        /// selected territory is deselected. Additionally, this method updates the display of the territory
        /// context card with information about the clicked territory and enables the associated actions.
        /// </summary>
        /// <param name="selectedTerritory">The <see cref="BoardTerritory"/> instance representing the
        /// territory that was clicked by the player.</param>
        private void OnBoardTerritoryClicked(BoardTerritory selectedTerritory)
        {
            selectedTerritory.IsSelected = true;
            if (_lastSelectedTerritory != null && _lastSelectedTerritory != selectedTerritory)
            {
                _lastSelectedTerritory.IsSelected = false;
            }
            _lastSelectedTerritory = selectedTerritory;
            

            UpdateTerritoryContextCard(selectedTerritory);
            
            _selectedTerritoryContextCard.ToggleActions(true);
            _hoveredTerritoryContextCard.ToggleCard(false);
            
        }

        private void UpdateTerritoryContextCard(BoardTerritory territory)
        {
            TerritoryContextCard.TerritoryDisplayData data = new()
            {
                Name = territory.TerritoryName,
                OwnerName = territory.OwnerId,
                TroopCount = territory.TroopCount,
                ScreenPosition = Camera.main.WorldToScreenPoint(territory.transform.position)
            };
            _selectedTerritoryContextCard.Set(data);
        }
    }
}
