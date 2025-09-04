namespace Plukliste;
public class Pluklist
{

    public string? Name { get; set; }
    public string? Forsendelse { get; set; }         // Pluklisten indeholder et navn, en forsendelse og en adresse
    public string? Adresse { get; set; }
    public List<Item> Lines { get; set; }// og listen af linjer er defineret som en liste af Item objekter
    // public List<Item> Lines = new List<Item>(); 
    public void AddItem(Item item) { Lines.Add(item); } // Metode til at tilføje et item til listen
    public Pluklist()
    {
        Lines = new List<Item>();
    }
}

public class Item
{
    public string ProductID { get; set; }    // klassen Item indeholder et produkt ID, titel, type og antal
    public string Title { get; set; }
    public ItemType Type { get; set; }
    public int Amount { get; set; } = 0;
}

public enum ItemType        /// og itemtypen kan være en af følgende:
{
    Fysisk, Print, Pickup
}
