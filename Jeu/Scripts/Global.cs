using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public class Global : Node
{
    private int money = 100000;
    private int ecology = 100;
    private int sociabilite = 100;
    private Global instance;


    public override void _Ready()
    {


    }

    public List<Activite> retrieveDataActivite()
    {
        string json = System.IO.File.ReadAllText("data/info.json");
        JObject jsonData = JsonConvert.DeserializeObject<JObject>(json);
        JArray activiteArray = (JArray)jsonData.GetValue("activite");
        List<Activite> activites = activiteArray.ToObject<List<Activite>>();
        return activites;
    }
    public Activite retrieveDataAmelioration_t1(int indexSelected)
    {
        string json = System.IO.File.ReadAllText("data/info.json");
        JObject jsonData = JsonConvert.DeserializeObject<JObject>(json);
        JArray activiteArray = (JArray)jsonData.GetValue("amelioration_t1");
        List<Activite> activites = activiteArray.ToObject<List<Activite>>();

        return activites[indexSelected];
    }
    public Activite retrieveDataAmelioration_t2(int indexSelected)
    {
        string json = System.IO.File.ReadAllText("data/info.json");
        JObject jsonData = JsonConvert.DeserializeObject<JObject>(json);
        JArray activiteArray = (JArray)jsonData.GetValue("amelioration_t2");
        List<Activite> activites = activiteArray.ToObject<List<Activite>>();

        return activites[indexSelected];
    }

    public Global getInstance()
    {
        if (instance == null)
        {
            instance = new Global();
        }
        return instance;
    }
    public void setModificationBar(int money, int ecology, int sociabilite)
    {
        if (this.money + money < 0)
        {
            this.money = 0;
        }
        else
        {
            this.money += money;
        }
        if (this.ecology + ecology < 0)
        {
            this.ecology = 0;
        }
        else
        {
            this.ecology += ecology;
        }
        if (this.sociabilite + sociabilite < 0)
        {
            this.sociabilite = 0;
        }
        else
        {
            this.sociabilite += sociabilite;
        }
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
