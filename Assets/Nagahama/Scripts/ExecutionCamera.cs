using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutionCamera : MonoBehaviour
{
    public Material _executionShaderMaterial;
    public Transform _headjoint;
    public Animator animator;
    public LayerMask layerMask;
    private bool isCPUMatch = false;
    private PauseManager pauseManager;
    private bool executionFlg = false;
    public bool isRayCast;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) isCPUMatch = true;
        if (isCPUMatch) {
            pauseManager = GameObject.Find("Canvas").GetComponent<PauseManager>();
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (executionFlg) {
            Graphics.Blit(src, dest, _executionShaderMaterial);
        } else {
            Graphics.Blit(src, dest);
        }
    }

    private void LateUpdate()
    {
        if (!isRayCast) return;
        WallDisable();
    }

    public IEnumerator Execution()
    {
        if (isCPUMatch) {
            pauseManager.isCanPause = false;
        }
        Time.timeScale = 0.1f;
        executionFlg = true;
        

        yield return new WaitForSeconds(0.05f);

        Time.timeScale = 1f;
        executionFlg = false;
        if (isCPUMatch) {
            pauseManager.isCanPause = false;
        }

        if (animator.GetBool("Death")) {
            // 死に様を見せるために位置変更
            _headjoint.localPosition = new Vector3(0, 2.9f, -2.6f);
            _headjoint.localEulerAngles = new Vector3(40f, 0, 0);
            transform.localEulerAngles = Vector3.zero;
            isRayCast = true;            
        }
    }

    private void WallDisable()
    {
        // カメラの前にある邪魔な壁などをオフにする
        Vector3 startPos = _headjoint.position;
        Vector3 targetPos = _headjoint.forward;
        float maxDistance = 4f;
        Ray ray = new Ray(startPos, targetPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask) ) {
            hit.collider.gameObject.SetActive(false);
            Debug.Log("壁にヒット");
        }
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

        targetPos.x += 0.6f;
        ray = new Ray(startPos, targetPos);
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            hit.collider.gameObject.SetActive(false);
            Debug.Log("壁にヒット");
        }
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);

        targetPos.x -= 1.2f;
        ray = new Ray(startPos, targetPos);
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            hit.collider.gameObject.SetActive(false);
            Debug.Log("壁にヒット");
        }
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
    }
}