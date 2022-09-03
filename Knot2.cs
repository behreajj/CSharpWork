using System;
using System.Text;

/// <summary>
/// Organizes the points that shape a Bezier curve into a coordinate (or anchor
/// point), fore handle (the following control point) and rear handle (the
/// preceding control point).
/// </summary>
[Serializable]
public class Knot2
{
    /// <summary>
    /// The spatial coordinate of the knot.
    /// </summary>
    protected Vec2 coord;

    /// <summary>
    /// The handle which warps the curve segment heading away from the knot
    /// along the direction of the curve.
    /// </summary>
    protected Vec2 foreHandle;

    /// <summary>
    /// The handle which warps the curve segment heading towards the knot along
    /// the direction of the curve.
    /// </summary>
    protected Vec2 rearHandle;

    /// <summary>
    /// The spatial coordinate of the knot.
    /// </summary>
    /// <value>coordinate</value>
    public Vec2 Coord
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
    /// The handle which warps the curve segment heading away from the knot
    /// along the direction of the curve.
    /// </summary>
    /// <value>fore handle</value>
    public Vec2 ForeHandle
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
    /// The handle which warps the curve segment heading towards the knot along
    /// the direction of the curve.
    /// </summary>
    /// <value>rear handle</value>
    public Vec2 RearHandle
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
    public Knot2()
    {
        /// Knots do not implement Equatable because, as classes, they are
        /// nullable and passed by reference. It is better to defer to 
        /// reference equality. They do not implement Comparable because
        /// it is ambiguous whether two knots with the same coordinate but
        /// different handles are equal or unequal.

        this.coord = Vec2.Zero;
        this.foreHandle = Vec2.Zero;
        this.rearHandle = Vec2.Zero;
    }

    /// <summary>
    /// Creates a knot from a coordinate. The forehandle and rearhandle are
    /// offset by a small amount.
    /// </summary>
    /// <param name="coord">coordinate</param>
    public Knot2(in Vec2 coord)
    {
        this.coord = coord;
        Vec2 eps = Vec2.CopySign(Utils.Epsilon, this.coord);
        this.foreHandle = this.coord + eps;
        this.rearHandle = this.coord - eps;
    }

    /// <summary>
    /// Creates a knot from a series of vectors.
    /// </summary>
    /// <param name="coord">coordinate</param>
    /// <param name="foreHandle">fore handle</param>
    /// <param name="rearHandle">rear handle</param>
    public Knot2(in Vec2 coord, in Vec2 foreHandle, in Vec2 rearHandle)
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
    public Knot2(in float xCo, in float yCo)
    {
        float xEps = Utils.CopySign(Utils.Epsilon, xCo);
        float yEps = Utils.CopySign(Utils.Epsilon, yCo);

        this.Set(
            xCo, yCo,

            xCo + xEps,
            yCo + yEps,

            xCo - xEps,
            yCo - yEps);
    }

    /// <summary>
    /// Creates a knot from real numbers.
    /// </summary>
    /// <param name="xCo">x coordinate</param>
    /// <param name="yCo">y coordinate</param>
    /// <param name="xFh">fore handle x</param>
    /// <param name="yFh">fore handle y</param>
    /// <param name="xRh">rear handle x</param>
    /// <param name="yRh">rear handle y</param>
    public Knot2( //
        in float xCo, in float yCo, //
        in float xFh, in float yFh, //
        in float xRh, in float yRh)
    {
        this.Set(
            xCo, yCo,
            xFh, yFh,
            xRh, yRh);
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
        return Knot2.ToString(this);
    }

    /// <summary>
    /// Adopts the fore handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot2 AdoptForeHandle(in Knot2 source)
    {
        this.foreHandle = this.coord + (source.foreHandle - source.coord);
        return this;
    }

    /// <summary>
    /// Adopts the fore handle and rear handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot2 AdoptHandles(in Knot2 source)
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
    public Knot2 AdoptRearHandle(in Knot2 source)
    {
        this.rearHandle = this.coord + (source.rearHandle - source.coord);
        return this;
    }

