using LevRobinGuss.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevRobinGuss.Core.Entities
{
    public class Gambling
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double AmountBet { get; set; }
        public WinOrNot WinOrNot { get; set; }
        public int GameId { get; set; }
        public double BetPercentage { get; set; }
        public WinOrNot BetOnHouse { get; set; }

        public virtual ApplicationUser User  { get; set; }



    }
}
