using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.Application.DTOs.Common;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new(); // Danh sách dữ liệu 
    public int TotalCount { get; set; } // Tổng số bản ghi 
    public int PageNumber { get; set; } // Trang hiện tại 
    public int PageSize { get; set; } // Số lượng mỗi trang 
}