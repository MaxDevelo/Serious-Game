using Godot;
using System;

public class Global : Node
{
    private int money;
    private int ecology;
    private int sociabilite;
    private Global instance;


    public override void _Ready()
    {


    }

    public void retrieveData(){
     
    }

    public Global getInstance()
    {
        if (instance == null)
        {
            instance = new Global();
        }
        return instance;
    }


    public int getMoney()
    {
        return money;
    }

    public int getEcology()
    {
        return ecology;
    }
    public int getSociabilite()
    {
        return sociabilite;
    }

}
