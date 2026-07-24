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