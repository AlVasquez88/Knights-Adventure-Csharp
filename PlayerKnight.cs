using Godot;
using System;

/**
Author: Alvin Vasquez
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
