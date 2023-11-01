using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentLevel;
        public List<int> UnlockedTasks;
        public List<AreaTask> AreaTaskData;
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
            PlayerData.CurrentLevel = 1;
            PlayerData.AreaTaskData = new List<AreaTask>();

            for (int i = 0; i < Main.Instance.AreaTasks.Count; i++)
            {
                PlayerData.AreaTaskData.Add(new AreaTask(i, TaskState.LOCKED)); ;
            }
        }
    }
}