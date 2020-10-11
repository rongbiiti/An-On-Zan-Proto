using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MaterialChanger : MonoBehaviourPunCallbacks
{

    [SerializeField] private Material material;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material swordMaterial;
    [SerializeField] private MeshRenderer swordMeshRenderer;

    private void Start()
    {
        PhotonView photonview = GetComponent<PhotonView>();
        if (!photonview.IsMine) {
            StartCoroutine("MaterialOff");
        }
    }

    public void MaterialOn()
    {
        skinnedMeshRenderer.material = material;
        swordMeshRenderer.material = swordMaterial;
        skinnedMeshRenderer.enabled = true;
        swordMeshRenderer.enabled = true;
    }

    public IEnumerator MaterialOff()
    {
        yield return new WaitForSeconds(2f);
        skinnedMeshRenderer.enabled = false;
        swordMeshRenderer.enabled = false;
    }

}
