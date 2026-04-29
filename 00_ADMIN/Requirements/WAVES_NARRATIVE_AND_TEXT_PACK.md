## Waves Narrative And Text Pack

Purpose:

- preserve the agreed story direction for Waves mode before implementation
- provide a concrete replacement text pack for wave intros, transitions, and end-state messaging
- give Priority 2 work a reusable narrative reference so future copy updates do not need to be reinvented

Status:

- design reference only
- not yet implemented in code

## Narrative Intent

Waves mode should feel like a contained disaster escalating in recognizable stages, not like disconnected flavor text.

Recommended fiction:

- the holes are not ordinary sinkholes
- they are manifestations of an extradimensional feeding event
- the city and military do not fully understand the phenomenon at first
- between waves, the active breach zone is reconstituting matter and repopulating the battlefield
- the `Parallax` is the working name for the underlying anomaly or breach event

Recommended tone:

- sci-fi disaster
- ominous military escalation
- clear, concrete stakes
- no vague civilian-reaction wording unless it directly supports gameplay

## Escalation Ladder

Wave 1:

- the district is still functioning like a normal day
- civilians, traffic, and city clutter dominate the battlefield
- the threat is real, but not yet fully understood

Wave 2:

- the city recognizes the event as an active emergency
- containment teams and first troop deployments enter the district
- the battlefield becomes more hostile and intentional

Wave 3:

- evacuation and hard containment are underway
- the city is no longer trying to preserve normal life inside the zone
- resistance is faster, more coordinated, and more lethal

Wave 4:

- the district has been sealed and written off as recoverable civic space
- command is not "sending everything" in a contradictory last-minute sense
- instead, command is converging the remaining authorized force already committed to final containment
- the feeling should be terminal lockdown, not heroic rescue

## Copy Principles

- keep each message readable in a single pass during active play
- every message should communicate a gameplay-relevant escalation
- avoid vague nouns like `witnesses` unless the fiction clearly defines them
- avoid contradictory framing like "written off" paired with "now they send everything they have"
- transition text should explain why the battlefield resets and why the pressure increases

## Reading Time Guidance

The current wave messages likely need a longer dwell time.

Recommended implementation guidance for future Priority 2 work:

- wave intro / threat briefing messages should stay visible for about 6.5 to 7.5 seconds
- between-wave transition messages should stay visible for about 7.5 to 9.0 seconds
- if transition timing is lengthened, the next-wave start delay should also be lengthened enough that the player can finish reading before control resumes
- copy should stay punchy enough that mobile players can still read it comfortably

These are implementation recommendations, not yet-approved code changes.

## Replacement Text Pack

### Wave Intro Banners

Wave 1 banner:

- `WAVE 1 - DISTRICT OPEN`

Wave 2 banner:

- `WAVE 2 - CONTAINMENT BREACH`

Wave 3 banner:

- `WAVE 3 - EVACUATION ZONE`

Wave 4 banner:

- `WAVE 4 - FINAL LOCKDOWN`

### Wave Threat Briefings

Wave 1 briefing:

- `Wave 1: Downtown is still moving like a normal day. Feed before the city understands what you are.`

Wave 2 briefing:

- `Wave 2: The emergency is now public. Troops are entering the district to contain the breach.`

Wave 3 briefing:

- `Wave 3: Evacuation is underway. The streets are clearing, and the response is faster and deadlier.`

Wave 4 briefing:

- `Wave 4: The district is sealed. Remaining containment forces are converging for a final lockdown.`

### Between-Wave Transition Messages

Into Wave 2:

- `Sirens take the district. As the Parallax rebuilds the zone, the first containment units move in.`

Into Wave 3:

- `The Parallax knits the battlefield back together. Civilians are being pushed out, and hard containment is taking over.`

Into Wave 4:

- `The district reforms one last time inside the breach. Command has sealed the perimeter and ordered final containment.`

### Waves End-State Messages

Player clears all four waves:

- title: `Containment Survived.`
- subtitle: `You outlasted the lockdown and devoured the district under maximum pressure.`

Player is eliminated during Waves mode:

- title: `Containment Held.`
- subtitle: `The breach stayed active, but your hole did not survive the crackdown.`

Single rival survives Waves mode:

- title: `Another Breach Survived.`
- subtitle: `You were consumed before the district finished collapsing.`

Round ends with player alive but not sole survivor:

- title: `Lockdown Complete.`
- subtitle: `The district is spent. Final standings are listed below.`

## Optional Alternate Tone

If later playtesting suggests the current tone is too severe, the safest alternate direction is:

- heightened arcade disaster
- still ominous, but less lore-heavy
- same escalation logic, simpler wording

That alternate should still keep the same core story structure:

- normal city
- emergency recognition
- evacuation and hard containment
- sealed final lockdown

## Open Decisions For Priority 2

- whether `Parallax` stays the in-world name or becomes a more specific phenomenon label
- whether Waves end-state messaging should emphasize survival, score, or both
- whether banner copy should stay all-caps for arcade tone or soften slightly for readability
- whether the city is rebuilding actual matter, looping a district snapshot, or manifesting a fresh breach-layer each wave

## Implementation Reminder

When Priority 2 work resumes, use this document as the copy baseline before changing:

- wave intro banners
- `waveThreatBriefing(...)`
- `waveTransitionLore(...)`
- Waves-specific end-state messaging
