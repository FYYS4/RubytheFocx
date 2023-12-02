using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();


        if (controller != null)
        {
                if (controller != null)
                {
                    controller.ChangeCandiesCount(1);
                     Destroy(gameObject);
                }
        }
    }
}
