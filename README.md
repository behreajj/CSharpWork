# CSharpWork
 
The C# files included are here are intended to walk a line between independence from -- and compatibility with -- Unity's C# Scripting API. For that reason, certain names are changed to avoid collisions: `Quat` is short for `Quaternion`; `ClrGradient` is elongated from `Gradient`; and so on.

Friendliness with Unity's API also means choices need to be made about encapsulation. A `public` field of a `Serializable` `struct` may appear in Unity's inspector. However, marking the `struct` and/or its fields `readonly` prevents this.