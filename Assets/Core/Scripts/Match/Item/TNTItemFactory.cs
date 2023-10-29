using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Match3
{
    [CreateAssetMenu(fileName = "TNT Item Factory", menuName = "TNT Item Factory")]
    public class TNTItemFactory : ItemFactory
    {
        #region VARIABLES
        [SerializeField] private GameObject bombPrefab;
        #endregion
        

        public override Item CreateItem()
        {
            GameObject createdItem = Instantiate(ItemPrefab);
            var skills = GetSkills();
            Item item = new TNTItem(dataId, itemName, createdItem, bombPrefab, skills, canFall);
            return item;
        }

        
    }
}