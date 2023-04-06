using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static Godot.Control;

public class MainScene : Node2D
{


    private Bar bar;
    private Global global;
    private Panel pnlChooseActivite, pnlEndDate, pnlEchap, pnlWinOrLose, pnlNews;
    private List<Button> buttonsTramWay;
    private List<Boolean> buttonsFree;
    private Label lblDate;
    private Boolean changeDate;
    private Panel pnlInfoMaire;
    private Godot.Image screen;
    private String currentTheme;
    private int currentPositionTheme;
    private Button parcheminDateClosed, parcheminDateOpen, btnNews;

    private GridContainer myGridContainer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        btnNews = GetNode<Button>("btnNews");
        pnlNews = GetNode<Panel>("pnlNews");
        pnlNews.Visible = false;
        currentPositionTheme = -1;
        parcheminDateClosed = GetNode<Button>("pnlMain/BtnCloseDate");
        parcheminDateOpen = GetNode<Button>("pnlMain/BtnOpenDate");
        parcheminDateOpen.Visible = false;
        pnlWinOrLose = GetNode<Panel>("pnlWinOrLose");
        pnlWinOrLose.Visible = false;
        pnlInfoMaire = GetNode<Panel>("pnlInfoMaire");
        pnlInfoMaire.Visible = true;
        pnlEchap = GetNode<Panel>("pnlEchap");
        pnlEchap.Visible = false;
        changeDate = true;
        buttonsFree = new List<Boolean>();
        buttonsTramWay = new List<Button>();
        pnlEndDate = GetNode<Panel>("pnlEndDate");
        lblDate = GetNode<Label>("pnlMain/BtnOpenDate/lblDate");
        pnlEndDate.Visible = false;
        // getInstance of global
        global = GetNode<Global>("/root/Global");
        global.getInstance();
        lblDate.Text = global.getDate().ToString();
        bar = GetNode<Bar>("Bar");
        pnlChooseActivite = GetNode<Panel>("pnlChooseActivite");
        pnlChooseActivite.Visible = false;
        setModificationBar(global.getMoney(), global.getEcology(), global.getSociabilite());
        generateMap();
        DisplayText(); // Afficher info Maire
    }
    public void _on_btnNews_pressed()
    {
        restoreNews();
        pnlNews.Visible = true;
    }
    public void restoreNews()
    {
        Label desc = GetNode<Label>("pnlNews/desc");
        Label date = GetNode<Label>("pnlNews/date");
        Label title = GetNode<Label>("pnlNews/title");
        List<News> news = global.retrieveDataNews();
        date.Text = news[global.getIndex()].Date + "";
        title.Text = news[global.getIndex()].Title;
        desc.Text = news[global.getIndex()].Description;
    }
    void _on_BtnQuit_News_pressed()
    {
        pnlNews.Visible = false;
    }
    public void _on_BtnOpenDate_pressed()
    {
        parcheminDateOpen.Visible = true;
        parcheminDateClosed.Visible = false;
    }
    public void _on_BtnCloseDate_pressed()
    {
        parcheminDateOpen.Visible = false;
        parcheminDateClosed.Visible = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            if (pnlEchap.Visible)
            {
                pnlEchap.Visible = false;
            }
            else
            {
                GetNode<Button>("pnlEchap/btnQuit").Connect("pressed", this, "_on_btnQuit_pressed");
                GetNode<Button>("pnlEchap/btnRestart").Connect("pressed", this, "_on_btnRestart_pressed");
                pnlEchap.Visible = true;
            }


        }
        // Si je clique sur entrer
        if (@event.IsActionPressed("ui_accept"))
        {
            pnlInfoMaire.Visible = false;
        }

        //hover du btn 
        if (@event is InputEventMouseMotion motion)
        {

            foreach (Button button in GetNode<GridContainer>("pnlMain/gridMap").GetChildren())
            {
                Rect2 buttonRect = new Rect2(button.RectGlobalPosition, button.RectSize);
                if (buttonRect.HasPoint(motion.Position))
                {
                    String x = button.Theme.ResourcePath.Substring(13, button.Theme.ResourcePath.Length - 18);
                    Label label = GetNode<Label>("Label");
                    label.Text = x;
                    label.RectGlobalPosition = new Vector2(motion.Position.x + 15, motion.Position.y - 15);
                }
            }


        }



    }
    /*
		Fonciton qui gère le Maire quand il parle
	*/
    private async void DisplayText()
    {
        Theme theme_base = GD.Load<Theme>("res://Themes/Maire/Maire_base.tres");
        Theme theme_open = GD.Load<Theme>("res://Themes/Maire/Maire_open.tres");



        String text = "Bonjour, je suis votre secrétaire M. Assog, responsable du quartier de Koenigshoffen-Est. Je suis là pour vous aider à gérer votre quartier !";
        for (int i = 0; i < text.Length; i++)
        {
            pnlInfoMaire.Theme = theme_open;
            GetNode<Label>("pnlInfoMaire/pnlInfo/Label").Text += text[i];
            await Task.Delay((int)10f);
            pnlInfoMaire.Theme = theme_base;
        }
        await Task.Delay((int)3000f);
        text = "Afin de ne pas vous perdre, je vais vous expliquer les différentes actions que vous pouvez faire.";
        GetNode<Label>("pnlInfoMaire/pnlInfo/Label").Text = "";
        for (int i = 0; i < text.Length; i++)
        {
            GetNode<Label>("pnlInfoMaire/pnlInfo/Label").Text += text[i];
            await Task.Delay((int)10f);
            pnlInfoMaire.Theme = theme_base;
        }
        await Task.Delay((int)3000f);
        text = "Vous avez juste à cliquer sur une case pour voir les actions possibles ; ensuite, il suffit de choisir une. Faites attention, car votre choix a un impact sur les jauges!";
        GetNode<Label>("pnlInfoMaire/pnlInfo/Label").Text = "";
        for (int i = 0; i < text.Length; i++)
        {
            GetNode<Label>("pnlInfoMaire/pnlInfo/Label").Text += text[i];
            pnlInfoMaire.Theme = theme_open;
            await Task.Delay((int)10f);
            pnlInfoMaire.Theme = theme_base;
        }

    }

    /*
		Recommencer le jeu
	*/

    public void _on_btnRestart_pressed()
    {
        global.clearAll();
        GetTree().ChangeScene("res://Scenes/Configuration.tscn");
    }

    public void _on_btnQuit_pressed()
    {
        GetTree().Quit();
    }


    /*
	* This method will generate a map of 100 buttons to create map
	*/
    public void generateMap()
    {
        myGridContainer = GetNode<GridContainer>("pnlMain/gridMap");
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
                myButton.Disabled = true;
                buttonsTramWay.Add(myButton);
            }
            else if (i < 50)
            {
                theme = GD.Load<Theme>("res://Themes/green_tree.tres");
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
        Label label = GetNode<Label>("Label");
        label.Visible = false;
        screen = createScreenshot();

        if (buttonsFree[index])
        {
            pnlChooseActivite.Visible = true;
            Button btnClose = GetNode<Button>("pnlChooseActivite/btnClose");
            GridContainer gridActivite = GetNode<GridContainer>("pnlChooseActivite/gridActivite");
            if (btnClose.GetSignalConnectionList("pressed").Count == 0)
            {
                btnClose.Connect("pressed", this, "_on_btnClose_pressed");
            }
            List<Activite> activites = (global.getIndex() == 0 ? global.retrieveDataActivite() : (global.getIndex() == 1 ? global.retrieveDataAmelioration_t1() : global.retrieveDataAmelioration_t2()));
            for (int i = 1; i <= activites.Count; i++)
            {
                Activite activite = activites[i - 1];
                Button btnActivite = new Button();
                Theme theme = GD.Load<Theme>("res://Themes/chooseActivite.tres");
                btnActivite.Theme = theme;
                btnActivite.Name = "btnActivite" + i;
                btnActivite.Text = activites[i - 1].Title;
                btnActivite.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
                btnActivite.SizeFlagsVertical = (int)SizeFlags.ExpandFill;

                if (btnActivite.GetSignalConnectionList("pressed").Count == 0)
                {
                    btnActivite.Connect("pressed", this, "onActivitePressed", new Godot.Collections.Array() { index, activite.Title, activite.isActivity, activite.Theme, activite.Ecology, activite.Sociability, activite.Money, btnActivite, myButton, activites[i - 1].Id });
                }
                gridActivite.AddChild(btnActivite);
            }
        }

    }


    public Godot.Image createScreenshot()
    {
        Viewport viewport = GetViewport();
        Texture texture = viewport.GetTexture();

        Godot.Image image = texture.GetData();
        image.FlipY();
        image = image.GetRect(new Rect2(0, 0, (float)(viewport.Size.x / 1.8), viewport.Size.y));

        image.SavePng("user://screenshot.png");
        return image;
    }

    public void onActivitePressed(int index, String name, Boolean isActivity, String themeActivite, int ecology, int sociabilite, int money, Button btnActivite, Button myButton, int id)
    {
        pnlNews.Visible = false;
        // Recuperer position du bouton pour recuperer les bouton autour de lui
        int position = Convert.ToInt32(myButton.Name.Split('n')[1]);
        Theme theme = GD.Load<Theme>("res://" + themeActivite);
        global.setModificationBar(money, ecology, sociabilite);
        setModificationBar(global.getMoney(), global.getEcology(), global.getSociabilite());
        if (name == "Agrandir" && currentTheme != null && currentPositionTheme != -1)
        {
            colorRandom(currentPositionTheme, GD.Load<Theme>("res://" + currentTheme));
        }
        else if (name == "Usine de Biere")
        {
            colorRandom(position, GD.Load<Theme>("res://Themes/champs.tres"));
            buttonsFree[index] = false;
            myButton.Theme = theme;
            colorRandom(position, theme);
        }
        else
        {
            colorRandom(position, GD.Load<Theme>("res://Themes/Habitations.tres"));
            buttonsFree[index] = false;
            myButton.Theme = theme;
            colorRandom(position, theme);
        }

        clearGridContainer();
        pnlChooseActivite.Visible = false;
        pnlEndDate.Visible = true;

        //Affichage des informations (Récapitulatif)
        Label lblDesc = GetNode<Label>("pnlEndDate/lblDesc");
        lblDesc.Text = "Titre:     " + name + "\n" +
            "Argent:     " + money + "\n" +
            "Sociabilité:     " + sociabilite + "\n" +
            "Ecologie:     " + ecology;
        // CLiquer sur "Avancer de 25 ans)
        Button btnContinue = GetNode<Button>("pnlEndDate/btnContinue");
        if (btnContinue.GetSignalConnectionList("pressed").Count == 0)
        {
            btnContinue.Connect("pressed", this, "_on_btnContinue_pressed");
        };
        // Stocker le thème principal
        if (isActivity)
        {
            currentTheme = themeActivite;
            currentPositionTheme = position;
        }

        bar.setScreen(screen);
        restoreNews();
        pnlNews.Visible = false;
		btnNews.Visible = false;

    }
    public void _on_btnContinue_pressed()
    {
        changeDate = true;
        pnlEndDate.Visible = false;
        global.newDate();
        Label label = GetNode<Label>("Label");
        label.Visible = true;
        pnlNews.Visible = false;
		btnNews.Visible = true;
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
        int numberButtonColor = random.Next(2, 4);
        List<Button> buttons = getButtons(position);

        for (int i = 0; i < numberButtonColor; i++)
        {
            int number;
            Button selectedButton;
            do
            {
                number = random.Next(buttons.Count);
                selectedButton = buttons[number];
                break;
            } while (!buttonsFree[number]);
            if (!verifiyIfButtonsTramWay(selectedButton))
            {
                buttonsFree[number] = false;
                selectedButton.Theme = theme;
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (changeDate)
        {
            Label lblDate = GetNode<Label>("pnlEndDate/lblTitle");
            lblDate.Text = "Date: " + global.getDate();
            lblDate = GetNode<Label>("pnlMain/BtnOpenDate/lblDate");
            lblDate.Text = "Date: " + global.getDate();
            changeDate = false;
        }
        if (global.getMoney() <= 0 || global.getEcology() <= 0 || global.getSociabilite() <= 0)
        {
            pnlEndDate.Visible = false;
            pnlWinOrLose.Visible = true;
            GetNode<Label>("pnlWinOrLose/lblWinOrLose").Text = "Vous avez Perdu !";
            GetNode<Label>("pnlWinOrLose/lblRecap").Text = "Date: " + global.getDate() + "\n\nRécapitulatif: \r" + "Argent: " + global.getMoney() + "%\n" + "Ecologie: " + global.getEcology() + "%\n" + "Sociabilité: " + global.getSociabilite() + "%";
        }
        if (global.endParty())
        {
            pnlEndDate.Visible = false;
            pnlWinOrLose.Visible = true;
            GetNode<Label>("pnlWinOrLose/lblWinOrLose").Text = "Vous avez Gagné !";
            GetNode<Label>("pnlWinOrLose/lblRecap").Text = "Date: " + global.getDate() + "\n\nRécapitulatif: \r" + "Argent: " + global.getMoney() + "%\n" + "Ecologie: " + global.getEcology() + "%\n" + "Sociabilité: " + global.getSociabilite() + "%";
        }
    }

    public void _on_btnRestartWinorLose_pressed()
    {
        global.clearAll();
        GetTree().ChangeScene("res://Scenes/Configuration.tscn");
    }
}
