using System;
using Unmanaged;

namespace Shapes.Types
{
    public readonly struct SphereShape : IShape, IEquatable<SphereShape>
    {
        public readonly float radius;

        public readonly float Diameter => radius * 2;

        readonly byte IShape.TypeIndex => 1;

        public SphereShape(float radius)
        {
            this.radius = radius;
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
            buffer[length++] = 'S';
            buffer[length++] = 'p';
            buffer[length++] = 'h';
            buffer[length++] = 'e';
            buffer[length++] = 'r';
            buffer[length++] = 'e';
            buffer[length++] = '(';
            length += radius.ToString(buffer.Slice(length));
            buffer[length++] = ')';
            return length;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is SphereShape shape && Equals(shape);
        }

        public readonly bool Equals(SphereShape other)
        {
            return radius == other.radius;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(radius);
        }

        public static implicit operator Shape(SphereShape shape)
        {
            return Shape.Create(shape);
        }

        public static bool operator ==(SphereShape left, SphereShape right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SphereShape left, SphereShape right)
        {
            return !(left == right);
        }
    }
}