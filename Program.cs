namespace Tic_Tac_Toe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.ShowIntro();
            game.PlayGame();

            // Press any key to close the game
            Console.ReadKey();
        }
    }

    public class Game
    {
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private Player? winner;
        private int turn;

        // Basic game setup
        public Game()
        {
            player1 = new Player(Symbol.X, "Player 1");
            player1 = new Player(Symbol.O, "Player 2");
            currentPlayer = player1;
            winner = null;
            turn = 0;
        }

        public void ShowIntro()
        {
            Console.WriteLine($"Let's play Tic-Tac-Toe!\n");
            Console.WriteLine($"Use the numpad on your keyboard to pick the square you want:\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" 7 | 8 | 9 ");
            Console.WriteLine($"---+---+---");
            Console.WriteLine($" 4 | 5 | 6 ");
            Console.WriteLine($"---+---+---");
            Console.WriteLine($" 1 | 2 | 3 \n");
            Console.ResetColor();
        }

        public void PlayGame()
        {
            // Start game
            while (turn < 9)
            {
                turn++;
                Game.Play(currentPlayer, turn);
                Console.Clear();
                GameBoard.DrawBoard();

                if (Game.CheckForWin(currentPlayer.Symbol))
                {
                    winner = currentPlayer;
                    break;
                }

                // Switch players if there's still no winner and loop again
                // Swapped to a "ternary" syntax
                currentPlayer = currentPlayer == player1 ? player2 : player1;
            }

            // Result
            if (winner == player1)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{player1.Name} wins!");
            }
            else if (winner == player2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{player2.Name} wins!");
            }
            else
            {
                Console.WriteLine("It's a draw.");
            }
        }

        public static void Play(Player player, int turn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Turn: {turn}\n");
            Console.ResetColor();

            while (true)
            {
                Console.Write($"{player.Name}, it's your turn. Pick a square: ");
                int input = Convert.ToInt32(Console.ReadLine());

                // GetCoordinates returnes a tuple
                // Can return -1, -1 if input was outside of 1-9, which will fail the CheckLegalInput below
                var (x, y) = GameBoard.GetCoordinates(input);

                // Also checks if square is not already taken
                if (GameBoard.CheckLegalInput(x, y))
                {
                    GameBoard.UpdateSquare(x, y, player.Symbol);
                    break;
                }
            }
        }

        public static bool CheckForWin(Symbol symbol)
        {
            // Rows
            if (GameBoard.Board[0, 0] == symbol && GameBoard.Board[0, 1] == symbol && GameBoard.Board[0, 2] == symbol) return true;
            if (GameBoard.Board[1, 0] == symbol && GameBoard.Board[1, 1] == symbol && GameBoard.Board[1, 2] == symbol) return true;
            if (GameBoard.Board[2, 0] == symbol && GameBoard.Board[2, 1] == symbol && GameBoard.Board[2, 2] == symbol) return true;

            // Columns
            if (GameBoard.Board[0, 0] == symbol && GameBoard.Board[1, 0] == symbol && GameBoard.Board[2, 0] == symbol) return true;
            if (GameBoard.Board[0, 1] == symbol && GameBoard.Board[1, 1] == symbol && GameBoard.Board[2, 1] == symbol) return true;
            if (GameBoard.Board[0, 2] == symbol && GameBoard.Board[1, 2] == symbol && GameBoard.Board[2, 2] == symbol) return true;

            // Diagonals
            if (GameBoard.Board[0, 0] == symbol && GameBoard.Board[1, 1] == symbol && GameBoard.Board[2, 2] == symbol) return true;
            if (GameBoard.Board[0, 2] == symbol && GameBoard.Board[1, 1] == symbol && GameBoard.Board[2, 0] == symbol) return true;

            return false;
        }
    }

    public class Player
    {
        public Symbol Symbol { get; }
        public string Name { get; }

        public Player(Symbol symbol, string name)
        {
            Symbol = symbol;
            Name = name;
        }
    }

    public class GameBoard
    {
        public static Symbol[,] Board { get; private set; } = new Symbol[3, 3]
        {
            { Symbol.Empty, Symbol.Empty, Symbol.Empty },
            { Symbol.Empty, Symbol.Empty, Symbol.Empty },
            { Symbol.Empty, Symbol.Empty, Symbol.Empty }
        };

        public static (int X, int Y) GetCoordinates(int input)
        {
            switch (input)
            {
                case 7:
                    return (0, 0);
                case 8:
                    return (0, 1);
                case 9:
                    return (0, 2);
                case 4:
                    return (1, 0);
                case 5:
                    return (1, 1);
                case 6:
                    return (1, 2);
                case 1:
                    return (2, 0);
                case 2:
                    return (2, 1);
                case 3:
                    return (2, 2);
                default:
                    return (-1, -1);
            }
        }

        public static void UpdateSquare(int x, int y, Symbol symbol)
        {
            Board[x, y] = symbol;
        }

        public static bool CheckLegalInput(int x, int y)
        {
            // GetCoordinates default returns -1, -1 if inputs were outside of 1-9
            if (x < 0 || x > 2 || y < 0 || y > 2)
            {
                Console.WriteLine("Please enter a number between 1 and 9.");
                return false;
            }
            if (Board[x, y] != Symbol.Empty)
            {
                Console.WriteLine("This square is already taken.");
                return false;
            }
            else return true;
        }

        public static void DrawBoard()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine($" {ShowSquareAs(Board[0, 0])} | {ShowSquareAs(Board[0, 1])} | {ShowSquareAs(Board[0, 2])} ");
            Console.WriteLine($"---+---+---");
            Console.WriteLine($" {ShowSquareAs(Board[1, 0])} | {ShowSquareAs(Board[1, 1])} | {ShowSquareAs(Board[1, 2])} ");
            Console.WriteLine($"---+---+---");
            Console.WriteLine($" {ShowSquareAs(Board[2, 0])} | {ShowSquareAs(Board[2, 1])} | {ShowSquareAs(Board[2, 2])} ");
            Console.WriteLine();
            Console.ResetColor();
        }

        private static string ShowSquareAs(Symbol symbol) => symbol switch
        {
            Symbol.Empty => " ",
            Symbol.X => "X",
            Symbol.O => "O",
        };
    }

    public enum Symbol { Empty, X, O };
}