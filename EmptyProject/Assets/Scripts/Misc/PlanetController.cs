using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public static PlanetController Instance;
    public List<Planet> planets;

    public bool allPlanetsSurveyed;
    bool alreadyAwarded = false;
    public Animator achievementAnim;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckList()
    {
        if(alreadyAwarded)
            return;

        bool temp = true;

        for(int i = 0; i < planets.Count; i++)
        {
            if(planets[i].surveyed==false)
            {
                temp = false;
            }
        }

        allPlanetsSurveyed = temp;

        if(allPlanetsSurveyed)
        {
            alreadyAwarded = true;
            achievementAnim.SetTrigger("Show");
        }
    }
}