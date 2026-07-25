# OKOLITSA conventions 

## Project naming

- Project name: OKOLITSA
- Repo name: okolitsa

## Namespace root

- Okolitsa

## Scene naming

- Boot
- Apartment_Proto
- Field_Proto
- Loop_Proto

## Script naming

- PascalCase

### Use clear suffixes only when they add meaning:

- Controller
- Manager
- Interactor
- Trigger
- Config

#### Examples:

- PlayerController
- PlayerLook
- LightPressureController
- WindowInteractor
- CandleConfig

## Folder rule

- Put all production assets inside Assets/_Project/

## Git rule

### Commit message rule
- English only
- imperative mood
- concise and professional

#### Examples:

- Set up startup scenes and production wiring
- Add production conventions and milestone board

### Branch rule

- main = stable
- dev = integration
- feature/<ticket-name> only when needed

## First Night Narrative Production Conventions

### Scene Naming
- `SCN_` — Unity scenes
- `SCN_FirstNight_Dev`
- `SCN_AtmospherePrototype_v0_1_1`

### Prefab Naming
- `PF_` — reusable prefabs
- `PF_SofaBed_Blockout`
- `PF_CRT_Television_Blockout`
- `PF_BedsideTable_Blockout`

### Timeline Naming
- `TL_` — Timeline assets
- `TL_FN01_SleepParalysis`
- `TL_FN02_RealAwakening`

### Signal Naming
- `SIG_` — Timeline signals and narrative events
- `SIG_EnableLookOnly`
- `SIG_EnableFullControl`
- `SIG_PlayKitchenImpact`

### Hierarchy Root Naming
Scene-level organizational objects use double underscores:

- `__ENVIRONMENT`
- `__PROPS`
- `__PLAYER`
- `__SYSTEMS`
- `__NARRATIVE`
- `__AUDIO`
- `__LIGHTING`
- `__UI`
- `__DEBUG`

### Control Modes
Narrative implementation must support three reusable player-control modes:

- `NoControl`
- `LookOnly`
- `FullControl`

Cutscene scripts must not directly modify unrelated movement, interaction, camera, and UI components independently.

### Modularity Rules
- Do not create one large FirstNightController.
- Use one responsibility per component.
- Timeline controls sequence timing, not all gameplay logic.
- General gameplay systems must not depend on a specific story scene.
- Scene-specific components may call reusable systems through explicit references or events.
- Furniture and interactive props must become prefabs before detailed art work.
- Blockout geometry must remain replaceable without rewriting gameplay code.
- Narrative content, system logic, and tuning data must remain separate.

### Scene Boundary Rule
Unity scenes represent large production spaces or development contexts, not individual story beats.

Individual cutscenes and narrative beats are implemented with Timeline assets inside the appropriate gameplay scene.

## Lore Asset Conventions

### Note Identifiers

Lore notes use permanent three-digit IDs:

- `NOTE-001`
- `NOTE-002`
- `NOTE-010`

IDs must not be reused or renumbered after a public build.

### Note Asset Naming

- `SO_NOTE_001_ShortTitle`
- `SO_NOTE_010_HospitalResearch`

### Lore Architecture

- Note content must be stored separately from scene objects.
- Scene pickups reference note data by stable ID.
- Save data stores collected note IDs.
- Journal UI reads note data and displays numbered empty or completed slots.
- Family lore, hospital research, and Yrka documents use the same reusable note system.

## Real-Life Reference and Fictionalization Conventions

### Reference Categories

Real-life material must be classified as one of the following:

- `REAL-SPACE` — architecture, room layout, object placement;
- `REAL-MEMORY` — remembered event or personal experience;
- `FAMILY-STORY` — event described by a relative;
- `FICTIONALIZED` — altered or combined real material;
- `FICTIONAL` — fully invented narrative content.

### Privacy Rules

- Do not publish an exact residential address.
- Do not include identifying information about living private individuals without permission.
- Real names may be replaced with fictional names.
- Personal photographs must be reviewed before inclusion in a public build.
- Private source photographs and public game assets must be stored separately.
- Sensitive reference material must not be committed to a public repository unless intentionally approved.

### Repository Rules

Private family references must not be placed in public asset folders by default.

Recommended local-only structure:

`Reference_Private/Family/`

If stored inside the repository, the folder must first be reviewed and explicitly covered by `.gitignore`.

Publicly usable recreated assets belong under:

`Assets/_Project/Art/References/PublicSafe/`

### Adaptation Rule

Gameplay and narrative clarity take priority over literal reconstruction.

Real events may be changed when necessary for:

- privacy;
- pacing;
- player comprehension;
- thematic consistency;
- emotional safety;
- technical feasibility.

### Public Description

Use:

`Inspired by real events, family memories, personal experiences, and a real apartment.`

Do not describe OKOLITSA as a fully factual reconstruction.

