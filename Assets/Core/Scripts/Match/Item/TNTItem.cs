using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace  Match3
{
    public class TNTItem : Item
    {
        
        
        public TNTItem(string id, string name, GameObject gameObject, List<ISkill> skills, bool canFall) : base(id, name, gameObject, skills, canFall)
        {
            
        }

        public override async void Despawn()
        {
            //trigger bomb with 5x5 impact radius
            Debug.Log("Trigger Bomb");
            base.Despawn();
        }
    }
}