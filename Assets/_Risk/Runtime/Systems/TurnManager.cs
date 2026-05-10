using MyUtils.DependencyValidator;
using Risk.Runtime.GameState;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameStateModel _gameStateModel;

    public enum TurnPhase
    {
        None,
        InitialReinforcementPhase,
        ReinforcementPhase,
        AttackPhase,
        InvadePhase,
        FortificationPhase
    }
    
    public TurnPhase CurrentPhase = TurnPhase.None;
    public string CurrentPlayer; 
    
    private void Awake()
    {
        _gameStateModel = GetComponent<GameStateModel>();
        DependencyValidator.ComponentExist(_gameStateModel, this);
    }

    private void OnEnable()
    {
        _gameStateModel.TurnStateUpdated += OnTurnStateUpdated;
    }

    private void OnTurnStateUpdated(TurnState turn)
    {
        CurrentPhase = turn.CurrentPhase switch
        {
            "initial_reinforcement_phase" => TurnPhase.InitialReinforcementPhase,
            "reinforcement_phase" => TurnPhase.ReinforcementPhase,
            "attack_phase" => TurnPhase.AttackPhase,
            "invade_phase" => TurnPhase.InvadePhase,
            "fortification_phase" => TurnPhase.FortificationPhase,
            _ => TurnPhase.None
        };
        
        CurrentPlayer = turn.CurrentPlayer;
    }
}
