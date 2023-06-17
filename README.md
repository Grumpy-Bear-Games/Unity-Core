# Core utilities for Unity

![GitHub](https://img.shields.io/github/license/Grumpy-Bear-Games/Unity-Core?style=plastic) ![GitHub package.json version](https://img.shields.io/github/package-json/v/Grumpy-Bear-Games/Unity-Core?filename=Packages%2Fgames.grumpybear.core%2Fpackage.json&style=plastic)

See main documentation on https://grumpy-bear-games.github.io/Unity-Core/


## Development
This repo contains a complete Unity project used for developing the package.
The package itself is located under `packages/games.grumpybear.core`.

### Updating the samples
1. Install the samples through the Unity package manager
2. Make your changes to the samples
3. Copy the updated samples back into `packages/games.grumpybear.core/Samples~`
4. Commit the changes

### Building documentation locally

1. Make sure you have opened the project in Unity at least once. This will create the `.csproj` files needed by DocFx
2. `docfx Documentation\docfx.json --serve`
3. Test documentation website
4. `git clean -Xdf` 
