namespace OpenAIProject.Interfaces
{
    using OpenAIProject.Models;

    public interface IChatService
    {
        List<ChatGPTMessage> GetAll();

        void Add(ChatGPTMessage message);
    }
}