## Prefab Socket Conventions

Reusable prefabs may contain named empty child transforms used as attachment points.

### Naming

- `Socket_TopSurface`
- `Socket_Hand`
- `Socket_DoorHandle`

### Rules

- Socket objects must have a clear technical placement purpose.
- Sockets do not contain narrative sequence logic.
- Replaceable props may be attached to sockets without modifying the base prefab structure.
- Socket transforms must use a scale of `1 / 1 / 1`.

## Prefab Variant Conventions

Prefab variants are used when multiple props share the same structural foundation but require different dimensions, materials, or content.

### Naming

- Shared base: `PF_WallFrame_Blockout_Base`
- Variant: `PF_GrandmotherPhotoFrame_Blockout`
- Variant: `PF_StrangePaintingFrame_Blockout`

### Rules

- Base prefabs contain shared structure.
- Variants contain content-specific overrides.
- Variant root scale should remain `1 / 1 / 1`.
- Replaceable visual content must remain on a separate object such as `ContentSurface`.
- Do not modify the base prefab when a change applies only to one narrative prop.

## Blockout Set Dressing Rule

Large set-dressing props must first establish spatial readability before detail density.

Examples:
- Sofa
- TV stand
- Sideboard
- Wall frames
- Rug

These objects are used to define room function, player orientation, and composition before smaller decorative props are added.

## Player Control Mode Conventions

Narrative sequences use one reusable player-control state controller.

### Supported Modes

- `NoControl`
  - movement disabled;
  - camera look disabled;
  - interaction disabled;
  - flashlight input disabled.

- `LookOnly`
  - movement disabled;
  - camera look enabled;
  - interaction disabled;
  - flashlight input disabled.

- `FullControl`
  - movement enabled;
  - camera look enabled;
  - interaction enabled;
  - flashlight input enabled.

### Rules

- Individual cutscenes must not enable or disable unrelated player components independently.
- Timeline signals and UnityEvents must call the parameterless control-mode methods.
- Control state belongs to the Player, not to an individual episode controller.
- Returning to gameplay must explicitly restore `FullControl`.
- Debug hotkeys must be disabled before a public production build.

## Timeline Control Handoff Conventions

### Generic Control Signals

- `SIG_EnterNoControlCutscene`
- `SIG_EnterLookOnly`
- `SIG_EnterFullControl`

### Rules

- Camera activation must happen before the previously active camera is disabled.
- Every sequence that blocks player control must explicitly restore either `LookOnly` or `FullControl`.
- Generic control signals may be reused by multiple Timeline assets.
- Signal Receiver reactions remain scene-specific bindings.
- Timeline must call `PlayerControlStateController` instead of directly changing movement, interaction, or flashlight components.
- Public builds must not depend on debug control hotkeys.

## Narrative Visual Placeholder Conventions

Narrative visual placeholders represent authored story content without becoming gameplay systems.

### Rules

- Narrative placeholders live under `__NARRATIVE`.
- Reusable placeholder prefabs live under `Prefabs/Narrative`.
- Placeholder figures do not use gameplay AI.
- Placeholder figures do not require physical colliders unless a later gameplay ticket explicitly needs them.
- Visibility and authored movement are controlled through Timeline.
- Final creature identity must not be inferred from temporary primitive geometry.
- Temporary visual placeholders must remain independently replaceable.

## Master Timeline and Sub-Timeline Conventions

Long authored sequences may be divided into modular Sub-Timelines, but one master Timeline must own global orchestration.

### Master Timeline Responsibilities

The master Timeline controls:

- global sequence timing;
- Sub-Timeline order;
- player-control mode changes;
- camera handoff;
- gameplay restoration;
- signals shared across multiple authored sections.

### Sub-Timeline Responsibilities

Sub-Timelines contain local authored content such as:

- camera animation;
- narrative prop animation;
- Activation Tracks;
- local sound effects;
- visual sequence timing.

### Rules

- Only the master `Playable Director` may use `Play On Awake`.
- Sub-Timeline directors must use `Play On Awake: Off`.
- Sub-Timelines must not start one another through signals when a master Timeline exists.
- Player-control signals must remain on the master Timeline.
- Camera-handoff signals must remain on the master Timeline.
- Sub-Timelines that share cameras or scene objects must execute sequentially rather than concurrently.
- Shared scene objects must remain active across Sub-Timeline boundaries.
- Disable `Control Activation` on Control Clips when deactivating the controlled object would also disable a shared camera, Audio Listener, or another persistent dependency.
- Temporary narrative figures must define an explicit Activation Track post-playback state.
- Temporary figures that must disappear after their section should use `Post-playback State: Inactive`.
- The master Timeline must explicitly restore `FullControl` before it ends.

## Constrained Perception Control Conventions

Temporary perception states may further restrict camera input while the Player remains in the reusable `LookOnly` control mode.

### Responsibilities

