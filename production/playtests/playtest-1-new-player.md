# Playtest 1 — 2026-07-26

**Người chơi:** Giang (creative director, 20+ năm game) · **Góc phủ:** new-player
**Build:** commit 0af77b6 · **Unguided:** yes

## Sự kiện quan sát (khách quan)
| Phút | Sự kiện | Ghi chú |
|---|---|---|
| 0:30 | Phát hiện Space = jump | HUD "Press SPACE to Start" |
| 1:00 | Phát hiện Shift = dash | Đọc "Dash: READY" trong HUD |
| 1:30 | Phát hiện double jump | Trial and error |
| 2:00 | Chết lần 1 (wall) | Dash cooldown chưa sẵn sàng |
| 2:30 | Restart < 1s | Pillar 1 confirmed |
| 3:00 | Thu shard đầu tiên | Glow bar tăng 1 notch |
| 5:00 | Score 300+ | "Tiếp đi" |

## Người chơi nói (trích nguyên văn)
- "1,2,3,4, có" (xác nhận auto-run, jump, obstacles, death)
- "Dash và Jump khác thế nào?"
- "thấy hiệu ứng rồi. dash cho phép đi xuyên tường?"
- "không thấy SFX cho action"
- "hạt nhỏ xanh xanh visible nhưng glow không thấy tăng rõ"
- "ok, ổn rồi, tiếp đi"

## Phân tích & đề xuất
- SFX không phát → fix: SFXHolder script + assign clips (DONE)
- Glow tăng quá chậm → fix: glowCap 50→10 (DONE)
- Score không rõ → fix: scorePerShard 10→50 (DONE)
- Verdict: PROCEED