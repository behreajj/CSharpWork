using System;
using System.Collections;
using System.Text;

/// <summary>
/// A four-dimensional complex number. The x, y and z components are
/// coefficients of the imaginary i, j, and k. Discovered by William R. Hamilton
/// with the formula i i = j j = k k = i j k = -1.0 . Quaternions with a
/// magnitude of 1.0 are commonly used to rotate 3D objects from one orientation
/// to another without suffering gimbal lock.
/// </summary>
[Serializable]
public readonly struct Quat : IEquatable<Quat>, IEnumerable
{
    /// <summary>
    /// The coefficients of the imaginary components i, j and k.
    /// </summary>
    private readonly Vec3 imag;

    /// <summary>
    /// The real component.
    /// </summary>
    private readonly float real;

    /// <summary>
    /// The forward axis.
    /// </summary>
    /// <value>forward</value>
    public Vec3 Forward
    {
        get
        {
            float w = this.real;
            float x = this.imag.x;
            float y = this.imag.y;
            float z = this.imag.z;

            float xy = x * y;
            float xw = x * w;
            float yz = y * z;
            float zw = z * w;

            return new Vec3 (
                xy + xy - (zw + zw),
                w * w + y * y - x * x - z * z,
                xw + xw + yz + yz);
        }
    }

    /// <summary>
    /// The coefficients of the imaginary components i, j and k.
    /// </summary>
    /// <value>the imaginary vector</value>
    public Vec3 Imag { get { return this.imag; } }

    /// <summary>
    /// Returns the number of elements in this quaternion.
    /// </summary>
    /// <value>the length</value>
    public int Length { get { return 4; } }

    /// <summary>
    /// The real component.
    /// </summary>
    /// <value>the real scalar</value>
    public float Real { get { return this.real; } }

    /// <summary>
    /// The right axis.
    /// </summary>
    /// <value>right</value>
    public Vec3 Right
    {
        get
        {
            float w = this.real;
            float x = this.imag.x;
            float y = this.imag.y;
            float z = this.imag.z;

            float xy = x * y;
            float xz = x * z;
            float yw = y * w;
            float zw = z * w;

            return new Vec3 (
                w * w + x * x - y * y - z * z,
                zw + zw + xy + xy,
                xz + xz - (yw + yw));
        }
    }

    /// <summary>
    /// The up axis.
    /// </summary>
    /// <value>up</value>
    public Vec3 Up
    {
        get
        {
            float w = this.real;
            float x = this.imag.x;
            float y = this.imag.y;
            float z = this.imag.z;

            float xz = x * z;
            float xw = x * w;
            float yz = y * z;
            float yw = y * w;

            return new Vec3 (
                yw + yw + xz + xz,
                yz + yz - (xw + xw),
                w * w + z * z - x * x - y * y);
        }
    }

    /// <summary>
    /// The real component.
    /// </summary>
    /// <value>w</value>
    public float w { get { return this.real; } }

    /// <summary>
    /// The coefficient of the imaginary i.
    /// </summary>
    /// <value>x</value>
    public float x { get { return this.imag.x; } }

    /// <summary>
    /// The coefficient of the imaginary j.
    /// </summary>
    /// <value>y</value>
    public float y { get { return this.imag.y; } }

    /// <summary>
    /// The coefficient of the imaginary k.
    /// </summary>
    /// <value>z</value>
    public float z { get { return this.imag.z; } }

    /// <summary>
    /// Retrieves an element by index. The real component is assumed to be the
    /// first element.
    /// </summary>
    /// <value>the element</value>
    public float this [int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                case -4:
                    return this.real;
                case 1:
                case -3:
                    return this.imag.x;
                case 2:
                case -2:
                    return this.imag.y;
                case 3:
                case -1:
                    return this.imag.z;
                default:
                    return 0.0f;
            }
        }
    }

    /// <summary>
    /// Constructs a quaternion from a real number and an imaginary vector.
    /// </summary>
    /// <param name="real">real number</param>
    /// <param name="imag">imaginary vector</param>
    public Quat (in float real = 1.0f, in Vec3 imag = new Vec3 ( ))
    {
        // TODO: Look up poincare duality, involute, outer product (meet) and regressive product (join).

        this.real = real;
        this.imag = imag;
    }

    /// <summary>
    /// Constructs a quaternion from a real number, w, and three imaginary
    /// numbers: x, y and z.
    /// </summary>
    /// <param name="w">real number</param>
    /// <param name="x">x imaginary</param>
    /// <param name="y">y imaginary</param>
    /// <param name="z">z imaginary</param>
    public Quat (in float w = 1.0f, in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
    {
        this.real = w;
        this.imag = new Vec3 (x, y, z);
    }

    /// <summary>
    /// Tests this quaternion for equivalence with an object.
    /// </summary>
    /// <param name="value">the object</param>
    /// <returns>the equivalence</returns>F
    public override bool Equals (object value)
    {
        if (Object.ReferenceEquals (this, value)) return true;
        if (Object.ReferenceEquals (null, value)) return false;
        if (value is Quat) return this.Equals ((Quat) value);
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this quaternion.
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode ( )
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.real.GetHashCode ( );
            hash = hash * Utils.HashMul ^ this.imag.GetHashCode ( );
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this quaternion.
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString ( )
    {
        return ToString (4);
    }

    /// <summary>
    /// Tests this quaternion for equivalence with another in compliance with
    /// the IEquatable interface.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>the equivalence</returns>
    public bool Equals (Quat q)
    {
        if (this.real.GetHashCode ( ) != q.real.GetHashCode ( )) return false;
        if (this.imag.GetHashCode ( ) != q.imag.GetHashCode ( )) return false;
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this quaternion, allowing its
    /// components to be accessed in a foreach loop. The real component, w, is
    /// treated as the first element.
    /// </summary>
    /// <returns>the enumerator</returns>
    public IEnumerator GetEnumerator ( )
    {
        yield return this.real;
        yield return this.imag.x;
        yield return this.imag.y;
        yield return this.imag.z;
    }

    /// <summary>
    /// Returns a float array of length 4 containing this quaternion's
    /// components. The real component, w, is treated as the first element.
    /// </summary>
    /// <returns>the array</returns>
    public float[ ] ToArray ( )
    {
        return this.ToArray (new float[4], 0);
    }

    /// <summary>
    /// Puts this quaternion's components into an array at a given index. The real component, w, is treated as the first element.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public float[ ] ToArray (in float[ ] arr, in int i = 0)
    {
        arr[i] = this.real;
        arr[i + 1] = this.imag.x;
        arr[i + 2] = this.imag.y;
        arr[i + 3] = this.imag.z;
        return arr;
    }

    /// <summary>
    /// Returns a string representation of this quaternion.
    /// </summary>
    /// <param name="places">number of decimal places</param>
    /// <returns>the string</returns>
    public string ToString (int places = 4)
    {
        return new StringBuilder (128)
            .Append ("{ real: ")
            .Append (Utils.ToFixed (this.real, places))
            .Append (", imag: ")
            .Append (this.imag.ToString (places))
            .Append (" }")
            .ToString ( );
    }

    /// <summary>
    /// Returns a named value tuple containing this quaternion's components.
    /// </summary>
    /// <returns>the tuple</returns>
    public (float w, float x, float y, float z) ToTuple ( )
    {
        return (w: this.real,
            x: this.imag.x,
            y: this.imag.y,
            z: this.imag.z);
    }

    /// <summary>
    /// Promotes a real number to a quaternion.
    /// </summary>
    /// <param name="v">real number</param>
    public static implicit operator Quat (in float v)
    {
        return new Quat (v, new Vec3 (0.0f, 0.0f, 0.0f));
    }

    /// <summary>
    /// Promotes a vector to a pure quaternion.
    /// </summary>
    /// <param name="v">vector</param>
    public static implicit operator Quat (in Vec3 v)
    {
        return new Quat (0.0f, new Vec3 (v.x, v.y, v.z));
    }

    /// <summary>
    /// Converts a four dimensional vector to a quaternion. The vector's w
    /// component is interpreted to be the real component.
    /// </summary>
    /// <param name="v">vector</param>
    public static explicit operator Quat (in Vec4 v)
    {
        return new Quat (v.w, new Vec3 (v.x, v.y, v.z));
    }

    /// <summary>
    /// Converts a quaternion to a four dimensional vector. The quaternion's
    /// real component is interpreted to be the w component.
    /// </summary>
    /// <param name="q">quaternion</param>
    public static explicit operator Vec4 (in Quat q)
    {
        Vec3 i = q.imag;
        return new Vec4 (i.x, i.y, i.z, q.real);
    }

    /// <summary>
    /// Converts a quaternion to a boolean by finding whether any of its
    /// components are non-zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    public static explicit operator bool (in Quat q)
    {
        return Quat.Any (q);
    }

    /// <summary>
    /// Converts a quaternion to a real number by finding its magnitude.
    /// </summary>
    /// <param name="q">quaternion</param>
    public static explicit operator float (in Quat q)
    {
        return Quat.Mag (q);
    }

    /// <summary>
    /// A quaternion evaluates to true when any of its components are not equal
    /// to zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>the evaluation</returns>
    public static bool operator true (in Quat q)
    {
        return Quat.Any (q);
    }

    /// <summary>
    /// A quaternion evaluates to false when all of its components are zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>the evaluation</returns>
    public static bool operator false (in Quat q)
    {
        return Quat.None (q);
    }

    /// <summary>
    /// Negates the real and imaginary components of a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>negation</returns>
    public static Quat operator - (in Quat q)
    {
        return new Quat (-q.real, -q.imag);
    }

    /// <summary>
    /// Multiplies two quaternions. Uses the formula:
    ///
    /// a b := { a.real b.real - dot ( a.imag, b.imag ), cross ( a.imag, b.imag
    /// ) + a.real b.imag + b.real a.imag }
    ///
    /// Quaternion multiplication is not commutative.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator * (in Quat a, in Quat b)
    {
        return new Quat (
            (a.real * b.real) -
            Vec3.Dot (a.imag, b.imag),

            Vec3.Cross (a.imag, b.imag) +
            (a.real * b.imag) +
            (b.real * a.imag));
    }

    /// <summary>
    /// Multiplies a quaternion and a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator * (in Quat a, in float b)
    {
        return new Quat (a.real * b, a.imag * b);
    }

    /// <summary>
    /// Multiplies a scalar and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator * (in float a, in Quat b)
    {
        return new Quat (a * b.real, a * b.imag);
    }

    /// <summary>
    /// Multiplies a quaternion and a vector; the latter is treated as a pure
    /// quaternion, _not_ as a point. Either see MulVector method or use RPR'.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator * (in Quat a, in Vec3 b)
    {
        return new Quat (-Vec3.Dot (a.imag, b),
            Vec3.Cross (a.imag, b) + (a.real * b));
    }

    /// <summary>
    /// Multiplies a vector and a quaternion; the former is treated as a pure
    /// quaternion, _not_ as a point. Either see MulVector method or use RPR'.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator * (in Vec3 a, in Quat b)
    {
        return new Quat (-Vec3.Dot (a, b.imag),
            Vec3.Cross (a, b.imag) + (b.real * a));
    }

    /// <summary>
    /// Divides two quaternions. Equivalent to multiplying the numerator and the
    /// inverse of the denominator.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator / (in Quat a, in Quat b)
    {
        return a * Quat.Inverse (b);
    }

    /// <summary>
    /// Divides a quaternion by a real number.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator / (in Quat a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new Quat (a.real * bInv, a.imag * bInv);
        }
        return new Quat ( );
    }

    /// <summary>
    /// Divides a real number by a quaternion.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator / (in float a, in Quat b)
    {
        return a * Quat.Inverse (b);
    }

    /// <summary>
    /// Divides a quaternion by a vector. the denominator is treated as a pure
    /// quaternion.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator / (in Quat a, in Vec3 b)
    {
        return a * (-b / Vec3.MagSq (b));
    }

    /// <summary>
    /// Divides a vector by a quaternion; the numerator is treated as a pure
    /// quaternion.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator / (in Vec3 a, in Quat b)
    {
        return a * Quat.Inverse (b);
    }

    /// <summary>
    /// Adds two quaternions.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator + (in Quat a, in Quat b)
    {
        return new Quat (a.real + b.real, a.imag + b.imag);
    }

    /// <summary>
    /// Adds a real number and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator + (in Quat a, in float b)
    {
        return new Quat (a.real + b, a.imag);
    }

    /// <summary>
    /// Adds a real number and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator + (in float a, in Quat b)
    {
        return new Quat (a + b.real, b.imag);
    }

    /// <summary>
    /// Adds an imaginary vector and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator + (in Quat a, in Vec3 b)
    {
        return new Quat (a.real, a.imag + b);
    }

    /// <summary>
    /// Adds an imaginary vector and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator + (in Vec3 a, in Quat b)
    {
        return new Quat (b.real, a + b.imag);
    }

    /// <summary>
    /// Subtracts the right quaternion from the left.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator - (in Quat a, in Quat b)
    {
        return new Quat (a.real - b.real, a.imag - b.imag);
    }

    /// <summary>
    /// Subtracts a real number from a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator - (in Quat a, in float b)
    {
        return new Quat (a.real - b, a.imag);
    }

    /// <summary>
    /// Subtracts a quaternion from a real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator - (in float a, in Quat b)
    {
        return new Quat (a - b.real, -b.imag);
    }

    /// <summary>
    /// Subtracts an imaginary vector from a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator - (in Quat a, in Vec3 b)
    {
        return new Quat (a.real, a.imag - b);
    }

    /// <summary>
    /// Subtracts a quaternion from an imaginary vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator - (in Vec3 a, in Quat b)
    {
        return new Quat (-b.real, a - b.imag);
    }

    /// <summary>
    /// Tests to see if all the quaternion's components are non-zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool All (in Quat q)
    {
        return (q.real != 0.0f) && Vec3.All (q.imag);
    }

    /// <summary>
    /// Tests to see if any of the quaternion's components are non-zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool Any (in Quat q)
    {
        return (q.real != 0.0f) || Vec3.Any (q.imag);
    }

    /// <summary>
    /// Evaluates whether or not two quaternions approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <returns>evaluation</returns>
    public static bool Approx (in Quat a, in Quat b)
    {
        return Utils.Approx (a.real, b.real) &&
            Vec3.Approx (a.imag, b.imag);
    }

    /// <summary>
    /// Returns the conjugate of the quaternion, where the imaginary component
    /// is negated.
    ///
    /// a* := { a.real, -a.imag }
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>conjugate</returns>
    public static Quat Conj (in Quat q)
    {
        return new Quat (q.real, -q.imag);
    }

    /// <summary>
    /// Finds the dot product of two quaternions by summing the products of
    /// their corresponding components.
    ///
    /// dot ( a, b ) := a.real b.real + dot ( a.imag, b.imag )
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Dot (in Quat a, in Quat b)
    {
        return a.real * b.real + Vec3.Dot (a.imag, b.imag);
    }

    /// <summary>
    /// Sets a quaternion from an angle. The axis is assumed to be (0.0, 0.0,
    /// 1.0) . Sets the real component of the quaternion to cosine of the angle;
    /// the imaginary z component is set to the sine. Useful when working in
    /// 2.5D, where a two-dimensional angle may need to be transferred to a
    /// three-dimensional transform.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <returns>quaternion</returns>
    public static Quat FromAngle (in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos ((radians % Utils.Tau) * 0.5f, out sina, out cosa);
        return new Quat (cosa, 0.0f, 0.0f, sina);
    }

    /// <summary>
    /// Sets a quaternion from an axis and angle. Normalizes the axis prior to
    /// calculating the quaternion.
    /// </summary>
    /// <param name="radians">the angle</param>
    /// <param name="axis">the axis</param>
    /// <returns>the quaternion</returns>
    public static Quat FromAxisAngle (in float radians, in Vec3 axis)
    {
        float amSq = Vec3.MagSq (axis);
        if (amSq == 0.0f) return Quat.Identity;

        float nx = axis.x;
        float ny = axis.y;
        float nz = axis.z;

        if (!Utils.Approx (amSq, 1.0f))
        {
            float amInv = 1.0f / Utils.Sqrt (amSq);
            nx *= amInv;
            ny *= amInv;
            nz *= amInv;
        }

        float sinHalf = 0.0f;
        float cosHalf = 0.0f;
        Utils.SinCos ((radians % Utils.Tau) * 0.5f, out sinHalf, out cosHalf);
        return new Quat (cosHalf,
            new Vec3 (nx * sinHalf,
                ny * sinHalf,
                nz * sinHalf));
    }

    /// <summary>
    /// Creates a quaternion with reference to two vectors. This function
    /// creates normalized copies of the vectors. Uses the formula:
    ///
    /// fromTo (a, b) := { a . b, a x b }
    /// </summary>
    /// <param name="origin">the origin</param>
    /// <param name="dest">the destination</param>
    /// <returns>the vector</returns>
    public static Quat FromTo (in Vec3 origin, in Vec3 dest)
    {
        Vec3 a = Vec3.Normalize (origin);
        Vec3 b = Vec3.Normalize (dest);

        return new Quat (
            Vec3.Dot (a, b),
            Vec3.Cross (a, b));
    }

    /// <summary>
    /// Finds the inverse, or reciprocal, of a quaternion, which is the its
    /// conjugate divided by its magnitude squared.
    ///
    /// 1 / a := a* / ( a a* )
    ///
    /// If a quaternion is of unit length, its inverse is equal to its
    /// conjugate.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the inverse</returns>
    public static Quat Inverse (in Quat q)
    {
        return Quat.Conj (q) / Quat.MagSq (q);
    }

    /// <summary>
    /// Multiplies a vector by a quaternion's inverse, allowing a prior
    /// rotation, allowing a prior rotation to be undone.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <param name="v">the input vector</param>
    /// <returns>the unrotated vector</returns>
    public static Vec3 InvMulVector (in Quat q, in Vec3 v)
    {
        float mSq = Quat.MagSq (q);
        if (mSq > 0.0f)
        {
            float mSqInv = 1.0f / mSq;

            float w = q.real * mSqInv;
            Vec3 i = q.imag;
            float qx = -i.x * mSqInv;
            float qy = -i.y * mSqInv;
            float qz = -i.z * mSqInv;

            float iw = -qx * v.x - qy * v.y - qz * v.z;
            float ix = w * v.x + qy * v.z - qz * v.y;
            float iy = w * v.y + qz * v.x - qx * v.z;
            float iz = w * v.z + qx * v.y - qy * v.x;

            return new Vec3 (
                ix * w + iz * qy - iw * qx - iy * qz,
                iy * w + ix * qz - iw * qy - iz * qx,
                iz * w + iy * qx - iw * qz - ix * qy);
        }
        return new Vec3 ( );
    }

    /// <summary>
    /// Tests if the quaternion is the identity, where its real component is 1.0
    /// and its imaginary components are all zero.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the evaluation</returns>
    public static bool IsIdentity (in Quat q)
    {
        return q.real == 1.0f && Vec3.None (q.imag);
    }

    /// <summary>
    /// Tests to see if a quaternion is pure, i.e. if its real component is
    /// zero.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the evaluation</returns>
    public static bool IsPure (in Quat q)
    {
        return q.real == 0.0f;
    }

    /// <summary>
    /// Tests if the quaternion is of unit magnitude.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the evaluation</returns>
    public static bool IsUnit (in Quat q)
    {
        return Utils.Approx (Quat.MagSq (q), 1.0f);
    }

    /// <summary>
    /// Finds the length, or magnitude, of a quaternion.
    ///
    /// |a| := sqrt ( dot ( a, a ) )
    ///
    /// |a| := sqrt ( a a* )
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the magnitude</returns>
    public static float Mag (in Quat q)
    {
        return Utils.Sqrt (Quat.MagSq (q));
    }

    /// <summary>
    /// Finds the magnitude squared of a quaternion. Equivalent to the dot
    /// product of a quaternion with itself and to the product of a quaternion
    /// with its conjugate.
    ///
    /// |a|<sup>2</sup> := dot ( a, a )
    ///
    /// |a|<sup>2</sup> := a a*
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the magnitude squared</returns>
    public static float MagSq (in Quat q)
    {
        return q.real * q.real + Vec3.MagSq (q.imag);
    }

    /// <summary>
    /// Eases between two quaternions by spherical linear interpolation (slerp).
    /// Chooses the shortest path between two orientations and maintains
    /// constant speed for a step in [0.0, 1.0] .
    /// </summary>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <param name="step">step</param>
    /// <returns>quaternion</returns>
    public static Quat Mix (in Quat origin, in Quat dest, in float step = 0.5f)
    {
        // Decompose origin quaternion.
        Vec3 ai = origin.imag;
        float aw = origin.real;
        float ax = ai.x;
        float ay = ai.y;
        float az = ai.z;

        // Decompose destination quaternion.
        Vec3 bi = dest.imag;
        float bw = dest.real;
        float bx = bi.x;
        float by = bi.y;
        float bz = bi.z;

        // Clamped dot product.
        float dotp = Utils.Clamp (
            aw * bw +
            ax * bx +
            ay * by +
            az * bz, -1.0f, 1.0f);

        // Flip values if the orientation is negative.
        if (dotp < 0.0f)
        {
            bw = -bw;
            bx = -bx;
            by = -by;
            bz = -bz;
            dotp = -dotp;
        }

        float theta = Utils.Acos (dotp);
        float sinTheta = Utils.Sqrt (1.0f - dotp * dotp);

        // The complementary step, i.e., 1.0 - step.
        float u = 1.0f;

        // The step.
        float v = 0.0f;

        if (sinTheta > Utils.Epsilon)
        {
            float sInv = 1.0f / sinTheta;
            float ang = step * theta;
            u = Utils.Sin (theta - ang) * sInv;
            v = Utils.Sin (ang) * sInv;
        }
        else
        {
            u = 1.0f - step;
            v = step;
        }

        // Unclamped linear interpolation.
        float cw = u * aw + v * bw;
        float cx = u * ax + v * bx;
        float cy = u * ay + v * by;
        float cz = u * az + v * bz;

        // Find magnitude squared.
        float mSq = cw * cw + cx * cx + cy * cy + cz * cz;

        // If 0, then invalid, reset to identity.
        if (Utils.Abs (mSq) < Utils.Epsilon)
        {
            return Quat.Identity;
        }

        // If 1, no need to normalize.
        if (Utils.Abs (1.0f - mSq) < Utils.Epsilon)
        {
            return new Quat (cw, cx, cy, cz);
        }

        // Normalize.
        float mInv = 1.0f / Utils.Sqrt (mSq);
        return new Quat (
            cw * mInv,
            cx * mInv,
            cy * mInv,
            cz * mInv);
    }

    /// <summary>
    /// Rotates a vector by a quaternion. Equivalent to promoting the vector to
    /// a pure quaternion, multiplying the rotation quaternion and promoted
    /// vector, then multiplying the product by the rotation's inverse.
    ///
    /// a b := ( a { 0.0, b } ) / a
    ///
    /// The result is then demoted to a vector, as the real component should be
    /// 0.0 . This is often denoted as P' = RPR'.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <param name="v">the input vector</param>
    /// <returns>the rotated vector</returns>
    public static Vec3 MulVector (in Quat q, in Vec3 v)
    {
        float w = q.real;
        Vec3 i = q.imag;
        float qx = i.x;
        float qy = i.y;
        float qz = i.z;

        float iw = -qx * v.x - qy * v.y - qz * v.z;
        float ix = w * v.x + qy * v.z - qz * v.y;
        float iy = w * v.y + qz * v.x - qx * v.z;
        float iz = w * v.z + qx * v.y - qy * v.x;

        return new Vec3 (
            ix * w + iz * qy - iw * qx - iy * qz,
            iy * w + ix * qz - iw * qy - iz * qx,
            iz * w + iy * qx - iw * qz - ix * qy);

        // Quat product = (q * source) / q; return product.imag;
    }

    /// <summary>
    /// Tests if all components of the quaternion are zero.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>the evaluation</returns>
    public static bool None (in Quat q)
    {
        return q.real == 0.0f && Vec3.None (q.imag);
    }

    /// <summary>
    /// Divides a quaternion by its magnitude, such that its new magnitude is
    /// one and it lies on a 4D hyper-sphere. Uses the formula:
    ///
    /// a^ = a / |a|
    ///
    /// Quaternions with zero magnitude will return the identity.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <returns>the normalized quaternion</returns>
    public static Quat Normalize (in Quat q)
    {
        return q / Quat.Mag (q);
    }

    /// <summary>
    /// Creates a random unit quaternion. Uses an algorithm by Ken Shoemake,
    /// reproduced at this Math Stack Exchange discussion:
    /// https://math.stackexchange.com/questions/131336/uniform-random-quaternion-in-a-restricted-angle-range
    /// .
    /// </summary>
    /// <param name="rng">the random number generator</param>
    /// <returns>the random quaternion</returns>
    public static Quat Random (in System.Random rng)
    {
        float t0 = Utils.Tau * (float) rng.NextDouble ( );
        float t1 = Utils.Tau * (float) rng.NextDouble ( );

        float r1 = (float) rng.NextDouble ( );
        float x0 = Utils.Sqrt (1.0f - r1);
        float x1 = Utils.Sqrt (r1);

        float sint0 = 0.0f;
        float cost0 = 0.0f;
        Utils.SinCos (t0, out sint0, out cost0);

        float sint1 = 0.0f;
        float cost1 = 0.0f;
        Utils.SinCos (t1, out sint1, out cost1);

        return new Quat (
            x0 * sint0,
            x0 * cost0,
            x1 * sint1,
            x1 * cost1);
    }

    /// <summary>
    /// Rotates a quaternion around an arbitrary axis by an angle.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="radians">the angle in radians</param>
    /// <param name="axis">the axis</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat Rotate (in Quat q, in float radians, in Vec3 axis)
    {
        float mSq = Quat.MagSq (q);
        if (mSq > 0.0f)
        {
            float wNorm = q.real * Utils.InvSqrtUnchecked (mSq);
            float halfAngle = Utils.Acos (wNorm);
            return Quat.FromAxisAngle (halfAngle + halfAngle + radians, axis);
        }
        return Quat.FromAxisAngle (radians, axis);
    }

    /// <summary>
    /// Rotates a quaternion about the x axis by an angle.
    ///
    /// Do not use sequences of orthonormal rotations by Euler angles; this will
    /// result in gimbal lock, defeating the purpose behind a quaternion.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat RotateX (in Quat q, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos ((radians % Utils.Tau) * 0.5f, out sina, out cosa);
        return Quat.RotateX (q, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the x axis. Accepts calculated sine and cosine
    /// of half the angle so that collections of quaternions can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="cosah">cosine of half the angle</param>
    /// <param name="sinah">sine of half the angle</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat RotateX (in Quat q, in float cosah, in float sinah)
    {
        Vec3 i = q.imag;
        return new Quat (
            cosah * q.real - sinah * i.x,
            cosah * i.x + sinah * q.real,
            cosah * i.y + sinah * i.z,
            cosah * i.z - sinah * i.y);
    }

    /// <summary>
    /// Rotates a quaternion about the y axis by an angle.
    ///
    /// Do not use sequences of orthonormal rotations by Euler angles; this will
    /// result in gimbal lock, defeating the purpose behind a quaternion.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat RotateY (in Quat q, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos ((radians % Utils.Tau) * 0.5f, out sina, out cosa);
        return Quat.RotateY (q, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the y axis. Accepts calculated sine and cosine
    /// of half the angle so that collections of quaternions can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="cosah">cosine of half the angle</param>
    /// <param name="sinah">sine of half the angle</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat RotateY (in Quat q, in float cosah, in float sinah)
    {
        Vec3 i = q.imag;
        return new Quat (
            cosah * q.real - sinah * i.y,
            cosah * i.x - sinah * i.z,
            cosah * i.y + sinah * q.real,
            cosah * i.z + sinah * i.x);
    }

    /// <summary>
    /// Rotates a quaternion about the z axis by an angle.
    ///
    /// Do not use sequences of orthonormal rotations by Euler angles; this will
    /// result in gimbal lock, defeating the purpose behind a quaternion.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="radians">the angle in radians</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat RotateZ (in Quat q, in float radians)
    {
        float sina = 0.0f;
        float cosa = 0.0f;
        Utils.SinCos ((radians % Utils.Tau) * 0.5f, out sina, out cosa);
        return Quat.RotateZ (q, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the z axis. Accepts calculated sine and cosine
    /// of half the angle so that collections of quaternions can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="q">the input quaternion</param>
    /// <param name="cosah">cosine of half the angle</param>
    /// <param name="sinah">sine of half the angle</param>
    /// <returns>the rotated quaternion</returns>
    public static Quat RotateZ (in Quat q, in float cosah, in float sinah)
    {
        Vec3 i = q.imag;
        return new Quat (
            cosah * q.real - sinah * i.z,
            cosah * i.x + sinah * i.y,
            cosah * i.y - sinah * i.x,
            cosah * i.z + sinah * q.real);
    }

    /// <summary>
    /// Converts a quaternion to three axes, which in turn may constitute a
    /// rotation matrix.
    ///
    /// Returns a named value tuple containing the right, forward and up axes.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>tuple</returns>
    public static (Vec3 right, Vec3 forward, Vec3 up) ToAxes (in Quat q)
    {
        float w = q.real;
        Vec3 i = q.imag;
        float x = i.x;
        float y = i.y;
        float z = i.z;

        float x2 = x + x;
        float y2 = y + y;
        float z2 = z + z;

        float xsq2 = x * x2;
        float ysq2 = y * y2;
        float zsq2 = z * z2;

        float xy2 = x * y2;
        float xz2 = x * z2;
        float yz2 = y * z2;

        float wx2 = w * x2;
        float wy2 = w * y2;
        float wz2 = w * z2;

        return (
            right: new Vec3 (
                1.0f - ysq2 - zsq2,
                xy2 + wz2,
                xz2 - wy2),
            forward : new Vec3 (
                xy2 - wz2,
                1.0f - xsq2 - zsq2,
                yz2 + wx2),
            up : new Vec3 (
                xz2 + wy2,
                yz2 - wx2,
                1.0f - xsq2 - ysq2));
    }

    /// <summary>
    /// Converts a quaternion to an axis and angle. The angle is returned from
    /// the function. The axis is assigned to an output vector.
    ///
    /// Returns a named value tuple containing the angle and axis.
    /// </summary>
    /// <param name="q">the quaternion</param>
    /// <returns>a tuple</returns>
    public static (float angle, Vec3 axis) ToAxisAngle (in Quat q)
    {
        float mSq = Quat.MagSq (q);
        if (mSq <= 0.0f)
        {
            return (
                angle: 0.0f,
                axis: Vec3.Forward);
        }

        float wNorm = q.real * Utils.InvSqrtUnchecked (mSq);
        float angle = 2.0f * Utils.Acos (wNorm);
        float wAsin = Utils.Tau - angle;
        // float wAsin = Utils.Pi - angle; float wAsin = Utils.Asin(wNorm);

        if (wAsin == 0.0f)
        {
            return (
                angle: angle,
                axis: Vec3.Forward);
        }

        float sInv = 1.0f / wAsin;
        Vec3 i = q.imag;
        float ax = i.x * sInv;
        float ay = i.y * sInv;
        float az = i.z * sInv;

        float amSq = ax * ax + ay * ay + az * az;
        if (amSq <= 0.0f)
        {
            return (
                angle: angle,
                axis: Vec3.Forward);
        }

        if (Utils.Approx (amSq, 1.0f))
        {
            return (
                angle: angle,
                axis: new Vec3 (ax, ay, az));
        }

        float mInv = Utils.InvSqrtUnchecked (amSq);
        return (
            angle: angle,
            axis: new Vec3 (ax * mInv, ay * mInv, az * mInv));
    }

    /// <summary>
    /// Returns the identity quaternion, where the real component is 1 and the
    /// imaginary components are 0, ( 1.0, 0.0, 0.0, 0.0 ).
    /// </summary>
    /// <value>the identity</value>
    public static Quat Identity
    {
        get
        {
            return new Quat (1.0f, new Vec3 (0.0f, 0.0f, 0.0f));
        }
    }
}