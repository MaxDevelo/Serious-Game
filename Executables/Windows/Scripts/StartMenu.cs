using Godot;
using System;

public class StartMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	public void _on_btnQuit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_btnPlay_pressed(){
		GetTree().ChangeScene("res://Scenes/Configuration.tscn");
	}
}
