namespace OpenAIProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    public class ChatGPTMessage
    {
        [Key]
        [Column("pkID")]
        public int id { get; set; }

        [Column("Role")]
        public string role { get; set; }

        [Column("Content")]
        public string content { get; set; }
    }
}
