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
Copy and paste the code below
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls character movement
/// </summary>
// "abstract" means that this component cannot used directly on a gameobject.
// we need to make child components that expand upon it (PCMovement, NPCMovement, etc.)!
public abstract class CharacterMovement : MonoBehaviour
{
    public LayerMask platformMask;
    public float speed;
    public float jumpForce;
    // a Rigidbody reference to cache the component
    protected Rigidbody2D m_Rigidbody2D;
    // a Animator reference to cache the component
    protected Animator m_Animator;
    protected bool m_IsGrounded;
    private float m_DistFromGround;
    private Collider2D m_Collider2D;
    private Vector2 m_LowerLeftCorner;
    private Vector2 m_LowerRightCorner;
    private RaycastHit2D[] raycastHits;
    private Vector3 m_originalPosition;
    private readonly float k_SmallNumber = 0.01f;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // cache the components using the GetComponent method
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Collider2D = GetComponent<Collider2D>();
        m_LowerLeftCorner = m_Collider2D.offset +
        new Vector2(-m_Collider2D.bounds.extents.x, -m_Collider2D.bounds.extents.y);
        m_LowerRightCorner = m_Collider2D.offset +
        new Vector2(m_Collider2D.bounds.extents.x, -m_Collider2D.bounds.extents.y);
        raycastHits = new RaycastHit2D[3];
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected virtual void Update()
    {
        UpdateAnimator();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        CalculatePosition();
    }

    protected virtual void Move(float amount)
    {
        // Preserve the vertical speed by using the original velocity.y
        // and apply the horizontal speed on the x component/axis 
        m_Rigidbody2D.velocity = new Vector2(amount * speed, m_Rigidbody2D.velocity.y);
    }

    protected virtual void Jump()
    {
        // Apply "jumpForce' on the y-axis
        // The second parameter tells Unity to use an impulse force.
        // If left empty, Unity will apply a slow, continuous "push" force by default,
        // which doesn't work for a jump - a impulsive upward force 
        m_Rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void CalculatePosition()
    {
        var centralRayOrigin = transform.position + Vector3.up * k_SmallNumber;
        var leftRayOrigin = centralRayOrigin + (Vector3)m_LowerLeftCorner;
        var rightRayOrigin = centralRayOrigin + (Vector3)m_LowerRightCorner;

        // Cast a ray from the position of this object, to 5 units down, and only detect objects in platformMask 
        var centerHit2D = raycastHits[0] =
        Physics2D.Raycast(centralRayOrigin, Vector2.down, 1 + k_SmallNumber, platformMask.value);
        var leftHit2D = raycastHits[1] =
        Physics2D.Raycast(leftRayOrigin, Vector2.down, 1 + k_SmallNumber, platformMask.value);
        var rightHit2D = raycastHits[2] =
        Physics2D.Raycast(rightRayOrigin, Vector2.down, 1 + k_SmallNumber, platformMask.value);

        // Use Debug.DrawRay to draw a visaulization of the raycast in the Scene View
        Debug.DrawLine(centralRayOrigin, centralRayOrigin + Vector3.down, Color.red);
        Debug.DrawLine(leftRayOrigin, leftRayOrigin + Vector3.down, Color.red);
        Debug.DrawLine(rightRayOrigin, rightRayOrigin + Vector3.down, Color.red);

        // Reset the distance from ground for new calculation
        m_DistFromGround = 1;
        foreach (var hit in raycastHits)
        {
            // If nothing is hit, character should be at least 1+ units from the ground
            // hence the default value of 1
            float currentDist = 1;
            if (hit.collider != null)
            {
                // If something is detected, store the distance from the result
                currentDist = centerHit2D.distance - k_SmallNumber;
            }
            m_DistFromGround = Mathf.Min(m_DistFromGround, currentDist);
        }

        m_IsGrounded = m_DistFromGround <= 0.02f;
    }

    protected virtual void UpdateAnimator()
    {
        m_Animator.SetFloat("distFromGround", m_DistFromGround);
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        if (raycastHits != null)
        {
            foreach (var hit in raycastHits)
            {
                // if raycast hits something, draw a little green circle to indicate it
                if (hit.collider != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(hit.point, 0.1f);
                }
            }
        }
    }
}
```

Let's also make a PCMovement class, and copy paste the code in. (PC stands for Playable Character)
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playable Character movement
/// </summary>
public class PCMovement : CharacterMovement
{
    public float acceleration = 20;
    public int numJumps = 1;
    private int m_AvailableJumps;
    private float m_HorizontalInput;
    private int m_FaceDirection;
    private bool m_jumpTrigger;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        // call the parent
        base.Start();
        // initialize available jumps
        m_AvailableJumps = numJumps;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Whenever "W" is pressed, trigger a jump
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // catch the jump key to use it in the Fixed/Physics update
            m_jumpTrigger = true;
        }

        m_HorizontalInput = Input.GetAxis("Horizontal");
        if (m_HorizontalInput != 0)
        {
            m_FaceDirection = (int)Mathf.Sign(m_HorizontalInput);
        }
        if (m_FaceDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (m_IsGrounded)
        {
            m_AvailableJumps = numJumps;
        }

        if (m_jumpTrigger)
        {
            Jump();
            m_jumpTrigger = false;
        }
        Move(m_HorizontalInput);

        
    }

    protected override void Move(float amount)
    {
        // Apply a continuous force to the rigidbody
        m_Rigidbody2D.AddForce(Vector2.right * acceleration * amount);
        var currentSpeed = Mathf.Abs(m_Rigidbody2D.velocity.x);
        if (currentSpeed > speed)
        {
            // If the actual speed exceeds the speed limit
            // Apply force in the opposite direction
            m_Rigidbody2D.AddForce(Vector2.left * m_FaceDirection * (currentSpeed - speed));
        }
    }

    protected override void Jump()
    {
        if (m_AvailableJumps > 0)
        {
            base.Jump();
            m_AvailableJumps--;
        }
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        m_Animator.SetFloat("speed", Mathf.Abs(m_HorizontalInput) * speed);
    }
}

```

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
