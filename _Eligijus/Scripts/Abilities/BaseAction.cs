using System;
using Godot;

    public abstract partial class BaseAction: Node
    {
       
        [Export] protected bool laserGrid = false;
        [Export] public Node spawnedCharacter;
        //private RaycastHit2D raycast;
        [Export]
        public int AttackRange = 1;
        [Export]
        public int AbilityCooldown = 1;
        [Export]
        public int minAttackDamage = 0;
        [Export]
        public int maxAttackDamage = 0;
        [Export]
        public bool isAbilitySlow = true;
        [Export]
        public bool friendlyFire = false;

        private PlayerInformationData _playerInformationData;

        void Awake()
        {
            
        }
        
        public virtual string GetDamageString()
        {
            return $"{minAttackDamage}-{maxAttackDamage}";
        }
        
    }

