using System.Collections.Generic;
using MyUtils.DependencyValidator;
using Risk.Runtime.GameBoard;
using TMPro;
using UnityEngine;


namespace Risk.Runtime.HUD
{
    public class TerritoryContextCard : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset = new Vector2(20, 20);
        
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _territoryName;
        [SerializeField] private TextMeshProUGUI _troopsNumber;
        [SerializeField] private TextMeshProUGUI _territoryOwner;

        [SerializeField] private bool _isForSelectedTerritory = false;
        [SerializeField] private TerritoryContextCardActions _cardActions;

        #region MonoBehaviour
        private void Awake()
        {
            DependencyValidator.NotNull(_territoryName, this);
            DependencyValidator.NotNull(_troopsNumber, this);
            DependencyValidator.NotNull(_territoryOwner, this);
            if (_isForSelectedTerritory)
                DependencyValidator.NotNull(_cardActions, this);
        }
        #endregion

        public void Set(TerritoryDisplayData data)
        {
            SetCardPosition(data.ScreenPosition);
            SetTerritoryData(data.Name, data.TroopCount, data.OwnerName);
            if (_isForSelectedTerritory)
            {
                _cardActions.CurrentAction = data.AvailableAction;
                ToggleActions(data.AvailableAction != TerritoryAction.None);
            }
            ToggleCard(true);
        }

        public void ToggleCard(bool show)
        {
            gameObject.SetActive(show);
            if (!_isForSelectedTerritory) return;
            if (!show) ToggleActions(false);
        }

        private void ToggleActions(bool show)
        {
            _cardActions.gameObject.SetActive(show);
        }

        private void SetCardPosition(Vector2 dataScreenPosition)
        {
            transform.position = dataScreenPosition + _offset;
        }


        private void SetTerritoryData(string territoryName, int troopsNumber, string territoryOwner)
        {
            _territoryName.text = territoryName;
            _troopsNumber.text = troopsNumber.ToString();
            _territoryOwner.text = territoryOwner;
        }
    }
    
    public struct TerritoryDisplayData
    {
        public string Name;
        public int TroopCount;
        public string OwnerName;
        public Vector2 ScreenPosition;
        public TerritoryAction AvailableAction;
    }
}
