namespace Gym.Application.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int TotalTrainers { get; set; }
    public decimal MonthlyRevenue { get; set; }
    // Bạn có thể thêm các chỉ số khác tùy ý
}