# OKOLITSA Daily Log

## 2026-07-25 — E02-T4E Master Bed Opening Timeline — E02-T4F Constrained Look and Kitchen Impact — E02-T5A Apartment Lighting Foundation and Opening Skip — E02-T5B/C Shoehorn Pickup and Held-Prop ViewModel — E02-T6A Dark Apartment Startup State

- Rebuilt the First Night bed-opening sequence around one master Timeline after validating that independently orchestrated Timeline assets caused conflicting camera, activation, and control states.
- Added a temporary perception-control phase after Andrey's real awakening.
- Created the reusable apartment-lighting foundation and added a development-friendly skip for the complete bed-opening sequence.
- Added the first lightweight narrative pickup and a dedicated first-person viewmodel rendering layer.
- Corrected the apartment's initial lighting state so every room begins dark while electrical power remains available.


### Added

- `TL_FN_BedOpening_MASTER`
- `FN_BedOpening_MASTER`
- Master `Playable Director`
- Master `Signal Receiver`
- Master `CameraPoseHandoff`
- `FN01` sleep-paralysis Sub-Timeline
- `FN02` real-awakening Sub-Timeline
- Sequential Control Tracks for both authored sections
- `PlayerLookLimiter`
- Limited horizontal and vertical mouse look
- Elliptical diagonal viewing boundary
- Separate upward and downward viewing limits
- Synchronization between limited look and `SimpleFPSController`
- `SIG_EnterLimitedLook`
- `SIG_PlayKitchenPotFall`
- Kitchen pot-fall audio event
- Ten-second limited-look phase before normal gameplay control
- `ApartmentPowerSupply`
- `RoomLightCircuit`
- `RoomLightSwitchInteractable`
- `BedOpeningSkipController`
- Independent light circuits for:
  - living room;
  - entry hall;
  - bathroom;
  - kitchen.
- Interactive wall switches using the existing `IInteractable` system
- Visible bulb-material synchronization
- Apartment-wide power testing through the temporary `L` hotkey
- Opening-sequence skip through `Space`
- `SimpleHeldPropController`
- `NarrativePickupInteractable`
- World shoehorn blockout in the entry hall
- Held shoehorn blockout beneath the Player camera
- `HeldProps` attachment root
- `ViewModel` layer
- `CAM_Player_ViewModel`
- URP Base and Overlay camera stack

### Architecture Changes

- The master Timeline is now the only owner of global sequence timing.
- `FN01` and `FN02` no longer start themselves through `Play On Awake`.
- Both Sub-Timelines contain authored animation and audio content without independently controlling the full sequence.
- Player-control and camera-handoff signals now exist only on the master Timeline.
- Control Clip activation was disabled so shared scene objects remain active across Sub-Timeline boundaries.
- The shared bed cutscene camera remains active between sleep paralysis and real awakening.
- The sleep-paralysis figure uses an Activation Track with `Post-playback State: Inactive`.

### Sequence Flow

- The bed-opening camera handoff completes.
- The Player enters `LookOnly`.
- Movement, jumping, interaction, and flashlight input remain disabled.
- The player may look horizontally, vertically, and diagonally within a constrained human-like viewing region.
- Approximately seven seconds into this phase, a metal pot impact is heard from the kitchen.
- After ten seconds, the limiter is released and `FullControl` is restored.

### Technical Structure

- `PlayerControlStateController` remains responsible for the general `LookOnly` and `FullControl` states.
- `PlayerLookLimiter` temporarily owns camera-look input during the constrained phase.
- `SimpleFPSController` disables its normal look processing while the limiter is active.
- `SimpleFPSController.SynchronizeLookState()` prevents the camera from snapping when normal look control returns.
- Horizontal and vertical offsets are clamped through one elliptical boundary rather than independent rectangular limits.

### Technical Correction

The legacy `ApartmentLightFailureController` component was removed from the active scene object.

Although the component had been disabled, its initialization still competed with the new room-circuit system and restored lamp components and bulb materials to their former powered state.

The current lighting ownership is now exclusive:

- `ApartmentPowerSupply` owns apartment-wide power availability.
- Each `RoomLightCircuit` owns one room's logical switch state.
- Each room circuit owns only its assigned `Light` component.
- Each room circuit owns only its assigned bulb renderer.
- No legacy controller modifies the same lights during scene initialization.

