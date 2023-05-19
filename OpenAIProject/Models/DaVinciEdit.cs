namespace OpenAIProject.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class DaVinciEdit
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
