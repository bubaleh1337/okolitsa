# OKOLITSA Daily Log

## 2026-07-24 — E02-T3B Bedside Table Blockout — E02-T3C CRT Television and TV Stand Blockout — E02-T3D Sideboard Blockout

- Created a reusable bedside table blockout prefab for the First Night apartment.
- Created modular blockout prefabs for the living-room CRT television area.
- Created a modular sideboard blockout prefab for the First Night apartment.


### Added
- `PF_BedsideTable_Blockout`
- `MAT_Blockout_FurnitureWood`
- Body, top, drawer front, and handle blockout geometry
- Single root-level collision volume
- `Socket_TopSurface` attachment point
- Left and right bedside table instances beside the sofa-bed
- `PF_TVStand_Blockout`
- `PF_CRTTV_Blockout`
- `MAT_Blockout_CRTPlastic`
- `MAT_Blockout_CRTScreen`
- `Socket_TVPlacement`
- `Socket_DVDPlayer`
- Separate `ScreenSurface` object for future display content
- `TV_Set` scene composition near the balcony door
- `PF_Sideboard_Blockout`
- `MAT_Blockout_Glass`
- Closed lower storage section
- Upper glass display section
- Three internal display shelves
- `Socket_DisplayShelf_01`
- `Socket_DisplayShelf_02`
- `Socket_DisplayShelf_03`
- Single root-level collision volume

### Production Purpose
- Establish the furniture composition surrounding Andrey's sleeping position.
- Reserve future locations for cigarettes, lighter, medicine, bottles, and personal objects.
- Keep the furniture replaceable without changing future prop placement logic.
- Establish the television position required by the authored apartment layout.
- Preserve a separate screen surface for the future animated DVD logo.
- Keep the television, stand, and DVD player independently replaceable.
- Validate the relationship between the sofa-bed, balcony entrance, and television corner.
- Establish the sideboard as a major family-memory and environmental-storytelling object.
- Reserve modular locations for Soviet service dishes, lace shelf liners, and family belongings.
- Preserve closed lower storage for future collectible model cars and hidden narrative objects.
- Establish wall composition for the grandmother photograph and strange painting.


### Test
- Both table instances inherit changes from the prefab.
- Player collision works.
- Apartment paths remain accessible.
- No new Console errors are present.
- Television faces the sofa-bed.
- Balcony access remains clear.
- Player collision works.
- Prefab instances remain connected to their source assets.
- No new Console errors are present.
- Player collision works.
- Apartment paths remain accessible.
- Glass doors remain visibly transparent.
- Prefab connection remains intact.
- No new Console errors are present.

### Next
Create the grandmother photograph and strange painting blockout props.

## 2026-07-24 — E02-T3A Sofa-Bed Blockout

Created the first replaceable furniture prefab for the First Night living-room production blockout.

### Added
- `__PROPS/Apartment/LivingRoom` hierarchy structure
- `PF_SofaBed_Blockout`
- `MAT_Blockout_Sofa`
- Single root-level collision volume

### Production Purpose
- Establish the physical location of Andrey's opening wake-up sequence.
- Define the initial camera relationship between the sofa-bed, painting, apartment arch, and blind hallway corner.
- Keep the blockout replaceable by a future final sofa model.

### Test
- Player cannot pass through the sofa-bed.
- Apartment and balcony paths remain accessible.
- Scene contains no new Console errors.

### Next
Create the bedside table blockout and validate the opening composition from the sofa-bed.

## 2026-07-24 — Real-Event Narrative Foundation Locked

Confirmed that a significant part of OKOLITSA is inspired by real events, family memories, personal experiences, and a real apartment.

### Real-Life Foundation
- The apartment layout and its atmosphere are based on a real family apartment.
- The apartment was the home and place of death of the author's great-grandmother.
- Alexandra's illness, death, and several surrounding family memories are based on real events.
- Sleep paralysis, auditory hallucinations, and fear responses are partially based on personal experiences and stories shared by family members.
- Soviet domestic objects and family belongings are intended to preserve the emotional truth of the real place.

