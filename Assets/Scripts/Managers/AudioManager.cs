using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource gameMusic;
    [SerializeField] AudioSource gameOver;
    [SerializeField] AudioSource click;
    [SerializeField] AudioSource collect;

    public static AudioManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    public void ClickSound()
    {
        click.Play();
    }

    public void CollectSound()
    {
        collect.Play();
    }

    public void GameOverSound()
    {
        gameOver.Play();
    }

    public void PlayMusic(bool play)
    {
        if(play)
            gameMusic.Play();
        else
            gameMusic.Stop();
    }
}