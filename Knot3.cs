using System;
using System.Text;

/// <summary>
/// Organizes the points that shape a cubic Bezier curve into a coordinate (or anchor
/// point), fore handle (the following control point) and rear handle (the
/// preceding control point).
/// </summary>
[Serializable]
public class Knot3
{
    /// <summary>
    /// The spatial coordinate of the knot.
    /// </summary>
    protected Vec3 coord;

    /// <summary>
    /// The handle that warps the curve segment heading away from the knot
    /// along the direction of the curve.
    /// </summary>
    protected Vec3 foreHandle;

    /// <summary>
    /// The handle that warps the curve segment heading towards the knot along
    /// the direction of the curve.
    /// </summary>
    protected Vec3 rearHandle;

    /// <summary>
    /// The spatial coordinate of the knot.
    /// </summary>
    /// <value>coordinate</value>
    public Vec3 Coord
    {
        get
        {
            return this.coord;
        }

        set
        {
            this.coord = value;
        }
    }

    /// <summary>
    /// The handle that warps the curve segment heading away from the knot
    /// along the direction of the curve.
    /// </summary>
    /// <value>fore handle</value>
    public Vec3 ForeHandle
    {
        get
        {
            return this.foreHandle;
        }

        set
        {
            this.foreHandle = value;
        }
    }

    /// <summary>
    /// The handle that warps the curve segment heading towards the knot along
    /// the direction of the curve.
    /// </summary>
    /// <value>rear handle</value>
    public Vec3 RearHandle
    {
        get
        {
            return this.rearHandle;
        }

        set
        {
            this.rearHandle = value;
        }
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public Knot3()
    {
        /// Knots do not implement Equatable because, as classes, they are
        /// nullable and passed by reference. It is better to defer to 
        /// reference equality. They do not implement Comparable because
        /// it is ambiguous whether two knots with the same coordinate but
        /// different handles are equal or unequal.

        this.coord = Vec3.Zero;
        this.foreHandle = Vec3.Zero;
        this.rearHandle = Vec3.Zero;
    }

    /// <summary>
    /// Creates a knot from a coordinate. The forehandle and rearhandle are
    /// offset by a small amount.
    /// </summary>
    /// <param name="coord">coordinate</param>
    public Knot3(in Vec3 coord)
    {
        this.coord = coord;
        Vec3 eps = Vec3.CopySign(Utils.Epsilon, this.coord);
        this.foreHandle = this.coord + eps;
        this.rearHandle = this.coord - eps;
    }

    /// <summary>
    /// Creates a knot from a series of vectors.
    /// </summary>
    /// <param name="coord">coordinate</param>
    /// <param name="foreHandle">fore handle</param>
    /// <param name="rearHandle">rear handle</param>
    public Knot3(in Vec3 coord, in Vec3 foreHandle, in Vec3 rearHandle)
    {
        this.coord = coord;
        this.foreHandle = foreHandle;
        this.rearHandle = rearHandle;
    }

    /// <summary>
    /// Creates a knot from real numbers. The forehandle and rearhandle are
    /// offset by a small amount.
    /// </summary>
    /// <param name="xCo">x coordinate</param>
    /// <param name="yCo">y coordinate</param>
    /// <param name="zCo">y coordinate</param>
    public Knot3(in float xCo, in float yCo, in float zCo = 0.0f)
    {
        float xEps = Utils.CopySign(Utils.Epsilon, xCo);
        float yEps = Utils.CopySign(Utils.Epsilon, yCo);
        float zEps = Utils.CopySign(Utils.Epsilon, zCo);

        this.Set(
            xCo, yCo, zCo,

            xCo + xEps,
            yCo + yEps,
            zCo + zEps,

            xCo - xEps,
            yCo - yEps,
            zCo - zEps);
    }

    /// <summary>
    /// Creates a knot from real numbers.
    /// </summary>
    /// <param name="xCo">x coordinate</param>
    /// <param name="yCo">y coordinate</param>
    /// <param name="zCo">y coordinate</param>
    /// <param name="xFh">fore handle x</param>
    /// <param name="yFh">fore handle y</param>
    /// <param name="zFh">fore handle z</param>
    /// <param name="xRh">rear handle x</param>
    /// <param name="yRh">rear handle y</param>
    /// <param name="zRh">rear handle z</param>
    public Knot3(
        in float xCo, in float yCo, in float zCo,
        in float xFh, in float yFh, in float zFh,
        in float xRh, in float yRh, in float zRh)
    {
        this.Set(
            xCo, yCo, zCo,
            xFh, yFh, zFh,
            xRh, yRh, zRh);
    }

    /// <summary>
    /// Returns the knot's hash code based on those of its three constituent
    /// vectors.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.coord.GetHashCode();
            hash = hash * Utils.HashMul ^ this.foreHandle.GetHashCode();
            hash = hash * Utils.HashMul ^ this.rearHandle.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this knot.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Knot3.ToString(this);
    }

