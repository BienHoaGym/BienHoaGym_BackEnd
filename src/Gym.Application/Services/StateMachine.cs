using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.Application.Services
{
    internal class StateMachine
    {
        // ───────────────────────────────────────────────────────────
        // SUBSCRIPTION STATUS ENUM
        // ───────────────────────────────────────────────────────────
        public enum SubscriptionStatus
        {
            Pending,    // Đang chờ thanh toán
            Active,     // Đang hoạt động
            Expired,    // Hết hạn
            Cancelled   // Đã hủy
        }

        // ───────────────────────────────────────────────────────────
        // PAYMENT STATUS ENUM
        // ───────────────────────────────────────────────────────────
        public enum PaymentStatus
        {
            Unpaid,     // Chưa thanh toán
            Paid,       // Đã thanh toán
            Refunded    // Đã hoàn tiền
        }

        // ───────────────────────────────────────────────────────────
        // SUBSCRIPTION STATE MACHINE
        // ───────────────────────────────────────────────────────────
        public class SubscriptionStateMachine
        {
            // Valid transitions
            private static readonly Dictionary<SubscriptionStatus, List<SubscriptionStatus>> ValidTransitions = new()
        {
            { SubscriptionStatus.Pending, new List<SubscriptionStatus> { SubscriptionStatus.Active, SubscriptionStatus.Cancelled } },
            { SubscriptionStatus.Active, new List<SubscriptionStatus> { SubscriptionStatus.Expired, SubscriptionStatus.Cancelled } },
            { SubscriptionStatus.Expired, new List<SubscriptionStatus> { } },  // Không chuyển được
            { SubscriptionStatus.Cancelled, new List<SubscriptionStatus> { } }  // Không chuyển được
        };

            /// <summary>
            /// Kiểm tra xem có thể chuyển từ trạng thái này sang trạng thái khác không
            /// </summary>
            public static bool CanTransition(SubscriptionStatus from, SubscriptionStatus to)
            {
                if (!ValidTransitions.ContainsKey(from))
                    return false;

                return ValidTransitions[from].Contains(to);
            }

            /// <summary>
            /// Validate và thực hiện chuyển trạng thái
            /// Throw exception nếu không hợp lệ
            /// </summary>
            public static void ValidateTransition(SubscriptionStatus from, SubscriptionStatus to)
            {
                if (!CanTransition(from, to))
                {
                    throw new InvalidOperationException(
                        $"Không thể chuyển Subscription từ '{from}' sang '{to}'. " +
                        $"Các trạng thái hợp lệ từ '{from}' là: {string.Join(", ", ValidTransitions[from])}");
                }
            }

            /// <summary>
            /// Lấy danh sách các trạng thái có thể chuyển đến
            /// </summary>
            public static List<SubscriptionStatus> GetPossibleTransitions(SubscriptionStatus current)
            {
                return ValidTransitions.ContainsKey(current)
                    ? ValidTransitions[current]
                    : new List<SubscriptionStatus>();
            }

            /// <summary>
            /// Business logic: Kích hoạt subscription
            /// </summary>
            public static void Activate(ref SubscriptionStatus status)
            {
                ValidateTransition(status, SubscriptionStatus.Active);
                status = SubscriptionStatus.Active;
            }

            /// <summary>
            /// Business logic: Hết hạn subscription
            /// </summary>
            public static void Expire(ref SubscriptionStatus status)
            {
                ValidateTransition(status, SubscriptionStatus.Expired);
                status = SubscriptionStatus.Expired;
            }

            /// <summary>
            /// Business logic: Hủy subscription
            /// </summary>
            public static void Cancel(ref SubscriptionStatus status)
            {
                ValidateTransition(status, SubscriptionStatus.Cancelled);
                status = SubscriptionStatus.Cancelled;
            }
        }

        // ───────────────────────────────────────────────────────────
        // PAYMENT STATE MACHINE
        // ───────────────────────────────────────────────────────────
        public class PaymentStateMachine
        {
            // Valid transitions
            private static readonly Dictionary<PaymentStatus, List<PaymentStatus>> ValidTransitions = new()
        {
            { PaymentStatus.Unpaid, new List<PaymentStatus> { PaymentStatus.Paid } },
            { PaymentStatus.Paid, new List<PaymentStatus> { PaymentStatus.Refunded } },
            { PaymentStatus.Refunded, new List<PaymentStatus> { } }  // Không chuyển được
        };

            /// <summary>
            /// Kiểm tra xem có thể chuyển từ trạng thái này sang trạng thái khác không
            /// </summary>
            public static bool CanTransition(PaymentStatus from, PaymentStatus to)
            {
                if (!ValidTransitions.ContainsKey(from))
                    return false;

                return ValidTransitions[from].Contains(to);
            }

            /// <summary>
            /// Validate và thực hiện chuyển trạng thái
            /// Throw exception nếu không hợp lệ
            /// </summary>
            public static void ValidateTransition(PaymentStatus from, PaymentStatus to)
            {
                if (!CanTransition(from, to))
                {
                    throw new InvalidOperationException(
                        $"Không thể chuyển Payment từ '{from}' sang '{to}'. " +
                        $"Các trạng thái hợp lệ từ '{from}' là: {string.Join(", ", ValidTransitions[from])}");
                }
            }

            /// <summary>
            /// Business logic: Thanh toán
            /// </summary>
            public static void MarkAsPaid(ref PaymentStatus status)
            {
                ValidateTransition(status, PaymentStatus.Paid);
                status = PaymentStatus.Paid;
            }

            /// <summary>
            /// Business logic: Hoàn tiền
            /// </summary>
            public static void Refund(ref PaymentStatus status)
            {
                ValidateTransition(status, PaymentStatus.Refunded);
                status = PaymentStatus.Refunded;
            }
        }

        // ───────────────────────────────────────────────────────────
        // EXAMPLE USAGE IN SERVICE LAYER
        // ───────────────────────────────────────────────────────────
        public class ExampleUsage
        {
            public void ActivateSubscription(Subscription subscription)
            {
                // Validate business rules
                if (subscription.Payment.Status != PaymentStatus.Paid)
                {
                    throw new InvalidOperationException("Không thể kích hoạt subscription khi chưa thanh toán");
                }

                // Validate state transition
                SubscriptionStateMachine.ValidateTransition(subscription.Status, SubscriptionStatus.Active);

                // Perform transition
                subscription.Status = SubscriptionStatus.Active;
                subscription.StartDate = DateTime.UtcNow;

                // Save to database
                // _repository.Update(subscription);

                // Log to audit
                // _auditService.Log("Activate Subscription", ...);
            }

            public void CancelSubscription(Subscription subscription)
            {
                // Business rules
                if (subscription.Status == SubscriptionStatus.Expired)
                {
                    throw new InvalidOperationException("Không thể hủy subscription đã hết hạn");
                }

                // Validate state transition
                SubscriptionStateMachine.ValidateTransition(subscription.Status, SubscriptionStatus.Cancelled);

                // Perform transition
                subscription.Status = SubscriptionStatus.Cancelled;
                subscription.CancelledAt = DateTime.UtcNow;

                // Check if eligible for refund
                var daysUsed = (DateTime.UtcNow - subscription.StartDate).Days;
                if (daysUsed <= 7 && subscription.Payment.Status == PaymentStatus.Paid)
                {
                    // Eligible for refund
                    ProcessRefund(subscription.Payment);
                }
            }

            private void ProcessRefund(Payment payment)
            {
                // Validate không hoàn tiền 2 lần
                if (payment.Status == PaymentStatus.Refunded)
                {
                    throw new InvalidOperationException("Payment đã được hoàn tiền rồi");
                }

                // Validate state transition
                PaymentStateMachine.ValidateTransition(payment.Status, PaymentStatus.Refunded);

                // Perform refund
                payment.Status = PaymentStatus.Refunded;
                payment.RefundedAt = DateTime.UtcNow;
                payment.RefundReason = "Cancelled within 7 days";

                // Process actual refund via payment gateway
                // _paymentGateway.ProcessRefund(payment);
            }
        }

        // ───────────────────────────────────────────────────────────
        // DUMMY MODELS FOR EXAMPLE
        // ───────────────────────────────────────────────────────────
        public class Subscription
        {
            public int Id { get; set; }
            public SubscriptionStatus Status { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? CancelledAt { get; set; }
            public Payment Payment { get; set; } = new();
        }

        public class Payment
        {
            public int Id { get; set; }
            public PaymentStatus Status { get; set; }
            public DateTime? RefundedAt { get; set; }
            public string RefundReason { get; set; } = string.Empty;
        }
    }
}

///*
//═══════════════════════════════════════════════════════════════
//TESTING STATE MACHINE

//Unit test examples:
//───────────────────────────────────────────────────────────────

//[Fact]
//public void Subscription_CannotGoFromExpiredToActive()
//{
//    // Arrange
//    var status = SubscriptionStatus.Expired;
    
//    // Act & Assert
//    Assert.Throws<InvalidOperationException>(() => 
//        SubscriptionStateMachine.ValidateTransition(status, SubscriptionStatus.Active)
//    );
//}

//[Fact]
//public void Subscription_CanGoFromPendingToActive()
//{
//    // Arrange
//    var status = SubscriptionStatus.Pending;
    
//    // Act
//    SubscriptionStateMachine.ValidateTransition(status, SubscriptionStatus.Active);
    
//    // Assert - no exception thrown
//    Assert.True(SubscriptionStateMachine.CanTransition(status, SubscriptionStatus.Active));
//}

//[Fact]
//public void Payment_CannotRefundTwice()
//{
//    // Arrange
//    var status = PaymentStatus.Refunded;
    
//    // Act & Assert
//    Assert.Throws<InvalidOperationException>(() => 
//        PaymentStateMachine.ValidateTransition(status, PaymentStatus.Refunded)
//    );
//}
//    }
//}
