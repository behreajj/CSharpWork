using UnityEngine;

/// <summary>
/// Converts basic representations to UnityEngine.
/// </summary>
public static class UnityBridge
{
    /// <summary>
    /// Converts from a Unity Bounds to a Bounds3.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>the conversion</returns>
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
    /// <returns>the conversion</returns>
    public static Clr FromColor(in Color c)
    {
        return new Clr(c.r, c.g, c.b, c.a);
    }

    /// <summary>
    /// Converts from a Unity Color32 to a Clr.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the conversion</returns>
    public static Clr FromColor32(in Color32 c)
    {
        return new Clr(c.r, c.g, c.b, c.a);
    }

    /// <summary>
    /// Converts from a Unity Matrix4x4 to a Mat4.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the conversion</returns>
    public static Mat4 FromMatrix4x4(in Matrix4x4 m)
    {
        // TODO: Test
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
    /// <returns>the conversion</returns>
    public static Quat FromQuaternion(in Quaternion q)
    {
        return new Quat(q.w, q.x, q.y, q.z);
    }

    /// <summary>
    /// Converts from a Unity Vector2 to a Vec2.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the conversion</returns>
    public static Vec2 FromVector2(in Vector2 v)
    {
        return new Vec2(v.x, v.y);
    }

    /// <summary>
    /// Converts from a Unity Vector3 to a Vec3.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the conversion</returns>
    public static Vec3 FromVector3(in Vector3 v)
    {
        return new Vec3(v.x, v.y, v.z);
    }

    /// <summary>
    /// Converts from a Unity Vector4 to a Vec4.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the conversion</returns>
    public static Vec4 FromVector4(in Vector4 v)
    {
        return new Vec4(v.x, v.y, v.z, v.w);
    }

    /// <summary>
    /// Converts to a Unity Bounds from a Bounds3.
    /// </summary>
    /// <param name="b">bounds</param>
    /// <returns>the conversion</returns>
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
    /// <returns>the conversion</returns>
    public static Color ToColor(in Clr c)
    {
        return new Color(c.r, c.g, c.b, c.a);
    }

    /// <summary>
    /// Converts to a Unity Color32 from a Clr.
    /// </summary>
    /// <param name="c">color</param>
    /// <returns>the conversion</returns>
    public static Color32 ToColor32(in Clr c)
    {
        return new Color32(
            (byte)(Utils.Clamp(c.r, 0.0f, 1.0f) * 255.0f + 0.5f),
            (byte)(Utils.Clamp(c.g, 0.0f, 1.0f) * 255.0f + 0.5f),
            (byte)(Utils.Clamp(c.b, 0.0f, 1.0f) * 255.0f + 0.5f),
            (byte)(Utils.Clamp(c.a, 0.0f, 1.0f) * 255.0f + 0.5f));
    }

    /// <summary>
    /// Converts to a Unity Matrix4x4 from a Mat4.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>the conversion</returns>
    public static Matrix4x4 ToMatrix4x4(in Mat4 m)
    {
        // TODO: Test
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
    /// <returns>the conversion</returns>
    public static Quaternion ToQuaternion(in Quat q)
    {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }

    /// <summary>
    /// Converts to a Unity Vector2 from a Vec2.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the conversion</returns>
    public static Vector2 ToVector2(in Vec2 v)
    {
        return new Vector2(v.x, v.y);
    }

    /// <summary>
    /// Converts to a Unity Vector3 from a Vec3.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the conversion</returns>
    public static Vector3 ToVector3(in Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    /// <summary>
    /// Converts to a Unity Vector4 from a Vec4.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>the conversion</returns>
    public static Vector4 ToVector4(in Vec4 v)
    {
        return new Vector4(v.x, v.y, v.z, v.w);
    }
}