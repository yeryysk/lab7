using System;

namespace Rozumatic_ASD_Lab7
{
    // ==========================================
    // БАЗОВА ЧАСТИНА: СИМУЛЯЦІЯ ROUND-ROBIN
    // ==========================================
    
    public class ProcessItem
    {
        public string Name { get; set; }
        public int RemainingTimeMs { get; set; }

        public ProcessItem(string name, int remainingTimeMs)
        {
            Name = name;
            RemainingTimeMs = remainingTimeMs;
        }
    }

    public class CircularNode
    {
        public ProcessItem Process { get; set; }
        public CircularNode Next { get; set; }

        public CircularNode(ProcessItem process)
        {
            Process = process;
            Next = this; // За замовчуванням посилається сам на себе
        }
    }

    public class RoundRobinScheduler
    {
        private CircularNode? _tail = null;
        private int _count = 0;

        public int Count => _count;

        // Додавання процесу в кільце
        public void AddProcess(ProcessItem proc)
        {
            CircularNode newNode = new CircularNode(proc);
            if (_tail == null)
            {
                _tail = newNode;
                _tail.Next = _tail;
            }
            else
            {
                newNode.Next = _tail.Next;
                _tail.Next = newNode;
                _tail = newNode;
            }
            _count++;
        }

        // Симуляція планувальника Round-Robin
        public void Run(int timeQuantumMs)
        {
            if (_tail == null) return;

            Console.WriteLine($"\n🚀 Запуск диспетчера процесора (Квант часу = {timeQuantumMs} ms)...\n");
            
            CircularNode prev = _tail;
            CircularNode curr = _tail.Next;
            int round = 1;

            while (_count > 0)
            {
                Console.WriteLine($"--- Раунд #{round++} ---");
                Console.Write($"  ⚙️ Процес [{curr.Process.Name}] (залишок: {curr.Process.RemainingTimeMs} ms) отримує CPU -> ");

                if (curr.Process.RemainingTimeMs <= timeQuantumMs)
                {
                    Console.WriteLine($"ЗАВЕРШЕНО за {curr.Process.RemainingTimeMs} ms! ✅");
                    
                    // Видаляємо завершений вузол з кільця
                    if (_count == 1)
                    {
                        _tail = null;
                        _count = 0;
                        break;
                    }
                    else
                    {
                        prev.Next = curr.Next;
                        if (curr == _tail) _tail = prev;
                        curr = curr.Next;
                        _count--;
                    }
                }
                else
                {
                    curr.Process.RemainingTimeMs -= timeQuantumMs;
                    Console.WriteLine($"Квант вичерпано, новий залишок: {curr.Process.RemainingTimeMs} ms ⏳");
                    prev = curr;
                    curr = curr.Next;
                }
            }

            Console.WriteLine("\n✨ Усі процеси в черзі успішно виконано!");
        }
    }

    // ==========================================
    // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ: ВАРІАНТ 1
    // Задача Йосипа Флавія (Josephus Problem)
    // ==========================================
    
    public class JosephusProblem
    {
        // Крок з умови завдання
        private const int STEP = 2024; // Спрощене значення замість @@ROZUMATIC_MATH_PH_O__. 
                                       // Якщо у вас є точне число, замініть 2024 на нього.

        public static void Solve(int n)
        {
            if (n <= 0) return;

            // 1. Створюємо циклічний список з n людей (0 до n-1)
            CircularNode? head = null;
            CircularNode? prev = null;

            for (int i = 0; i < n; i++)
            {
                CircularNode newNode = new CircularNode(new ProcessItem($"Людина {i}", 0));
                if (head == null)
                {
                    head = newNode;
                    head.Next = head;
                }
                else
                {
                    prev!.Next = newNode;
                    newNode.Next = head;
                }
                prev = newNode;
            }

            // 2. Алгоритм виключення
            CircularNode curr = head;
            CircularNode prevNode = prev; // Вказує на останній елемент перед head

            Console.WriteLine($"\n🔪 Задача Йосипа Флавія (N = {n}, крок = {STEP}). Починаємо...");

            while (curr.Next != curr) // Поки не залишиться один
            {
                // Робимо STEP-1 кроків, щоб знайти того, кого видаляємо
                for (int i = 1; i < STEP; i++)
                {
                    prevNode = curr;
                    curr = curr.Next;
                }

                // Видаляємо curr
                prevNode!.Next = curr.Next;
                // Console.WriteLine($"Видалено: {curr.Process.Name}"); // Розкоментуйте для детального логу
                curr = prevNode.Next;
            }

            Console.WriteLine($"\n🏆 Переможець (вцілілий) у Задачі Йосипа Флавія: {curr.Process.Name}");
        }
    }

    // ==========================================
    // ГОЛОВНИЙ КЛАС ПРОГРАМИ
    // ==========================================
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №7: ROUND-ROBIN");
            Console.WriteLine("==================================================");

            // --- ЧАСТИНА 1: Базова симуляція ---
            RoundRobinScheduler scheduler = new RoundRobinScheduler();
            scheduler.AddProcess(new ProcessItem("IDE Rider", 25));
            scheduler.AddProcess(new ProcessItem("Browser Chrome", 40));
            scheduler.AddProcess(new ProcessItem("Telegram Bot", 15));
            scheduler.AddProcess(new ProcessItem("Audio Player", 10));

            scheduler.Run(timeQuantumMs: 15);

            // --- ЧАСТИНА 2: Індивідуальне завдання (Варіант 1) ---
            Console.WriteLine("\n==================================================");
            Console.WriteLine(" ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (ВАРІАНТ 1)");
            Console.WriteLine("==================================================");
            
            // Перевіримо на 41 людині (класичний приклад для Йосипа Флавія)
            JosephusProblem.Solve(41); 
        }
    }
}
