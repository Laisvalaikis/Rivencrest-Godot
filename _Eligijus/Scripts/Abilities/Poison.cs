using Godot;

public class Poison
{
    public Player Poisoner;
    public ChunkData chunk;
    public int turnsLeft;
    public int poisonValue;

    //Do not use this class anymore (bijau istrint)
    public Poison()
    {
        
    }
    public Poison(ChunkData chunk, int turnsleft, int poisonvalue)
    {
        this.chunk = chunk;
        turnsLeft = turnsleft;
        poisonValue = poisonvalue;
    }
}