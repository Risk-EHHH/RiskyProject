using System.Collections.Generic;
using Risk.Runtime.GameState;

namespace Risk.Runtime.BackendCommunication
{
    public static class GameStateMapper
    {
        public static GameState.GameState ToGameState(Game dto)
        {
            return new GameState.GameState
            {
                GameID = dto.GameID
            };
        }

        public static BoardState ToBoardState(BoardInfo dto)
        {
            var continents = new List<ContinentState>();
            foreach (Continent continent in dto.Continents)
            {
                var territories = new List<TerritorySetupState>();
                foreach (Territory territory in continent.Territories)
                {
                    territories.Add(new TerritorySetupState
                    {
                        Name = territory.Name,
                        Borders = territory.Borders,
                        Unit = territory.Unit,
                        Owner = territory.Owner,
                        Armies = territory.Armies
                    });
                }
                continents.Add(new ContinentState
                {
                    Name = continent.Name,
                    Territories = territories
                });
            }
            return new BoardState { Continents = continents };
        }

        public static List<TerritoryState> ToTerritoryStates(Dictionary<string, TerritoryInfo> dto)
        {
            var result = new List<TerritoryState>();
            foreach ((string name, TerritoryInfo info) in dto)
            {
                result.Add(new TerritoryState
                {
                    Name = name,
                    Owner = info.Owner,
                    Armies = info.Armies
                });
            }
            return result;
        }

        public static List<PlayerState> ToPlayerStates(Dictionary<string, PlayerInfo> dto)
        {
            var result = new List<PlayerState>();
            foreach ((string id, PlayerInfo info) in dto)
            {
                result.Add(new PlayerState
                {
                    Id = id,
                    Name = info.Name,
                    Color = info.Color,
                    Territories = info.Territories,
                    ArmyPool = info.ArmyPool,
                    IsWinner = info.IsWinner,
                    IsEliminated = info.IsEliminated,
                    Eliminations = info.Eliminations
                });
            }
            return result;
        }

        public static SecretPlayerState ToSecretPlayerState(SecretPlayerInfo dto)
        {
            return new SecretPlayerState
            {
                Id = dto.Id,
                Name = dto.Name,
                Color = dto.Color,
                Territories = dto.Territories,
                ArmyPool = dto.ArmyPool,
                IsWinner = dto.IsWinner,
                IsEliminated = dto.IsEliminated,
                Eliminations = dto.Eliminations,
                Mission = ToMissionState(dto.Mission)
            };
        }

        private static MissionState ToMissionState(Mission dto)
        {
            return new MissionState
            {
                Description = dto.Description,
                Target = dto.Target,
                FallbackMission = new FallbackMissionState
                {
                    Description = dto.FallbackMission.Description,
                    TerritoryCount = dto.FallbackMission.TerritoryCount,
                    ArmyCount = dto.FallbackMission.ArmyCount
                }
            };
        }
    }
}