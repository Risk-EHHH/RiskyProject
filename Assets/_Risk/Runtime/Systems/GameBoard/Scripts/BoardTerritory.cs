using System;
using Risk.Runtime.Utils;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Risk.Runtime.GameBoard
{
    public class BoardTerritory : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool _showName;
        
        [Header("Data")] 
        [SerializeField] private string _territoryName;
        
        [Header("References")] 
        [SerializeField] private TMP_Text _nameTMPText;
        [SerializeField] private TMP_Text _troopsNumberTMPText;
        [SerializeField] private SpriteRenderer _territorySprite;
        [SerializeField] private SpriteRenderer _territorySpriteSelected;
        [SerializeField] private SpriteRenderer _troopsNumberBackGroundSprite;
        
        private int _troopCount = 0;
        private string _ownerId;
        
        public int TroopCount
        {
            get => _troopCount;
            set
            {
                _troopCount = value;
                _troopsNumberTMPText.text = _troopCount.ToString();
            }
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _nameTMPText.gameObject.SetActive(_showName);

            if (!_showName) return;
            _territoryName = gameObject.transform.name;
            _nameTMPText.text = _territoryName;
        }

        private void Awake()
        {
            DependencyValidator.NotNull(_territorySprite, this);
            DependencyValidator.NotNull(_troopsNumberBackGroundSprite, this);
            DependencyValidator.NotNull(_troopsNumberTMPText, this);
            DependencyValidator.NotNull(_nameTMPText, this);
        }

        #endregion

        /// <summary>
        /// Sets the color of the territory based on <see cref="BoardContinent"/>
        /// </summary>
        /// <param name="color"></param>
        public void SetTerritoryColor(Color color)
        {
            _territorySprite.color = color;
        }

        public void HoverTerritory(bool isHovered)
        {
            _territorySpriteSelected.color = Color.softYellow;
            _territorySpriteSelected.enabled = isHovered; 
            
        }
        
        public void SelectTerritory(bool isSelected)
        {
            _territorySpriteSelected.color = Color.yellowNice;  
            _territorySpriteSelected.enabled = isSelected;  
        }

    }
}
