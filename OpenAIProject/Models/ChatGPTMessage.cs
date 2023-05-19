namespace OpenAIProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    public class ChatGPTMessage
    {
        [Key]
        [Column("pkID")]
        public int Id { get; set; }

        [Column("Role")]
        public string Role { get; set; }

        [Column("Content")]
        public string Content { get; set; }
    }
}
