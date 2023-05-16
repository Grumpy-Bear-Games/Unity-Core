# Changelog

## 0.1.5

### Breaking change
- Get rid of wrapping VisualElement in custom VideoSettings controls. This _might_ be a breaking change for 
  you, if you're referencing the old `VideoSettingsControl` base class. If so, please use the new
  `IVideoSettingsControl` interface instead.

### Changes
- Fix broken sample

## 0.1.4
- Add documentation to ObjectGuid
- [bugfix] Make SerializableScriptableObject<T> work in game mode as well.
- Introduce SerializableScriptableObject base-class to SerializableScriptableObject<T>. This will allow serialization
  of assorted SerializableScriptableObject<T> and it should be safe as their asset ObjectGuids should never overlap.

## 0.1.3

- Add new editor window under Tools/Core Game Utilities/Scene Groups, which makes it easy to quickly switch between
  scene groups. This editor window is meant for level designers, who needs to switch between scene groups without
  editing them.
- Add "AfterLoad" UnityEvent to SceneGroupColdStartInitializer, which is triggered after the scene group have
  completed loading. This event is useful for doing bootstrapping of systems, when start playing inside the editor.
- Rewrite inspector for SceneGroupColdStartInitializer
- [bugfix] Create UI for Package Settings editor window in CreateGUI() instead of in OnEnable()

## 0.1.2

- [bugfix] Fix SerializableScriptableObject initialization


## 0.1.1

- Make SaveSystem.FileSystem more generic
- Make GlobalStateT<T> inherit from SerializableScriptableObject<T>
- [bugfix] Stop SaveSystem.SaveableEntry from needlessly regenerate IDs
- [bugfix] Hide ObjectID field in SaveSystem.SerializableScriptableObject<T> from inspector
- [bugfix] Fix ObjectGuid.operator== when both sides are null


## 0.1.0
This release contains many changes (some of them incomplete), so don't upgrade if you strictly depend on old behavior.
Please make special note of the breaking changes.

### Known problems
- A lot of this package is still undocumented.

### New features
- Added Observables.ScriptableObjects.GlobalStateT. This class is useful for situations, where you need a global (singleton) enum-like state, e.g. the game state.
- Added SaveSystem. This is a collection of classes useful for building a custom save system from.
- Migrated GrumpyBearGames.LevelManagement into this package.
- All editors rewritten using UI Toolkit
- This package now supports optional experimental features. Experimental features are enable under
  Tools/Core Game Utilities/Package Settings 

### Breaking changes
- This release drops support for the old IMGUI system and goes all in on UI Elements.
- There is currently no way to automatically migrate from GrumpyBearGames.LevelManagement to
  GrumpyBearGames.Core.LevelManagement. This needs to be done manually.
- This package now requires Unity 2022.2.1f1 or newer.

### Bug fixes
- LevelManagement.SceneGroupColdStartInitializer will refuse to load a scene group, if it's not part of it. This was
  a bug inherited from GrumpyBearGames.LevelManagement
- (Experimental) SceneReferences will no longer loose track of scenes, if you move or rename them
  _inside the editor_. Moving or renaming scenes _outside_ the editor will still break the reference. Note that this
  is an experimental features, which needs to be explicitly enabled.
- Scene Groups are now implemented using SerializableScriptableObject, so you can save and load a reference to them.


## 0.0.1
Initial version. This is very much work in progress.
