using System;
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
            _cardBackground.color = FromColorName(playerInfo.Color);
            Debug.Log(_cardBackground.color);
        }
        
        private static Color FromColorName(string colorName)
        {
            colorName = colorName.ToLower();
            switch (colorName)
            {
                case "red": return Color.red;
                case "green": return Color.green;
                case "blue": return Color.blue;
                case "yellow": return Color.yellow;
                case "purple": return Color.magenta;
                case "black": return Color.black;
                default: return Color.white;
            }
        }
    }
}
