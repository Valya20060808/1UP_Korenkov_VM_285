using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using UP_Korenkov_VM_285.Models;

namespace UP_Korenkov_VM_285.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RequestHistory> RequestHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Master)
                .WithMany(u => u.AssignedRequests)
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(u => u.ClientRequests)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.EquipmentType)
                .WithMany()
                .HasForeignKey(r => r.EquipmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestStatus)
                .WithMany()
                .HasForeignKey(r => r.RequestStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Master)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Request)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestHistory>()
                .HasOne(h => h.Request)
                .WithMany()
                .HasForeignKey(h => h.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestHistory>()
                .HasOne(h => h.User)
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Менеджер" },
                new Role { Id = 2, Name = "Специалист" },
                new Role { Id = 3, Name = "Оператор" },
                new Role { Id = 4, Name = "Заказчик" },
                new Role { Id = 5, Name = "Менеджер по качеству" }
            );

            modelBuilder.Entity<RequestStatus>().HasData(
                new RequestStatus { Id = 1, Name = "В процессе ремонта" },
                new RequestStatus { Id = 2, Name = "Готова к выдаче" },
                new RequestStatus { Id = 3, Name = "Новая заявка" },
                new RequestStatus { Id = 4, Name = "Ожидание комплектующих" }
            );

            modelBuilder.Entity<EquipmentType>().HasData(
                new EquipmentType { Id = 1, Name = "Кондиционер" },
                new EquipmentType { Id = 2, Name = "Увлажнитель воздуха" },
                new EquipmentType { Id = 3, Name = "Сушилка для рук" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Широков Василий Матвеевич", Phone = "89210563128", Login = "login1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass1"), RoleId = 1 },
                new User { Id = 2, FullName = "Кудрявцева Ева Ивановна", Phone = "89535078985", Login = "login2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass2"), RoleId = 2 },
                new User { Id = 3, FullName = "Гончарова Ульяна Ярославовна", Phone = "89210673849", Login = "login3", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass3"), RoleId = 2 },
                new User { Id = 4, FullName = "Гусева Виктория Данииловна", Phone = "89990563748", Login = "login4", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass4"), RoleId = 3 },
                new User { Id = 5, FullName = "Баранов Артём Юрьевич", Phone = "89994563847", Login = "login5", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass5"), RoleId = 3 },
                new User { Id = 6, FullName = "Овчинников Фёдор Никитич", Phone = "89219567849", Login = "login6", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass6"), RoleId = 4 },
                new User { Id = 7, FullName = "Петров Никита Артёмович", Phone = "89219567841", Login = "login7", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass7"), RoleId = 4 },
                new User { Id = 8, FullName = "Ковалева Софья Владимировна", Phone = "89219567842", Login = "login8", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass8"), RoleId = 4 },
                new User { Id = 9, FullName = "Кузнецов Сергей Матвеевич", Phone = "89219567843", Login = "login9", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass9"), RoleId = 4 },
                new User { Id = 10, FullName = "Беспалова Екатерина Даниэльевна", Phone = "89219567844", Login = "login10", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass10"), RoleId = 2 }
            );

            modelBuilder.Entity<Request>().HasData(
                new Request
                {
                    Id = 1,
                    StartDate = DateTime.SpecifyKind(DateTime.Parse("2023-06-06"), DateTimeKind.Utc),
                    EquipmentTypeId = 1,
                    EquipmentModel = "TCL TAC-12CHSA/TPG-W белый",
                    ProblemDescription = "Не охлаждает воздух",
                    RequestStatusId = 1,
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = 2,
                    ClientId = 7
                },
                new Request
                {
                    Id = 2,
                    StartDate = DateTime.SpecifyKind(DateTime.Parse("2023-05-05"), DateTimeKind.Utc),
                    EquipmentTypeId = 1,
                    EquipmentModel = "Electrolux EACS/I-09HAT/N3_21Y белый",
                    ProblemDescription = "Выключается сам по себе",
                    RequestStatusId = 1,
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = 3,
                    ClientId = 8
                },
                new Request
                {
                    Id = 3,
                    StartDate = DateTime.SpecifyKind(DateTime.Parse("2022-07-07"), DateTimeKind.Utc),
                    EquipmentTypeId = 2,
                    EquipmentModel = "Xiaomi Smart Humidifier 2",
                    ProblemDescription = "Пар имеет неприятный запах",
                    RequestStatusId = 2,
                    CompletionDate = DateTime.SpecifyKind(DateTime.Parse("2023-01-01"), DateTimeKind.Utc),
                    RepairParts = null,
                    MasterId = 3,
                    ClientId = 9
                },
                new Request
                {
                    Id = 4,
                    StartDate = DateTime.SpecifyKind(DateTime.Parse("2023-08-02"), DateTimeKind.Utc),
                    EquipmentTypeId = 2,
                    EquipmentModel = "Polaris PUH 2300 WIFI IQ Home",
                    ProblemDescription = "Увлажнитель воздуха продолжает работать при предельном снижении уровня воды",
                    RequestStatusId = 3,
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = null,
                    ClientId = 8
                },
                new Request
                {
                    Id = 5,
                    StartDate = DateTime.SpecifyKind(DateTime.Parse("2023-08-02"), DateTimeKind.Utc),
                    EquipmentTypeId = 3,
                    EquipmentModel = "Ballu BAHD-1250",
                    ProblemDescription = "Не работает",
                    RequestStatusId = 3,
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = null,
                    ClientId = 9
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, Message = "Всё сделаем!", MasterId = 2, RequestId = 1 },
                new Comment { Id = 2, Message = "Всё сделаем!", MasterId = 3, RequestId = 2 },
                new Comment { Id = 3, Message = "Починим в момент.", MasterId = 3, RequestId = 3 }
            );
        }
    }
}