    /// <summary>
    /// Adopts the fore handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot3 AdoptForeHandle(in Knot3 source)
    {
        this.foreHandle = this.coord + (source.foreHandle - source.coord);

        return this;
    }

    /// <summary>
    /// Adopts the fore handle and rear handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot3 AdoptHandles(in Knot3 source)
    {
        this.AdoptForeHandle(source);
        this.AdoptRearHandle(source);

        return this;
    }

    /// <summary>
    /// Adopts the rear handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot3 AdoptRearHandle(in Knot3 source)
    {
        this.rearHandle = this.coord + (source.rearHandle - source.coord);

        return this;
    }

    /// <summary>
    /// Aligns this knot's fore handle to its rear handle while preserving
    /// magnitude.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 AlignHandlesBackward()
    {
        Vec3 rDir = this.rearHandle - this.coord;
        float rMagSq = Vec3.MagSq(rDir);
        if (rMagSq > 0.0f)
        {
            float flipRescale = -Vec3.DistEuclidean(this.foreHandle, this.coord) / MathF.Sqrt(rMagSq);
            this.foreHandle = this.coord + (flipRescale * rDir);
        }

        return this;
    }

    /// <summary>
    /// Aligns this knot's rear handle to its fore handle while preserving
    /// magnitude.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 AlignHandlesForward()
    {
        Vec3 fDir = this.foreHandle - this.coord;
        float fMagSq = Vec3.MagSq(fDir);
        if (fMagSq > 0.0f)
        {
            float flipRescale = -Vec3.DistEuclidean(this.rearHandle, this.coord) / MathF.Sqrt(fMagSq);
            this.rearHandle = this.coord + (flipRescale * fDir);
        }

        return this;
    }

    /// <summary>
    /// Negates the x component of a knot's
    /// coordinate and handles, flipping it
    /// about the x axis.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 FlipX()
    {
        this.coord = Vec3.FlipX(this.coord);
        this.foreHandle = Vec3.FlipX(this.foreHandle);
        this.rearHandle = Vec3.FlipX(this.rearHandle);

        return this;
    }

    /// <summary>
    /// Negates the y component of a knot's
    /// coordinate and handles, flipping it
    /// about the y axis.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 FlipY()
    {
        this.coord = Vec3.FlipY(this.coord);
        this.foreHandle = Vec3.FlipY(this.foreHandle);
        this.rearHandle = Vec3.FlipY(this.rearHandle);

        return this;
    }

    /// <summary>
    /// Negates the z component of a knot's
    /// coordinate and handles, flipping it
    /// about the z axis.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 FlipZ()
    {
        this.coord = Vec3.FlipZ(this.coord);
        this.foreHandle = Vec3.FlipZ(this.foreHandle);
        this.rearHandle = Vec3.FlipZ(this.rearHandle);

        return this;
    }

    /// <summary>
    /// Sets the forward-facing handle to mirror the rear-facing handle: the
    /// fore will have the same magnitude and negated direction of the rear.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 MirrorHandlesBackward()
    {
        this.foreHandle = this.coord - (this.rearHandle - this.coord);
        return this;
    }

    /// <summary>
    /// Sets the rear-facing handle to mirror the forward-facing handle: the
    /// rear will have the same magnitude and negated direction of the fore.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 MirrorHandlesForward()
    {
        this.rearHandle = this.coord - (this.foreHandle - this.coord);

        return this;
    }