### Current Look Limits

- Horizontal: approximately `85°` left and `85°` right
- Upward: approximately `45°`
- Downward: approximately `35°`
- Diagonal look: supported within the elliptical boundary

### Apartment Lighting Structure

- `PWR_Apartment` represents whether electrical power reaches the apartment.
- Each illuminated room has an independent `RoomLightCircuit`.
- Each wall switch changes only its assigned room's requested switch state.
- Actual light output requires both:
  - the room switch to be on;
  - apartment power to be available.
- The connecting corridor intentionally has no lamp or wall switch, matching the real apartment layout.


### Power-State Behaviour

- Turning a room switch off keeps that room dark even when apartment power is available.
- Cutting apartment power turns off every room.
- Room-switch positions remain stored while power is unavailable.
- Restoring apartment power turns on only rooms whose switches were previously left on.
- `Debug/Toggle Wall Switch` simulates a room's wall switch.
- The temporary `L` hotkey simulates the future building electrical breaker.

### Visual Behaviour

- Each circuit controls its assigned Unity `Light` components.
- Each circuit also changes its assigned visible bulb renderers between powered and unpowered materials.
- The obsolete `ApartmentLightFailureController` is disabled to prevent competing ownership of the same lights.

### Opening Skip

- `Space` skips the complete bed-opening master Timeline while it is playing.
- The skip stops the master and Sub-Timelines.
- Temporary sequence audio is stopped.
- The sleep-paralysis figure is forced inactive.
- The gameplay camera and Audio Listener are restored safely.
- Limited look is released.
- `FullControl` returns on the following frame so the skip key does not immediately trigger a jump.

### ViewModel Rendering

- The Main Camera renders the game world but excludes the `ViewModel` layer.
- `CAM_Player_ViewModel` renders only the `ViewModel` layer.
- The ViewModel camera uses URP `Overlay` rendering.
- `Clear Depth` prevents apartment walls from hiding the held prop.
- Only the Main Camera contains an `Audio Listener`.
- The world shoehorn remains on the `Interactable` layer.
- The held shoehorn has no collider, Rigidbody, or interaction component.

### Pickup Behaviour

- The world shoehorn uses the existing `IInteractable` system.
- Pressing `E` gives the shoehorn to the Player.
- The world object becomes inactive after successful pickup.
- The held version becomes visible beneath the Player camera.
- Duplicate pickup is prevented.
- The Player exposes `HasShoehorn` for future narrative progression.
- No general inventory or combat system was introduced.

### Final Sequence Flow

- `0.00–11.50`: `FN01` sleep paralysis
- `11.50–24.00`: `FN02` real awakening
- `24.00–26.50`: camera handoff from the bed cutscene camera to the Player camera
- `26.50`: `FullControl` restored

### Final Starting State

- Apartment-wide electrical power begins available.
- The living-room wall switch begins off.
- The entry-hall wall switch begins off.
- The bathroom wall switch begins off.
- The kitchen wall switch begins off.
- The connecting corridor has no independent lamp or wall switch.
- Every room must be illuminated manually by the player.

### Resolved Problems

- Removed overlapping independent Timeline execution.
- Removed conflicting camera animation ownership.
- Prevented `No cameras rendering` during the Sub-Timeline transition.
- Prevented the scene from temporarily losing its active `Audio Listener`.
- Ensured the sleep-paralysis figure becomes inactive after `FN01`.
- Restored player control reliably after the complete bed-opening sequence.
- Preserved a continuous camera view between sleep paralysis and real awakening.

### Result

- All room circuits initialize with `Requested On: Off`.
- All room circuits initialize with `Actually On: Off`.
- The first interaction with a wall switch turns its room on.
- The second interaction turns the room off.
- The temporary `L` hotkey still simulates apartment-wide power interruption.
- Restoring power relights only rooms whose wall switches remain on.

### Test

