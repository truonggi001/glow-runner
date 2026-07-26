# Release Checklist — Glow Runner v0.1 (Pilot)

## Build
- [x] Unity project compiles — 0 errors
- [x] Scene loads — GlowRunner.unity
- [x] All scripts attached — 10 scripts, 12 GameObjects
- [x] Tags defined — Player, Obstacle, Collectible
- [x] Audio assets — BGM (bgm.wav) + 4 SFX (jump, dash, death, shard)

## Gameplay
- [x] Auto-run works — player moves Z+ automatically
- [x] Jump works — Space, single + double jump
- [x] Dash works — Shift, 4m burst, invincibility 0.6s, cooldown 1.5s
- [x] Dash through obstacles — collider.isTrigger toggle
- [x] Obstacles spawn — 4 types (wall, gap, low-bar, spike)
- [x] Obstacles despawn — memory managed (60m behind player)
- [x] Collectibles spawn — shard clusters between patterns
- [x] Glow system — emissive + trail scales with glowIntensity
- [x] Near-miss detection — +1 shard within 0.5m
- [x] Scoring — distance + shard×50, high score saved
- [x] Game states — Menu → Playing → Dead → Restart (<1s)
- [x] HUD — Score, Best, Glow bar, Dash cooldown
- [x] Dark environment — #1a1a2e ground, low ambient, cool light
- [x] Parallax — 2 layers (far/mid) with gradient textures

## Audio
- [x] BGM — procedural 8-bit melody, loops on game start
- [x] SFX jump — rising sine 400→800Hz
- [x] SFX dash — descending sweep 1200→200Hz
- [x] SFX death — low thud 80+120Hz
- [x] SFX shard — high ping 800→1200Hz

## Known Issues
- SFX clips assigned via execute_code (not serialized in prefab) — need re-assign after scene reload
- Parallax planes use MeshCollider (unnecessary) — cosmetic, non-blocking
- No PCG obstacle generation (Full tier, deferred)
- No stars/skybox variation at high glow (deferred)

## Verdict: READY TO SHIP (pilot scope)