using OpenAIProject.Data;
using OpenAIProject.Interfaces;
using OpenAIProject.Models;

namespace OpenAIProject.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext context;

        public ImageService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<ImageGenerationAI> GetAll() 
        {
            return this.context.Set<ImageGenerationAI>().ToList();
        }

        public void Add(ImageGenerationAI image)
        {
            this.context.Set<ImageGenerationAI>().Add(image);
            this.context.SaveChanges();
        }
    }
}
