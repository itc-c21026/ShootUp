using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B002_Anim : MonoBehaviour
{
    PlayerScript Pscript;

    Animator animBeam;
    Animator animCharge;

    public GameObject charge;
    public GameObject BeamObj;

    void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();

        animBeam = GetComponent<Animator>();

        animCharge = gameObject.transform.parent
            .gameObject.transform.Find("charge")
            .gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (Pscript.pause)
        {
                animBeam.SetFloat("AnimSpeed", 1.5f);
                animCharge.SetFloat("AnimSpeed", 1.5f);
        }
        else
        {
            animBeam.SetFloat("AnimSpeed", 0.0f);
            animCharge.SetFloat("AnimSpeed", 0.0f);
        }
    }
    public void Beam()
    {
        BeamObj.SetActive(true);
    }
    public void End()
    {
        this.gameObject.SetActive(false);
        charge.SetActive(false);
        BeamObj.SetActive(false);
    }
}
