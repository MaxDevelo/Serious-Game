using Godot;
using System;
using System.Collections.Generic;
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
            // Charger le thème à partir du fichier de ressources
            Theme theme;
            if (i > 50 && i <= 60)
            {
                theme = GD.Load<Theme>("res://Themes/tramway.tres");
            }
            else
            {
                theme = GD.Load<Theme>("res://Themes/green.tres");
            }
            myButton.Theme = theme;
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
        if (verifyColor(myButton))
        {
            myButton.Modulate = new Color(1, 59, 0);
            Color backgroundColor = myButton.Modulate;
            // Recuperer position du bouton pour recuperer les bouton autour de lui
            int position = Convert.ToInt32(myButton.Name.Split('n')[1]);
            colorRandomCase(position);
        }

    }
    public List<Button> getButtons(int position)
    {
        List<Button> listButton = new List<Button>();
        Button buttonTop = GetNode<Button>("pnlMain/gridMap/Button" + (position - 10));
        listButton.Add(buttonTop);
        Button buttonBottom = GetNode<Button>("pnlMain/gridMap/Button" + (position + 10));
        listButton.Add(buttonBottom);
        Button buttonLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position - 1));
        listButton.Add(buttonLeft);
        Button buttonRight = GetNode<Button>("pnlMain/gridMap/Button" + (position + 1));
        listButton.Add(buttonRight);
        // Recuperer les boutons en diagonale
        Button buttonTopLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position - 11));
        listButton.Add(buttonTopLeft);
        Button buttonTopRight = GetNode<Button>("pnlMain/gridMap/Button" + (position - 9));
        listButton.Add(buttonTopRight);
        Button buttonBottomLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position + 9));
        listButton.Add(buttonBottomLeft);
        Button buttonBottomRight = GetNode<Button>("pnlMain/gridMap/Button" + (position + 11));
        listButton.Add(buttonBottomRight);
        return listButton;
    }
    public void colorRandomCase(int position)
    {


        Random random = new Random();
        int numberButtomColor = random.Next(1, 6);
        List<int> listButton = new List<int>();
        for (int i = 0; i < getButtons(position).Count; i++)
        {
            int number = random.Next(0, 7);
            if (!listButton.Contains(number))
            {
                listButton.Add(number);
                if (verifyColor(getButtons(position)[number]))
                {
                    getButtons(position)[number].Modulate = new Color(1, 59, 0);
                }

            }
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
