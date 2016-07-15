using UnityEngine;
using System.Collections;

public class Sys_NodeCreate : MonoBehaviour
{
    static public Sys_ParameterNode AddNode(string name, int rank, Sys_NodeGroup group, int option, string comment)
    {
        Sys_ParameterNode addNode = new Sys_ParameterNode();

        addNode.Name = name;
        addNode.Comment = comment;
        addNode.Rank = rank;
        addNode.Group = group;

        switch (group)
        {
            case Sys_NodeGroup.Core:
                addNode.GroupNext.Add(Sys_NodeGroup.Type);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Type:
                addNode.GroupPrev = Sys_NodeGroup.Core;

                addNode.GroupNext.Add(Sys_NodeGroup.Shape);
                addNode.GroupNext.Add(Sys_NodeGroup.TypePlus);
                addNode.GroupNext.Add(Sys_NodeGroup.TypeCritical);
                addNode.GroupNext.Add(Sys_NodeGroup.Critical);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Shape:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.GroupNext.Add(Sys_NodeGroup.Scale);
                addNode.GroupNext.Add(Sys_NodeGroup.Tern);
                addNode.GroupNext.Add(Sys_NodeGroup.Status);
                addNode.GroupNext.Add(Sys_NodeGroup.NodeKey);
                addNode.GroupNext.Add(Sys_NodeGroup.NodeHindrance);
                addNode.GroupNext.Add(Sys_NodeGroup.NodeEditor);
                addNode.GroupNext.Add(Sys_NodeGroup.Number);

                addNode.Angle = 30;
                break;

            case Sys_NodeGroup.TypePlus:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.Angle = 60;
                break;

            case Sys_NodeGroup.TypeCritical:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.Angle = 90;
                break;

            case Sys_NodeGroup.Critical:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.GroupNext.Add(Sys_NodeGroup.CriticalOption);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.CriticalOption:
                addNode.GroupPrev = Sys_NodeGroup.Critical;

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Tern:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.GroupNext.Add(Sys_NodeGroup.TernTime);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.TernTime:
                addNode.GroupPrev = Sys_NodeGroup.Tern;

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Scale:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 15;
                break;

            case Sys_NodeGroup.Status:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 30;
                break;

            case Sys_NodeGroup.NodeKey:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 45;
                break;

            case Sys_NodeGroup.NodeHindrance:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.GroupNext.Add(Sys_NodeGroup.NodeHindranceTime);

                addNode.Angle = 90;
                break;

            case Sys_NodeGroup.NodeHindranceTime:
                addNode.GroupPrev = Sys_NodeGroup.NodeHindrance;

                addNode.Angle = 90;
                break;

            case Sys_NodeGroup.NodeEditor:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 75;
                break;

            case Sys_NodeGroup.Number:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.GroupNext.Add(Sys_NodeGroup.Move);
                addNode.GroupNext.Add(Sys_NodeGroup.NumberScale);
                addNode.GroupNext.Add(Sys_NodeGroup.Speed);

                addNode.Angle = 60;
                break;

            case Sys_NodeGroup.Move:
                addNode.GroupPrev = Sys_NodeGroup.Number;

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.NumberScale:
                addNode.GroupPrev = Sys_NodeGroup.Number;

                addNode.Angle = 45;
                break;

            case Sys_NodeGroup.Speed:
                addNode.GroupPrev = Sys_NodeGroup.Number;

                addNode.Angle = 90;
                break;
        }

        addNode.Option = option;

        addNode.This = null;

        if (Sys_Node.NodeList.Count != 0) addNode.SelectStart = Sys_Node.NodeList[Sys_Node.NodeList.Count - 1].SelectEnd + 1;
        else addNode.SelectStart = 0;

        addNode.SelectEnd = addNode.SelectStart + 200 / (int)Mathf.Pow(2.0f, (float)rank);

        addNode.Used = true;
        addNode.Penalty = false;

        Sys_Node.NodeList.Add(addNode);
        return addNode;
    }
    
