using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]

public class PlayerDeathProcess : MonoBehaviour
{
<<<<<<< HEAD
    private Animator animator;
    private NavMeshAgent meshAgent;
    private Rigidbody rb;
    private CapsuleCollider capsule;
    private EnemyMove enemyMove;
    private FirstPersonAIO firstPersonAIO;
    private FPSMove fPSMove;
    private PauseManager _pauseManager;
    private MaterialChanger materialChanger;
    private AttackProcess attackProcess;

    [SerializeField] private AudioSource _breathSource;
    [SerializeField] private GameObject _katana;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform[] meshs;
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private List<AudioClip> _deathVoiceClip = null;

    private AudioSource rootAudioSource;
=======
    Animator animator;
    NavMeshAgent meshAgent;
    Rigidbody rb;
    CapsuleCollider capsule;
    EnemyMove enemyMove;
    FirstPersonAIO firstPersonAIO;
    FPSMove fPSMove;
    PauseManager _pauseManager;
    [SerializeField] AudioSource _breathSource;
    [SerializeField] BoxCollider _swordCollider;
    [SerializeField] GameObject _camera;
    [SerializeField] Transform[] meshs;
    [SerializeField] GameObject _bloodEffect;
    [SerializeField] public List<AudioClip> _deathVoiceClip = null;
    AudioSource rootAudioSource;
>>>>>>> parent of 5201b18 (刀のコライダーを正確にした。キャラクターのコライダーを小さくした。これによりファイナル判定になりづらくなった。試合開始時光が消えたとき、相手の色が一瞬で消えるのではなく1秒かけてJOJOに消えるように変更した。ネット対戦時のデバッグがまだ)
    public GameObject reflectionprobe;

    void Start()
    {
        animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        enemyMove = GetComponent<EnemyMove>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        firstPersonAIO = GetComponent<FirstPersonAIO>();
        fPSMove = GetComponent<FPSMove>();
        rootAudioSource = GetComponent<AudioSource>();
        materialChanger = GetComponent<MaterialChanger>();
        attackProcess = GetComponent<AttackProcess>();
        _pauseManager = GameObject.Find("Canvas").GetComponent<PauseManager>();
        reflectionprobe = GameObject.Find("Reflection Probe");

        MeshtoOne();
    }

    public void MeshtoOne()
    {
        foreach (Transform mesh in meshs) {
            mesh.localScale = Vector3.one;
        }
    }

    public void MeshtoZero()
    {
        foreach (Transform mesh in meshs) {
            mesh.localScale = Vector3.zero;
        }
    }

    // CPU用
    public void KillPlayer()
    {
<<<<<<< HEAD
        animator.SetBool("Death", true);    // 死亡アニメーション
        enemyMove.enabled = false;          // CPUの制御スクリプト停止
        materialChanger.MaterialOn();       // 透明化解除
        attackProcess.EffectStop();         // 攻撃エフェクトオフ
        attackProcess.AttackEnd();          // 攻撃判定オフにさせる

        foreach (var wpcol in weaponColliders) {
            wpcol.enabled = false;          // 刀のコライダーオフ
        }

        _breathSource.enabled = false;      // 吐息オフ
        gameObject.tag = "Untagged";        // タグを変えて刀への当たり判定なくす
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");  // レイヤーを変えて刀への当たり判定なくす
        InstantiateBloodEffect();           // 出血エフェクト出す

=======
        meshAgent.enabled = false;
        enemyMove.enabled = false;
        _swordCollider.enabled = false;
        _breathSource.enabled = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
        InstantiateBloodEffect();
>>>>>>> parent of 5201b18 (刀のコライダーを正確にした。キャラクターのコライダーを小さくした。これによりファイナル判定になりづらくなった。試合開始時光が消えたとき、相手の色が一瞬で消えるのではなく1秒かけてJOJOに消えるように変更した。ネット対戦時のデバッグがまだ)
        ZanAnimation.flg = true;
        replay.PlayerDeathflg = true;
        _pauseManager.isCanPause = false;

        StartCoroutine("PlayDeathVoice");   // 死亡音声うわっ！再生
        reflectionprobe.SetActive(true);
    }

    // プレイヤー用
    public void KillPlayer_Net()
    {
        animator.SetBool("Death", true);    // 死亡アニメーション
        MeshtoOne();                        // 頭と足を生やす
        firstPersonAIO.enabled = false;     // 操作受付スクリプト停止
        fPSMove.enabled = false;            // 攻撃操作受付スクリプト停止
        materialChanger.MaterialOn();       // 透明化解除
        attackProcess.EffectStop();         // 攻撃エフェクトオフ
        attackProcess.AttackEnd();          // 攻撃判定オフにさせる

        foreach (var wpcol in weaponColliders) {
            wpcol.enabled = false;          // 刀のコライダーオフ
        }
        
        _breathSource.enabled = false;      // 吐息オフ
        gameObject.tag = "Untagged";        // タグを変えて刀への当たり判定なくす
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");  // レイヤーを変えて刀への当たり判定なくす

        rb.velocity = Vector3.zero;         // RigidBodyの運動量オフ
        InstantiateBloodEffect();           // 出血エフェクト出す

        ZanAnimation.flg = true;
        replay.PlayerDeathflg = true;
        _pauseManager.isCanPause = false;
<<<<<<< HEAD

        StartCoroutine("PlayDeathVoice");   // 死亡音声うわっ！再生
=======
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
        _swordCollider.enabled = false;
        rb.velocity = Vector3.zero;
        InstantiateBloodEffect();
        StartCoroutine("PlayDeathVoice");
>>>>>>> parent of 5201b18 (刀のコライダーを正確にした。キャラクターのコライダーを小さくした。これによりファイナル判定になりづらくなった。試合開始時光が消えたとき、相手の色が一瞬で消えるのではなく1秒かけてJOJOに消えるように変更した。ネット対戦時のデバッグがまだ)
        reflectionprobe.SetActive(true);

        // 斬ったときの反転エフェクト出す
        // 太陽光ON
        if (_camera.activeSelf) {
            _camera.GetComponent<ExecutionCamera>().StartCoroutine("Execution");
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
        
    }

    public void InstantiateBloodEffect()
    {
        //_bloodEffect.SetActive(true);
    }

    private IEnumerator PlayDeathVoice()
    {
        // 死亡音声を登録されているものの中からランダムに再生
        yield return new WaitForSeconds(0.17f);
        int n = Random.Range(0, _deathVoiceClip.Count);
        if (_deathVoiceClip.Any() && _deathVoiceClip[n] != null) {
            rootAudioSource.PlayOneShot(_deathVoiceClip[n]);
        }
        
    }
}