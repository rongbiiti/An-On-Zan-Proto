﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{

    [SerializeField] private Material material;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material swordMaterial;
    [SerializeField] private MeshRenderer swordMeshRenderer;

    void Start()
    {
        skinnedMeshRenderer.enabled = false;
        swordMeshRenderer.enabled = false;
    }

    public void MaterialOn()
    {
        skinnedMeshRenderer.material = material;
        swordMeshRenderer.material = swordMaterial;
        skinnedMeshRenderer.enabled = true;
        swordMeshRenderer.enabled = true;
    }

}
