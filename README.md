# BreathDetection
Simple breath detection script for unity

# 使い方
1. BreathDetectorをAudioSourceに貼り付ける
2. MicをAudioSourceに貼り付ける
3. BreathDetectorコンポーネントのBreathLevelプロパティから吐息の音量を取得する

# 仕組み
設定した周波数以下のパワースペクトルの総和と、全帯域のパワースペクトルの総和の比率から、
吐息かどうかを判定しています。
吐息はホワイトノイズに近いので、この比率が小さいほど吐息っぽいという判定です。
これだけだと無音と区別がつかないので、無音検出も併用しています。

参考： https://www.vocal.com/noise-reduction/single-channel-wind-noise-detection/

# ライセンス
MIT License
