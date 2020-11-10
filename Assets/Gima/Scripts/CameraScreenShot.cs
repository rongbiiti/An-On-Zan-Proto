using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraScreenShot : MonoBehaviour
{

    //カウントアップ
    private int countup = 0;
    //タイムリミット
    [SerializeField, Tooltip("何秒間撮り続けるか")]
    public float timeLimit = 5.0f;
    private bool flg = false;

    //フレームカウント
    private int cnt = 0;
    private int count = 0;

    //指定枚数
    [SerializeField, Tooltip("1秒間に何枚撮り続けるか")]
    public int SetNumber= 15;
    //1フレーム当たりの撮影フレーム数
    public static int set = 0;
    private float mCur;
    private float mLength;

    public static bool replay = false;
    private GameObject mainCamera;      //メインカメラ格納用
    private GameObject subCamera;       //サブカメラ格納用 
    [SerializeField] private Camera _camera;

    //ファイル名の指定
    [SerializeField, Tooltip("ファイル名の末尾に付く文字")]
    private string _imageTitle = "img";

    //保存先の指定 (末尾に / を付けてください)
    [SerializeField, Tooltip("ファイルの保存先 末尾の/ を含めてください")]
    private string _screenShotFolder = "ScreenShots/";

    void Start()
    {
        //メインカメラとサブカメラをそれぞれ取得
        mainCamera = GameObject.Find("MainCamera");
        subCamera = GameObject.Find("SubCamera");

        set = 60 / SetNumber;
        //サブカメラを非アクティブにする
        subCamera.SetActive(false);
    }


    void FixedUpdate()
    {

        //「P」で撮影
        if (Input.GetMouseButton(0))
        {
            flg = true;
        }
        
        if (flg)
        {
            if (countup < timeLimit * 60)
            {
                if(0　== countup % set) { 
                    string path = Application.dataPath + _screenShotFolder;
                    imageShooting(path, _imageTitle);
                }
            }
            else if (countup >= timeLimit * 60)
            {
                countup = 0;
                flg = false;
                replay = true;
                count = 0;
            }
            countup++;
        }
        count++;
    }

    //撮影処理
    //第一引数 ファイルパス / 第二引数 タイトル
    private void imageShooting(string path, string title)
    {
        //ファイルパスの確認
        imagePathCheck(path);
        //スクショのタイトル+拡張子
        string name = count + ".png";

        //カメラの比率 写ってるテクスチャの情報処理
        var rt = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 24);
        var prev = _camera.targetTexture;
        _camera.targetTexture = rt;
        _camera.Render();
        _camera.targetTexture = prev;
        RenderTexture.active = rt;

        var screenShot = new Texture2D(_camera.pixelWidth,
            _camera.pixelHeight,
            TextureFormat.RGB24,
            false);
        //スクリーン画面をテクスチャ情報へ変換(Pixelデータ)
        screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
        screenShot.Apply();

        //テクスチャをpngへエンコード
        var bytes = screenShot.EncodeToPNG();
        Destroy(screenShot);

        File.WriteAllBytes(path + name, bytes);
        return;
    }

    //ファイルパスの確認
    private void imagePathCheck(string path)
    {
        if (Directory.Exists(path))
        {
            //Debug.Log("The path exists");
        }
        else
        {
            //パスが存在しなければフォルダを作成
            Directory.CreateDirectory(path);
            //Debug.Log("CreateFolder: " + path);
        }
    }

}
