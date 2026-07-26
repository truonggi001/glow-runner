Status: Todo
# story-glow-visual

**Epic:** collectibles · **GDD ref:** collectibles-light-shards-glow-system#formulas

## Mô tả
Glow intensity tăng theo shard count → player emissive material + trail particle rate tăng.

## Acceptance Criteria (từ GDD section 8)
- [ ] Glow intensity = min(points/50, 1.0)
- [ ] Emissive material multiply theo glowIntensity
- [ ] Particle trail rate tăng theo glow

## Notes triển khai
- PlayerController đọc GameManager.GlowIntensity
- Material: mat.SetColor("_EmissionColor", baseEmission * (1 + glowIntensity * 2))
- TrailRenderer: trail.time = 0.3 + glowIntensity * 0.5 (longer trail when brighter)