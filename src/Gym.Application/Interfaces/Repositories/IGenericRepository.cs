using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    // L?y d? li?u theo ID
    Task<T?> GetByIdAsync(Guid id);

    // L?y toàn b? d? li?u
    Task<IEnumerable<T>> GetAllAsync();

    // Tìm ki?m theo di?u ki?n (Tr? v? danh sách)
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    // L?y ra IQueryable d? có th? dùng LINQ (Include, Where, OrderBy...) ? t?ng Service
    IQueryable<T> GetQueryable();

    // Thêm m?i
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    // C?p nh?t (EF Core Update là hàm d?ng b?)
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    // Xóa
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}
