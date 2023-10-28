using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Match3
{
    public abstract class GameSystem
    {
        public virtual void Initialize()
        {

        }
    }

    public class SingletonGameSystem<T> : GameSystem where T : GameSystem, new()
    {
        public static T Instance { get; private set; }

        public static T CreateInstance()
        {
            Instance = new T();
            return Instance;
        }

        public static void InitializeInstance()
        {
            Instance.Initialize();
        }
    }
}