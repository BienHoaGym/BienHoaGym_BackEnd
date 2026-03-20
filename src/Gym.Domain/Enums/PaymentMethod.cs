namespace Gym.Domain.Enums;

public enum PaymentMethod
{
    Cash = 1,           // Tiền mặt
    BankTransfer = 2,   // Chuyển khoản
    EWallet = 3,        // Ví điện tử (Momo, ZaloPay...)
    Card = 4,           // Thẻ (Bank card, Credit card)
    Other = 5
}