using System.Collections.Generic;
using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using UnityEngine;

namespace Risk.Runtime.HUD
{
    public class TopBarManager : MonoBehaviour
    {
        [SerializeField] private PlayerInfoCard _playerInfoCardPrefab;
        
        private List<PlayerInfoCard> _playerInfoCards = new();
        
        #region MonoBehaviour

        private void Awake()
        {
            DependencyValidator.NotNull(_playerInfoCardPrefab, this);
        }
        
        #endregion

        public void InitializePlayers(List<PlayerState> players)
        {
            foreach (PlayerState player in players)
            {
                PlayerInfoCard card = Instantiate(_playerInfoCardPrefab, transform);
                card.UpdatePlayerInfo(player);
                _playerInfoCards.Add(card);
            }
        }

        public void UpdatePlayers(List<PlayerState> players)
        {
            foreach (PlayerState player in players)
            {
                PlayerInfoCard card = _playerInfoCards.Find(c => c.PlayerId == player.Id);
                card?.UpdatePlayerInfo(player);
            }
        }
        
        public void HighlightPlayer(string playerId)
        {
            foreach (PlayerInfoCard card in _playerInfoCards)
            {
                card.Highlight(card.PlayerId == playerId);
            }
        }
    }
}
