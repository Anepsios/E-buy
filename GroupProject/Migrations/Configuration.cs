namespace GroupProject.Migrations
{
    using GroupProject.Models;
    using GroupProject.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GroupProject.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {

            var categories = new List<Category>
            {
                new Category { Name = "Laptop" },
                new Category { Name = "Mobile" },
                new Category { Name = "Tablet" },
                new Category { Name = "TV" }
            };

            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Name = "Apple" },
                new Manufacturer { Name = "HP" },
                new Manufacturer { Name = "Samsung" },
                new Manufacturer { Name = "Asus" },
                new Manufacturer { Name = "Huawei" },
                new Manufacturer { Name = "Lenovo" },
                new Manufacturer { Name = "Toshiba" },
                new Manufacturer { Name = "Dell" },
                new Manufacturer { Name = "Cubot" },
                new Manufacturer { Name = "LG" },
                new Manufacturer { Name = "Xiaomi" },
                new Manufacturer { Name = "Alcatel" },
                new Manufacturer { Name = "Microsoft" },
                new Manufacturer { Name = "Amazon" },
                new Manufacturer { Name = "Sony" }
            };
            manufacturers.ForEach(s => context.Manufacturers.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            new List<Product>
            {

                // Laptops
                
                // Apples
                new Product { Name = "MacBook Air 13.3\"", Description = "M1/8GB/256GB/Retina Display/MacOS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1165M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/laptop_apple_macbookair13,3.jpeg" },
                new Product { Name = "MacBook Pro 13\"", Description = "M1/8GB/256GB/Retina Display/macOS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1499M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/laptop_apple_macbookpro13.jpeg" },
                new Product { Name = "MacBook Pro 16\"", Description = "i7/16GB/512GB/Radeon Pro 5300M/Touchbar", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 2677.70m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/laptop_apple_macbookpro16.jpeg" },
                new Product { Name = "MacBook Air 13\"", Description = "i5/8GB/256GB/Retina", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1699m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/laptop_apple_macbookair13.jpeg" },

                // Xiaomis
                new Product { Name = "Mi Notebook Air 13.3\"", Description = "i5/6200U/8GB/256GB/Geforce 940MX/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 935m, ManufacturerID = manufacturers.Single(a => a.Name == "Xiaomi").ID, ProductImage = "~/Images/laptop_xiaomi_minotebookair.jpeg" },
                new Product { Name = "Mi Gaming", Description = "i7/9750H/16GB/1TB/RTX2060Ti/FHD/144Hz/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1538m, ManufacturerID = manufacturers.Single(a => a.Name == "Xiaomi").ID, ProductImage = "~/Images/laptop_xiaomi_migaming.jpeg" },

                // LG
                new Product { Name = "Ultra 17", Description = "i5/10210U/16GB/512TB/Geforce GTX 1650/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1499m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/laptop_lg_ultra17.jpeg" },

                // Toshiba
                new Product { Name = "Satellite Pro L50-G-1CK", Description = "i7-10710U/8GB/256GB/GeForce MX250/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1099m, ManufacturerID = manufacturers.Single(a => a.Name == "Toshiba").ID, ProductImage = "~/Images/laptop_toshiba_satelliteprol50-g-1ck.jpeg" },
                new Product { Name = "Satellite Pro L50-G-1CJ", Description = "i5-10210U/8GB/256GB/GeForce MX250/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 899m, ManufacturerID = manufacturers.Single(a => a.Name == "Toshiba").ID, ProductImage = "~/Images/laptop_toshiba_satelliteprol50-g-1cj.jpeg" },
                new Product { Name = "Satellite Pro C50-H-10D", Description = "i3-1005G1/8GB/256GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 659m, ManufacturerID = manufacturers.Single(a => a.Name == "Toshiba").ID, ProductImage = "~/Images/laptop_toshiba_satelliteproc50-h-10d.jpeg" },
                new Product { Name = "Satellite Pro L50-G-1CG", Description = "i7-10710U/16GB/1TB + 512GB/GeForce MX250/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1399m, ManufacturerID = manufacturers.Single(a => a.Name == "Toshiba").ID, ProductImage = "~/Images/laptop_toshiba_satelliteprol50-g-1cg.jpeg" },
                new Product { Name = "Satellite Pro C50-H-101", Description = "i5-1035G1/8GB/256GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 799m, ManufacturerID = manufacturers.Single(a => a.Name == "Toshiba").ID, ProductImage = "~/Images/laptop_toshiba_satelliteproc50-h-101.jpeg" },

                // Lenovo
                new Product { Name = "V145", Description = "A6-9225/8GB/256GB/Radeon 530/FHD/No OS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 479m, ManufacturerID = manufacturers.Single(a => a.Name == "Lenovo").ID, ProductImage = "~/Images/laptop_lenovo_v145.jpeg" },
                new Product { Name = "IdeaPad 3 CB 11AST5", Description = "A6-Series-9220C/4GB/32GB//Chrome OS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 258.70m, ManufacturerID = manufacturers.Single(a => a.Name == "Lenovo").ID, ProductImage = "~/Images/laptop_lenovo_ideapad3cb11ast5.jpeg" },
                new Product { Name = "Legion 5 15IMH05H", Description = "i7-10750H/8GB/512GB/GTX 1660 Ti/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1097.70m, ManufacturerID = manufacturers.Single(a => a.Name == "Lenovo").ID, ProductImage = "~/Images/laptop_lenovo_legion515imh05h.jpeg" },
                new Product { Name = "Yoga Slim 7 14ARE05", Description = "Ryzen 7-4800U/16GB/1TB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1299m, ManufacturerID = manufacturers.Single(a => a.Name == "Lenovo").ID, ProductImage = "~/Images/laptop_lenovo_yogaslim714are05.jpeg" },

                // Asus
                new Product { Name = "TUF Gaming A15 FA506IU-HN156T", Description = "R7-4800H/16GB/512GB/GeForce GTX 1660 Ti/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1349m, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/laptop_asus_tufgaminga15fa506iu-hn156t.jpeg" },
                new Product { Name = "Chromebook C523NA", Description = "3350/4GB/64GB/FHD/Chrome OS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 309m, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/laptop_asus_chromebookc523na.jpeg" },
                new Product { Name = "VivoBook R564JA-UH51T", Description = "i5-1035G1/8GB/256GB/FHD/W10s", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 627m, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/laptop_asus_vivobookr564ja-uh51t.jpeg" },
                new Product { Name = "ZenBook 14 UM425IA-WB502T", Description = "Ryzen 5-4500U/8GB/512GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 861.86m, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/laptop_asus_zenbook14um425ia-wb502t.jpeg" },
                new Product { Name = "ROG Zephyrus G14 GA401IV-BR9N6", Description = "Ryzen 9-4900HS/16GB/1TB/GeForce RTX 2060 Max-Q/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1635.70m, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/laptop_asus_rogzephyrusg14ga401ivbr9n6.jpeg" },
                new Product { Name = "ROG Zephyrus Duo 15 SE GX551QR-HF014R", Description = "Ryzen 7-5800H/32GB/1TB/GeForce RTX 3070/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 2999m, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/laptop_asus_rogzephyrusduo15segx551qr-hf014r.jpeg" },

                // Samsung
                new Product { Name = "Chromebook 4 11.6\"", Description = "N4000/4GB/32GB/Chrome OS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 249.80m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/laptop_samsung_chromebook411,6.jpeg" },
                new Product { Name = "Galaxy Book Flex 13.3\"", Description = "i7-1065G7/8GB/512GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1899m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/laptop_samsung_galaxybookflex13,3.jpeg" },
                new Product { Name = "Chromebook 4+", Description = "N4000/4GB/128GB/FHD/Chrome OS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 368m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/laptop_samsung_chromebook4plus.jpeg" },

                // HP
                new Product { Name = "250 G7", Description = "i5-1035G1/8GB/256GB/FHD/No OS", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 598.99m, ManufacturerID = manufacturers.Single(a => a.Name == "HP").ID, ProductImage = "~/Images/laptop_hp_250g7.jpeg" },
                new Product { Name = "14-dk1031dx", Description = "Ryzen 3-3250U/8GB/1TB/W10 S", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 448.90m, ManufacturerID = manufacturers.Single(a => a.Name == "HP").ID, ProductImage = "~/Images/laptop_hp_14-dk1031dx.jpeg" },
                new Product { Name = "255 G7", Description = "R5-3500U/8GB/256GB/FHD/DVD-RW/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 669m, ManufacturerID = manufacturers.Single(a => a.Name == "HP").ID, ProductImage = "~/Images/laptop_hp_255g7.jpeg" },
                new Product { Name = "Pavilion 15-eh0090wm", Description = "Ryzen 5-4500U/8GB/512GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 719m, ManufacturerID = manufacturers.Single(a => a.Name == "HP").ID, ProductImage = "~/Images/laptop_hp_pavilion15-eh0090wm.jpeg" },
                new Product { Name = "Omen 15T DH00 Gaming", Description = "i9-10885H/32GB/1TB + 512GB/RTX 2080 Super Max-Q/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 2889m, ManufacturerID = manufacturers.Single(a => a.Name == "HP").ID, ProductImage = "~/Images/laptop_hp_omen15tdh00gaming.jpeg" },

                // Dell
                new Product { Name = "XPS 15 9500", Description = "i9-10885?/32GB/2TB/GeForce GTX 1650 Ti//W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 4048m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_xps159500.jpeg" },
                new Product { Name = "Inspiron 5501", Description = "i7-1065G7/12GB/1TB/GeForce MX330/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 1289.19m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_inspiron5501.jpeg" },
                new Product { Name = "Inspiron 13 7000", Description = "i5-10510U/8GB/512GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 829m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_inspiron137000.jpeg" },
                new Product { Name = "Inspiron 5406", Description = "i5-1135G7/8GB/256GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 879m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_inspiron5406.jpeg" },
                new Product { Name = "Inspiron 5505", Description = "R5-4500U/8GB/256GB/FHD/W10", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 759m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_inspiron5505.jpeg" },
                new Product { Name = "Inspiron 3593", Description = "i7-1065G7/12GB/512GB/W10 S", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 813.90m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_inspiron3593.jpeg" },
                new Product { Name = "Inspiron 3505", Description = "Ryzen 5-3450U/12GB/1TB + 256GB/FHD/W10 S", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 686m, ManufacturerID = manufacturers.Single(a => a.Name == "Dell").ID, ProductImage = "~/Images/laptop_dell_inspiron3505.jpeg" },


                // TVs

                // Samsung
                new Product { Name = "QE50Q60T", Description = "Smart 4K UHD 50\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 567m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_QE50Q60T.jpeg" },
                new Product { Name = "QE55Q70T", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 749m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_QE55Q70T.jpeg" },
                new Product { Name = "UE43TU7172", Description = "Smart 4K UHD 43\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 365m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_UE43TU7172.jpeg" },
                new Product { Name = "UE32T4302", Description = "Smart HD Ready 32\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 228.80m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_UE32T4302.jpeg" },
                new Product { Name = "UE55TU7172", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 428m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_UE55TU7172.jpeg" },
                new Product { Name = "QE55Q80T", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 939m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_QE55Q80T.jpeg" },
                new Product { Name = "QE55Q60T", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 625m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tv_samsung_QE55Q60T.jpeg" },

                // LG
                new Product { Name = "32LM6300PLA", Description = "Smart Full HD 32\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 244.07m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_32LM6300PLA.jpeg" },
                new Product { Name = "49NANO866NA", Description = "Smart 4K UHD 49\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 689m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_49NANO866NA.jpeg" },
                new Product { Name = "OLED55BX6LB", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 1078m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_OLED55BX6LB.jpeg" },
                new Product { Name = "32LK6200", Description = "Smart Full HD 32\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 267.61m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_32LK6200.jpeg" },
                new Product { Name = "OLED65CX6LA", Description = "Smart 4K UHD 65\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 1939m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_OLED65CX6LA.jpeg" },
                new Product { Name = "49NANO816NA", Description = "Smart 4K UHD 49\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 518m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_49NANO816NA.jpeg" },
                new Product { Name = "OLED55CX6LA", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 1329m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_OLED55CX6LA.jpeg" },
                new Product { Name = "43UN73006LC", Description = "Smart 4K UHD 43\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 366.98m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/tv_lg_43UN73006LC.jpeg" },

                // Sony
                new Product { Name = "KD-77AG9", Description = "Smart 4K UHD 77\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 3698.99m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-77AG9.jpeg" },
                new Product { Name = "KD-85XH8096", Description = "Smart 4K UHD 85\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 2099m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-85XH8096.jpeg" },
                new Product { Name = "KD-75XH9096", Description = "Smart 4K UHD 75\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 1890m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-75XH9096.jpeg" },
                new Product { Name = "KD-43XH8096", Description = "Smart 4K UHD 43\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 546m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-43XH8096.jpeg" },
                new Product { Name = "KD-55AG9", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 1998.93m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-55AG9.jpeg" },
                new Product { Name = "KD-49XH8096", Description = "Smart 4K UHD 49\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 599m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-49XH8096.jpeg" },
                new Product { Name = "KD-55XH9096", Description = "Smart 4K UHD 55\"", CategoryID = categories.SingleOrDefault(g => g.Name == "TV").ID,
                    Price = 1098m, ManufacturerID = manufacturers.Single(a => a.Name == "Sony").ID, ProductImage = "~/Images/tv_sony_KD-55XH9096.jpeg" },


                // Mobiles

                // Xiaomi
                new Product { Name = "Redmi Note 9S", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 207m, ManufacturerID = manufacturers.Single(a => a.Name == "Xiaomi").ID, ProductImage = "~/Images/mobile_xiaomi_redminote9s.jpeg" },
                new Product { Name = "Redmi Note 8 Pro", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 184m, ManufacturerID = manufacturers.Single(a => a.Name == "Xiaomi").ID, ProductImage = "~/Images/mobile_xiaomi_redminote8pro.jpeg" },
                new Product { Name = "Redmi Note 9 Pro", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 2019m, ManufacturerID = manufacturers.Single(a => a.Name == "Xiaomi").ID, ProductImage = "~/Images/mobile_xiaomi_redminote9pro.jpeg" },
                new Product { Name = "Poco X3 NFC", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 235m, ManufacturerID = manufacturers.Single(a => a.Name == "Xiaomi").ID, ProductImage = "~/Images/mobile_xiaomi_pocox3nfc.jpeg" },

                // Cubot
                new Product { Name = "King Kong CS", Description = "16GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 87m, ManufacturerID = manufacturers.Single(a => a.Name == "Cubot").ID, ProductImage = "~/Images/mobile_cubot_kingkongcs.jpeg" },
                new Product { Name = "J9 ", Description = "16GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 78m, ManufacturerID = manufacturers.Single(a => a.Name == "Cubot").ID, ProductImage = "~/Images/mobile_cubot_j9.jpeg" },
                new Product { Name = "X30 ", Description = "8GB/128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 218m, ManufacturerID = manufacturers.Single(a => a.Name == "Cubot").ID, ProductImage = "~/Images/mobile_cubot_x30.jpeg" },
                new Product { Name = "Note 7", Description = "16GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 70.98m, ManufacturerID = manufacturers.Single(a => a.Name == "Cubot").ID, ProductImage = "~/Images/mobile_cubot_note7.jpeg" },

                // Alcatel
                new Product { Name = "3L", Description = "2020 64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 136.50m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/mobile_alcatel_3l.jpeg" },
                new Product { Name = "1B", Description = "2020 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 78.89m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/mobile_alcatel_1b.jpeg" },
                new Product { Name = "1 5033D", Description = "8GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 47.90m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/mobile_alcatel_15033d.jpeg" },
                new Product { Name = "1SE ", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 98.80m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/mobile_alcatel_1se.jpeg" },
                new Product { Name = "1S", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 112.50m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/mobile_alcatel_1s.jpeg" },
                new Product { Name = "3X", Description = "2020 128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 147m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/mobile_alcatel_3x.jpeg" },

                // Huawei
                new Product { Name = "P40 Lite E", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 139.90m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_p40litee.jpeg" },
                new Product { Name = "P30 Lite Dual", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 169.99m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_p40litedual.jpeg" },
                new Product { Name = "P Smart S", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 165.85m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_psmarts.jpeg" },
                new Product { Name = "P30 Pro Dual", Description = "6GB/128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 488.83m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_p30produal.jpeg" },
                new Product { Name = "P40 Pro", Description = "256GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 689.15m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_p40pro.jpeg" },
                new Product { Name = "Nova 5T", Description = "6GB/128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 241m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_nova5t.jpeg" },
                new Product { Name = "P40 Lite", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 198m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/mobile_huawei_p40lite.jpeg" },

                // Apple
                new Product { Name = "iPhone 11", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 697.99m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/mobile_apple_iphone11.jpeg" },
                new Product { Name = "iPhone 12 Pro Max", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 1336.80m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/mobile_apple_iphone12promax.jpeg" },
                new Product { Name = "iPhone 11 Pro", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 917.90m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/mobile_apple_iphone11pro.jpeg" },
                new Product { Name = "iPhone SE", Description = "2020 64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 444.30m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/mobile_apple_iphonese.jpeg" },
                new Product { Name = "iPhone XR", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 551.14m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/mobile_apple_iphonexr.jpeg" },
                new Product { Name = "iPhone 12 Pro", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 1228.89m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/mobile_apple_iphone12pro.jpeg" },

                // LG
                new Product { Name = "K41S ", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 126.45m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_k41s.jpeg" },
                new Product { Name = "G8S ThinQ", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 450m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_g8sthinq.jpeg" },
                new Product { Name = "Velvet 5G", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 422m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_velvet5g.jpeg" },
                new Product { Name = "K61 ", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 187.60m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_k61.jpeg" },
                new Product { Name = "K50S ", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 235.90m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_k50s.jpeg" },
                new Product { Name = "K22 ", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 112.95m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_k22.jpeg" },
                new Product { Name = "V30+", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 253.20m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_v30plus.jpeg" },
                new Product { Name = "Wing 5G", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 871.88m, ManufacturerID = manufacturers.Single(a => a.Name == "LG").ID, ProductImage = "~/Images/mobile_LG_wing5g.jpeg" },

                // Samsung
                new Product { Name = "Galaxy A71", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 337.21m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxya71.jpeg" },
                new Product { Name = "Galaxy A51 ", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 250.96m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxya51.jpeg" },
                new Product { Name = "Galaxy A20e Dual", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 126m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxya20edual.jpeg" },
                new Product { Name = "Galaxy A21s", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 163m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxya21s.jpeg" },
                new Product { Name = "Galaxy A41", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 197m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxya41.jpeg" },
                new Product { Name = "Galaxy A31", Description = "64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 198m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxya31.jpeg" },
                new Product { Name = "Galaxy S20 Ultra 5G", Description = "128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 1044.08m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/mobile_samsung_galaxys20ultra5g.jpeg" },


                // Tablets

                // Microsoft
                new Product { Name = "Surface Pro 7 12.3\"", Description = "Windows 10 Home/i5/128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 849m, ManufacturerID = manufacturers.Single(a => a.Name == "Microsoft").ID, ProductImage = "~/Images/tablet_microsoft_surfacepro712,3.jpeg" },
                new Product { Name = "Surface Pro 6 12.3\"", Description = "i5-8250U/8GB/256GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 939.52m, ManufacturerID = manufacturers.Single(a => a.Name == "Microsoft").ID, ProductImage = "~/Images/tablet_microsoft_surfacepro612,3.jpeg" },

                // Amazon
                new Product { Name = "Fire 10.1\"", Description = "HD 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 130.59m, ManufacturerID = manufacturers.Single(a => a.Name == "Amazon").ID, ProductImage = "~/Images/tablet_amazon_fire10,1.jpeg" },
                new Product { Name = "Fire HD 8\"", Description = "32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 87.03m, ManufacturerID = manufacturers.Single(a => a.Name == "Amazon").ID, ProductImage = "~/Images/tablet_amazon_firehd8.jpeg" },
                new Product { Name = "Fire 7 7\"", Description = "16GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 65.25m, ManufacturerID = manufacturers.Single(a => a.Name == "Amazon").ID, ProductImage = "~/Images/tablet_amazon_fire77.jpeg" },

                // Alcatel
                new Product { Name = "1T ", Description = "10.1\" 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 111.99m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/tablet_alcatel_1t.jpeg" },
                new Product { Name = "Smart Tab 7", Description = "7\" 16GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 71.80m, ManufacturerID = manufacturers.Single(a => a.Name == "Alcatel").ID, ProductImage = "~/Images/tablet_alcatel_smarttab7.jpeg" },

                // Huawei
                new Product { Name = "MatePad Pro", Description = "10.8\" 4G 128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 573.90m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/tablet_huawei_matepadpro.jpeg" },
                new Product { Name = "MediaPad T5", Description = "10.1\" 64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 228.98m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/tablet_huawei_mediapadt5.jpeg" },
                new Product { Name = "MediaPad T5 lite", Description = "10.1\" 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 199m, ManufacturerID = manufacturers.Single(a => a.Name == "Huawei").ID, ProductImage = "~/Images/tablet_huawei_mediapadt5lite.jpeg" },

                // Lenovo
                new Product { Name = "Tab M10 X505", Description = "10.1\" 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 150m, ManufacturerID = manufacturers.Single(a => a.Name == "Lenovo").ID, ProductImage = "~/Images/tablet_lenovo_tabm10x505.jpeg" },

                // Samsung
                new Product { Name = "Galaxy Tab A7", Description = "2020 10.4\" 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 218.10m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tablet_samsung_galaxytaba7.jpeg" },
                new Product { Name = "Galaxy Tab S7", Description = "11\" 128GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 690.31m, ManufacturerID = manufacturers.Single(a => a.Name == "Samsung").ID, ProductImage = "~/Images/tablet_samsung_galaxytabs7.jpeg" },

                // Apple
                new Product { Name = "iPad", Description = "2020 10.2\" 32GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 391.83m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/tablet_apple_ipad.jpeg" },
                new Product { Name = "iPad Air", Description = "2020 10.9\" 64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 704.38m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/tablet_apple_ipadair.jpeg" },
                new Product { Name = "iPad Pro", Description = "2020 11\" 256GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 993.92m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/tablet_apple_ipadpro.jpeg" },
                new Product { Name = "iPad Mini", Description = "2019 Wi-Fi 7.9\" 64GB", CategoryID = categories.SingleOrDefault(g => g.Name == "Tablet").ID,
                    Price = 418.99m, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/tablet_apple_ipadmini.jpeg" }



                // test products
                //new Product { Name = "mobile1", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                //    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/iphone12.png" },
                //new Product { Name = "mobile2", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                //    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/iphone12.png" },
                //new Product { Name = "mobile3", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                //    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "~/Images/iphone12.png" },
                //new Product { Name = "FA506IU-HN156T", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                //    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/asus.jpg" },
                //new Product { Name = "FA706IU-H7006T", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                //    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "~/Images/asus.jpg" },

            }.ForEach(s => context.Products.AddOrUpdate(a => a.Name, s));
        }
    }
}
