using Godot;
using System;

public class Bar : Panel
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public int getMoney()
    {
        Label value = GetTree().CurrentScene.GetNode("pnlEconomy/lblMoney") as Label;
        string[] parts = value.Text.Split('€');
        long money = Convert.ToInt64(parts[0]);
        return (int)money;
    }

    public int getEcology()
    {
        Label lblEcology = GetNode<Label>("pnlEcology/lblEcology");
        int ecology = Convert.ToInt32(lblEcology.Text.Split('%')[0]);
        return ecology;
    }

    public int getSociabilite()
    {
        Label lblSociabilite = GetNode<Label>("pnlSociabilite/lblSociabilite");
        int sociabilite = Convert.ToInt32(lblSociabilite.Text.Split('%')[0]);
        return sociabilite;
    }
    public void setMoney(int value)
    {
        // Display Money
        Label lblMoney = GetNode("pnlEconomy/lblMoney") as Label;
        string[] parts = lblMoney.Text.Split('€');
        long money = Convert.ToInt64(parts[0]);
        money += value;
        lblMoney.Text = money.ToString() + "€";

    }

    public void setEcology(int value)
    {
        // Display Ecology
        Label lblEcology= GetNode("pnlEcology/lblEcology") as Label;
        string[] parts = lblEcology.Text.Split('%');
        long ecology = Convert.ToInt64(parts[0]);
        ecology += value;
        lblEcology.Text = ecology.ToString() + "%";

    }

    public void setSociabilite(int value)
    {
        // Display Sociabilite
        Label lblSociabilite= GetNode("pnlSociabilite/lblSociabilite") as Label;
        string[] parts = lblSociabilite.Text.Split('%');
        long sociabilite = Convert.ToInt64(parts[0]);
        sociabilite += value;
        lblSociabilite.Text = sociabilite.ToString() + "%";
    }
}
