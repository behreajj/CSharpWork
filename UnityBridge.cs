using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Converts basic representations to UnityEngine.
/// </summary>
public static class UnityBridge
{
    /// <summary>
    /// Converts from a Unity Bounds to a Bounds3.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>conversion</returns>
    public static Bounds3 FromBounds(in Bounds b)
    {
        return new Bounds3(
            UnityBridge.FromVector3(b.min),
            UnityBridge.FromVector3(b.max));
    }

    /// <summary>
    /// Converts from a Unity Color to a Clr.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>conversion</returns>
    public static Clr FromColor(in Color c)
    {
        return new Clr(c.r, c.g, c.b, c.a);
    }

    /// <summary>
    /// Converts from an array of Unity colors to
    /// an array of integers. The ClrChannel enumeration
    /// signals the order for how integers should be packed.
    /// </summary>
    /// <param name="cs">colors</param>
    /// <returns>conversion</returns>
    public static Clr[] FromColor(in Color[] cs)
    {
        int len = cs.Length;
        Clr[] target = new Clr[len];
        for (int i = 0; i < len; ++i)
        {
            target[i] = UnityBridge.FromColor(cs[i]);
        }
        return target;
    }

    /// <summary>
    /// Converts from a Unity Color32 to a Clr.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>conversion</returns>
    public static Clr FromColor32(in Color32 c)
    {
        return new Clr(c.r, c.g, c.b, c.a);
    }

    /// <summary>
    /// Converts from an array of Unity colors to
    /// an array of integers. The ClrChannel enumeration
    /// signals the order for how integers should be packed.
    /// </summary>
    /// <param name="cs">colors</param>
    /// <returns>conversion</returns>
    public static Clr[] FromColor32(in Color32[] cs)
    {
        int len = cs.Length;
        Clr[] target = new Clr[len];
        for (int i = 0; i < len; ++i)
        {
            target[i] = UnityBridge.FromColor32(cs[i]);
        }
        return target;
    }

    /// <summary>
    /// Converts from a Unity Gradient to a ClrGradient.
    /// </summary>
    /// <param name="g">gradient</param>
    /// <returns>conversion</returns>
    public static ClrGradient FromGradient(in Gradient g)
    {
        GradientColorKey[] colorKeys = g.colorKeys;
        GradientAlphaKey[] alphaKeys = g.alphaKeys;

        int ckLen = colorKeys.Length;
        int akLen = alphaKeys.Length;

        SortedSet<float> steps = new SortedSet<float>();
        for (int h = 0; h < ckLen; ++h) { steps.Add(colorKeys[h].time); }
        for (int i = 0; i < akLen; ++i) { steps.Add(alphaKeys[i].time); }

        ClrKey[] keys = new ClrKey[steps.Count];
        int j = 0;
        foreach (float step in steps)
        {
            Color c = g.Evaluate(step);
            keys[j] = new ClrKey(step, UnityBridge.FromColor(c));
            ++j;
        }

        return new ClrGradient(keys);
    }

    /// <summary>
    /// Converts from a Unity Matrix4x4 to a Mat4.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>conversion</returns>
    public static Mat4 FromMatrix4x4(in Matrix4x4 m)
    {
        return new Mat4(
            m[0, 0], m[0, 1], m[0, 2], m[0, 3],
            m[1, 0], m[1, 1], m[1, 2], m[1, 3],
            m[2, 0], m[2, 1], m[2, 2], m[2, 3],
            m[3, 0], m[3, 1], m[3, 2], m[3, 3]);
    }

    /// <summary>
    /// Converts from a Unity Quaternion to a Quat.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>conversion</returns>
    public static Quat FromQuaternion(in Quaternion q)
    {
        return new Quat(q.w, q.x, q.y, q.z);
    }

    /// <summary>
    /// Converts from a Unity Vector2 to a Vec2.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>conversion</returns>
    public static Vec2 FromVector2(in Vector2 v)
    {
        return new Vec2(v.x, v.y);
    }

    /// <summary>
    /// Converts from a Unity Vector3 to a Vec3.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>conversion</returns>
    public static Vec3 FromVector3(in Vector3 v)
    {
        return new Vec3(v.x, v.y, v.z);
    }

    /// <summary>
    /// Converts from a Unity Vector4 to a Vec4.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>conversion</returns>
    public static Vec4 FromVector4(in Vector4 v)
    {
        return new Vec4(v.x, v.y, v.z, v.w);
    }

