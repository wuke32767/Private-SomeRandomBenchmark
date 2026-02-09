using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace MiscTests;

[SimpleJob()]
[DisassemblyDiagnoser(printInstructionAddresses: true, syntax: DisassemblySyntax.Intel)]
public class Hitbox : Collider
{
    public float width;
    public float height;

    public sealed override float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => width;
    }

    public sealed override float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => height;
    }
    public sealed override float Left
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pos.x;
    }
    public sealed override float Right
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pos.x + Width;
    }
    public sealed override float Top
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pos.y;
    }
    public sealed override float Bottom
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pos.y + Height;
    }

    [Params(0, 1, 2)]
    public int rectidx { get; set; }
    public static Rectangle[] rects = new[] {
        new Rectangle(),
        new Rectangle() {
            x = 10,
            y = (10 * 1001) % 3100,
            width = 10 * 1002 % 3100,
            height = 10 * 1002 % 3100
        },
        new Rectangle() {
            x = 100,
            y = (100 * 1001) % 3100,
            width = 100 * 1002 % 3100,
            height = 100 * 1002 % 3100
        }
    };


    [Benchmark]
    public void BenchmarkVanilla() => Collide(rects[rectidx]);

    [Benchmark]
    public void BenchmarkNew() => Collide2(rects[rectidx]);

    public bool Collide(Rectangle rect)
    {
        return base.AbsoluteRight > rect.Left
        && base.AbsoluteBottom > rect.Top
        && base.AbsoluteLeft < rect.Right
        && base.AbsoluteTop < rect.Bottom;
    }

    public bool Collide2(Rectangle rect)
    {
        Vector2 pos = AbsolutePosition;

        return pos.x + width > rect.Left
            && pos.y + height > rect.Top
            && pos.x < rect.Right
            && pos.y < rect.Bottom;
    }
}


public abstract class Collider
{
    public Vector2 pos;

    public Vector2 EntityPos;
    public object? EntityRef = new(); // This should be a property but it does not matter here

    public Vector2 AbsolutePosition
    {
        get
        {
            if (EntityRef != null)
                return pos + EntityPos;
            return pos;
        }
    }
    public abstract float Left { get; }
    public abstract float Right { get; }
    public abstract float Top { get; }
    public abstract float Bottom { get; }
    public abstract float Width { get; }
    public abstract float Height { get; }

    public float AbsoluteRight
    {
        get
        {
            if (EntityRef != null)
                return Right + EntityPos.x;
            return Right;
        }
    }

    public float AbsoluteLeft
    {
        get
        {
            if (EntityRef != null)
                return Left + EntityPos.x;
            return Left;
        }
    }

    public float AbsoluteBottom
    {
        get
        {
            if (EntityRef != null)
                return Bottom + EntityPos.y;
            return Bottom;
        }
    }

    public float AbsoluteTop
    {
        get
        {
            if (EntityRef != null)
                return Top + EntityPos.y;
            return Top;
        }
    }

}

public struct Rectangle
{
    public int x;
    public int y;
    public int width;
    public int height;

    public int Left => x;
    public int Right => x + width;
    public int Top => y;
    public int Bottom => y + height;
}

public struct Vector2
{
    public float x;
    public float y;

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        a.x += b.x;
        a.y += b.y;
        return a;
    }
}