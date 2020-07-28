using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [Header("RequiredObject")]
    public GameObject Canvas;
    public ThirdPersonControl controller;

    [HideInInspector]
    public bool itemNearby;
    List<GameObject> nearbyItemList = new List<GameObject>();
    GameObject showItem;

    public void ItemRegister(GameObject register)
    {
        nearbyItemList.Add(register);
        itemNearby = true;
        if (nearbyItemList.Count >= 2)
        {
            enabled = true;
        }
        else
        {
            PickingItem(register);
        }
    }

    public void ItemUnregister(GameObject register)
    {
        nearbyItemList.Remove(register);
        if (nearbyItemList.Count == 0)
        {
            enabled = false;
            nearbyItemList.Clear();
            CloseCanvas();
        }
        else if (nearbyItemList.Count == 1)
        {
            enabled = false;
            PickingItem(nearbyItemList[nearbyItemList.Count-1]);
        }
    }

    private void Start()
    {
        //disable the script by default
        enabled = false;
    }

    private void Update()
    {
        //the update function should only called when there are multiple item in the list
        float distanceCompare = 1000f;
        foreach (GameObject item in nearbyItemList)
        {
            //get the nearest item
            float newDistance = Vector3.Distance(item.transform.position, transform.position);
            if (newDistance <= distanceCompare)
            {
                distanceCompare = newDistance;
                PickingItem(item);
            }
        }
    }

    public bool PickUpAvailable()
    {
        // check if there is an item available to pickup from
        return (nearbyItemList.Count > 0);
    }

    private void PickingItem(GameObject item)
    {
        showItem = item;
        Canvas.SetActive(showItem != null);

        ItemProperties properties = showItem.GetComponent<ItemProperties>();
        if (properties != null)
        {
            Canvas.GetComponent<CanvasTransparency>().SetTextMesh(showItem.GetComponent<ItemProperties>().itemName);
            Canvas.GetComponent<CanvasTransparency>().SetAlpha(0.8f, 0.05f);
            Canvas.GetComponent<CanvasTransparency>().SetTextColor(showItem.GetComponent<ItemProperties>().textColor);
            Canvas.GetComponent<CanvasTransparency>().SetKeyInputTextMesh(controller.PickupKey.ToString());
        }
    }

    private void CloseCanvas()
    {
        ItemProperties properties = showItem.GetComponent<ItemProperties>();
        if (properties != null)
        {
            Canvas.GetComponent<CanvasTransparency>().SetAlpha(0.0f, 0.05f);
        }
    }

    public void TakeItem()
    {
        //search the backweapon list
        GameObject newWeapon = controller.AttackScript.secondWeaponParent.Find(showItem.GetComponent<ItemProperties>().itemName).gameObject;
        newWeapon.SetActive(true);
        if (controller.AttackScript.secondWeapon != null)
        {
            //drop item
            controller.AttackScript.secondWeapon.gameObject.SetActive(false);
            ItemProperties newProperties = controller.AttackScript.secondWeapon.GetComponent<ItemProperties>();
            if (newProperties != null)
            {
                Debug.Log("pick up " + newProperties.itemName);
                ItemProperties properties = showItem.GetComponent<ItemProperties>();
                // apply new properties to the item
                properties.itemName = newProperties.itemName;
                properties.visualMesh = newProperties.visualMesh;
                properties.textColor = newProperties.textColor;
                properties.visualScale = newProperties.visualScale;
                properties.turnRate = newProperties.turnRate;
                properties.YAxis = newProperties.YAxis;
                properties.initiateRotate = newProperties.initiateRotate;
                // apply physically
                properties.ResetItem();
                // apply to the UI
                PickingItem(showItem);
                // reset tranform
                showItem.transform.position = new Vector3(transform.position.x, 
                    controller.AttackScript.secondWeaponParent.transform.position.y + 0.5f, 
                    transform.position.z);
                // toss it to with a random force
                Vector3 torque;
                float range = 5f;
                torque.x = (Random.Range(0, 2) * 2 - 1) * range;
                torque.y = Random.Range(4, 5);
                torque.z = (Random.Range(0, 2) * 2 - 1) * range;
                showItem.GetComponent<Rigidbody>().AddForce(torque, ForceMode.Impulse);
                Debug.Log(torque.x + "," + torque.z);
            }
        }
        else
        {
            Destroy(showItem);
        }
        controller.AttackScript.secondWeapon = newWeapon.transform;
    }
}
