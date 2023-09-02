using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IDrinkRepository
    {
        DrinkEntity GetSingle(int id);
        void Add(DrinkEntity item);
        void Delete(int id);
        DrinkEntity Update(int id, DrinkEntity item);
        IQueryable<DrinkEntity> GetAll(QueryParameters queryParameters);
        ICollection<DrinkEntity> GetRandomBeverages();
        int Count();
        bool Save();
    }
}
