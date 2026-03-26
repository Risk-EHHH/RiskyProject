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
            string playerNamesJson = JsonConvert.SerializeObject(new PlayerNames { player_names = playerNames });
            Debug.Log($"Posting new game with {playerNamesJson}");
            using var request = UnityWebRequest.Post(url, playerNamesJson, "application/json");
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<NewGameMetadata>>(request.downloadHandler.text);
            if (response.status != "success")
            {
                Debug.LogError($"PostNewGame failed: {response.message}");
                return null;
            }
            
            NewGameMetadata newGameMetadata = response.metadata;
            _gameId = newGameMetadata.game_id;
            return newGameMetadata;
        }
        
        private class PlayerNames { public List<string> player_names; }
        
        private static bool HasError(UnityWebRequest request) 
            => request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
        
        //----------- OBSOLETE ---------
        
        /// <summary>
        /// Gets GameInfo + all PlayerInfo in optimal sequence (1 GameInfo call + N Player calls)
        /// </summary>
        public async Task<(GameInfo gameInfo, List<PlayerInfo> players)> GetInitialDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Step 1: Get GameInfo ONCE
                GameInfo gameInfo = await GetGameInfoAsync(cancellationToken);
                
                // Step 2: Get all players in parallel using gameInfo.Players
                List<PlayerInfo> players = await GetPlayersParallelAsync(gameInfo.Players, cancellationToken);
                
                Debug.Log($"✅ Loaded {gameInfo.Territories.Count} territories, {players.Count} players");
                return (gameInfo, players);
            }
            catch (Exception ex)
            {
                Debug.LogError($"GetInitialData failed: {ex.Message}");
                return (null, null);
            }
        }

        private async Task<GameInfo> GetGameInfoAsync(CancellationToken cancellationToken = default)
        {
            string url = $"{_defaultLocalURL}/{_gameId}/info";
            using var request = UnityWebRequest.Get(url);
            
            await request.SendWebRequest();

            if (HasError(request)) 
                throw new Exception($"GetGameInfo failed: {request.error}");

            string json = request.downloadHandler.text;
            return JsonConvert.DeserializeObject<GameInfo>(json);
        }

        private async Task<List<PlayerInfo>> GetPlayersParallelAsync(List<string> playerIds, CancellationToken cancellationToken = default)
        {
            var playerTasks = playerIds.Select(playerId => GetPlayerInfoAsync(playerId, cancellationToken));
            PlayerInfo[] players = await Task.WhenAll(playerTasks);
            return players.ToList();
        }

        private async Task<PlayerInfo> GetPlayerInfoAsync(string playerId, CancellationToken cancellationToken = default)
        {
            string url = $"{_defaultLocalURL}/{_gameId}/players/{playerId}/info";
            using var request = UnityWebRequest.Get(url);
            
            await request.SendWebRequest();

            if (HasError(request)) 
                throw new Exception($"GetPlayerInfo({playerId}) failed: {request.error}");

            return JsonConvert.DeserializeObject<PlayerInfo>(request.downloadHandler.text);
        }

        
    }
}
