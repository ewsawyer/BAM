using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterStamp : JuiceEffect
{
    [Tooltip("The sprite to leave behind after the can was hit")] [SerializeField]
    private GameObject Stamp;

    [Tooltip("The smallest possible scale of the splatter stamp, used when a splat is placed at the edge of the radius")] [SerializeField]
    private float MinScale;

    [Tooltip("The largest possible scale of the splatter stamp, used when a splat is placed at the center of the radius")] [SerializeField]
    private float MaxScale;

    [Tooltip("The radius in which to distribute the splatters")] [SerializeField]
    private float SplatterRadius;

    [Tooltip("The minimum number of splatter stamps to leave behind")] [SerializeField]
    private int MinSplatterCount;

    [Tooltip("The maximum number of splatter stamps to leave behind")] [SerializeField]
    private int MaxSplatterCount;

    [Tooltip("Randomly delay the creation of each splat by this much so they appear in sequence")] [SerializeField]
    private float RandomSplatDelay;

    [Tooltip("Color will randomly be lerped between this color and the other serialized one")] [SerializeField]
    private Color RandomColorLower;
    [SerializeField] private Color RandomColorUpper;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Play()
    {
        StartCoroutine(SplatCoroutine());
    }

    private IEnumerator SplatCoroutine()
    {
        int numSplatters = Random.Range(MinSplatterCount, MaxSplatterCount);

        for (int i = 0; i < numSplatters; i++)
        {
            GameObject stamp = Instantiate(Stamp);
            // Randomly position the stamp within the radius, non-evenly wrt radius
            stamp.transform.position = transform.position;
            stamp.transform.Rotate(Vector3.forward, Random.Range(0.0f, 360.0f));
            stamp.transform.position += stamp.transform.up * Random.Range(0.0f, SplatterRadius);
            // Scale the stamp according to how far it is from the center of the splatter
            float scaleFactor = 1 - Vector2.Distance(stamp.transform.position, transform.position) / SplatterRadius;
            float scale = Mathf.Lerp(MinScale, MaxScale, scaleFactor);
            stamp.transform.localScale = new Vector2(scale, scale);
            // Randomly pick a color
            stamp.GetComponent<SpriteRenderer>().color =
                Color.Lerp(RandomColorLower, RandomColorUpper, Random.Range(0.0f, 1.0f));
            // Wait a bit before the next splat
            // yield return new WaitForSecondsRealtime(Random.Range(0.0f, RandomSplatDelay));
        }
        
        yield return null;
    }
}
