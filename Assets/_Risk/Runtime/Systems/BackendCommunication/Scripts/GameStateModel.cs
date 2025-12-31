using UnityEngine;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// This class is responsible for local mirroring of the current Game Data from the web Python backend.
    /// This class is the Model of MVC pattern to sync the UI with the Game State.
    /// The data present in this class needs to be updated and synced with the backend every time the backend gets updated 
    /// </summary>
    public class GameStateModel : MonoBehaviour
    {
        [SerializeField] private GameInfo _gameInfo;

        public GameInfo GameInfo
        {
            get => _gameInfo;
            set
            {
                _gameInfo = value;
                Debug.Log($"[GameStateModel] Game Info Updated: {_gameInfo}");
            }
        }
        
        
    }
    
}
