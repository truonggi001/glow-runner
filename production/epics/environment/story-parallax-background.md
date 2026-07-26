Status: Todo
# story-parallax-background

**Epic:** environment · **GDD ref:** environment-track-visual-theme#formulas

## Mô tả
2-3 parallax layers (far/mid) scroll theo player Z với tốc độ khác nhau.

## Acceptance Criteria (từ GDD section 8)
- [ ] Parallax 2-3 layers scroll theo player
- [ ] Far layer speed = 0.1 × playerSpeed, mid = 0.3 × playerSpeed

## Notes triển khai
- 2 planes behind track, textured (procedural gradient PNG)
- Move opposite to player Z at parallaxSpeed rate
- Wrap position when player moves far enough