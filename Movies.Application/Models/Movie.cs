using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; set; }

    public string Slug => GenerateSlug();

    public required int YearOfRelease { get; set; }

    public required List<string> Genres { get; init; } = new();

    private static readonly Regex SlugRegex =
        new Regex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, TimeSpan.FromMilliseconds(5));

    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex
            .Replace(Title, string.Empty)
            .ToLowerInvariant()
            .Replace(" ", "-");

        return $"{sluggedTitle}-{YearOfRelease}";
    }
}