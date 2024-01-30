public class Poison
{
    public ChunkData chunk;
    public Player Poisoner;
    public int poisonValue;
    public int turnsLeft;

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