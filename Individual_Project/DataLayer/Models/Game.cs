namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Game
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool InProgress { get; set; }

        [Required]
        [StringLength(20)]
        public string WhitePlayer { get; set; }

        [Required]
        [StringLength(20)]
        public string BlackPlayer { get; set; }

        [StringLength(20)]
        public string Turn { get; set; }

        [StringLength(20)]
        public string Winner { get; set; }

        [Required]
        public string BoardData { get; set; }

        public virtual User User_BlackPlayer { get; set; }

        public virtual User User_WhitePlayer { get; set; }

        public virtual User User_Turn { get; set; }

        public virtual User User_Winner { get; set; }
    }
}
