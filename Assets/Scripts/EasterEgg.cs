using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EasterEgg : MonoBehaviour {
	public VideoClip videoToPlay;
	public AudioClip audio_p1;
	public AudioClip audio_p2;

	VideoPlayer videoPlayer;
	AudioSource audioSource;

	CarMotion carMotion;
	Collider2D carCollider;
	GameObject mainCamera;
	bool _playingVideo;

	void Start () {
		carMotion = GetComponent<CarMotion>();
		carCollider = GetComponent<Collider2D>();
		mainCamera = GameObject.Find("Main Camera");
	}

	// Update is called once per frame
	void Update () {
		if (carMotion.GetSpeed() > 20f) {
			carMotion.acceleration += carMotion.acceleration * 0.3f * Time.deltaTime;
			carMotion.lockInput = true;
			carCollider.enabled = false;
		}
		if (carMotion.GetSpeed() > 100f && !_playingVideo) {
			FindEgg();
		}
	}
	IEnumerator playVideo() {
		_playingVideo = true;
		// Will attach a VideoPlayer to the main mainCamera.
		mainCamera = GameObject.Find("Main Camera");

		// VideoPlayer automatically targets the mainCamera backplane when it is added
		// to a mainCamera object, no need to change videoPlayer.targetCamera.
		videoPlayer = mainCamera.AddComponent<VideoPlayer>();
		audioSource = mainCamera.AddComponent<AudioSource>();
		audioSource.clip = audio_p1;
		audioSource.Play();
		yield return new WaitForSeconds(audio_p1.length);

		audioSource.clip = audio_p2;

		// Play on awake defaults to true. Set it to false to avoid the url set
		// below to auto-start playback since we're in Start().
		videoPlayer.playOnAwake = false;

		// By default, VideoPlayers added to a mainCamera will use the far plane.
		// Let's target the near plane instead.
		videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;

		// This will cause our scene to be visible through the video being played.
		// videoPlayer.targetCameraAlpha = 0.5F;

		// Set the video to play. URL supports local absolute or relative paths.
		// Here, using absolute.
		// videoPlayer.url = "/media/Data/Code/unity/Hello for CE/Assets/all.webm";
		videoPlayer.clip = videoToPlay;

		// Skip the first 100 frames.
		// videoPlayer.frame = 100;

		// Restart from beginning when done.
		// videoPlayer.isLooping = true;

		// Each time we reach the end, we slow down the playback by a factor of 10.

		// Start playback. This means the VideoPlayer may have to prepare (reserve
		// resources, pre-load a few frames, etc.). To better control the delays
		// associated with this preparation one can use videoPlayer.Prepare() along with
		// its prepareCompleted event.
		videoPlayer.Play();
		audioSource.Play();
		yield return new WaitForSeconds(audio_p2.length);
		Destroy(audioSource);
		Destroy(videoPlayer);
		// This may be removed because there will be new scene
		carMotion.lockInput = false;
		_playingVideo = false;
		carCollider.enabled = true;
		carMotion.ResetCar();
		Destroy(this);
	}

	public void FindEgg() {
		if (_playingVideo) return;
		StartCoroutine(playVideo());
	}
}
