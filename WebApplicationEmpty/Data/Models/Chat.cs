using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST.Data.Models
{
    [Table("Chat", Schema = "dbo")]
    public class Chat : ILogModel, IIdModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///  writer
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        ///  Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  likes count
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        ///  likes users
        /// </summary>
        public string RatedUsers { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(100)]
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}
