using System;
using System.Numerics;

namespace Shapes.Types
{
    public readonly struct CubeShape : IShape, IEquatable<CubeShape>
    {
        public readonly Vector3 extents;

        public readonly Vector3 Size => extents * 2;

        readonly byte IShape.TypeIndex => 2;

#if NET
        /// <summary>
        /// Creates a cube shape with no size.
        /// </summary>
        public CubeShape()
        {
        }
#endif

        public CubeShape(Vector3 extents)
        {
            this.extents = extents;
        }

        public CubeShape(float extent)
        {
            extents = new(extent);
        }

        public CubeShape(float x, float y, float z)
        {
            extents = new(x, y, z);
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> destination)
        {
            int length = 0;
            destination[length++] = 'C';
            destination[length++] = 'u';
            destination[length++] = 'b';
            destination[length++] = 'e';
            destination[length++] = '(';
            length += extents.ToString(destination.Slice(length));
            destination[length++] = ')';
            return length;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is CubeShape shape && Equals(shape);
        }

        public readonly bool Equals(CubeShape other)
        {
            return extents.Equals(other.extents);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(extents);
        }

        public static implicit operator Shape(CubeShape shape)
        {
            return Shape.Create(shape);
        }

        public static bool operator ==(CubeShape left, CubeShape right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeShape left, CubeShape right)
        {
            return !(left == right);
        }
    }
}