using Godot;

public class Poison
{
    public Node2D Poisoner;
    public ChunkData chunk;
    public int turnsLeft;
    public int poisonValue;
    
    public Poison(ChunkData chunk, int turnsleft, int poisonvalue)
    {
        this.chunk = chunk;
        turnsLeft = turnsleft;
        poisonValue = poisonvalue;
    }
}