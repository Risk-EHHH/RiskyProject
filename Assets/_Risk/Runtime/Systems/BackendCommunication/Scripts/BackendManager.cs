using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Manages backend communication for game-related data retrieval and updates.
    /// </summary>
    public class BackendManager : MonoBehaviour
    {
        [SerializeField] private string _defaultLocalURL = "http://127.0.0.1:8000"; // default url for local testing
        [SerializeField] private string _gameId = "000"; // default game id, game id logic is currently missing on backend

        private GameStateModel _gameStateModel;

        #region MonoBehaviour

        private void Awake()
        {
            _gameStateModel = GetComponent<GameStateModel>();
            Debug.Assert(_gameStateModel != null, "GameStateModel component not found.");
            if (_gameStateModel == null)
            {
                throw new Exception("GameStateModel component not found.");
            }
        }

        private void Update()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame) // F1 to call GetGameInfo, used for testing
            {
                StartCoroutine(GetGameInfo());
            }
        }
        

        #endregion
        
        /// <summary>
        /// Retrieves game info from backend.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetGameInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/info";

            using UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"GetGameInfo failed: {webRequest.error}");
                yield break;
            }
            
            string json = webRequest.downloadHandler.text;
            Debug.Log($"GetGameInfo response: {json}");
            
            // TODO deserialize json into GameInfo object
            _gameStateModel.GameInfo = JsonConvert.DeserializeObject<GameInfo>(json);
        }
    }
}