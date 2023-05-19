namespace OpenAIProject.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class DaVinciEdit
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
