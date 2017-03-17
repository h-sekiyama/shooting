using UnityEngine;
using System.Collections;

/// ゲーム管理
public class GameMgr : MonoBehaviour
{
  /// 状態
  enum eState
  {
    Init, // 初期化
    Main, // メイン
    GameClear, // ゲームクリア
    GameOver, // ゲームオーバー
  }
  /// 開始時は初期化状態にする
  eState _state = eState.Init;

  public static int difficulty = 0;

  //ゲーム開始直後のみtrueになる変数
  static bool firstFlag = true;

  /// 開始
  void Start()
  {
    // ショットオブジェクトを32個確保しておく
    Shot.parent = new TokenMgr<Shot>("Shot", 32);
    // パーティクルオブジェクトを256個確保しておく
    Particle.parent = new TokenMgr<Particle>("Particle", 256);
    // 敵弾オブジェクトを256個確保しておく
    Bullet.parent = new TokenMgr<Bullet>("Bullet", 256);
    // 敵オブジェクトを64個確保しておく
    Enemy.parent = new TokenMgr<Enemy>("Enemy", 64);

    // プレイヤーの参照を敵に登録する
    Enemy.target = GameObject.Find("Player").GetComponent<Player>();
  }

  /// 現在の難易度に対応するBGMを鳴らす
  void PlayNowBgm() {
      switch(difficulty) {
        case 0:
          Sound.PlayBgm("bgm1");
          break;
        case 1:
          Sound.PlayBgm("bgm2");
          break;
        case 2:
          Sound.PlayBgm("bgm3");
          break;
        case 3:
          Sound.PlayBgm("bgm4");
          break;
        case 4:
          Sound.PlayBgm("bgm5");
          break;
        case 5:
          Sound.PlayBgm("bgm6");
          break;
      }
  }

  /// 更新
  void Update()
  {
    switch (_state)
    {
      case eState.Init:
        if(firstFlag == true) {
          PlayNowBgm();
          firstFlag = false;
        }
        // メイン状態へ遷移する
        _state = eState.Main;
        break;
      case eState.Main:
        if (Boss.bDestroyed)
        {
          // ボスを倒したのでゲームクリア
          // BGMを止める
          if(difficulty == 5) {
            Sound.StopBgm();
          }
          _state = eState.GameClear;
        }
        else if (Enemy.target.Exists == false)
        {
          // プレイヤーが死亡したのでゲームオーバー
          _state = eState.GameOver;
        }
        break;
      case eState.GameClear:
        if (Util.SmartphoneCheck()) {
          if (Input.touchCount > 0){
            Touch _touch = Input.GetTouch(0);
            Vector2 newVec = new Vector2(_touch.position.x,Screen.height - _touch.position.y);
            if(_touch.phase == TouchPhase.Began){
              if(difficulty < 5) {
                // 難易度アップしてゲームをやり直す
                // Application.LoadLevel("Main");
                difficulty++;
                PlayNowBgm();
                FadeManager.Instance.LoadLevel ("Main", 0.3f);
              } else if(difficulty == 5) {
                // 最終ステージクリアでエンディング
                FadeManager.Instance.LoadLevel ("Ending", 0.8f);
              } else {
                Application.LoadLevel("Title");
              }
            }
          }
        } else {
          if(Input.GetMouseButtonDown(0)) {
              if(difficulty < 5) {
                // 難易度アップしてゲームをやり直す
                // Application.LoadLevel("Main");
                difficulty++;
                PlayNowBgm();
                FadeManager.Instance.LoadLevel ("Main", 0.3f);
              } else if(difficulty == 5) {
                // 最終ステージクリアでエンディング
                FadeManager.Instance.LoadLevel ("Ending", 0.8f);
              } else {
                Application.LoadLevel("Title");
              }
          }
        }
        break;
      case eState.GameOver:
        if (Util.SmartphoneCheck()) {
          if (Input.touchCount > 0){
            Touch _touch = Input.GetTouch(0);
            if(_touch.phase == TouchPhase.Began){
              // ゲームをやり直す
              Application.LoadLevel("Main");
              if(difficulty == 5 && Boss.lastBossFlag == false) {
                Boss.lastBossFlag = true;
                Sound.PlayBgm("bgm6");
              }
            }
          }
        } else {
          if(Input.GetMouseButtonDown(0)) {
            // ゲームをやり直す
            Application.LoadLevel("Main");
            if(difficulty == 5 && Boss.lastBossFlag == false) {
              Boss.lastBossFlag = true;
              Sound.PlayBgm("bgm6");
            }
          }
        }
        break;
    }
  }

  /// ラベルを画面中央に表示
  void DrawLabelCenter(string message)
  {
    // フォントサイズ設定
    Util.SetFontSize(32);
    // 中央揃え
    Util.SetFontAlignment(TextAnchor.MiddleCenter);

    // フォントの位置
    float w = 128; // 幅
    float h = 32; // 高さ
    float px = Screen.width / 2 - w / 2;
    float py = Screen.height / 2 - h / 2;

    // フォント描画
    Util.GUILabel(px, py, w, h, message);
  }

  void OnGUI()
  {
    Util.SetFontSize(28);
    Util.SetFontColor(Color.white);
    switch(difficulty) {
      case 0:
        Util.GUILabel(16, 0, 120, 30, "超かんたん");
        break;
      case 1:
        Util.GUILabel(16, 0, 120, 30, "かんたん");
        break;
      case 2:
        Util.GUILabel(16, 0, 120, 30, "ふつう");
        break;
      case 3:
        Util.GUILabel(16, 0, 120, 30, "むずい");
        break;
      case 4:
        Util.GUILabel(16, 0, 120, 30, "超むずい");
        break;
      case 5:
        Util.GUILabel(16, 0, 120, 30, "神");
        break;
    }
    switch (_state)
    {
      case eState.GameClear:
        Util.SetFontColor(Color.white);
        DrawLabelCenter("GAME CLEAR!");
        break;
      case eState.GameOver:
        Util.SetFontColor(Color.red);
        DrawLabelCenter("GAME OVER");
        break;
    }
  }

  /// 破棄
  void OnDestroy()
  {
    // TokenMgrの参照を消す
    Shot.parent = null;
    Enemy.parent = null;
    Bullet.parent = null;
    Particle.parent = null;

    Enemy.target = null;
  }

}