`PlayerControlStateController` controls:

- movement permission;
- general camera-look permission;
- interaction permission;
- flashlight-input permission.

`PlayerLookLimiter` controls:

- temporary horizontal look limits;
- temporary vertical look limits;
- diagonal viewing boundaries;
- transition back to normal FPS look processing.

### Rules

- Limited perception must use `LookOnly` as its base player-control mode.
- The normal FPS look controller must not process mouse input while `PlayerLookLimiter` is active.
- Horizontal and vertical limits should form one elliptical viewing region rather than two independent rectangular clamps.
- The limiter must capture the current camera orientation when it begins.
- Ending limited look must synchronize the FPS controller's stored pitch before restoring normal look processing.
- Signal reaction order must be explicit.

### Enter Limited Look Order

1. `PlayerControlStateController.SetLookOnly()`
2. `PlayerLookLimiter.BeginLimitedLook()`

### Exit Limited Look Order

1. `PlayerLookLimiter.EndLimitedLook()`
2. `PlayerControlStateController.SetFullControl()`

### Default Bed-Awakening Limits

- Horizontal limit: `85°` per side
- Upward limit: `45°`
- Downward limit: `35°`

These values are authored tuning defaults, not universal settings for every narrative sequence.

## Apartment Electrical System Conventions

Apartment lighting uses two separate state layers.

### Apartment Power

`ApartmentPowerSupply` represents whether electrical power reaches the complete apartment.

It may later be controlled by:

- the building electrical breaker;
- a narrative power-failure event;
- a Timeline signal;
- another explicit gameplay system.

Apartment power must not directly change the logical positions of room switches.

### Room Circuits

Each illuminated room owns one `RoomLightCircuit`.

Current circuits:

- `LGT_LivingRoom_Circuit`
- `LGT_EntryHall_Circuit`
- `LGT_Bathroom_Circuit`
- `LGT_Kitchen_Circuit`

Each circuit stores:

- the requested wall-switch state;
- the actual powered-light state;
- assigned Unity `Light` components;
- assigned visible bulb renderers;
- powered and unpowered bulb materials.

### Final Light Rule

A room produces light only when:

- its wall switch requests light;
- apartment power is available.

### Ownership Rules

- One `Light` component belongs to only one room circuit.
- One visible bulb renderer belongs to only one room circuit.
- Wall switches communicate with their assigned room circuit.
- Wall switches must not directly control apartment-wide power.
- Apartment-wide events must communicate with `ApartmentPowerSupply`.
- Legacy systems must not control the same lights in parallel.

### Spatial Rule

The connecting apartment corridor intentionally has:

- no ceiling lamp;
- no room-light circuit;
- no wall switch.

Its visibility comes from neighbouring rooms and external night light. This is a canonical property of the real apartment layout, not missing content.

## Development Skip Conventions

Long development sequences may provide a temporary skip input while they remain under active iteration.

### Rules

- A skip must restore a valid gameplay state rather than only stopping Timeline playback.
- Temporary narrative objects must be removed.
- Sequence audio must be stopped.
- Exactly one camera and one Audio Listener must remain active.
- Temporary perception restrictions must end.
- Player control must return explicitly.
- The skip input must stop working after the sequence has ended.
- Development skip behaviour must be reviewed before a public build.

## Lightweight Narrative Pickup Conventions

Small authored props may use a lightweight possession system when a full inventory would add unnecessary scope.

### Rules

- World pickups use the existing `IInteractable` contract.
- The world object and held visual must be separate objects.
- A successful pickup must disable the world object's interaction colliders.
- Duplicate acquisition must be rejected.
- Possession state must remain queryable by later narrative systems.
- A lightweight narrative pickup must not silently expand into a general inventory, weapon, or combat system.
- Removal of a held prop does not automatically respawn its collected world object.

## Held-Prop ViewModel Conventions

First-person held props use a dedicated URP Overlay camera.

### Layer Ownership

- World objects remain on their normal world or interaction layers.
- Held first-person visuals use the `ViewModel` layer.
- The Main Camera excludes the `ViewModel` layer.
- The ViewModel camera renders only the `ViewModel` layer.

### Camera Rules

- The Main Camera uses `Render Type: Base`.
- The ViewModel camera uses `Render Type: Overlay`.
- The ViewModel camera is included in the Main Camera stack.
- The ViewModel camera uses `Clear Depth`.
- Only one active `Audio Listener` may exist.
- Held-prop cameras must not render apartment geometry.
- Held visuals must not contain gameplay colliders, Rigidbody components, or world interaction scripts.

### Hierarchy Rule

`HeldProps` is a clean attachment root:

- local position `0 / 0 / 0`;
- local rotation `0 / 0 / 0`;
- local scale `1 / 1 / 1`;
- no collider;
- no interaction logic.

Individual held visuals own their authored local position, rotation, and scale.