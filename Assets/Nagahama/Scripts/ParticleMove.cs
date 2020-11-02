using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, _speed * Time.deltaTime);
    }
}
