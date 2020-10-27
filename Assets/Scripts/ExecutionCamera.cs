using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutionCamera : MonoBehaviour
{
    public Material _executionShaderMaterial;
    public Transform _headjoint;
    public Animator animator;
    private bool isCPUMatch = false;
    private PauseManager pauseManager;
    private bool executionFlg = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) isCPUMatch = true;
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
            pauseManager.isCanPause = true;
        }

        if (animator.GetBool("Death")) {
            // 死に様を見せるために位置変更
            _headjoint.localPosition = new Vector3(0, 2.9f, -2.6f);
            _headjoint.localRotation = Quaternion.Euler(31.72f, 0, 0);
        }
    }
}