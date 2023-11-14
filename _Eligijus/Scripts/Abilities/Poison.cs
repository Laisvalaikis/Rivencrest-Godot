using Godot;

public class Poison
{
    public Player Poisoner;
    public ChunkData chunk;
    public int turnsLeft;
    public int poisonValue;

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