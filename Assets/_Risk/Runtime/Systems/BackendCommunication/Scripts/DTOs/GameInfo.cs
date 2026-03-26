// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using Newtonsoft.Json;
// using UnityEngine;
//
// namespace Risk.Runtime.BackendCommunication
// {
//     /// <summary>
//     /// Represents the state of the game, including territories and players.
//     /// </summary>
//     [Serializable]
//     public class GameInfo
//     {
//         [SerializeField] private List<Territory> _territories;
//         [SerializeField] private List<string> _players;
//
//         [JsonProperty("territories")]
//         public List<Territory> Territories 
//         { 
//             get => _territories; 
//             set => _territories = value; 
//         }
//         
//         [JsonProperty("players")]
//         public List<string> Players 
//         { 
//             get => _players; 
//             set => _players = value; 
//         }
//         
//         public override string ToString()
//         {
//             var sb = new StringBuilder();
//             sb.AppendLine("GameInfo {");
//             sb.AppendLine($"  Players: [{string.Join(", ", Players ?? new List<string>())}]");
//             sb.AppendLine($"  Territories ({Territories?.Count ?? 0}):");
//             if (Territories != null)
//             {
//                 foreach (var t in Territories.Take(5)) // Limit to 5 for readability
//                 {
//                     sb.AppendLine($"    - {t}");
//                 }
//                 if (Territories.Count > 5)
//                     sb.AppendLine($"    ... and {Territories.Count - 5} more.");
//             }
//             sb.AppendLine("}");
//             return sb.ToString();
//         }
//     }
// }
