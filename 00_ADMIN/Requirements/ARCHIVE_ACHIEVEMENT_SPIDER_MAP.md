# Archive / Achievement Spider Map

Status: project artifact only. Do not ship this page or diagram inside the player-facing game package unless a future backlog item explicitly promotes an in-game archive map.

Purpose: show how the recovered archive threads connect to lore-based achievements and active buffs.

```mermaid
flowchart LR
  Archive["Recovered Archive"]

  Archive --> Witnesses["Witnesses Thread"]
  Archive --> Pattern["The Pattern Thread"]
  Archive --> Origins["Origins Thread"]
  Archive --> Starter["Starter Field Patterns"]
  Archive --> Future["Future / Unhinted Buffs"]

  Starter --> ParallaxCourier["Parallax Courier\ncyan Parallax beacon\nshort x2 speed boost"]
  Starter --> MassReceipt["Mass Receipt\ngold Parallax cache\ninstant hole-size increase"]
  Starter --> EveryoneFriend["Everyone's Friend\n10 pedestrians in 15 seconds\nCrowd Magnet"]

  Witnesses --> WIT4["WIT-4 Everyone's Friend"]
  WIT4 --> PedestrianPull["Pedestrian Pull / Crowd Magnet\nwider pull radius for 5 seconds"]

  Witnesses --> WIT6["WIT-6 Missing House Forum Post"]
  Witnesses --> WIT7["WIT-7 How I'm Still Here"]
  WIT6 --> ForumUser["The Forum User\nsurvive while staying low-profile"]
  WIT7 --> ForumUser

  Witnesses --> WIT8["WIT-8 Parks Complaint Form"]
  WIT8 --> TreeHugger["Tree Hugger / Tree Feast\ntrees score x2 for 20 seconds"]

  Pattern --> PAT6["PAT-6 Missing Microfiche Note"]
  PAT6 --> QuietBlock["The Quiet Block / Block Sweep\nall score x1.5 for 15 seconds"]

  Pattern --> PAT4["PAT-4 Linden Street Resolution"]
  PAT4 --> LindenStreet["Linden Street / Building Chain\nbuilding score bonus and instant mass"]

  Pattern --> PAT9["PAT-9 Bellmar Christmas Card"]
  PAT9 --> Bellmar["Bellmar\nwin without growing large"]

  Origins --> ORI1["ORI-1 Aresty Grant Excerpt"]
  ORI1 --> FirstBite["First Bite\nfirst eat is a person\npeople score +15 percent all round"]

  Origins --> ORI6["ORI-6 Brother A. Interview"]
  ORI6 --> TheQuiet["The Quiet / Quiet Bite\n30 seconds without eating\nnext bite scores x10"]

  Future --> FreeLunch["Free Lunch\nentry-level chain\nsurfaced through HUD, no lore doc required"]
  Future --> ReturningHole["The Returning Hole\ncross-session buff\nblocked until cross-session state exists"]

  QuietBlock --> BlockParty["Combo: Block Party\nCrowd Magnet + Block Sweep\npull plus score surge"]
  PedestrianPull --> BlockParty

  LindenStreet --> TooLoud["Combo: Too Loud\nBuilding Chain + Crowd Magnet\nbuilding score up, speed down"]
  PedestrianPull --> TooLoud
```

## Notes

- Starter Field Patterns stay visible in the Archive so new players can understand the first three useful patterns without solving lore clues.
- Other archive-linked achievements should remain discoverable through documents, not explained directly in the How to Play screen.
- This artifact is for product/design alignment, QA reviews, and future implementation planning.