    /// <summary>
    /// Relocates the knot to a new location while maintaining the relationship
    /// between the central coordinate and its two handles.
    /// </summary>
    /// <param name="v">coordinate</param>
    /// <returns>this knot</returns>
    public Knot3 Relocate(in Vec3 v)
    {
        this.foreHandle -= this.coord;
        this.rearHandle -= this.coord;

        this.coord = v;

        this.foreHandle += this.coord;
        this.rearHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Reverses the knot's direction by swapping the fore and rear handles.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot3 Reverse()
    {
        (this.rearHandle, this.foreHandle) = (this.foreHandle, this.rearHandle);
        return this;
    }

    /// <summary>
    /// Rotates this knot by a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>this knot</returns>
    public Knot3 Rotate(in Quat q)
    {
        this.coord = Quat.MulVector(q, this.coord);
        this.foreHandle = Quat.MulVector(q, this.foreHandle);
        this.rearHandle = Quat.MulVector(q, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Rotates this knot around an arbitrary axis by an angle in radians.
    /// </summary>
    /// <param name="radians">angle in radians</param>
    /// <param name="axis">axis of rotation</param>
    /// <returns>this knot</returns>
    public Knot3 Rotate(in float radians, in Vec3 axis)
    {
        return this.Rotate(MathF.Cos(radians), MathF.Sin(radians), axis);
    }

    /// <summary>
    /// Rotates this knot around an axis by an angle in radians. The axis is
    /// assumed to be of unit length.
    ///
    /// Accepts pre-calculated sine and cosine of an angle, so that collections
    /// of knots can be efficiently rotated without repeatedly calling cos and
    /// sin.
    /// </summary>
    /// <param name="cosa">cosine of the angle</param>
    /// <param name="sina">sine of the angle</param>
    /// <param name="axis">axis of rotation</param>
    /// <returns>this knot</returns>
    public Knot3 Rotate(in float cosa, in float sina, in Vec3 axis)
    {
        this.coord = Vec3.Rotate(this.coord, cosa, sina, axis);
        this.foreHandle = Vec3.Rotate(this.foreHandle, cosa, sina, axis);
        this.rearHandle = Vec3.Rotate(this.rearHandle, cosa, sina, axis);

        return this;
    }

    /// <summary>
    /// Rotates this knot's fore handle by a quaternion with its coordinate
    /// serving as a pivot.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>this knot</returns>
    public Knot3 RotateForeHandle(in Quat q)
    {
        this.foreHandle -= this.coord;
        this.foreHandle = Quat.MulVector(q, this.foreHandle);
        this.foreHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Rotates this knot's handles by a quaternion with its coordinate serving
    /// as a pivot.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>this knot</returns>
    public Knot3 RotateHandles(in Quat q)
    {
        this.RotateForeHandle(q);
        this.RotateRearHandle(q);

        return this;
    }

    /// <summary>
    /// Rotates this knot's rear handle by a quaternion with its coordinate
    /// serving as a pivot.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>this knot</returns>
    public Knot3 RotateRearHandle(in Quat q)
    {
        this.rearHandle -= this.coord;
        this.rearHandle = Quat.MulVector(q, this.rearHandle);
        this.rearHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Rotates this knot around the x axis by an angle in radians.
    /// </summary>
    /// <param name="radians">radians</param>
    /// <returns>this knot</returns>
    public Knot3 RotateX(in float radians)
    {
        return this.RotateX(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a knot around the x axis. 
    ///
    /// Accepts calculated sine and cosine of an angle, so that collections of
    /// knots can be efficiently rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="cosa">cosine of the angle</param>
    /// <param name="sina">sine of the angle</param>
    /// <returns>this knot</returns>
    public Knot3 RotateX(in float cosa, in float sina)
    {
        this.coord = Vec3.RotateX(this.coord, cosa, sina);
        this.foreHandle = Vec3.RotateX(this.foreHandle, cosa, sina);
        this.rearHandle = Vec3.RotateX(this.rearHandle, cosa, sina);

        return this;
    }

    /// <summary>
    /// Rotates this knot around the y axis by an angle in radians.
    /// </summary>
    /// <param name="radians">radians</param>
    /// <returns>this knot</returns>
    public Knot3 RotateY(in float radians)
    {
        return this.RotateY(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a knot around the y axis. 
    ///
    /// Accepts calculated sine and cosine of an angle, so that collections of
    /// knots can be efficiently rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="cosa">cosine of the angle</param>
    /// <param name="sina">sine of the angle</param>
    /// <returns>this knot</returns>
    public Knot3 RotateY(in float cosa, in float sina)
    {
        this.coord = Vec3.RotateY(this.coord, cosa, sina);
        this.foreHandle = Vec3.RotateY(this.foreHandle, cosa, sina);
        this.rearHandle = Vec3.RotateY(this.rearHandle, cosa, sina);

        return this;
    }

    /// <summary>
    /// Rotates this knot around the z axis by an angle in radians.
    /// </summary>
    /// <param name="radians">radians</param>
    /// <returns>this knot</returns>
    public Knot3 RotateZ(in float radians)
    {
        return this.RotateZ(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates a knot around the z axis. 
    ///
    /// Accepts calculated sine and cosine of an angle, so that collections of
    /// knots can be efficiently rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="cosa">cosine of the angle</param>
    /// <param name="sina">sine of the angle</param>
    /// <returns>this knot</returns>
    public Knot3 RotateZ(in float cosa, in float sina)
    {
        this.coord = Vec3.RotateZ(this.coord, cosa, sina);
        this.foreHandle = Vec3.RotateZ(this.foreHandle, cosa, sina);
        this.rearHandle = Vec3.RotateZ(this.rearHandle, cosa, sina);

        return this;
    }

    /// <summary>
    /// Scales this knot by a factor.
    /// </summary>
    /// <param name="scale">factor</param>
    /// <returns>this knot</returns>
    public Knot3 Scale(in float scale)
    {
        this.coord *= scale;
        this.foreHandle *= scale;
        this.rearHandle *= scale;

        return this;
    }

    /// <summary>
    /// Scales this knot by a non uniform scalar.
    /// </summary>
    /// <param name="scale">non uniform scalar</param>
    /// <returns>this knot</returns>
    public Knot3 Scale(in Vec3 scale)
    {
        this.coord *= scale;
        this.foreHandle *= scale;
        this.rearHandle *= scale;

        return this;
    }

    /// <summary>
    /// Scales the fore handle by a factor.
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>this knot</returns>
    public Knot3 ScaleForeHandleBy(in float scalar)
    {
        this.foreHandle -= this.coord;
        this.foreHandle *= scalar;
        this.foreHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Scales the fore handle to a magnitude.
    /// </summary>
    /// <param name="magnitude">magnitude</param>
    /// <returns>this knot</returns>
    public Knot3 ScaleForeHandleTo(in float magnitude)
    {
        this.foreHandle -= this.coord;
        this.foreHandle = Vec3.Rescale(this.foreHandle, magnitude);
        this.foreHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Scales both the fore and rear handle by a factor.
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>this knot</returns>
    public Knot3 ScaleHandlesBy(in float scalar)
    {
        this.ScaleForeHandleBy(scalar);
        this.ScaleRearHandleBy(scalar);

        return this;
    }

    /// <summary>
    /// Scales both the fore and rear handle to a magnitude.
    /// </summary>
    /// <param name="magnitude">magnitude</param>
    /// <returns>the knot</returns>
    public Knot3 ScaleHandlesTo(in float magnitude)
    {
        this.ScaleForeHandleTo(magnitude);
        this.ScaleRearHandleTo(magnitude);

        return this;
    }

    /// <summary>
    /// Scales the rear handle by a factor.
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>this knot</returns>
    public Knot3 ScaleRearHandleBy(in float scalar)
    {
        this.rearHandle -= this.coord;
        this.rearHandle *= scalar;
        this.rearHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Scales the rear handle to a magnitude.
    /// </summary>
    /// <param name="magnitude">magnitude</param>
    /// <returns>this knot</returns>
    public Knot3 ScaleRearHandleTo(in float magnitude)
    {
        this.rearHandle -= this.coord;
        this.rearHandle = Vec3.Rescale(this.rearHandle, magnitude);
        this.rearHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Sets a knot from real numbers.
    /// </summary>
    /// <param name="xCo">x coordinate</param>
    /// <param name="yCo">y coordinate</param>
    /// <param name="zCo">y coordinate</param>
    /// <param name="xFh">fore handle x</param>
    /// <param name="yFh">fore handle y</param>
    /// <param name="zFh">fore handle z</param>
    /// <param name="xRh">rear handle x</param>
    /// <param name="yRh">rear handle y</param>
    /// <param name="zRh">rear handle z</param>
    /// <returns>this knot</returns>
    public Knot3 Set(
        in float xCo, in float yCo, in float zCo,
        in float xFh, in float yFh, in float zFh,
        in float xRh, in float yRh, in float zRh)
    {
        this.coord = new Vec3(xCo, yCo, zCo);
        this.foreHandle = new Vec3(xFh, yFh, zFh);
        this.rearHandle = new Vec3(xRh, yRh, zRh);

        return this;
    }

    /// <summary>
    /// Transforms this knot by a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>this knot</returns>
    public Knot3 Transform(in Mat4 m)
    {
        this.coord = Mat4.MulPoint(m, this.coord);
        this.foreHandle = Mat4.MulPoint(m, this.foreHandle);
        this.rearHandle = Mat4.MulPoint(m, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Transforms this knot by a transform.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>this knot</returns>
    public Knot3 Transform(in Transform3 tr)
    {
        this.coord = Transform3.MulPoint(tr, this.coord);
        this.foreHandle = Transform3.MulPoint(tr, this.foreHandle);
        this.rearHandle = Transform3.MulPoint(tr, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Translates this knot by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this knot</returns>
    public Knot3 Translate(in Vec3 v)
    {
        this.coord += v;
        this.foreHandle += v;
        this.rearHandle += v;

        return this;
    }

    /// <summary>
    /// Converts a vector to a knot.
    /// </summary>
    /// <param name="v">vector</param>
    public static implicit operator Knot3(in Vec3 v)
    {
        return new Knot3(v);
    }

    /// <summary>
    /// Promotes a 2D knot to a 3D knot.
    /// </summary>
    /// <param name="kn">knot</param>
    public static implicit operator Knot3(in Knot2 kn)
    {
        return new Knot3(kn.Coord, kn.ForeHandle, kn.RearHandle);
    }

    /// <summary>
    /// Evaluates a point between two knots given an origin, destination and a
    /// step.
    /// </summary>
    /// <param name="a">origin</param>
    /// <param name="b">destination</param>
    /// <param name="step">step</param>
    /// <returns>evaluation</returns>
    public static Vec3 BezierPoint(in Knot3 a, in Knot3 b, in float step)
    {
        return Vec3.BezierPoint(
            a.coord, a.foreHandle,
            b.rearHandle, b.coord,
            step);
    }

    /// <summary>
    /// Evaluates a tangent given an origin, a destination knot and a step.
    /// </summary>
    /// <param name="a">origin</param>
    /// <param name="b">destination</param>
    /// <param name="step">step</param>
    /// <returns>evaluation</returns>
    public static Vec3 BezierTangent(in Knot3 a, in Knot3 b, in float step)
    {
        return Vec3.BezierTangent(
            a.coord, a.foreHandle,
            b.rearHandle, b.coord,
            step);
    }

    /// <summary>
    /// Evaluates a normalized tangent given an origin, a destination knot and a
    /// step.
    /// </summary>
    /// <param name="a">origin</param>
    /// <param name="b">destination</param>
    /// <param name="step">step</param>
    /// <returns>evaluation</returns>
    public static Vec3 BezierTanUnit(in Knot3 a, in Knot3 b, in float step)
    {
        return Vec3.BezierTanUnit(
            a.coord, a.foreHandle,
            b.rearHandle, b.coord,
            step);
    }

    /// <summary>
    /// Gets the fore handle of a knot as a direction, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>fore handle vector</returns>
    public static Vec3 ForeDir(in Knot3 kn)
    {
        return Vec3.Normalize(Knot3.ForeVec(kn));
    }

    /// <summary>
    /// Returns the magnitude of the knot's fore handle, i.e., the Euclidean
    /// distance between the fore handle and the coordinate.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>magnitude</returns>
    public static float ForeMag(in Knot3 kn)
    {
        return Vec3.DistEuclidean(kn.foreHandle, kn.coord);
    }

    /// <summary>
    /// Gets the fore handle of a knot as a vector, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>fore handle vector</returns>
    public static Vec3 ForeVec(in Knot3 kn)
    {
        return kn.foreHandle - kn.coord;
    }

    /// <summary>
    /// Gets the rear handle of a knot as a direction, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>rear handle vector</returns>
    public static Vec3 RearDir(in Knot3 kn)
    {
        return Vec3.Normalize(Knot3.RearVec(kn));
    }

    /// <summary>
    /// Returns the magnitude of the knot's rear handle, i.e., the Euclidean
    /// distance between the rear handle and the coordinate.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>magnitude</returns>
    public static float RearMag(in Knot3 kn)
    {
        return Vec3.DistEuclidean(kn.rearHandle, kn.coord);
    }

    /// <summary>
    /// Gets the rear handle of a knot as a vector, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>rear handle vector</returns>
    public static Vec3 RearVec(in Knot3 kn)
    {
        return kn.rearHandle - kn.coord;
    }

    /// <summary>
    /// Smooths the handles of a knot in a curve.
    /// Returns the carry for the next iteration.
    /// </summary>
    /// <param name="prev">previous knot</param>
    /// <param name="curr">current knot</param>
    /// <param name="next">next knot</param>
    /// <param name="carry">carry</param>
    /// <returns>carry next</returns>
    public static Vec3 SmoothHandles(
        in Knot3 prev,
        in Knot3 curr,
        in Knot3 next,
        in Vec3 carry)
    {
        Vec3 coCurr = curr.coord;
        Vec3 coPrev = prev.coord;
        Vec3 coNext = next.coord;

        float xBack = coPrev.X - coCurr.X;
        float yBack = coPrev.Y - coCurr.Y;
        float zBack = coPrev.Z - coCurr.Z;

        float xFore = coNext.X - coCurr.X;
        float yFore = coNext.Y - coCurr.Y;
        float zFore = coNext.Z - coCurr.Z;

        float bmSq = xBack * xBack +
            yBack * yBack +
            zBack * zBack;
        float bmInv = bmSq != 0.0f ? 1.0f / MathF.Sqrt(bmSq) : 0.0f;

        float fmSq = xFore * xFore +
            yFore * yFore +
            zFore * zFore;
        float fmInv = fmSq != 0.0f ? 1.0f / MathF.Sqrt(fmSq) : 0.0f;

        float xDir = carry.X + xBack * bmInv - xFore * fmInv;
        float yDir = carry.Y + yBack * bmInv - yFore * fmInv;
        float zDir = carry.Z + zBack * bmInv - zFore * fmInv;

        float dmSq = xDir * xDir +
            yDir * yDir +
            zDir * zDir;
        float rescl = dmSq != 0.0f ? Utils.OneThird / MathF.Sqrt(dmSq) : 0.0f;

        float xCarry = xDir * rescl;
        float yCarry = yDir * rescl;
        float zCarry = zDir * rescl;

        float bMag = bmSq * bmInv;
        curr.rearHandle = new Vec3(
            coCurr.X + bMag * xCarry,
            coCurr.Y + bMag * yCarry,
            coCurr.Z + bMag * zCarry);

        float fMag = fmSq * fmInv;
        curr.foreHandle = new Vec3(
            coCurr.X - fMag * xCarry,
            coCurr.Y - fMag * yCarry,
            coCurr.Z - fMag * zCarry);

        return new Vec3(
            xCarry,
            yCarry,
            zCarry);
    }

    /// <summary>
    /// Smooths the handles of the first knot
    /// in a curve. Returns the carry for the
    /// next iteration.
    /// </summary>
    /// <param name="curr">current knot</param>
    /// <param name="next">next knot</param>
    /// <param name="carry">carry</param>
    /// <returns>carry next</returns>
    public static Vec3 SmoothHandlesFirst(
        in Knot3 curr,
        in Knot3 next,
        in Vec3 carry)
    {
        Vec3 coCurr = curr.coord;
        Vec3 coNext = next.coord;

        float xBack = -coCurr.X;
        float yBack = -coCurr.Y;
        float zBack = -coCurr.Z;

        float xFore = coNext.X - coCurr.X;
        float yFore = coNext.Y - coCurr.Y;
        float zFore = coNext.Z - coCurr.Z;

        float bmSq = xBack * xBack +
            yBack * yBack +
            zBack * zBack;
        float bmInv = bmSq > 0.0f ? 1.0f / MathF.Sqrt(bmSq) : 0.0f;

        float fmSq = xFore * xFore +
            yFore * yFore +
            zFore * zFore;
        float fmInv = fmSq > 0.0f ? 1.0f / MathF.Sqrt(fmSq) : 0.0f;

        float xDir = carry.X + xBack * bmInv - xFore * fmInv;
        float yDir = carry.Y + yBack * bmInv - yFore * fmInv;
        float zDir = carry.Z + zBack * bmInv - zFore * fmInv;

        float dmSq = xDir * xDir +
            yDir * yDir +
            zDir * zDir;
        float rescl = dmSq > 0.0f ? Utils.OneThird / MathF.Sqrt(dmSq) : 0.0f;

        float xCarry = xDir * rescl;
        float yCarry = yDir * rescl;
        float zCarry = zDir * rescl;

        float fMag = fmSq * fmInv;
        curr.foreHandle = new Vec3(
            coCurr.X - fMag * xCarry,
            coCurr.Y - fMag * yCarry,
            coCurr.Z - fMag * zCarry);

        return new Vec3(
            xCarry,
            yCarry,
            zCarry);
    }

    /// <summary>
    /// Smooths the handles of the last knot
    /// in a curve. Returns the carry for the
    /// next iteration.
    /// </summary>
    /// <param name="prev">previous knot</param>
    /// <param name="curr">current knot</param>
    /// <param name="carry">carry</param>
    /// <returns>carry next</returns>
    public static Vec3 SmoothHandlesLast(
        in Knot3 prev,
        in Knot3 curr,
        in Vec3 carry)
    {
        Vec3 coCurr = curr.coord;
        Vec3 coPrev = prev.coord;

        float xBack = coPrev.X - coCurr.X;
        float yBack = coPrev.Y - coCurr.Y;
        float zBack = coPrev.Z - coCurr.Z;

        float xFore = -coCurr.X;
        float yFore = -coCurr.Y;
        float zFore = -coCurr.Z;

        float bmSq = xBack * xBack +
            yBack * yBack +
            zBack * zBack;
        float bmInv = bmSq > 0.0f ? 1.0f / MathF.Sqrt(bmSq) : 0.0f;

        float fmSq = xFore * xFore +
            yFore * yFore +
            zFore * zFore;
        float fmInv = fmSq > 0.0f ? 1.0f / MathF.Sqrt(fmSq) : 0.0f;

        float xDir = carry.X + xBack * bmInv - xFore * fmInv;
        float yDir = carry.Y + yBack * bmInv - yFore * fmInv;
        float zDir = carry.Z + zBack * bmInv - zFore * fmInv;

        float dmSq = xDir * xDir +
            yDir * yDir +
            zDir * zDir;
        float rescl = dmSq > 0.0f ? Utils.OneThird / MathF.Sqrt(dmSq) : 0.0f;

        float xCarry = xDir * rescl;
        float yCarry = yDir * rescl;
        float zCarry = zDir * rescl;

        float bMag = bmSq * bmInv;
        curr.rearHandle = new Vec3(
            coCurr.X + bMag * xCarry,
            coCurr.Y + bMag * yCarry,
            coCurr.Z + bMag * zCarry);

        return new Vec3(
            xCarry,
            yCarry,
            zCarry);
    }

    /// <summary>
    /// Returns a string representation of a knot.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <param name="places">places</param>
    /// <returns>string</returns>
    public static string ToString(in Knot3 kn, in int places = 4)
    {
        return Knot3.ToString(new StringBuilder(512), kn, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a knot to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="kn">knot</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Knot3 kn, in int places = 4)
    {
        sb.Append("{ coord: ");
        Vec3.ToString(sb, kn.coord, places);
        sb.Append(", foreHandle: ");
        Vec3.ToString(sb, kn.foreHandle, places);
        sb.Append(", rearHandle: ");
        Vec3.ToString(sb, kn.rearHandle, places);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }
}