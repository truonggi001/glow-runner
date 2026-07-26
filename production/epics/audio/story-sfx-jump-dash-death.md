Status: Todo
# story-sfx-jump-dash-death

**Epic:** audio · **GDD ref:** game-states-menu-play-death-restart#detailed-rules

## Mô tả
SFX for jump, dash, death, shard collect. Procedural via Python stdlib.

## Acceptance Criteria
- [ ] Jump SFX: rising sine 400→800Hz, 0.2s
- [ ] Dash SFX: descending sweep 1200→200Hz, 0.3s
- [ ] Death SFX: low thud 80+120Hz, 0.5s
- [ ] Shard collect SFX: high ping 800→1200Hz, 0.1s
- [ ] All SFX play at correct events, volume balanced

## Asset cần
| Asset | Spec | Nguồn |
|---|---|---|
| jump.wav | Rising sine 0.2s | Python stdlib |
| dash.wav | Descending sweep 0.3s | Python stdlib |
| death.wav | Low thud 0.5s | Python stdlib |
| shard.wav | High ping 0.1s | Python stdlib |

## Notes triển khai
- Generate via Python wave+struct (like BGM)
- AudioSource on Player for SFX, separate from BGM on GameManager
- PlayOneShot at events