using System.Collections.Generic;
using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using Risk.Runtime.HUD;
using Risk.Runtime.Input;
using UnityEngine;

namespace Risk.Runtime.GameBoard
{
    public class GameBoardView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoardInputManager _boardInputManager;
        [SerializeField] private GameStateModel _gameStateModel;
        [SerializeField] private List<BoardContinent> _boardContinents;
        [SerializeField] private TerritoryContextCard _selectedTerritoryContextCard;
        [SerializeField] private TerritoryContextCard _hoveredTerritoryContextCard;
        
        private readonly Dictionary<string, BoardTerritory> _territoryViews = new();
        private readonly Dictionary<string, string> _playerNames = new();
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
            _gameStateModel.BoardUpdated += OnBoardUpdated;
            _gameStateModel.TerritoriesUpdated += OnTerritoriesUpdated;

            _gameStateModel.PlayersInitialized += OnPlayersInitialized;
            _gameStateModel.PlayersUpdated     += OnPlayersUpdated;
            
            _boardInputManager.BoardTerritoryClicked += OnBoardTerritoryClicked;
            _boardInputManager.BoardTerritoryHovered += OnBoardTerritoryHovered;
            _boardInputManager.BoardTerritoryHoverExited += OnBoardTerritoryHoverExited;
        }
        
        private void OnDisable()
        {
            _gameStateModel.BoardUpdated -= OnBoardUpdated;
            _gameStateModel.TerritoriesUpdated -= OnTerritoriesUpdated;
            
            _gameStateModel.PlayersInitialized -= OnPlayersInitialized;
            _gameStateModel.PlayersUpdated     -= OnPlayersUpdated;
            
            _boardInputManager.BoardTerritoryClicked -= OnBoardTerritoryClicked;
            _boardInputManager.BoardTerritoryHovered -= OnBoardTerritoryHovered;
            _boardInputManager.BoardTerritoryHoverExited -= OnBoardTerritoryHoverExited;
        }

        private void Start()
        {
            PopulateTerritories();   
        }

        #endregion

        private void PopulateTerritories()
        {
            foreach (BoardContinent boardContinent in _boardContinents)
            {
                foreach (BoardTerritory boardTerritory in boardContinent.BoardTerritories)
                {
                    string normalizedName = NormalizeTerritoryName(boardTerritory.name);
                    _territoryViews.Add(normalizedName, boardTerritory);
                }
            }
        }

        // Called once at game start
        private void OnBoardUpdated(BoardState boardState)
        {
            foreach (ContinentState continent in boardState.Continents)
            {
                foreach (TerritorySetupState territory in continent.Territories)
                {
                    string normalizedName = NormalizeTerritoryName(territory.Name);
                    if (_territoryViews.TryGetValue(normalizedName, out BoardTerritory territoryView))
                    {
                        territoryView.TroopCount = territory.Armies;
                        territoryView.OwnerId = territory.Owner;
                        territoryView.OwnerName = _playerNames.GetValueOrDefault(territory.Owner, "Unknown");
                    }
                }
            }
        }

        // Called every turn
        private void OnTerritoriesUpdated(List<TerritoryState> territories)
        {
            foreach (TerritoryState territory in territories)
            {
                string normalizedName = NormalizeTerritoryName(territory.Name);
                if (_territoryViews.TryGetValue(normalizedName, out BoardTerritory territoryView))
                {
                    territoryView.TroopCount = territory.Armies;
                    territoryView.OwnerId = territory.Owner;
                    territoryView.OwnerName = _playerNames.GetValueOrDefault(territory.Owner, "Unknown");
                }
            }
        }

        private void OnPlayersInitialized(List<PlayerState> players) => RebuildPlayerNames(players);
        private void OnPlayersUpdated(List<PlayerState> players)     => RebuildPlayerNames(players);
        
        private void RebuildPlayerNames(List<PlayerState> players)
        {
            _playerNames.Clear();
            foreach (PlayerState player in players)
                _playerNames[player.Id] = player.Name;
        }
        
        private static string NormalizeTerritoryName(string territoryName)
        {
            return territoryName.Replace(" ", "").ToLower();
        }
        
        private void OnBoardTerritoryHovered(BoardTerritory hoveredTerritory)
        {
            hoveredTerritory.IsHovered = true;
            if (_lastHoveredTerritory != null && _lastHoveredTerritory != hoveredTerritory)
                _lastHoveredTerritory.IsHovered = false;

            _lastHoveredTerritory = hoveredTerritory;
            
            if (hoveredTerritory == _lastSelectedTerritory)
            {
                _hoveredTerritoryContextCard.ToggleCard(false);
                return;
            }

            _hoveredTerritoryContextCard.Set(new TerritoryContextCard.TerritoryDisplayData
            {
                Name = hoveredTerritory.TerritoryName,
                OwnerName = hoveredTerritory.OwnerName,
                TroopCount = hoveredTerritory.TroopCount,
                ScreenPosition = Camera.main.WorldToScreenPoint(hoveredTerritory.transform.position)
            });
        }

        private void OnBoardTerritoryHoverExited()
        {
            if (_lastHoveredTerritory != null)
            {
                _lastHoveredTerritory.IsHovered = false;
                _lastHoveredTerritory = null;
            }
            _hoveredTerritoryContextCard.ToggleCard(false);
        }

        private void OnBoardTerritoryClicked(BoardTerritory selectedTerritory)
        {
            selectedTerritory.IsSelected = true;
            if (_lastSelectedTerritory != null && _lastSelectedTerritory != selectedTerritory)
                _lastSelectedTerritory.IsSelected = false;

            _lastSelectedTerritory = selectedTerritory;

            _selectedTerritoryContextCard.Set(new TerritoryContextCard.TerritoryDisplayData
            {
                Name = selectedTerritory.TerritoryName,
                OwnerName = selectedTerritory.OwnerName,
                TroopCount = selectedTerritory.TroopCount,
                ScreenPosition = Camera.main.WorldToScreenPoint(selectedTerritory.transform.position)
            });
            _selectedTerritoryContextCard.ToggleActions(true);
            _hoveredTerritoryContextCard.ToggleCard(false);
        }
    }
}