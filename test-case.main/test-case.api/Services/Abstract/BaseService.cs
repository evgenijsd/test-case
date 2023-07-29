using test_case.api.Context;

namespace test_case.api.Services.Abstract
{
    public abstract class BaseService
    {
        private protected readonly TestCaseContext _context;

        protected BaseService(TestCaseContext context)
        {
            _context = context;
        }
    }
}