### Fictionalization
- Andrey, Yrka, the endless night, the hospital research, and the supernatural journey are fictional or heavily fictionalized.
- Real experiences may be combined, reordered, altered, or assigned to fictional characters.
- The game will not present itself as a literal documentary reconstruction.

### Production Impact
- Environmental storytelling must prioritize emotional and domestic authenticity.
- The apartment must feel inhabited, specific, and personal rather than generically Soviet.
- Real-life reference material will inform proportions, object placement, photographs, sound, and narrative details.
- Public-facing materials should describe the project as inspired by real events and family memories.

## 2026-07-24 — Core Narrative Canon Expanded

Locked several major story and system decisions for OKOLITSA.

### Canon
- The entire game takes place during one endless night.
- Andrey dies from an accidental overdose during the opening but does not know it.
- The truth about his death is revealed only in the ending.
- The apartment will serve as an interactive archive of Soviet domestic life and family memory.
- Lore will be discovered through photographs, albums, diaries, documents, and numbered notes.
- Hospital records will reveal research connected to the creation of Yrka as both a physical and supernatural entity.

### Architecture Impact
- No traditional day/night cycle is required.
- Narrative progression will use night phases and world-state changes.
- Lore notes will use permanent IDs and a data-driven journal system.
- Save data must track collected note IDs.

## 2026-07-24 — E02-T2 First Night Development Scene

Created an isolated Unity scene for the authored First Night production work.

### Added
- `SCN_FirstNight_Dev`
- First Night scene folder
- Apartment blockout prefab folder
- Blockout material folder
- First Night Timeline folder
- Narrative scripts folder
- `__NARRATIVE/FN_FirstNight` scene hierarchy root

### Changed
- Disabled the old Episode 01 prototype sequence in the First Night development scene.

### Preserved
- The stable v0.1.1 prototype scene remains unchanged.
- Existing lighting, interaction, doors, movement, audio, and environmental systems remain available.

### Test
- Player movement works.
- Apartment and stairwell navigation work.
- Door interaction and auto-close work.
- Old Episode 01 does not start.
- No Console errors are present.

### Next
Create the essential living-room furniture blockout required for the opening wake-up sequence.

## 2026-07-24 — First Night Sequence Map Locked

Completed the narrative segmentation of the current OKOLITSA story draft.

### Completed
- Published the v0.1.1 Atmosphere Systems Update on itch.io.
- Prepared "Околица. Виденье моей игры_v2_sequence-map.docx".
- Divided the First Night story into:
  - lore and backstory;
  - non-interactive cutscenes;
  - limited-control sequences;
  - full-control gameplay;
  - narrative transitions.

### Production Decisions
- The old light-failure and candle sequence remains a reusable technical prototype.
- The next playable version will follow the authored First Night story.
- Substance-use content will be presented through a non-interactive cinematic transition rather than a detailed gameplay mechanic.
- Cutscenes will be created primarily as in-engine Timeline sequences.
- Gameplay systems must remain modular and reusable.

### Active Milestone
Milestone 1.5 — First Night Production Foundation

### Next
Create an isolated First Night development scene without modifying the stable v0.1.1 prototype scene.

## 2026-07-18 — E02-T1 First Night Narrative Breakdown

Created the first narrative breakdown based on the new 20-page OKOLITSA vision draft.

### Changed
- Stopped treating the candle/light-failure prototype as the final story direction.
- Defined the next playable target as "v0.2 — First Night Prototype".
- Identified the real first playable flow:
  - wake up
  - check apartment
  - balcony cigarette ritual
  - field / grave observation
  - reality break
  - leave apartment
  - stairwell descent
  - rain outside
  - move toward the cross

### Kept
- Existing apartment, stairwell, lighting, interaction, audio, and build systems remain useful as technical foundation.

### Next
Start restructuring the current Unity scene around the First Night sequence instead of the old candle quest prototype.

