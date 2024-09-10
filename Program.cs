using Company;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            while (true) { Function.Start(); }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}