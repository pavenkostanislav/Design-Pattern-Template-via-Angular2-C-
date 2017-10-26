using System;
using System.ComponentModel.DataAnnotations;

namespace KPMA.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string LastModifyBy { get; set; }
        public DateTime LastModifyDate { get; set; }
    }
}
