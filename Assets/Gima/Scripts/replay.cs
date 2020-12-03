using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class replay : MonoBehaviour
{
    [SerializeField]
    string dirPath;     // 連番画像が保存されているディレクトリ

    // コマ表示キュー
    Queue<System.IO.FileInfo> fileQueue;
    public RenderTexture texture;
    // 表示用テクスチャ
    Texture2D tex;
    RawImage AnimImage;
    //フラグ
    public static bool PlayerDeathflg = false;
    private bool flg = false;
    private bool flg2 = true;
    private int cunt = 0;

    void FixedUpdate()
    {
        
        if (CameraScreenShot.replay && flg == false)
        {
            AnimImage = GetComponent<RawImage>();
            // ディレクトリ内のpngファイル一覧を取得
            //   -> キューに入れる
            if (0 == cunt % CameraScreenShot.set)
            {
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(dirPath);
                fileQueue = new Queue<System.IO.FileInfo>(info.GetFiles("*.png"));

                // テクスチャオブジェクトを作成しておく
                tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
                AnimImage.texture = Texture2D.blackTexture;
                flg = true;
            }
            
        }
        if (flg)
        {

            if (fileQueue.Count <= 0)
            {
                if (flg2)
                {
                    DeleteFile();
                }
                // 全部表示したので何もしない
                Debug.Log("何もないぞ");
                return;
            }

            flg2 = true;

            if (0 == cunt % CameraScreenShot.set)
            {
                if (!PlayerDeathflg)
                {
                    DeleteFile();
                }
                // ファイルからテクスチャデータ読み込み
                System.IO.FileInfo targetImage = fileQueue.Dequeue();
                System.IO.FileStream stream = targetImage.OpenRead();
                var data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                tex.LoadImage(data);
                // RawImageにテクスチャとして設定
                AnimImage.texture = tex;
                //Debug.Log("cunt % CameraScreenShot.set  :" + cunt % CameraScreenShot.set);
            }
            cunt++;
        }
    }

    public void DeleteFile()
    {
        //初期化処理
        //RawImageのクリア、ディレクトリの削除
        AnimImage.texture = texture;
        CameraScreenShot.replay = false;
        System.IO.Directory.Delete(dirPath, true);
        flg2 = false;
        flg = false;
        PlayerDeathflg = false;
        CameraScreenShot.CPUAttack = false;
        cunt = 0;
    }
}
