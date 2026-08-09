using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShelterApi.Models;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShelterApi.date;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Shelter> Shelters => Set<Shelter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shelter>()
            .HasOne(e => e.Area)
            .WithMany(e => e.Shelters)
            .HasForeignKey(e => e.AreaCode)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inspection>()
           .HasOne(e => e.Shelter)
           .WithMany(e => e.Inspections)
           .HasForeignKey(e => e.ShelterId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);
        USE ShelterDb;

      Clear existing data(in correct order due to foreign keys)
DELETE FROM Inspections;
        DELETE FROM Shelters;
        DELETE FROM Areas;

        --Reset auto - increment
ALTER TABLE Areas AUTO_INCREMENT = 1;
        ALTER TABLE Shelters AUTO_INCREMENT = 1;
        ALTER TABLE Inspections AUTO_INCREMENT = 1;

        -- =====================================================
        --AREAS(10 areas across different cities)
        -- =====================================================

        INSERT INTO Areas(City, Neighborhood, AreaCode, RiskLevel) VALUES
        ('Tel Aviv', 'Florentin', 'TLV-FLR', 3),
('Tel Aviv', 'Neve Tzedek', 'TLV-NTZ', 3),
('Tel Aviv', 'Old Jaffa', 'TLV-YFO', 4),
('Haifa', 'Hadar', 'HFA-HDR', 2),
('Haifa', 'French Carmel', 'HFA-KRM', 2),
('Jerusalem', 'City Center', 'JRS-CTR', 5),
('Jerusalem', 'Givat Shaul', 'JRS-GSH', 4),
('Beer Sheva', 'Ramot', 'BRS-RMT', 4),
('Beer Sheva', 'Old City', 'BRS-OLD', 3),
('Ashdod', 'Ganei Ashdod', 'ASD-GAN', 5);

        -- =====================================================
        --SHELTERS(50 shelters distributed across areas)
        -- =====================================================

        --Tel Aviv - Florentin(Area 1) - 6 shelters
        INSERT INTO Shelters(Name, Street, BuildingNumber, Capacity, IsAccessible, IsPublic, ShelterType, AreaId) VALUES
        ('Alon School Shelter', 'Vital Street', '12', 200, 1, 1, 'School', 1),
('Florentin Parking Shelter', 'Salame Street', '8', 350, 1, 1, 'Parking', 1),
('Aviv Residential Building', 'Abarbanel Street', '25', 80, 0, 0, 'Residential', 1),
('Community Center Shelter', 'HaAliya Street', '15', 150, 1, 1, 'PublicBuilding', 1),
('Great Synagogue Shelter', 'Herzl Street', '33', 120, 0, 1, 'PublicBuilding', 1),
('Florentin Mall Shelter', 'Levinsky Street', '101', 400, 1, 1, 'Commercial', 1),

--Tel Aviv - Neve Tzedek(Area 2) - 4 shelters
('Shevazi School Shelter', 'Shevazi Street', '18', 180, 1, 1, 'School', 2),
('Rokach Parking Shelter', 'Rokach Street', '5', 250, 1, 1, 'Parking', 2),
('Historic Building Shelter', 'Neve Tzedek Boulevard', '7', 60, 0, 1, 'PublicBuilding', 2),
('New Residential Tower', 'Ahronowitz Street', '42', 100, 1, 0, 'Residential', 2),

--Tel Aviv - Old Jaffa(Area 3) - 5 shelters
('Jaffa Port Shelter', 'Port Wharf', '1', 500, 1, 1, 'PublicBuilding', 3),
('Flea Market Shelter', 'Olei Zion Street', '10', 200, 0, 1, 'Commercial', 3),
('Tabor School Shelter', 'Jerusalem Boulevard', '88', 220, 1, 1, 'School', 3),
('Carmel Market Parking', 'HaCarmel Street', '3', 300, 1, 1, 'Parking', 3),
('Jaffa Lighthouse Shelter', 'Lighthouse Road', '1', 50, 0, 1, 'PublicBuilding', 3),

