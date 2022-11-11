using System;
using System.Text;

/// <summary>
/// Facilitates 3D affine transformations for entities.
/// </summary>
[Serializable]
public class Transform3
{
    /// <summary>
    /// The transform's location.
    /// </summary>
    protected Vec3 location = Vec3.Zero;

    /// <summary>
    /// The transform's rotation in radians.
    /// </summary>
    protected Quat rotation = Quat.Identity;

    /// <summary>
    /// The transform's scale.
    /// </summary>
    protected Vec3 scale = Vec3.One;

    /// <summary>
    /// The transform's forward axis.
    /// </summary>
    /// <value>forward</value>
    public Vec3 Forward { get { return this.rotation.Forward; } }

    /// <summary>
    /// The transform's location.
    /// </summary>
    /// <value>location</value>
    public Vec3 Location
    {
        get
        {
            return this.location;
        }

        set
        {
            this.location = value;
        }
    }

    /// <summary>
    /// The transform's right axis.
    /// </summary>
    /// <value>right</value>
    public Vec3 Right { get { return this.rotation.Right; } }

    /// <summary>
    /// The transform's rotation in radians.
    /// </summary>
    /// <value>rotation</value>
    public Quat Rotation
    {
        get
        {
            return this.rotation;
        }

        set
        {
            if (Quat.Any(value)) { this.rotation = value; }
        }
    }

    /// <summary>
    /// The transform's scale.
    /// </summary>
    /// <value>scale</value>
    public Vec3 Scale
    {
        get
        {
            return this.scale;
        }

        set
        {
            if (Vec3.All(value)) { this.scale = value; }
        }
    }

    /// <summary>
    /// The transform's up axis.
    /// </summary>
    /// <value>up</value>
    public Vec3 Up { get { return this.rotation.Up; } }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Transform3() { }

    /// <summary>
    /// Creates a transform from a location, rotation and scale.
    /// </summary>
    /// <param name="location">location</param>
    /// <param name="rotation">rotation</param>
    /// <param name="scale">scale</param>
    public Transform3(in Vec3 location, in Quat rotation, in Vec3 scale)
    {
        this.Location = location;
        this.Rotation = rotation;
        this.Scale = scale;
    }

    /// <summary>
    /// Creates a transform from loose real numbers.
    /// </summary>
    /// <param name="x">x location</param>
    /// <param name="y">y location</param>
    /// <param name="z">z location</param>
    /// <param name="real">quaternion real</param>
    /// <param name="i">x imaginary</param>
    /// <param name="j">y imaginary</param>
    /// <param name="k">z imaginary</param>
    /// <param name="width">x scale</param>
    /// <param name="height">y scale</param>
    /// <param name="depth">z scale</param>
    public Transform3(
        in float x, in float y, in float z,
        in float real, in float i, in float j, in float k,
        in float width, in float height, in float depth)
    {
        this.Location = new Vec3(x, y, z);
        this.Rotation = new Quat(real, new Vec3(i, j, k));
        this.Scale = new Vec3(width, height, depth);
    }

