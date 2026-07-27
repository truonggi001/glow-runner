# Glow Runner — Handover Log

> Cập nhật: 2026-07-27 16:30
> Repo: github.com/truonggi001/glow-runner (9 commits)
> Unity 6000.5.5f1, macOS, MCP for Unity v10.1.0 port 6400
> Blender 5.1, Blender MCP port 9876 (connected)

## Pipeline Status

| Phase | Gate | Trạng thái |
|-------|------|-----------|
| P1 Concept | PASS | ✅ concept.md, systems-index.md |
| P2 Design | PASS | ✅ 6 GDD, entities.yaml, 6 data JSON, cross-review |
| P3 Slice | PASS | ✅ 11 scripts, scene, 4 evidence |
| P4 Production | — | ⚠️ 8/8 stories done NHƯNG art chưa hoàn chỉnh |
| P5 Ship | PASS | ✅ release-checklist, 3 playtests, balance-sim PASS, telemetry |
| P6 Earn | — | ✅ monetization plan, marketing plan |

**Lưu ý:** Gate ship PASS nhưng P4 art chưa xong thật — gate chỉ check artifacts tồn tại, không check chất lượng art.

## Bugs Còn Tồn (chưa fix)

### 1. Obstacle lệch vị trí
- **Nguyên nhân:** OBJ import từ Blender có scale/origin khác Unity primitive. ObstacleWall OBJ scale (1.5, 0.2, 1) trong Blender, nhưng ObstacleSpawner code scale lại = (trackWidth=4, heightWall, 0.5) → scale bị nhân đôi/sai.
- **Fix cần:** Hoặc export OBJ với scale chính xác, hoặc không scale lại trong code khi dùng OBJ. Cần normalize OBJ scale = 1x1x1 trong Blender trước khi export.
- **File:** ObstacleSpawner.cs:SpawnWall() line 92-93

### 2. Game "treo" giữa chừng
- **Nguyên nhân khả năng:** Player collider isTrigger=true trong DashRoutine() nhưng nếu player chết trong khi dashing, isTrigger không reset → player rơi xuyên ground → game stuck.
- **Hoặc:** OBJ obstacle có MeshCollider nhưng không có tag đúng → collision không register → player không chết nhưng cũng không vượt qua được.
- **Fix cần:** Thêm `col.isTrigger = false` trong OnDeath callback. Đảm bảo OBJ obstacle có BoxCollider + tag "Obstacle".
- **File:** PlayerController.cs:DashRoutine() line 148-149, 173-174

### 3. Player không glow
- **Nguyên nhân:** PlayerGlow.mat có _EmissionColor nhưng chưa EnableKeyword("_EMISSION"). Material tạo qua MCP manage_material có thể không enable emission keyword.
- **Fix cần:** Set material shader keyword via execute_code hoặc editor: `mat.EnableKeyword("_EMISSION")` trước khi SetColor.
- **File:** PlayerGlow.mat, PlayerController.cs:UpdateGlowVisual() line 231

### 4. Shard OBJ renderer ở child
- **Đã fix:** GetComponentInChildren thay vì GetComponent (commit 95e1a79)
- **Vẫn cần verify:** OBJ shard có collider không? Nếu không, Shard OnTriggerEnter không fire.

## Scripts (12 files trong Assets/Scripts/)

| Script | Chức năng | Trạng thái |
|--------|-----------|-----------|
| GameData.cs | Load JSON runtime | ✅ |
| PlayerController.cs | Auto-run + jump + dash + glow | ⚠️ dash isTrigger bug |
| GameManager.cs | Game states + scoring + BGM | ✅ |
| ObstacleSpawner.cs | Spawn 4 pattern types | ⚠️ OBJ scale lệch |
| CollectibleSpawner.cs | Spawn shard clusters | ✅ fixed GetComponentInChildren |
| UIController.cs | HUD + menu + death screen | ✅ |
| CameraFollow.cs | Camera follow player | ✅ |
| GroundScroll.cs | Infinite ground | ✅ |
| SFXHolder.cs | AudioClip holder + Play | ✅ |
| NearMissDetector.cs | Near-miss +1 shard | ✅ fixed tag error |
| ParallaxBackground.cs | 2-layer parallax | ✅ |
| ArtUpgrade.cs | Editor-only menu item | ⚠️ chưa chạy được |