    void Start()
    {
        AddNode("ファイヤー", 1, Sys_NodeGroup.Type, 0, "火属性");
        AddNode("ダークネス", 1, Sys_NodeGroup.Type, 0, "闇属性");
        AddNode("フィジックス", 1, Sys_NodeGroup.Type, 0, "無属性");

        AddNode("ウォーター", 1, Sys_NodeGroup.Type, 0, "水属性");
        AddNode("アイス", 1, Sys_NodeGroup.Type, 0, "氷属性");
        AddNode("ゴッド", 1, Sys_NodeGroup.Type, 0, "神属性");

        AddNode("グラス", 1, Sys_NodeGroup.Type, 0, "草属性");
        AddNode("ウィンド", 1, Sys_NodeGroup.Type, 0, "風属性");
        AddNode("ソイル", 1, Sys_NodeGroup.Type, 0, "土属性");

        AddNode("サンダー", 1, Sys_NodeGroup.Type, 0, "雷属性");
        AddNode("ライト", 1, Sys_NodeGroup.Type, 0, "光属性");
        AddNode("ドラゴン", 1, Sys_NodeGroup.Type, 0, "竜属性");
        
        //--------------------------------------------------------------------------------------------------------------------------------//

        AddNode("ブースト", 4, Sys_NodeGroup.TypePlus, 2, "ダメージ２倍");
        AddNode("バースト", 5, Sys_NodeGroup.TypePlus, 3, "ダメージ３倍");

        AddNode("スター", 3, Sys_NodeGroup.TypeCritical, 1, "弱点属性ダメージ２倍");
        AddNode("ギャラクシー", 5, Sys_NodeGroup.TypeCritical, 2, "弱点属性ダメージ４倍");
        AddNode("グレート", 3, Sys_NodeGroup.TypeCritical, 3, "弱点以外の属性ダメージ２倍");
        AddNode("マーベラス", 5, Sys_NodeGroup.TypeCritical, 4, "弱点以外の属性ダメージ４倍");

        AddNode("ワールド", 2, Sys_NodeGroup.Critical, 2, "クリティカルダメージ２倍");
        AddNode("ノヴァ", 3, Sys_NodeGroup.Critical, 4, "クリティカルダメージ４倍");
        AddNode("ヘヴン", 4, Sys_NodeGroup.Critical, 8, "クリティカルダメージ８倍");

        AddNode("ナイフ", 1, Sys_NodeGroup.Shape, 5, "ダメージボーナス＋５％");
        AddNode("カッター", 1, Sys_NodeGroup.Shape, 10, "ダメージボーナス＋１０％");
        AddNode("ソード", 1, Sys_NodeGroup.Shape, 15, "ダメージボーナス＋１５％");
        AddNode("ニードル", 1, Sys_NodeGroup.Shape, 20, "ダメージボーナス＋２０％");
        AddNode("メイス", 1, Sys_NodeGroup.Shape, 25, "ダメージボーナス＋２５％");
        AddNode("ハンマー", 2, Sys_NodeGroup.Shape, 30, "ダメージボーナス＋３０％");
        AddNode("シックル", 2, Sys_NodeGroup.Shape, 35, "ダメージボーナス＋３５％");
        AddNode("アロー", 2, Sys_NodeGroup.Shape, 40, "ダメージボーナス＋４０％");
        AddNode("ランス", 2, Sys_NodeGroup.Shape, 45, "ダメージボーナス＋４５％");
        AddNode("スピアー", 2, Sys_NodeGroup.Shape, 50, "ダメージボーナス＋５０％");
        AddNode("アックス", 3, Sys_NodeGroup.Shape, 55, "ダメージボーナス＋５５％");
        AddNode("バルディッシュ", 3, Sys_NodeGroup.Shape, 60, "ダメージボーナス＋６０％");
        AddNode("バレット", 3, Sys_NodeGroup.Shape, 65, "ダメージボーナス＋６５％");
        AddNode("スローイングスター", 3, Sys_NodeGroup.Shape, 70, "ダメージボーナス＋７０％");
        AddNode("ハルバード", 3, Sys_NodeGroup.Shape, 75, "ダメージボーナス＋７５％");
        AddNode("フラッシュ", 4, Sys_NodeGroup.Shape, 80, "ダメージボーナス＋８０％");
        AddNode("ソニック", 4, Sys_NodeGroup.Shape, 85, "ダメージボーナス＋８５％");
        AddNode("レーザー", 4, Sys_NodeGroup.Shape, 90, "ダメージボーナス＋９０％");
        AddNode("ボム", 4, Sys_NodeGroup.Shape, 95, "ダメージボーナス＋９５％");
        AddNode("メテオ", 5, Sys_NodeGroup.Shape, 100, "ダメージボーナス＋１００％");

        //--------------------------------------------------------------------------------------------------------------------------------//

        AddNode("ファルコン", 2, Sys_NodeGroup.CriticalOption, 25, "クリティカル率プラス１５％");
        AddNode("フェニックス", 3, Sys_NodeGroup.CriticalOption, 50, "クリティカル率プラス４０％");
        AddNode("リーパー", 4, Sys_NodeGroup.CriticalOption, 75, "クリティカル率プラス６５％");
        AddNode("ゼウス", 5, Sys_NodeGroup.CriticalOption, 100, "必ずクリティカルヒット");

        AddNode("スーパー", 1, Sys_NodeGroup.Scale, 2, "更にダメージ２倍");
        AddNode("ハイパー", 3, Sys_NodeGroup.Scale, 4, "更にダメージ４倍");
        AddNode("アルティメット", 5, Sys_NodeGroup.Scale, 8, "更にダメージ８倍");

        AddNode("ポイズン", 2, Sys_NodeGroup.Tern, 1, "ターンごとに定期ダメージ");
        AddNode("パラサイト", 2, Sys_NodeGroup.Tern, 2, "ターンごとに定期ダメージ");

        AddNode("ウィーク", 5, Sys_NodeGroup.Status, 1, "全てが弱点");
        AddNode("ダウン", 4, Sys_NodeGroup.Status, 2, "弱点ダメージボーナス半減");
        AddNode("アブソープション", 5, Sys_NodeGroup.Status, 3, "弱点ダメージボーナス無効");

        AddNode("リバース", 3, Sys_NodeGroup.NodeKey, 1, "ノード選択のキーが反転する");
        AddNode("カオス", 4, Sys_NodeGroup.NodeKey, 2, "ノード選択のキーが時々移動する");
        AddNode("フリーズ", 2, Sys_NodeGroup.NodeKey, 3, "ノード選択の一部のキーが封印される");
        AddNode("ホールド", 5, Sys_NodeGroup.NodeKey, 4, "ノード選択でキーを長押しする手間が増える");

        AddNode("スモーク", 4, Sys_NodeGroup.NodeHindrance, 1, "ノード選択画面を曇らせて妨害");
        AddNode("フェスティバル", 3, Sys_NodeGroup.NodeHindrance, 2, "ノード選択画面を音符マークで妨害");

        AddNode("シールド", 3, Sys_NodeGroup.NodeEditor, 1, "サポートノード禁止");
        AddNode("ヘル", 3, Sys_NodeGroup.NodeEditor, 2, "ペナルティノードの出現率が上昇");
        AddNode("ブレイク", 5, Sys_NodeGroup.NodeEditor, 3, "ノード選択時間が減る");

        AddNode("エクストラ", 1, Sys_NodeGroup.Number, 2, "攻撃個数ボーナス×２");
        AddNode("デラックス", 2, Sys_NodeGroup.Number, 3, "攻撃個数ボーナス×３");
        AddNode("グランド", 3, Sys_NodeGroup.Number, 4, "攻撃個数ボーナス×４");
        AddNode("リッチ", 5, Sys_NodeGroup.Number, 5, "攻撃個数ボーナス×５");

        //--------------------------------------------------------------------------------------------------------------------------------//

        AddNode("フォール", 1, Sys_NodeGroup.Move, 5, "落下移動　ダメージボーナス＋５％");
        AddNode("ショット", 2, Sys_NodeGroup.Move, 10, "発射　ダメージボーナス＋１０％");
        AddNode("グラウンド", 3, Sys_NodeGroup.Move, 20, "地面から　ダメージボーナス＋２０％");
        AddNode("ロックオン", 5, Sys_NodeGroup.Move, 40, "様々な方向から　ダメージボーナス＋４０％");

        AddNode("ショート", 1, Sys_NodeGroup.TernTime, 2, "効果継続２ターン");
        AddNode("ロング", 3, Sys_NodeGroup.TernTime, 3, "効果継続３ターン");
        AddNode("インフィニティ", 5, Sys_NodeGroup.TernTime, 5, "効果継続５ターン");

        AddNode("ストレンジ", 1, Sys_NodeGroup.NodeHindranceTime, 2, "ノード選択画面妨害の効果継続２ターン");
        AddNode("イリュージョン", 3, Sys_NodeGroup.NodeHindranceTime, 3, "ノード選択画面妨害の効果継続３ターン");
        AddNode("ファントム", 5, Sys_NodeGroup.NodeHindranceTime, 5, "ノード選択画面妨害の効果継続５ターン");

        AddNode("プリンス", 2, Sys_NodeGroup.NumberScale, 2, "更に攻撃個数ボーナス×２");
        AddNode("ヴァイスロイ", 3, Sys_NodeGroup.NumberScale, 3, "更に攻撃個数ボーナス×３");
        AddNode("キング", 4, Sys_NodeGroup.NumberScale, 4, "更に攻撃個数ボーナス×４");
        AddNode("エンペラー", 5, Sys_NodeGroup.NumberScale, 5, "更に攻撃個数ボーナス×５");

        AddNode("アルファ", 1, Sys_NodeGroup.Speed, 2, "加えてダメージ２倍");
        AddNode("ベータ", 2, Sys_NodeGroup.Speed, 3, "加えてダメージ３倍");
        AddNode("ガンマ", 3, Sys_NodeGroup.Speed, 4, "加えてダメージ４倍");
        AddNode("デルタ", 4, Sys_NodeGroup.Speed, 5, "加えてダメージ５倍");
        AddNode("イプシロン", 5, Sys_NodeGroup.Speed, 7, "加えてダメージ７倍");

        //--------------------------------------------------------------------------------------------------------------------------------//

        Destroy(this.GetComponent<Sys_NodeCreate>());
    }
}