--Haifa - Hadar(Area 4) - 6 shelters
('Bialik School Shelter', 'Bialik Street', '22', 250, 1, 1, 'School', 4),
('Hadar Parking Shelter', 'Herzl Street', '45', 400, 1, 1, 'Parking', 4),
('City Hall Shelter', 'Hassan Shukri Street', '14', 180, 1, 1, 'PublicBuilding', 4),
('Panorama Mall Shelter', 'HaNasi Boulevard', '123', 600, 1, 1, 'Commercial', 4),
('Hadar Residential Building', 'Herzl Street', '67', 90, 0, 0, 'Residential', 4),
('Central Synagogue Shelter', 'HaNevi\'im Street', '8', 100, 0, 1, 'PublicBuilding', 4),

--Haifa - French Carmel(Area 5) - 4 shelters
('Reali School Shelter', 'Shmaryahu Levin Street', '1', 300, 1, 1, 'School', 5),
('Carmel Parking Shelter', 'Yefe Nof Street', '80', 350, 1, 1, 'Parking', 5),
('Luxury Residential Tower', 'HaNasi Boulevard', '200', 120, 1, 0, 'Residential', 5),
('Culture Center Shelter', 'Abba Hushi Avenue', '38', 200, 1, 1, 'PublicBuilding', 5),

--Jerusalem - City Center(Area 6) - 7 shelters
('Etz Haim School Shelter', 'Jaffa Street', '100', 280, 1, 1, 'School', 6),
('Mamilla Parking Shelter', 'Mamilla Avenue', '5', 450, 1, 1, 'Parking', 6),
('Mamilla Mall Shelter', 'Mamilla Avenue', '8', 700, 1, 1, 'Commercial', 6),
('City Hall Jerusalem', 'Safra Square', '1', 200, 1, 1, 'PublicBuilding', 6),
('City Center Residential', 'Ben Yehuda Street', '55', 100, 0, 0, 'Residential', 6),
('Central Bus Station Shelter', 'Jaffa Street', '224', 800, 1, 1, 'PublicBuilding', 6),
('Great Synagogue Jerusalem', 'King George Street', '58', 150, 1, 1, 'PublicBuilding', 6),

--Jerusalem - Givat Shaul(Area 7) - 4 shelters
('Givat Shaul School', 'Givat Shaul Street', '12', 200, 1, 1, 'School', 7),
('Har Nof Parking Shelter', 'Har Nof Street', '3', 300, 1, 1, 'Parking', 7),
('Malha Mall Shelter', 'Malha Road', '1', 650, 1, 1, 'Commercial', 7),
('Residential Building GS', 'Shmuel HaNavi Street', '78', 85, 0, 0, 'Residential', 7),

--Beer Sheva - Ramot(Area 8) - 5 shelters
('Ramot School Shelter', 'Ramot Street', '5', 220, 1, 1, 'School', 8),
('Ramot Parking Shelter', 'Ramot Boulevard', '10', 280, 1, 1, 'Parking', 8),
('Grand Canyon Mall Shelter', 'Herzl Street', '1', 900, 1, 1, 'Commercial', 8),
('Community Center Ramot', 'Ramot Street', '18', 160, 1, 1, 'PublicBuilding', 8),
('Residential Building Ramot', 'Ramot Street', '42', 95, 1, 0, 'Residential', 8),

--Beer Sheva - Old City(Area 9) - 4 shelters
('Old City School Shelter', 'Herzl Street', '88', 180, 0, 1, 'School', 9),
('Central Parking BS', 'Herzl Street', '120', 320, 1, 1, 'Parking', 9),
('City Market Shelter', 'Herzl Street', '95', 250, 0, 1, 'Commercial', 9),
('City Hall Beer Sheva', 'Ben Gurion Boulevard', '2', 200, 1, 1, 'PublicBuilding', 9),

--Ashdod - Ganei Ashdod(Area 10) - 5 shelters
('Ashdod School Shelter', 'Herzl Street', '30', 240, 1, 1, 'School', 10),
('Lev Ashdod Parking', 'Rogozin Street', '7', 400, 1, 1, 'Parking', 10),
('Lev Ashdod Mall Shelter', 'Rogozin Street', '1', 850, 1, 1, 'Commercial', 10),
('Sports Center Shelter', 'HaHistadrut Street', '5', 300, 1, 1, 'PublicBuilding', 10),
('New Residential Ashdod', 'Bar Kochva Street', '15', 110, 1, 0, 'Residential', 10);

        -- =====================================================
        --INSPECTIONS(150 inspections distributed across shelters)
        -- =====================================================

        --Multiple inspections per shelter, with varying dates and scores
        -- This supports queries about latest inspections, averages, failed inspections, etc.
        

        -- Shelter 1(School in Florentin) - 4 inspections
        INSERT INTO Inspections(InspectionDate, ReadinessScore, Passed, DefectsCount, Notes, ShelterId) VALUES
        ('2023-06-15 09:00:00', 85, 1, 2, 'Good condition, minor lighting issues', 1),
