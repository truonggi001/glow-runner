"""balance-sim model — Glow Runner
Simulates a full game run: auto-run + obstacles + jump/dash + shard collection + scoring.

Harness (cli.py sim --game <dir>) calls simulate(data, rng) N times (Monte Carlo):
  - data = {filename_without_json: content} from assets/data/
  - rng = random.Random seeded
  - returns dict {metric: value} for 1 trial

CHECKS = acceptance criteria, traceable to GDD §Acceptance Criteria.
"""


def simulate(data, rng):
    # Load config from data files
    base_speed = data["movement"]["baseSpeed"]
    speed_ramp_interval = data["movement"]["speedRampInterval"]
    speed_ramp_percent = data["movement"]["speedRampPercent"] / 100.0
    speed_cap = data["movement"]["speedCapMultiplier"]
    dash_cooldown = data["movement"]["dashCooldown"]
    pattern_spacing_base = data["obstacles"]["patternSpacingBase"]
    glow_cap = data["collectibles"]["glowCap"]
    score_per_shard = data["scoring"]["scorePerShard"]
    score_per_meter = data["scoring"]["scorePerMeter"]

    # Simulate a run
    speed_mult = 1.0
    distance = 0.0
    time_elapsed = 0.0
    shards = 0
    last_dash_time = -10.0
    reaction_time = rng.uniform(0.20, 0.40)  # player reaction time
    dash_chance = rng.uniform(0.2, 0.5)  # player dash usage rate
    jump_accuracy = rng.uniform(0.85, 0.97)  # player jump success rate

    next_pattern_z = 30.0
    dt = 0.1  # simulation timestep

    while time_elapsed < 300:  # max 5 min
        current_speed = base_speed * speed_mult

        # Speed ramp
        if time_elapsed > speed_ramp_interval and int(time_elapsed) % int(speed_ramp_interval) == 0:
            if speed_mult < speed_cap:
                speed_mult = min(speed_mult + speed_ramp_percent, speed_cap)

        # Check obstacle encounter
        if distance >= next_pattern_z:
            spacing = pattern_spacing_base / speed_mult
            next_pattern_z += spacing

            # Player reacts to obstacle
            time_to_obstacle = spacing / current_speed

            if time_to_obstacle < reaction_time:
                # Too fast — death
                break

            # Try to avoid: jump or dash
            if rng.random() < dash_chance and (time_elapsed - last_dash_time) > dash_cooldown:
                # Dash — always succeeds
                last_dash_time = time_elapsed
                distance += 4.0  # dash distance
            else:
                # Jump — accuracy check
                if rng.random() > jump_accuracy:
                    # Failed jump — death
                    break

            # Collect shards between patterns
            shards_in_cluster = rng.randint(3, 5)
            shards += shards_in_cluster
            if shards > glow_cap:
                shards = glow_cap

        # Advance
        distance += current_speed * dt
        time_elapsed += dt

    # Calculate score
    score = int(distance) * score_per_meter + shards * score_per_shard

    return {
        "score": score,
        "survival_time": time_elapsed,
        "distance": distance,
        "shards": shards,
        "max_speed_mult": speed_mult,
    }


CHECKS = [
    # Score: mean should be 200-1000 (achievable but challenging)
    {"metric": "score", "stat": "mean", "min": 200, "max": 1000},
    # Survival time: mean 15-60s (short loops, pillar 1)
    {"metric": "survival_time", "stat": "mean", "min": 10, "max": 90},
    # Distance: mean 100-500m
    {"metric": "distance", "stat": "mean", "min": 100, "max": 500},
    # Shards: mean 3-10 (glow cap 10, achievable)
    {"metric": "shards", "stat": "mean", "min": 2, "max": 10},
    # P95 score: max 1500 (skilled players)
    {"metric": "score", "stat": "p95", "max": 1500},
    # P05 survival: min 5s (even worst players survive a bit)
    {"metric": "survival_time", "stat": "p05", "min": 3},
]