using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*--------------------------------------------
 éGãõìGÇÃHPÉQÅ[ÉWÇÃÉvÉçÉOÉâÉÄ
--------------------------------------------*/ 

public class EnemyHPbarScript : MonoBehaviour
{
    Camera Camera;

    GameObject Target;

    int TargetHP;
    float tTargetHP;

    bool tHP;

    Slider slider;

    private void Start()
    {
        if (Camera == null) Camera = Camera.main;

        slider = this.GetComponent<Slider>();
        slider.maxValue = TargetHP;
    }

    private void Update()
    {
        this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera, Target.transform.position);
        this.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y - 30f,
            this.transform.position.z);

        HP_Reload();
        if (tHP)
            slider.value = TargetHP;
        else
            slider.value = tTargetHP;
    }

    public void Initialize(GameObject target, int HP)
    {
        Target = target;
        TargetHP = HP;
    }

    public void HP_Reload()
    {
        switch (Target.transform.root.gameObject.name)
        {
            case "E101(Clone)":
                TargetHP = Target.GetComponent<E101>().HP;
                tHP = true;
                break;

            case "E102(Clone)":
                TargetHP = Target.GetComponent<E102>().HP;
                tHP = true;
                break;

            case "E103(Clone)":
                TargetHP = Target.GetComponent<E103>().HP;
                tHP = true;
                break;

            case "E201(Clone)":
                TargetHP = Target.GetComponent<E201>().HP;
                tHP = true;
                break;

            case "E202(Clone)":
                TargetHP = Target.GetComponent<E202>().HP;
                tHP = true;
                break;

            case "E203(Clone)":
                TargetHP = Target.GetComponent<E203>().HP;
                tHP = true;
                break;

            case "E204(Clone)":
                TargetHP = Target.GetComponent<E204>().HP;
                tHP = true;
                break;

            case "E205(Clone)":
                TargetHP = Target.GetComponent<E205>().HP;
                tHP = true;
                break;

            case "E206(Clone)":
                TargetHP = Target.GetComponent<E206>().HP;
                tHP = true;
                break;

            case "E207(Clone)":
                TargetHP = Target.GetComponent<E207>().HP;
                tHP = true;
                break;

            case "T001(Clone)":
                tTargetHP = Target.GetComponent<T001>().HP;
                tHP = false;
                break;

            case "T002(Clone)":
                tTargetHP = Target.GetComponent<T002>().HP;
                tHP = false;
                break;

            case "T101(Clone)":
                tTargetHP = Target.GetComponent<T101>().HP;
                tHP = false;
                break;

            case "T102(Clone)":
                tTargetHP = Target.GetComponent<T102>().HP;
                tHP = false;
                break;

            case "T201(Clone)":
                tTargetHP = Target.GetComponent<T201>().HP;
                tHP = false;
                break;

            case "T202(Clone)":
                tTargetHP = Target.GetComponent<T202>().HP;
                tHP = false;
                break;
        }
    }
}
