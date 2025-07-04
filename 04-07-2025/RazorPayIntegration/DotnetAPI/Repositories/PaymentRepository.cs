using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public  class PaymentRepository : Repository<Guid, Payment>
    {
        public PaymentRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override async Task<Payment> Get(Guid key)
        {
            var payment = await _databaseContext.payments.Include(p => p.Order).SingleOrDefaultAsync(p => p.Id == key);

            return payment??throw new Exception("No payment with the given ID");
        }

        public override async Task<IEnumerable<Payment>> GetAll()
        {
            var payments = _databaseContext.payments.Include(p => p.Order);
            if (!payments.Any())
                return Enumerable.Empty<Payment>();
            return (await payments.ToListAsync());
        }
    }
}