using UnityEngine;
using System.Collections;
using System.Linq;

public class BreathDetector : MonoBehaviour {
	// パワースペクトラム比の閾値。これより小さい値は吐息とみなされる。
	public float Threashold = 0.15f;

	// 音量の閾値。これより小さい値は無音とみなされる。
	public float VolumeThreashold = 1e-08f;

	// 吐息の場合はこの周波数より大きい帯域にもエネルギーが集中している、という境界値
	public float BreathFrequency = 1000.0f;

	// 吐息の音量。吐息でない場合は 0.0 を返す。
	public float BreathLevel
	{
		get;
		private set;
	}

	// パワースペクトラム比をデバッグ表示するための球。設定しなくてもよい。
	public GameObject Sphere;

	// 閾値をデバッグ表示するための球。設定しなくてもよい。
	public GameObject ThreasholdSphere;

	// デバッグ描画を有効にするかどうか。
	public bool DebugDraw = false;

	private float[] Spectrum = new float[1024];
	private float[] WaveData = new float[1024];

	void Update() {
		var Source = GetComponent<AudioSource>();
		if (!Source) {
			Debug.LogError("AudioSource not found.");
			return;
		}

		// 設定した周波数までのパワースペクトラムの総和 / 全帯域のパワースペクトラムの総和 を求める
		// 参考：https://www.vocal.com/noise-reduction/single-channel-wind-noise-detection/
		Source.GetSpectrumData(Spectrum, 0, FFTWindow.Hamming);

		var SumOfPowerSpectrum = 0.0f;
		foreach (var Bin in Spectrum) {
			SumOfPowerSpectrum += Bin;
		}
		var LowBandSumOfPowerSpectrum = 0.0f;
		var BinWidth = AudioSettings.outputSampleRate / Spectrum.Length;
		for (int i = 0; i < Mathf.FloorToInt(BreathFrequency / BinWidth); ++i) {
			LowBandSumOfPowerSpectrum += Spectrum[i];
		}

		var WND_1 = LowBandSumOfPowerSpectrum / SumOfPowerSpectrum;

		// 音量を求める
		AudioListener.GetOutputData(WaveData, 1);
		var Volume = WaveData.Select(x => x * x).Sum() / WaveData.Length;

		// 吐息レベルを設定する
		if (Volume < VolumeThreashold) {
			BreathLevel = 0.0f;
		} else if (WND_1 < Threashold) {
			BreathLevel = Volume;
		} else {
			BreathLevel = 0.0f;
		}

		// デバッグ描画
		if (DebugDraw && Sphere && ThreasholdSphere) {
			Color SphereColor;
			if (Volume < VolumeThreashold) {
				SphereColor = Color.green;
			} else if (WND_1 < Threashold) {
				SphereColor = Color.cyan;
			} else {
				SphereColor = Color.yellow;
			}
			Sphere.GetComponent<MeshRenderer>().material.color = SphereColor;
			Sphere.transform.localScale = Vector3.one * WND_1 * 10;
			ThreasholdSphere.transform.localScale = Vector3.one * Threashold * 10;
		}
	}
}
