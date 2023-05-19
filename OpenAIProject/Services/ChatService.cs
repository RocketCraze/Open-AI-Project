namespace OpenAIProject.Services
{
    using OpenAIProject.Data;
    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;

    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext context;

        public ChatService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<ChatGPTMessage> GetAll() 
        {
            return this.context.Set<ChatGPTMessage>().ToList();
        }

        public void Add(ChatGPTMessage message)
        {
            this.context.Set<ChatGPTMessage>().Add(message);
            this.context.SaveChanges();
        }
    }
}
