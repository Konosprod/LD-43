using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    public static TowerManager _instance;

    [Header("UI")]
    public GameObject towerShopPanel;

    private int layerTowerSpot;
    public List<GameObject> towers = new List<GameObject>();
    public GameObject selectedSpot;

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
                    selectedSpot = rayHitTowerSpot.collider.gameObject;
                    towerShopPanel.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedSpot = null;
            towerShopPanel.SetActive(false);
        }
    }
}
