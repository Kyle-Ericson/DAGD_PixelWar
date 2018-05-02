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
	private AudioClip sfxWalk = null;

	
	private AudioClip musicTogetherWeAreStronger = null;
	private AudioClip musicQuietSaturday = null;

	public override void Init () 
	{
		musicSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		sfxSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		SetMusicVolume();
		SetSFXVolume();

		sfxDeath = Resources.Load<AudioClip>("audio/sfx/death1");
		sfxShoot = Resources.Load<AudioClip>("audio/sfx/shoot1");
		sfxCursor = Resources.Load<AudioClip>("audio/sfx/cursor_move1");
		sfxHover = Resources.Load<AudioClip>("audio/sfx/hover_button");
		sfxWalk = Resources.Load<AudioClip>("audio/sfx/move1");
		
		musicTogetherWeAreStronger = Resources.Load<AudioClip>("audio/music/Komiku_-_59_-_Together_we_are_stronger");
		musicQuietSaturday = Resources.Load<AudioClip>("audio/music/Komiku_-_57_-_Quiet_Saturday");
	}
	public void PlayTitleMusic()
	{
		if(musicSource.isPlaying) return;
		musicSource.clip = musicTogetherWeAreStronger;
		musicSource.Play();
	}	
	public void PlayGameMusic()
	{
		musicSource.clip = musicQuietSaturday;
		musicSource.Play();
	}
	public void PlayShoot()
	{
		sfxSource.clip = sfxShoot;
		sfxSource.Play();
	}
	public void PlayDeath()
	{
		sfxSource.clip = sfxDeath;
		sfxSource.Play();
	}
	public void PlayeMoveCursor()
	{
		//sfxSource.clip = sfxCursor;
		//sfxSource.Play();
	}
	public void PlayWalk()
	{
		if(sfxSource.isPlaying) return;
		sfxSource.clip = sfxWalk;
		sfxSource.Play();
	}
	public void PlayHoverButton()
	{
		sfxSource.clip = sfxHover;
		sfxSource.Play();
	}
	public void SetMusicVolume()
	{
		musicSource.volume = PersistentSettings.musicVolume / 5;
	}
	public void SetSFXVolume()
	{
		sfxSource.volume = PersistentSettings.sfxVolume;
	}
}
