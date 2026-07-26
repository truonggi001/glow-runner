# Human Playtest — Glow Runner

## Player: Giang Nguyen Truong (Creative Director)
## Date: 2026-07-26
## Session: 1 (unguided, ~5 min)

## Setup
- Unity Editor Play mode, keyboard input
- No instructions given — player discovered controls by trial

## Observations

### Controls discovered
- Space = jump (discovered immediately)
- Shift = dash (discovered after ~30s, noticed "Dash: READY" in HUD)
- Double jump discovered after ~1 min

### Core loop experience
- Player auto-runs — confirmed working
- Obstacles spawn and approach — confirmed
- Jump over walls/spikes — works
- Dash through walls — works (with visual feedback: trail + color flash + particles)
- Death on obstacle contact (non-dash) — works, death screen appears
- Restart via Space — works, <1s

### Issues found
1. Ground initially fell away (fixed with GroundScroll)
2. Dash initially not visually distinct (fixed with trail + color + particles)
3. Dash distance/duration felt short (fixed: doubled to 4m/0.4s)
4. Collectibles (shards) not spawning yet — CollectibleSpawner exists but SpawnCluster not called from ObstacleSpawner

### Fun factor
- Core auto-run + jump + dash loop is engaging
- Dash-through-obstacle feels satisfying (invincibility + visual feedback)
- Speed ramp adds tension over time
- Death → instant restart keeps "one more try" pull (pillar 1 confirmed)

## Verdict: PROCEED

The core fantasy (sprinting as light, dodging obstacles) is experienced within first 30 seconds. The core loop works. Missing: collectible spawning (non-blocking for slice — gameplay is fun without). Dash mechanic adds meaningful decision (dash to skip or jump to avoid).

Recommend: proceed to P4 PRODUCTION to flesh out collectibles, add environment visuals, polish.