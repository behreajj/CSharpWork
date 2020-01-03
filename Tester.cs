using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{

    public Complex a = new Complex (1f, 3f);
    public Vec2 b = 4.0f;
    public Vec2 c = new Vec2 (1f, 1f);

    void Awake ( )
    {

        // long a = -1426096321L;
        // long b = 2868870975L;
        // int c = unchecked((int)0xaaff7f3f);

        // Debug.Log(string.Format("0xaaff7f3f as int: {0}", c));        
        // Debug.Log(string.Format("2868870975L in hex: 0x{0:x2}", b));
        // Debug.Log(string.Format("a == c: {0}", a == c));
        // Debug.Log(string.Format("int Max: {0}", int.MaxValue));        
        // Debug.Log(string.Format("int Min: {0}", int.MinValue));
        // long excess = b - int.MaxValue;
        // Debug.Log(string.Format("Excess: {0}", excess));
        // Debug.Log(string.Format("Wrapped (+1): {0}", int.MinValue + excess));

        Clr x = 0xff_ff7_f3f;
        Debug.Log (x);
        Debug.Log ((long) x);
        Debug.Log ((int) x);
        int z = unchecked ((int) 0xff_ff7_f3f);
        Clr y = z;
        Debug.Log (y);
        Debug.Log (z);
        Debug.Log ((long) y);
        Debug.Log ((int) y);
    }

}