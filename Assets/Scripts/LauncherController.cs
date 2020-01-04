using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LauncherController : MonoBehaviour

{
    public GameObject projectile; //reference to prefab
    GameObject curProjectile;
    Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    void Update()
    {
        if (curProjectile == null)
        {
            Vector3 t = transform.position;
            Vector3 ahead = new Vector3(t.x, t.y, t.z - 3);
            curProjectile = Instantiate(projectile, ahead, Quaternion.Euler(new Vector3(0, 0, 0)));
        }

        FacePlayer();
    }

    void FacePlayer()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "RogueProjectile")
        {
            Destroy(gameObject);
            SceneManager.LoadScene("TestRange");
        }
    }
}
