using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [Header("RequiredObject")]
    public GameObject Canvas;
    public ThirdPersonControl controller;
    [Range(0.0f, 1.0f)]
    public float itemNameDisplayAlpha = 0.8f;
    [Range(0.0f, 0.1f)]
    public float itemNameDisplaySpeed = 0.05f;
    [Range(0.0f, 10.0f)]
    public float tossItemForce = 5.00f;

    [HideInInspector]
    public bool itemNearby;
    public float itemDistanceCompareMax = 1000.0f;
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
        float distanceCompare = itemDistanceCompareMax;
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
            Canvas.GetComponent<CanvasProperties>().SetTextMesh(0, showItem.GetComponent<ItemProperties>().itemName);
            Canvas.GetComponent<CanvasProperties>().SetAlpha(itemNameDisplayAlpha, itemNameDisplaySpeed);
            Canvas.GetComponent<CanvasProperties>().SetTextColor(0, showItem.GetComponent<ItemProperties>().textColor);
            Canvas.GetComponent<CanvasProperties>().SetTextMesh(1, controller.PickupKey.ToString());
        }
    }

    private void CloseCanvas()
    {
        ItemProperties properties = showItem.GetComponent<ItemProperties>();
        if (properties != null)
        {
            Canvas.GetComponent<CanvasProperties>().SetAlpha(0.0f, itemNameDisplaySpeed);
        }
    }

    public void TakeItem()
    {
        //search the backweapon list and enable it
        GameObject newWeapon = controller.AttackScript.secondWeaponParent.Find(showItem.GetComponent<ItemProperties>().itemName).gameObject;
        newWeapon.SetActive(true);
        if (controller.AttackScript.secondWeapon != null)
        {
            //drop item
            controller.AttackScript.secondWeapon.gameObject.SetActive(false);
            ItemProperties newProperties = controller.AttackScript.secondWeapon.GetComponent<ItemProperties>();
            if (newProperties != null)
            {
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
                // toss it to a random direction with constant force apply
                Vector3 torque;
                torque.x = (Random.Range(0, 2) * 2 - 1) * tossItemForce;
                torque.y = Random.Range(4, 5);
                torque.z = (Random.Range(0, 2) * 2 - 1) * tossItemForce;
                showItem.GetComponent<Rigidbody>().AddForce(torque, ForceMode.Impulse);
            }
        }
        else
        {
            Destroy(showItem);
        }
        controller.AttackScript.secondWeapon = newWeapon.transform;
    }
}
