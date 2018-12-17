namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Message
    {
        public int Id { get; set; }

        public DateTime Tstamp { get; set; }

        [Required]
        [StringLength(20)]
        public string Sender { get; set; }

        [Required]
        [StringLength(20)]
        public string Receiver { get; set; }

        [MinLength(1)]
        [StringLength(250)]
        public string Data { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