    /// <summary>
    /// Returns a hash code representing this transform.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.location.GetHashCode();
            hash = hash * Utils.HashMul ^ this.rotation.GetHashCode();
            hash = hash * Utils.HashMul ^ this.scale.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this transform.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Transform3.ToString(this);
    }

    /// <summary>
    /// Moves the transform by a direction to a new location in the global
    /// coordinate system.
    /// </summary>
    /// <param name="v">direction</param>
    /// <returns>this transform</returns>
    public Transform3 MoveByGlobal(in Vec3 v)
    {
        this.Location += v;
        return this;
    }

    /// <summary>
    /// Moves the transform by a direction rotated according to the transform's
    /// rotation.
    /// </summary>
    /// <param name="v">direction</param>
    /// <returns>this transform</returns>
    public Transform3 MoveByLocal(in Vec3 v)
    {
        this.Location += Transform3.MulDir(this, v);
        return this;
    }

    /// <summary>
    /// Eases the transform to a location by a step.
    /// </summary>
    /// <param name="v">direction</param>
    /// <param name="step">step</param>
    /// <returns>this transform</returns>
    public Transform3 MoveTo(in Vec3 v, in Vec3 step)
    {
        this.Location = Vec3.Mix(this.location, v, step);
        return this;
    }

    /// <summary>
    /// Eases the transform to a location by a step according to an easing
    /// function.
    /// </summary>
    /// <param name="v">direction</param>
    /// <param name="step">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>this transform</returns>
    public Transform3 MoveTo(
        in Vec3 v,
        in Vec3 step,
        in Func<Vec3, Vec3, Vec3, Vec3> easing)
    {
        Vec3 t = easing(this.location, v, step);
        this.Location = Vec3.Mix(this.location, v, t);
        return this;
    }

    /// <summary>
    /// Rotates this transform around an axis by an angle in radians.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this transform</returns>
    public Transform3 RotateBy(in float radians, in Vec3 axis)
    {
        this.rotation = this.rotation * Quat.FromAxisAngle(radians, axis);
        return this;
    }

    /// <summary>
    /// Eases the transform toward a new orientation by a step in [0.0, 1.0] .
    /// </summary>
    /// <param name="q">orientation</param>
    /// <param name="step">step</param>
    /// <returns>this transform</returns>
    public Transform3 RotateTo(in Quat q, in float step)
    {
        this.Rotation = Quat.Mix(this.rotation, q, step);
        return this;
    }

    /// <summary>
    /// Rotates this transform around the x axis by an angle in radians.
    ///
    /// Beware that using sequences of orthonormal rotations will result in gimbal
    /// lock.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this transform</returns>
    public Transform3 RotateX(in float radians)
    {
        this.rotation = Quat.RotateX(this.rotation, radians);
        return this;
    }

    /// <summary>
    /// Rotates this transform around the y axis by an angle in radians.
    ///
    /// Beware that using sequences of orthonormal rotations will result in gimbal
    /// lock.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this transform</returns>
    public Transform3 RotateY(in float radians)
    {
        this.rotation = Quat.RotateY(this.rotation, radians);
        return this;
    }

    /// <summary>
    /// Rotates this transform around the z axis by an angle in radians.
    ///
    /// Beware that using sequences of orthonormal rotations will result in gimbal
    /// lock.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this transform</returns>
    public Transform3 RotateZ(in float radians)
    {
        this.rotation = Quat.RotateZ(this.rotation, radians);
        return this;
    }

    /// <summary>
    /// Scales the transform by a uniform scalar, i.e., multiplies the scale by
    /// the argument.
    /// </summary>
    /// <param name="v">uniform scalar</param>
    /// <returns>this transform</returns>    
    public Transform3 ScaleBy(in float v)
    {
        this.Scale *= v;
        return this;
    }

    /// <summary>
    /// Scales the transform by a nonuniform scalar, i.e., multiplies the scale
    /// by the argument.
    /// </summary>
    /// <param name="v">nonuniform scalar</param>
    /// <returns>this transform</returns>
    public Transform3 ScaleBy(in Vec3 v)
    {
        this.Scale *= v;
        return this;
    }

    /// <summary>
    /// Eases the transform to a scale by a step.
    /// </summary>
    /// <param name="v">nonuniform scale</param>
    /// <param name="step">step</param>
    /// <returns>this transform</returns>
    public Transform3 ScaleTo(in Vec3 v, in Vec3 step)
    {
        this.Scale = Vec3.Mix(this.scale, Vec3.CopySign(v, this.scale), step);
        return this;
    }

    /// <summary>
    /// Eases the transform to a scale by a step according to an easing function.
    /// </summary>
    /// <param name="v">nonuniform scale</param>
    /// <param name="step">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>this transform</returns>
    public Transform3 ScaleTo(
        in Vec3 v,
        in Vec3 step,
        in Func<Vec3, Vec3, Vec3, Vec3> easing)
    {
        Vec3 s = Vec3.CopySign(v, this.scale);
        Vec3 t = easing(this.scale, s, step);
        this.Scale = Vec3.Mix(this.scale, s, t);
        return this;
    }

    /// <summary>
    /// Converts a 2D transform to a 3D transform.
    /// </summary>
    /// <param name="t">transform</param>
    public static implicit operator Transform3(in Transform2 t)
    {
        return new Transform3(
            t.Location,
            Quat.FromAngle(t.Rotation),
            Vec3.Promote(t.Scale, 1.0f));
    }

    /// <summary>
    /// Multiplies a direction by a transform's inverse. This rotates the
    /// direction by the transform's negative angle.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="dir">direction</param>
    /// <returns>direction</returns>
    public static Vec3 InvMulDir(in Transform3 transform, in Vec3 dir)
    {
        return Quat.InvMulVector(transform.rotation, dir);
    }

    /// <summary>
    /// Multiplies a normal by a transform's inverse. This rotates the normal by
    /// the inverse quaternion, multiplies the normal by the scale, then normalizes.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="normal">normal</param>
    /// <returns>normal</returns>
    public static Vec3 InvMulNormal(in Transform3 transform, in Vec3 normal)
    {
        return Vec3.Normalize(transform.scale * Quat.InvMulVector(transform.rotation, normal));
    }

    /// <summary>
    /// Multiplies a point by a transform's inverse. This subtracts the
    /// translation from the point, divides the point by the scale, then rotates
    /// by the negative angle.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="point">point</param>
    /// <returns>point</returns>
    public static Vec3 InvMulPoint(in Transform3 transform, in Vec3 point)
    {
        return Quat.InvMulVector(transform.rotation, (point - transform.location) / transform.scale);
    }

    /// <summary>
    /// Multiplies a vector by a transform's inverse. This divides the vector by
    /// the scale, then rotates by the negative angle.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="vec">vector</param>
    /// <returns>vector</returns>
    public static Vec3 InvMulVector(in Transform3 transform, in Vec3 vec)
    {
        return Quat.InvMulVector(transform.rotation, vec / transform.scale);
    }

    /// <summary>
    /// Returns the maximum dimension occupied by a transform.
    /// </summary>
    /// <param name="t">transform</param>
    /// <returns>maximum</returns>
    public static float MaxDimension(in Transform3 t)
    {
        Vec3 scl = Vec3.Abs(t.scale);
        return Utils.Max(scl.x, scl.y, scl.z);
    }

    /// <summary>
    /// Returns the minimum dimension occupied by a transform.
    /// </summary>
    /// <param name="t">transform</param>
    /// <returns>minimum</returns>
    public static float MinDimension(in Transform3 t)
    {
        Vec3 scl = Vec3.Abs(t.scale);
        return Utils.Min(scl.x, scl.y, scl.z);
    }

    /// <summary>
    /// Multiplies a direction by a transform. This rotates the direction by the
    /// transform's rotation.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="dir">direction</param>
    /// <returns>direction</returns>
    public static Vec3 MulDir(in Transform3 transform, in Vec3 dir)
    {
        return Quat.MulVector(transform.rotation, dir);
    }

    /// <summary>
    /// Multiplies a normal by a transform. This divides the normal by the
    /// transform's scale, rotates it by the transform's rotation, then
    /// normalizes the result.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="normal">normal</param>
    /// <returns>normal</returns>
    public static Vec3 MulNormal(in Transform3 transform, in Vec3 normal)
    {
        return Vec3.Normalize(Quat.MulVector(transform.rotation, normal / transform.scale));
    }

    /// <summary>
    /// Multiplies a point by a transform. This rotates the point, multiplies the
    /// point by the scale, then adds the translation.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="point">point</param>
    /// <returns>point</returns>
    public static Vec3 MulPoint(in Transform3 transform, in Vec3 point)
    {
        return transform.location + transform.scale * Quat.MulVector(transform.rotation, point);
    }

    /// <summary>
    /// Multiplies a vector by a transform. This rotates the vector by the
    /// transform's rotation and then multiplies it by the transform's scale.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="vec">vector</param>
    /// <returns>vector</returns>
    public static Vec3 MulVector(in Transform3 transform, in Vec3 vec)
    {
        return transform.scale * Quat.MulVector(transform.rotation, vec);
    }

    /// <summary>
    /// Converts a transform to two axes, which in turn may constitute a rotation
    /// matrix.
    ///
    /// Returns a named value tuple containing the right, forward and up axes.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>axes</returns>
    public static (Vec3 right, Vec3 forward, Vec3 up) ToAxes(in Transform3 tr)
    {
        return Quat.ToAxes(tr.rotation);
    }

    /// <summary>
    /// Returns a string representation of a transform.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Transform3 tr, in int places = 4)
    {
        return Transform3.ToString(new StringBuilder(354), tr, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a transform to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="tr">transform</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(
        in StringBuilder sb,
        in Transform3 tr,
        in int places = 4)
    {
        sb.Append("{ location: ");
        Vec3.ToString(sb, tr.location, places);
        sb.Append(", rotation: ");
        Quat.ToString(sb, tr.rotation, places);
        sb.Append(", scale: ");
        Vec3.ToString(sb, tr.scale, places);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns an identity transform.
    /// </summary>
    /// <value>identity</value>
    public static Transform3 Identity
    {
        get
        {
            return new Transform3(Vec3.Zero, Quat.Identity, Vec3.One);
        }
    }
}