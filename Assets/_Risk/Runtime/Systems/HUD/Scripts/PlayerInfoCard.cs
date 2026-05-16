using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using Risk.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Risk.Runtime.HUD
{
    public class PlayerInfoCard : MonoBehaviour
    {
        public string PlayerId;
        
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _playerArmyPool;
        [SerializeField] private Image _cardBackground;
        [SerializeField] private Image _highlightedBackground;
        
        
        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_playerName, this);
            DependencyValidator.NotNull(_playerArmyPool, this);
            DependencyValidator.NotNull(_cardBackground, this);
            DependencyValidator.NotNull(_highlightedBackground, this);
        }

        #endregion


        public void UpdatePlayerInfo(PlayerState player)
        {
            PlayerId = player.Id;
            _playerName.text = player.Name;
            _playerArmyPool.text = $"Troops: {player.ArmyPool.ToString()}";
            _cardBackground.color = ColorUtils.FromColorName(player.Color);
        }
        
        public void Highlight(bool isHighlighted = false)
        {
            _highlightedBackground.enabled = isHighlighted;
        }
    }
}
