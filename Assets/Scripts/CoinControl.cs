using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.Play("金币");
        UserStorage.AddScore(1);
        Destroy(gameObject);
    }


}
