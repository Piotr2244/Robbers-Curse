using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    private Transform player;
    public GameObject background;

    private Vector3 tempPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        tempPos = transform.position;
        tempPos.x = player.position.x;
        tempPos.y = player.position.y;// + (1.5f);
        transform.position = tempPos;
        background.transform.position = new Vector3(tempPos.x, tempPos.y - 2.0f, background.transform.position.z);

    }
}
