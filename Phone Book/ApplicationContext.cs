using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Phone_Book
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица местных номеров телефонов
    public class Local
    {
        public int LocalId { get; set; }
        public int LocalNumber { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица городских номеров телефонов
    public class City
    {
        public int CityId { get; set; }
        public int CityNumber { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица пользователей 
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? LocalId { get; set; }
        public Local LocalNumber { get; set; }

        public int? CityId { get; set; }
        public City CityNumber { get; set; }

        public long? MobileNumber { get; set; }
        public string Absence { get; set; }
    }
    // Создание контекста для базы данных
    class ApplicationContext : DbContext
    {
        private string connectionString;
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<Local> Locals { get; set; }
        public DbSet<City> Cities { get; set; }

        // Создание конструктора, который загружает строку подключения из json файла
        public ApplicationContext()
            : base()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        // Первичные данные при создании базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department[]
                {
                    new Department { DepartmentId=1 ,DepartmentName = "Отдел №12" },
                    new Department { DepartmentId=2, DepartmentName = "Отдел №20" }
                });

            modelBuilder.Entity<Local>().HasData(
                new Local[]
                {
                    new Local { LocalId=1, LocalNumber = 238 },
                    new Local { LocalId=2, LocalNumber = 228 },
                    new Local { LocalId=3, LocalNumber = 140 }
                });

            modelBuilder.Entity<City>().HasData(
                new City[]
                {
                    new City {CityId = 1, CityNumber = 344154 }
                });

            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User
                    {
                        UserId = 1,
                        Name = "Петров Петр Петрович",
                        DepartmentId = 1,
                        Position = "Начальник отдела",
                        LocalId = 1,
                        CityId = 1,
                        MobileNumber = 89099099090,
                        Absence = "Отпуск по 16.11.2020"
                    },
                    new User
                    {
                        UserId = 2,
                        Name = "Иванов Иван Иванович",
                        DepartmentId = 2,
                        Position = "Начальник бюро",
                        LocalId = 2
                    }
                });
        }
    }
}
