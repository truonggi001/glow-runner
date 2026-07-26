Status: Approved
# Movement — GDD

## Overview
Auto-run + jump + dash. Hệ thống duy nhất người chơi trực tiếp điều khiển. Phục vụ pillar 1 (flow over frustration) và pillar 3 (one input, many decisions).

## Player Fantasy
Sprinting as pure light — bạn không dừng lại, thế giới chạy qua bạn. Jump = lightness, dash = burst of radiance.

## Detailed Rules
1. Nhân vật tự chạy tiến (Z+) với tốc độ không đổi, không thể dừng/k quay đầu.
2. Jump: nhấn Space. Ground jump → 1 unit lên. Double jump: nhấn Space lần 2 trong không trung → +0.7 unit.
3. Dash: nhấn Shift. Burst forward +2 units trong 0.2s + invincibility 0.3s. Cooldown 1.5s.
4. Rơi khỏi track = death tức thì.
5. Tốc độ tăng dần: mỗi 30s +10% (cap 200% base speed).

## Formulas
- baseSpeed: 8 m/s (assets/data/movement.json)
- jumpForce: 9.8 m/s² × jumpMultiplier
- jumpMultiplier: 1.0 (ground), 0.7 (double)
- dashDistance: 2 m
- dashDuration: 0.2 s
- dashInvincibility: 0.3 s
- dashCooldown: 1.5 s
- speedRamp: +10% per 30s, cap 2.0×
- gravity: -9.81 m/s² (Unity default, scale 1)

## Edge Cases
1. Dash khi đang rơi → dash vẫn tiến nhưng Y giữ nguyên (không cứu rơi).
2. Double jump rồi dash rồi nhảy lại → KHÔNG (chỉ 2 jump total per ground touch).
3. Dash vào wall → dừng tại wall, invincibility vẫn active 0.3s.
4. Jump ngay lúc chạm ground edge → coyote time 0.1s (vẫn jump được).
5. Tốc độ cap đạt → visual glow max, không tăng thêm.

## Dependencies
- Provides: position, velocity, isGrounded, isDashing, isInvincible → cho obstacles, collectibles, scoring, environment
- Receives: nothing (input-driven, root system)

## Tuning Knobs
- baseSpeed: 6-12 (an toàn) — quá chậm = nhàm chán, quá nhanh = phản xạ không kịp
- jumpMultiplier: 0.5-1.2 — quá thấp = không qua gap, quá cao = bay quá lâu
- dashCooldown: 0.5-3.0 — quá thấp = spam dash, quá cao = dash vô dụng
- speedRamp interval: 15-60s — quá nhanh = khó tăng, quá chậm = không cảm nhận

## Acceptance Criteria
1. Nhân vật tự chạy Z+ liên tục, không cần input
2. Space = jump (ground), Space again = double jump (air)
3. Shift = dash forward + invincibility 0.3s + cooldown 1.5s
4. Rơi khỏi track = death event fired
5. Tốc độ tăng 10% mỗi 30s, cap 200%
6. Coyote time 0.1s hoạt động
7. Dash không reset jump count