using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATPHeaderDisplay : MonoBehaviour
{
    public Text text;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "ATP: " + GameObject.FindObjectOfType<CellScript>().getGolgiCurrentEnergy().ToString();
    }
}
