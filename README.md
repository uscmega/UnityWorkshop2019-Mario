## Table of Contents  
- [Quick Start](#quick-start)  
- [Setup](#setup)  
- [Programming](#programming)  
- [Level Editing](#level-editing)  
- [Creators](#creators)  
- [Organization](#organization)  


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
Open Unity and click "Open". Navigate to where you cloned/unzipped the project, and hit "Select Folder".

In the Project tab, double-click EmptyScene under Assets/Scenes.

Right-click in the [Hierarchy window](https://docs.unity3d.com/Manual/Hierarchy.html), and create a Quad from 3D Objects for a temporary ground

![01createquad png](https://user-images.githubusercontent.com/20757517/53938558-7c222d80-4065-11e9-99d7-687e74d437bc.jpg)

From the Prefab folder, drag and drop the Mario [prefab](https://docs.unity3d.com/Manual/Prefabs.html) to the [Scene View](https://docs.unity3d.com/Manual/UsingTheSceneView.html) (not the [Game View](https://docs.unity3d.com/Manual/GameView.html)!) 

![03prefab png](https://user-images.githubusercontent.com/20757517/53938655-b5f33400-4065-11e9-926b-a05ab5851808.jpg)

Compared to the 1x1 quad, Mario appears too small! OMG how do we fix that?
**PPU (Pixel Per Unit)** is how Unity scales all your images. Our images are 32x32 pixels but their PPU's are 100, which means Mario would be 3.125 meters tall! Let's set the PPU to 32 to match the sprite sizes! Select all the images in the Images folder and use the following setting (except fro pivots)

![04spritesettingsjpg](https://user-images.githubusercontent.com/20757517/53939235-53029c80-4067-11e9-9458-09cfcf4c9a36.jpg)

Now, use "Q", "W", "E", "R", "T", "Y" to conveniently navigate between tools and scale your Quad up so there's enough space for Mario to walk on.

![02navigatetools](https://user-images.githubusercontent.com/20757517/53939164-28b0df00-4067-11e9-8c0d-a56f598c8303.jpg)

When we zoom in a lot, Mario looks super blurry! Select all the images under Images folder, and you can set the "Filter Mode" to "Point" and "Compression" to "None". Let's also set the "Pivot" to "Bottom" because Mario's feet should be on the bottom of the sprite (this is common practice)

## Programming
### Adding Physics
Unity uses components to make things work. Let's start off by adding gravity to Mario! Click on Mario, Click "Add Component" and type in "Rigidbody", we can see Rigidbody and Rigidbody2D. Since we are making a 2D game, we will go with Rigidbody2D.

Under Rigidbody2D's Constraints menu, let's tick "Freeze Rotation". If it's not selected, Mario would actually be able to rotate freely like a wheel, but we don't want that!

If we "Play" now, Mario simply falls indefinitely into the abyss. That's because we haven't added colliders yet. Let's add 2D BoxColliders to the Quad/floor and Mario the same way we add the Rigidbody (or any other component) 

The physics is should be fully working now! Let's start adding our own scripts to animate Mario!

### Adding Logic
In the Scripts folder, right-click and create a C# script and name it "Character Movement". Drag and drop it onto Mario.

Double-click on CharacterMovement to edit in the default code editor (you can set your preferred editor in Edit->Preferences->External Tools)

You can see that the code is already generated for you. It inherits from Monobehaviour, which is a class you'll have to regularly use in Unity. Special functions like "Start", "Update" will be regularly be called by Unity to run your code. More info on execution order can be found [here](https://docs.unity3d.com/Manual/ExecutionOrder.html)

Let's add a couple of class members and functions. CharacterMovement will serve as a base for all the Character controls we make.

Let's also make a PCMovement class, and copy paste the code in. (PC stands for Playable Character)

Now drag and drop the PCMovement script onto Mario. We can see that the public fields are dispalyed in the [Inspector Window] (https://docs.unity3d.com/Manual/UsingTheInspector.html) and the private members are hidden. If at any time you need to see the private members, simply click on the arrow on the upper-right corner and select "Debug"

In the Inspector, set PCMovement's "Speed" and "Jump Force" to 10. We can now move Mario around!

## Level Editing
### Create a Pallete
We can certainly drag and drop images one by one to create the level, but this extremely inefficient. Instead, let's use Unity's [Tilemap](https://docs.unity3d.com/Manual/class-Tilemap.html) to speed up the process. Go to Window->2D->Tile Pallete.

Let's first drag the Background from the Images folder, and set its Y axis position to -1 to align it with the ground

Because there's nothing rendered behind the background, a default gray color is applied from the camera. Let's go to the Main Camera and click the eye dropper in the "Background" field, and pick the background color from the scene view. Note that the scene view background color stays the same, but the actual rendered footage (in the Game View) will reflect this change.

In the Tile Pallete window, choose "Create New Pallete", name the pallete CommonTiles, and place it under Assets/Tilemap. Select all the block sprites into the Tile Palette and save them under Assets/Tilemap

Using the Tile Pallete, we can now paint in the level very quickly!

### Create a Tilemap
Similar to how we created the quad, we can right-click in the hierarchy window and create 2D Object->Tilemap. Let's name it "Static Tilemap" and paint everything that doesn't move, such as the ground, in this layer. We'll also duplicate it by hitting "Ctrl+D" on Windows or "Cmd+D" on Mac, and name the duplicate "Interactive Tilemap", where we'll paint interactive objects such as the question block. 

Let's delete the quad and click on StaticTilemap. Mess around with the tilemap to get familiar with the tool!

Remember colliders? Let's add them to the tilemaps too! We need to add a TilemapCollider2D to both tilemaps. This will generate a collider for each single tile, but this would make the scene way too complete. Fortunately, Unity also provides the CompositeCollider2D. We can add this component to Static Tilemap, and tick the "Used by Composite" box under TilemapCollider2D to apply the effect. Unity will automatically merge closeby tiles. Since the Interactive Tilemap will hold seperate, individual objects, we will not add composite collider to it.

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