('2023-09-20 10:30:00', 78, 1, 3, 'Improved from previous inspection', 1),
('2024-01-10 14:00:00', 92, 1, 1, 'Excellent condition', 1),
('2024-03-05 11:15:00', 88, 1, 1, 'Excellent ongoing maintenance', 1),

--Shelter 2(Parking in Florentin) - 3 inspections
('2023-07-01 08:00:00', 72, 1, 5, 'Maintenance required', 2),
('2023-11-15 09:30:00', 68, 0, 7, 'Failed - ventilation issues', 2),
('2024-02-20 13:00:00', 75, 1, 4, 'Improved after repairs', 2),

--Shelter 3(Residential in Florentin) - 2 inspections
('2023-08-10 10:00:00', 55, 0, 12, 'Poor condition, extensive repairs needed', 3),
('2024-01-25 15:00:00', 62, 0, 9, 'Slight improvement but still insufficient', 3),

--Shelter 4(Community Center in Florentin) - 3 inspections
('2023-05-20 11:00:00', 90, 1, 1, 'Excellent condition', 4),
('2023-10-05 14:30:00', 87, 1, 2, 'Good maintenance', 4),
('2024-02-15 10:00:00', 91, 1, 1, 'Continues at high level', 4),

--Shelter 5(Synagogue in Florentin) - 2 inspections
('2023-09-01 09:00:00', 65, 0, 8, 'Accessibility issues', 5),
('2024-01-20 11:00:00', 70, 1, 6, 'Slightly improved', 5),

--Shelter 6(Mall in Florentin) - 4 inspections
('2023-06-10 08:30:00', 95, 1, 0, 'Perfect condition', 6),
('2023-09-15 10:00:00', 93, 1, 1, 'High standard', 6),
('2023-12-20 13:00:00', 94, 1, 0, 'No defects', 6),
('2024-03-10 09:30:00', 96, 1, 0, 'Continued excellence', 6),

--Shelter 7(School in Neve Tzedek) - 3 inspections
('2023-07-15 10:00:00', 82, 1, 3, 'Good condition', 7),
('2023-11-20 11:30:00', 80, 1, 3, 'Stable', 7),
('2024-02-28 14:00:00', 84, 1, 2, 'Slight improvement', 7),

--Shelter 8(Parking in Neve Tzedek) - 3 inspections
('2023-08-05 09:00:00', 76, 1, 4, 'Fair', 8),
('2023-12-10 10:30:00', 78, 1, 3, 'Improvement', 8),
('2024-03-01 13:00:00', 79, 1, 3, 'Continued improvement', 8),

--Shelter 9(Historic Building in Neve Tzedek) - 2 inspections
('2023-09-10 11:00:00', 58, 0, 10, 'Old building, many issues', 9),
('2024-02-05 14:00:00', 64, 0, 8, 'Minor improvement', 9),

--Shelter 10(New Residential in Neve Tzedek) - 2 inspections
('2023-10-15 10:00:00', 88, 1, 2, 'New building, good condition', 10),
('2024-03-12 11:00:00', 90, 1, 1, 'Excellent', 10),

--Shelter 11(Jaffa Port) - 4 inspections
('2023-06-20 08:00:00', 70, 1, 6, 'Maintenance needed', 11),
('2023-09-25 09:30:00', 73, 1, 5, 'Slight improvement', 11),
('2023-12-30 11:00:00', 75, 1, 4, 'Continued improvement', 11),
('2024-03-15 13:30:00', 77, 1, 3, 'Positive trend', 11),

--Shelter 12(Flea Market) - 2 inspections
('2023-08-15 10:00:00', 60, 0, 9, 'Infrastructure problems', 12),
('2024-01-30 12:00:00', 66, 0, 7, 'Improvement insufficient', 12),

