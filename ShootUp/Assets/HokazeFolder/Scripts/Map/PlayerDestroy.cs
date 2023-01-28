using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------
 オブジェクト削除のプログラム
----------------------------------------*/

public class PlayerDestroy : MonoBehaviour
{
    GameObject Player;
    GameObject GC;

    PlayerScript Pscript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        GC = GameObject.Find("GC");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            if (Pscript.pause && !Pscript.dead)
            {
                if (GC.GetComponent<MapScript>().tutorialClear)
                {
                    if (this.gameObject.name == "WallDestroy")
                        WallTransform(130);
                    else
                        WallTransform(100);
                }
            }
        }
    }

    void WallTransform(int Y)
    {
        var y = Player.transform.position.y - this.transform.position.y;
        if (y <= Y)
            this.transform.position += new Vector3(0, 0.015f, 0);
        else
            this.transform.position += new Vector3(0, 1.0f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript script = collision.gameObject.transform.root.gameObject.GetComponent<PlayerScript>();
            script.Guard = false;
            script.PlayerHitDamage(script.HP);
        }

        if (this.gameObject.name == "WallDestroy")
        {
            if (!collision.gameObject.CompareTag("StealthWall")) 
                Destroy(collision.gameObject);
        }
        else
        {
            if (!collision.gameObject.CompareTag("Wall") && !collision.gameObject.CompareTag("StealthWall"))
            {
                if (collision.gameObject.name != "GroundCheck" && collision.gameObject.name != "GrenadeReceive")
                {
                    if (collision.gameObject.CompareTag("MainEnemy"))
                    {
                        string s = collision.gameObject.name;
                        GameObject enemy = collision.gameObject.transform.Find("Enemy").gameObject;

                        switch (s)
                        {
                            case "E101(Clone)":
                                Destroy(enemy.GetComponent<E101>().ebar);
                                break;

                            case "E102(Clone)":
                                Destroy(enemy.GetComponent<E102>().ebar);
                                break;

                            case "E103(Clone)":
                                Destroy(enemy.GetComponent<E103>().ebar);
                                break;

                            case "E201(Clone)":
                                Destroy(enemy.GetComponent<E201>().ebar);
                                break;

                            case "E202(Clone)":
                                Destroy(enemy.GetComponent<E202>().ebar);
                                break;

                            case "E203(Clone)":
                                Destroy(enemy.GetComponent<E203>().ebar);
                                break;

                            case "E204(Clone)":
                                Destroy(enemy.GetComponent<E204>().ebar);
                                break;

                            case "E205(Clone)":
                                Destroy(enemy.GetComponent<E205>().ebar);
                                break;

                            case "E206(Clone)":
                                Destroy(enemy.GetComponent<E206>().ebar);
                                break;

                            case "E207(Clone)":
                                Destroy(enemy.GetComponent<E207>().ebar);
                                break;
                        }
                    }
                    else if (collision.gameObject.CompareTag("Turret"))
                    {
                        string s = collision.gameObject.name;
                        GameObject enemy = collision.gameObject;

                        switch (s)
                        {
                            case "T001(Clone)":
                                Destroy(enemy.GetComponent<T001>().ebar);
                                break;

                            case "T002(Clone)":
                                Destroy(enemy.GetComponent<T002>().ebar);
                                break;

                            case "T101(Clone)":
                                Destroy(enemy.GetComponent<T101>().ebar);
                                break;

                            case "T102(Clone)":
                                Destroy(enemy.GetComponent<T102>().ebar);
                                break;

                            case "T201(Clone)":
                                Destroy(enemy.GetComponent<T201>().ebar);
                                break;

                            case "T202(Clone)":
                                Destroy(enemy.GetComponent<T202>().ebar);
                                break;
                        }
                    }
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