## 2026-07-18 — E01-T6 Episode Ending Beat — E01-T7 Pacing and Audio Polish — E01-T8 Controlled Flicker Audio — E01-T9 Objective Text Prototype — E01-T10 Interaction Prompt UI — E01-T11 Stairwell Flickering Lamp Audio — E01-T12 Episode Start Trigger — E01-T15 Stair Descent Footstep Cadence


- Implemented the ending beat for Episode 01: "Light Went Out".
- Polished Episode 01: "Light Went Out" with slower pacing, candle blowout, and audio hooks.
- Fixed the light flicker audio timing in Episode 01.
- Added temporary objective text for Episode 01: "Light Went Out".
- Added a temporary interaction prompt for interactable objects.
- Added looping spatial audio to the flickering stairwell lamp near the player's apartment.
- Added a trigger zone for starting Episode 01: "Light Went Out".
- Adjusted the Player collision setup after removing the redundant Capsule Collider.
- Fixed visible apartment bulb meshes during power failure.
- Added automatic closing behavior to interactable doors.
- Fixed unnatural rapid footstep sounds while descending stairs.


### Added
- Delayed ending beat after the balcony/window impact
- Short unstable power flicker before electricity returns
- Final episode completion state
- Debug logs for power return and episode completion
- Slower timing between episode beats
- Candle extinguish behavior after the balcony/window impact
- Candle lighting audio hook
- Candle extinguish audio hook
- Apartment power off audio hook
- Apartment power on audio hook
- Apartment light flicker audio hook
- Unstable power return sequence with flickering lights
- Replaced one-shot flicker playback with controlled flicker audio playback.
- Added separate power and flicker AudioSource references.
- Flicker sound now starts when visual flicker begins.
- Flicker sound now stops before stable power returns.
- UI_Episode canvas
- TXT_EpisodeObjective legacy UI text object
- EpisodeObjectiveTextController.cs
- Objective Text Controller reference in Episode01LightWentOutController
- Objective messages for major episode beats
- TXT_InteractionPrompt legacy UI text object
- InteractionPromptController.cs
- Interaction prompt reference in PlayerInteraction
- AudioSource on Stairwell_FlickeringLamp_6
- Looping flicker audio assigned to the stairwell flickering lamp
- 3D spatial audio settings for localized electrical buzzing
- EpisodeStartTrigger.cs
- Episode01_StartTrigger_Apartment scene object
- Box Collider trigger for apartment entry
- Player-based trigger detection

### Changed
- PlayerInteraction now checks the current interactable target every frame.
- PlayerInteraction shows "E — Interact" when the player looks at an interactable object.
- PlayerInteraction hides the prompt when no interactable object is targeted.
- Episode 01 no longer starts automatically on Play.
- Episode 01 now starts when the player enters the apartment trigger zone.
- Kept Character Controller as the only movement collider.
- Set Character Controller to a normal FPS body size:
  - Height: 1.8
  - Center Y: 0.9
  - Radius: 0.3
- Added apartment bulb Mesh Renderers to the Apartment Light Failure Controller.
- Created and assigned a darker bulb-off material.
- Reduced visible bulb scale for a less artificial look.
- Updated DoorInteractable.cs with optional auto-close behavior.
- Doors can now close automatically after a configurable delay.
- Manual closing cancels the pending auto-close timer.
- Existing open/close interaction behavior remains unchanged.
- Updated PlayerFootsteps.cs to use horizontal movement for step validation.
- Added slower stair descent footstep interval.
- Added landing sound filtering to prevent small stair contacts from triggering landing spam.
- Reduced ground check distance for the current prototype-scale player.

