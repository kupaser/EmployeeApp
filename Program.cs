using EmployeeApp;
using System.Data.Common;
using System.Threading.Tasks.Dataflow;
using ConsoleTableExt;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

public class Program
{
    //public Context Context;
    public static void Main() 
    {
        while (true)
        {
            Console.WriteLine("Выбери пункт (1-5): ");
            var result = Console.ReadLine()!.ToString();
            switch (result)
            {
                case "1":
                    {
                        Modes.mode1();
                        break;
                    }
                    case "2":
                    {
                        Modes.mode2();
                        break;
                    }
                    case "3":
                    {
                        Modes.mode3();
                        break;
                    }
                    case "4":
                    {
                        Modes.mode4();
                        break;
                    }
                    case "5":
                    {
                        Modes.mode5();
                        break;
                    }
                    //для теста
                    case "dropall":
                    {
                        Context context = new();
                        context.DROPALL();
                        context.SaveChanges();
                        Console.WriteLine("УНИЧТОЖЕНО");
                        break;
                    }
                default:
                    {                        
                        Console.WriteLine("Такого пункта нет!");
                        break;
                    }
            }
        }
    }
}

public static class Modes
{
    //ок
    public static bool mode1()
    {
        try
        {
            Console.Clear();
            using Context context = new();

            var emp = context.Employees.Select(x => x).ToList();
            

            ConsoleTableBuilder
                .From(emp)
                .WithTitle("Сотрудники")
                .WithColumn("Идентификатор","Фамилия","Имя", "Отчество", "Дата рождения", "Пол")
                .ExportAndWriteLine();
            Console.ReadKey();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при выводе строк: {e.Message}");
            return false;
        }
    }
    //ок
    public static bool mode2()
    {
        using Context context = new();
        try
        {
            Console.Clear();
            Console.WriteLine("Имя сотрудника: ");
            string FN = Console.ReadLine()!.ToString();
            Console.WriteLine("Фамилия сотрудника: ");
            string LN = Console.ReadLine()!.ToString();
            Console.WriteLine("Отчество сотрудника: ");
            string OTC = Console.ReadLine()!.ToString();
            Console.WriteLine("Дата рождения сотрудника: (ГГГГ,ММ,ДД)");
            DateOnly BD = DateOnly.Parse(Console.ReadLine()!.ToString());
            Console.WriteLine("Пол: (М/Ж): ");
            bool sex;
            switch (Console.ReadLine()!.ToString())
            {
                case "М":
                    {
                        sex = true;
                        break;
                    }
                case "Ж":
                    {
                        sex = false;
                        break;
                    }
                default:
                    {
                        throw new Exception("Неизвестный символ при выборе пола!");//сочувствую гендерам
                    }
            }
            context.Employees.Add(new(
                FN, LN, OTC, BD, sex
                ));
            context.SaveChanges();
            Console.ReadKey();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"При создании нового сотрудика возникла ошибка: {e.Message}");
            return false;
        }
    }
    //ок
    public static bool mode3()
    {
        try
        {
            Console.Clear();

            using Context context = new();
            var result = context.Employees.ToList()//у EF возникает ошибка если не переводить в лист
                .GroupBy(e => new { e.FirstName, e.LastName, e.OTC})//группируем по ФИО
                .Select(g => g.First())//Выбираем самый первый "уникальный"
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ThenBy(e => e.OTC)
                .ThenBy(e => e.BDate)
                .ToList();

            if (result.Any())
            {
                foreach (var item in result)
                {
                    var gender = item.Sex ? "Мужчина" : "Женщина";

                    Console.WriteLine($"ФИО: {item.FirstName} {item.LastName} {item.OTC}\n" +
                        $"Дата рождения: {item.BDate}\n" +
                        $"Возраст: {item.GetAge()}\n" +
                        $"Пол: {gender}\n\n");
                }
            }
            else
            {
                Console.WriteLine("Уникальных не нашлось");
            }
            Console.ReadKey();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при обработке запроса: {e.Message}");
            return false;
        }
    }
    //ок
    public static bool mode4()
    {
        try
        {
            Console.Clear();
            var random = new Random();
            using Context context = new();
            string[] FirstNames = { "Иван", "Петр", "Сергей", "Мария", "Анна", "Елена" };
            string[] LastNames = { "Иванов.", "Петров.", "Сидоров.", "Федоров.", "Смирнов.", "Кузнецов." };
            string[] MiddleNames = { "Ивано.", "Петров.", "Сергее.", "Алексеев.", "Владимиров." };
            

            for (int i = 0; i < 10000; i++)
            {
                Console.Clear();
                var firstname = FirstNames[random.Next(FirstNames.Length)];
                string lastname;
                if (i < 900)
                {
                    lastname = LastNames[random.Next(LastNames.Length)];
                }
                else
                {
                    lastname = "Федоров.";
                }
                var otc = MiddleNames[random.Next(MiddleNames.Length)];
                var gender = random.Next(3) == 1;

                var year = random.Next(1950, 2005);
                var month = random.Next(1, 13);
                var days = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
                var dtm = new DateOnly(year, month, days);

                context.Employees.Add(new(
                    FirstName: firstname,
                    LastName: lastname,
                    OTC: otc,
                    BDate: dtm,
                    Sex: gender
                    ));
                Console.WriteLine($"Добавлено: {i} сотрудников, последний добавленный: {firstname} {lastname} {otc}");
            }

            context.SaveChanges();
            Console.ReadKey();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при заполнении таблиц: {e.Message}");
            return false;
        }
    }
    //ок
    public static bool mode5()
    {
        try
        {
            Console.Clear();
            var time = new Stopwatch();
            
            time.Start();
            Context context = new();

            //Для SQL запроса (зачем - не знаю):
            //var selected = context.Employees.FromSqlRaw("select * from Employees where Sex = 1 and LastName like 'Ф%' ")

            var selected = context.Employees
                .Where(e => e.Sex == true && e.LastName.IndexOf("Ф") == 0)//теперь на серверной стороне (SQL)                
                .ToList();
            //.AsEnumerable() //на клиентской стороне
            //.Where(e => e.LastName.StartsWith('Ф'))
            time.Stop();
            Console.WriteLine($"Количество затраченного времени: {time.Elapsed.TotalMilliseconds}");
            
            foreach (var item in selected)
            {
                var gender = item.Sex ? "Мужчина" : "Женщина";
                Console.WriteLine($"ФИО - {item.FirstName} {item.LastName} {item.OTC}, Пол - {gender}\n");
            }
            Console.ReadKey();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при обработке запроса: {e.Message}");
            return false;
        }
    }
    
}