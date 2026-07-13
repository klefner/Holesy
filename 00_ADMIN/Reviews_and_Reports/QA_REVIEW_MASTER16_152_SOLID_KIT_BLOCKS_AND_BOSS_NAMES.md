# QA Review - Master 16.152 Solid Kit Blocks and Boss Names

## Defects Addressed

- Imported textured buildings were hollow clipped shells with no closed roof.
- Their destruction pieces resembled the skyscraper's unusual panels instead of physical blocks.
- Boss names were absent above active bosses.

## Repair

- The loaded kit model remains the material source, including its actual texture maps.
- Each edible building piece is now closed BoxGeometry with six faces, including roof and underside.
- Imported buildings use cubic physics pieces; skyscrapers retain their unique panel-style exception.
- Each boss type has exactly one permanent name mapped to its skin.
- A small outlined, depth-visible name sprite rides above the boss during descent and combat.
- Incoming boss messages include the same permanent name.

## Validation Focus

- Inspect all three rotating MegaKit building selections for a roof and solid appearance.
- Collapse them and verify cubic pieces fall, collide, settle, and are consumed like other building blocks.
- Verify every boss displays its name above its head during parachute descent and after landing.
- Verify the announcement name matches the name above the boss.