- The master Timeline starts automatically.
- `FN01` and `FN02` execute sequentially rather than concurrently.
- The cutscene camera remains active across the Sub-Timeline boundary.
- The sleep-paralysis figure disappears after its authored section.
- Camera handoff starts and completes successfully.
- `FullControl` returns at the end of the master sequence.
- No `No cameras rendering` message appears.
- No missing or duplicate `Audio Listener` warning appears.
- No new Console errors are present.
- The player cannot move during the limited-look phase.
- Horizontal, vertical, and diagonal camera movement work.
- The player cannot rotate through a full 360 degrees.
- Diagonal viewing range reduces naturally near the boundary.
- The kitchen impact is heard from the kitchen location.
- Limited look lasts approximately ten seconds.
- `FullControl` returns after the phase.
- The camera does not snap when normal look control resumes.
- No new Console errors are present.
- `Space` skips the opening sequence and restores gameplay safely.
- Each room switch works through `E`.
- Each switch affects only its assigned room.
- The connecting corridor remains without its own light source.
- Bulb materials match the actual light state.
- `L` cuts and restores apartment-wide power.
- Previously enabled rooms return after power restoration.
- Rooms switched off remain off after power restoration.
- No competing light controller changes the room states.
- No new Console errors are present.
- The world shoehorn displays an interaction prompt.
- Pressing `E` removes the world shoehorn.
- The held shoehorn appears immediately after pickup.
- The shoehorn cannot be collected twice.
- The held prop remains visible when the Player approaches walls.
- Apartment geometry does not visually occlude the held prop.
- No duplicate Audio Listener warning appears.
- No new Console errors are present.
- The apartment begins dark.
- Electrical power remains available.
- Every room switch responds correctly on the first press of `E`.
- Each switch controls only its assigned room.
- Bulb materials match the actual light state.
- Apartment-wide power interruption still preserves wall-switch positions.
- No conflicting light controller remains active in the scene.
- No new Console errors are present.

### Next

Lock the apartment's main entrance door until a later explicit narrative event permits Andrey to leave.

## 2026-07-24 — E02-T3B Bedside Table Blockout — E02-T3C CRT Television and TV Stand Blockout — E02-T3D Sideboard Blockout — E02-T3E Photograph and Painting Variants — E02-T3F Living Room Rug Blockout and Composition Review — E02-T4A Wake-Up Composition Test — E02-T4B Reusable Player Control Modes — E02-T4C Wake-Up Timeline Control Handoff — E02-T4D Sleep-Paralysis Figure and Close Audio

- Created a reusable bedside table blockout prefab for the First Night apartment.
- Created modular blockout prefabs for the living-room CRT television area.
- Created a modular sideboard blockout prefab for the First Night apartment.
- Created modular wall-frame prefabs for the First Night apartment.
- Created the blockout rug for the First Night living room and used it to perform the first composition review of the apartment’s main inhabited space.
- Created a temporary scene camera to validate the authored wake-up composition before implementing Timeline.
- Created a reusable player-control state system for gameplay and future Timeline sequences.
- Created the first Timeline-driven control handoff for the First Night opening.
- Added the first authored horror-content placeholder to the First Night wake-up Timeline.


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
- `PF_WallFrame_Blockout_Base`
- `PF_GrandmotherPhotoFrame_Blockout`
- `PF_StrangePaintingFrame_Blockout`
- `MAT_Blockout_PhotoPlaceholder`
- `MAT_Blockout_PaintingPlaceholder`
- Separate replaceable `ContentSurface` objects
- `MAT_Blockout_Rug`
- `PF_Rug_Blockout`
- `Rug_LivingRoom` placement in the living room
- `FN01_WakeUp` narrative hierarchy group
- `CAM_FN01_WakeUp_Test`
- `PlayerControlMode`
- `PlayerControlStateController`
- `NoControl`
- `LookOnly`
- `FullControl`
- Temporary F6, F7, and F8 development hotkeys
- `TL_FN01_WakeUp_Prototype`
- `SIG_EnterNoControlCutscene`
- `SIG_EnterLookOnly`
- `SIG_EnterFullControl`
- Signal Track and Signal Receiver reactions
- First recorded wake-up camera rotation
- `PF_SleepParalysisFigure_Blockout`
- `MAT_Blockout_SleepParalysisFigure`
- Timeline-controlled figure activation
- Timeline-controlled unnatural approach movement
- `AUD_FN01_CloseCrunch`
- Close spatial audio accent near the cutscene camera

