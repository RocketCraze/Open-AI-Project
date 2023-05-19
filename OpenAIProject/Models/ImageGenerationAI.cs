using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenAIProject.Models
{
    public class ImageGenerationAI
    {
        [Key]
        [Column("pkID")]
        public int Id { get; set; }

        [Column("Prompt")]
        public string Prompt { get; set; }

        [Column("Image")]
        public string Image { get; set; }
    }
}
