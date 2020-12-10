using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;

namespace Phone_Book
{
    #region Таблицы базы данных
    // Таблица с должностями сотрудников
    public class Position
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица для подразделения
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
        [MaxLength(3)]
        public int LocalNumber { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица городских номеров телефонов
    public class City
    {
        public int CityId { get; set; }
        [MaxLength(6)]
        public int CityNumber { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица пользователей 
    public class User 
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public int? PositionId { get; set; }
        public Position Position { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? LocalId { get; set; }
        public Local LocalNumber { get; set; }

        public int? CityId { get; set; }
        public City CityNumber { get; set; }

        [MaxLength(9)]
        public long? MobileNumber { get; set; }
        public string Absence { get; set; }
    }
    #endregion
    // Создание контекста для базы данных
    class ApplicationContext : DbContext
    {
        private string connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Local> Locals { get; set; }
        public DbSet<City> Cities { get; set; }

        // Создание конструктора, который загружает строку подключения из json файла
        public ApplicationContext()
            : base()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("Resources//appsettings.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Подключение к sql серверу
            optionsBuilder.UseSqlServer(connectionString);
            //optionsBuilder.UseSqlite("Filename=phonebook.db");
        }

        // Первичные данные при создании базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().HasData(
             new Position[]
                {
                     new Position { PositionId=1 ,PositionName = "Начальник отдела" },
                     new Position { PositionId=2, PositionName = "Начальник бюро" }
                });

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
                        PositionId = 1,
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
                        PositionId = 2,
                        LocalId = 2
                    }
                });
        }
    }
}
