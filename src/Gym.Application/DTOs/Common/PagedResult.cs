using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.Application.DTOs.Common;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new(); // Danh sách d? li?u 
    public int TotalCount { get; set; } // T?ng s? b?n ghi 
    public int PageNumber { get; set; } // Trang hi?n t?i 
    public int PageSize { get; set; } // S? lu?ng m?i trang 
}
