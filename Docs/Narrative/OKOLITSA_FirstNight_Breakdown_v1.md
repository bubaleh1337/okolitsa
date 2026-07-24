# OKOLITSA — First Night Breakdown v1

## Current Narrative Source

Based on:
- "Околица. Виденье моей игры_v1.docx"
- Current Unity prototype state as of 2026-07-18

## Core Direction

OKOLITSA is not a quest-chain horror game.

The game is about Andrey trying to move through a familiar apartment while his perception, memory, fear, addiction, and family trauma make reality unreliable.

The main fear is not simply darkness or a monster.

The main fear is:
"I know this place, but I do not know what is behind the corner anymore."

## First Playable Title

v0.2 — First Night Prototype

## First Playable Scope

The first playable covers the beginning of Andrey's night:

1. Wake up after sleep paralysis.
2. Regain control in the living room / bedroom.
3. Hear a sound from the kitchen.
4. Check the hallway, corridor, and kitchen.
5. Go to the balcony to smoke.
6. See the field, forest, and Liza's grave cross.
7. Return to the apartment.
8. Experience a memory / hallucination / reality-break involving blood and the grandmother.
9. Decide to leave the apartment.
10. Descend through the stairwell.
11. Exit into rain.
12. End near the start of the path toward the cross.

## Core Player Verbs

- Wake up
- Look
- Listen
- Check corners
- Turn lights on
- Open / close doors
- Pick up simple domestic objects
- Go to the balcony
- Observe the field
- Leave the apartment
- Move carefully through the stairwell

## Current Prototype Systems To Keep

- First-person movement
- Interaction system
- Apartment lights
- Door interaction and auto-close
- Footstep system
- Stairwell flickering lamp
- Outdoor fog
- Basic field terrain
- Objective text prototype
- Interaction prompt UI

## Current Prototype Systems To Demote

These systems are useful, but not the narrative center anymore:

- Candle episode
- Automatic light failure sequence
- Window knock sequence
- Balcony door check objective

They should be kept as reusable tools, not treated as the final opening structure.

## First Playable Out of Scope

- Full hospital interior
- Full Yrka AI
- Full grandmother character model
- Full inventory system
- Complex cutscenes
- Dialogue system
- Multiple endings
- Advanced UI
- Large map expansion

## Next Unity Goal

Rebuild the current prototype flow into the First Night sequence:

Wake up → apartment check → balcony ritual → reality break → leave apartment → stairwell descent → rain outside → cross direction.

## Approved Sequence Segmentation

Narrative source:
- "Околица. Виденье моей игры_v2_sequence-map.docx"

### FN-00 — World and Character Backstory
Mode: Lore / Backstory

Introduces the apartment block, Black River district, abandoned hospital, Liza's grave, Andrey, Alexandra, the apartment history, and the childhood legends surrounding the field.

### FN-01 — Sleep Paralysis
Mode:
- Cutscene — No Control
- Hybrid — Look Only

Andrey opens his eyes, gradually perceives the black figure, and experiences paralysis while the creature approaches.

### FN-02 — Real Awakening and Kitchen Impact
Mode:
- Cutscene — No Control
- Hybrid — Look Only

Andrey wakes, sits on the bed, recovers his breathing, studies the room, and hears an impact from the kitchen.

### FN-03 — First Apartment Investigation
Mode: Gameplay — Full Control

The player turns on apartment lights, takes the shoehorn, checks the blind corners, investigates the corridor and kitchen, and confirms that no visible intruder is present.

### FN-04 — First Balcony Ritual
Mode:
- Gameplay — Full Control
- Hybrid — Look Only

The player collects cigarettes and a lighter, enters the balcony, and observes the field, forest, moon, and Liza's grave while Andrey smokes.

### FN-05 — Presence Behind the Player
Mode:
- Gameplay — Full Control
- Cutscene — No Control

The player searches the balcony surroundings. Andrey then closes his eyes, crosses himself, and tries to reject what he has perceived.

### FN-06 — Altered-State Transition
Mode:
- Cutscene — No Control
- Transition

Andrey returns to the apartment and enters an altered state. The sequence is narrative and non-interactive. It must not be implemented as a detailed player-operated procedure.

