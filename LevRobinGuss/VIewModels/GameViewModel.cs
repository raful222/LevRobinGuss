using LevRobinGuss.Core.Entities;

namespace LevRobinGuss.VIewModels
{
    public class GameViewModel
    {
        public decimal AmountBet { get;set;}
        public int GameId { get;set;}
        public WinOrNot BetOnHouse { get;set;}
        public decimal BetPercentage { get;set;}

    }
}
