using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lets the user select this agent and updates the GUI with the agent details, as well as showing/hiding the selection sphere around the agent
[RequireComponent(typeof(Agent))]
public class SelectableAgent : MonoBehaviour
{
    [Header("The colours that the selectable sphere turns")]
    public Color selectedColour;
    public Color hoverColour;

    public GameObject selectableSphere;
    Material selectableSphereMaterial;
    public bool isSelected = false;
    public bool isHovered = false;
    public Agent agent;

    void Start()
    {
        agent = GetComponent<Agent>();
        selectableSphereMaterial = selectableSphere.GetComponent<MeshRenderer>().material;
    }

    public void Hover()
    {
        isHovered = true;
        if (isSelected) return;

        selectableSphere.SetActive(true);
        selectableSphereMaterial.SetColor("_MainColor", hoverColour);
    }

    public void Unhover()
    {
        isHovered = false;

        if (!isSelected) selectableSphere.SetActive(false);
    }

    public void Select()
    {
        selectableSphere.SetActive(true);
        selectableSphereMaterial.SetColor("_MainColor", selectedColour);
        isSelected = true;
    }

    public void Deselect()
    {
        selectableSphere.SetActive(false);
        isSelected = false;
    }

}
