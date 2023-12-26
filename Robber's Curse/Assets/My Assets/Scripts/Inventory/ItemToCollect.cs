using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/* Item that can be picked on map */
public class ItemToCollect : MonoBehaviour
{
    public int index = 0; //tem indexes are in InventoryBase
    // Events and delegates
    public delegate void ItemCollected(int itemIndex, GameObject instance);
    public static event ItemCollected OnItemCollision;
    // Get item after player colides with it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (OnItemCollision != null)
            {
                OnItemCollision.Invoke(index, gameObject);
            }
        }
    }
}
