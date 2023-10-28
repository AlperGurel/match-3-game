using System.Collections.Generic;
using Match3;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Factory - Cube Random", menuName = "Item Factory - Cube Random")]
public class RandomItemFactory : ItemFactory
{
    #region VARIABLES

    [SerializeField] public List<ItemFactory> itemFactories;

    #endregion

    public override Item CreateItem()
    {
        int ind = Random.Range(0, itemFactories.Count); // Generate a random index within the range of the list.
        ItemFactory cubeFactory = itemFactories[ind];
        return cubeFactory.CreateItem();
    }
}