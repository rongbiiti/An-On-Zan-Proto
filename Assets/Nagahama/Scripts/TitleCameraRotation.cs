using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraRotation : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 15f;
    [SerializeField] private float _startDelayTime = 3f;
    [SerializeField] private bool _rotateAroundFlg = false;
    private Transform myTransform;
    private bool rotateFlg;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
        StartCoroutine(nameof(DelayRotateStart));
    }

    private void LateUpdate()
    {
        if (!rotateFlg) return;
        if (_rotateAroundFlg) {
            transform.Rotate(0f, _rotateSpeed * Time.deltaTime, 0f);
        } else {
            transform.Rotate(new Vector3(0, _rotateSpeed, 0) * Time.deltaTime, Space.World);
        }
    }

    IEnumerator DelayRotateStart()
    {
        yield return new WaitForSeconds(_startDelayTime);
        rotateFlg = true;
    }
}
