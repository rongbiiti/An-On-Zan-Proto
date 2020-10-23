using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionCamera : MonoBehaviour
{
    public Material _executionShaderMaterial;
    public Transform _headjoint;
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

        yield return new WaitForSeconds(0.05f);

        Time.timeScale = 1f;
        executionFlg = false;

        // 死に様を見せるために位置変更
        _headjoint.localPosition = new Vector3(0, 2.9f, -2.6f);
        _headjoint.localRotation = Quaternion.Euler(31.72f, 0, 0);
    }
}
