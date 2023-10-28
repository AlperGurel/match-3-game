using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3;
using UnityEngine;

namespace Match3
{
    public class ItemFactoryManager : SingletonGameSystem<ItemFactoryManager>
    {
        #region VARIABLES

        private List<ItemFactory> itemFactories = new List<ItemFactory>();
        #endregion
        
        public override void Initialize()
        {
            itemFactories = Resources.LoadAll<ItemFactory>("").ToList();
            
            base.Initialize();
        }

        public ItemFactory GetItemFactory(string id)
        {
            return itemFactories.FirstOrDefault(x => x.DataId == id);
        }
    }
}