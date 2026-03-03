using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Risk.Runtime.GameBoard
{
    public class BoardContinent : MonoBehaviour
    {
        [SerializeField] private Color _continentColor;
        
        private List<BoardTerritory> _boardTerritories;

        public List<BoardTerritory> BoardTerritories => _boardTerritories;

        private void OnValidate()
        {
            _boardTerritories = GetComponentsInChildren<BoardTerritory>().ToList();
            
            
            foreach (var territory in BoardTerritories)
            {
                territory.SetOriginalColor(_continentColor);
            }
        }
    }
}