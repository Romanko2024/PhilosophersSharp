public class Table {
    private readonly SemaphoreSlim[] forks = new SemaphoreSlim[5];
    private readonly SemaphoreSlim waiter = new SemaphoreSlim(4);
    private readonly bool[] forkInUse = new bool[5]; // Для MONITOR
    private readonly object monitorLock = new object(); // Об'єкт для lock
    public Table() {
        for (int i = 0; i < 5; i++) {
            forks[i] = new SemaphoreSlim(1, 1);
            forkInUse[i] = false;
        }
    }
    //HIERARCHY, WAITER, TRY_LOCK
    public void GetFork(int id) => forks[id].Wait();
    //TRY_LOCK
    public bool TryGetFork(int id) => forks[id].Wait(0); // 0 означає "спробувати і не чекати"
    //HIERARCHY, WAITER, TRY_LOCK
    public void PutFork(int id) => forks[id].Release();
    //WAITER
    public void AskWaiter() => waiter.Wait();
    public void ReleaseWaiter() => waiter.Release();
    //MONITOR
    public void GetBothForks(int left, int right) {
        lock (monitorLock) {
            while (forkInUse[left] || forkInUse[right]) {
                Monitor.Wait(monitorLock);
            }
            forkInUse[left] = true;
            forkInUse[right] = true;
        }
    }
    public void PutBothForks(int left, int right) {
        lock (monitorLock) {
            forkInUse[left] = false;
            forkInUse[right] = false;
            Monitor.PulseAll(monitorLock);
        }
    }
}