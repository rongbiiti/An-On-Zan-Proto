using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProcess : MonoBehaviour
{

    [SerializeField]
    private BoxCollider weaponCollider;

    [SerializeField]
    private GameObject effect;

    private void Start()
    {
        effect.SetActive(false);
    }

    void AttackStart()
    {
        weaponCollider.enabled = true;
        Debug.Log("攻撃判定ON");
        effect.SetActive(true);
    }

    void AttackEnd()
    {
        weaponCollider.enabled = false;
        Debug.Log("攻撃判定OFF");
        effect.SetActive(false);
    }
}
