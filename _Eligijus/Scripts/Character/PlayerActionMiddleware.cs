namespace Rivencrestgodot._Eligijus.Scripts.Character;

public class PlayerActionMiddleware
{
    public Player _player;
    
    public void Death()
    {
        _player.Death();    
    }

    public void PlayerWasDamaged()
    {
        _player.PlayerWasDamaged();
    }

}