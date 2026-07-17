# OKOLITSA Daily Log

## 2026-07-17 — E01-T3 Episode State Controller

Implemented the first state controller for Episode 01: "Light Went Out".

### Added
- Episode01LightWentOutController.cs
- Automatic apartment power failure after a short delay
- Candle-lit state detection after the power failure
- Debug logs for episode start, power failure, and candle progress

### Test
- Episode starts automatically in Play Mode.
- Apartment lights turn off after the configured delay.
- Candle can be lit through the existing interaction system.
- Episode controller detects when the candle is lit.

### Next
Add the first apartment disturbance after the candle is lit.

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