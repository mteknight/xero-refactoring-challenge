using System.Data;

namespace RefactorThis.Data
{
    public interface IDataService
    {
        IDbConnection NewConnection();
    }
}
