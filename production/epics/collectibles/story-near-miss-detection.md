Status: Todo
# story-near-miss-detection

**Epic:** collectibles · **GDD ref:** collectibles-light-shards-glow-system#detailed-rules

## Mô tả
Player bay qua obstacle trong 0.5m nhưng không chạm = +1 shard (reward skill).

## Acceptance Criteria (từ GDD section 8)
- [ ] Near-miss trong 0.5m = +1 shard
- [ ] Near-miss + dash = chỉ tính 1 lần (không stack)

## Notes triển khai
- Cần: khi player passes obstacle (Z beyond obstacle Z), check distance < 0.5m
- ObstacleSpawner track obstacles list, check each frame if player just passed
- Add to GameManager.ShardCount via AddShard(1)