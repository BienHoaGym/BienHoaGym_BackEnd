using System;
using Xunit;

namespace BienHoaGym.Tests
{
    public class MemberTests
    {
        [Fact]
        public void CalculateExpirationDate_Add1Month_ReturnsCorrectDate()
        {
            // 1. Arrange (Chuẩn bị)
            // Giả lập ngày hội viên đăng ký là 01/04/2026
            DateTime joinDate = new DateTime(2026, 4, 1);
            int subscriptionMonths = 1; 

            // 2. Act (Thực thi)
            // Giả sử hàm xử lý logic của bạn cộng thêm số tháng vào ngày đăng ký
            // Trong thực tế, bạn sẽ gọi hàm từ class Member trong dự án chính: member.CalculateExpiration(subscriptionMonths)
            DateTime actualExpirationDate = joinDate.AddMonths(subscriptionMonths);

            // 3. Assert (Kiểm chứng)
            // Kỳ vọng ngày hết hạn phải là 01/05/2026
            DateTime expectedDate = new DateTime(2026, 5, 1);
            
            // Xac nhận kết quả thực tế khớp với kỳ vọng
            Assert.Equal(expectedDate, actualExpirationDate);
        }
    }
}
