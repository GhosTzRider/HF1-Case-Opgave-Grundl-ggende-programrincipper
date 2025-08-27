namespace Plukliste;
public class Pluklist
{
    public string? Name;
    public string? Forsendelse;         // Pluklisten indeholder et navn, en forsendelse og en adresse
    public string? Adresse;
    public List<Item> Lines = new List<Item>(); // og listen af linjer er defineret som en liste af Item objekter
    public void AddItem(Item item) { Lines.Add(item); } // Metode til at tilføje et item til listen
}

public class Item
{
    public string ProductID;    // klassen Item indeholder et produkt ID, titel, type og antal
    public string Title;
    public ItemType Type;
    public int Amount;
}

public enum ItemType        /// og itemtypen kan være en af følgende:
{
    Fysisk, Print, Pickup
}
