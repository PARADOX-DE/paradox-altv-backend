using AltV.Net.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PARADOX_RP.Core.Database.Models
{
    [Table("player_weapons")]
    public class PlayerWeapons
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        public WeaponModel WeaponHash { get; set; }
        public int Ammo { get; set; }

        public virtual Players Player { get; set; }
    }
}
