# Session State — Glow Runner

## Current Phase
slice (P3) — vertical slice: 1 vòng chơi end-to-end bằng pipeline thật

## Completed
- P1 CONCEPT: concept.md + systems-index.md → gate PASS
- P2 DESIGN: 6 GDD-lite (movement, obstacles, collectibles, scoring, game-states, environment) + entities.yaml + 6 data JSON + cross-review → gate PASS

## Next Steps
1. Load `references/phase-3-slice.md`
2. Create Unity project (GitHub-first workflow)
3. Implement core systems: movement → obstacles → collectibles → scoring → game-states
4. Build headless → evidence/build.log
5. Run play-mode test → evidence/test-report.xml
6. Render screenshot → evidence/screenshot-*.png + vision-verdict.json
7. Human playtest → evidence/human-played.md
8. Run `check slice`

## Pilot Findings (for skill update)
1. CLI needs absolute paths (no ~ expansion)
2. GDD filename must match systems-index slug EXACTLY (including parenthetical) — gate check is filename-based
3. Cross-review template works well for catching dependency issues
4. entities.yaml + data JSON separation is clean — formulas reference data, not hardcode