# Balance report — PASS

Monte Carlo 1000 trials, seed 0, data: collectibles, environment, game-states, movement, obstacles, scoring

| Metric | mean | p05 | p50 | p95 |
|---|---|---|---|---|
| score | 608.953 | 30.0 | 630.0 | 848.0 |
| survival_time | 17.7457 | 3.8 | 14.8 | 34.9 |
| distance | 162.6482 | 30.4 | 130.4 | 348.4 |
| shards | 8.935 | 0.0 | 10.0 | 10.0 |
| max_speed_mult | 1.2214 | 1.0 | 1.0 | 2.0 |

| Check | bound | value | OK |
|---|---|---|---|
| score.mean | ≥200 ∧ ≤1000 | 608.953 | ✓ |
| survival_time.mean | ≥10 ∧ ≤90 | 17.7457 | ✓ |
| distance.mean | ≥100 ∧ ≤500 | 162.6482 | ✓ |
| shards.mean | ≥2 ∧ ≤10 | 8.935 | ✓ |
| score.p95 | ≤1500 | 848.0 | ✓ |
| survival_time.p05 | ≥3 | 3.8 | ✓ |

Số nằm trong `assets/data/`; check ✗ → chỉnh data (không chỉnh check cho vừa số) hoặc đưa user quyết đổi Acceptance Criteria trong GDD.
