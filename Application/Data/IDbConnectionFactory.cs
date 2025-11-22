using System.Data;

namespace Application.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
