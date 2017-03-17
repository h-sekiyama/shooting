using UnityEngine;
using System.Collections;

/// 敵
public class Enemy : Token
{
  /// スプライト
  public Sprite Spr0;
  public Sprite Spr1;
  public Sprite Spr2;
  public Sprite Spr3;
  public Sprite Spr4;
  public Sprite Spr5;

  /// 難易度別のHPテーブル変数
  public static int[] hps;

  /// 難易度別の発射間隔変数
  public static float sec1;
  public static float sec2;
  public static float sec3;

  /// 難易度別のホーミング回転速度
  public static float rot;

  /// 敵管理
  public static TokenMgr<Enemy> parent = null;
  
  /// 敵の追加
  public static Enemy Add(int id, float x, float y, float direction, float speed)
  {
    Enemy e = parent.Add(x, y, direction, speed);
    if(e == null)
    {
      return null;
    }
    
    // 初期パラメータ設定
    e.SetParam(id);
    return e;
  }

  /// 狙い撃ちするターゲット
  public static Player target = null;
  /// 狙い撃ち角度を取得する
  public float GetAim()
  {
    float dx = target.X - X;
    float dy = target.Y - Y;
    return Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
  }

  /// 敵のID
  int _id = 0;

  /// HP
  int _hp = 0;
  /// HPの取得
  public int Hp
  {
    get { return _hp; }
  }

  /// IDからパラメータを設定
  public void SetParam(int id)
  {
    //            0 ,  1   2   3   4   5
    // HPテーブル
    int[] hps = { 500, 30, 30, 30, 30, 30 };

    switch(GameMgr.difficulty) {
      case 0:
        hps[0] = 300;
        hps[5] = 15;
        sec1 = 4.0f;
        sec2 = 5.0f;
        sec3 = 5.0f;
        rot = 4.8f;
        break;
      case 1:
        hps[0] = 400;
        hps[5] = 25;
        sec1 = 3.5f;
        sec2 = 4.0f;
        sec3 = 5.0f;
        rot = 5.0f;
        break;
      case 2:
        hps[0] = 500;
        hps[5] = 30;
        sec1 = 3.0f;
        sec2 = 4.0f;
        sec3 = 3.0f;
        rot = 5.4f;
        break;
      case 3:
        hps[0] = 600;
        hps[5] = 32;
        sec1 = 2.4f;
        sec2 = 3.0f;
        sec3 = 2.6f;
        rot = 6.0f;
        break;
      case 4:
        hps[0] = 800;
        hps[5] = 36;
        sec1 = 2.0f;
        sec2 = 2.5f;
        sec3 = 2.4f;
        rot = 6.4f;
        break;
      case 5:
        hps[0] = 1600;
        hps[5] = 40;
        sec1 = 1.6f;
        sec2 = 1.8f;
        sec3 = 2.0f;
        rot = 6.8f;
        break;
    }
    if (_id != 0)
    {
      // 前回のコルーチンを終了する
      StopCoroutine("_Update" + _id);
    }
    if (id != 0)
    {
      // コルーチンを新しく開始する
      StartCoroutine("_Update" + id);
    }

    // IDを設定
    _id = id;

    // スプライトテーブル
    Sprite[] sprs = { Spr0, Spr1, Spr2, Spr3, Spr4, Spr5 };

    // HPを設定
    _hp = hps[id];

    // スプライトを設定
    SetSprite(sprs[id]);

    // サイズ変更
    Scale = 0.5f;
  }

  /// ダメージを与える
  bool Damage(int v)
  {
    _hp -= v;
    if (_hp <= 0)
    {
      // HPがなくなったので死亡
      Vanish();
      // 倒した
      for (int i = 0; i < 4; i++)
      {
        Particle.Add(X, Y);
      }
      // 破壊SE再生
      Sound.PlaySe("destroy", 0);

      // ボスを倒したらザコ敵と敵弾を消す
      if (_id == 0)
      {
        // 生存しているザコ敵を消す
        Enemy.parent.ForEachExist(e => e.Damage(9999));

        // 敵弾をすべて消す
        if (Bullet.parent != null)
        {
          Bullet.parent.Vanish();
        }
      }

      return true;
    }

    // まだ生きている
    return false;
  }
  