    /// <summary>
    /// Aligns this knot's fore handle to its rear handle while preserving
    /// magnitude.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 AlignHandlesBackward()
    {
        Vec2 rDir = this.rearHandle - this.coord;
        float rMagSq = Vec2.MagSq(rDir);
        if (rMagSq > 0.0f)
        {
            float flipRescale = -Vec2.DistEuclidean(this.foreHandle, this.coord) / MathF.Sqrt(rMagSq);
            this.foreHandle = this.coord + (flipRescale * rDir);
        }

        return this;
    }

    /// <summary>
    /// Aligns this knot's rear handle to its fore handle while preserving
    /// magnitude.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 AlignHandlesForward()
    {
        Vec2 fDir = this.foreHandle - this.coord;
        float fMagSq = Vec2.MagSq(fDir);
        if (fMagSq > 0.0f)
        {
            float flipRescale = -Vec2.DistEuclidean(this.rearHandle, this.coord) / MathF.Sqrt(fMagSq);
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
    public Knot2 FlipX()
    {
        this.coord = new Vec2( //
            -this.coord.x,
            this.coord.y);
        this.foreHandle = new Vec2( //
            -this.foreHandle.x,
            this.foreHandle.y);
        this.rearHandle = new Vec2( //
            -this.rearHandle.x,
            this.rearHandle.y);
        return this;
    }

    /// <summary>
    /// Negates the y component of a knot's
    /// coordinate and handles, flipping it
    /// about the y axis.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 FlipY()
    {
        this.coord = new Vec2(
            this.coord.x, //
            -this.coord.y);
        this.foreHandle = new Vec2(
            this.foreHandle.x, //
            -this.foreHandle.y);
        this.rearHandle = new Vec2(
            this.rearHandle.x, //
            -this.rearHandle.y);
        return this;
    }

    /// <summary>
    /// Sets the forward-facing handle to mirror the rear-facing handle: the
    /// fore will have the same magnitude and negated direction of the rear.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 MirrorHandlesBackward()
    {
        this.foreHandle = this.coord - (this.rearHandle - this.coord);
        return this;
    }

    /// <summary>
    /// Sets the rear-facing handle to mirror the forward-facing handle: the
    /// rear will have the same magnitude and negated direction of the fore.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 MirrorHandlesForward()
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
    public Knot2 Relocate(Vec2 v)
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
    public Knot2 Reverse()
    {
        Vec2 temp = this.foreHandle;
        this.foreHandle = this.rearHandle;
        this.rearHandle = temp;

        return this;
    }

    /// <summary>
    /// Rotates this knot's fore handle by an angle in radians.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this knot</returns>
    public Knot2 RotateForeHandle(in float radians)
    {
        return this.RotateForeHandle(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates this knot's fore handle by the cosine and sine of an angle.
    /// </summary>
    /// <param name="cosa">cosine</param>
    /// <param name="sina">sine</param>
    /// <returns>this knot</returns>
    public Knot2 RotateForeHandle(in float cosa, in float sina)
    {
        this.foreHandle -= this.coord;
        this.foreHandle = Vec2.RotateZ(this.foreHandle, cosa, sina);
        this.foreHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Rotates this knot's fore and rear handles by an angle in radians.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this knot</returns>
    public Knot2 RotateHandles(in float radians)
    {
        return this.RotateHandles(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates this knot's fore and rear handles by the cosine and sine of an
    /// angle.
    /// </summary>
    /// <param name="cosa">cosine</param>
    /// <param name="sina">sine</param>
    /// <returns>this knot</returns>
    public Knot2 RotateHandles(in float cosa, in float sina)
    {
        this.RotateForeHandle(cosa, sina);
        this.RotateRearHandle(cosa, sina);

        return this;
    }

    /// <summary>
    /// Rotates this knot's rear handle by an angle in radians.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>this knot</returns>
    public Knot2 RotateRearHandle(in float radians)
    {
        return this.RotateRearHandle(MathF.Cos(radians), MathF.Sin(radians));
    }

    /// <summary>
    /// Rotates this knot's rear handle by the cosine and sine of an angle.
    /// </summary>
    /// <param name="cosa">cosine</param>
    /// <param name="sina">sine</param>
    /// <returns>this knot</returns>
    public Knot2 RotateRearHandle(in float cosa, in float sina)
    {
        this.rearHandle -= this.coord;
        this.rearHandle = Vec2.RotateZ(this.rearHandle, cosa, sina);
        this.rearHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Rotates this knot around the z axis by an angle in radians.
    /// </summary>
    /// <param name="radians">radians</param>
    /// <returns>this knot</returns>
    public Knot2 RotateZ(in float radians)
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
    public Knot2 RotateZ(in float cosa, in float sina)
    {
        this.coord = Vec2.RotateZ(this.coord, cosa, sina);
        this.foreHandle = Vec2.RotateZ(this.foreHandle, cosa, sina);
        this.rearHandle = Vec2.RotateZ(this.rearHandle, cosa, sina);

        return this;
    }

    /// <summary>
    /// Scales this knot by a factor.
    /// </summary>
    /// <param name="scale">factor</param>
    /// <returns>this knot</returns>
    public Knot2 Scale(in float scale)
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
    public Knot2 Scale(in Vec2 scale)
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
    public Knot2 ScaleForeHandleBy(in float scalar)
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
    public Knot2 ScaleForeHandleTo(in float magnitude)
    {
        this.foreHandle -= this.coord;
        this.foreHandle = Vec2.Rescale(this.foreHandle, magnitude);
        this.foreHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Scales both the fore and rear handle by a factor.
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>this knot</returns>
    public Knot2 ScaleHandlesBy(in float scalar)
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
    public Knot2 ScaleHandlesTo(in float magnitude)
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
    public Knot2 ScaleRearHandleBy(in float scalar)
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
    public Knot2 ScaleRearHandleTo(in float magnitude)
    {
        this.rearHandle -= this.coord;
        this.rearHandle = Vec2.Rescale(this.rearHandle, magnitude);
        this.rearHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Sets a knot from real numbers.
    /// </summary>
    /// <param name="xCo">x coordinate</param>
    /// <param name="yCo">y coordinate</param>
    /// <param name="xFh">fore handle x</param>
    /// <param name="yFh">fore handle y</param>
    /// <param name="xRh">rear handle x</param>
    /// <param name="yRh">rear handle y</param>
    /// <returns>this knot</returns>
    public Knot2 Set(
        in float xCo, in float yCo,
        in float xFh, in float yFh,
        in float xRh, in float yRh)
    {
        this.coord = new Vec2(xCo, yCo);
        this.foreHandle = new Vec2(xFh, yFh);
        this.rearHandle = new Vec2(xRh, yRh);

        return this;
    }

    /// <summary>
    /// Transforms this knot by a matrix.
    /// </summary>
    /// <param name="m">matrix</param>
    /// <returns>this knot</returns>
    public Knot2 Transform(in Mat3 m)
    {
        this.coord = Mat3.MulPoint(m, this.coord);
        this.foreHandle = Mat3.MulPoint(m, this.foreHandle);
        this.rearHandle = Mat3.MulPoint(m, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Transforms this knot by a transform.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>this knot</returns>
    public Knot2 Transform(in Transform2 tr)
    {
        this.coord = Transform2.MulPoint(tr, this.coord);
        this.foreHandle = Transform2.MulPoint(tr, this.foreHandle);
        this.rearHandle = Transform2.MulPoint(tr, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Translates this knot by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this knot</returns>
    public Knot2 Translate(in Vec2 v)
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
    public static implicit operator Knot2(in Vec2 v)
    {
        return new Knot2(v);
    }

    /// <summary>
    /// Evaluates a point between two knots given an origin, destination and a
    /// step.
    /// </summary>
    /// <param name="a">origin</param>
    /// <param name="b">destination</param>
    /// <param name="step">step</param>
    /// <returns>evaluation</returns>
    public static Vec2 BezierPoint(in Knot2 a, in Knot2 b, in float step)
    {
        return Vec2.BezierPoint(
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
    public static Vec2 BezierTangent(in Knot2 a, in Knot2 b, in float step)
    {
        return Vec2.BezierTangent(
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
    public static Vec2 BezierTanUnit(in Knot2 a, in Knot2 b, in float step)
    {
        return Vec2.BezierTanUnit(
            a.coord, a.foreHandle,
            b.rearHandle, b.coord,
            step);
    }

    /// <summary>
    /// Gets the fore handle of a knot as a direction, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>fore handle vector</returns>
    public static Vec2 ForeDir(in Knot2 kn)
    {
        return Vec2.Normalize(Knot2.ForeVec(kn));
    }

    /// <summary>
    /// Returns the magnitude of the knot's fore handle, i.e., the Euclidean
    /// distance between the fore handle and the coordinate.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>magnitude</returns>
    public static float ForeMag(in Knot2 kn)
    {
        return Vec2.DistEuclidean(kn.foreHandle, kn.coord);
    }

    /// <summary>
    /// Gets the fore handle of a knot as a vector, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>fore handle vector</returns>
    public static Vec2 ForeVec(in Knot2 kn)
    {
        return kn.foreHandle - kn.coord;
    }

    /// <summary>
    /// Creates a knot from polar coordinates, where the knot's fore handle is
    /// tangent to the radius.
    /// </summary>
    /// <param name="cosa">cosine of the angle</param>
    /// <param name="sina">sine of the angle</param>
    /// <param name="radius">radius</param>
    /// <param name="handleMag">length of handles</param>
    /// <param name="xCenter">x center</param>
    /// <param name="yCenter">y center</param>
    /// <returns>the knot</returns>
    public static Knot2 FromPolar(
        in float cosa = 1.0f,
        in float sina = 0.0f,
        in float radius = 1.0f,
        in float handleMag = Utils.FourThirds,
        in float xCenter = 0.0f,
        in float yCenter = 0.0f)
    {
        // TODO: Remove this from knots? Inline in curve.

        float cox = xCenter + radius * cosa;
        float coy = yCenter + radius * sina;

        float hmsina = sina * handleMag;
        float hmcosa = cosa * handleMag;

        float fhx = cox - hmsina;
        float fhy = coy + hmcosa;

        float rhx = cox + hmsina;
        float rhy = coy - hmcosa;

        return new Knot2(cox, coy, fhx, fhy, rhx, rhy);
    }

    /// <summary>
    /// Gets the rear handle of a knot as a direction, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>rear handle vector</returns>
    public static Vec2 RearDir(in Knot2 kn)
    {
        return Vec2.Normalize(Knot2.RearVec(kn));
    }

    /// <summary>
    /// Returns the magnitude of the knot's rear handle, i.e., the Euclidean
    /// distance between the rear handle and the coordinate.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>magnitude</returns>
    public static float RearMag(in Knot2 kn)
    {
        return Vec2.DistEuclidean(kn.rearHandle, kn.coord);
    }

    /// <summary>
    /// Gets the rear handle of a knot as a vector, rather than as a point.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <returns>rear handle vector</returns>
    public static Vec2 RearVec(in Knot2 kn)
    {
        return kn.rearHandle - kn.coord;
    }

    /// <summary>
    /// Returns a string representation of a knot.
    /// </summary>
    /// <param name="kn">knot</param>
    /// <param name="places">places</param>
    /// <returns>string</returns>
    public static string ToString(in Knot2 kn, in int places = 4)
    {
        return Knot2.ToString(new StringBuilder(256), kn, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a knot to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="kn">knot</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Knot2 kn, in int places = 4)
    {
        sb.Append("{ coord: ");
        Vec2.ToString(sb, kn.coord, places);
        sb.Append(", foreHandle: ");
        Vec2.ToString(sb, kn.foreHandle, places);
        sb.Append(", rearHandle: ");
        Vec2.ToString(sb, kn.rearHandle, places);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }
}