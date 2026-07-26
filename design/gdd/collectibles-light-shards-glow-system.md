Status: Approved
# Collectibles — GDD

## Overview
Light shards dọc track + glow system. Phục vụ pillar 2 (speed = beauty) — thu shard = glow sáng hơn.

## Player Fantasy
Mỗi shard bạn thu makes you brighter — bạn thấy mình tiến bộ qua ánh sáng, không qua số đếm.

## Detailed Rules
1. Light shard: floating orb, tự thu khi chạm (auto-collect, không cần input).
2. Shard value: 1 shard = +1 glow point.
3. Shard clusters: 3-5 shards nhóm gần nhau (2-3m), giữa các pattern.
4. Glow visual: glowIntensity = min(glowPoints / 50, 1.0) — cap tại 50 shards.
5. Glow effect: particle trail intensity + emissive material multiplier.
6. Near-miss bonus: bay qua obstacle trong 0.5m nhưng không chạm = +1 shard (reward skill).

## Formulas
- shardValue: 1 (assets/data/collectibles.json)
- glowCap: 50 shards
- glowIntensity: min(glowPoints / glowCap, 1.0)
- glowEmissiveMultiplier: 1.0 + glowIntensity × 2.0 (material emission)
- trailParticleRate: 10 + glowIntensity × 50 (particles/sec)
- nearMissDistance: 0.5 m
- nearMissReward: 1 shard

## Edge Cases
1. Dash qua shard cluster → thu tất cả trong dash path (width 1m).
2. Shard spawn trong obstacle → KHÔNG (shard luôn ở safe path).
3. Glow cap đạt → shard vẫn thu nhưng glow không tăng thêm (chỉ cho scoring).
4. Near-miss + dash = chỉ tính 1 lần (không stack).

## Dependencies
- Provides: glowPoints, glowIntensity → cho scoring, vfx, environment (lighting)
- Receives: playerPosition từ movement, nearMissEvent từ obstacles

## Tuning Knobs
- glowCap: 30-100 — quá thấp = cap quá sớm, quá cao = glow tăng quá chậm
- nearMissDistance: 0.3-1.0 — quá rộng = dễ quá, quá hẹp = không thấy
- nearMissReward: 1-3 — quá cao = near-miss quan trọng hơn shard thường
- clusterSize: 2-7 — quá nhiều = oversaturate, quá ít = không đáng notice

## Acceptance Criteria
1. Shard auto-collect khi chạm player
2. Glow intensity = min(points/50, 1.0)
3. Emissive material multiply theo glowIntensity
4. Particle trail rate tăng theo glow
5. Near-miss trong 0.5m = +1 shard
6. Shard không spawn trong obstacle path