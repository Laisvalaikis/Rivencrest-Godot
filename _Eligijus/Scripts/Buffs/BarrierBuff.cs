using System;
using System.Threading.Tasks;
using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BarrierBuff : BaseBuff
{
    public BarrierBuff()
    {
        
    }
    public BarrierBuff(BarrierBuff buff): base(buff)
    {
        buffAnimation = "Barrier"; //this bitch dont exist, cia per parametrus gal pavadinima
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        BarrierBuff buff = new BarrierBuff((BarrierBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        BarrierBuff buff = new BarrierBuff(this);
        return buff;
    }
    
    public override async void Start()
    {
        
    }
    
    public override void ResolveBuff(ChunkData chunkData)
    {
        
    }
    
    public override void ModifyDamage(ref int damage, ref Player player)
    {
        damage = 0;
    }
}