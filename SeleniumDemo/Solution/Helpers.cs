namespace SeleniumDemo.Solution;

public static class Helpers
{
    public static bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && y >= 0 && x <= Config.BoardWith - 1 && y <= Config.BoardHeight - 1;
    }
}
