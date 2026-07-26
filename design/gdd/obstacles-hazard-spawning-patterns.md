Status: Approved
# Obstacles — GDD

## Overview
Spawn hazard patterns trên track. Phục vụ pillar 3 (one input, many decisions) — mỗi pattern có 2+ cách qua.

## Player Fantasy
Thế giới tối tăm ném mọi thứ vào đường chạy của bạn — bạn né qua chúng như ánh sáng xuyên bóng.

## Detailed Rules
1. Obstacle types: wall (jump over), gap (jump across), low-bar (dash under), spike (jump or dash).
2. Patterns = tổ hợp 2-3 obstacle cách nhau 3-8m.
3. Mỗi pattern có tối thiểu 2 cách qua (pillar 3).
4. Spawn density tăng theo speed tier: tier 1 (100% speed) = 1 pattern/10m, tier 2 (150%) = 1/7m, tier 3 (200%) = 1/5m.
5. Hit obstacle khi KHÔNG invincible = death tức thì.
6. Hit obstacle khi invincible (dash) = pass through + particle burst.

## Formulas
- patternSpacing: 10 / speedMultiplier (m) — càng nhanh càng dày
- obstacleHeight_wall: 1.5 m (assets/data/obstacles.json)
- obstacleHeight_lowbar: 1.0 m (dash under)
- gapWidth: 2-4 m
- spikeDamage: instant death
- speedMultiplier: đọc từ movement system

## Edge Cases
1. Dash qua spike nhưng dash kết thúc giữa spike → death (invincibility hết giữa obstacle).
2. Jump qua wall nhưng wall cao quá (tốc độ cao → không kịp) → cần dash thay vì jump.
3. Gap + spike ngay sau → double jump rồi dash (combo).
4. Obstacle spawn ngay checkpoint → tránh (safe zone 3m trước/sau checkpoint).

## Dependencies
- Provides: obstaclePositions, obstacleTypes, collisionEvents → cho scoring (near-miss bonus), vfx
- Receives: speedMultiplier từ movement, currentPosition từ movement

## Tuning Knobs
- patternSpacing divisor: 7-12 — nhỏ hơn = dày hơn = khó hơn
- obstacleHeight: 1.0-2.0 — quá cao = jump không qua (frustration)
- gapWidth: 1.5-5.0 — quá rộng = dash bắt buộc, quá hẹp = không cần dash
- safe zone around checkpoint: 2-5m

## Acceptance Criteria
1. 4 obstacle types spawn đúng (wall, gap, low-bar, spike)
2. Mỗi pattern có ≥2 cách qua
3. Hit obstacle non-invincible = death
4. Hit obstacle invincible = pass through + VFX
5. Spawn density tăng theo speed tier
6. Safe zone 3m quanh checkpoint không spawn obstacle