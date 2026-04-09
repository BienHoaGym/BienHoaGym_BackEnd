import os

path = r"d:\Tài liệu - Copy\Gym Project\BienHoaGym_BackEnd\src\Gym.Application\Services\SubscriptionService.cs"

with open(path, "r", encoding="utf-8-sig") as f:
    text = f.read()

# Fix ResumeAsync
old_logic = """        if (subscription.LastPausedAt.HasValue)

        {

            var pauseDuration = DateTime.UtcNow - subscription.LastPausedAt.Value;

            if (pauseDuration.TotalDays > 0)

            {

                subscription.EndDate = subscription.EndDate.AddDays(pauseDuration.TotalDays);

            }

        }"""

new_logic = """        if (subscription.LastPausedAt.HasValue)
        {
            var actualPauseDuration = DateTime.UtcNow - subscription.LastPausedAt.Value;
            var actualDays = (int)Math.Ceiling(actualPauseDuration.TotalDays);

            if (subscription.AutoPauseExtensionDays.HasValue && subscription.AutoPauseExtensionDays.Value > 0)
            {
                // Điều chỉnh: Ngày hết hạn thực tế = Ngày hết hạn cũ + Số ngày nghỉ thực tế
                // Vì ngày hiện tại đã được cộng EstimatedDays, nên ta cộng chênh lệch (actual - estimated)
                var adjustmentDays = actualDays - subscription.AutoPauseExtensionDays.Value;
                subscription.EndDate = subscription.EndDate.AddDays(adjustmentDays);
            }
            else
            {
                // Trường hợp nghỉ tự do, cộng dồn toàn bộ số ngày nghỉ thực tế
                subscription.EndDate = subscription.EndDate.AddDays(actualDays);
            }
        }"""

text = text.replace(old_logic, new_logic)

# Reset state
text = text.replace("subscription.LastPausedAt = null;", "subscription.LastPausedAt = null;\n        subscription.AutoPauseExtensionDays = null;")

# Clean up excessive blank lines (optional but good)
import re
text = re.sub(r'\n\s*\n\s*\n', '\n\n', text)

with open(path, "w", encoding="utf-8-sig") as f:
    f.write(text)

print("Logic repair and cleanup completed.")
