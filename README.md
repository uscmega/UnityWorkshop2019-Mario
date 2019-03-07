## Table of Contents  
- [Quick Start](#quick-start)  
- [Setup](#setup)  

## Quick Start

[Download Unity](https://store.unity.com/download?ref=personal) (2018.3 or higher is preferred)

There are 2 ways to download this project:

### Option 1
* On your machine, go to the terminal and type in:

```
git clone git@github.com:uscmega/UnityWorkshop2019-Mario.git
```
By default you will be on the [master/incomplete branch](https://github.com/uscmega/UnityWorkshop2019-Mario), but you can checkout [the finished project](https://github.com/uscmega/UnityWorkshop2019-Mario) using the following command:

```
git checkout complete
```

### Option 2

* [download](https://github.com/uscmega/UnityWorkshop2019-Mario/archive/master.zip) the zipped starter files and unzip it in your preferred folder

  or

* [download](https://github.com/uscmega/UnityWorkshop2019-Mario/archive/complete.zip) the zipped finished project and unzip it in your preferred folder

Lastly, for reference, You can play Super Mario Bros [here](https://supermarioemulator.com/mario.php)

## Setup
1. Open Unity and click "Open". Navigate to where you cloned/unzipped the project, and hit "Select Folder".
1. In the project view, under the Assets/Scenes, double-click EmptyScene if it's not open already.
1. Right-click in hierarchy tab, and create a Quad from 3D Objects for a temporary ground
![01createquad png](https://user-images.githubusercontent.com/20757517/53938558-7c222d80-4065-11e9-99d7-687e74d437bc.jpg)
1. From the Prefab folder, drag and drop the Mario prefab to the scene view (Not the game view!) 
![03prefab png](https://user-images.githubusercontent.com/20757517/53938655-b5f33400-4065-11e9-926b-a05ab5851808.jpg)

1. Compared to the 1x1 quad, Mario appears too small! OMG how do we fix that?
1. PPU (Pixel Per Unit) is how Unity scales all your images and it also has . Our images are 32x32 pixels but their PPU's are 100, which means Mario would be 3.125 meters tall! OK, let's set the PPU to 32 so that Mario and other blocks are all 1 meter tall! Select all the images in the Images folder and use the following setting (EXCEPT PIVOTS)
![04spritesettingsjpg](https://user-images.githubusercontent.com/20757517/53939235-53029c80-4067-11e9-9458-09cfcf4c9a36.jpg)

1. Now, use "Q", "W", "E", "R", "T", "Y" to conveniently navigate between tools and scale your Quad up so there's enough space for Mario to walk on.

![02navigatetools](https://user-images.githubusercontent.com/20757517/53939164-28b0df00-4067-11e9-8c0d-a56f598c8303.jpg)

1. Hmmm, when we zoom in a lot, Mario looks super blurry! Maybe we should give up... But we haven't explored all image settings yet! Click on an image, and you can set the "Filter Mode" to "Point" and "Compression" to "None"
1. Let's also set the "Pivot" to "Bottom" because Mario's feet should be on the bottom of the sprite (this is common practice)
1. Now, when we click "Play", nothing happens! Maybe it's not too late to quit?

## Programming
1. Unity uses components to make things work. Let's start off by adding gravity to Mario! Click on Mario, Click "Add Component" and type in "Rigidbody", we can see Rigidbody and Rigidbody2D. Since we are making a 2D game, we will go with Rigidbody2D.
1. Under Rigidbody2D's Constraints menu, let's tick "Freeze Rotation". You can also leave it unticked to see what will happen and suffer the consequences :>
2. If we "play" now, Mario simply falls indefinitely into the abyss. That's because we haven't added colliders yet. Let's add 2D BoxColliders to the Quad/floor and Mario the same way we add the Rigidbody (or any other component) 
3. The physics is fully working now! Let's start adding our own scripts to animate Mario!
4. In the Scripts folder, right-click and create a C# script and name it "Character Movement". Drag and drop it onto Mario.
5. Double-click on CharacterMovement to edit in the default code editor (you can set your preferred editor in Edit->Preferences->External Tools)
6. You can see that the code is already generated for you. It inherits from Monobehaviour, which is a class you'll have to regularly use in Unity. Special functions like "Start", "Update" will be regularly be called by Unity to run your code. More info on execution order can be found [here](https://docs.unity3d.com/Manual/ExecutionOrder.html)
7. Let's add a couple of class members and functions. CharacterMovement will serve as a base for all the Character controls we make.
8. Let's also make a PCMovement class, and copy paste the code in. (PC stands for Playable Character)
7. Now drag and drop the PCMovement script onto Mario. We can see that the public fields are dispalyed in the [Inspector Window] (https://docs.unity3d.com/Manual/UsingTheInspector.html) and the private members are hidden. If at any time you need to see the private members, simply click on the arrow on the upper-right corner and select "Debug"
8. In the Inspector, set PCMovement's "Speed" and "Jump Force" to 10. We can now move Mario around!

## Level Editing
1. We can certainly drag and drop images one by one to create the level, but this extremely inefficient. Instead, let's use Unity's tilemap to speed up the process. Go to Window->2D->Tile Pallete.
1. Let's first drag the Background from the Images folder, and set its Y axis position to -1 to align it with the ground
1. Because there's nothing rendered behind the background, a default gray color is applied from the camera. Let's go to the Main Camera and click the eye dropper in the "Background" field, and pick the background color from the scene view. Note that the scene view background color stays the same, but the actual rendered footage (in the Game View) will reflect this change.
1. In the Tile Pallete window, choose "Create New Pallete", name the pallete CommonTiles, and place it under Assets/Tilemap
1. Select all the block sprites into the Tile Palette and save them under Assets/Tilemap
1. Using the Tile Pallete, we can now paint in the level very quickly!
1. Similar to how we created the quad, we can right-click in the hierarchy window and create 2D Object->Tilemap.
1. Let's name it "Static Tilemap" and paint everything that doesn't move, such as the ground, in this layer. We'll also duplicate it by hitting "Ctrl+D" on Windows or "Cmd+D" on Mac, and name the duplicate "Interactive Tilemap", where we'll paint interactive objects such as the question block. 
1. Let's delete the quad and click on StaticTilemap. Mess around with the tilemap to get familiar with the tool!
1. Remember colliders? Let's add them to the tilemaps too! We need to add a TilemapCollider2D to both tilemaps. This will generate a collider for each single tile, but this would make the scene way too complete. Fortunately, Unity also provides the CompositeCollider2D. We can add this component to Static Tilemap, and tick the "Used by Composite" box under TilemapCollider2D to apply the effect. Unity will automatically merge closeby tiles. Since the Interactive Tilemap will hold seperate, individual objects, we will not add composite collider to it.

## Creators

**Stewart Smith**

- [Stewart's Facebook](https://www.facebook.com/stewart.smith.908132)
- stewarms@usc.edu

**Sichen Liu**

- <https://twitter.com/lil_sichen>
- sichenli@usc.edu

## Organization
![mega_logo_small](https://user-images.githubusercontent.com/20757517/53943101-38cdbc00-4071-11e9-886e-cf7a377ab9ef.png)

[Twitter](https://twitter.com/MEGA_USC) | [Facebook](https://www.facebook.com/MEGAannouncements/)
