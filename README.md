# Knights-Adventure-Csharp
•	The elements and goals of the game
-	The goal of this platformer game is to survive against the three knights with a sense of collecting coins only to die when the player knight reaches the finish line believing that the player is claiming a prize. 
•	How the elements moved.
- The player knight can move using the left and right arrow keys or the left A and right D keys to move the player knight. The design sprite of the player knight flips when moving left or right. When the player knight jumps, it performs a front flip. 
- The enemies are moved horizontally at a high rate of speed for the player to jump passed them. The sprite of the enemy design flips correctly when they collide with an object.
- The coin pickup item spins horizontally as it floats in the air for the player knight to pick up.
•	How the parts interact.
- When the player knight collides with the coin, the coin vanishes and plays a sound effect audio.
- When the player knight collides with the enemy player, the player dies by falling off slowly down the platform.
- When the player dies from the enemy or falls off the platform, the player returns to the original position and all the pickup coins respawn.
•	Four code snippets from your game.  Each one must be:
- At least five lines in length
- Paired with a paragraph explaining what it does and how that affects your game.



1: Coin.cs
using Godot;
using System;
/**
Author: Alvin Vasquez, 000857238
Date: 2024-12-04
Version: Coin.cs
Summary: This program is the pickup item logic for the coin for the player to collect. Once the player interacts with
the coin, it plays the pickup animation which results making a sound and disables the nodes of the coin as an indication
that the coin is picked up.
*/

public partial class Coin : Area2D
{
	private AnimationPlayer _animationPlayer;
	 public override void _Ready()
	{
		// Accessing child nodes
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	




private void OnBodyEntered(Node body)
	{
		
		_animationPlayer.Play("pickupAnimation");
		GD.Print("+1 Coin!");
	}
}

2: CrystalKnight.cs (SilverKnight.cs & GoldenKnight.cs are the exact same code, the only difference is the speed value difference with the
speed value with the CrystalKnight set to 150, GoldenKnight set to 120, and SilverKnight set to 100.)
using Godot;
using System;


/**
Author: Alvin Vasquez, 000857238
Date: 2024-12-04
Version: CrystalKnight.cs
Summary: This program is the enemy logic for the Crystal Knight with the highest speed of all the enemy knights.
It gets the nodes Killzone, and the design sprite nodes to interact with the objects in the game causing it to flip
the sprite's design horizontally.
*/







public partial class CrystalKnight : Node2D
{
	private const int SPEED = 150; // Speed constant
	private int direction = 1; // Current movement direction

	private RayCast2D rayCastRight; // Reference to the right RayCast2D
	private RayCast2D rayCastLeft;  // Reference to the left RayCast2D
	private AnimatedSprite2D animatedSprite;
	public override void _Ready()
	{
		// Initialize the ray casts
		rayCastRight = GetNode<RayCast2D>("RayCastRight");
		rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
		// Check collisions for right ray cast
		if (rayCastRight.IsColliding())
		{
			direction = -1;
			animatedSprite.FlipH = true;
		}

		


// Check collisions for left ray cast
		if (rayCastLeft.IsColliding())
		{
			direction = 1;
			animatedSprite.FlipH = false;
		}

		// Update position
		Position += new Vector2(direction * SPEED * (float)delta, 0);
	}
}

3: PlayerKnight.cs
using Godot;
using System;

/**
Author: Alvin Vasquez, 000857238
Date: 2024-12-04
Version: PlayerKnight.cs
Summary: This program is the the game logic for the player knight. It contains the keybind controls that also
assigns the animations when the player knight moves, jumps and stands on idle animation when the player doesn't move
the player knight. 
*/


public partial class PlayerKnight : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D _animatedSprite;

	public override void _PhysicsProcess(double delta)
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		






// Get the input direction: -1, 0, 1
		Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		if (IsOnFloor())
		{
			if (direction == Vector2.Zero)
			{
				_animatedSprite.Play("idle");
			}
			else if(direction > Vector2.Zero)
			{
				_animatedSprite.FlipH = false;
				_animatedSprite.Play("run");
			}
			else if (direction < Vector2.Zero)
			{
				_animatedSprite.FlipH = true;
				_animatedSprite.Play("run");
			}
		}
		else
		{
			_animatedSprite.Play("jump");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}

4: Killzone.cs
using Godot;
using System;

/**
Author: Alvin Vasquez, 000857238
Date: 2024-12-04
Version: Killzone.cs
Summary: This program is the death logic for the player knight. It takes the Area2D node called Killzone
to signal when the player knight collides with the collision node of the object assigned to Killzone. 
Once interacted, a timer starts and the player falls off the game with a 0.6 second respawn back to the 
original position. The pickups also respawn after the player dies.
*/

public partial class Killzone : Area2D
{
	private Timer _timer; // Reference to the Timer node

	public override void _Ready()
	{
		// Initialize the Timer reference
		_timer = GetNode<Timer>("Timer");

		// Connect signals
		BodyEntered += OnBodyEntered;
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("You Died!");
		Engine.TimeScale = 0.5;
		
		var collisionShape = body.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
		if (collisionShape != null)
		{
			collisionShape.QueueFree();
		}
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		Engine.TimeScale = 1.0;
		// Reload the current scene
		GetTree().ReloadCurrentScene();
	}
}

Citation Link of the assets used for this project:
https://brackeysgames.itch.io/brackeys-platformer-bundle?download
