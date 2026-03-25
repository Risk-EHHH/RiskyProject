using MyUtils.DependencyValidator;
using Risk.Runtime.BackendCommunication;
using Risk.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Risk.Runtime.HUD
{
    public class PlayerInfoCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _playerArmyPool;
        [SerializeField] private Image _cardBackground;
            
        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_playerName, this);
            DependencyValidator.NotNull(_playerArmyPool, this);
            
            _cardBackground = GetComponent<Image>();
            DependencyValidator.ComponentExist(_cardBackground, this);
        }

        #endregion


        public void UpdatePlayerInfo(PlayerInfo playerInfo)
        {
            _playerName.text = playerInfo.Name;
            _playerArmyPool.text = $"Troops: {playerInfo.ArmyPool.ToString()}";
            _cardBackground.color = ColorUtils.FromColorName(playerInfo.Color);
        }
    }
}
