Status: Approved
# Game States — GDD

## Overview
Menu → Play → Death → Restart. Quản lý vòng đời 1 attempt. Phục vụ pillar 1 (flow over frustration — fast restart).

## Player Fantasy
Vào game ngay, chết = thử lại ngay. Không menu rườm rà, không loading.

## Detailed Rules
1. States: MENU → PLAYING → DEAD → (MENU or PLAYING).
2. MENU: title + high score + "Press Space to Start". Space → PLAYING.
3. PLAYING: gameplay active, HUD minimal (score + glow bar).
4. DEAD: death screen overlay (score, high score, "Try Again [Space]" / "Menu [Esc]").
5. Transition DEAD → PLAYING: <1s (pillar 1). Scene reload nhẹ (reset obstacles, player, score).
6. Transition DEAD → MENU: Esc.

## Formulas
- transitionTime_dead_to_playing: 0.5s (assets/data/game-states.json)
- transitionTime_menu_to_playing: 0.3s

## Edge Cases
1. Press Space trong DEAD state quá sớm (trong 0.5s) → queue, thực hiện sau transition time.
2. Alt-Tab trong PLAYING → auto-pause (Time.timeScale = 0).
3. Application quit trong PLAYING → save high score nếu cần.

## Dependencies
- Provides: currentState → cho tất cả systems (movement disable khi không PLAYING)
- Receives: deathEvent từ movement/obstacles, score từ scoring

## Tuning Knobs
- transitionTime: 0.3-1.0 — quá nhanh = jarring, quá chậm = phá pillar 1

## Acceptance Criteria
1. 4 states chuyển đúng (MENU→PLAYING→DEAD→PLAYING/MENU)
2. Space ở MENU → PLAYING
3. Death event → DEAD state + death screen
4. Space ở DEAD → PLAYING (<1s)
5. Esc ở DEAD → MENU
6. Movement/obstacles chỉ active khi PLAYING