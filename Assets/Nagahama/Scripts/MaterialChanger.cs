﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MaterialChanger : MonoBehaviourPunCallbacks
{

    [SerializeField] private Material material;
    [SerializeField] private Material _2pMaterial;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material swordMaterial;
    [SerializeField] private MeshRenderer swordMeshRenderer;
    [SerializeField] private float _materialOffWaitTime = 2f;
    [SerializeField] private bool _isCPU;

    private void Start()
    {
        if (PhotonNetwork.IsConnected && !_isCPU) {
            PhotonView photonview = GetComponent<PhotonView>();

            if (!photonview.IsMine) {

                if (!photonview.Owner.IsMasterClient) {
                    skinnedMeshRenderer.material = _2pMaterial;
                }
                StartCoroutine("MaterialOff");
            }
        } else {
            StartCoroutine("MaterialOff");
        }
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F4))
        {
            MaterialOn();
        }
    }

    public void Change2pMaterial()
    {
        if (!_isCPU && !photonView.Owner.IsMasterClient) {
            skinnedMeshRenderer.material = _2pMaterial;
        }
    }

    public void MaterialOn()
    {
        if (PhotonNetwork.IsConnected && !_isCPU && !photonView.Owner.IsMasterClient) {
            skinnedMeshRenderer.material = _2pMaterial;
        } else {
            skinnedMeshRenderer.material = material;
        }
        
        swordMeshRenderer.material = swordMaterial;
        skinnedMeshRenderer.enabled = true;
        swordMeshRenderer.enabled = true;
    }

    public IEnumerator MaterialOff()
    {
        yield return new WaitForSeconds(_materialOffWaitTime);
        skinnedMeshRenderer.enabled = false;
        swordMeshRenderer.enabled = false;
        yield return new WaitForSeconds(0.3f);
        GetComponent<AudioSource>().volume = 1;
    }

}
