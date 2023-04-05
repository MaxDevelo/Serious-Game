using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public class Global : Node
{
    private int money = 50;
    private int ecology = 100;
    private int sociabilite = 100;
    private Global instance;
    private int date;
    private int index; // Index dans le JSON (Activite -> amelioration_t1 -> etc...)


    public override void _Ready()
    {
        date = 1900;
        index = 0;
    }

    public int getIndex(){
        return index;
    }

    public int getDate(){
        return date;
    }
    public void setDate(){
        this.date += 25;
    }

    public List<Activite> retrieveDataActivite()
    {
        string json = System.IO.File.ReadAllText("data/info.json");
        JObject jsonData = JsonConvert.DeserializeObject<JObject>(json);
        JArray activiteArray = (JArray)jsonData.GetValue("activite");
        List<Activite> activites = activiteArray.ToObject<List<Activite>>();
        return activites;
    }
    public List<Activite> retrieveDataAmelioration_t1()
    {
        string json = System.IO.File.ReadAllText("data/info.json");
        JObject jsonData = JsonConvert.DeserializeObject<JObject>(json);
        JArray activiteArray = (JArray)jsonData.GetValue("amelioration_t1");
        List<Activite> activites = activiteArray.ToObject<List<Activite>>();

        return activites;
    }
    public List<Activite> retrieveDataAmelioration_t2()
    {
        string json = System.IO.File.ReadAllText("data/info.json");
        JObject jsonData = JsonConvert.DeserializeObject<JObject>(json);
        JArray activiteArray = (JArray)jsonData.GetValue("amelioration_t2");
        List<Activite> activites = activiteArray.ToObject<List<Activite>>();

        return activites;
    }

    public Global getInstance()
    {
        if (instance == null)
        {
            instance = new Global();
        }
        return instance;
    }
    public void clearAll(){
        this.money = 50;
        this.ecology = 100;
        this.sociabilite = 100;
        this.date = 1900;
        this.index = 0;
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
        if(this.ecology > 100){
            this.ecology = 100;
        }
        if(this.money > 100){
            this.money = 100;
        }
        if(this.sociabilite > 100){
            this.sociabilite = 100;
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

    public void newDate(){
        setDate();
        this.index++;
    }
}
