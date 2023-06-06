# CSharpWork
 
This repository's original aim was to be roughly analogous to the library [Cam Z-Up](https://github.com/behreajj/CamZup/) for [Processing](https://processing.org/), save that it worked in conjunction with the [Unity](https://unity.com/) game development engine.

Because it has had to walk a line between independence from -- and compatibility with -- Unity's C# Scripting API, certain naming conventions were followed to avoid collisions. For example, `Quat` is short for `Quaternion`; `ClrGradient` is elongated from `Gradient`; and so on.

Since its origin, this repository's focus has evolved. Unlike its Java Processing counterpart, there is a greater emphasis on immutability in basic math objects through `struct`s. It now priveleges perceptual color over standard RGB where possible. Support for HSL and HSV has been removed. [SR LAB 2](https://www.magnetkern.de/srlab2.html) and its LCH variant are included.