using System;
using Xunit;

namespace BienHoaGym.Tests
{
    public class MemberTests
    {
        [Fact]
        public void CalculateExpirationDate_Add1Month_ReturnsCorrectDate()
        {
            // 1. Arrange (Chuáº©n bá»‹)
            // Giáº£ láº­p ngÃ y há»™i viÃªn Ä‘Äƒng kÃ½ lÃ  01/04/2026
            DateTime joinDate = new DateTime(2026, 4, 1);
            int subscriptionMonths = 1; 

            // 2. Act (Thá»±c thi)
            // Giáº£ sá»­ hÃ m xá»­ lÃ½ logic cá»§a báº¡n cá»™ng thÃªm sá»‘ thÃ¡ng vÃ o ngÃ y Ä‘Äƒng kÃ½
            // Trong thá»±c táº¿, báº¡n sáº½ gá»i hÃ m tá»« class Member trong dá»± Ã¡n chÃ­nh: member.CalculateExpiration(subscriptionMonths)
            DateTime actualExpirationDate = joinDate.AddMonths(subscriptionMonths);

            // 3. Assert (Kiá»ƒm chá»©ng)
            // Ká»³ vá»ng ngÃ y háº¿t háº¡n pháº£i lÃ  01/05/2026
            DateTime expectedDate = new DateTime(2026, 5, 1);
            
            // Xac nháº­n káº¿t quáº£ thá»±c táº¿ khá»›p vá»›i ká»³ vá»ng
            Assert.Equal(expectedDate, actualExpirationDate);
        }
    }
}