    /// <summary>
    /// Converts to a Unity Bounds from a Bounds3.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>conversion</returns>
    public static Bounds ToBounds(in Bounds3 b)
    {
        return new Bounds(
            UnityBridge.ToVector3(Bounds3.Center(b)),
            UnityBridge.ToVector3(Bounds3.Extent(b)));
    }

    /// <summary>
    /// Converts to a Unity Color from a Clr.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>conversion</returns>
    public static Color ToColor(in Clr c)
    {
        return new Color(c.r, c.g, c.b, c.a);
    }

    /// <summary>
    /// Converts to an array of Unity colors from
    /// an array of integers. The ClrChannel enumeration
    /// signals the order for how integers should be packed.
    /// </summary>
    /// <param name="cs">colors</param>
    /// <returns>conversion</returns>
    public static Color[] ToColor(in Clr[] cs)
    {
        int len = cs.Length;
        Color[] target = new Color[len];
        for (int i = 0; i < len; ++i)
        {
            target[i] = UnityBridge.ToColor(cs[i]);
        }
        return target;
    }

    /// <summary>
    /// Converts to a Unity Color32 from a Clr.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>conversion</returns>
    public static Color32 ToColor32(in Clr c)
    {
        return new Color32(
            (byte)(Utils.Clamp(c.r, 0.0f, 1.0f) * 255.0f + 0.5f),
            (byte)(Utils.Clamp(c.g, 0.0f, 1.0f) * 255.0f + 0.5f),
            (byte)(Utils.Clamp(c.b, 0.0f, 1.0f) * 255.0f + 0.5f),
            (byte)(Utils.Clamp(c.a, 0.0f, 1.0f) * 255.0f + 0.5f));
    }

    /// <summary>
    /// Converts to an array of Unity colors from
    /// an array of integers. The ClrChannel enumeration
    /// signals the order for how integers should be packed.
    /// </summary>
    /// <param name="cs">colors</param>
    /// <returns>conversion</returns>
    public static Color32[] ToColor32(in Clr[] cs)
    {
        int len = cs.Length;
        Color32[] target = new Color32[len];
        for (int i = 0; i < len; ++i)
        {
            target[i] = UnityBridge.ToColor32(cs[i]);
        }
        return target;
    }

    /// <summary>
    /// Converts to a Unity Matrix4x4 from a Mat4.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>conversion</returns>
    public static Matrix4x4 ToMatrix4x4(in Mat4 m)
    {
        return new Matrix4x4(
            new Vector4(m.m00, m.m10, m.m20, m.m30),
            new Vector4(m.m01, m.m11, m.m21, m.m31),
            new Vector4(m.m02, m.m12, m.m22, m.m32),
            new Vector4(m.m03, m.m13, m.m23, m.m33));
    }

    /// <summary>
    /// Converts to a Unity Quaternion from a Quat.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>conversion</returns>
    public static Quaternion ToQuaternion(in Quat q)
    {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }

    /// <summary>
    /// Converts to a Unity Vector2 from a Vec2.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>conversion</returns>
    public static Vector2 ToVector2(in Vec2 v)
    {
        return new Vector2(v.x, v.y);
    }

    /// <summary>
    /// Converts to a Unity Vector3 from a Vec3.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>conversion</returns>
    public static Vector3 ToVector3(in Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    /// <summary>
    /// Converts to a Unity Vector4 from a Vec4.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>conversion</returns>
    public static Vector4 ToVector4(in Vec4 v)
    {
        return new Vector4(v.x, v.y, v.z, v.w);
    }

    /// <summary>
    /// Converts to a Unity Gradient from a ClrGradient.
    /// </summary>
    /// <param name="cg">color gradient</param>
    /// <returns>conversion</returns>
    public static Gradient ToGradient(in ClrGradient cg)
    {
        int keyCount = 8;
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[keyCount];
        GradientColorKey[] colorKeys = new GradientColorKey[keyCount];

        float toStep = 1.0f / (keyCount - 1.0f);
        for (int i = 0; i < keyCount; ++i)
        {
            float step = i * toStep;
            Clr c = ClrGradient.Eval(cg, step);
            alphaKeys[i] = new GradientAlphaKey(c.a, step);
            Clr o = new Clr(c.r, c.g, c.b, 1.0f);
            colorKeys[i] = new GradientColorKey(UnityBridge.ToColor(o), step);
        }

        Gradient unityGradient = new Gradient();
        unityGradient.SetKeys(colorKeys, alphaKeys);
        return unityGradient;
    }
}