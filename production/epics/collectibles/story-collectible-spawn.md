Status: Todo
# story-collectible-spawn

**Epic:** collectibles · **GDD ref:** collectibles-light-shards-glow-system#detailed-rules

## Mô tả
Shard clusters spawn giữa obstacle patterns. Player chạm shard = auto-collect + glow tăng.

## Acceptance Criteria (từ GDD section 8)
- [ ] Shard auto-collect khi chạm player
- [ ] Shard clusters (3-5) spawn giữa patterns, cách 2-3m
- [ ] Shard không spawn trong obstacle path

## Asset cần
| Asset | Spec | Nguồn |
|---|---|---|
| shard_sphere | Sphere 0.3m, cyan emissive | Unity primitive |

## Notes triển khai
- CollectibleSpawner đã có SpawnCluster() nhưng ObstacleSpawner chưa call
- Cần: ObstacleSpawner gọi CollectibleSpawner.SpawnCluster(zCenter) giữa mỗi pattern
- Shard: trigger collider, tag "Collectible", OnTriggerEnter check tag "Player"