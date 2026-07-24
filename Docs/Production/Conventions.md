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