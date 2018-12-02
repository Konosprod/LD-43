using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    public static TowerManager _instance;

    [Header("UI")]
    public GameObject towerShopPanel;
    public GameObject upgradeTowerPanel;

    private int layerTowerSpot;
    private int layerTower;
    private Dictionary<GameObject, GameObject> usedTowerSpots = new Dictionary<GameObject, GameObject>(); // TowerSpot => Tower logic
    public GameObject selectedSpot;
    public GameObject selectedTower;
    public Tower selectedTowerTower;

    void Awake()
    {
        if (TowerManager._instance == null)
            TowerManager._instance = this;
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start()
    {
        layerTowerSpot = LayerMask.NameToLayer("TowerSpot");
        layerTower = LayerMask.NameToLayer("Tower");
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // We use a raycast to find the tower spot when the player clicks
                RaycastHit rayHitTowerSpot;
                Ray rayPiece = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayPiece, out rayHitTowerSpot, Mathf.Infinity, 1 << layerTowerSpot))
                {
                    //Debug.Log("Click on tower spot : " + rayHitTowerSpot.collider.name);
                    if (!usedTowerSpots.ContainsKey(rayHitTowerSpot.collider.gameObject)) // If the spot isn't used we can put a tower
                    {
                        selectedSpot = rayHitTowerSpot.collider.gameObject;
                        towerShopPanel.SetActive(true);
                    }
                    else
                    {
                        // The spot is used, we can upgrade/delete the tower
                        selectedTower = usedTowerSpots[rayHitTowerSpot.collider.gameObject];
                        selectedTowerTower = selectedTower.GetComponent<Tower>();
                        selectedTowerTower.isSelectedMode = true;
                        upgradeTowerPanel.SetActive(true);
                    }
                }
                else if (Physics.Raycast(rayPiece, out rayHitTowerSpot, Mathf.Infinity, 1 << layerTower))
                {
                    // We can upgrade/delete the tower
                    selectedTower = rayHitTowerSpot.collider.gameObject;
                    selectedTowerTower = selectedTower.GetComponent<Tower>();
                    selectedTowerTower.isSelectedMode = true;
                    upgradeTowerPanel.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            selectedSpot = null;
            towerShopPanel.SetActive(false);
            DisableUpgradeTowerPanel();
        }
    }

    public void DisableUpgradeTowerPanel()
    {
        selectedTower = null;
        if (selectedTowerTower != null)
            selectedTowerTower.isSelectedMode = false;
        selectedTowerTower = null;
        upgradeTowerPanel.SetActive(false);
    }

    public void DeleteSelectedTower()
    {
        if(selectedTower != null)
        {
            GameObject key = null;
            foreach (KeyValuePair<GameObject, GameObject> pair in usedTowerSpots)
            {
                if (pair.Value == selectedTower)
                {
                    key = pair.Key;
                    break;
                }
            }
            
            if(key != null)
            {
                usedTowerSpots.Remove(key);
                Destroy(selectedTower);
            }
            else
            {
                Debug.LogError("Couldn't find the spot that goes with the tower : " + selectedTower.name + ", in position : " + selectedTower.transform.position);
            }
        }
    }

    public void AddUsedTowerSpot(GameObject tower)
    {
        usedTowerSpots.Add(selectedSpot, tower);
    }
}
