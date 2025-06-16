using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    [SerializeField] private AudioSource SourcePrefab;
    
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public AudioSource PlayClip(AudioClip clip, float pitch=1.0f, float volume=1.0f, bool loop=false, float duration=-1.0f, bool doNotDestroy=false)
    {
        AudioSource source = Instantiate(SourcePrefab);
        source.transform.position = Vector3.zero;

        source.pitch = pitch;
        source.volume = volume;
        source.loop = loop;
        source.PlayOneShot(clip);

        if (doNotDestroy)
            return source;
        
        if (duration == -1.0f || duration >= clip.length)
            Destroy(source.gameObject, clip.length);
        else
            Destroy(source.gameObject, duration);

        return source;
    }
}
