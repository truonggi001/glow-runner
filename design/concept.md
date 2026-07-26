# Glow Runner — Concept

## Elevator Pitch
3D auto-runner where you time jumps and dashes through a neon gauntlet — the faster you go, the brighter you glow.

## Core Fantasy
You are a being of pure light sprinting through a dark world. Every obstacle you dodge, every shard you collect, makes you glow brighter. The world transforms from dark to radiant as you survive longer. You feel speed, flow, and the thrill of perfect timing.

## MDA
- Mechanics: auto-run (constant forward velocity), jump (single + double), dash (burst forward + brief invincibility), collect light shards
- Dynamics: react to obstacle patterns, time jumps precisely, decide when to dash (risk: dash into next obstacle vs reward: skip gap + invincibility), route through shard clusters, build momentum visual feedback (glow intensity)
- Aesthetics: sensation (speed, flow), challenge (timing precision), fantasy (becoming light)

## Core Loop
Auto-run → encounter obstacle pattern → jump/dash to avoid → collect light shard → glow brighter → survive segment → reach checkpoint (shard tally) → next segment (faster, harder) → repeat

Loop duration: 15-30s per segment (obstacle pattern → resolution → checkpoint feedback)

## Pillars (3, bất di bất dịch)
1. **Flow over frustration** — death is instant but restart is instant too; short loops, "one more try" pull. When deciding: always favor fast restart over punishment.
2. **Speed = beauty** — the core visual reward is your glow growing brighter with progress. When deciding art/VFX: glow transformation is the priority, not environmental detail.
3. **One input, many decisions** — only 2 buttons (jump, dash) but each encounter has multiple valid solutions. When designing obstacles: always allow 2+ solutions, never forced single-answer.

### Anti-pillars (game này CỐ TÌNH không làm)
- No combat (no enemies to fight — only environmental hazards)
- No inventory/upgrade shop (glow is purely visual, not a stat system)
- No story/narrative (pure gameplay, no cutscenes, no dialogue)
- No open world (linear track, not explorable)

## Target Audience
- Bartle types: Achiever (beat high score, perfect runs), Daredevil (speed, risk-taking)
- Platform: PC (Steam), mobile (portrait mode possible later)
- Comparable titles: Geometry Dash (rhythm-based auto-runner), Subway Surfers (3D auto-runner), Flappy Bird (one-button timing)
- USP: 3D auto-runner with glow transformation — your visual reward IS your progress meter. Most auto-runners are 2D or don't have visual progression tied to performance.