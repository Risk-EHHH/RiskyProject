using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Manages backend communication for game-related data retrieval and updates.
    /// </summary>
    public class BackendManager : MonoBehaviour
    {
        [SerializeField] private string _defaultLocalURL = "http://127.0.0.1:8000/games";
        private string _gameId;
        
        private class PlayerNames
        {
            [JsonProperty("player_names")]
            public List<string> Names;
        }
        
        /// <summary>
        /// Called once at the beginning of the game to create a new game session.
        /// Creates a new game on the backend server with the provided list of player names.
        /// This method sends a POST request to initialize a new game session and retrieves the associated game metadata.
        /// </summary>
        /// <param name="playerNames">A list of player names to be included in the new game session.</param>
        /// <returns>
        /// An instance of <see cref="Game"/> containing information about the newly created game session,
        /// including its unique game ID.
        /// Returns null if the request fails or if the backend response indicates an error.
        /// </returns>
        public async Task<Game> PostNewGame(List<string> playerNames)
        {
            string url = $"{_defaultLocalURL}/new_game";
            string playerNamesJson = JsonConvert.SerializeObject(new PlayerNames { Names = playerNames });
            Debug.Log($"Posting new game with {playerNamesJson}");
            using UnityWebRequest request = UnityWebRequest.Post(url, playerNamesJson, "application/json");
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<Game>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"PostNewGame failed: {response.Message}");
                return null;
            }
            
            Game game = response.Metadata;
            _gameId = game.GameID;
            return game;
        }

        public async Task<bool> PostStartGame()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/start";
            using UnityWebRequest request = UnityWebRequest.Post(url, null, "application/json");
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<Game>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"StartGame failed: {response.Message}");
                return false;
            }    
            
            return true;
        }
        
        
        /// <summary>
        /// Called once at the beginning of the game to setup the board.
        /// Retrieves the current board information, including details about continents and other game-related metadata.
        /// This method sends a GET request to the backend server using the active game ID to fetch the board state.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="BoardInfo"/> containing data about the game's board configuration,
        /// including continents and their composition.
        /// Returns null if the request fails or if the response indicates an error.
        /// </returns>
        public async Task<BoardInfo> GetBoardInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/get_board_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<BoardInfo>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetBoardInfo failed: {response.Message}");
                return null;
            }
            
            return response.Metadata;
        }

        /// <summary>
        /// Called every time there is a change in the board state (probably every turn phase!) to get the territories info after player actions.
        /// Retrieves information about all territories, including ownership and the number of armies stationed.
        /// The data is fetched from the backend server based on the current game context.
        /// </summary>
        /// <returns>
        /// A dictionary where the keys are territory identifiers as strings and the values are
        /// <see cref="TerritoryInfo"/> objects containing details about the territories.
        /// Returns null if the request fails or if an error occurs.
        /// </returns>
        public async Task<Dictionary<string, TerritoryInfo>> GetTerritoriesInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/get_territories_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);
    
            await request.SendWebRequest();
    
            if (HasError(request))
                Debug.LogError($"Request failed: {request.error}");
    
            var response = JsonConvert.DeserializeObject<ApiResponse<Dictionary<string, TerritoryInfo>>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetTerritoriesInfo failed: {response.Message}");
                return null;
            }
    
            return response.Metadata;
        }

        /// <summary>
        /// Retrieves information about all players in the current game, including their names, territories, armies,
        /// and game status details such as whether they are eliminated or have won.
        /// The data is fetched from the backend server based on the current game context.
        /// </summary>
        /// <returns>
        /// A dictionary where the keys are player identifiers as strings, and the values are
        /// <see cref="PlayerInfo"/> objects containing details about each player.
        /// Returns null if the request fails, an error occurs, or the response indicates a failure.
        /// </returns>
        public async Task<Dictionary<string, PlayerInfo>> GetPlayersInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/get_players_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);
    
            await request.SendWebRequest();
    
            if (HasError(request))
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<Dictionary<string, PlayerInfo>>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetPlayersInfo failed: {response.Message}");
                return null;
            }

            return response.Metadata;
        }
        
        /// <summary>
        /// Retrieves the secret info for a specific player, including their mission and fallback mission.
        /// This should only be fetched for the local player, never exposed to opponents.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player whose secret info is being requested.</param>
        /// <returns>
        /// A <see cref="SecretPlayerInfo"/> instance containing the player's private data including mission details.
        /// Returns null if the request fails or the response indicates an error.
        /// </returns>
        public async Task<SecretPlayerInfo> GetSecretPlayerInfo(string playerId)
        {
            string url = $"{_defaultLocalURL}/{_gameId}/player/{playerId}/get_secret_player_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);

            await request.SendWebRequest();

            if (HasError(request))
                Debug.LogError($"Request failed: {request.error}");

            var response = JsonConvert.DeserializeObject<ApiResponse<SecretPlayerInfo>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetSecretPlayerInfo failed: {response.Message}");
                return null;
            }

            return response.Metadata;
        }

        public async Task<TurnInfo> GetTurnInfo()
        {
            string url = $"{_defaultLocalURL}/{_gameId}/get_turn_info";
            using UnityWebRequest request = UnityWebRequest.Get(url);

            await request.SendWebRequest();
            
            if (HasError(request))
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<TurnInfo>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetTurnInfo failed: {response.Message}");
                return null;
            }
            
            return response.Metadata;
        }
        
        /// <summary>
        /// Retrieves the result of a previously submitted action.
        /// Should be called after any POST action to check whether it was accepted or rejected by the backend.
        /// </summary>
        /// <param name="actionId">The unique identifier of the action to retrieve the result for.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing the action status, result, and error details if any.
        /// Returns null if the request fails or the response indicates an error.
        /// </returns>
        public async Task<ActionResult> GetActionResult(string actionId)
        {
            string url = $"{_defaultLocalURL}/{_gameId}/action_result/{actionId}";
            using UnityWebRequest request = UnityWebRequest.Get(url);

            await request.SendWebRequest();

            if (HasError(request))
                Debug.LogError($"Request failed: {request.error}");

            Debug.Log($"GetActionResult: {request.downloadHandler.text}");
            var response = JsonConvert.DeserializeObject<ApiResponse<ActionResult>>(request.downloadHandler.text);
            if (response.Status != "success")
            {
                Debug.LogError($"GetActionResult failed: {response.Message}");
                return null;
            }

            return response.Metadata;
        }

        public async Task<PlayerAction> PostReinforce(string playerId, string territoryName, int armies)
        {
            string url = $"{_defaultLocalURL}/{_gameId}/action";

            List<ReinforceTerritory> reinforceTerritories = new() { new ReinforceTerritory { Name = territoryName, Armies = armies } };

            Reinforce reinforceData = new Reinforce
            {
                PlayerId = playerId,
                Payload = new Payload
                {
                    Territories = reinforceTerritories
                }
            };
            
            string reinforceJson = JsonConvert.SerializeObject(reinforceData);
            using UnityWebRequest request = UnityWebRequest.Post(url, reinforceJson, "application/json");
            
            await request.SendWebRequest();
            
            if (HasError(request)) 
                Debug.LogError($"Request failed: {request.error}");
            
            var response = JsonConvert.DeserializeObject<ApiResponse<PlayerAction>>(request.downloadHandler.text);
            if (response.Status != "accepted")
            {
                Debug.LogError($"PostReinforce error: {response.Message}");
                Debug.LogError($"PostReinforce error: {response.Metadata.Error}");
                return null;
            }
            
            return response.Metadata;
        }
        
        private static bool HasError(UnityWebRequest request) 
            => request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
    }
}
