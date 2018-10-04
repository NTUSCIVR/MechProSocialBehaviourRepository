using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSolver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //dir is the direction of the vector, fwd is the direction to decide whether dir is left or right, and up is the up vector of fwd
    //returns 1 (right), -1 (left), 0 (directly forward or backwards)
    public static int DeriveDirection(Vector3 dir, Vector3 fwd, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, dir);
        float result = Vector3.Dot(perp, up);

        if (result > 0f)
        {
            //1 is right
            return 1;
        }
        else if (result < 0f)
        {
            //-1 is left
            return -1;
        }
        else
        {
            //0 is directly forward or backwards
            return 0;
        }
    }
}
