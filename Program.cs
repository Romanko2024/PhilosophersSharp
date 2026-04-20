class Program {
    static void Main() {
        Table table = new Table();
        SolutionMode selectedMode = SolutionMode.TRY_LOCK; //HIERARCHY, WAITER, MONITOR, TRY_LOCK 
        
        Console.WriteLine($"Запуск режиму: {selectedMode}");

        for (int i = 0; i < 5; i++) {
            new Philosopher(i, table, selectedMode).Start();
        }
    }
}
