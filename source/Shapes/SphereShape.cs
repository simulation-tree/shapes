using System;

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
            Span<char> buffer = stackalloc char[32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> destination)
        {
            int length = 0;
            destination[length++] = 'S';
            destination[length++] = 'p';
            destination[length++] = 'h';
            destination[length++] = 'e';
            destination[length++] = 'r';
            destination[length++] = 'e';
            destination[length++] = '(';
            length += radius.ToString(destination.Slice(length));
            destination[length++] = ')';
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