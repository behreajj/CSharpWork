using System;
using System.Text;

/// <summary>
/// Facilitates 2D affine transformations for entities.
/// </summary>
[Serializable]
public class Transform2
{
    /// <summary>
    /// The transform's location.
    /// </summary>
    protected Vec2 location = Vec2.Zero;

    /// <summary>
    /// The transform's rotation in radians.
    /// </summary>
    protected float rotation = 0.0f;

    /// <summary>
    /// The transform's scale.
    /// </summary>
    protected Vec2 scale = Vec2.One;

    /// <summary>
    /// The transform's forward axis.
    /// </summary>
    /// <value>forward</value>
    public Vec2 Forward
    {
        get
        {
            return Vec2.FromPolar (this.rotation + Utils.HalfPi);
        }
    }

    /// <summary>
    /// The transform's location.
    /// </summary>
    /// <value>location</value>
    public Vec2 Location
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
    public Vec2 Right
    {
        get
        {
            return Vec2.FromPolar (this.rotation);
        }
    }

    /// <summary>
    /// The transform's rotation in radians.
    /// </summary>
    /// <value>rotation</value>
    public float Rotation
    {
        get
        {
            return this.rotation;
        }

        set
        {
            this.rotation = value;
        }
    }

    /// <summary>
    /// The transform's scale.
    /// </summary>
    /// <value>scale</value>
    public Vec2 Scale
    {
        get
        {
            return this.scale;
        }

        set
        {
            if (Vec2.All (value)) { this.scale = value; }
        }
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Transform2 ( ) { }

    /// <summary>
    /// Creates a transform from a location, rotation and scale.
    /// </summary>
    /// <param name="location">location</param>
    /// <param name="rotation">rotation</param>
    /// <param name="scale">scale</param>
    public Transform2 (in Vec2 location, in float rotation, in Vec2 scale)
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
    /// <param name="rotation">rotation</param>
    /// <param name="width">x scale</param>
    /// <param name="height">y scale</param>
    public Transform2 ( //
        in float x, in float y, //
        in float rotation, //
        in float width, in float height)
    {
        this.Location = new Vec2 (x, y);
        this.Rotation = rotation;
        this.Scale = new Vec2 (width, height);
    }

    /// <summary>
    /// Returns a hash code representing this transform.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.location.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.rotation.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.scale.GetHashCode ( );
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this transform.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return Transform2.ToString (this);
    }

    /// <summary>
    /// Flips the transform's scale on the horizontal axis, i.e., negates the
    /// scale's x component.
    /// </summary>
    /// <returns>this transform</returns>
    public Transform2 FlipX ( )
    {
        this.Scale = new Vec2 (-this.scale.x, this.scale.y);
        return this;
    }

    /// <summary>
    /// Flips the transform's scale on the vertical axis, i.e., negates the
    /// scale's y component.
    /// </summary>
    /// <returns>this transform</returns>
    public Transform2 FlipY ( )
    {
        this.Scale = new Vec2 (this.scale.x, -this.scale.y);
        return this;
    }

    /// <summary>
    /// Moves the transform by a direction to a new location in the global
    /// coordinate system.
    /// </summary>
    /// <param name="v">direction</param>
    /// <returns>this transform</returns>
    public Transform2 MoveByGlobal (in Vec2 v)
    {
        this.Location += v;
        return this;
    }

    /// <summary>
    /// Moves the transform by a direction rotated according to the transform's
    /// rotation.
    /// </summary>
    /// <param name="v">direction</param>
    /// <returns>transform</returns>
    public Transform2 MoveByLocal (in Vec2 v)
    {
        this.Location += Transform2.MulDir (this, v);
        return this;
    }

