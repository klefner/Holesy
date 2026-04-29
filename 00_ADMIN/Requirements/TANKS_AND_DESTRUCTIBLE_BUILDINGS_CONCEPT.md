## Tanks And Destructible Buildings Concept

Purpose:

- preserve the product idea for tanks, destructive shelling, and building-value degradation
- capture the relationship between military escalation, scoring, destructible buildings, and future debris behavior
- give the backlog a concrete design reference before implementation work begins

Status:

- concept only
- not implemented in code

## Core Idea

Add a heavier military unit type: `tank`.

Tanks should feel fundamentally different from soldiers:

- longer effective firing range
- dramatically higher radial damage than soldiers
- able to fire through buildings instead of requiring clean line of sight
- capable of changing the economic value of the battlefield by damaging buildings before the player eats them

This creates a strong Holesy-specific tension:

- tanks are not only a direct threat to holes
- they also destroy future score opportunity
- the player is pressured to intervene quickly, route more precisely, and protect valuable city mass before it is devalued or erased

## Tank Behavior Goals

System-level combat tuning rule:

- reload speed should be an explicit variable for every offensive combat unit type
- combat units should not all converge on the same DPS shape
- different units should express pressure through different mixes of:
  - reload speed
  - fire rate
  - direct-hit damage
  - splash or radial damage
  - range
  - line-of-sight rules

Desired combat behavior:

- tanks should engage holes from farther away than soldiers
- tank shells should feel heavy, dangerous, and unmistakable
- a tank hit should matter by magnitudes more than a soldier hit
- tanks should not be blocked by buildings when choosing to fire
- tank fire should be able to hit both the hole target area and any building along or near that firing path

Recommended DPS identity:

- soldiers should fire at the fastest rate
- soldiers should do relatively small damage per hit
- soldiers may still have higher sustained DPS than tanks because of that rate advantage
- tanks should fire more slowly
- tanks should do much larger damage per shot
- tanks should feel especially punishing on direct hits, even if their sustained DPS is lower than soldier DPS in some scenarios

Recommended navigation identity:

- tanks should be limited to roads
- tank movement speed should be slightly faster than soldier movement speed

Recommended world interaction:

- when tanks make contact with cars, the cars are destroyed
- tank movement should therefore feel weighty and disruptive, not delicate
- crushed-car behavior should reinforce the idea that tanks reshape the battlefield as they advance

Open implementation choice:

- whether tanks fire true projectiles with travel time
- or whether tanks fire simulated shells with impact resolution at the destination

Either way, the player-facing result should feel like:

- heavy artillery
- not rifle fire

## Building Damage Model

Recommended first model:

- buildings have discrete damage states instead of continuously simulated structural destruction
- each tank hit that meaningfully connects to a building advances it one damage state
- each damage state lowers that building's score value when consumed
- the fifth hit destroys the building entirely

Recommended five-stage progression:

1. pristine
2. lightly damaged
3. visibly damaged
4. heavily damaged
5. critical / near-collapse
6. destroyed on the next qualifying hit

If implementation prefers exactly five total visual states, then map it as:

1. pristine
2. damaged
3. more damaged
4. severe damage
5. destroyed

The important gameplay rule is:

- more tank damage means less value remains for the player to collect

## Building Value Degradation

Recommended product rule:

- every qualifying tank hit reduces future building payout
- the loss should be meaningful enough that the player notices and cares
- the reduction should be visible in both score outcome and visual presentation

Recommended tuning direction:

- early damage states reduce value moderately
- later damage states reduce value aggressively
- final destruction removes the building as a meaningful collectible

That supports the emotional loop:

- "I need to get there before the tank ruins this block"

## Visual Damage Direction

Two valid approaches:

### Option A — Discrete damaged models

- each hit swaps the building to a more damaged visual variant
- simpler to reason about
- easier to tune and test
- aligns cleanly with discrete score-value steps

### Option B — Continuous visible damage

- building damage appears progressively instead of as model swaps
- more immersive
- more expensive and more complex

Recommended first implementation:

- start with discrete damaged variants
- only revisit continuous damage later if the simpler model proves insufficient

## Debris / Ragdoll Ambition

Stretch goal with high excitement value:

- tank hits can blow visible building chunks outward
- those chunks fall to the ground and ragdoll or physics-bounce
- while on the ground they produce no score value if consumed
- after a short delay they flash a few times and disappear

This is specifically attractive because it creates:

- spectacle
- urgency
- battlefield change over time
- strong visual proof that tanks are permanently altering the city

Important dependency note:

- this debris vision overlaps directly with Priority 3 physics stack / collapse work
- do not treat it like a low-risk isolated unit addition

## Product Value

Why this idea matters:

- makes military escalation more varied than "more soldiers"
- creates a new kind of threat: economic denial
- creates a clearer per-unit combat identity instead of treating all threats as variations of the same rifle-pressure model
- gives the player another reason to move aggressively and prioritize targets
- increases battlefield drama and memorable moments
- creates a natural bridge into future physics/debris systems

## Suggested Backlog Placement

This concept spans multiple areas:

- military unit expansion
- building damage and scoring
- debris / collapse spectacle

Recommended practical placement:

- reference it from Priority 3 because destructible buildings and debris depend on heavier systems work
- also treat it as a future military escalation feature layered on top of the current soldier system

## Open Design Questions

- when do tanks first appear: only Wave 4, or earlier on high difficulty
- what reload-speed / fire-rate variables should be standardized across all future offensive unit types
- how many tank hits should a building survive by class: small, mid, large, skyscraper
- does a tank shell do splash damage to nearby holes, or only direct impact damage
- can tanks damage or kill soldiers / rivals incidentally
- can the player consume tanks directly, and if so at what minimum size
- should damaged buildings visually telegraph reduced score value before consumption
- should destroyed buildings leave zero-value rubble, temporary debris, or both

## Implementation Reminder

When this work is revisited, use this concept before changing:

- military unit roster
- building-value logic
- destructible-building visuals
- debris persistence / disappearance rules
