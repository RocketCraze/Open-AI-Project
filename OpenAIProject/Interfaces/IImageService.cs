namespace OpenAIProject.Interfaces
{
    using OpenAIProject.Models;

    public interface IImageService
    {
        List<ImageGenerationAI> GetAll();

        void Add(ImageGenerationAI image);
    }
}
