Status: Approved
# Environment — GDD

## Overview
Track + visual theme (dark world → glow transformation). Phục vụ pillar 2 (speed = beauty) — môi trường phản ánh progress.

## Player Fantasy
Chạy qua thế giới tối tăm, mỗi segment sáng dần khi bạn tiến bộ. Thế giới = canvas cho glow của bạn.

## Detailed Rules
1. Track: linear, rộng 4m, dài vô hạn (spawn segments động).
2. Segment: 30m block, chứa 1-2 obstacle patterns + shard cluster.
3. Visual theme: dark base (unlit, #1a1a2e) → sáng dần theo glowIntensity.
4. Ambient light: base 0.2 → +0.6 × glowIntensity (player glow lights environment).
5. Ground: dark material with emissive grid lines (neon cyan) — brighter khi glow cao.
6. Skybox: dark gradient (#0a0a1a → #1a1a3e), stars appear khi glowIntensity > 0.5.
7. Parallax background: 2-3 layers far/mid, scroll theo player Z.

## Formulas
- segmentLength: 30 m (assets/data/environment.json)
- trackWidth: 4 m
- ambientBase: 0.2
- ambientGlowMultiplier: 0.6
- gridEmissiveBase: 0.3
- gridEmissiveGlowMultiplier: 2.0
- parallaxSpeed_far: 0.1 × playerSpeed
- parallaxSpeed_mid: 0.3 × playerSpeed

## Edge Cases
1. Segment spawn rate > despawn → memory leak. Despawn segment khi player > 60m qua.
2. GlowIntensity = 0 (chưa thu shard) → environment base dark, chỉ grid line sáng nhẹ.
3. GlowIntensity = 1.0 → environment sáng rõ, stars visible, grid line bright.

## Dependencies
- Provides: trackSegments, ambientLight, gridEmissive → cho visual feedback
- Receives: playerZ từ movement, glowIntensity từ collectibles, speedMultiplier từ movement

## Tuning Knobs
- segmentLength: 20-50 — quá ngắn = jarring spawn, quá dài = lặp lại
- ambientGlowMultiplier: 0.3-1.0 — quá cao = dark theme mất, quá thấp = glow không thấy
- parallaxSpeed ratios: 0.05-0.5 — quá chậm = không cảm nhận speed, quá nhanh = nausea

## Acceptance Criteria
1. Track linear, 4m wide, segments spawn động
2. Dark base + ambient light tăng theo glowIntensity
3. Grid lines emissive, brighter khi glow cao
4. Parallax 2-3 layers scroll theo player
5. Segments despawn khi player > 60m qua
6. Stars appear khi glowIntensity > 0.5