# CSharpWork
 
The C# files included are here are intended to walk a line between independence from -- and compatibility with -- Unity's C# Scripting API. For that reason, certain names are changed to avoid collisions: `Quat` is short for `Quaternion`; `Clr`, for `Color`. `ColorGradient` is elongated from `Gradient`. And so on.

Friendliness with Unity's API also means choices need to be made about encapsulation. A `public` field of a `Serializable` `struct` may appear in Unity's inspector. However, marking the `struct` and/or its fields `readonly` prevents this.

This also trickles down to style convention. Mostly I've tried to follow a convention for capitalizing methods and properties `Struct.Method();` . However, in the case of vectors, quaternions and colors, where one letter properties are common, lower-case has been used: `Vec3 v = new Vec3(); float a = v.x;` .