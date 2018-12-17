namespace DataLayer
{
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text;

    public class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Games = new HashSet<Game>();
            Games1 = new HashSet<Game>();
            Games2 = new HashSet<Game>();
            Games3 = new HashSet<Game>();
            Messages = new HashSet<Message>();
            Messages1 = new HashSet<Message>();
            Rights = (byte)AccessRights.A;
        }

        [Key]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public byte? Rights { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages1 { get; set; }

        public string DisplayInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0,-13}: {1,-50}","First Name",FirstName));
            sb.AppendLine(string.Format("{0,-13}: {1,-50}","Last Name",LastName));
            sb.AppendLine(string.Format("{0,-13}: {1,-50}","Email",Email));
            sb.AppendLine(string.Format("{0,-13}: {1,-50}","Username",Username));
            sb.AppendLine(string.Format("{0,-13}: {1,-50}","Password",Password));
            sb.AppendLine(string.Format("{0,-13}: {1,-50}","Access Rights",(AccessRights)Rights));
            return sb.ToString();
        }
    }
}
