using System.Collections.Immutable;
using ScrabbleScorer.Core.Constants;

namespace ScrabbleScorer.Core.Extensions;

public static class CoordinateExtensions
{
    public static bool IsWithinBoardDimensions(this Coordinate coordinate)
    {
        return coordinate.X is >= 1 and <= BoardCoordinateConstants.BoardSize
               && coordinate.Y is >= 1 and <= BoardCoordinateConstants.BoardSize;
    }

    public static Coordinate FirstNonBlank(
        this Coordinate coordinate,
        ImmutableHashSet<Coordinate> occupiedCoordinates,
        Alignment alignment
    )
    {
        var currCoordinate = coordinate;

        while (currCoordinate.Prev(alignment, true).IsWithinBoardDimensions())
        {
            if (!occupiedCoordinates.Contains(currCoordinate.Prev(alignment)))
                break;

            currCoordinate = currCoordinate.Prev(alignment);
        }

        return currCoordinate;
    }

    public static Coordinate LastNonBlank(
        this Coordinate coordinate,
        ImmutableHashSet<Coordinate> occupiedCoordinates,
        Alignment alignment
    )
    {
        var currCoordinate = coordinate;

        while (currCoordinate.Next(alignment, true).IsWithinBoardDimensions())
        {
            if (!occupiedCoordinates.Contains(currCoordinate.Next(alignment)))
                break;

            currCoordinate = currCoordinate.Next(alignment);
        }

        return currCoordinate;
    }

    public static IEnumerable<Coordinate> GetSurrounding(this Coordinate coordinate)
    {
        if ((coordinate with { X = coordinate.X - 1 }).IsWithinBoardDimensions())
            yield return coordinate with
            {
                X = coordinate.X - 1
            };

        if ((coordinate with { Y = coordinate.Y - 1 }).IsWithinBoardDimensions())
            yield return coordinate with
            {
                Y = coordinate.Y - 1
            };

        if ((coordinate with { X = coordinate.X + 1 }).IsWithinBoardDimensions())
            yield return coordinate with
            {
                X = coordinate.X + 1
            };

        if ((coordinate with { Y = coordinate.Y + 1 }).IsWithinBoardDimensions())
            yield return coordinate with
            {
                Y = coordinate.Y + 1
            };
    }

    public static IEnumerable<Coordinate> To(
        this Coordinate start,
        Coordinate end,
        Alignment alignment
    )
    {
        var diff = Math.Abs(alignment == Alignment.Horizontal ? end.X - start.X : end.Y - start.Y);

        if (alignment == Alignment.Horizontal)
            return Enumerable
                .Range(Math.Min(start.X, end.X), diff + 1)
                .Select(r => start with { X = r });

        return Enumerable
            .Range(Math.Min(start.Y, end.Y), diff + 1)
            .Reverse()
            .Select(r => start with { Y = r });
    }

    public static Coordinate Prev(
        this Coordinate coordinate,
        Alignment alignment,
        bool peek = false
    )
    {
        var prev = new Coordinate(
            alignment == Alignment.Horizontal ? coordinate.X - 1 : coordinate.X,
            alignment == Alignment.Vertical ? coordinate.Y + 1 : coordinate.Y
        );

        if (!peek && !prev.IsWithinBoardDimensions())
            throw new ArgumentOutOfRangeException($"{prev.X}, {prev.Y} {alignment.ToString()}");

        return prev;
    }

    public static Coordinate Next(
        this Coordinate coordinate,
        Alignment alignment,
        bool peek = false
    )
    {
        var next = new Coordinate(
            alignment == Alignment.Horizontal ? coordinate.X + 1 : coordinate.X,
            alignment == Alignment.Vertical ? coordinate.Y - 1 : coordinate.Y
        );

        if (!peek && !next.IsWithinBoardDimensions())
            throw new ArgumentOutOfRangeException($"{next.X}, {next.Y} {alignment.ToString()}");

        return next;
    }
}