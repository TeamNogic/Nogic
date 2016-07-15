using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

[System.Serializable]
public enum Sys_SceneState
{
    Start,
    Wait,
    CreatePlayer,
    CreatePlayer_Wait,
    VS,
    VS_Wait,
    ChangePlayer,
    ChangePlayer_Wait,
    CreateNode,
    CreateNode_Wait,
    Timeup,
    Timeup_Wait,
    Node_Close,
    Node_Close_Wait,
    CreateAttackName,
    AttackName_Show_Wait,
    AttackName_Hide,
    AttackName_Hide_Wait,
    MapAlpha_Hide,
    MapAlpha_Hide_Wait,
    Attack,
    Attack_Wait,
    MapAlpha_Show,
    MapAlpha_Show_Wait,
}

[System.Serializable]
public class Sys_PlayerScene
{
    public GameObject prefab;       //モデルデータ
    public Vector3 position;        //プレイヤーの出現座標
    public float rotationY;         //Y軸回転
    public Vector3 cameraStart;     //カメラの移動開始位置
    public Vector3 cameraEnd;       //カメラの移動終了位置
    public Vector3 thumbnailMove;   //サムネイルの移動量
}

[System.Serializable]
public class Sys_Scene : MonoBehaviour
{
    private float startWait;                            //最初の遅延

    private int showWait;                               //登場中のプレイヤー

    private List<Image> thumbnail = new List<Image>();  //サムネイルデータ
    private Image thumbnailSingle;                      //サムネイルデータ

    public Image timeup;                                //時間切れオブジェクト

    private Image nodeCore;                             //ノード選択コア
    private Image nodeName;                             //ノード名とコメント名を表示
    private int nodeNamePos;                            //表示位置

    private GameObject attackEngine;                    //攻撃生成オブジェクト
    private GameObject cameraMove;                      //移動演出オブジェクト

    private GameObject nodeEditor;                      //キャッチしたノードエディタ

    private Sys_SceneState sceneState;                  //シーンの状態

    public List<Sys_PlayerScene> player;                //プレイヤーデータ
    public List<GameObject> playerObject;               //プレイヤーのオブジェクト

    public string nodeEditorName;                       //ノードエディタ名

    public Image nodeNameBase;                          //ノード名とコメント名を表示するためのベース
    public Image nodeCoreBase;                          //ノード選択コア
    public Image thumbnailBase;                         //サムネイルコア
    public Image VSBase;                                //サムネイルコア
    public Image Change;                                //チェンジUI
    public Image timeupBase;                            //時間切れオブジェクト
    public Image perfectBase;                           //完璧終了オブジェクト

    public GameObject cameraMoveBase;                   //移動演出オブジェクト
    public GameObject attackEngineBase;                 //攻撃生成オブジェクト

    public AudioClip thumbnailMove;                     //サムネイル移動音
    public AudioClip changeSound;                       //チェンジ音

    void Start()
    {
        sceneState = 0;
        showWait = 0;

        startWait = 1.0f;

        nodeEditor = GameObject.Find(nodeEditorName);

        Change = Instantiate(Change, new Vector3(0.0f, 100.0f), Quaternion.identity) as Image;
        Change.transform.SetParent(nodeEditor.transform, false);
    }

