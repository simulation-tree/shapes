using System;
using System.Numerics;
using Unmanaged;

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
            USpan<char> buffer = stackalloc char[32];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = 0;
            buffer[length++] = 'C';
            buffer[length++] = 'u';
            buffer[length++] = 'b';
            buffer[length++] = 'e';
            buffer[length++] = '(';
            length += extents.ToString(buffer.Slice(length));
            buffer[length++] = ')';
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