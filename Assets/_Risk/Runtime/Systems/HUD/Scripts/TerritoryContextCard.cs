using MyUtils.DependencyValidator;
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

        [SerializeField] private bool _hasActions = false;
        [SerializeField] private GameObject _cardActions;

        #region MonoBehaviour
        private void Awake()
        {
            DependencyValidator.NotNull(_territoryName, this);
            DependencyValidator.NotNull(_troopsNumber, this);
            DependencyValidator.NotNull(_territoryOwner, this);
            if (_hasActions)
                DependencyValidator.NotNull(_cardActions, this);
        }
        #endregion

        public struct TerritoryDisplayData
        {
            public string Name;
            public int TroopCount;
            public string OwnerName;
            public Vector2 ScreenPosition;
        }

        public void Set(TerritoryDisplayData data)
        {
            SetCardPosition(data.ScreenPosition);
            SetTerritoryData(data.Name, data.TroopCount, data.OwnerName);
            ToggleCard(true);
        }

        public void ToggleCard(bool show)
        {
            gameObject.SetActive(show);
        }

        public void ToggleActions(bool show)
        {
            if (!_hasActions) return;
            _cardActions.SetActive(show);
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
}