--Shelter 13(School Tabor) - 3 inspections
('2023-07-20 09:00:00', 86, 1, 2, 'Good condition', 13),
('2023-11-25 10:30:00', 88, 1, 2, 'Stable', 13),
('2024-02-25 14:00:00', 89, 1, 1, 'Improvement', 13),

--Shelter 14(Carmel Market Parking) - 3 inspections
('2023-08-20 08:30:00', 74, 1, 5, 'Fair', 14),
('2023-12-15 10:00:00', 76, 1, 4, 'Improvement', 14),
('2024-03-08 12:00:00', 78, 1, 3, 'Continued improvement', 14),

--Shelter 15(Jaffa Lighthouse) - 2 inspections
('2023-09-15 11:00:00', 52, 0, 14, 'Old structure, serious issues', 15),
('2024-02-10 13:00:00', 58, 0, 11, 'Minor improvement', 15),

--Shelter 16(School Bialik Haifa) - 4 inspections
('2023-06-25 09:00:00', 91, 1, 1, 'Excellent', 16),
('2023-10-10 10:30:00', 89, 1, 2, 'Very good', 16),
('2024-01-15 14:00:00', 92, 1, 1, 'Outstanding', 16),
('2024-03-20 11:15:00', 93, 1, 0, 'Perfect', 16),

--Shelter 17(Hadar Parking) - 3 inspections
('2023-07-25 08:00:00', 80, 1, 3, 'Good', 17),
('2023-11-30 09:30:00', 82, 1, 2, 'Improvement', 17),
('2024-03-05 13:00:00', 84, 1, 2, 'Continued improvement', 17),

--Shelter 18(City Hall Haifa) - 3 inspections
('2023-08-25 10:00:00', 87, 1, 2, 'Good condition', 18),
('2023-12-20 11:00:00', 88, 1, 1, 'Improvement', 18),
('2024-03-18 14:00:00', 90, 1, 1, 'Excellent', 18),

--Shelter 19(Panorama Mall Haifa) - 4 inspections
('2023-06-30 08:30:00', 94, 1, 1, 'Excellent', 19),
('2023-10-15 10:00:00', 95, 1, 0, 'Perfect', 19),
('2024-01-20 13:00:00', 96, 1, 0, 'No defects', 19),
('2024-03-25 09:30:00', 97, 1, 0, 'Outstanding level', 19),

--Shelter 20(Residential Hadar) - 2 inspections
('2023-09-20 10:00:00', 63, 0, 8, 'Maintenance issues', 20),
('2024-02-15 12:00:00', 68, 0, 6, 'Slight improvement', 20),

--Shelter 21(Synagogue Haifa) - 2 inspections
('2023-10-20 11:00:00', 71, 1, 5, 'Fair', 21),
('2024-03-10 13:00:00', 74, 1, 4, 'Improvement', 21),

--Shelter 22(Reali School) - 3 inspections
('2023-07-30 09:00:00', 89, 1, 2, 'Excellent', 22),
('2023-12-05 10:30:00', 90, 1, 1, 'Outstanding', 22),
('2024-03-15 14:00:00', 91, 1, 1, 'Continues at high level', 22),

--Shelter 23(Carmel Parking) - 3 inspections
('2023-08-30 08:00:00', 77, 1, 4, 'Good', 23),
('2023-12-25 09:30:00', 79, 1, 3, 'Improvement', 23),
('2024-03-22 13:00:00', 81, 1, 2, 'Continued improvement', 23),

--Shelter 24(Luxury Residential) - 2 inspections
('2023-10-25 10:00:00', 92, 1, 1, 'New and well-maintained building', 24),
('2024-03-28 11:00:00', 94, 1, 0, 'Excellent', 24),

--Shelter 25(Culture Center) - 2 inspections
('2023-11-05 11:00:00', 85, 1, 2, 'Good', 25),
('2024-03-30 14:00:00', 87, 1, 2, 'Improvement', 25),

--Shelter 26(School Etz Haim Jerusalem) - 4 inspections
('2023-06-05 09:00:00', 83, 1, 3, 'Good', 26),
('2023-09-30 10:30:00', 85, 1, 2, 'Improvement', 26),
('2024-01-05 14:00:00', 87, 1, 2, 'Continued improvement', 26),
('2024-03-12 11:15:00', 88, 1, 1, 'Excellent', 26),

