class Program
{
    static async Task Main(string[] args)
    {
        int start = 0;
        int end = 100;
        
        List<int> primes = await FindPrimesAsync(start, end);
        
        Console.WriteLine($"Prime numbers between {start} and {end}: {string.Join(", ", primes)}");
    }

    static async Task<List<int>> FindPrimesAsync(int start, int end)
    {
        var tasks = new List<Task<(int number, bool isPrime)>>();

        for (int number = start; number <= end; number++)
        {
            tasks.Add(IsPrimeAsync(number));
        }

        var results = await Task.WhenAll(tasks);

        return results.Where(result => result.isPrime).Select(result => result.number).ToList();  
    }

    static Task<(int number, bool isPrime)> IsPrimeAsync(int number)
    {
        return Task.Run(() =>
        {
            bool isPrime = IsPrime(number);
            return (number, isPrime);
        });
    }

    static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }
}

