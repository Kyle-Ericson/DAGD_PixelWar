using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;

public class SoundManager : eSingletonMono<SoundManager> {

	private AudioSource sfxSource;
	private AudioSource musicSource;

	private AudioClip sfxDeath = null;
	private AudioClip sfxShoot = null;
	private AudioClip sfxCursor = null;
	private AudioClip sfxHover = null;

	private AudioClip musicFireDarer = null;
	private AudioClip musicPyramidLevel = null;
	private AudioClip musicAirship = null;
	private AudioClip musicWeCanDoIt = null;

	public override void Init () 
	{
		musicSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		sfxSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		musicSource.volume = PersistentSettings.musicVolume;
		sfxSource.volume = PersistentSettings.sfxVolume;

		sfxDeath = Resources.Load<AudioClip>("audio/sfx/death1");
		sfxShoot = Resources.Load<AudioClip>("audio/sfx/shoot1");
		sfxCursor = Resources.Load<AudioClip>("audio/sfx/cursor_move1");
		sfxHover = Resources.Load<AudioClip>("audio/sfx/hover_button");

		musicFireDarer = Resources.Load<AudioClip>("audio/music/sawsquarenoise_-_02_-_Fire_Darer");
		musicPyramidLevel = Resources.Load<AudioClip>("audio/music/Visager_-_06_-_Pyramid_Level");
		musicAirship = Resources.Load<AudioClip>("audio/music/Visager_-_08_-_Airship");
		musicWeCanDoIt = Resources.Load<AudioClip>("audio/music/Visager_-_26_-_We_Can_Do_It_Loop");
	}
	public void PlayTitleMusic()
	{
		musicSource.clip = musicFireDarer;
		musicSource.Play();
	}	
	public void PlayGameMusic()
	{
		musicSource.clip = musicAirship;
		musicSource.Play();
	}
	public void PlayShoot()
	{
		sfxSource.clip = sfxShoot;
		sfxSource.Play();
	}
	public void PlayDeath()
	{
		sfxSource.clip = sfxShoot;
		sfxSource.Play();
	}
	public void PlayeMoveCursor()
	{
		sfxSource.clip = sfxCursor;
		sfxSource.Play();
	}
	public void PlayHoverButton()
	{
		sfxSource.clip = sfxHover;
		sfxSource.Play();
	}
	public void SetMusicVolume()
	{
		musicSource.volume = PersistentSettings.musicVolume;
	}
	public void SetSFXVolume()
	{
		sfxSource.volume = PersistentSettings.sfxVolume;
	}
}
