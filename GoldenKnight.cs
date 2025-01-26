using Godot;
using System;

/**
Author: Alvin Vasquez
Date: 2024-12-04
Version: GoldenKnight.cs
Summary: This program is the enemy Golden Knight logic with speed that is faster than the silver knight.
It gets the nodes Killzone, and the design sprite nodes to interact with the objects in the game causing it to flip
the sprite's design horizontally.
*/


public partial class GoldenKnight : Node2D
{
	private const int SPEED = 120; // Speed constant
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
