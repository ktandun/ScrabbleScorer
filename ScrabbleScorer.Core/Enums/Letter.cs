namespace ScrabbleScorer.Core.Enums;

public enum Letter
{
    Blank,
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z
}

public static class LetterHelper
{
    public static readonly char[] AllLetters =
    {
        '_', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z'
    };
}

public static class LetterExtensions
{
    extension(Letter letter)
    {
        public char ToChar()
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
                Letter.Blank => '_',
                _ => throw new ArgumentOutOfRangeException(nameof(letter), letter, null)
            };
        }

        public int ToLetterValue()
        {
            return letter switch
            {
                Letter.A => 1,
                Letter.B => 3,
                Letter.C => 3,
                Letter.D => 2,
                Letter.E => 1,
                Letter.F => 4,
                Letter.G => 2,
                Letter.H => 4,
                Letter.I => 1,
                Letter.J => 8,
                Letter.K => 5,
                Letter.L => 1,
                Letter.M => 3,
                Letter.N => 1,
                Letter.O => 1,
                Letter.P => 3,
                Letter.Q => 10,
                Letter.R => 1,
                Letter.S => 1,
                Letter.T => 1,
                Letter.U => 1,
                Letter.V => 4,
                Letter.W => 4,
                Letter.X => 8,
                Letter.Y => 4,
                Letter.Z => 10,
                _ => 0
            };
        }
    }

    extension(char letter)
    {
        public Letter ToLetter()
        {
            return letter switch
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
        }
    }
}