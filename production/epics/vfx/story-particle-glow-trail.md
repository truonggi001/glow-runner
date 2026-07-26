Status: Todo
# story-particle-glow-trail

**Epic:** vfx · **GDD ref:** collectibles-light-shards-glow-system#formulas

## Mô tả
Player trail particle system scales with glow intensity. More glow = more particles + longer trail.

## Acceptance Criteria
- [ ] Trail particle rate = 10 + glowIntensity × 50
- [ ] Trail color shifts from green (low glow) to white-cyan (high glow)
- [ ] Visible difference between glow=0 and glow=1

## Notes triển khai
- PlayerController update trail in Update: trail.startColor = Color.Lerp(green, white, glowIntensity)
- Particle emission rate via ParticleSystem.emission.rateOverTime