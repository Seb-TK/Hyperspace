using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject DropDownObj;
    
    public void CreateDropdown(Vector3 position, List<String> options)
    {
        GameObject dropDown = Instantiate(DropDownObj, position, Quaternion.identity);
        dropDown.GetComponent<Dropdown>().ClearOptions();
        dropDown.GetComponent<Dropdown>().AddOptions(options);
    }
}
