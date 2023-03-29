using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            //Destroy(gameObject);
        }
    }
}
