using System.Data;
using test_case.api.Context;

namespace test_case.api.Services.Abstract
{
    public abstract class BaseService
    {
        private protected readonly TestCaseContext _context;
        private protected readonly IDbConnection _dbConnection;

        protected BaseService(TestCaseContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }
    }
}
