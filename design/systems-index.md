# Systems Index

> Tier: MVP (bắt buộc cho slice) / Full (bản đầy đủ). Thiết kế theo thứ tự dependency.

| System | Tier | Depends on | Status |
|---|---|---|---|
| movement (auto-run + jump + dash) | MVP | - | Not started |
| obstacles (hazard spawning + patterns) | MVP | movement | Not started |
| collectibles (light shards + glow system) | MVP | movement | Not started |
| scoring (distance + shard count + high score) | MVP | collectibles | Not started |
| game-states (menu → play → death → restart) | MVP | scoring | Not started |
| environment (track + visual theme) | MVP | movement | Not started |
| audio (music + SFX) | Full | - | Not started |
| vfx (glow particles + trail) | Full | collectibles | Not started |
| pcg (procedural obstacle generation) | Full | obstacles | Not started |