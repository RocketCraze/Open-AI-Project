namespace OpenAIProject.Interfaces
{
    using OpenAIProject.Models;

    public interface IEditService
    {
        List<DaVinciEdit> GetAll();

        void Add(DaVinciEdit edit);
    }
}
