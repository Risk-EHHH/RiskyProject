using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using UnityEngine;

namespace Risk.Runtime.HUD
{
    public class TopBarManager : MonoBehaviour
    {
        [SerializeField] private PlayerInfoCard _playerInfoCardPrefab;

        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_playerInfoCardPrefab, this);
        }
        
        #endregion

        public void AddPlayerInfo(PlayerState playerInfo)
        {
            var playerInfoCard = Instantiate(_playerInfoCardPrefab, transform);
            playerInfoCard.UpdatePlayerInfo(playerInfo);
        }
    }
}
