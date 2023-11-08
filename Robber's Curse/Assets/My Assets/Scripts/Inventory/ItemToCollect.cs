using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemToCollect : MonoBehaviour
{

    public int index = 0; //tem indexes are in InventoryBase
    public delegate void ItemCollected(int itemIndex, GameObject instance);
    public static event ItemCollected OnItemCollision;

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
    private void UpdateIfCanBEPicked()
    {

    }
}
