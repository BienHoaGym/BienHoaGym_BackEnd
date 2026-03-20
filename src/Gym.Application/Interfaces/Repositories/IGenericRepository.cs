using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    // Lấy dữ liệu theo ID
    Task<T?> GetByIdAsync(Guid id);

    // Lấy toàn bộ dữ liệu
    Task<IEnumerable<T>> GetAllAsync();

    // Tìm kiếm theo điều kiện (Trả về danh sách)
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    // Lấy ra IQueryable để có thể dùng LINQ (Include, Where, OrderBy...) ở tầng Service
    IQueryable<T> GetQueryable();

    // Thêm mới
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    // Cập nhật (EF Core Update là hàm đồng bộ)
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    // Xóa
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}