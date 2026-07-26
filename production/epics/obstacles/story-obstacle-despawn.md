Status: Todo
# story-obstacle-despawn

**Epic:** obstacles · **GDD ref:** obstacles-hazard-spawning-patterns#detailed-rules

## Mô tả
Obstacles despawn khi player đã qua 60m+ để tránh memory leak.

## Acceptance Criteria
- [ ] Obstacle despawn khi player.z - obstacle.z > 60
- [ ] No memory leak over long sessions (5+ min)

## Notes triển khai
- ObstacleSpawner maintain List<GameObject> activeObstacles
- Each Update: foreach obstacle, if player.z - obs.z > 60 → Destroy(obstacle)
- Remove from list after destroy