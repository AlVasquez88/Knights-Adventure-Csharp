using Godot;
using System;
/**
Author: Alvin Vasquez
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
