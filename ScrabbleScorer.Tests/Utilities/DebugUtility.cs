using HandlebarsDotNet;
using ScrabbleScorer.Core.Enums;

namespace ScrabbleScorer.Tests.Utilities;

public static class DebugUtility
{
    public static void DrawInHtml(
        Coordinate[] occupiedCoordinates,
        (Coordinate coordinate, Alignment alignment)[] possibleLetterCoordinates,
        int wordLength
    )
    {
        const string source = """
                    <html lang="en">
                        <meta charset="UTF-8">
                        <title>Debug</title>
                        <meta name="viewport" content="width=device-width,initial-scale=1">
                        <head>
                            <style>
                                table {
                                    border-collapse: collapse;
                                }
                                td {
                                    width: 40px;
                                    height: 40px;
                                    border: 1px solid black;
                                    text-align: center;
                                }
                                .vert {
                                    background-color: #FFCCCB; // light red
                                }
                                .hori {
                                    background-color: #ADD8E6; // light blue
                                }
                                .occupied {
                                    background-color: yellow;
                                }
                                .vert-hori {
                                    background-color: #8467D7; // light purple
                                }
                            </style>
                        </head>
                        <body>
                            <table>
                                {{#each grid}}
                                    <tr>
                                    {{#each this}}
                                        <td
                                            {{#if this.Occupied}}class='occupied'
                                            {{else}}
                                                {{#if this.BothVerticalHorizontal}}class='vert-hori'
                                                {{else}}
                                                    {{#if this.Vertical}}class='vert'{{/if}}
                                                    {{#if this.Horizontal}}class='hori'{{/if}}
                                                {{/if}}
                                            {{/if}}
                                            >
                                            ({{this.x}},{{this.y}})
                                        </td>
                                    {{/each}}
                                    </tr>
                                {{/each}}
                            </table>
                        </body>
                        </html>
            """;

        var template = Handlebars.Compile(source);
        var data = new
        {
            grid = ShowHighlightedGrids(occupiedCoordinates, possibleLetterCoordinates),
        };

        var html = template(data);

        var filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            $"test-{wordLength}.html"
        );
        File.WriteAllText(filePath, html);
    }

    private static object[][] ShowHighlightedGrids(
        Coordinate[] occupiedCoordinates,
        (Coordinate coordinate, Alignment alignment)[] possibleCoords
    )
    {
        var grid = (
            from y in Enumerable.Range(1, 15).Reverse()
            select (
                from x in Enumerable.Range(1, 15)
                select new
                {
                    BothVerticalHorizontal = possibleCoords.Any(c =>
                        c.coordinate.X == x
                        && c.coordinate.Y == y
                        && c.alignment == Alignment.Horizontal
                    )
                        && possibleCoords.Any(c =>
                            c.coordinate.X == x
                            && c.coordinate.Y == y
                            && c.alignment == Alignment.Vertical
                        ),
                    Vertical = possibleCoords.Any(c =>
                        c.coordinate.X == x
                        && c.coordinate.Y == y
                        && c.alignment == Alignment.Vertical
                    ),
                    Horizontal = possibleCoords.Any(c =>
                        c.coordinate.X == x
                        && c.coordinate.Y == y
                        && c.alignment == Alignment.Horizontal
                    ),
                    Occupied = occupiedCoordinates.Any(oc => oc.X == x && oc.Y == y),
                    X = x,
                    Y = y,
                }
            ).ToArray()
        ).ToArray();

        return grid;
    }
}