### Sequence Flow
- The scene starts in `NoControl`.
- A dedicated cutscene camera presents the initial wake-up shot.
- Control transitions to `LookOnly` on the player camera.
- The sequence explicitly restores `FullControl`.
- The room initially appears empty.
- A black elongated figure gradually becomes visible.
- The figure approaches without a normal walking cycle.
- A close dry crunch is heard near Andrey's ear.
- The figure disappears during the camera handoff into `LookOnly`.

### Changed
- `SimpleFPSController` now supports independent movement and camera-look permissions.
- `PlayerInteraction` now supports explicit interaction blocking and immediate prompt cleanup.
- `FlashlightToggle` now supports explicit input permission and externally controlled flashlight state.

### Validation Focus
- Physical position of Andrey's head on the sofa-bed
- Initial ceiling view
- Visibility of the strange painting and grandmother photograph
- Partial visibility of the apartment arch
- Preservation of the blind hallway corner

### Rules
- The test camera is not the final cutscene implementation.
- The camera remains disabled outside composition testing.
- The real apartment furniture arrangement is preserved rather than rearranged only for cinematic convenience.

### Architecture Purpose
- Keep narrative sequences independent from low-level player components.
- Allow Timeline signals to switch control modes through reusable public methods.
- Prevent individual cutscenes from manually disabling unrelated systems.
- Preserve future expansion without creating one monolithic First Night controller.
- Validate Timeline as the orchestration layer for authored narrative sequences.
- Keep player-control logic inside the reusable Player system.
- Establish reusable generic control signals.
- Prove safe camera handoff between cutscene and gameplay.
- Keep the sleep-paralysis figure separate from gameplay AI.
- Validate Timeline-driven narrative presence and disappearance.
- Keep placeholder geometry independently replaceable by final artwork.
- Test close spatial audio as part of the project's perception-based horror direction.


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
- Establish the grandmother photograph and strange painting required by the opening composition.
- Keep frame structure reusable through prefab variants.
- Allow future real photographs and final painted artwork to replace placeholder materials without changing scene placement.
- Preserve the relationship between the sofa-bed, sideboard, wall imagery, and apartment arch.
- Visually anchor the sofa and TV area into a single living zone.
- Improve spatial readability from player viewpoints.
- Support future placement of smaller props and narrative set dressing.
- Bring the apartment closer to a believable lived-in Soviet interior.

### Review Focus
- Entrance view into the living room
- Wake-up / sofa-area readability
- Reverse view from the TV zone back into the room

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
- Both props remain connected to the shared base prefab.
- Variant root scales remain `1 / 1 / 1`.
- Frames do not intersect or flicker against the wall.
- Both narrative images are visible from the sofa-bed area.
- No new Console errors are present.
- Rug does not intersect the floor visibly.
- Rug does not block player navigation.
- Living room reads more clearly as a coherent space.
- Main furniture composition remains readable from gameplay viewpoints.
- No new Console errors are present.
- One camera position supports the opening ceiling view and later head movement.
- The hallway remains partially concealed.
- The Player is active after testing.
- The test camera is disabled after testing.
- No new Console errors are present.
- FullControl restores normal gameplay.
- LookOnly allows mouse look but blocks movement, interaction, and flashlight input.
- NoControl blocks all player input.
- Interaction prompt is hidden when interaction is disabled.
- Returning to FullControl restores all tested functionality.
- No new Console errors are present.
- Cutscene camera activates without a black frame.
- NoControl blocks all tested player input.
- LookOnly allows camera movement but blocks gameplay actions.
- FullControl restores movement, interaction, and flashlight input.
- No duplicate Audio Listener warning appears.
- No new Console errors are present.
- Figure activation and disappearance follow the Timeline.
- Figure movement does not require gameplay code.
- Close audio is spatialized and does not loop.
- Player control handoff still works.
- No duplicate Audio Listener warning appears.
- No new Console errors are present.

### Next
Add a simple blink and visual-obstruction layer to support blurred opening eyes and the transition into real awakening.

## 2026-07-24 — E02-T3A Sofa-Bed Blockout

Add the first sleep-paralysis visual placeholder and sound layer to the wake-up Timeline.

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