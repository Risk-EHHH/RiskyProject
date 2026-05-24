using System;
using Risk.Runtime.GameBoard;
using UnityEngine;

namespace Risk.Runtime.HUD
{
    public class TerritoryContextCardActions : MonoBehaviour
    {
        [SerializeField] private GameObject _reinforceButton;
        [SerializeField] private GameObject _attackButton;
        [SerializeField] private GameObject _fortifyButton;

        private TerritoryAction _currentAction = TerritoryAction.None;

        public TerritoryAction CurrentAction
        {
            get => _currentAction;
            set
            {
                _currentAction = value;
                ToggleAction(_currentAction);
            }
        }
        
        private void ToggleAction(TerritoryAction action)
        {
            _reinforceButton.SetActive(action == TerritoryAction.Reinforce);
            _attackButton.SetActive(action == TerritoryAction.Attack);
            _fortifyButton.SetActive(action == TerritoryAction.Fortify);
        }
    }
}
