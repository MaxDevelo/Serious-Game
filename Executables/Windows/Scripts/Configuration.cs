using Godot;
using System;

public class Configuration : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Global global;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        global =  GetNode<Global>("/root/Global");
        GetNode<Button>("pnlConfig/BtnMap1").Connect("pressed", this, "_on_btnMap1_pressed");
    }
    void _on_btnMap1_pressed(){
        global.setIndexMap(1);
         GetTree().ChangeScene("res://Scenes/MainScene.tscn");
    }
    void _on_BtnMap2_pressed(){
        global.setIndexMap(2);
         GetTree().ChangeScene("res://Scenes/MainScene_Medium.tscn");
    }

    void _on_BtnMap3_pressed(){
        global.setIndexMap(3);
         GetTree().ChangeScene("res://Scenes/MainScene_Large.tscn");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
