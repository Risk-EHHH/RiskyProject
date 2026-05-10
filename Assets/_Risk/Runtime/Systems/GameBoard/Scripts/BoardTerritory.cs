using MyUtils.DependencyValidator;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Risk.Runtime.GameBoard
{
    public class BoardTerritory : MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] private string _territoryName;
        
        [Header("References")] 
        [SerializeField] private TMP_Text _troopsNumberTMPText;
        [SerializeField] private SpriteRenderer _territorySprite;
        [SerializeField] private SpriteRenderer _territorySpriteSelected;
        [SerializeField] private SpriteRenderer _troopsNumberBackGroundSprite;
        [SerializeField] private PolygonCollider2D _territoryCollider;
        
        private string _ownerId;
        private int _troopCount = 0;
        private bool _isHovered;
        private bool _isSelected;
        private bool _isInteractive = true;
        private Color _territoryOriginalColor;
        
        public string TerritoryName => _territoryName;
        public string OwnerId
        {
            get => _ownerId;
            set => _ownerId = value;
        }

        public int TroopCount
        {
            get => _troopCount;
            set
            {
                _troopCount = value;
                _troopsNumberTMPText.text = _troopCount.ToString();
            }
        }
        
        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                _isHovered = value;
                HoverTerritory(value);
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;  
                SelectTerritory(value);
            }
                
        }

        /// <summary>
        /// Gets or sets a value indicating whether the territory is interactive.
        /// When set to false, the territory becomes non-interactive, disabling its collider,
        /// resetting hover and selection states, and applying a dimmed appearance to indicate inactivity.
        /// When set to true, the territory regains its interactivity and original appearance.
        /// </summary>
        public bool IsInteractive
        {
            get => _isInteractive;
            set
            {
                _isInteractive = value;
                _territoryCollider.enabled = value;

                if (!value)
                {
                    SetNotInteractiveColor();
                    IsSelected = false;
                    IsHovered = false;
                }
                else
                {
                    ResetTerritoryColor();
                }
            } 
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            DependencyValidator.NotNull(_territorySprite, this);
            DependencyValidator.NotNull(_troopsNumberBackGroundSprite, this);
            DependencyValidator.NotNull(_troopsNumberTMPText, this);
            
            _territoryName = gameObject.transform.name;
        }
        
        #endregion

        public void SetOriginalColor(Color color)
        {
            SetTerritoryColor(color);
            _territoryOriginalColor = color;
        }
 
        private void SetTerritoryColor(Color color)
        {
            _territorySprite.color = color;
        }

        private void HoverTerritory(bool isHovered)
        {
            if (_isSelected) return;
            
            _territorySpriteSelected.color = Color.softYellow;
            _territorySpriteSelected.enabled = isHovered; 
        }
        
        private void SelectTerritory(bool isSelected)
        {
            _territorySpriteSelected.color = Color.yellowNice;  
            _territorySpriteSelected.enabled = isSelected;  
        }

        private void SetNotInteractiveColor()
        {
            Color current = _territorySprite.color;
            Color.RGBToHSV(current, out float h, out float s, out float v);
            v *= 0.4f; // 40% of original value
            Color notInteractiveColor = Color.HSVToRGB(h, s, v);
            SetTerritoryColor(notInteractiveColor);
        }

        private void ResetTerritoryColor()
        {
            SetTerritoryColor(_territoryOriginalColor);
        }

    }
}
