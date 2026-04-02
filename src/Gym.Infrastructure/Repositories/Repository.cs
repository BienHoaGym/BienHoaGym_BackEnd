// File: Gym.Infrastructure/Repositories/Repository.cs
using Gym.Application.Interfaces;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gym.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly GymDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(GymDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public virtual void Update(T entity) => _dbSet.Update(entity);

    public virtual void Delete(T entity) => _dbSet.Remove(entity);
    // --- THÊM ĐOẠN NÀY ĐỂ SỬA LỖI ---
    public IQueryable<T> GetQueryable()
    {
        // Trả về IQueryable để Service có thể Count/Sum trực tiếp trên DB
        return _dbSet.AsQueryable();
    }
}
