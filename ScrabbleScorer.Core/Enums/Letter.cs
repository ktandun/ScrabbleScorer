namespace ScrabbleScorer.Core.Enums;

public enum Letter
{
    Blank,
    E,
    A,
    I,
    O,
    N,
    R,
    T,
    L,
    S,
    U,
    D,
    G,
    B,
    C,
    M,
    P,
    F,
    H,
    V,
    W,
    Y,
    K,
    J,
    X,
    Q,
    Z
}

public static class LetterHelper
{
    public static readonly char[] AllLetters =
    {
        '_', 'A', 'B', 
        'C', 'D', 'E', 
        'F', 'G', 'H', 
        'I', 'J', 'K', 
        'L', 'M', 'N', 
        'O', 'P', 'Q', 
        'R', 'S', 'T', 
        'U', 'V', 'W', 
        'X', 'Y', 'Z'
    };
}

public static class LetterExtensions
{
    public static char ToChar(this Letter letter)
    {
        return letter switch
        {
            Letter.A => 'A',
            Letter.B => 'B',
            Letter.C => 'C',
            Letter.D => 'D',
            Letter.E => 'E',
            Letter.F => 'F',
            Letter.G => 'G',
            Letter.H => 'H',
            Letter.I => 'I',
            Letter.J => 'J',
            Letter.K => 'K',
            Letter.L => 'L',
            Letter.M => 'M',
            Letter.N => 'N',
            Letter.O => 'O',
            Letter.P => 'P',
            Letter.Q => 'Q',
            Letter.R => 'R',
            Letter.S => 'S',
            Letter.T => 'T',
            Letter.U => 'U',
            Letter.V => 'V',
            Letter.W => 'W',
            Letter.X => 'X',
            Letter.Y => 'Y',
            Letter.Z => 'Z',
            Letter.Blank => throw new Exception("Unable to convert blank to char"),
            _ => throw new ArgumentOutOfRangeException(nameof(letter), letter, null)
        };
    }
    public static Letter ToLetter(this char letter) =>
        letter switch
        {
            '_' => Letter.Blank,
            'A' => Letter.A,
            'B' => Letter.B,
            'C' => Letter.C,
            'D' => Letter.D,
            'E' => Letter.E,
            'F' => Letter.F,
            'G' => Letter.G,
            'H' => Letter.H,
            'I' => Letter.I,
            'J' => Letter.J,
            'K' => Letter.K,
            'L' => Letter.L,
            'M' => Letter.M,
            'N' => Letter.N,
            'O' => Letter.O,
            'P' => Letter.P,
            'Q' => Letter.Q,
            'R' => Letter.R,
            'S' => Letter.S,
            'T' => Letter.T,
            'U' => Letter.U,
            'V' => Letter.V,
            'W' => Letter.W,
            'X' => Letter.X,
            'Y' => Letter.Y,
            'Z' => Letter.Z,
            'a' => Letter.A,
            'b' => Letter.B,
            'c' => Letter.C,
            'd' => Letter.D,
            'e' => Letter.E,
            'f' => Letter.F,
            'g' => Letter.G,
            'h' => Letter.H,
            'i' => Letter.I,
            'j' => Letter.J,
            'k' => Letter.K,
            'l' => Letter.L,
            'm' => Letter.M,
            'n' => Letter.N,
            'o' => Letter.O,
            'p' => Letter.P,
            'q' => Letter.Q,
            'r' => Letter.R,
            's' => Letter.S,
            't' => Letter.T,
            'u' => Letter.U,
            'v' => Letter.V,
            'w' => Letter.W,
            'x' => Letter.X,
            'y' => Letter.Y,
            'z' => Letter.Z,
            _ => throw new ArgumentOutOfRangeException(nameof(letter), letter, null)
        };

    public static int ToLetterValue(this char letter) =>
        letter switch
        {
            '_' => (int)Letter.Blank,
            'A' => (int)Letter.A,
            'B' => (int)Letter.B,
            'C' => (int)Letter.C,
            'D' => (int)Letter.D,
            'E' => (int)Letter.E,
            'F' => (int)Letter.F,
            'G' => (int)Letter.G,
            'H' => (int)Letter.H,
            'I' => (int)Letter.I,
            'J' => (int)Letter.J,
            'K' => (int)Letter.K,
            'L' => (int)Letter.L,
            'M' => (int)Letter.M,
            'N' => (int)Letter.N,
            'O' => (int)Letter.O,
            'P' => (int)Letter.P,
            'Q' => (int)Letter.Q,
            'R' => (int)Letter.R,
            'S' => (int)Letter.S,
            'T' => (int)Letter.T,
            'U' => (int)Letter.U,
            'V' => (int)Letter.V,
            'W' => (int)Letter.W,
            'X' => (int)Letter.X,
            'Y' => (int)Letter.Y,
            'Z' => (int)Letter.Z,
            _ => throw new ArgumentOutOfRangeException(nameof(letter), letter, null)
        };
}