--Shelter 27(Mamilla Parking) - 3 inspections
('2023-07-05 08:00:00', 79, 1, 3, 'Good', 27),
('2023-11-10 09:30:00', 81, 1, 3, 'Improvement', 27),
('2024-02-20 13:00:00', 83, 1, 2, 'Continued improvement', 27),

--Shelter 28(Mamilla Mall) - 4 inspections
('2023-06-10 08:30:00', 96, 1, 0, 'Perfect', 28),
('2023-09-20 10:00:00', 97, 1, 0, 'Excellence', 28),
('2023-12-25 13:00:00', 98, 1, 0, 'No defects', 28),
('2024-03-15 09:30:00', 99, 1, 0, 'Exceptional level', 28),

--Shelter 29(City Hall Jerusalem) - 3 inspections
('2023-08-05 10:00:00', 86, 1, 2, 'Good', 29),
('2023-12-10 11:00:00', 88, 1, 1, 'Improvement', 29),
('2024-03-18 14:00:00', 89, 1, 1, 'Excellent', 29),

--Shelter 30(Residential Center Jerusalem) - 2 inspections
('2023-09-25 10:00:00', 61, 0, 9, 'Maintenance issues', 30),
('2024-02-18 12:00:00', 67, 0, 7, 'Improvement insufficient', 30),

--Shelter 31(Central Station Jerusalem) - 4 inspections
('2023-06-15 08:00:00', 75, 1, 4, 'Fair', 31),
('2023-10-05 09:30:00', 77, 1, 4, 'Slight improvement', 31),
('2024-01-10 13:00:00', 79, 1, 3, 'Continued improvement', 31),
('2024-03-20 11:00:00', 81, 1, 2, 'Good', 31),

--Shelter 32(Great Synagogue Jerusalem) - 2 inspections
('2023-10-30 11:00:00', 72, 1, 5, 'Fair', 32),
('2024-03-25 13:00:00', 75, 1, 4, 'Improvement', 32),

--Shelter 33(School Givat Shaul) - 3 inspections
('2023-07-10 09:00:00', 84, 1, 2, 'Good', 33),
('2023-11-15 10:30:00', 86, 1, 2, 'Improvement', 33),
('2024-02-22 14:00:00', 87, 1, 1, 'Excellent', 33),

--Shelter 34(Har Nof Parking) - 3 inspections
('2023-08-10 08:00:00', 78, 1, 3, 'Good', 34),
('2023-12-15 09:30:00', 80, 1, 3, 'Improvement', 34),
('2024-03-08 13:00:00', 82, 1, 2, 'Continued improvement', 34),

--Shelter 35(Malha Mall) - 4 inspections
('2023-06-20 08:30:00', 93, 1, 1, 'Excellent', 35),
('2023-10-10 10:00:00', 94, 1, 0, 'Perfect', 35),
('2024-01-15 13:00:00', 95, 1, 0, 'No defects', 35),
('2024-03-22 09:30:00', 96, 1, 0, 'Excellent', 35),

--Shelter 36(Residential Givat Shaul) - 2 inspections
('2023-09-30 10:00:00', 64, 0, 8, 'Issues', 36),
('2024-02-20 12:00:00', 69, 0, 6, 'Slight improvement', 36),

--Shelter 37(School Ramot) - 3 inspections
('2023-07-15 09:00:00', 85, 1, 2, 'Good', 37),
('2023-11-20 10:30:00', 87, 1, 2, 'Improvement', 37),
('2024-02-25 14:00:00', 88, 1, 1, 'Excellent', 37),

--Shelter 38(Ramot Parking) - 3 inspections
('2023-08-15 08:00:00', 76, 1, 4, 'Fair', 38),
('2023-12-20 09:30:00', 78, 1, 3, 'Improvement', 38),
('2024-03-10 13:00:00', 80, 1, 3, 'Continued improvement', 38),

--Shelter 39(Grand Canyon Mall) - 4 inspections
('2023-06-25 08:30:00', 95, 1, 0, 'Perfect', 39),
('2023-10-15 10:00:00', 96, 1, 0, 'Excellence', 39),
('2024-01-20 13:00:00', 97, 1, 0, 'No defects', 39),
('2024-03-25 09:30:00', 98, 1, 0, 'Exceptional level', 39),

