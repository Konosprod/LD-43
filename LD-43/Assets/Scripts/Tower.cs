using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats")]
    public int price = 30;
    public float range = 3f;
    public float damage = 5f;
    public float fireTime = 0.5f; // In seconds
    public int level = 1;

    [Header("Preview")]
    public bool isPreviewMode = true;
    public GameObject previewCirclePrefab;
    private GameObject previewCircleInst;
    private MeshRenderer previewCircleMeshRend;
    [HideInInspector]
    public bool isSelectedMode = false;

    // Use this for initialization
    void Start()
    {

    }

    public int GetUpgradeCost()
    {
        return price / 2 * (int)Mathf.Ceil(Mathf.Pow(1.5f, level-1));
    }

    public int GetSellValue()
    {
        int value = price / 2;

        for(int i=1; i<level; i++)
        {
            value += price / 4 * (int)Mathf.Ceil(Mathf.Pow(1.5f, i - 1));
        }

        return value;
    }

    // Update is called once per frame
    void Update()
    {
        if (previewCircleInst == null)
        {
            Vector3 position = transform.position;
            position += new Vector3(0f, 0f, 0f);
            previewCircleInst = Instantiate(previewCirclePrefab, position, Quaternion.identity);
            previewCircleInst.transform.localScale += new Vector3(range - 1f, 0f, range - 1f);
            previewCircleInst.transform.parent = transform;

            previewCircleMeshRend = previewCircleInst.GetComponent<MeshRenderer>();
        }
        else
        {
            previewCircleMeshRend.enabled = isPreviewMode || isSelectedMode;
        }
    }
}