### Test
- Episode starts automatically.
- Apartment lights turn off after the configured delay.
- Player can light Candle_01 through interaction.
- First disturbance triggers after candle lighting.
- Window/balcony impact triggers after the first disturbance.
- Apartment power flickers and returns after the impact.
- Candle remains lit.
- Episode completion log appears.
- Episode no longer plays all events too quickly.
- Candle lights through player interaction.
- First disturbance triggers after a delay.
- Balcony/window impact triggers after a delay.
- Candle is blown out after the impact.
- Apartment lights flicker before returning.
- Episode completion log appears.
- Flicker sound no longer continues after lights stabilize.
- Power-on sound plays after flicker audio stops.
- Episode 01 ending beat feels cleaner and more synchronized.
- Objective text appears correctly in the top-left corner.
- Objective updates after the apartment power failure.
- Objective updates after candle lighting.
- Objective updates after first disturbance.
- Objective updates after balcony/window impact.
- Objective updates after candle blowout.
- Objective updates after power return.
- Final message appears when the episode completes.
- Prompt appears when looking at Candle_01.
- Prompt disappears when looking away.
- Candle interaction still works.
- Door interaction still works.
- Elevator button interaction still works.
- No errors appear in Console.
- Flicker audio plays automatically in Play Mode.
- Audio is localized near the stairwell lamp.
- Audio fades with distance.
- Sound does not overpower the Episode 01 audio events.
- Episode does not start immediately after pressing Play.
- Episode starts after the player enters the apartment trigger.
- Apartment lights turn off after the configured delay.
- Full episode sequence still works after trigger activation.
- Player movement still works.
- Player does not fall through floors.
- Stairs still work.
- Apartment trigger still starts Episode 01.
- Interactions still work.
- Apartment bulb meshes no longer stay bright yellow when power fails.
- Bulbs become dark when lights are off.
- Bulbs return to the warm on-material when power returns.
- Doors open through E interaction.
- Doors close automatically after the configured delay.
- Manual close still works.
- Door audio still plays.
- Episode 01 systems still work.
- Walking on flat ground still sounds normal.
- Running on flat ground still sounds faster.
- Descending stairs no longer sounds like rapid running.
- Small stair contacts no longer spam landing sounds.
- Jump and landing sounds still work for real jumps.

### Next
Run a full episode pass from outside the apartment and tune the trigger position if needed.

## 2026-07-17 — E01-T3 Episode State Controller

- Implemented the first state controller for Episode 01: "Light Went Out".
- Implemented the first apartment disturbance for Episode 01: "Light Went Out".
- Implemented the window/balcony impact event for Episode 01: "Light Went Out".

### Added
- Episode01LightWentOutController.cs
- Automatic apartment power failure after a short delay
- Candle-lit state detection after the power failure
- Debug logs for episode start, power failure, and candle progress
- Episode01DisturbanceController.cs
- Disturbance object movement after candle lighting
- Delayed disturbance trigger from Episode01LightWentOutController
- Debug logs for first disturbance state
- Episode01WindowImpactController.cs
- Window impact event after the first apartment disturbance
- Optional 3D audio source near the balcony/window
- Optional visual shake support for future balcony/window objects
- Episode controller state tracking for window impact completion

### Test
- Episode starts automatically in Play Mode.
- Apartment lights turn off after the configured delay.
- Candle can be lit through the existing interaction system.
- Episode controller detects when the candle is lit.
- Episode starts automatically.
- Apartment lights turn off after the configured delay.
- Player can light Candle_01 through interaction.
- First disturbance triggers after candle lighting.
- Disturbance prop changes position/rotation once.
- Episode starts automatically.
- Apartment lights turn off after the configured delay.
- Player can light Candle_01 through interaction.
- First disturbance triggers after candle lighting.
- Window impact triggers after the first disturbance.
- Episode logs show the full sequence.

### Next
Add a simple ending beat for the episode: light returns, but the apartment remains unsafe.

## 2026-07-16 — E01-T2 Candle Interaction

Implemented the first interactable candle for Episode 01: "Light Went Out".

### Added
- Candle_01 scene object
- Candle body, flame visual, and candle point light
- CandleInteractable.cs
- Interaction through existing PlayerInteraction / IInteractable system

### Test
- Player can turn apartment lights off with debug key L.
- Player can approach the candle and press E.
- Candle flame visual appears.
- Candle point light turns on.
- Repeated interaction does not break the candle state.

### Next
Connect the candle lighting event to the Episode 01 state controller.

