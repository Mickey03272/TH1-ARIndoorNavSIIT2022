using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField]
    private Slider navigationYOffset;

    private NavMeshPath path; //current calculated path
    private LineRenderer line; //linerenderer to display path
    private Vector3 targetPositon = Vector3.zero; //current target position

    private bool lineToggle = false;
    private int currentFloor;

    private void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
    }

    private void Update()
    {
        if(lineToggle && targetPositon != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPositon, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            Vector3[] calculatedPathAndOffset = AddLineOffset();
            line.SetPositions(calculatedPathAndOffset);
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        targetPositon = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null)
        {
            if (!line.enabled)
            {
                ToggleVisibility();
            }

            // Check if floor is changing
            // if yes, lead to elevator
            // if no, navigate

            targetPositon = currentTarget.PositionObject.transform.position;
        }
    }

    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
    }

    public void ChangeActiveFloor(int floorNumber)
    {
        currentFloor = floorNumber;
        SetNavigationTargetDropDownOptions(currentFloor);
    }

    private Vector3[] AddLineOffset()
    {
        if (navigationYOffset.value == 0)
        {
            return path.corners;
        }

        Vector3[] calculatedLine = new Vector3[path.corners.Length];
        for(int i = 0; i < path.corners.Length; i++)
        {
            calculatedLine[i] = path.corners[i] + new Vector3(0, navigationYOffset.value, 0);
        }
        return calculatedLine;
    }

    private void SetNavigationTargetDropDownOptions(int floorNumber)
    {
        navigationTargetDropDown.ClearOptions();
        navigationTargetDropDown.value = 0;

        if (line.enabled)
        {
            ToggleVisibility();
        }

        if (floorNumber == 1)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3201"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3202"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3206"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3207"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3214"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3215"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3216"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Computer Center Office"));
        }

        if (floorNumber == 2)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3506"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3507"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3509"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("BKD 3510"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("EC Lab"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("DC Lab"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("SP Lab"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("FO Lab"));
        }
    }
}
