// using System;
// using System.Collections.Generic;
// using Newtonsoft.Json;
// using UnityEngine;
//
// namespace Risk.Runtime.BackendCommunication
// {
//     /// <summary>
//     /// Represents a territory in the game.
//     /// </summary>
//     [Serializable]
//     public class Territory
//     {
//         [SerializeField] private string _name;
//         [SerializeField] private List<string> _borders;
//         [SerializeField] private string _owner;
//         [SerializeField] private int _armies;
//
//         [JsonProperty("name")]
//         public string Name 
//         { 
//             get => _name; 
//             set => _name = value; 
//         }
//         
//         [JsonProperty("borders")]
//         public List<string> Borders 
//         { 
//             get => _borders; 
//             set => _borders = value; 
//         }
//         
//         [JsonProperty("owner")]
//         public string Owner 
//         { 
//             get => _owner; 
//             set => _owner = value; 
//         }
//         
//         [JsonProperty("armies")]
//         public int Armies 
//         { 
//             get => _armies; 
//             set => _armies = value; 
//         }
//         
//         
//         public override string ToString()
//         {
//             return $"Territory(Name='{Name}', Owner='{Owner}', Armies={Armies}, Borders=[{string.Join(", ", Borders ?? new List<string>())}])";
//         }
//     }
// }
