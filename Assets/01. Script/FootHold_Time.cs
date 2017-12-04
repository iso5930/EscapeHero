using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHold_Time : MonoBehaviour
{
    public float SpareTime = 3.0f;

    private bool IsTrigger = false;

    // Update is called once per frame
    void Update()
    {
        if(SpareTime <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            float PlayerY = collision.gameObject.transform.position.y;
            float FootHoldY = transform.position.y;

            if (PlayerY >= (FootHoldY + 0.25f))
                IsTrigger = true;

            SpareTime = 3.0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && IsTrigger)
        {
            SpareTime -= Time.deltaTime;
        }
        else if(collision.gameObject.tag == "Player" && !IsTrigger)
        {
            float PlayerY = collision.gameObject.transform.position.y;
            float FootHoldY = transform.position.y;

            if (PlayerY >= (FootHoldY + 0.25f))
                IsTrigger = true;
        }
    }
}
