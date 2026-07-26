# Cross-Review — 2026-07-26

## Verdict: PASS (with minor concerns)

## Reviewed
- movement.md ✓
- obstacles.md ✓
- collectibles.md ✓
- scoring.md ✓
- game-states.md ✓
- environment.md ✓

## Findings

### Dependency check (2- chiều)
- movement → obstacles: obstacles đọc speedMultiplier từ movement ✓
- movement → collectibles: collectibles đọc playerPosition ✓
- movement → scoring: scoring đọc distanceZ ✓
- obstacles → collectibles: nearMissEvent từ obstacles ✓
- collectibles → scoring: shardCount ✓
- collectibles → environment: glowIntensity ✓
- scoring → game-states: score, highScore ✓
- game-states → tất cả: currentState (enable/disable) ✓

### Rule conflicts
- Không phát hiện mâu thuẫn rule giữa GDDs.

### Formula range check
- baseSpeed 8 m/s × cap 2.0 = 16 m/s max. Dash +2m trong 0.2s = 10 m/s burst. OK — dash nhanh hơn base nhưng không vượt cap.
- jumpForce 9.8 × 1.0 = 9.8 m/s. Với gravity -9.81, jump height ≈ 4.9m. Wall height 1.5m → jump qua dễ. OK.
- patternSpacing 10m / speedMultiplier. Ở cap (2.0×): 5m giữa patterns. baseSpeed 16m/s → 0.3s giữa patterns. Dash cooldown 1.5s → dash không dùng mỗi pattern. OK — phải chọn dash hoặc jump.

### Reward space overlap
- scoring: distance + shard×10. collectibles: near-miss +1 shard. Không giành cùng reward space — near-miss là bonus hiếm, shard thường là chính. OK.

### Pillar violations
- Pillar 1 (flow): restart <1s ✓, death instant ✓
- Pillar 2 (speed=beauty): glow system ✓, environment reacts to glow ✓
- Pillar 3 (one input, many decisions): mỗi pattern ≥2 cách qua ✓

### Concerns (không chặn)
1. movement GDD ghi "jumpForce: 9.8 m/s² × jumpMultiplier" — đơn vị hơi lẫn. Là force (N) hay velocity (m/s)? Nên clarify: đây là initial velocity (m/s), gravity áp dụng sau.
2. environment parallax cần asset art — chưa có spec art cho parallax layers. Slice phase sẽ cần.
3. entities.yaml ghi jumpForce: 9.8 nhưng movement.json ghi jumpForce: 9.8 + jumpMultiplier riêng — nhất quán nhưng nên clarify đây là velocity không phải force.