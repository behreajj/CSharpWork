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

            return new Vec3(
                xy + xy - (zw + zw),
                w * w + y * y - x * x - z * z,
                xw + xw + yz + yz);
        }
    }

    /// <summary>
    /// The coefficients of the imaginary components i, j and k.
    /// </summary>
    /// <value>imaginary vector</value>
    public Vec3 Imag { get { return this.imag; } }

    /// <summary>
    /// Returns the number of elements in this quaternion.
    /// </summary>
    /// <value>length</value>
    public int Length { get { return 1 + this.imag.Length; } }

    /// <summary>
    /// The real component.
    /// </summary>
    /// <value>real scalar</value>
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

            return new Vec3(
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

            return new Vec3(
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
    /// <value>element</value>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 or -4 => this.real,
                1 or -3 => this.imag.x,
                2 or -2 => this.imag.y,
                3 or -1 => this.imag.z,
                _ => 0.0f,
            };
        }
    }

    /// <summary>
    /// Constructs a quaternion from a real number and an imaginary vector.
    /// </summary>
    /// <param name="real">real number</param>
    /// <param name="imag">imaginary vector</param>
    public Quat(in float real = 1.0f, in Vec3 imag = new Vec3())
    {
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
    public Quat(in float w = 1.0f, in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
    {
        this.real = w;
        this.imag = new Vec3(x, y, z);
    }

    /// <summary>
    /// Tests this quaternion for equivalence with an object.
    /// </summary>
    /// <param name="value">object</param>
    /// <returns>equivalence</returns>
    public override bool Equals(object value)
    {
        if (Object.ReferenceEquals(this, value)) { return true; }
        if (value is null) { return false; }
        if (value is Quat quat) { return this.Equals(quat); }
        return false;
    }

    /// <summary>
    /// Returns a hash code representing this quaternion.
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = Utils.HashBase;
            hash = hash * Utils.HashMul ^ this.real.GetHashCode();
            hash = hash * Utils.HashMul ^ this.imag.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Returns a string representation of this quaternion.
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return Quat.ToString(this);
    }

    /// <summary>
    /// Tests this quaternion for equivalence with another in compliance with
    /// the IEquatable interface.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>equivalence</returns>
    public bool Equals(Quat q)
    {
        if (this.real.GetHashCode() != q.real.GetHashCode()) { return false; }
        if (this.imag.GetHashCode() != q.imag.GetHashCode()) { return false; }
        return true;
    }

    /// <summary>
    /// Returns an enumerator (or iterator) for this quaternion, allowing its
    /// components to be accessed in a foreach loop. The real component, w, is
    /// treated as the first element.
    /// </summary>
    /// <returns>enumerator</returns>
    public IEnumerator GetEnumerator()
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
    /// <returns>array</returns>
    public float[] ToArray()
    {
        return this.ToArray(new float[this.Length], 0);
    }

    /// <summary>
    /// Puts this quaternion's components into an array at a given index.
    /// The real component, w, is treated as the first element.
    /// </summary>
    /// <param name="arr">array</param>
    /// <param name="i">index</param>
    /// <returns>array</returns>
    public float[] ToArray(in float[] arr, in int i = 0)
    {
        arr[i] = this.real;
        arr[i + 1] = this.imag.x;
        arr[i + 2] = this.imag.y;
        arr[i + 3] = this.imag.z;
        return arr;
    }

    /// <summary>
    /// Returns a named value tuple containing this quaternion's components.
    /// </summary>
    /// <returns>tuple</returns>
    public (float w, float x, float y, float z) ToTuple()
    {
        return (w: this.real,
            this.imag.x, this.imag.y, this.imag.z);
    }

    /// <summary>
    /// Promotes a real number to a quaternion.
    /// </summary>
    /// <param name="real">real number</param>
    public static implicit operator Quat(in float real)
    {
        return new Quat(real, Vec3.Zero);
    }

    /// <summary>
    /// Promotes an imaginary vector to a pure quaternion.
    /// </summary>
    /// <param name="imag">vector</param>
    public static implicit operator Quat(in Vec3 imag)
    {
        return new Quat(0.0f, new Vec3(imag.x, imag.y, imag.z));
    }

    /// <summary>
    /// Converts a four dimensional vector to a quaternion. The vector's w
    /// component is interpreted to be the real component.
    /// </summary>
    /// <param name="v">vector</param>
    public static explicit operator Quat(in Vec4 v)
    {
        return new Quat(v.w, new Vec3(v.x, v.y, v.z));
    }

    /// <summary>
    /// Converts a quaternion to a four dimensional vector. The quaternion's
    /// real component is interpreted to be the w component.
    /// </summary>
    /// <param name="q">quaternion</param>
    public static explicit operator Vec4(in Quat q)
    {
        Vec3 i = q.imag;
        return new Vec4(i.x, i.y, i.z, q.real);
    }

    /// <summary>
    /// Converts a quaternion to a boolean by finding whether any of its
    /// components are non-zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    public static explicit operator bool(in Quat q)
    {
        return Quat.Any(q);
    }

    /// <summary>
    /// A quaternion evaluates to true when any of its components are not equal
    /// to zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool operator true(in Quat q)
    {
        return Quat.Any(q);
    }

    /// <summary>
    /// A quaternion evaluates to false when all of its components are zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool operator false(in Quat q)
    {
        return Quat.None(q);
    }

    /// <summary>
    /// Negates the real and imaginary components of a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>negation</returns>
    public static Quat operator -(in Quat q)
    {
        return new Quat(-q.real, -q.imag);
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
    public static Quat operator *(in Quat a, in Quat b)
    {
        return new Quat(
            (a.real * b.real) -
            Vec3.Dot(a.imag, b.imag),

            Vec3.Cross(a.imag, b.imag) +
            (a.real * b.imag) +
            (b.real * a.imag));
    }

    /// <summary>
    /// Multiplies a quaternion and a scalar.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator *(in Quat a, in float b)
    {
        return new Quat(a.real * b, a.imag * b);
    }

    /// <summary>
    /// Multiplies a scalar and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator *(in float a, in Quat b)
    {
        return new Quat(a * b.real, a * b.imag);
    }

    /// <summary>
    /// Multiplies a quaternion and a vector. The latter is treated as a pure
    /// quaternion, not as a point. Either see MulVector method or use RPR'.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator *(in Quat a, in Vec3 b)
    {
        return new Quat(-Vec3.Dot(a.imag, b),
            Vec3.Cross(a.imag, b) + (a.real * b));
    }

    /// <summary>
    /// Multiplies a vector and a quaternion. The former is treated as a pure
    /// quaternion, not as a point. Either see MulVector method or use RPR'.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>product</returns>
    public static Quat operator *(in Vec3 a, in Quat b)
    {
        return new Quat(-Vec3.Dot(a, b.imag),
            Vec3.Cross(a, b.imag) + (b.real * a));
    }

    /// <summary>
    /// Divides two quaternions. Equivalent to multiplying the numerator and the
    /// inverse of the denominator.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator /(in Quat a, in Quat b)
    {
        if (Quat.Any(b))
        {
            return a * Quat.Inverse(b);
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Divides a quaternion by a real number.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator /(in Quat a, in float b)
    {
        if (b != 0.0f)
        {
            float bInv = 1.0f / b;
            return new Quat(a.real * bInv, a.imag * bInv);
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Divides a real number by a quaternion.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator /(in float a, in Quat b)
    {
        if (Quat.Any(b))
        {
            return a * Quat.Inverse(b);
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Divides a quaternion by a vector. the denominator is treated as a pure
    /// quaternion.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator /(in Quat a, in Vec3 b)
    {
        if (Vec3.Any(b))
        {
            return a * (-b / Vec3.MagSq(b));
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Divides a vector by a quaternion; the numerator is treated as a pure
    /// quaternion.
    /// </summary>
    /// <param name="a">numerator</param>
    /// <param name="b">denominator</param>
    /// <returns>quotient</returns>
    public static Quat operator /(in Vec3 a, in Quat b)
    {
        if (Quat.Any(b))
        {
            return a * Quat.Inverse(b);
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Adds two quaternions.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator +(in Quat a, in Quat b)
    {
        return new Quat(a.real + b.real, a.imag + b.imag);
    }

    /// <summary>
    /// Adds a real number and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator +(in Quat a, in float b)
    {
        return new Quat(a.real + b, a.imag);
    }

    /// <summary>
    /// Adds a real number and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator +(in float a, in Quat b)
    {
        return new Quat(a + b.real, b.imag);
    }

    /// <summary>
    /// Adds an imaginary vector and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator +(in Quat a, in Vec3 b)
    {
        return new Quat(a.real, a.imag + b);
    }

    /// <summary>
    /// Adds an imaginary vector and a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>sum</returns>
    public static Quat operator +(in Vec3 a, in Quat b)
    {
        return new Quat(b.real, a + b.imag);
    }

    /// <summary>
    /// Subtracts the right quaternion from the left.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator -(in Quat a, in Quat b)
    {
        return new Quat(a.real - b.real, a.imag - b.imag);
    }

    /// <summary>
    /// Subtracts a real number from a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator -(in Quat a, in float b)
    {
        return new Quat(a.real - b, a.imag);
    }

    /// <summary>
    /// Subtracts a quaternion from a real number.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator -(in float a, in Quat b)
    {
        return new Quat(a - b.real, -b.imag);
    }

    /// <summary>
    /// Subtracts an imaginary vector from a quaternion.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator -(in Quat a, in Vec3 b)
    {
        return new Quat(a.real, a.imag - b);
    }

    /// <summary>
    /// Subtracts a quaternion from an imaginary vector.
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>difference</returns>
    public static Quat operator -(in Vec3 a, in Quat b)
    {
        return new Quat(-b.real, a - b.imag);
    }

    /// <summary>
    /// Tests to see if all the quaternion's components are non-zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool All(in Quat q)
    {
        return (q.real != 0.0f) && Vec3.All(q.imag);
    }

    /// <summary>
    /// Tests to see if any of the quaternion's components are non-zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool Any(in Quat q)
    {
        return (q.real != 0.0f) || Vec3.Any(q.imag);
    }

    /// <summary>
    /// Evaluates whether or not two quaternions approximate each other.
    /// </summary>
    /// <param name="a">left comparisand</param>
    /// <param name="b">right comparisand</param>
    /// <param name="tolerance">tolerance</param>
    /// <returns>evaluation</returns>
    public static bool Approx(in Quat a, in Quat b, in float tolerance = Utils.Epsilon)
    {
        return Utils.Approx(a.real, b.real, tolerance) &&
            Vec3.Approx(a.imag, b.imag, tolerance);
    }

    /// <summary>
    /// Returns the conjugate of the quaternion, where the imaginary component
    /// is negated.
    ///
    /// a* := { a.real, -a.imag }
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>conjugate</returns>
    public static Quat Conj(in Quat q)
    {
        return new Quat(q.real, -q.imag);
    }

    /// <summary>
    /// Finds the dot product of two quaternions by summing the products of
    /// their corresponding components.
    ///
    /// dot ( a, b ) := a.real b.real + dot ( a.imag, b.imag )
    /// </summary>
    /// <param name="a">left operand</param>
    /// <param name="b">right operand</param>
    /// <returns>dot product</returns>
    public static float Dot(in Quat a, in Quat b)
    {
        return a.real * b.real + Vec3.Dot(a.imag, b.imag);
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
    public static Quat FromAngle(in float radians)
    {
        float rHalf = radians % Utils.Tau * 0.5f;
        float cosa = MathF.Cos(rHalf);
        float sina = MathF.Sin(rHalf);
        return new Quat(cosa, 0.0f, 0.0f, sina);
    }

    /// <summary>
    /// Creates a quaternion from an axis and angle. Normalizes the axis.
    /// If the axis has no magnitude, returns the identity.
    /// </summary>
    /// <param name="radians">angle</param>
    /// <param name="axis">axis</param>
    /// <returns>quaternion</returns>
    public static Quat FromAxisAngle(in float radians, in Vec3 axis)
    {
        float ax = axis.x;
        float ay = axis.y;
        float az = axis.z;
        float amSq = ax * ax + ay * ay + az * az;
        if (amSq > 0.0f)
        {
            float rHalf = radians % Utils.Tau * 0.5f;
            float amInv = MathF.Sin(rHalf) / MathF.Sqrt(amSq);
            return new Quat(MathF.Cos(rHalf), new Vec3(
                    ax * amInv,
                    ay * amInv,
                    az * amInv));
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Creates a quaternion from spherical coordinates. The quaternion's right
    /// axis corresponds to the point on the sphere, i.e., what would be
    /// returned from Vec3.FromSpherical.
    /// </summary>
    /// <param name="azimuth">azimuth</param>
    /// <param name="inclination">axis</param>
    /// <returns>quaternion</returns>
    public static Quat FromSpherical(in float azimuth, in float inclination)
    {
        float azHalf = 0.5f * (azimuth % Utils.Tau);
        float cosAzim = MathF.Cos(azHalf);
        float sinAzim = MathF.Sin(azHalf);

        float inHalf = Utils.Tau - inclination * 0.5f;
        float cosIncl = MathF.Cos(inHalf);
        float sinIncl = MathF.Sin(inHalf);

        return new Quat(
            cosAzim * cosIncl,
            sinAzim * -sinIncl,
            sinIncl * cosAzim,
            sinAzim * cosIncl);
    }

    /// <summary>
    /// Creates a quaternion with reference to two vectors. This function
    /// creates normalized copies of the vectors. Uses the formula:
    ///
    /// fromTo (a, b) := { a . b, a x b }
    /// </summary>
    /// <param name="origin">origin</param>
    /// <param name="dest">destination</param>
    /// <returns>quaternion</returns>
    public static Quat FromTo(in Vec3 origin, in Vec3 dest)
    {
        Vec3 o = Vec3.Normalize(origin);
        Vec3 d = Vec3.Normalize(dest);
        return new Quat(Vec3.Dot(o, d), Vec3.Cross(o, d));
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
    /// <param name="q">quaternion</param>
    /// <returns>inverse</returns>
    public static Quat Inverse(in Quat q)
    {
        // This should be inlined to avoid circularity with division
        // operator definitions above, where divide by zero must return
        // consistent results for floats, vectors, quaternions.
        Vec3 i = q.imag;
        float mSq = q.real * q.real + Vec3.MagSq(i);
        if (mSq != 0.0f)
        {
            float msi = 1.0f / mSq;
            return new Quat(q.real * msi, -i.x * msi, -i.y * msi, -i.z * msi);
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Multiplies a vector by a quaternion's inverse, allowing a prior
    /// rotation to be undone.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <param name="v">input vector</param>
    /// <returns>the unrotated vector</returns>
    public static Vec3 InvMulVector(in Quat q, in Vec3 v)
    {
        float mSq = Quat.MagSq(q);
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

            return new Vec3(
                ix * w + iz * qy - iw * qx - iy * qz,
                iy * w + ix * qz - iw * qy - iz * qx,
                iz * w + iy * qx - iw * qz - ix * qy);
        }
        return Vec3.Zero;
    }

    /// <summary>
    /// Tests if the quaternion is the identity, where its real component is 1.0
    /// and its imaginary components are all zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool IsIdentity(in Quat q)
    {
        return q.real == 1.0f && Vec3.None(q.imag);
    }

    /// <summary>
    /// Tests to see if a quaternion is pure, i.e. if its real component is
    /// zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool IsPure(in Quat q)
    {
        return q.real == 0.0f;
    }

    /// <summary>
    /// Tests if the quaternion is of unit magnitude.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool IsVersor(in Quat q)
    {
        return Utils.Approx(Quat.MagSq(q), 1.0f);
    }

    /// <summary>
    /// Finds the length, or magnitude, of a quaternion.
    ///
    /// |a| := sqrt ( dot ( a, a ) )
    ///
    /// |a| := sqrt ( a a* )
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>magnitude</returns>
    public static float Mag(in Quat q)
    {
        return MathF.Sqrt(Quat.MagSq(q));
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
    /// <param name="q">quaternion</param>
    /// <returns>magnitude squared</returns>
    public static float MagSq(in Quat q)
    {
        return q.real * q.real + Vec3.MagSq(q.imag);
    }

    /// <summary>
    /// Eases between two quaternions by spherical linear interpolation (slerp).
    /// Chooses the shortest path between two orientations and maintains
    /// constant speed for a step in [0.0, 1.0] .
    /// </summary>
    /// <param name="o">origin</param>
    /// <param name="d">destination</param>
    /// <param name="step">step</param>
    /// <returns>quaternion</returns>
    public static Quat Mix(in Quat o, in Quat d, in float step = 0.5f)
    {
        // Decompose origin quaternion.
        Vec3 oi = o.imag;
        float ow = o.real;
        float ox = oi.x;
        float oy = oi.y;
        float oz = oi.z;

        // Decompose destination quaternion.
        Vec3 di = d.imag;
        float dw = d.real;
        float dx = di.x;
        float dy = di.y;
        float dz = di.z;

        // Clamped dot product.
        float dotp = Utils.Clamp(
            ow * dw +
            ox * dx +
            oy * dy +
            oz * dz, -1.0f, 1.0f);

        // Flip values if the orientation is negative.
        if (dotp < 0.0f)
        {
            dw = -dw;
            dx = -dx;
            dy = -dy;
            dz = -dz;
            dotp = -dotp;
        }

        float theta = MathF.Acos(dotp);
        float sinTheta = MathF.Sqrt(1.0f - dotp * dotp);

        // The complementary step, i.e., 1.0 - step.
        float u;

        // The step.
        float v;

        if (sinTheta > Utils.Epsilon)
        {
            float sInv = 1.0f / sinTheta;
            float ang = step * theta;
            u = MathF.Sin(theta - ang) * sInv;
            v = MathF.Sin(ang) * sInv;
        }
        else
        {
            u = 1.0f - step;
            v = step;
        }

        // Unclamped linear interpolation.
        float cw = u * ow + v * dw;
        float cx = u * ox + v * dx;
        float cy = u * oy + v * dy;
        float cz = u * oz + v * dz;

        // Find magnitude squared.
        float mSq = cw * cw + cx * cx + cy * cy + cz * cz;

        // If 0, then invalid, reset to identity.
        if (MathF.Abs(mSq) < Utils.Epsilon)
        {
            return Quat.Identity;
        }

        // If 1, no need to normalize.
        if (MathF.Abs(1.0f - mSq) < Utils.Epsilon)
        {
            return new Quat(cw, cx, cy, cz);
        }

        // Normalize.
        float mInv = 1.0f / MathF.Sqrt(mSq);
        return new Quat(
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
    /// <param name="q">quaternion</param>
    /// <param name="v">input vector</param>
    /// <returns>rotated vector</returns>
    public static Vec3 MulVector(in Quat q, in Vec3 v)
    {
        // Quat product = (q * source) / q; return product.imag;

        float w = q.real;
        Vec3 i = q.imag;
        float qx = i.x;
        float qy = i.y;
        float qz = i.z;

        float iw = -qx * v.x - qy * v.y - qz * v.z;
        float ix = w * v.x + qy * v.z - qz * v.y;
        float iy = w * v.y + qz * v.x - qx * v.z;
        float iz = w * v.z + qx * v.y - qy * v.x;

        return new Vec3(
            ix * w + iz * qy - iw * qx - iy * qz,
            iy * w + ix * qz - iw * qy - iz * qx,
            iz * w + iy * qx - iw * qz - ix * qy);
    }

    /// <summary>
    /// Tests if all components of the quaternion are zero.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>evaluation</returns>
    public static bool None(in Quat q)
    {
        return q.real == 0.0f && Vec3.None(q.imag);
    }

    /// <summary>
    /// Divides a quaternion by its magnitude, such that its new magnitude is
    /// one and it lies on a 4D hyper-sphere. Uses the formula:
    ///
    /// a^ = a / |a|
    ///
    /// Quaternions with zero magnitude will return the identity.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>normalized</returns>
    public static Quat Normalize(in Quat q)
    {
        Vec3 i = q.imag;
        float mSq = q.real * q.real + Vec3.MagSq(i);
        if (mSq > 0.0f)
        {
            float mInv = 1.0f / MathF.Sqrt(mSq);
            return new Quat(q.real * mInv, i.x * mInv, i.y * mInv, i.z * mInv);
        }
        return Quat.Identity;
    }

    /// <summary>
    /// Creates a random unit quaternion. Uses an algorithm by Ken Shoemake,
    /// reproduced at this Math Stack Exchange discussion:
    /// https://math.stackexchange.com/questions/131336/uniform-random-quaternion-in-a-restricted-angle-range
    /// .
    /// </summary>
    /// <param name="rng">random number generator</param>
    /// <returns>random quaternion</returns>
    public static Quat Random(in System.Random rng)
    {
        float t0 = Utils.Tau * (float)rng.NextDouble();
        float t1 = Utils.Tau * (float)rng.NextDouble();

        float r1 = (float)rng.NextDouble();
        float x0 = MathF.Sqrt(1.0f - r1);
        float x1 = MathF.Sqrt(r1);

        float cost0 = MathF.Cos(t0);
        float sint0 = MathF.Sin(t0);
        float cost1 = MathF.Cos(t1);
        float sint1 = MathF.Sin(t1);

        return new Quat(
            x0 * sint0,
            x0 * cost0,
            x1 * sint1,
            x1 * cost1);
    }

    /// <summary>
    /// Rotates a quaternion about the x axis by an angle.
    ///
    /// Do not use sequences of orthonormal rotations by Euler angles; this will
    /// result in gimbal lock, defeating the purpose behind a quaternion.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <param name="radians">angle in radians</param>
    /// <returns>rotated quaternion</returns>
    public static Quat RotateX(in Quat q, in float radians)
    {
        float rHalf = radians % Utils.Tau * 0.5f;
        float cosa = MathF.Cos(rHalf);
        float sina = MathF.Sin(rHalf);
        return Quat.RotateX(q, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the x axis. Accepts calculated sine and cosine
    /// of half the angle so that collections of quaternions can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <param name="cosah">cosine of half the angle</param>
    /// <param name="sinah">sine of half the angle</param>
    /// <returns>rotated quaternion</returns>
    public static Quat RotateX(in Quat q, in float cosah, in float sinah)
    {
        Vec3 i = q.imag;
        return new Quat(
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
    /// <param name="q">quaternion</param>
    /// <param name="radians">angle in radians</param>
    /// <returns>rotated quaternion</returns>
    public static Quat RotateY(in Quat q, in float radians)
    {
        float rHalf = radians % Utils.Tau * 0.5f;
        float cosa = MathF.Cos(rHalf);
        float sina = MathF.Sin(rHalf);
        return Quat.RotateY(q, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the y axis. Accepts calculated sine and cosine
    /// of half the angle so that collections of quaternions can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <param name="cosah">cosine of half the angle</param>
    /// <param name="sinah">sine of half the angle</param>
    /// <returns>rotated quaternion</returns>
    public static Quat RotateY(in Quat q, in float cosah, in float sinah)
    {
        Vec3 i = q.imag;
        return new Quat(
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
    /// <param name="q">quaternion</param>
    /// <param name="radians">angle in radians</param>
    /// <returns>rotated quaternion</returns>
    public static Quat RotateZ(in Quat q, in float radians)
    {
        float rHalf = radians % Utils.Tau * 0.5f;
        float cosa = MathF.Cos(rHalf);
        float sina = MathF.Sin(rHalf);
        return Quat.RotateZ(q, cosa, sina);
    }

    /// <summary>
    /// Rotates a vector around the z axis. Accepts calculated sine and cosine
    /// of half the angle so that collections of quaternions can be efficiently
    /// rotated without repeatedly calling cos and sin.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <param name="cosah">cosine of half the angle</param>
    /// <param name="sinah">sine of half the angle</param>
    /// <returns>rotated quaternion</returns>
    public static Quat RotateZ(in Quat q, in float cosah, in float sinah)
    {
        Vec3 i = q.imag;
        return new Quat(
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
    /// <returns>axes</returns>
    public static (Vec3 right, Vec3 forward, Vec3 up) ToAxes(in Quat q)
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
            right: new Vec3(
                1.0f - ysq2 - zsq2,
                xy2 + wz2,
                xz2 - wy2),
            forward: new Vec3(
                xy2 - wz2,
                1.0f - xsq2 - zsq2,
                yz2 + wx2),
            up: new Vec3(
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
    /// <param name="q">quaternion</param>
    /// <returns>axis angle</returns>
    public static (float angle, Vec3 axis) ToAxisAngle(in Quat q)
    {
        float mSq = Quat.MagSq(q);
        if (mSq <= 0.0f)
        {
            return (
                angle: 0.0f,
                axis: Vec3.Forward);
        }

        float wNorm = q.real * Utils.InvSqrtUnchecked(mSq);
        float angle = 2.0f * MathF.Acos(wNorm);
        float wAsin = Utils.Tau - angle;
        // float wAsin = MathF.PI - angle; float wAsin = Utils.Asin(wNorm);

        if (wAsin == 0.0f)
        {
            return (
                angle,
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
                angle,
                axis: Vec3.Forward);
        }

        if (Utils.Approx(amSq, 1.0f))
        {
            return (
                angle,
                axis: new Vec3(ax, ay, az));
        }

        float mInv = Utils.InvSqrtUnchecked(amSq);
        return (
            angle,
            axis: new Vec3(ax * mInv, ay * mInv, az * mInv));
    }

    /// <summary>
    /// Returns a named value tuple containing the quaternion's azimuth,
    /// theta and inclination, phi. The quaternion's right axis is measured.
    /// </summary>
    /// <param name="q">quaternion</param>
    /// <returns>tuple</returns>
    public static (float theta, float phi) ToSpherical(in Quat q)
    {
        float w = q.real;
        float ix = q.imag.x;
        float iy = q.imag.y;
        float iz = q.imag.z;

        float xy = ix * iy;
        float xz = ix * iz;
        float yw = iy * w;
        float zw = iz * w;

        float vx = w * w + ix * ix - iy * iy - iz * iz;
        float vy = zw + zw + xy + xy;
        float vz = xz + xz - (yw + yw);

        float mSq = vx * vx + vy * vy + vz * vz;
        float inUnsigned = (mSq > 0.0f) ?
            MathF.Acos(vz / MathF.Sqrt(mSq)) :
            Utils.HalfPi;
        return (theta: MathF.Atan2(vy, vx), phi: Utils.HalfPi - inUnsigned);
    }

    /// <summary>
    /// Returns a string representation of a quaternion.
    /// </summary>
    /// <param name="v">quaternion</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string</returns>
    public static string ToString(in Quat q, in int places = 4)
    {
        return Quat.ToString(new StringBuilder(128), q, places).ToString();
    }

    /// <summary>
    /// Appends a representation of a quaternion to a string builder.
    /// </summary>
    /// <param name="sb">string builder</param>
    /// <param name="q">quaternion</param>
    /// <param name="places">number of decimal places</param>
    /// <returns>string builder</returns>
    public static StringBuilder ToString(in StringBuilder sb, in Quat q, in int places = 4)
    {
        sb.Append("{ real: ");
        Utils.ToFixed(sb, q.real, places);
        sb.Append(", imag: ");
        Vec3.ToString(sb, q.imag, places);
        sb.Append(' ');
        sb.Append('}');
        return sb;
    }

    /// <summary>
    /// Returns the identity quaternion, where the real component is 1 and the
    /// imaginary components are 0, ( 1.0, 0.0, 0.0, 0.0 ).
    /// </summary>
    /// <value>identity</value>
    public static Quat Identity
    {
        get
        {
            return new Quat(1.0f, Vec3.Zero);
        }
    }
}