## 2026-05-05 - My birthday is coming soon!

..so I will continue to work on my game.
Ok, that's what I'll gonnna do:

- Done:
  - Sounds (doors, flashlight, elevator)
  - Sounds (steps, running, jumping):
     - SurfaceAudio.cs → on the floor/concrete
     - PlayerFootsteps.cs → on the Player
  - Add sounds for ambience: wind:
     - WindAmbienceController.cs → on the object Ambience
     - WindZoneTrigger.cs → on the street trigger
  - Add sounds for ambience: extra steps: 
     - SurfaceAudio.cs → on the ground/metal
  - Create new textures
  - Finish decorating the apartment
  
- Blockers:
  - None
- Next action:
  - Add tag
  - Build the project
  - Create a link and share to my friends
  - Share this new on LinkedIn 

## 2026-05-04 - My new way

I've started my own production - without any Milestones and AI. Just me and my project.

Now I gonna conitnue my frist Okolitsa project. Its name was - Irka Horror Game. Now I've merged two projects, and we have what we have.

Ok, let's start!

- Done:
  - Animation of doors
  - Create a basement
  - Finish a balcony
  - Finish a toilet
  - Create a flashlight
  - Finish all lights in building
  - Finish with lift shift. Currently, it only works for the 5th and 1st floors.
  - 
  
- Blockers:
  - None
- Next action:
  - 


## 2026-04-22 — M1-T2 complete
- Active milestone: M1 — Apartment Pressure Prototype
- Active ticket: M1-T2 — Camera baseline
- Done:
  - Added CameraPivot under Player
  - Implemented mouse look baseline
  - Added pitch clamp
  - Locked cursor for play mode
- Blockers:
  - None
- Next action:
  - Start M1-T3 — Simple interaction baseline

## 2026-04-22 — M1-T2 start
- Active milestone: M1 — Apartment Pressure Prototype
- Active ticket: M1-T2 — Camera baseline
- Goal for session:
  - Add camera pivot
  - Implement mouse look
  - Clamp pitch
  - Lock cursor
- Blockers:
  - Low energy / low focus today
- Next action:
  - Paste finished PlayerLook code and wire it in Unity
- Done:
  - Added CameraPivot under Player
  - Implemented mouse look baseline
  - Added pitch clamp
  - Locked cursor for play mode
- Blockers:
  - None
- Next action:
  - Start M1-T3 — Simple interaction baseline

## 2026-04-22 — M1-T1 complete
- Active milestone: M1 — Apartment Pressure Prototype
- Active ticket: M1-T1 — First-person movement baseline
- Done:
  - Created movement test room in Apartment_Proto
  - Created Player with child camera
  - Added CharacterController
  - Implemented movement baseline with gravity
- Blockers:
  - None
- Next action:
  - Start M1-T2 — Camera baseline

## 2026-04-22 — M1-T1 start
- Active milestone: M1 — Apartment Pressure Prototype
- Active ticket: M1-T1 — First-person movement baseline
- Goal for session:
  - Create movement test area
  - Create Player object
  - Add CharacterController
  - Implement basic WASD movement with gravity
- Blockers:
  - Low energy / low focus today
- Next action:
  - Paste finished PlayerController baseline and test it
- Done:
  - Created movement test area in Apartment_Proto
  - Created Player with CharacterController
  - Implemented first-person movement baseline with gravity
- Blockers:
  - None
- Next action:
  - Start M1-T2 — Camera baseline

## 2026-04-16 — M0 closeout
- Active milestone: M0 — Production Lock and Project Setup
- Active ticket: M0-T4 — M0 Review and Tag
- Done:
  - Corrected production conventions
  - Merged dev into main
  - Fixed M0_ProjectSetup tag
- Blockers:
  - None
- Next action:
  - Start M1-T1 on dev


## 2026-04-16
- Active milestone: M0 — Production Lock and Project Setup
- Active ticket: M0-T2 — Scene/Startup Cleanup and Production Wiring
- Done:
- Blockers:
- Next action: