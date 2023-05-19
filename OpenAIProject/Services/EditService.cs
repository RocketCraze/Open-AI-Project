using OpenAIProject.Data;
using OpenAIProject.Interfaces;
using OpenAIProject.Models;

namespace OpenAIProject.Services
{
    public class EditService : IEditService
    {
        private readonly ApplicationDbContext context;

        public EditService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<DaVinciEdit> GetAll() 
        {
            return this.context.Set<DaVinciEdit>().ToList();
        }

        public void Add(DaVinciEdit edit)
        {
            this.context.Set<DaVinciEdit>().Add(edit);
            this.context.SaveChanges();
        }
    }
}
