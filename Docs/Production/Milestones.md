# OKOLITSA Milestones

## M0 — Production Lock and Project Setup
- M0-T1 — Unity project foundation
- M0-T2 — Scene/Startup Cleanup and Production Wiring
- M0-T3 — Conventions Lock and Milestone Board
- M0-T4 — M0 Review and Tag

## M1 — Apartment Pressure Prototype
- M1-T1 — First-person movement baseline
- M1-T2 — Camera baseline
- M1-T3 — Simple interaction baseline
- M1-T4 — Apartment prototype blockout
- M1-T5 — Light failure state
- M1-T6 — Candle as limited light source
- M1-T7 — Window close / reinforce interaction
- M1-T8 — One domovoy disturbance placeholder
- M1-T9 — Basic apartment pressure loop

## Change Request CR-001 — Authored First Night Direction

Status: Approved  
Date: 2026-07-24

### Reason
The initial apartment pressure prototype successfully validated lighting, candle, audio, interaction, door, UI, and environmental systems.

A new authored narrative draft now defines the intended opening of OKOLITSA in substantially greater detail.

### Decision
The generic apartment pressure loop will no longer define the opening structure.

The project will proceed through the authored First Night sequence described in:
- "Околица. Виденье моей игры_v2_sequence-map.docx"
- `Docs/Narrative/OKOLITSA_FirstNight_Breakdown_v1.md`

### Existing Work
The v0.1.1 systems remain reusable production components and are not discarded.

### Milestone Impact
The next active milestone is Milestone 1.5 — First Night Production Foundation.

---

## Milestone 1.5 — First Night Production Foundation

### Objective
Create a clean, modular production foundation for implementing the authored First Night opening.

### Included Work
- lock the First Night sequence map;
- preserve the stable v0.1.1 prototype;
- create an isolated First Night development scene;
- establish scene, prefab, Timeline, and narrative folder conventions;
- create the essential apartment furniture blockout;
- create reusable player-control modes;
- validate one short in-engine Timeline sequence;
- prove that Timeline can safely transition between NoControl, LookOnly, and FullControl.

### Definition of Done
- the v0.1.1 prototype remains intact and playable;
- the First Night development scene is isolated;
- required opening furniture exists as replaceable blockout prefabs;
- control modes are reusable and independent of individual sequences;
- one wake-up Timeline prototype works;
- control returns to the player without camera or interaction errors;
- project documentation reflects the new architecture.

### Out of Scope
- final furniture models;
- final character animation;
- final creature model;
- complete First Night implementation;
- full substance-use cinematic;
- blood effects;
- grandmother character presentation;
- field encounter implementation.

### Required Git Checkpoint
- Tag: `M1_5_FirstNightFoundation`

### Required Portfolio Checkpoint
- 3–6 screenshots of the production blockout;
- one short wake-up Timeline clip;
- short English development summary.