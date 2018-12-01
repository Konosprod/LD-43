using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyTowerButton : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{

    private GameObject towerPrefab;
    private Tower tower;
    private Button button;
    private GameObject towerPreview;

    // Use this for initialization
    void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void SetTower(GameObject towerPref)
    {
        towerPrefab = towerPref;
        tower = towerPref.GetComponent<Tower>();
        //Debug.Log(towerPrefab.name);
        button.image.sprite = Resources.Load<Sprite>("Previews/" + towerPref.name + "Preview");
        button.GetComponentInChildren<Text>().text = towerPref.name + " : " + tower.price;
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = GameManager._instance.CanAfford(tower.price);
    }

    // Buys the related tower
    public void BuyTower()
    {
        Destroy(towerPreview);
        GameManager._instance.BuyTower(towerPrefab, tower);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Create/Enable the preview tower
        if(towerPreview == null)
        {
            Vector3 position = TowerManager._instance.selectedSpot.transform.position;
            position += new Vector3(0f, 0.275f, 0f);
            towerPreview = Instantiate(towerPrefab, position, Quaternion.identity);
        }
        else if(!towerPreview.activeSelf)
        {
            towerPreview.SetActive(true);
            Vector3 position = TowerManager._instance.selectedSpot.transform.position;
            position += new Vector3(0f, 0.275f, 0f);
            towerPreview.transform.position = position;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Disable the preview tower
        if(towerPreview != null && towerPreview.activeSelf)
        {
            towerPreview.SetActive(false);
        }
    }

    void OnDisable()
    {
        // Disable the preview tower
        if (towerPreview != null && towerPreview.activeSelf)
        {
            towerPreview.SetActive(false);
        }
    }
}
