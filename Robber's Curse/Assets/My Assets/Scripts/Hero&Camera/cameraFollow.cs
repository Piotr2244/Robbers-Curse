using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script making camera object follow the
 *  player on scene*/
public class cameraFollow : MonoBehaviour
{
    // Variables
    private Vector3 tempPos;
    // References
    private Transform player;
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    // Update is called after update
    void LateUpdate()
    {
        tempPos = transform.position;
        tempPos.x = player.position.x;
        tempPos.y = player.position.y;
        transform.position = tempPos;
        background.transform.position = new Vector3(tempPos.x, tempPos.y - 2.0f, background.transform.position.z);
    }
}
