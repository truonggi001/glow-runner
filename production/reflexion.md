# Reflexion — Glow Runner

## Sprint 1 Lessons

1. **CollectibleSpawner existed but wasn't called** — ObstacleSpawner didn't call SpawnCluster. Fixed: added call after each pattern spawn. Lesson: integration between systems needs explicit wiring, not just GDD dependency declaration.

2. **FindFirstObjectByType not available in CodeDom** — execute_code with CodeDom compiler (C# 6) doesn't support Unity 6 new API. Use Object.FindObjectOfType or GameObject.Find instead. Lesson: execute_code CodeDom is limited — keep code simple.

3. **SFX loading via Resources.Load fails** — audio files in Assets/Audio/ not in Resources/ folder. PlaySFX fallback scans all AudioClips by name. Works but inefficient. Better: assign clips via SerializedObject or move to Resources/.

4. **Dark environment = immediate mood change** — switching from bright default to dark (#1a1a2e) + low ambient + cool light made the game feel completely different. Pillar 2 (speed=beauty) starts to emerge even with primitives.

5. **Glow visual needs material emission keyword** — mat.EnableKeyword("_EMISSION") required before SetColor("_EmissionColor"). Without keyword, emission silently ignored.