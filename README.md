# Mono Rink

**友情を壊すのは、いつだってエアホッケー。**

1 台のキーボードで 2 人が向かい合って遊ぶ、白黒のエアホッケーゲームです。操作はシンプルで、勝負を分けるのは一瞬の判断と、3 回しか使えないカーブショットの使いどころ。先に 9 点取ったほうが勝ちです。

**▶ [unityroom で遊ぶ](https://unityroom.com/games/monorink)**（ブラウザでそのまま遊べます）

<img width="1451" height="807" alt="プレイ画面" src="https://github.com/user-attachments/assets/aea4ab98-7e26-4a0e-824c-497c786486f5" />

## 遊び方

自分のパドルを上下に動かし、回転させて角度をつけながらパックを打ち返します。相手のゴールにパックを入れると 1 点。9 点先取で勝ちです。

### カーブショット

カーブショットのキーを押したまま、上か下に動きながらパックを打つと、少し遅れてパックがその方向に曲がります。

使えるのは 1 試合に **3 回まで**。残り回数は自分の陣地のランプで分かります。ここぞという場面で切るか、最後まで温存するかが駆け引きのポイントです。

### 操作

| | Player Red | Player Blue |
|---|---|---|
| 上下に移動 | `W` / `S` | `↑` / `↓` |
| 回転 | `A` / `D` | `←` / `→` |
| カーブショット | `Space` | `Right Shift` |

## 開発

| | |
|---|---|
| ジャンル | 対戦エアホッケー |
| プレイ人数 | 2 人（ローカル対戦） |
| エンジン | Unity 2022.3.61f1 |
| 配信 | [unityroom](https://unityroom.com/games/monorink) |

Unity Hub からこのフォルダを開けば、そのまま編集・実行できます。シーンは `Assets/Project/Scenes/` にあり、`Start`（タイトル）から `Play`（試合）に進みます。

```
Assets/Project/Scripts/
  Start/                     タイトル画面の演出
  Play/PlayerControllerBase  パドルの移動・回転とカーブショットの残り回数
  Play/PlayerRed/BlueController  各プレイヤーのキー割り当て
  Play/PuckBlackController   パックの速度制御、カーブ、得点と勝敗判定
  FadeController             シーン切り替えのフェード
```

`WebGLRelease/` は unityroom に上げている WebGL ビルドです。

## ライセンス

[MIT](LICENSE)