## Art Assets

| Asset | Loại | Trạng thái |
|-------|------|-----------|
| PlayerGlow.mat | Material | ⚠️ emission chưa enabled |
| NeonRed.mat | Material | ✅ |
| NeonMagenta.mat | Material | ✅ |
| NeonOrange.mat | Material | ✅ |
| NeonCyan.mat | Material | ✅ |
| player.obj | 3D model | ⚠️ chưa gán vào Player GO |
| obstaclewall.obj | 3D model | ⚠️ scale lệch |
| shard.obj | 3D model | ⚠️ collider? |
| bgm.wav | Audio | ✅ |
| jump/dash/death/shard.wav | Audio | ✅ |

## Cần Làm Tiếp (Priority Order)

1. **Fix obstacle scale lệch** — Re-export OBJ từ Blender với scale 1x1x1, hoặc bỏ scale code trong SpawnWall khi dùng OBJ
2. **Fix game treo** — Đảm bảo isTrigger reset khi death, đảm bảo OBJ có BoxCollider + tag
3. **Fix player glow** — EnableKeyword("_EMISSION") trên PlayerGlow.mat
4. **Replace player capsule bằng player.obj** — Instantiate player.obj làm child của Player GO
5. **Verify shard collision** — Đảm bảo shard OBJ có collider trigger
6. **Playtest thật** — User chơi 30s+, chụp screenshot gameplay, cho verdict
7. **Tắt autoStartEnabled** trong GameManager.cs (đã set false)
8. **Clean up ArtUpgrade.cs** — Xoá hoặc làm cho menu item hoạt động

## Key Config Values

```
baseSpeed=8.0, jumpForce=9.8, dashDistance=4.0, dashDuration=0.4
dashInvincibility=0.6, dashCooldown=1.5
speedRampInterval=30, speedRampPercent=10, speedCapMultiplier=2.0
glowCap=10, scorePerMeter=1, scorePerShard=50
trackWidth=4.0, patternSpacingBase=10.0
```

## Tags
Player, Obstacle, Collectible, NearMissed

## CLI Commands

```bash
# Gate check
uv run --project /Users/minhson/AI/arsenal/tools/game-production python3 /Users/minhson/AI/arsenal/tools/game-production/cli.py check <gate> --game /Users/minhson/GameDev/glow-runner --json

# Balance sim
uv run --project /Users/minhson/AI/arsenal/tools/game-production python3 /Users/minhson/AI/arsenal/tools/game-production/cli.py sim --game /Users/minhson/GameDev/glow-runner --json

# Status
uv run --project /Users/minhson/AI/arsenal/tools/game-production python3 /Users/minhson/AI/arsenal/tools/game-production/cli.py status --game /Users/minhson/GameDev/glow-runner --json
```

## Blender MCP Workflow (proven)

```
1. Blender mở, N-panel → Blender MCP → Connect (port 9876)
2. execute_blender_code: tạo procedural mesh + materials
3. Export OBJ: bpy.ops.wm.obj_export(filepath=..., export_selected_objects=True)
4. Copy OBJ+MTL vào Assets/Resources/Models/ (cho Resources.Load)
5. Unity refresh, scripts dùng Resources.Load<GameObject>("Models/<name>")
6. GetComponentInChildren<Renderer/Collider> vì OBJ import có mesh ở child
```

## Lịch Sử Commits

1. Initial: P1+P2+P3 (concept, design, slice)
2. P4 Sprint 1: 6/8 stories
3. SFX fix: SFXHolder + glowCap 10 + scorePerShard 50
4. P4-P6: 8/8 stories + ship gate + earn
5. Art: neon materials + obstacle emissive + NearMissDetector fix
6. Art: Blender procedural models + OBJ import
7. Fix: Renderer/Collider GetComponentInChildren + PlayerGlow material
8. Fix: Resources.Load OBJ + BoxCollider fallback
9. (pending) Fix: obstacle scale + game treo + player glow