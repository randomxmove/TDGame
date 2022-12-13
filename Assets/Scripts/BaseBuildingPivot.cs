using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBuildingPivot : MonoBehaviour
{
    [SerializeField] private GameObject _placementIndicator = null;

    public BaseBuilding CurrentBuilding { get; private set; }

    public Quaternion Rotation => transform.rotation;
    public Vector3 WorldPosition => transform.position;

    private bool isSelected = false;

    public void SetBuilding(BaseBuilding building)
    {
        if (CurrentBuilding != null)
        {
            Debug.LogError("Current building already set, cannot add another building to pivot", gameObject);
            return;
        }
        _placementIndicator.SetActive(false);
        CurrentBuilding = building;
        CurrentBuilding.SetParentPivot(transform);
        CurrentBuilding.OnPlacement();

        isSelected = false;
    }

    public void RemoveBuilding()
    {
        if (CurrentBuilding == null)
        {
            Debug.LogError("There is no current building to remove on this pivot", gameObject);
            return;
        }
        var building = CurrentBuilding;
        CurrentBuilding = null;
        building.OnRemoval();

        isSelected = false;
    }

    private void OnDestroy()
    {
        CurrentBuilding = null;
    }

    private void OnMouseEnter()
    {
        if (CurrentBuilding != null) return;
        _placementIndicator.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (CurrentBuilding != null) return;
        _placementIndicator.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (CurrentBuilding != null) return;

        BuildManager.Instance.currentPivot = this;
        BuildManager.Instance.DisplayBuildPanel(true);

        _placementIndicator.SetActive(true);
        isSelected = true;
    }

}
