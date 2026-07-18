# OKOLITSA Daily Log

## 2026-07-18 — E01-T6 Episode Ending Beat — E01-T7 Pacing and Audio Polish — E01-T8 Controlled Flicker Audio — E01-T9 Objective Text Prototype — E01-T10 Interaction Prompt UI — E01-T11 Stairwell Flickering Lamp Audio — E01-T12 Episode Start Trigger

- Implemented the ending beat for Episode 01: "Light Went Out".
- Polished Episode 01: "Light Went Out" with slower pacing, candle blowout, and audio hooks.
- Fixed the light flicker audio timing in Episode 01.
- Added temporary objective text for Episode 01: "Light Went Out".
- Added a temporary interaction prompt for interactable objects.
- Added looping spatial audio to the flickering stairwell lamp near the player's apartment.
- Added a trigger zone for starting Episode 01: "Light Went Out".

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