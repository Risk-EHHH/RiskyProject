using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Risk
{
    public class Territory : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool _showName;
        
        [Header("Data")] 
        [SerializeField] private string _territoryName;

        [Header("References")] 
        [SerializeField] private TMP_Text _nameTMPText;
        [SerializeField] private TMP_Text _troopsNumberTMPText;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private int _troopCount = 0;

        public int TroopCount
        {
            get => _troopCount;
            set
            {
                _troopCount = value;
                _troopsNumberTMPText.text = _troopCount.ToString();
            }
        }

        public void SetSpriteColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        private void OnValidate()
        {
            _nameTMPText.gameObject.SetActive(_showName);

            if (!_showName) return;
            _territoryName = gameObject.transform.name;
            _nameTMPText.text = _territoryName;
        }

        private void Start()
        {
            TroopCount = Random.Range(1, 5);
        }
        
        
    }
}