    void Update()
    {
        //シーン分岐
        switch (sceneState)
        {
            case Sys_SceneState.Start:
                player[0].prefab = playerObject[Char_SelectData.player_1];
                player[1].prefab = playerObject[Char_SelectData.player_2];

                for (int i = 0; i < player.Count; ++i)
                {
                    player[i].prefab = Instantiate(player[i].prefab, player[i].position, Quaternion.Euler(0.0f, player[i].rotationY, 0.0f)) as GameObject;
                    Sys_Status.Player.Add(new Sys_PlayerData());
                    Sys_Status.Player[i].Weak = player[i].prefab.GetComponent<Obj_PlayerAsset>().weak;
                }

                ++sceneState;
                break;

            case Sys_SceneState.Wait:
                startWait -= Time.deltaTime;

                if (Input.anyKey) sceneState = Sys_SceneState.CreateNode;

                if (startWait <= 0.0f)
                {
                    startWait = 0.0f;
                    ++sceneState;
                }
                break;

            case Sys_SceneState.CreatePlayer: //最初のシーン
                //指定した分のプレイヤーを生成
                Sys_Sound.Play(player[showWait].prefab.GetComponent<Obj_PlayerAsset>().startSound);

                Sys_Camera.Setup(player[showWait].cameraStart, player[showWait].cameraEnd, player[showWait].position, 15.0f);

                player[showWait].prefab.GetComponent<Animator>().SetBool("Attack", true);

                thumbnail.Add(Instantiate(thumbnailBase, new Vector2(0.0f, -100.0f), Quaternion.identity) as Image);
                thumbnail[showWait].GetComponent<Image>().sprite = player[showWait].prefab.GetComponent<Obj_PlayerAsset>().thumbnail;
                thumbnail[showWait].transform.SetParent(nodeEditor.transform, false);

                //次のシーンへ
                ++sceneState;
                break;

            case Sys_SceneState.CreatePlayer_Wait: //プレイヤーが移動を完了するまで待機する
                //出現が完了していれば
                if (player[showWait].prefab.GetComponent<Obj_Scale>() == null && !Sys_Camera.isMove())
                {
                    startWait += Time.deltaTime;

                    if (1.0f <= startWait)
                    {
                        startWait = 0.0f;

                        //サムネイルを移動する
                        thumbnail[showWait].gameObject.AddComponent<UI_Move>();
                        thumbnail[showWait].gameObject.GetComponent<UI_Move>().Setup(player[showWait].thumbnailMove);
                        Sys_Sound.Play(thumbnailMove);

                        //次のシーンへ
                        if (player.Count <= ++showWait) ++sceneState;
                        else sceneState = Sys_SceneState.CreatePlayer;
                    }
                }

                break;

            case Sys_SceneState.VS: //
                Sys_Camera.Setup_Rotate(new Vector3(0.0f, 0.0f, 7.5f), 20.0f, 10.0f, 0.2f);
                thumbnail.Add(Instantiate(VSBase, new Vector2(0.0f, -100.0f), Quaternion.identity) as Image);
                thumbnail[showWait].transform.SetParent(nodeEditor.transform, false);

                startWait = 7.0f;

                ++sceneState;
                break;

            case Sys_SceneState.VS_Wait: //
                startWait -= Time.deltaTime;

                if (startWait <= 0.0f)
                {
                    for (int i = 0; i < thumbnail.Count; ++i)
                    {
                        thumbnail[i].gameObject.AddComponent<UI_Scale>();
                        thumbnail[i].gameObject.GetComponent<UI_Scale>().Setup(new Vector2(0.0f, 0.0f), 1.5f, true);
                    }

                    //カメラの回転移動を終了
                    Sys_Camera.StopMove();

                    //次のシーンへ
                    ++sceneState;
                }
                break;

            case Sys_SceneState.ChangePlayer: //
                Change.gameObject.AddComponent<UI_Scale>();
                Change.gameObject.GetComponent<UI_Scale>().Setup(new Vector2(6.8f, 2.4f), 1.5f, false);

                Sys_Sound.Play(changeSound);
                Sys_Sound.Play(player[Sys_Status.activePlayer].prefab.GetComponent<Obj_PlayerAsset>().nodeSound);

                Sys_Camera.Setup(player[Sys_Status.activePlayer].cameraStart, player[Sys_Status.activePlayer].cameraEnd, player[Sys_Status.activePlayer].position, 15.0f);

                thumbnailSingle = Instantiate(thumbnailBase, new Vector2(0.0f, -100.0f), Quaternion.identity) as Image;
                thumbnailSingle.GetComponent<Image>().sprite = player[Sys_Status.activePlayer].prefab.GetComponent<Obj_PlayerAsset>().thumbnail;
                thumbnailSingle.transform.SetParent(nodeEditor.transform, false);

                //状態異常
                Sys_PlayerData tmp = Sys_Status.Player[Sys_Status.activePlayer];
                if (--tmp.State_Tern_Time <= 0) tmp.State_Tern = 0;
                if (--tmp.State_Status_Time <= 0) tmp.State_Status = 0;
                if (--tmp.State_NodeKey_Time <= 0) tmp.State_NodeKey = 0;
                if (--tmp.State_NodeHindrance_Time <= 0) tmp.State_NodeHindrance = 0;
                if (--tmp.State_NodeEditor_Time <= 0) tmp.State_NodeEditor = 0;
                
                startWait = 4.0f;
                ++sceneState;
                break;

            case Sys_SceneState.ChangePlayer_Wait: //
                startWait -= Time.deltaTime;

                if (startWait <= 0.0f)
                {
                    startWait = 0.0f;

                    Change.gameObject.AddComponent<UI_Scale>();
                    Change.gameObject.GetComponent<UI_Scale>().Setup(new Vector2(6.8f, 0.0f), 1.5f, false);

                    thumbnailSingle.gameObject.AddComponent<UI_Scale>();
                    thumbnailSingle.gameObject.GetComponent<UI_Scale>().Setup(new Vector2(0.0f, 0.0f), 1.5f, true);

                    ++sceneState;
                }
                break;

            case Sys_SceneState.CreateNode: //ノード管理の生成
                nodeCore = Instantiate(nodeCoreBase, nodeCoreBase.transform.position, nodeCoreBase.transform.rotation) as Image;

                cameraMove = Instantiate(cameraMoveBase, cameraMoveBase.transform.position, cameraMoveBase.transform.rotation) as GameObject;
                cameraMove.GetComponent<Sys_Camera_Move>().lookAt = player[Sys_Status.activePlayer].position;

                ++sceneState;
                break;

            case Sys_SceneState.CreateNode_Wait: //待機
                //ノード選択完了で次のシーンへ
                if (!nodeCore.GetComponent<Sys_NodeEngine>().isUpdate())
                {
                    //カメラ移動の終了
                    Destroy(cameraMove);

                    ++sceneState;
                }
                break;

            case Sys_SceneState.Timeup: //タイムアップ
                timeup = Instantiate(nodeCore.GetComponent<Sys_NodeEngine>().isPerfect() ? perfectBase : timeupBase
                    , Vector2.zero, Quaternion.identity) as Image;

                timeup.transform.SetParent(nodeEditor.transform, false);

                ++sceneState;
                break;

            case Sys_SceneState.Timeup_Wait: //待機
                if (timeup == null) ++sceneState;
                break;

            case Sys_SceneState.Node_Close: //ノード画面を閉じる
                nodeCore.GetComponent<Sys_NodeEngine>().End();

                ++sceneState;
                break;

            case Sys_SceneState.Node_Close_Wait: //待機
                if (nodeCore == null)
                {
                    nodeNamePos = 0;
                    ++sceneState;
                }
                break;

            case Sys_SceneState.CreateAttackName:
                //名前とコメントの生成
                nodeName = Instantiate(nodeNameBase, Vector2.zero, Quaternion.identity) as Image;
                nodeName.transform.SetParent(nodeEditor.transform, false);

                nodeName.transform.FindChild("Name").gameObject.GetComponent<Text>().text = Sys_Status.Attack_Name[nodeNamePos];
                nodeName.transform.FindChild("Comment").gameObject.GetComponent<Text>().text = Sys_Status.Attack_Comment[nodeNamePos];

                ++sceneState;
                break;

            case Sys_SceneState.AttackName_Show_Wait:
                //拡大しきったらシーンの切り替え
                if (nodeName.GetComponent<UI_Scale>() == null) ++sceneState;
                break;

            case Sys_SceneState.AttackName_Hide:
                //縮小設定
                nodeName.gameObject.AddComponent<UI_Scale>();
                nodeName.GetComponent<UI_Scale>().Setup(Vector2.zero, 2.5f, true);
                ++sceneState;
                break;

            case Sys_SceneState.AttackName_Hide_Wait:
                //縮小しきって消滅したら
                if (nodeName == null)
                {
                    //まだノードが残っている場合は繰り返す　そうでない場合は次のシーンへ
                    if (Sys_Status.Attack_Name.Count <= ++nodeNamePos) ++sceneState;
                    else sceneState = Sys_SceneState.CreateAttackName;
                }
                //何かのキーが押されたら　強制的に次のシーンへ
                else if (Input.anyKey) ++sceneState;
                break;

            case Sys_SceneState.MapAlpha_Hide: //マップを透明
                GameObject.Find(Sys_Status.stageName).AddComponent<Obj_Alpha>();
                GameObject.Find(Sys_Status.stageName).GetComponent<Obj_Alpha>().Setup(0.0f, 1.0f, false);
                ++sceneState;
                break;

            case Sys_SceneState.MapAlpha_Hide_Wait: //透明になるまで待機
                //マップが完全に透明になったら
                if (GameObject.Find(Sys_Status.stageName).GetComponent<Obj_Alpha>() == null)
                {
                    //カメラを戻す
                    Sys_Camera.Setup(Sys_Camera.getPosition(), new Vector3(0.0f, 25.0f, -30.0f), player[Sys_Status.targetPlayer].position, 30.0f);

                    ++sceneState;
                }
                break;

            case Sys_SceneState.Attack: //攻撃開始
                //攻撃エンジン生成
                if (attackEngine != null) Destroy(attackEngine);

                attackEngine = Instantiate(attackEngineBase, attackEngineBase.transform.position, attackEngineBase.transform.rotation) as GameObject;
                attackEngine.GetComponent<Sys_Instance>().targetPosition = player[Sys_Status.targetPlayer].prefab.transform.position;

                Sys_Sound.Play(player[Sys_Status.activePlayer].prefab.GetComponent<Obj_PlayerAsset>().attackSound);

                player[Sys_Status.activePlayer].prefab.GetComponent<Animator>().SetBool("Attack", true);

                //次のシーンへ
                ++sceneState;
                break;

            case Sys_SceneState.Attack_Wait: //攻撃終了まで待機
                //次のシーンへ
                if (attackEngine == null) ++sceneState;
                //ダメージサウンド
                else if (attackEngine.GetComponent<Sys_Instance>().isEnd)
                    Sys_Sound.Play(player[Sys_Status.targetPlayer].prefab.GetComponent<Obj_PlayerAsset>().damageSound);

                break;

            case Sys_SceneState.MapAlpha_Show: //マップを透明
                GameObject.Find(Sys_Status.stageName).AddComponent<Obj_Alpha>();
                GameObject.Find(Sys_Status.stageName).GetComponent<Obj_Alpha>().Setup(1.0f, 1.0f, false);
                ++sceneState;
                break;

            case Sys_SceneState.MapAlpha_Show_Wait: //透明になるまで待機
                //マップが完全に透明になったら
                if (GameObject.Find(Sys_Status.stageName).GetComponent<Obj_Alpha>() == null)
                {
                    startWait = 4.0f;

                    Sys_Status.activePlayer = (Sys_Status.activePlayer + 1) % player.Count;
                    Sys_Status.targetPlayer = (Sys_Status.targetPlayer + 1) % player.Count;

                    sceneState = Sys_SceneState.ChangePlayer;
                }
                break;

            default:
                Debug.Log("Sys_Scene -> StateError");
                break;
        }
    }
}
