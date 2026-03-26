using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Manages backend communication for game-related data retrieval and updates.
    /// </summary>
    public class BackendManager : MonoBehaviour
    {
        [SerializeField] private string _defaultLocalURL = "http://127.0.0.1:8000";
        private string _gameId;

        public async Task<NewGameMetadata> PostNewGame(List<string> playerNames)
        {
            string url = $"{_defaultLocalURL}/new_game";
            string playerNamesJson = JsonConvert.SerializeObject(new PlayerNames { Names = playerNames });
            Debug.Log($"Posting new game with {playerNamesJson}");
            using UnityWebRequest request = UnityWebRequest.Post(url, playerNamesJson, "application/json");
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<NewGameMetadata>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"PostNewGame failed: {response.Message}");
                return null;
            }
            
            NewGameMetadata newGameMetadata = response.Metadata;
            _gameId = newGameMetadata.GameID;
            return newGameMetadata;
        }

        private class PlayerNames
        {
            [JsonProperty("player_names")]
            public List<string> Names;
        }

        public async Task<BoardInfoMetadata> GetBoardInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/get_board_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<BoardInfoMetadata>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetBoardInfo failed: {response.Message}");
                return null;
            }
            
            return response.Metadata;
        }
        
        public async Task<Dictionary<string, TerritoryState>> GetTerritoriesInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/get_territories_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);
    
            await request.SendWebRequest();
    
            if (HasError(request))
                Debug.LogError($"Request failed: {request.error}");
    
            var response = JsonConvert.DeserializeObject<ApiResponse<Dictionary<string, TerritoryState>>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetTerritoriesInfo failed: {response.Message}");
                return null;
            }
    
            return response.Metadata;
        }
        
        
        private static bool HasError(UnityWebRequest request) 
            => request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
        
    }
}
