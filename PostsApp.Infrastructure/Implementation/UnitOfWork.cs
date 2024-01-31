using PostsApp.Application.Common.Interfaces;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure.Implementation;

public class UnitOfWork : IUnitOfWork
{
    public AppDbContext _dbContext { get; private set; }
    public UnitOfWork(AppDbContext dbContext)
    {
        Posts = new PostsRepository(dbContext);
        Users = new UsersRepository(dbContext);
        Likes = new LikesRepository(dbContext);
        _dbContext = dbContext;
    }
    public IPostsRepository Posts { get; private set; }
    public IUsersRepository Users { get; private set; }
    public ILikesRepository Likes { get; private set; }
    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}