using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionCamera : MonoBehaviour
{
    public Material _executionShaderMaterial;
    private bool executionFlg = false;

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
        Time.timeScale = 0.1f;
        executionFlg = true;

        yield return new WaitForSeconds(0.07f);

        Time.timeScale = 1f;
        executionFlg = false;
    }
}
