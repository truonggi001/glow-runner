Status: Todo
# story-environment-dark-theme

**Epic:** environment · **GDD ref:** environment-track-visual-theme#detailed-rules

## Mô tả
Dark base world (#1a1a2e) + ambient light tăng theo glowIntensity. Grid lines emissive neon cyan.

## Acceptance Criteria (từ GDD section 8)
- [ ] Dark base + ambient light tăng theo glowIntensity
- [ ] Grid lines emissive, brighter khi glow cao
- [ ] Stars appear khi glowIntensity > 0.5

## Notes triển kiến
- Ground material: dark color #1a1a2e + emissive grid texture (procedural PNG)
- Directional light: intensity = 0.2 + glowIntensity * 0.6
- RenderSettings.ambientLight = Color.Lerp(dark, bright, glowIntensity)
- Stars: simple particle system or skybox change at glowIntensity > 0.5