    /// <summary>
    /// Eases the transform to a location by a step.
    /// </summary>
    /// <param name="v">direction</param>
    /// <param name="step">step</param>
    /// <returns>this transform</returns>
    public Transform2 MoveTo (in Vec2 v, in Vec2 step)
    {
        this.Location = Vec2.Mix (this.location, v, step);
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
    public Transform2 MoveTo (in Vec2 v, in Vec2 step, in Func<Vec2, Vec2, Vec2, Vec2> easing)
    {
        Vec2 t = easing (this.location, v, step);
        this.Location = Vec2.Mix (this.location, v, t);
        return this;
    }

    /// <summary>
    /// Rotates the transform to a new orientation by a step.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <param name="step">step</param>
    /// <returns>this transform</returns>
    public Transform2 RotateTo (in float radians, in float step = 1.0f)
    {
        this.Rotation = Utils.LerpAngleNear (this.rotation, radians, step);
        return this;
    }

    /// <summary>
    /// Rotates this transform around the z axis by an angle in radians.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this transform</returns>
    public Transform2 RotateZ (in float radians)
    {
        this.Rotation += radians;
        return this;
    }

    /// <summary>
    /// Scales the transform by a uniform scalar, i.e., multiplies the scale by
    /// the argument.
    /// </summary>
    /// <param name="v">uniform scalar</param>
    /// <returns>this transform</returns>    
    public Transform2 ScaleBy (in float v)
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
    public Transform2 ScaleBy (in Vec2 v)
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
    public Transform2 ScaleTo (in Vec2 v, in Vec2 step)
    {
        this.Scale = Vec2.Mix (this.scale, Vec2.CopySign (v, this.scale), step);
        return this;
    }

    /// <summary>
    /// Eases the transform to a scale by a step according to an easing
    /// function.
    /// </summary>
    /// <param name="v">nonuniform scale</param>
    /// <param name="step">step</param>
    /// <param name="easing">easing function</param>
    /// <returns>this transform</returns>
    public Transform2 ScaleTo (in Vec2 v, in Vec2 step, in Func<Vec2, Vec2, Vec2, Vec2> easing)
    {
        Vec2 s = Vec2.CopySign (v, this.scale);
        Vec2 t = easing (this.scale, s, step);
        this.Scale = Vec2.Mix (this.scale, s, t);
        return this;
    }

    /// <summary>
    /// Multiplies a direction by a transform's inverse. This rotates the
    /// direction by the transform's negative angle.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="dir">direction</param>
    /// <returns>direction</returns>
    public static Vec2 InvMulDir (in Transform2 transform, in Vec2 dir)
    {
        return Vec2.RotateZ (dir, -transform.rotation);
    }

    /// <summary>
    /// Multiplies a point by a transform's inverse. This subtracts the
    /// translation from the point, divides the point by the scale, then rotates
    /// by the negative angle.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="point">point</param>
    /// <returns>point</returns>
    public static Vec2 InvMulPoint (in Transform2 transform, in Vec2 point)
    {
        return Vec2.RotateZ ((point - transform.location) / transform.scale, -transform.rotation);
    }

    /// <summary>
    /// Multiplies a vector by a transform's inverse. This divides the vector by
    /// the scale, then rotates by the negative angle.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="vec">vector</param>
    /// <returns>vector</returns>
    public static Vec2 InvMulVector (in Transform2 transform, in Vec2 vec)
    {
        return Vec2.RotateZ (vec / transform.scale, -transform.rotation);
    }

    /// <summary>
    /// Returns the maximum dimension occupied by a transform.
    /// </summary>
    /// <param name="t">transform</param>
    /// <returns>maximum</returns>
    public static float MaxDimension (in Transform2 t)
    {
        Vec2 scl = Vec2.Abs (t.scale);
        return Utils.Max (scl.x, scl.y);
    }

    /// <summary>
    /// Returns the minimum dimension occupied by a transform.
    /// </summary>
    /// <param name="t">transform</param>
    /// <returns>minimum</returns>
    public static float MinDimension (in Transform2 t)
    {
        Vec2 scl = Vec2.Abs (t.scale);
        return Utils.Min (scl.x, scl.y);
    }

    /// <summary>
    /// Multiplies a direction by a transform. This rotates the direction by the
    /// transform's rotation.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="dir">direction</param>
    /// <returns>direction</returns>
    public static Vec2 MulDir (in Transform2 transform, in Vec2 dir)
    {
        return Vec2.RotateZ (dir, transform.rotation);
    }

    /// <summary>
    /// Multiplies a point by a transform. This rotates the point, multiplies
    /// the point by the scale, then adds the translation.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="point">point</param>
    /// <returns>point</returns>
    public static Vec2 MulPoint (in Transform2 transform, in Vec2 point)
    {
        return transform.location + transform.scale * Vec2.RotateZ (point, transform.rotation);
    }

    /// <summary>
    /// Multiplies a vector by a transform. This rotates the vector by the
    /// transform's rotation and then multiplies it by the transform's scale.
    /// </summary>
    /// <param name="transform">transform</param>
    /// <param name="vec">vector</param>
    /// <returns>vector</returns>
    public static Vec2 MulVector (in Transform2 transform, in Vec2 vec)
    {
        return transform.scale * Vec2.RotateZ (vec, transform.rotation);
    }

    /// <summary>
    /// Converts a transform to two axes, which in turn may constitute a
    /// rotation matrix.
    ///
    /// Returns a named value tuple containing the right and forward axes.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>tuple</returns>
    public static (Vec2 right, Vec2 forward) ToAxes (in Transform2 tr)
    {
        Vec2 r = Vec2.FromPolar (tr.rotation);
        return (right: r, forward: Vec2.PerpendicularCCW (r));
    }

    /// <summary>
    /// Returns a string representation of a transform.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString (in Transform2 tr, in int places = 4)
    {
        return Transform2.ToString (new StringBuilder (160), tr, places).ToString ( );
    }

    /// <summary>
    /// Appends a representation of a transform to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="tr">transform</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString ( //
        in StringBuilder sb, //
        in Transform2 tr, //
        in int places = 4)
    {
        sb.Append ("{ location: ");
        Vec2.ToString (sb, tr.location, places);
        sb.Append (", rotation: ");
        Utils.ToFixed (sb, tr.rotation, places);
        sb.Append (", scale: ");
        Vec2.ToString (sb, tr.scale, places);
        sb.Append (' ');
        sb.Append ('}');
        return sb;
    }

    /// <summary>
    /// Returns an identity transform.
    /// </summary>
    /// <value>identity</value>
    public static Transform2 Identity
    {
        get
        {
            return new Transform2 (Vec2.Zero, 0.0f, Vec2.One);
        }
    }
}