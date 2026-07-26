Status: Approved
# Scoring — GDD

## Overview
Distance + shard count + high score. Hệ thống đo lường cho player cảm giác tiến bộ. Phục vụ pillar 1 (flow — restart nhanh, "one more try").

## Player Fantasy
Mỗi lần chơi = 1 attempt ngắn. Xem con số tăng, so với lần trước, "lần sau hơn".

## Detailed Rules
1. Score = distance (m) + shardCount × 10.
2. Distance: đo liên tục theo Z position (1m = 1 point).
3. High score: lưu PlayerPrefs, hiển thị ở menu + death screen.
4. Death screen: show score + high score + "Try Again" (instant restart, <1s).
5. No leaderboard (scope: pilot, không có online).

## Formulas
- scorePerMeter: 1 (assets/data/scoring.json)
- scorePerShard: 10
- score: floor(distanceZ) + shardCount × scorePerShard
- highScoreKey: "glowrunner_highscore" (PlayerPrefs)

## Edge Cases
1. Dash forward +2m → distance tăng += 2 (tính vị trí thực, không speed).
2. Rơi khỏi track → distance freeze tại điểm rơi.
3. Restart → score reset 0, high score giữ nguyên.

## Dependencies
- Provides: score, highScore → cho game-states (menu/death display)
- Receives: distanceZ từ movement, shardCount từ collectibles

## Tuning Knobs
- scorePerShard: 5-20 — quá cao = shard quan trọng hơn distance, quá thấp = shard vô nghĩa
- restartTime: <1s (fixed — pillar 1)

## Acceptance Criteria
1. Score = floor(distanceZ) + shardCount × 10
2. High score lưu PlayerPrefs, persist qua restart
3. Death screen hiển thị score + high score
4. Restart <1s từ death screen
5. Score reset 0 khi restart, high score giữ