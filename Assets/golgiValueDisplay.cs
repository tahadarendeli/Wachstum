﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class golgiValueDisplay : MonoBehaviour
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

    }

    public void setText(int newValue)
    {
        text.text = newValue.ToString();
    }
}
