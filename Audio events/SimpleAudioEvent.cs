using UnityEngine;

namespace NTools
{
    [CreateAssetMenu(fileName = "Simple audio event", menuName = "NTools/Audio Events/Simple")]
    public class SimpleAudioEvent : AudioEvent
    {
        public AudioClip clip;

        public Vector2 volume = new Vector2(0, 2);
        public Vector2 pitch = new Vector2(0, 1);

        public override void Play (AudioSource source)
        {
            if (!clip)
                return;

            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);

            source.PlayOneShot(clip);
        }
    }
}
