using System;
using System.Collections;
using System.Text;

/// <summary>
/// Organizes the vectors the shape a Bezier curve into a coordinate (or anchor
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
    /// The handle which warps the curve segment heading away from the knot along
    /// the direction of the curve.
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
    /// The handle which warps the curve segment heading away from the knot along
    /// the direction of the curve.
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
    /// <value>the rear handle</value>
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
    public Knot2 ( )
    {
        this.coord = Vec2.Zero;
        this.foreHandle = Vec2.Zero;
        this.rearHandle = Vec2.Zero;
    }

    /// <summary>
    /// Creates a knot from a coordinate.
    /// </summary>
    /// <param name="coord">coordinate</param>
    public Knot2 (Vec2 coord)
    {
        this.coord = coord;
        this.foreHandle = this.coord + Utils.Epsilon;
        this.rearHandle = this.coord - Utils.Epsilon;
    }

    /// <summary>
    /// Creates a knot from a series of vectors.
    /// </summary>
    /// <param name="coord">coordinate</param>
    /// <param name="foreHandle">fore handle</param>
    /// <param name="rearHandle">rear handle</param>
    public Knot2 (
        Vec2 coord,
        Vec2 foreHandle,
        Vec2 rearHandle)
    {
        this.coord = coord;
        this.foreHandle = foreHandle;
        this.rearHandle = rearHandle;
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
    public Knot2 (
        float xCo, float yCo,
        float xFh, float yFh,
        float xRh, float yRh)
    {
        this.Set (
            xCo, yCo,
            xFh, yFh,
            xRh, yRh);
    }

    /// <summary>
    /// Returns the knot's hash code based on those of its three constituent
    /// vectors.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.coord.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.foreHandle.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.rearHandle.GetHashCode ( );
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this knot.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return ToString (4);
    }

    /// <summary>
    /// Adopts the fore handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot2 AdoptForeHandle (in Knot2 source)
    {
        this.foreHandle = this.coord + (source.foreHandle - source.coord);
        return this;
    }

    /// <summary>
    /// Adopts the fore handle and rear handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot2 AdoptHandles (in Knot2 source)
    {
        this.AdoptForeHandle (source);
        this.AdoptRearHandle (source);
        return this;
    }

    /// <summary>
    /// Adopts the rear handle of a source knot.
    /// </summary>
    /// <param name="source">source knot</param>
    /// <returns>this knot</returns>
    public Knot2 AdoptRearHandle (in Knot2 source)
    {
        this.rearHandle = this.coord + (source.rearHandle - source.coord);
        return this;
    }

    /// <summary>
    /// Aligns this knot's fore handle to its rear handle while preserving
    /// magnitude.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 AlignHandlesBackward ( )
    {
        Vec2 rdir = this.rearHandle - this.coord;
        Vec2 fdir = this.foreHandle - this.coord;
        float flipRescale = Utils.Div (-Vec2.Mag (fdir), Vec2.Mag (rdir));
        this.foreHandle = this.coord + (flipRescale * rdir);
        return this;
    }

    /// <summary>
    /// Aligns this knot's rear handle to its fore handle while preserving
    /// magnitude.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 AlignHandlesForward ( )
    {
        Vec2 rdir = this.rearHandle - this.coord;
        Vec2 fdir = this.foreHandle - this.coord;
        float flipRescale = Utils.Div (-Vec2.Mag (rdir), Vec2.Mag (fdir));
        this.rearHandle = this.coord + (flipRescale * fdir);
        return this;
    }

    /// <summary>
    /// Sets the forward-facing handle to mirror the rear-facing handle: the fore
    /// will have the same magnitude and negated direction of the rear.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 MirrorHandlesBackward ( )
    {
        this.foreHandle = this.coord - (this.rearHandle - this.coord);
        return this;
    }

    /// <summary>
    /// Sets the rear-facing handle to mirror the forward-facing handle: the rear
    /// will have the same magnitude and negated direction of the fore.
    /// </summary>
    /// <returns>this knot</returns>
    public Knot2 MirrorHandlesForward ( )
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
    public Knot2 Relocate (Vec2 v)
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
    public Knot2 Reverse ( )
    {
        Vec2 temp = this.foreHandle;
        this.foreHandle = this.rearHandle;
        this.rearHandle = temp;
        return this;
    }

    /// <summary>
    /// Rotates this knot around the z axis by an angle in radians.
    /// </summary>
    /// <param name="radians">radians</param>
    /// <returns>this knot</returns>
    public Knot2 RotateZ (in float radians)
    {
        float sina = 0.0f;
        float cosa = 1.0f;
        Utils.SinCos (radians, out sina, out cosa);
        return this.RotateZ (cosa, sina);
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
    public Knot2 RotateZ (in float cosa, in float sina)
    {
        this.coord = Vec2.RotateZ (this.coord, cosa, sina);
        this.foreHandle = Vec2.RotateZ (this.foreHandle, cosa, sina);
        this.rearHandle = Vec2.RotateZ (this.rearHandle, cosa, sina);

        return this;
    }

    /// <summary>
    /// Scales this knot by a factor.
    /// </summary>
    /// <param name="scale">factor</param>
    /// <returns>this knot</returns>
    public Knot2 Scale (in float scale)
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
    public Knot2 Scale (in Vec2 scale)
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
    public Knot2 ScaleForeHandleBy (in float scalar = 1.0f)
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
    public Knot2 ScaleForeHandleTo (in float magnitude)
    {
        this.foreHandle -= this.coord;
        this.foreHandle = Vec2.Rescale (this.foreHandle, magnitude);
        this.foreHandle += this.coord;

        return this;
    }

    /// <summary>
    /// Scales both the fore and rear handle by a factor.
    /// </summary>
    /// <param name="magnitude">magnitude</param>
    /// <returns>the knot</returns>
    public Knot2 ScaleHandlesBy (in float scalar)
    {
        this.ScaleForeHandleBy (scalar);
        this.ScaleRearHandleBy (scalar);

        return this;
    }

    /// <summary>
    /// Scales both the fore and rear handle to a magnitude.
    /// </summary>
    /// <param name="magnitude">magnitude</param>
    /// <returns>the knot</returns>
    public Knot2 ScaleHandlesTo (in float magnitude)
    {
        this.ScaleForeHandleTo (magnitude);
        this.ScaleRearHandleTo (magnitude);

        return this;
    }

    /// <summary>
    /// Scales the rear handle by a factor.
    /// </summary>
    /// <param name="scalar">scalar</param>
    /// <returns>this knot</returns>
    public Knot2 ScaleRearHandleBy (in float scalar = 1.0f)
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
    public Knot2 ScaleRearHandleTo (in float magnitude = 1.0f)
    {
        this.rearHandle -= this.coord;
        this.rearHandle = Vec2.Rescale (this.rearHandle, magnitude);
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
    public Knot2 Set (in float xCo, in float yCo, in float xFh, in float yFh, in float xRh, in float yRh)
    {
        this.coord = new Vec2 (xCo, yCo);
        this.foreHandle = new Vec2 (xFh, yFh);
        this.rearHandle = new Vec2 (xRh, yRh);

        return this;
    }

    /// <summary>
    /// Returns a string representation of this vector.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (int places = 4)
    {
        return new StringBuilder (256)
            .Append ("{ coord: ")
            .Append (this.coord.ToString (places))
            .Append (", foreHandle: ")
            .Append (this.foreHandle.ToString (places))
            .Append (", rearHandle: ")
            .Append (this.rearHandle.ToString (places))
            .Append (' ')
            .Append ('}')
            .ToString ( );
    }

    /// <summary>
    /// Transforms this knot by a matrix.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>this knot</returns>
    public Knot2 Transform (in Mat3 m)
    {
        this.coord = Mat3.MulPoint (m, this.coord);
        this.foreHandle = Mat3.MulPoint (m, this.foreHandle);
        this.rearHandle = Mat3.MulPoint (m, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Transforms this knot by a transform.
    /// </summary>
    /// <param name="tr">transform</param>
    /// <returns>this knot</returns>
    public Knot2 Transform (in Transform2 tr)
    {
        this.coord = Transform2.MulPoint (tr, this.coord);
        this.foreHandle = Transform2.MulPoint (tr, this.foreHandle);
        this.rearHandle = Transform2.MulPoint (tr, this.rearHandle);

        return this;
    }

    /// <summary>
    /// Translates this knot by a vector.
    /// </summary>
    /// <param name="v">vector</param>
    /// <returns>this knot</returns>
    public Knot2 Translate (in Vec2 v)
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
    public static implicit operator Knot2 (Vec2 v)
    {
        return new Knot2 (v);
    }

    /// <summary>
    /// Evaluates a point between two knots given an origin, destination and a step.
    /// </summary>
    /// <param name="a">origin</param>
    /// <param name="b">destination</param>
    /// <param name="step">step</param>
    /// <returns>the evaluation</returns>
    public static Vec2 BezierPoint (in Knot2 a, in Knot2 b, in float step)
    {
        return Vec2.BezierPoint (
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
    /// <returns>the evaluation</returns>
    public static Vec2 BezierTangent (in Knot2 a, in Knot2 b, in float step)
    {
        return Vec2.BezierTangent (
            a.coord, a.foreHandle,
            b.rearHandle, b.coord,
            step);
    }

    /// <summary>
    /// Evaluates a normalized tangent given an origin, a destination knot and a step.
    /// </summary>
    /// <param name="a">origin</param>
    /// <param name="b">destination</param>
    /// <param name="step">step</param>
    /// <returns>the evaluation</returns>
    public static Vec2 BezierTanUnit (in Knot2 a, in Knot2 b, in float step)
    {
        return Vec2.BezierTanUnit (
            a.coord, a.foreHandle,
            b.rearHandle, b.coord,
            step);
    }

    /// <summary>
    /// Gets the fore handle of a knot as a direction, rather than as a point.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>fore handle vector</returns>
    public static Vec2 ForeDir (in Knot2 knot)
    {
        return Vec2.Normalize (Knot2.ForeVec (knot));
    }

    /// <summary>
    /// Returns the magnitude of the knot's fore handle, i.e., the Euclidean
    /// distance between the fore handle and the coordinate.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>magnitude</returns>
    public static float ForeMag (in Knot2 knot)
    {
        return Vec2.DistEuclidean (knot.foreHandle, knot.coord);
    }

    /// <summary>
    /// Gets the fore handle of a knot as a vector, rather than as a point.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>fore handle vector</returns>
    public static Vec2 ForeVec (in Knot2 knot)
    {
        return knot.foreHandle - knot.coord;
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
    public static Knot2 FromPolar (in float cosa = 1.0f, in float sina = 0.0f, in float radius = 1.0f, in float handleMag = Utils.FourThirds, in float xCenter = 0.0f, in float yCenter = 0.0f)
    {
        float cox = xCenter + radius * cosa;
        float coy = yCenter + radius * sina;

        float hmsina = sina * handleMag;
        float hmcosa = cosa * handleMag;

        float fhx = cox - hmsina;
        float fhy = coy + hmcosa;

        float rhx = cox + hmsina;
        float rhy = coy - hmcosa;

        return new Knot2 (cox, coy, fhx, fhy, rhx, rhy);
    }

    /// <summary>
    /// Sets a knot from line segment. Assumes that that the previous knot's
    /// coordinate is set to the first anchor point. The previous knot's fore
    /// handle, the next knot's rear handle and the next knot's coordinate are set
    /// by this function.
    /// </summary>
    /// <param name="nextAnchor">next anchor</param>
    /// <param name="prev">previous knot</param>
    /// <param name="next">next knot</param>
    /// <returns>the next knot</returns>
    public static Knot2 FromSegLinear (in Vec2 nextAnchor, in Knot2 prev, in Knot2 next)
    {
        return Knot2.FromSegLinear (nextAnchor.x, nextAnchor.y, prev, next);
    }

    /// <summary>
    /// Sets a knot from line segment. Assumes that that the previous knot's
    /// coordinate is set to the first anchor point. The previous knot's fore
    /// handle, the next knot's rear handle and the next knot's coordinate are set
    /// by this function.
    /// </summary>
    /// <param name="xNextAnchor">next anchor x</param>
    /// <param name="yNextAnchor">next anchor y</param>
    /// <param name="prev">previous knot</param>
    /// <param name="next">next knot</param>
    /// <returns>the next knot</returns>
    public static Knot2 FromSegLinear (in float xNextAnchor, in float yNextAnchor, in Knot2 prev, in Knot2 next)
    {
        next.Coord = new Vec2 (xNextAnchor, yNextAnchor);
        prev.ForeHandle = Vec2.Mix (prev.Coord, next.Coord, Utils.OneThird);
        next.RearHandle = Vec2.Mix (next.Coord, prev.Coord, Utils.OneThird);

        return next;
    }

    /// <summary>
    /// Gets the rear handle of a knot as a direction, rather than as a point.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>rear handle vector</returns>
    public static Vec2 RearDir (in Knot2 knot)
    {
        return Vec2.Normalize (Knot2.RearVec (knot));
    }

    /// <summary>
    /// Returns the magnitude of the knot's rear handle, i.e., the Euclidean
    /// distance between the rear handle and the coordinate.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>magnitude</returns>
    public static float RearMag (in Knot2 knot)
    {
        return Vec2.DistEuclidean (knot.rearHandle, knot.coord);
    }

    /// <summary>
    /// Gets the rear handle of a knot as a vector, rather than as a point.
    /// </summary>
    /// <param name="knot">knot</param>
    /// <returns>rear handle vector</returns>
    public static Vec2 RearVec (in Knot2 knot)
    {
        return knot.rearHandle - knot.coord;
    }
}