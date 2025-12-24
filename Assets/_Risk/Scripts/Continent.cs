using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Risk
{
    public class Continent : MonoBehaviour
    {
        [SerializeField] private Color _continentColor;
        
        private List<Territory> _territories;
        
        private void OnValidate()
        {
            _territories = GetComponentsInChildren<Territory>().ToList();
            
            
            foreach (var territory in _territories)
            {
                territory.SetSpriteColor(_continentColor);
            }
        }
    }
}