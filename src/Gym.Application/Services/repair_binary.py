import os

path = r"d:\Tài liệu - Copy\Gym Project\BienHoaGym_BackEnd\src\Gym.Application\Services\SubscriptionService.cs"

with open(path, "rb") as f:
    content = f.read()

# We try multiple variations of the mojibake Đã
bad_sequences = [
    b"\xc3\x84\x20\xc3\x83\xc2\xa3", # Ä Ã£ with space
    b"\xc3\x84\xc3\x83\xc2\xa3",    # ÄÃ£ without space
    b"\xc4\x90\xc3\xa3",            # Đã (proper UTF-8 but maybe wrongly matched)
]

for bad in bad_sequences:
    content = content.replace(bad, "Đã".encode("utf-8"))

with open(path, "wb") as f:
    f.write(content)

print("Binary repair finished.")
