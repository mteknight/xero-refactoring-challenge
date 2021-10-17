using System.Data;

using RefactorThis.Models;

namespace RefactorThis.Data
{
    public class SqLiteDataService : ISqLiteDataService
    {
        public IDbConnection NewConnection() => Helpers.NewConnection();
    }
}
