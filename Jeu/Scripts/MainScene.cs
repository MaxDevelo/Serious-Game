using Godot;
using System;
using static Godot.Control;

public class MainScene : Node2D
{


    private Bar bar;
    private int money = 10000;
    private int ecology = 100;
    private int sociabilite = 100;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        bar = GetNode<Bar>("Bar");

        setModificationBar(money, ecology, sociabilite);
        generateMap();
    }
    /*
    * This method will generate a map of 100 buttons to create map
    */
    public void generateMap()
    {
        GridContainer myGridContainer = GetNode<GridContainer>("pnlMain/gridMap");
        myGridContainer.Columns = 10;
        for (int i = 1; i <= 100; i++)
        {
            // Create a new button
            Button myButton = new Button();
            myButton.Modulate = new Color(0, 0, 0);
            myButton.Name = "Button" + i;
            myGridContainer.AddChild(myButton);

            // Set the button's size flags
            myButton.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
            myButton.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
            myButton.RectMinSize = new Vector2(50, 50);
            myButton.Connect("pressed", this, "onButtonPressed", new Godot.Collections.Array() { myButton });
        }
    }

    public void onButtonPressed(Button myButton)
    {
        myButton.Modulate = new Color(1, 59, 0);
        Color backgroundColor = myButton.Modulate;
        // Recuperer position du bouton pour recuperer les bouton autour de lui
        int position = Convert.ToInt32(myButton.Name.Split('n')[1]);

        // Recuperer les boutons autour de lui
        Button buttonTop = GetNode<Button>("pnlMain/gridMap/Button" + (position - 10));
        Button buttonBottom = GetNode<Button>("pnlMain/gridMap/Button" + (position + 10));
        Button buttonLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position - 1));
        Button buttonRight = GetNode<Button>("pnlMain/gridMap/Button" + (position + 1));
        // Recuperer les boutons en diagonale
        Button buttonTopLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position - 11));
        Button buttonTopRight = GetNode<Button>("pnlMain/gridMap/Button" + (position - 9));
        Button buttonBottomLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position + 9));
        Button buttonBottomRight = GetNode<Button>("pnlMain/gridMap/Button" + (position + 11));

        if (verifyColor(buttonTop))
        {
            buttonTop.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonBottom))
        {
            buttonBottom.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonLeft))
        {
            buttonLeft.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonRight))
        {
            buttonRight.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonTopLeft))
        {
            buttonTopLeft.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonTopRight))
        {
            buttonTopRight.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonBottomLeft))
        {
            buttonBottomLeft.Modulate = new Color(1, 59, 0);
        }
        if (verifyColor(buttonBottomRight))
        {
            buttonBottomRight.Modulate = new Color(1, 59, 0);
        }

    }
    public Boolean verifyColor(Button myButton)
    {
        Color backgroundColor = myButton.Modulate;
        if (backgroundColor.r == 0 && backgroundColor.g == 0 && backgroundColor.b == 0)
        {
            return true;
        }
        return false;
    }

    public void setModificationBar(int money, int ecology, int sociabilite)
    {
        bar.setMoney(money);
        bar.setEcology(ecology);
        bar.setSociabilite(sociabilite);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
