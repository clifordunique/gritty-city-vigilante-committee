
using UnityEngine;
using System.Collections;

/*!
 *  @class LoopOne
 *  Loop through all animation sequences of a single game object.
 */
[AddComponentMenu("Harmony/Utils/LoopOne")]
public class LoopOne : MonoBehaviour {
    public float frameRate = 24.0f;
    public int anim = 0;
  IEnumerator Start()
  {
    HarmonyAnimation[] animations = FindObjectsOfType<HarmonyAnimation>();

    //  Wait for audio to be complete before playing animation.
    foreach( HarmonyAnimation animation in animations )
    {
      GameObject gameObject = animation.gameObject;

      HarmonyRenderer renderer = gameObject.GetComponent<HarmonyRenderer>();
      if ( renderer != null )
      {
        //  Preemptively load clip.
        renderer.LoadClipIndex( anim/* first clip */);
      }

      //  Wait for audio if necessary.
      HarmonyAudio audio = gameObject.GetComponent<HarmonyAudio>();
      if ( audio != null )
      {
        yield return StartCoroutine(audio.WaitForDownloads());
      }
    }

    foreach( HarmonyAnimation animation in animations )
    {
      animation.LoopAnimation( 24, anim /* first clip */ );

      //  Loop only part of the animation.
      //animation.LoopFrames( 24.0f, 1.0f, 30.0f );
    }
  }
}

