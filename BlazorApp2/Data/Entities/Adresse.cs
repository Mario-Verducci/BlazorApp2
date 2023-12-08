namespace BlazorApp2.Data.Entities;

public class Adresse
{
    public int Id { get; set; }
    public string Name1 { get; set; }
    public string Name2 { get; set; }
    public string Strasse { get; set; }
    public string Plz { get; set; }
    public string Ort { get; set; }

    //Timestamps for Sync
    public DateTime? LastSynced { get; set; }
    public bool Modified { get; set; }
    public bool Deleted { get; set; }
}