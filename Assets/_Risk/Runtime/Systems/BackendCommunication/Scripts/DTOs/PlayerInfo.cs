// using System;
// using System.Collections.Generic;
// using Newtonsoft.Json;
// using UnityEngine;
//
// namespace Risk.Runtime.BackendCommunication
// {
//     /// <summary>
//     /// Represents a player in the game.
//     /// </summary>
//     [Serializable]
//     public class PlayerInfo
//     {
//         [SerializeField] private string _id;
//         [SerializeField] private string _name;
//         [SerializeField] private string _color;
//         [SerializeField] private List<string> _territories;
//         [SerializeField] private int _armyPool;
//         [SerializeField] private bool _isEliminated;
//
//         [JsonProperty("id")]
//         public string Id 
//         { 
//             get => _id; 
//             set => _id = value; 
//         }
//
//         [JsonProperty("name")]
//         public string Name 
//         { 
//             get => _name; 
//             set => _name = value; 
//         }
//
//         [JsonProperty("color")]
//         public string Color 
//         { 
//             get => _color; 
//             set => _color = value; 
//         }
//
//         [JsonProperty("territories")]
//         public List<string> Territories 
//         { 
//             get => _territories; 
//             set => _territories = value; 
//         }
//
//         [JsonProperty("army_pool")]
//         public int ArmyPool 
//         { 
//             get => _armyPool; 
//             set => _armyPool = value; 
//         }
//
//         [JsonProperty("is_eliminated")]
//         public bool IsEliminated 
//         { 
//             get => _isEliminated; 
//             set => _isEliminated = value; 
//         }
//         
//         public override string ToString()
//         {
//             return $"PlayerInfo(Id='{Id}', Name='{Name}', Color='{Color}', Territories=[{string.Join(", ", Territories ?? new List<string>())}], ArmyPool={ArmyPool}, Eliminated={IsEliminated})";
//         }
//     }
//         
// }
