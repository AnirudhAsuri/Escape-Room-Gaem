using System.Collections.Generic;
using UnityEngine;

public class ButtonRegistrar : MonoBehaviour
{
    public static Dictionary<string, GameObject> buttonMappings = new Dictionary<string, GameObject>();

    public string buttonName;
    public GameObject affectedObject;

    private void Start()
    {
        if (!buttonMappings.ContainsKey(buttonName))
        {
            buttonMappings.Add(buttonName, affectedObject);
        }
    }
}