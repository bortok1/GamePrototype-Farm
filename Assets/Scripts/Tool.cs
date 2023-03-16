using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETool
{
    None,
    WateringCan,
    Shovel,
    Hoe
}

public class Tool : MonoBehaviour
{
    public ETool toolType;
    public EPlayerID owner = EPlayerID.None;
    Rigidbody rb;
    CapsuleCollider collider;
    Vector2 center;

    public void DropTool()
    {
        Debug.Log("droptool");
        transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        transform.parent = null;
        owner = EPlayerID.None;
        rb.isKinematic = false;
        collider.isTrigger = false;
        collider.enabled = true;
        Vector2 shift = center - new Vector2(transform.position.x, transform.position.z);
        shift.Normalize();
        transform.position += new Vector3(shift.x * 2, 2, shift.y*2);
    }

    public void TakeTool(GameObject player)
    {
        Debug.Log("taketool");
        rb.isKinematic = true;
        collider.isTrigger = true;
        collider.enabled = false;
        owner = player.GetComponent<Player>().playerID;
        transform.SetParent(player.transform);
        transform.SetLocalPositionAndRotation(new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        Map map = FindObjectOfType<Map>();
        center = map.getCenter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