  /// 弾を発射する
  void DoBullet(float direction, float speed)
  {
    Bullet.Add(X, Y, direction, speed);
  }

  /// 更新
  void Update()
  {
    if (_id == 4)
    {
      // だいこんのみ
      Vector2 min = GetWorldMin();
      Vector2 max = GetWorldMax();

      if (Y < min.y || max.y < Y)
      {
        // 上下ではみ出したら跳ね返るようにする
        ClampScreen();
        // 移動速度を反転
        VY *= -1;
      }
      if (X < min.x || max.x < X)
      {
        // 左右ではみ出したら消滅する
        Vanish();
      }

      // 移動方向を向くようにする
      Angle = Direction;
    }
  }

  /// 固定フレームで更新
  void FixedUpdate()
  {
    if (_id <= 3)
    {
      // 通常の敵だけ移動速度を減衰する
      MulVelocity(0.93f);
    }
  }

  /// 更新
  IEnumerator _Update1()
  {
    while (true)
    {
      // ◯秒おきに弾を撃つ
      yield return new WaitForSeconds(sec1);
      // 狙い撃ち角度を取得
      float dir = GetAim();
      Bullet.Add(X, Y, dir, 2);
    }
  }

  IEnumerator _Update2()
  {
    // 発射角度を回転しながら撃つ
    yield return new WaitForSeconds(sec2);
    float dir = 0;
    while (true)
    {
      Bullet.Add(X, Y, dir, 2);
      dir += 16;
      yield return new WaitForSeconds(0.1f);
    }
  }

  IEnumerator _Update3()
  {
    // 3Way弾を撃つ
    while (true)
    {
      // ◯秒おきに弾を撃つ
      yield return new WaitForSeconds(sec3);
      DoBullet(180 - 2, 2);
      DoBullet(180, 2);
      DoBullet(180 + 2, 2);
    }
  }

  IEnumerator _Update4()
  {
    // 何もしない
    yield return new WaitForSeconds(1.0f);
  }

  IEnumerator _Update5()
  {
    // 1回の更新で旋回する角度
    float ROT = rot;
    // ホーミングする
    while (true)
    {
      // 0.02秒おきに更新する
      yield return new WaitForSeconds(0.02f);
      // 現在の角度
      float dir = Direction;
      // 狙い撃ち角度
      float aim = GetAim();
      // 角度差を求める
      float delta = Mathf.DeltaAngle(dir, aim);
      if (Mathf.Abs(delta) < ROT)
      {
        // 角度差が小さいので回転不要
      }
      else if (delta > 0)
      {
        // 左回り
        dir += ROT;
      }
      else
      {
        // 右回り
        dir -= ROT;
      }
      SetVelocity(dir, Speed);

      // 画像も回転させる
      Angle = dir;

      // 画面外に出たら消える
      if (IsOutside())
      {
        Vanish();
      }
    }
  }

  //以下ラスボス用
  IEnumerator _Update6()
  {
    // 発射角度を回転しながら撃つ
    // yield return new WaitForSeconds(0.1f);
    float dir = 0;
    while (true)
    {
      Bullet.Add(X, Y, dir, 2);
      dir += Random.Range(25, 100);
      yield return new WaitForSeconds(0.0024f);
    }
  }

  /// 衝突判定
  void OnTriggerEnter2D(Collider2D other)
  {
    // Layer名を取得する
    string name = LayerMask.LayerToName(other.gameObject.layer);

    if (name == "Shot")
    {
      // ショットであれば当たりとする
      Shot s = other.GetComponent<Shot>();
      // ショットを消す
      s.Vanish();
      // ダメージ処理
      Damage(1);
    }
  }
}
