using UnityEngine;
using System.Collections;

/// ボス
public class Boss : Enemy
{
  /// ボスを倒したフラグ
  public static bool bDestroyed = false;

  /// コルーチンを開始したかどうか
  bool _bStart = false;

  SpriteRenderer spriteRenderer;
  public Sprite sprite_god;
  public Sprite sprite_saturn;

  public static bool lastBossFlag = true;

  /// 開始
  void Start()
  {
    // パラメータを設定
    SetParam(0);
    // ボス倒したフラグ初期化
    bDestroyed = false;

    Gauge.maxHp = Hp;
  }
  /// 消滅
  public override void Vanish()
  {
    // ボスを倒した
    bDestroyed = true;
    base.Vanish();
  }

  /// 更新
  void Update()
  {
    if (_bStart == false)
    {
      // 敵生成開始
      StartCoroutine("_GenerateEnemy");
      _bStart = true;
    }

    if(GameMgr.difficulty == 5 && Hp > 800) {
      spriteRenderer = GetComponent<SpriteRenderer>();
      spriteRenderer.sprite = sprite_saturn;
    }

    //ラスボスのHPが800以下になった場合
    if(GameMgr.difficulty == 5 && Hp <= 800 && lastBossFlag == true) {
      FadeManager.Instance.LoadLevel ("", 0.8f);
      Sound.PlayBgm("bgm7");
      lastBossFlag = false;
      spriteRenderer.sprite = sprite_god;
    }

    Gauge.nowHp = Hp;
  }

  /// 敵の生成
  Enemy AddEnemy(int id, float direction, float speed)
  {
    return Enemy.Add(id, X, Y, direction, speed);
  }

  /// だいこんを3方向に発射
  void BulletRadish()
  {
    // プレイヤーと±30度にだいこんを発射
    float aim = GetAim();
    AddEnemy(4, aim, 3);
    AddEnemy(4, aim - 30, 3);
    AddEnemy(4, aim + 30, 3);
  }

  /// にんじんを発射
  void BulletCarrot()
  {
    AddEnemy(5, 45, 3);
    AddEnemy(5, -45, 3);
  }

  /// 敵生成
  IEnumerator _GenerateEnemy() {
    while (true)
    {
      //ラスボスでHPが800以下になった場合の攻撃パターン
      if(GameMgr.difficulty == 5 && Hp <= 800) {
        yield return new WaitForSeconds(2);
        StartCoroutine("_Update6");
      } else {
        AddEnemy(1, 135, 5);
        AddEnemy(1, 225, 5);
        yield return new WaitForSeconds(3);
        BulletRadish(); // だいこんを発射
        yield return new WaitForSeconds(2);
        AddEnemy(2, 90, 5);
        AddEnemy(2, 270, 5);
        BulletCarrot(); // にんじんを発射
        yield return new WaitForSeconds(3);
        AddEnemy(3, 45, 5);
        AddEnemy(3, -45, 5);
        yield return new WaitForSeconds(3);
        BulletRadish(); // だいこんを発射
        yield return new WaitForSeconds(2);
        BulletCarrot(); // にんじんを発射
      }
    }
  }
  /// HPの描画
  void OnGUI()
  {
    // // テキストを大きめにする
    // Util.SetFontSize(32);
    // // 中央揃えにする
    // Util.SetFontAlignment(TextAnchor.LowerRight);
    // Util.SetFontColor(Color.white);

    // // フォントの位置
    // float w = 64; // 幅
    // float h = 16; // 高さ
    // float px = Screen.width / 2 + Screen.width / 5;
    // float py = Screen.height / 2 + Screen.height / 7;

    // // テキスト描画
    // string text = string.Format("{0,3}", Hp);
    // Util.GUILabel(px, py, w, h, "HP:" + text);
  }

}