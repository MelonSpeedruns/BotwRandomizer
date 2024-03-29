# BOTW Randomizer Mod

## Table of contents

- [Quick start](#quick-start)
- [What's randomized](#whats-randomized)
- [How it works](#how-it-works)
- [Known Bugs](#known-bugs)
- [Contributing](#contributing)
- [Creators](#creators)
- [Thanks](#special-thanks)
- [Copyright and license](#copyright-and-license)


## Quick start

Head over to the [Releases](https://github.com/MelonSpeedruns/BotwRandomizer/releases) section and download the latest built .exe file. Run it and configure it. Once the randomization process starts, it will create a new Cemu graphic pack in the proper folder. Make sure to enable it in Cemu's graphic packs settings!

If the app doesn't launch for you when double clicking it, then you might be missing the latest [.NET Desktop Runtime 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

The BOTW Randomizer expects you to have an extracted copy of the Wii U version of BOTW, including the following:
- The base game
- The latest update (1.5.0)
- The 2 DLCs

It is also recommended (for now) to use Cemu to play the Randomizer, as it wasn't tested on an actual Wii U console yet.

## What's randomized

Here's what's randomized with the latest version of the Randomizer:

 - Enemy Types
 - Enemy Levels
 - Enemy Weapons
 - Enemy Arrows
 - Taluses
 - Hinoxes
 - Lynels
 - Overworld & Shrine Weapons
 - Overworld & Shrine Chest Drops
 - Armors in Chests
 - Armor Shops
 - Food Shops
 - Arrow Shops

## How it works

Every time to decide to randomize the game, it creates a new graphic pack, and generates a new seed. This means that every object and item will always be the same until you do a new seed.

On top of everything above being randomized, the Randomizer changes slightly the goal of the game.

First of all, the Paraglider is randomized within the Great Plateau in a random chest. (Monster camp chests don't count.)

Next, to enter Ganon, you need the Master Sword. This makes it so you need at least 13 hearts to beat the game.

Secondly, completing shrines doesn't give you Spirit Orbs anymore, so how do you get more hearts?

The answer is simple! Spirit Orbs can now be found throughout Hyrule within it's numerous chests.

There are more Spirit Orbs placed in chests than you need. This is done so runs are shorter and so they don't last for days.

PS: You start the game with the Camera rune, which allows you to scan a treasure chest, and use the Sheikah Sensor to easily find all the other ones!

## Contributing

Feel free to contribute in any way you can! I'm very open to suggestions and ideas, as long as they are explained carefully and in detail. Thanks for helping out making this rando a better piece of software!

### Dependencies
You need to have the [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed to build this app in Visual Studio.

## Known Bugs

 - Rarely, enemy camps are empty.
 - And plenty more I don't know about! :)

## Creators

* [MelonSpeedruns](https://github.com/MelonSpeedruns/)
* [Miepee](https://github.com/miepee/)

## Special Thanks

* [Linkus7](https://www.twitch.tv/linkus7/) For helping out with design, bug hunting, beta testing, advertising and general support!
* [Leoetlino](https://github.com/leoetlino/) For answering my dumb Python questions, and being very patient with me!
* [LinoYeen](https://www.twitch.tv/linoyeen/) For helping out with design ideas, beta testing, advertising and general support!
* [tLeves](https://www.twitch.tv/tLeves/) For helping out with design ideas, beta testing, advertising and general support!

If I forgot your name, let me know and I'll add you!

## Copyright and license

The code is released under the [GPL-3.0 License](https://github.com/MelonSpeedruns/BotwRandomizer/blob/main/LICENSE).

Toolbox.Library is available as part of Switch Toolbox made by KillzXGaming!
Here's a link to it's source code: https://github.com/KillzXGaming/Switch-Toolbox
