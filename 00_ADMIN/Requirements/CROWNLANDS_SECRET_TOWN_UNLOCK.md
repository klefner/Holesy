# Crownlands Secret Town Unlock

## Player Contract

Crownlands is the secret town. It is not part of automatic rotation and must not appear in the city selector before it is unlocked.

The player must earn all four proofs during one game run:

1. `first_bite`
2. `tree_hugger`
3. `quiet_block`
4. `linden_street`

Prior permanent achievement history does not count. Each achievement event must occur after the current run begins.

## Lifetime Rules

- Wave transitions and Endless world shifts preserve the run constellation.
- An explicit Endless save serializes the run constellation.
- Loading that save restores the same constellation.
- Starting a new game creates an empty constellation.
- Ending or abandoning an unsaved game discards incomplete progress.
- Completing the constellation permanently unlocks Crownlands.
- The permanent unlock controls future Crownlands rotation/selector eligibility once the Crownlands environment package is shipped.

## Archive Clues

- `crn-1`, *Margin Note in a Road Atlas*, is visible from the start.
- `crn-2`, *The Fifth Bell*, can enter the normal Archive recovery pool.
- `crn-3`, *A Road That Was Not There*, unlocks with Crownlands.

The clues describe the actions metaphorically and state that old medals do not satisfy the gate.

## Implementation Boundary

Master 16.185 implements the unlock contract, run/save lifetime, permanent gate, instrumentation, and Archive clues. Crownlands remains excluded from selection and rotation until its environment assets, population ladder, destruction set, and QA package are complete.