--Shelter 40(Community Center Ramot) - 2 inspections
('2023-11-10 11:00:00', 82, 1, 3, 'Good', 40),
('2024-03-28 14:00:00', 84, 1, 2, 'Improvement', 40),

--Shelter 41(Residential Ramot) - 2 inspections
('2023-10-05 10:00:00', 89, 1, 2, 'Excellent', 41),
('2024-03-30 11:00:00', 91, 1, 1, 'Outstanding', 41),

--Shelter 42(School Old City Beer Sheva) - 3 inspections
('2023-07-20 09:00:00', 68, 0, 7, 'Old building, issues', 42),
('2023-11-25 10:30:00', 71, 1, 6, 'Improvement', 42),
('2024-02-28 14:00:00', 73, 1, 5, 'Continued improvement', 42),

--Shelter 43(Central Parking Old City) - 3 inspections
('2023-08-20 08:00:00', 75, 1, 4, 'Fair', 43),
('2023-12-25 09:30:00', 77, 1, 4, 'Improvement', 43),
('2024-03-12 13:00:00', 79, 1, 3, 'Continued improvement', 43),

--Shelter 44(City Market) - 2 inspections
('2023-09-05 10:00:00', 59, 0, 10, 'Infrastructure problems', 44),
('2024-01-25 12:00:00', 65, 0, 8, 'Improvement insufficient', 44),

--Shelter 45(City Hall Beer Sheva) - 3 inspections
('2023-08-25 11:00:00', 88, 1, 2, 'Good', 45),
('2023-12-30 12:00:00', 89, 1, 1, 'Improvement', 45),
('2024-03-15 14:00:00', 90, 1, 1, 'Excellent', 45),

--Shelter 46(School Ashdod) - 3 inspections
('2023-07-25 09:00:00', 86, 1, 2, 'Good', 46),
('2023-11-30 10:30:00', 88, 1, 2, 'Improvement', 46),
('2024-03-05 14:00:00', 89, 1, 1, 'Excellent', 46),

--Shelter 47(Lev Ashdod Parking) - 3 inspections
('2023-08-30 08:00:00', 81, 1, 3, 'Good', 47),
('2024-01-05 09:30:00', 83, 1, 2, 'Improvement', 47),
('2024-03-18 13:00:00', 85, 1, 2, 'Continued improvement', 47),

--Shelter 48(Lev Ashdod Mall) - 4 inspections
('2023-06-30 08:30:00', 94, 1, 1, 'Excellent', 48),
('2023-10-20 10:00:00', 95, 1, 0, 'Perfect', 48),
('2024-01-25 13:00:00', 96, 1, 0, 'No defects', 48),
('2024-03-28 09:30:00', 97, 1, 0, 'Excellent', 48),

--Shelter 49(Sports Center Ashdod) - 2 inspections
('2023-11-15 11:00:00', 83, 1, 3, 'Good', 49),
('2024-03-30 14:00:00', 85, 1, 2, 'Improvement', 49),

--Shelter 50(New Residential Ashdod) - 2 inspections
('2023-10-10 10:00:00', 90, 1, 1, 'New building, excellent', 50),
('2024-03-20 11:00:00', 92, 1, 1, 'Outstanding', 50);

        -- =====================================================
        --VERIFICATION QUERIES
        -- =====================================================
        --Run these to verify the data was loaded correctly:

--SELECT COUNT(*) AS TotalAreas FROM Areas; --Should be 10
-- SELECT COUNT(*) AS TotalShelters FROM Shelters; --Should be 50
-- SELECT COUNT(*) AS TotalInspections FROM Inspections; --Should be 150

-- SELECT City, COUNT(*) AS ShelterCount
--FROM Areas a
--JOIN Shelters s ON a.Id = s.AreaId
-- GROUP BY City;

        --SELECT ShelterType, COUNT(*) AS Count
        --FROM Shelters
        -- GROUP BY ShelterType;

        --SELECT
        --     CASE WHEN Passed = 1 THEN 'Passed' ELSE 'Failed' END AS Status,
--COUNT(*) AS Count
--FROM Inspections
-- GROUP BY Passed;

        -- =====================================================
        --END OF SEED DATA
        -- =====================================================

    }
    
}
