using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [SerializeField] private BaseBuilding[] buildings;

    [SerializeField] private Button backButton;

    [SerializeField] private BuildButton buildButtonPrefab;
    [SerializeField] private Transform buildPanelTransform;

    [SerializeField] private Button sellButton;
    [SerializeField] private Transform sellPanelTransform;
    [HideInInspector] public BaseBuildingPivot currentPivot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        foreach(BaseBuilding building in buildings)
        {
            BuildButton newButton = Instantiate(buildButtonPrefab, buildPanelTransform);
            newButton.GetComponent<Button>().onClick.AddListener(() => AddBuilding(building));
            newButton.SetTitle(building.name);
            newButton.SetPrice(building.BuildCost);
            newButton.SetIcon(building.UIIcon);
        }
    }

    public void AddBuilding(BaseBuilding building) 
    {
        if(GameManager.Instance.PlayerState.TryTakeFunds(building.BuildCost))
        {
            BaseBuilding newBuilding = Instantiate(building, currentPivot.transform);
            GameManager.Instance.BuildingCells.Add(currentPivot);
            currentPivot.SetBuilding(newBuilding);
        }

        DisplayBuildPanel(false);
    }

    public void RemoveBuilding(BaseBuilding building)
    {
        GameManager.Instance.PlayerState.AddFunds(building.SellPrice);
        GameManager.Instance.BuildingCells.Remove(currentPivot);

        currentPivot = building.GetComponentInParent<BaseBuildingPivot>();
        currentPivot.RemoveBuilding();
        currentPivot = null;
        DisplaySellPanel(false);
    }

    public void DisplayBuildPanel(bool display)
    {
        Vector3 offset = new Vector3(0, 2.5f, 0);
        buildPanelTransform.position = Camera.main.WorldToScreenPoint(currentPivot.transform.position + offset);
        buildPanelTransform.gameObject.SetActive(display);

        backButton.onClick.AddListener(HideBuildPanel);
        backButton.gameObject.SetActive(display);

    }

    public void HideBuildPanel()
    {
        buildPanelTransform.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        backButton.onClick.RemoveAllListeners();
    }

    public void DisplaySellPanel(bool display, BaseBuilding building = null)
    {

        sellPanelTransform.gameObject.SetActive(display);
        backButton.gameObject.SetActive(display);

        if(display)
        {
            if (building != null)
            {
                Vector3 offset = new Vector3(0, 2.5f, 0);
                currentPivot = building.GetComponentInParent<BaseBuildingPivot>();
                sellPanelTransform.position = Camera.main.WorldToScreenPoint(currentPivot.transform.position + offset);
                sellButton.onClick.AddListener(() => RemoveBuilding(building));
            }
        }
        else
        {
            sellButton.onClick.RemoveAllListeners();
        }

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => DisplaySellPanel(false));
    }

}
