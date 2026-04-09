import os

file_path = r"d:\Tài liệu - Copy\Gym Project\BienHoaGym_BackEnd\src\Gym.Application\Services\SubscriptionService.cs"

# Dictionary of mojibake to correct Vietnamese
replacements = {
    "Ä Ã£": "Đã",
    "táº¡m dá»«ng": "tạm dừng",
    "gÃ³i táº­p": "gói tập",
    "thÃ nh cÃ´ng": "thành công",
    "KÃ­ch hoáº¡t": "Kích hoạt",
    "há»§y": "hủy",
    "TÃ¬m tháº¥y": "Tìm thấy",
    "sáº¯p háº¿t háº¡n": "sắp hết hạn",
    "Gia háº¡n": "Gia hạn",
    "chá»  thanh toÃ¡n": "chờ thanh toán",
    "Táº¡o Ä‘Äƒng kÃ½": "Tạo đăng ký",
    "KhÃ´ng tÃ¬m tháº¥y": "Không tìm thấy",
    "Chá»‰ cÃ³ thá»ƒ": "Chỉ có thể",
    "Ä‘ang hoáº¡t Ä‘á»™ng": "đang hoạt động",
    "kÃ­ch hoáº¡t láº¡i": "kích hoạt lại",
    "Ä‘ang á»Ÿ tráº¡ng thÃ¡i": "đang ở trạng thái",
    "thÃnh cÃ´ng": "thành công",
    "chá» ": "chờ",
    "Ã¡n": "án",
    "Ã ": "à"
}

with open(file_path, "rb") as f:
    content = f.read()

# Try to decode as utf-8 (which will contain the mojibake)
text = content.decode("utf-8")

for key, val in replacements.items():
    text = text.replace(key, val)

# Handle some specific leftovers
text = text.replace("thÃnh cÃ´ng", "thành công")
text = text.replace("Ã ", "à")

# Write back as UTF-8 with BOM
with open(file_path, "w", encoding="utf-8-sig") as f:
    f.write(text)

print("Repair completed.")
