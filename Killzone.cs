using Godot;
using System;

/**
Author: Alvin Vasquez
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
