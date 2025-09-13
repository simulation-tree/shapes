using System;
using System.Diagnostics;

namespace Shapes
{
    public unsafe struct Shape : IEquatable<Shape>
    {
        public readonly byte TypeIndex
        {
            get
            {
                fixed (float* data = this.data)
                {
                    uint header = *(uint*)data;
                    return (byte)(header & 0xFF);
                }
            }
        }

        private fixed float data[8];

#if NET
        [Obsolete("Default constructor not supported", true)]
        public Shape()
        {
            throw new NotSupportedException();
        }
#endif

        private Shape(byte type, float* data)
        {
            fixed (float* destination = this.data)
            {
                *(uint*)destination = type;
            }

            this.data[1] = data[0];
            this.data[2] = data[1];
            this.data[3] = data[2];
            this.data[4] = data[3];
            this.data[5] = data[4];
            this.data[6] = data[5];
            this.data[7] = data[6];
        }

        public readonly bool Is<T>(out T shape) where T : unmanaged, IShape
        {
            if (TypeIndex == default(T).TypeIndex)
            {
                fixed (float* data = this.data)
                {
                    shape = *(T*)(data + 1);
                    return true;
                }
            }
            else
            {
                shape = default;
                return false;
            }
        }

        public readonly bool Is<T>() where T : unmanaged, IShape
        {
            return TypeIndex == default(T).TypeIndex;
        }

        public readonly T Read<T>() where T : unmanaged, IShape
        {
            ThrowIfTypeIsNot<T>();

            fixed (float* data = this.data)
            {
                return *(T*)(data + 1);
            }
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Shape shape && Equals(shape);
        }

        public readonly bool Equals(Shape other)
        {
            fixed (float* data = this.data)
            {
                float* otherData = other.data;
                return *(ulong*)data == *(ulong*)otherData &&
                       data[2] == otherData[2] &&
                       data[3] == otherData[3] &&
                       data[4] == otherData[4] &&
                       data[5] == otherData[5] &&
                       data[6] == otherData[6] &&
                       data[7] == otherData[7];
            }
        }

        public readonly override int GetHashCode()
        {
            int hash = 17;
            fixed (float* data = this.data)
            {
                hash = hash * 31 + *(int*)data;
                hash = hash * 31 + data[2].GetHashCode();
                hash = hash * 31 + data[3].GetHashCode();
                hash = hash * 31 + data[4].GetHashCode();
                hash = hash * 31 + data[5].GetHashCode();
                hash = hash * 31 + data[6].GetHashCode();
                hash = hash * 31 + data[7].GetHashCode();
            }

            return hash;
        }

        public static Shape Create<T>(T shape) where T : unmanaged, IShape
        {
            ThrowIfSizeIsTooGreat<T>();

            void* shapePointer = &shape;
            return new Shape(shape.TypeIndex, (float*)shapePointer);
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfTypeIsNot<T>() where T : unmanaged, IShape
        {
            if (TypeIndex != default(T).TypeIndex)
            {
                throw new InvalidOperationException($"The shape is not of type {typeof(T).Name}");
            }
        }

        [Conditional("DEBUG")]
        private static void ThrowIfSizeIsTooGreat<T>() where T : unmanaged
        {
            int maxSize = sizeof(float) * 7;
            int size = sizeof(T);
            if (size > maxSize)
            {
                throw new ArgumentException($"The size of {typeof(T).Name} is too great. The maximum amount of floats that can be stored is 7");
            }
        }

        public static bool operator ==(Shape left, Shape right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Shape left, Shape right)
        {
            return !(left == right);
        }
    }
}