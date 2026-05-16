using System;
using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Serializable]
    public enum TurnPhase
    {
        None,
        InitialReinforcementPhase,
        ReinforcementPhase,
        AttackPhase,
        InvadePhase,
        FortificationPhase
    }

    [Serializable]
    public class Turn
    {
        public TurnPhase PreviousPhase = TurnPhase.None;
        public TurnPhase CurrentPhase = TurnPhase.None;
        public string PreviousPlayer; 
        public string CurrentPlayer;
    }
    
    public Turn CurrentTurn;
    
    private GameStateModel _gameStateModel;
    
    private void Awake()
    {
        _gameStateModel = GetComponent<GameStateModel>();
        DependencyValidator.ComponentExist(_gameStateModel, this);
    }

    private void OnEnable()
    {
        _gameStateModel.TurnStateUpdated += OnTurnStateUpdated;
    }

    private void OnDisable()
    {
        _gameStateModel.TurnStateUpdated -= OnTurnStateUpdated;
    }

    private void OnTurnStateUpdated(TurnState turn)
    {
        if (turn.CurrentPlayer != CurrentTurn.CurrentPlayer) //turn has changed
        {
            CurrentTurn.PreviousPlayer = CurrentTurn.CurrentPlayer;
            CurrentTurn.CurrentPlayer = turn.CurrentPlayer;
        }

        TurnPhase newPhase = turn.CurrentPhase switch
        {
            "initial_reinforcement_phase" => TurnPhase.InitialReinforcementPhase,
            "reinforcement_phase"         => TurnPhase.ReinforcementPhase,
            "attack_phase"                => TurnPhase.AttackPhase,
            "invade_phase"                => TurnPhase.InvadePhase,
            "fortification_phase"         => TurnPhase.FortificationPhase,
            _                             => TurnPhase.None
        };

        if (newPhase != CurrentTurn.CurrentPhase)
        {
            CurrentTurn.PreviousPhase = CurrentTurn.CurrentPhase;
            CurrentTurn.CurrentPhase = newPhase;
        }
    }
}
