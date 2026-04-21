public class Philosopher {
    private readonly int id;
    private readonly int leftFork, rightFork;
    private readonly Table table;
    private readonly SolutionMode mode;
    private readonly Random random = new Random();
    public Philosopher(int id, Table table, SolutionMode mode) {
        this.id = id;
        this.table = table;
        this.mode = mode;
        this.rightFork = id;
        this.leftFork = (id + 1) % 5;
    }
    public void Start() {
        new Thread(Run).Start();
    }
    private void Run() {
        for (int i = 0; i < 10; i++) {
            Console.WriteLine($"Philosopher {id} is thinking {i + 1} times");
            Thread.Sleep(random.Next(50, 150));

            PickForks();

            Console.WriteLine($"Philosopher {id} is EATING {i + 1} times");
            Thread.Sleep(random.Next(50, 150));

            PutForks();
        }
        Console.WriteLine($"--- Philosopher {id} FINISHED ---");
    }
    private void PickForks() {
        switch (mode) {
            case SolutionMode.HIERARCHY:
                int first = Math.Min(leftFork, rightFork);
                int second = Math.Max(leftFork, rightFork);
                table.GetFork(first);
                table.GetFork(second);
                break;
            case SolutionMode.WAITER:
                table.AskWaiter();
                table.GetFork(rightFork);
                table.GetFork(leftFork);
                break;
            case SolutionMode.MONITOR:
                table.GetBothForks(leftFork, rightFork);
                break;
            case SolutionMode.TRY_LOCK:
                while (true) {
                    table.GetFork(rightFork);
                    if (table.TryGetFork(leftFork)) return;
                    table.PutFork(rightFork);
                    Thread.Sleep(random.Next(10, 30));
                }
        }
    }

    private void PutForks() {
        if (mode == SolutionMode.MONITOR) {
            table.PutBothForks(leftFork, rightFork);
        } else {
            table.PutFork(leftFork);
            table.PutFork(rightFork);
            if (mode == SolutionMode.WAITER) table.ReleaseWaiter();
        }
    }
}