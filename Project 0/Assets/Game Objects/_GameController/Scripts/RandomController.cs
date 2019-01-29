using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomController : MonoBehaviour {

    [Header("Floats:")]
    public float PubRmax;
    public float PubRmin;
    public float Rmax;
    public float Rmin;

    [Header("Random:")]
    public static float rand;

    void FixRandom(float _StatRMax, float _StatRMin, float _Rmax, float _Rmin)
    {
        Rmax = PubRmax;
        Rmin = PubRmin;


        rand = Random.Range(Rmin, Rmax);

        Rmax = rand + 10;
        Rmin = rand - 10;

        if (Rmax > PubRmax)
        {
            Rmax = 50;
        }
        if (Rmin < PubRmin)
        {
            Rmin = 1;
        }

        PubRmax = _StatRMax;
        PubRmin = _StatRMin;
        Rmax = _Rmax;
        Rmin = _Rmin;
    }
}
