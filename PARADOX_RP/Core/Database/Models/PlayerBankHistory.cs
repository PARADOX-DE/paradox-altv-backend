using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_bank_history")]
    public class PlayerBankHistory
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public virtual Players Player { get; set; }

        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public BankActionTypes Action { get; set; }
        public int Money { get; set; }
    }
}
