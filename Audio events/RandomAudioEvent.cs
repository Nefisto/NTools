using UnityEngine;

namespace NTools
{
    [CreateAssetMenu(fileName = "RandomAudioEvent", menuName = "NTools/Audio Events/Random")]
    public class RandomAudioEvent : AudioEvent
    {
        public AudioClip[] clips;

        public Vector2 volume = new Vector2(0, 2);
        public Vector2 pitch = new Vector2(0, 1);

        public override void Play (AudioSource source)
        {
            if (clips.Length == 0)
                return;

            source.clip = clips[Random.Range(0, clips.Length)];

            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);

            source.Play();
        }
    }
}