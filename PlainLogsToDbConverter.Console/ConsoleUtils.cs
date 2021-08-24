namespace PlainLogsToDbConverter.Console
{
    internal static class ConsoleUtils
    {
        public static void WriteLineInColor(string value, System.ConsoleColor color)
        {
            var originalColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(value);
            System.Console.ForegroundColor = originalColor;
        }
    }
}
