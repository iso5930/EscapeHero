using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHold_Disposable : MonoBehaviour
{
    private bool IsTrigger = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float PlayerY = collision.gameObject.transform.position.y;
            float FootHoldY = transform.position.y;

            if (PlayerY >= (FootHoldY + 0.25f))
                IsTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(IsTrigger)
        {
            if (collision.gameObject.tag == "Player")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
