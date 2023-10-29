using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentLevel;
    }

    public class Player : SingletonGameSystem<Player>
    {
        #region PROPERTIES
        
        public PlayerData PlayerData;

        #endregion

        public override void Initialize()
        {
            base.Initialize();
            PlayerData = new PlayerData();
            PlayerData.CurrentLevel = 7;
        }
    }
}