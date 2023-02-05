using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceRay : MonoBehaviour
{
    public int RayCount = 2;
    void Update()
    {
        CastRay(transform.position, transform.forward);
    }

    void CastRay(Vector3 position, Vector3 direction)
    {
        // for (int i = 0; i < RayCount; i++)
        // {
        //     Ray ray2D = new Ray(position, direction);
        //     RaycastHit2D hit2D;
        //
        //     if (Physics.Raycast(ray2D, out hit2D, 10f))
        //     {
        //         Debug.DrawLine(position, hit2D.point, Color.red);
        //         position = hit2D.point;
        //         direction = hit2D.normal;
        //     }
        //     else
        //     {
        //         Debug.DrawRay(position, direction * 10, Color.red);
        //         break;
        //     }
        // }

        for (int i = 0; i < RayCount; i++)
        {
            RaycastHit2D hit2D;
            hit2D = Physics2D.Raycast(position, direction);
            
            if (hit2D)
            {
                Debug.DrawLine(position, new Vector3(hit2D.point.x, hit2D.point.y, 0), Color.red);
                position =  hit2D.point;
                direction = hit2D.normal;
            }
            else
            {
                Debug.DrawRay(new Vector3(position.x, position.y, 0), new Vector3(direction.x, direction.y, 0f) * 10, Color.red); 
                break;
            }
        }
    }
}
