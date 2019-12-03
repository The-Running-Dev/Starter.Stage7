using Starter.Data.Entities;

namespace Starter.Data.Repositories
{
    /// <summary>
    /// Defines the contract for cat CRUD operation
    /// </summary>
    public interface ICatRepository : IRepository<Cat>
    {
    }
}