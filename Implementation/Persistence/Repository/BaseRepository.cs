using GudelIdService.Implementation.Persistence.Context;

namespace GudelIdService.Implementation.Persistence.Repository
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}