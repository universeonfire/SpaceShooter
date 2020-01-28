﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.2f;

    private Material myMaterial;
    private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
