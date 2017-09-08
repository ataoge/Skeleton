using System.Data.Common;

namespace Ataoge.EntityFrameworkCore.Repositories
{
    interface IRepositoryHelper
    {
        string ProviderName {get;}

        string CreateParameterName(string name);

        DbParameter CreateDbParmeter(string name, object value);
    }
}