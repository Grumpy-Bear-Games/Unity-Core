# Changelog

## 0.1.0
This release contains many changes (some of them incomplete), so don't upgrade if you strictly depend on old behavior. Please make special note of the breaking changes.

### Known problems
- A lot of this package is still undocumented.

### New features
- Added Observables.ScriptableObjects.GlobalStateT. This class is useful for situations, where you need a global (singleton) enum-like state, e.g. the game state.
- Added SaveSystem. This is a collection of classes useful for building a custom save system from.
- Migrated GrumpyBearGames.LevelManagement into this package.
- All editors rewritten using UI Toolkit
- This package now supports optional experimental features. Experimental features are enable under Tools/Core Game Utilities/Package Settings 

### Breaking changes
- This release drops support for the old IMGUI system and goes all in on UI Elements.
- There is currently no way to automatically migrate from GrumpyBearGames.LevelManagement to GrumpyBearGames.Core.LevelManagement. This needs to be done manually.
- This package now requires Unity 2022.2.1f1 or newer.

### Bug fixes
- LevelManagement.SceneGroupColdStartInitializer will refuse to load a scene group, if it's not part of it. This was a bug inherited from GrumpyBearGames.LevelManagement
- (Experimental) SceneReferences will no longer loose track of scenes, if you move or rename them _inside the editor_. Moving or renaming scenes _outside_ the editor will still break the reference. Note that this is an experimental features, which needs to be explicitly enabled.
- Scene Groups are now implemented using SerializableScriptableObject, so you can save and load a reference to them.

## 0.0.1
Initial version. This is very much work in progress.
