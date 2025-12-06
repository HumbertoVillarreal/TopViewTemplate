using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController_Dynamic : MonoBehaviour
{

    [Header("UI References")]
    public RectTransform mapParent;
    public GameObject areaPrefab;
    public RectTransform playerIcon;

    [Header("Colors")]
    public Color defaultColor = Color.gray;
    public Color currAreaColor = Color.green;

    [Header("Map Setting")]
    public GameObject mapBounds;
    public PolygonCollider2D initialArea;
    public float mapScale = 10f;

    private PolygonCollider2D[] mapAreas;
    private Dictionary<string, RectTransform> uiAreas = new Dictionary<string, RectTransform>();

    public static MapController_Dynamic Instance { get; set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mapAreas = mapBounds.GetComponentsInChildren<PolygonCollider2D>();
    }


    //Generate Map
    public void GenerateMap(PolygonCollider2D newCurrentArea = null)
    {
        PolygonCollider2D currArea = newCurrentArea != null ? newCurrentArea : initialArea;

        ClearMap();

        foreach(PolygonCollider2D area in mapAreas)
        {
            //Create Area UI
            CreateAreaUI(area, area == currArea);
        }

        //MovePlayerIcon
        MovePlayerIcon(currArea.name);
    }


    //Clear Map
    private void ClearMap()
    {
        foreach(Transform child in mapParent)
        {
            Destroy(child.gameObject);
        }

        uiAreas.Clear();
    }


    private void CreateAreaUI(PolygonCollider2D area, bool isCurrent)
    {
        //Instantiate prefab image
        GameObject areaImage = Instantiate(areaPrefab, mapParent);
        RectTransform rectTransform = areaImage.GetComponent<RectTransform>();

        //Get bounds
        Bounds bounds = area.bounds;

        //Scale UI image fit map bounds
        rectTransform.sizeDelta = new Vector2(bounds.size.x * mapScale, bounds.size.y * mapScale);
        rectTransform.anchoredPosition = bounds.center * mapScale;

        //Set color based on curr or not
        areaImage.GetComponent<Image>().color = isCurrent ? currAreaColor : defaultColor;

        //Add to dictionary
        uiAreas[area.name] = rectTransform;
    }


    //Move PlayerIcon
    private void MovePlayerIcon(string newCurrArea)
    {
        if (uiAreas.TryGetValue(newCurrArea, out RectTransform areaUI))
        {
            playerIcon.anchoredPosition = areaUI.anchoredPosition;
        }
    }


    public void UpdateCurrentArea(string newCurrArea)
    {
        //UpdateColor
         foreach(KeyValuePair<string, RectTransform> area in uiAreas)
        {
            area.Value.GetComponent<Image>().color = area.Key == newCurrArea ? currAreaColor : defaultColor;
        }

        //Move PlayerIcon
        MovePlayerIcon(newCurrArea);
    }

}
