using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static Godot.Control;

public class MainScene : Node2D
{
    private Bar bar;
    private Global global;
    private Panel pnlChooseActivite;
    private List<Button> buttonsTramWay;
    private List<Boolean> buttonsFree;
    private Activite currentlyActivite; // Sauvegarder Activite actuel
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        buttonsFree = new List<Boolean>();
        buttonsTramWay = new List<Button>();
        // getInstance of global
        global = GetNode<Global>("/root/Global");
        global.getInstance();
        bar = GetNode<Bar>("Bar");
        pnlChooseActivite = GetNode<Panel>("pnlChooseActivite");
        pnlChooseActivite.Visible = false;

        setModificationBar(global.getMoney(), global.getEcology(), global.getSociabilite());
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
            buttonsFree.Add(true);
            // Create a new button
            Button myButton = new Button();
            // Charger le thème à partir du fichier de ressources
            Theme theme;
            if (i > 50 && i <= 60)
            {
                theme = GD.Load<Theme>("res://Themes/tramway.tres");
                buttonsTramWay.Add(myButton);
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
            // Generate Button Pressed with function
            myButton.Connect("pressed", this, "onButtonPressed", new Godot.Collections.Array() { myButton, i });
        }


    }
    /*
        Clique sur une case
    */
    public void onButtonPressed(Button myButton, int index)
    {
        if (buttonsFree[index])
        {
            buttonsFree[index] = false;
            pnlChooseActivite.Visible = true;
            Button btnClose = GetNode<Button>("pnlChooseActivite/btnClose");
            GridContainer gridActivite = GetNode<GridContainer>("pnlChooseActivite/gridActivite");
            if (btnClose.GetSignalConnectionList("pressed").Count == 0)
            {
                btnClose.Connect("pressed", this, "_on_btnClose_pressed");
            }
            for (int i = 1; i <= global.retrieveDataActivite().Count; i++)
            {
                Button btnActivite = new Button();
                Theme theme = GD.Load<Theme>("res://Themes/chooseActivite.tres");
                btnActivite.Theme = theme;
                btnActivite.Name = "btnActivite" + i;
                btnActivite.Text = global.retrieveDataActivite()[i - 1].Title;
                btnActivite.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
                btnActivite.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
                if (btnActivite.GetSignalConnectionList("pressed").Count == 0)
                {
                    Activite activite = global.retrieveDataActivite()[i - 1];
                    currentlyActivite = activite;
                    btnActivite.Connect("pressed", this, "onActivitePressed", new Godot.Collections.Array() { activite.Theme, btnActivite, myButton, global.retrieveDataActivite()[i - 1].Id });
                }
                gridActivite.AddChild(btnActivite);
            }
        }

    }

    public void onActivitePressed(String themeActivit, Button btnActivite, Button myButton, int id)
    {
        Color backgroundColor = myButton.Modulate;
        // Recuperer position du bouton pour recuperer les bouton autour de lui
        int position = Convert.ToInt32(myButton.Name.Split('n')[1]);
        Theme theme = GD.Load<Theme>("res://" + themeActivit);
        myButton.Theme = theme;
        GD.Print("Money: " + currentlyActivite.Money + " Ecology: " + currentlyActivite.Ecology + " Sociability: " + currentlyActivite.Sociability);
        global.setModificationBar(currentlyActivite.Money, currentlyActivite.Ecology, currentlyActivite.Sociability);
        setModificationBar(global.getMoney(), global.getEcology(), global.getSociabilite());
        colorRandom(position, theme);

        clearGridContainer();
        pnlChooseActivite.Visible = false;
    }

    public void _on_btnClose_pressed()
    {
        pnlChooseActivite.Visible = false;
        clearGridContainer();

    }

    public List<Button> getButtons(int position)
    {
        List<Button> listButton = new List<Button>();
        if (position - 10 > 0 && position - 10 < 100)
        {
            if (buttonsFree[position - 10])
            {
                // Vérifier que le Button existe avant de l'ajouter
                Button buttonTop = GetNode<Button>("pnlMain/gridMap/Button" + (position - 10));
                listButton.Add(buttonTop);
            }
        }
        if (position + 10 > 0 && position + 10 < 100 && buttonsFree[position - 10])
        {
            if (buttonsFree[position + 10])
            {
                ;
                Button buttonBottom = GetNode<Button>("pnlMain/gridMap/Button" + (position + 10));
                listButton.Add(buttonBottom);
            }
        }
        if (position - 1 > 0 && position - 1 < 100)
        {
            if (buttonsFree[position - 1])
            {
                ;
                Button buttonLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position - 1));
                listButton.Add(buttonLeft);
            }
        }
        if (position + 1 > 0 && position + 1 < 100)
        {
            if (buttonsFree[position + 1])
            {
                Button buttonRight = GetNode<Button>("pnlMain/gridMap/Button" + (position + 1));
                listButton.Add(buttonRight);
            }
        }
        // Recuperer les boutons en diagonale
        if (position - 11 > 0 && position - 11 < 100)
        {
            if (buttonsFree[position - 11])
            {
                Button buttonTopLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position - 11));
                listButton.Add(buttonTopLeft);
            }
        }
        if (position - 9 > 0 && position - 9 < 100)
        {
            if (buttonsFree[position - 9])
            {
                Button buttonTopRight = GetNode<Button>("pnlMain/gridMap/Button" + (position - 9));
                listButton.Add(buttonTopRight);
            }
        }
        if (position + 9 > 0 && position + 9 < 100)
        {
            if (buttonsFree[position + 9])
            {
                Button buttonBottomLeft = GetNode<Button>("pnlMain/gridMap/Button" + (position + 9));
                listButton.Add(buttonBottomLeft);
            }
        }
        if (position + 11 > 0 && position + 11 < 100)
        {
            if (buttonsFree[position + 11])
            {
                Button buttonBottomRight = GetNode<Button>("pnlMain/gridMap/Button" + (position + 11));
                listButton.Add(buttonBottomRight);
            }
        }
        return listButton;
    }
    /*
        Colorier de manière random les cases
    */
    public void colorRandom(int position, Theme theme)
    {
        Random random = new Random();
        int numberButtomColor = random.Next(1, 6);
        List<int> listButton = new List<int>();
        List<Button> buttons = getButtons(position);
        GD.Print(buttons.Count);
        for (int i = 0; i < buttons.Count; i++)
        {
            int number = random.Next(0, 7);
            if (!listButton.Contains(number))
            {
                listButton.Add(number);
                if (number < listButton.Count && !verifiyIfButtonsTramWay(buttons[number]))
                {
                    buttonsFree[number] = false;
                    // Construction en méttant une couleur
                    buttons[number].Theme = theme;
                }
            }
        }
    }
    /*
        Vérifier qu'il s'agit ou non du chemin de fer
    */
    public Boolean verifiyIfButtonsTramWay(Button button)
    {
        for (int i = 0; i < buttonsTramWay.Count; i++)
        {
            if (button.Name == buttonsTramWay[i].Name)
            {
                return true;
            }
        }
        return false;
    }

    /*
        Modification des Bars
    */
    public void setModificationBar(int money, int ecology, int sociabilite)
    {
        bar.setMoney(money);
        bar.setEcology(ecology);
        bar.setSociabilite(sociabilite);
    }

    public void clearGridContainer()
    {
        GridContainer gridActivite = GetNode<GridContainer>("pnlChooseActivite/gridActivite");
        for (int i = 0; i < gridActivite.GetChildCount(); i++)
        {
            gridActivite.GetChild(i).QueueFree();
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