### FN-07 — Blood Trail and Grandmother
Mode:
- Cutscene — No Control
- Gameplay — Full Control
- Hybrid — Look Only

Andrey regains awareness, discovers the blood trail, follows it through the apartment, and encounters Alexandra in the kitchen doorway.

### FN-08 — Reality Reset
Mode: Gameplay — Full Control

Andrey wakes in the corridor, searches the kitchen and living room, and finds no physical evidence of the blood or apparition.

### FN-09 — Second Balcony Decision
Mode:
- Hybrid — Look Only
- Cutscene — No Control

Andrey studies Liza's grave again, notices an unfamiliar object on the cross, loses confidence in his perception of time, and decides to leave the building.

### FN-10 — Escape Preparation
Mode: Gameplay — Full Control

The player extinguishes the cigarette, enters the apartment, takes the coat and keys, exits, and locks the apartment door.

### FN-11 — Stairwell Descent
Mode: Gameplay — Full Control

The player descends from the fifth floor through the dim stairwell and exits the building.

### FN-12 — Storm and Cross Reveal
Mode:
- Hybrid — Look Only
- Gameplay — Full Control

Lightning reveals the cross in the field. Control returns immediately and the player begins moving toward it.

## Implementation Rule

Narrative beats must not be implemented as one monolithic episode controller.

Each sequence must use:
- reusable player-control states;
- independent Timeline assets;
- small scene-bound components;
- signals or explicit events;
- reusable gameplay systems;
- content stored separately from general-purpose systems.

## Canonical Story Truth

## Real-Event Narrative Foundation

OKOLITSA is a fictional psychological horror game built on a substantial real-life foundation.

### Real Sources

The following elements are inspired by or based on real experiences:

- the apartment and its unusual layout;
- the apartment's connection to the author's great-grandmother;
- family memories surrounding illness, death, and the discovery of the body;
- Soviet furniture, household objects, photographs, and domestic rituals;
- recurring sleep paralysis;
- visual and auditory experiences associated with sleep paralysis;
- stories and fears shared by members of the author's family.

### Fictional Transformation

The game is not a documentary reconstruction.

Real memories may be:

- combined;
- reordered;
- reassigned to fictional characters;
- visually altered;
- expanded through supernatural fiction;
- used symbolically rather than literally.

Andrey, the endless night, the post-mortem journey, Yrka, the hospital experiments, and the final supernatural structure belong to the fictional narrative.

### Creative Principle

Authenticity should come from specific domestic details, emotional truth, spatial memory, sound, and family history.

The project should avoid relying on generic Soviet-horror decoration when a more specific real-life detail is available.

### Public Positioning

Recommended public wording:

> Inspired by real events, family memories, personal experiences, and a real apartment.

Exact addresses and identifying private information must not appear in public builds or promotional materials.

### Endless Night

The entire game takes place during one endless night.

There is no normal daytime and no traditional day/night cycle. Narrative progression is represented through changing night phases, weather, environment states, sound, lighting, and supernatural escalation.

### Andrey's Death

Andrey dies from an accidental overdose during the opening night.

He did not intend to die and does not understand that he is dead.

The world experienced after this moment is a post-mortem or liminal reality in which physical spaces, memories, ghosts, and supernatural entities can overlap.

Andrey and the player discover the truth only during the final reveal.

### Apartment as a Memory Archive

The apartment is a major narrative location and must contain interactive Soviet-era domestic objects, family belongings, photographs, albums, diaries, and personal documents.

Objects should primarily support observation, memory, atmosphere, and environmental storytelling rather than arbitrary quest progression.

### Numbered Lore Notes

Lore notes use permanent sequential identifiers.

Each note corresponds to a numbered journal slot. Empty slots remain visible so the player can understand that a note was missed.

The note system must be data-driven and save progress using stable note IDs.

Example:
- NOTE-001
- NOTE-002
- NOTE-003

### Lore Categories

The shared note and journal system will support several content categories:

- Andrey
- Alexandra and family history
- Apartment history
- Black River district
- Liza
- Hospital records
- Yrka research
- Witness accounts