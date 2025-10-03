using System.Data;

namespace DENTISTA360_BACKEND.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
