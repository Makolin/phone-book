using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Phone_Book
{
    #region Таблицы базы данных
    // Таблица с должностями сотрудников
    public class Position
    {
        public int PositionId { get; set; }
        [Required]
        public string PositionName { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица подразделений предприятия
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentNumber { get; set; }
        [Required]
        public string DepartmentFullName { get; set; }
        public string DepartmentShortName { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица местных номеров телефонов
    public class Local
    {
        public int LocalId { get; set; }
        [Required]
        [MaxLength(3)]
        public int LocalNumber { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица городских номеров телефонов
    public class City
    {
        public int CityId { get; set; }
        [Required]
        [MaxLength(6)]
        public int CityNumber { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица для хранения данных об онлайне компьютера пользователя
    public class Computer
    {
        public int ComputerId { get; set; }
        [Required]
        public string NameComputer { get; set; }
        public bool? Status { get; set; }
        public DateTime? LastOnline { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    // Таблица об отсутствии пользователей
    public class Absence
    {
        public int AbsenceId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public User UserAbsence { get; set; }
        [Required]
        public string Reason { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateBefore { get; set; }
    }

    // Таблица пользователей 
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string DomainName { get; set; }
        public DateTime Birthday { get; set; }
        public bool Common { get; set; }

        public int? PositionId { get; set; }
        public Position Position { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? LocalId { get; set; }
        public Local LocalNumber { get; set; }

        public int? CityId { get; set; }
        public City CityNumber { get; set; }

        public int? ComputerId { get; set; }
        public Computer ComputerStatus { get; set; }

        [MaxLength(9)]
        public long? MobileNumber { get; set; }
        public List<Absence> Absences { get; set; } = new List<Absence>();
    }
    #endregion

    // Создание контекста для базы данных
    internal class ApplicationContext : DbContext
    {
        private readonly string connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Local> Locals { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Absence> Absences { get; set; }

        // Создание конструктора, который загружает строку подключения из json файла
        public ApplicationContext()
            : base()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("Resources//appsettings.json");
            IConfigurationRoot config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");
            Database.EnsureCreated();
        }

        // Строка подключения к SQL серверу
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        // Первичные данные при создании базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().HasData(
                new Position[]
                {
                    new Position { PositionId = 1, PositionName = "Начальник отдела" },
                    new Position { PositionId = 2, PositionName = "Начальник бюро" }
                });

            modelBuilder.Entity<Department>().HasData(
                new Department[]
                {
                    new Department
                    {
                        DepartmentId = 1,
                        DepartmentNumber = "12",
                        DepartmentFullName = "Отдел информационных технологий",
                        DepartmentShortName = "Отдел ИТ"
                    },
                    new Department
                    {
                        DepartmentId = 2,
                        DepartmentNumber = "20",
                        DepartmentFullName = "Отдел организации труда и заработной платы",
                        DepartmentShortName = "Отдел организации труда и ЗП"
                    }
                });

            modelBuilder.Entity<Local>().HasData(
                new Local[]
                {
                    new Local { LocalId = 1, LocalNumber = 238 },
                    new Local { LocalId = 2, LocalNumber = 228 },
                    new Local { LocalId = 3, LocalNumber = 140 }
                });

            modelBuilder.Entity<City>().HasData(
                new City[]
                {
                    new City {CityId = 1, CityNumber = 344154 }
                });

            modelBuilder.Entity<Computer>().HasData(
                new Computer[]
                {
                    new Computer {ComputerId = 1, NameComputer = "DESKTOP-MAKOLIN" }
                });

            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User
                    {
                        UserId = 1,
                        Name = "Петров Петр Петрович",
                        Birthday = new DateTime(1992,12,24),
                        DepartmentId = 1,
                        PositionId = 1,
                        LocalId = 1,
                        CityId = 1,
                        MobileNumber = 89099099090,
                        ComputerId = 1
                    },
                    new User
                    {
                        UserId = 2,
                        Name = "Иванов Иван Иванович",
                        Birthday = new DateTime(1992,11,05),
                        DepartmentId = 2,
                        PositionId = 2,
                        LocalId = 2
                    },
                    new User
                    {
                        UserId = 3,
                        Name = "Карлов Карл Карлович",
                        DomainName = "CRYONT\\Vasiliev.MA",
                        DepartmentId = 2,
                        PositionId = 2,
                        LocalId = 2
                    }
                });

            modelBuilder.Entity<Absence>().HasData(
                new Absence[]
                {
                    new Absence
                    {
                        AbsenceId = 1,
                        UserId = 1,
                        Reason = "Отпуск",
                        DateFrom = new DateTime(2020,12,24),
                        DateBefore = new DateTime(2020,12,31)
                    },
                    new Absence
                    {
                        AbsenceId = 2,
                        UserId = 2,
                        Reason = "Декрет"
                    }
                });
        }
    }
}
