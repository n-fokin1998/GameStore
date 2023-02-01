using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Cryptography;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF
{
    public class GameStoreDbInitializer : DropCreateDatabaseIfModelChanges<GameStoreContext>
    {
        protected override void Seed(GameStoreContext context)
        {
            var strategy = new Genre { Id = 1, NameEn = "Strategy", NameRu = "Стратегия" };
            var rpg = new Genre { Id = 2, NameEn = "RPG", NameRu = "RPG" };
            var sports = new Genre { Id = 3, NameEn = "Sports", NameRu = "Спорт" };
            var races = new Genre { Id = 4, NameEn = "Races", NameRu = "Гонки" };
            var action = new Genre { Id = 5, NameEn = "Action", NameRu = "Экшен" };
            var adventure = new Genre { Id = 6, NameEn = "Adventure", NameRu = "Приключения" };
            var puzzle = new Genre { Id = 7, NameEn = "Puzzle&Skill", NameRu = "Логические" };
            var rts = new Genre
            {
                NameEn = "RTS",
                NameRu = "RTS",
                ParentGenreId = strategy.Id,
                ParentGenre = strategy
            };
            var tbs = new Genre
            {
                NameEn = "TBS",
                NameRu = "TBS",
                ParentGenreId = strategy.Id,
                ParentGenre = strategy
            };
            var rally = new Genre
            {
                NameEn = "Rally",
                NameRu = "Ралли",
                ParentGenreId = races.Id,
                ParentGenre = races
            };
            var arcade = new Genre
            {
                NameEn = "Arcade",
                NameRu = "Аркада",
                ParentGenreId = races.Id,
                ParentGenre = races
            };
            var formula = new Genre
            {
                NameEn = "Formula",
                NameRu = "Формула 1",
                ParentGenreId = races.Id,
                ParentGenre = races
            };
            var offRoad = new Genre
            {
                NameEn = "Off-Road",
                NameRu = "Гонки по бездорожью",
                ParentGenreId = races.Id,
                ParentGenre = races
            };
            var fps = new Genre
            {
                NameEn = "FPS",
                NameRu = "FPS",
                ParentGenreId = action.Id,
                ParentGenre = action
            };
            var tps = new Genre
            {
                NameEn = "TPS",
                NameRu = "TPS",
                ParentGenreId = action.Id,
                ParentGenre = action
            };
            var misc = new Genre
            {
                NameEn = "Misc",
                NameRu = "Разное",
                ParentGenreId = action.Id,
                ParentGenre = action
            };
            var other = new Genre
            {
                NameEn = "Other",
                NameRu = "Другое",
            };
            context.Genres.AddRange(new List<Genre>()
            {
                strategy, rpg, sports, races, action, adventure, puzzle,
                rts, tbs, rally, arcade, formula, offRoad, fps, tps, misc, other
            });
            var mobile = new PlatformType { TypeEn = "Mobile", TypeRu = "Мобильные" };
            var browser = new PlatformType { TypeEn = "Browser", TypeRu = "Браузерные" };
            var desktop = new PlatformType { TypeEn = "Desktop", TypeRu = "ПК" };
            var console = new PlatformType { TypeEn = "Console", TypeRu = "Консольные" };
            context.PlatformTypes.AddRange(new List<PlatformType> { mobile, browser, desktop, console });
            var valve = new Publisher
            {
                CompanyName = "Valve",
                DescriptionEn = "Valve Corporation is an American video game developer and digital distribution" +
                " company headquartered in Bellevue, Washington. The company is known for its software distribution " +
                "platform Steam and the Half-Life, Counter-Strike, Portal, Day of Defeat, Team Fortress, Left 4 Dead," +
                " and Dota 2 games.",
                DescriptionRu = "Valve Corporation — американская компания-разработчик компьютерных игр, создавшая серии компьютерных игр Half-Life," +
                                " Portal, Counter-Strike, Team Fortress, Day of Defeat, Left 4 Dead, Ricochet, Alien Swarm и Dota 2; игровые движки GoldSrc," +
                                " Source, Source 2; программное обеспечение: Steam, Valve Anti-Cheat, SteamOS, Steam Audio; а также аппаратное обеспечение:" +
                                " Steam Machines, Steam Controller. Основана 24 августа 1996 года бывшими сотрудниками Microsoft Гейбом Ньюэллом и Майком Харрингтоном.",
                HomePage = "valvesoftware.com"
            };
            var electronicArts = new Publisher
            {
                CompanyName = "Electronic Arts",
                DescriptionEn = "Electronic Arts Inc. (EA) is an American video game company headquartered in Redwood City," +
                " California. Founded and incorporated on May 28, 1982 by Trip Hawkins, the company was a pioneer of the " +
                "early home computer games industry and was notable for promoting the designers and programmers responsible" +
                " for its games.",
                DescriptionRu = "Electronic Arts (EA) — американская корпорация, которая занимается распространением видеоигр." +
                                " Компания была основана 28 мая 1982 Трипом Хокинсом, и стала одной из первых компаний в игровой индустрии," +
                                " и отличалась тем, что популяризировала людей, работавших над играми — дизайнеров и программистов." +
                                " Стартовый капитал, сформированный целиком за счёт личных накоплений Хокинса, составил 200 тыс. долларов США." +
                                " В то время компания называлась Amazin' Software.",
                HomePage = "ea.com"
            };
            var rovio = new Publisher
            {
                CompanyName = "Rovio Entertainment",
                DescriptionEn = "Rovio Entertainment Corporation (formerly Relude and later Rovio Mobile) is a Finnish " +
                "developer, publisher, distributor of video games and is an entertainment company headquartered in Espoo," +
                " Finland.",
                DescriptionRu = "Rovio Entertainment или Rovio — финский разработчик компьютерных игр с офисом в городе Эспоо. " +
                                "Компания была основана в 2003 году как студия разработки мобильных игр Relude, а в 2005 году была " +
                                "переименована в Rovio. Компания наиболее известна своей игровой франшизой Angry Birds.",
                HomePage = "rovio.com"
            };
            var rockstar = new Publisher
            {
                CompanyName = "Rockstar Games",
                DescriptionEn = "Rockstar Games, Inc. is an American video game publisher based in New York City." +
                " The company was established in December 1998 as a publishing subsidiary of Take-Two Interactive, " +
                "and as successor to BMG Interactive, a dormant video game publisher Take-Two Interactive had previously " +
                "acquired the assets of.",
                DescriptionRu = "Rockstar Games (Rockstar NYC) — американская компания, специализирующаяся на разработке и издании компьютерных игр." +
                                " Владельцем Rockstar Games является корпорация Take-Two Interactive. Бренд компании широко известен по сериям игр Grand" +
                                " Theft Auto, Midnight Club, Max Payne и Red Dead. Главный офис компании расположен в Нью-Йорке.",
                HomePage = "rockstargames.com"
            };
            var ubisoft = new Publisher
            {
                CompanyName = "Ubisoft",
                DescriptionEn = "Ubisoft Entertainment SA is a French video game publisher headquartered in Montreuil." +
                " It is known for publishing games for several acclaimed video game franchises including Assassin's Creed," +
                " Far Cry, Just Dance, Prince of Persia, Rayman, and Tom Clancy's. ",
                DescriptionRu = "Ubisoft Entertainment — французская компания, специализирующаяся на разработке и издании компьютерных игр," +
                                " главный офис которой располагается в Монтрёй, Франция. Компания включает в себя студии в более чем 20 странах," +
                                " среди них США, Канада, Испания, Китай, Германия, Россия, Болгария, Украина, Румыния и Италия. Ubisoft является" +
                                " одним из крупнейших игровых издателей в Европе.",
                HomePage = "ubisoft.com"
            };
            var bethesda = new Publisher
            {
                CompanyName = "Bethesda Softworks",
                DescriptionEn = "Bethesda Softworks LLC is an American video game publisher based in Rockville, Maryland." +
                " The company was founded by Christopher Weaver in 1986 as a division of Media Technology Limited, and" +
                " in 1999 became a subsidiary of ZeniMax Media.",
                DescriptionRu = "Bethesda Softworks LLC — американский издатель развлекательного интерактивного контента. Основана в 1986 году" +
                                " Кристофером Уивером в Бетесде (штат Мэриленд). В 1999 году вошла в медиа-компанию ZeniMax Media и" +
                                " была перемещена в Роквилл (Мэриленд). Родственные компании — id Software, Arkane Studios, Tango Gameworks," +
                                " MachineGames и ZeniMax Online Studios.",
                HomePage = "bethesda.net"
            };
            var naughtyDog = new Publisher
            {
                CompanyName = "Naughty Dog",
                DescriptionEn = "Naughty Dog, LLC is an American first-party video game developer based in Santa Monica, " +
                "California. Founded by Andy Gavin and Jason Rubin in 1984 as an independent developer, the studio was " +
                "acquired by Sony Computer Entertainment in 2001. ",
                DescriptionRu = "Naughty Dog — американская частная компания, специализирующаяся на разработке компьютерных игр." +
                                " Основана в 1984 году. Штаб-квартира расположена в США, в городе Санта-Моника. Дочерняя компания Sony Interactive Entertainment",
                HomePage = "naughtydog.com"
            };
            var telltale = new Publisher
            {
                CompanyName = "Telltale Games",
                DescriptionEn = "Telltale Incorporated, doing business as Telltale Games, is an American independent video " +
                "game developer and publisher founded in June 2004. Based in San Rafael, California, the studio includes " +
                "designers formerly employed by LucasArts.",
                DescriptionRu = "Telltale Games — независимая компания, издатель и дистрибьютор компьютерных игр. " +
                                "Одна из самых известных компаний по производству приключенческих видеоигр, основанных на популярных" +
                                " телевизионных шоу и комиксах. Основана в июне 2004 года как Telltale, Incorporated. Основной офис" +
                                " располагается в городе Сан-Рафаэл. В студии задействованы дизайнеры, ранее работавшие в LucasArts. " +
                                "Бизнес-модель заключается в создании эпизодической игровой системы и цифровой дистрибуции",
                HomePage = "telltale.com"
            };
            var gsc = new Publisher
            {
                CompanyName = "GSC Game World",
                DescriptionEn = "Transavision Ltd., doing business as GSC Game World, is a Ukrainian video game developer" +
                " based in Kiev, Ukraine. Founded in 1995 by Sergey Grigorovich, it is best known for the Cossacks and " +
                "S.T.A.L.K.E.R. series of games. Their first game was the 2000 Cossacks: European Wars, which directly led" +
                " to their breakthrough.",
                DescriptionRu = "GSC Game World — украинская частная компания, разработчик компьютерных игр (в том числе серий Казаки и S.T.A.L.K.E.R.)," +
                                " локализировала игры на русский язык. Основана в 1995 году Сергеем Григоровичем, который является её бессменным руководителем. " +
                                "Название компании содержит в себе аббревиатуру его фамилии и инициалов — GSC.",
                HomePage = "gsc-game.com"
            };
            context.Publishers.AddRange(new List<Publisher> { valve, electronicArts, rovio, rockstar, ubisoft, bethesda, naughtyDog, telltale, gsc });
            var halfLife = new Game
            {
                Key = "g1",
                NameEn = "Half-Life",
                NameRu = "Half-Life",
                DescriptionEn = "Half-Life is a first-person shooter that requires the player to perform combat tasks and puzzle solving to advance through the game." +
                              " Unlike most of its peers at the time, Half-Life used scripted sequences, such as a Vortigaunt ramming down a door, to advance major plot points. ",
                DescriptionRu = "Half-Life — компьютерная игра в жанре научно-фантастического шутера от первого лица, разработанная компанией Valve Software и изданная Sierra Studios" +
                                " 8 ноября 1998 года для персонального компьютера. Первая игра в серии Half-Life. Технически игра основана на значительно переработанном движке" +
                                " Quake от id Software. В 2001 году студия Gearbox Software портировала Half-Life на игровую приставку Sony PlayStation 2. Также игра разрабатывалась" +
                                " для платформы Sega Dreamcast, но релиз был отменён.",
                Price = (decimal)9.99,
                UnitsInStock = 50,
                Discontinued = false,
                PublishDate = new DateTime(2018, 3, 30),
                UploadDate = DateTime.UtcNow,
                Publisher = valve,
                Genres = new List<Genre>() { action, fps },
                PlatformTypes = new List<PlatformType>() { desktop },
                ImageReference = "~/Content/Images/Games/half-life.jpg"
            };
            var needForSpeed = new Game
            {
                Key = "g2",
                NameEn = "Need For Speed Most Wanted",
                NameRu = "Жажда скорости: Особо опасен",
                DescriptionEn = "Need for Speed, also known by its initials NFS, is a racing video game franchise published by Electronic Arts and currently developed by Ghost Games." +
                              " The series centers around illicit street racing and in general tasks players to complete various types of races while evading the local law" +
                              " enforcement in police pursuits.",
                DescriptionRu = "Need for Speed: Most Wanted (рус. Жажда скорости: Самый разыскиваемый; сокр. NFSMW) — видеоигра серии Need for Speed в жанре аркадной автогонки," +
                                " разработанная студией EA Canada и изданная компанией Electronic Arts для консолей, персональных компьютеров и мобильных телефонов в 2005 году." +
                                " Выход игры был приурочен к началу продаж новой консоли Xbox 360, диск с игрой поставлялся в комплекте с приставкой",
                Price = (decimal)7.5,
                UnitsInStock = 25,
                Discontinued = true,
                PublishDate = new DateTime(2018, 3, 15),
                UploadDate = DateTime.UtcNow,
                Publisher = electronicArts,
                Genres = new List<Genre>() { races, arcade },
                PlatformTypes = new List<PlatformType>() { desktop, console },
                ImageReference = "~/Content/Images/Games/need-for-speed.jpg"
            };
            var angryBirds = new Game
            {
                Key = "g3",
                NameEn = "Angry Birds",
                NameRu = "Злые птицы",
                DescriptionEn = "Angry Birds is a video game franchise created by Finnish company Rovio Entertainment. The series focuses on multi-colored birds who try to save" +
                              " their eggs from green-colored pigs, their enemies. Inspired by Crush the Castle, the game has been praised for its successful combination of" +
                              " fun gameplay, comical style, and low price. ",
                DescriptionRu = "Злые птицы — серия компьютерных игр, разработанных финской компанией Rovio, в которых (в основной линии) игрок с помощью рогатки должен выстреливать " +
                                "птицами по зелёным свиньям, расставленным на различных конструкциях.",
                Price = (decimal)0.99,
                UnitsInStock = 30,
                Discontinued = false,
                PublishDate = new DateTime(2016, 4, 8),
                UploadDate = DateTime.UtcNow,
                Publisher = rovio,
                Genres = new List<Genre>() { puzzle },
                PlatformTypes = new List<PlatformType>() { mobile },
                ImageReference = "~/Content/Images/Games/angry-birds.jpg"
            };
            var assassinsCreed2 = new Game
            {
                Key = "g4",
                NameEn = "Assassin's Creed II",
                NameRu = "Кредо убийцы II",
                DescriptionEn = "Assassin's Creed II is a 2009 action-adventure video game developed by Ubisoft Montreal " +
                "and published by Ubisoft. It is the second major installment in the Assassin's Creed series, a sequel" +
                " to 2007's Assassin's Creed, and the first chapter in the Ezio trilogy.",
                DescriptionRu = "Assassin’s Creed II — компьютерная мультиплатформенная игра в жанре action-adventure, продолжение игры Assassin's Creed от компании Ubisoft." +
                                " Официальный анонс состоялся 16 апреля 2009 года в журнале Game Informer. Главным героем выступает предок Дезмонда — Эцио Аудиторе да Фиренце," +
                                " молодой флорентийский аристократ. Игрок побывает в Италии эпохи Возрождения (события игры происходят с 1476 по 1499 год) и встретится с выдающимися" +
                                " людьми того времени — к примеру, с четой Медичи, Леонардо да Винчи и Никколо Макиавелли, а во врагах у героя будут числиться Родриго Борджиа," +
                                " Франческо Пацци и Джироламо Савонарола. ",
                Price = (decimal)14.99,
                UnitsInStock = 23,
                Discontinued = false,
                PublishDate = new DateTime(2009, 4, 8),
                UploadDate = DateTime.UtcNow,
                Publisher = ubisoft,
                Genres = new List<Genre>() { action, tps, adventure },
                PlatformTypes = new List<PlatformType>() { desktop },
                ImageReference = "~/Content/Images/Games/assassins-creed.jpg"
            };
            var farcCry5 = new Game
            {
                Key = "g5",
                NameEn = "Far Cry 5",
                NameRu = "Far Cry 5",
                DescriptionEn = "Far Cry 5 is an action-adventure first-person shooter game developed by Ubisoft Montreal" +
                " and Ubisoft Toronto and published by Ubisoft for Microsoft Windows, PlayStation 4 and Xbox One. It is" +
                " an entry in the Far Cry series, and was released on March 27, 2018.",
                DescriptionRu = "Far Cry 5 — мультиплатформенная компьютерная игра 2018 года в жанре шутера от первого лица и action-adventure," +
                                " разработанная студией Ubisoft Montreal и изданная компанией Ubisoft. Является шестой игрой из одноимённой серии игр." +
                                " Действие игры происходит в округе Хоуп, штат Монтана, и повествует о противостоянии помощника шерифа и культа судного дня под названием «Врата Эдема». ",
                Price = (decimal)59.99,
                UnitsInStock = 100,
                Discontinued = false,
                PublishDate = new DateTime(2018, 3, 27),
                UploadDate = DateTime.UtcNow,
                Publisher = ubisoft,
                Genres = new List<Genre>() { action, fps },
                PlatformTypes = new List<PlatformType>() { desktop, console },
                ImageReference = "~/Content/Images/Games/far-cry.png"
            };
            var tomClancy = new Game
            {
                Key = "g6",
                NameEn = "Tom Clancy's The Division",
                NameRu = "Том Клэнси Дивизион",
                DescriptionEn = "Tom Clancy's The Division is an online-only action role-playing video game developed" +
                " by Massive Entertainment and published by Ubisoft, with assistance from Red Storm Entertainment and" +
                " Ubisoft Annecy, for Microsoft Windows, PlayStation 4 and Xbox One. It was announced during Ubisoft's" +
                " E3 2013 press conference, and was released worldwide on 8 March 2016.",
                DescriptionRu = "Tom Clancy’s The Division — мультиплатформенная компьютерная игра в жанре TPS," +
                                " первая по счёту из серии игр Tom Clancy’s The Division, разработаннаяшведской студией Ubisoft Massive," +
                                " британской Ubisoft Reflections и американской Red Storm, изданная Ubisoft для платформ ПК, PlayStation 4 и Xbox One." +
                                " Официальный анонс состоялся на Е3 2013.",
                Price = 25,
                UnitsInStock = 54,
                Discontinued = true,
                PublishDate = new DateTime(2016, 3, 8),
                UploadDate = DateTime.UtcNow,
                Publisher = ubisoft,
                Genres = new List<Genre>() { action, tps },
                PlatformTypes = new List<PlatformType>() { desktop, console },
                ImageReference = "~/Content/Images/Games/tom-clancy.jpg"
            };
            var crew = new Game
            {
                Key = "g7",
                NameEn = "The Crew",
                NameRu = "The Crew",
                DescriptionEn = "The Crew is an online-only racing video game developed by Ivory Tower and Ubisoft" +
                " Reflections and published by Ubisoft for Microsoft Windows, PlayStation 4 and Xbox One, with an Xbox " +
                "360 port developed by Asobo Studio.",
                DescriptionRu = "The Crew — онлайн-видеоигра в жанре автосимулятор, разработанная Ivory Tower совместно с Ubisoft Reflections." +
                                " 13 августа 2014 года было объявлено о выходе на Xbox 360. 2 декабря игра поступила в продажу для Xbox One, PlayStation 4 и PC.",
                Price = (decimal)11.99,
                UnitsInStock = 8,
                Discontinued = false,
                PublishDate = new DateTime(2014, 12, 2),
                UploadDate = DateTime.UtcNow,
                Publisher = ubisoft,
                Genres = new List<Genre>() { sports, races, arcade, rally },
                PlatformTypes = new List<PlatformType>() { desktop, console },
                ImageReference = "~/Content/Images/Games/the-crew.jpg"
            };
            var fifa = new Game
            {
                Key = "g8",
                NameEn = "FIFA 18",
                NameRu = "FIFA 18",
                DescriptionEn = "FIFA 18 is a football simulation video game in the FIFA series of video games, " +
                "developed and published by Electronic Arts and was released worldwide on 29 September 2017 for" +
                " Microsoft Windows, PlayStation 3, PlayStation 4, Xbox 360, Xbox One and Nintendo Switch. ",
                DescriptionRu = "FIFA 18 — 25-я футбольная игра из серии игр FIFA, разработанная для платформ Windows, Nintendo Switch, PlayStation 4," +
                                " PlayStation 3, Xbox One и Xbox 360. Игра выпущена компанией Electronic Arts 29 сентября 2017 года. Лицом игры является футболист" +
                                " мадридского «Реала» и сборной Португалии Криштиану Роналду.",
                Price = (decimal)24.99,
                UnitsInStock = 130,
                Discontinued = false,
                PublishDate = new DateTime(2017, 9, 29),
                UploadDate = DateTime.UtcNow,
                Publisher = electronicArts,
                Genres = new List<Genre>() { sports },
                PlatformTypes = new List<PlatformType>() { desktop, console, mobile },
                ImageReference = "~/Content/Images/Games/fifa18.png"
            };
            var gta = new Game
            {
                Key = "g9",
                NameEn = "Grand Theft Auto V",
                NameRu = "Великий автоугонщик V",
                DescriptionEn = "Grand Theft Auto V is an action-adventure video game developed by Rockstar North and" +
                " published by Rockstar Games. It was released in September 2013 for PlayStation 3 and Xbox 360, in " +
                "November 2014 for PlayStation 4 and Xbox One, and in April 2015 for Microsoft Windows.",
                DescriptionRu = "Grand Theft Auto V (сокр. GTA V) — мультиплатформенная компьютерная игра в жанре action-adventure с открытым миром, " +
                                "разработанная компанией Rockstar North и изданная компанией Rockstar Games для игровых консолей PlayStation 3 и Xbox 360 в 2013 году. " +
                                "В последующие годы игра была выпущена на платформах Microsoft Windows, PlayStation 4 и Xbox One. ",
                Price = 40,
                UnitsInStock = 31,
                Discontinued = false,
                PublishDate = new DateTime(2015, 4, 14),
                UploadDate = DateTime.UtcNow,
                Publisher = rockstar,
                Genres = new List<Genre>() { action, tps, misc },
                PlatformTypes = new List<PlatformType>() { desktop, console },
                ImageReference = "~/Content/Images/Games/gta5.jpeg"
            };
            var laNoire = new Game
            {
                Key = "g10",
                NameEn = "L.A. Noire",
                NameRu = "L.A. Noire",
                DescriptionEn = "L.A. Noire is a neo-noir detective action-adventure video game developed by Team Bondi" +
                " and published by Rockstar Games. It was released on 17 May 2011 for PlayStation 3 and Xbox 360," +
                " and on 8 November 2011 for Microsoft Windows; a re-release for Nintendo Switch, PlayStation 4" +
                " and Xbox One was released worldwide on 14 November 2017. ",
                DescriptionRu = "L.A. Noire — мультиплатформенная игра в жанре action-adventure/симулятор детектива," +
                                " разработанная австралийской студией Team Bondi и выпущенная издательством Rockstar Games 17 мая 2011 года на PlayStation 3 и Xbox 360." +
                                " 23 июня 2011 года было заявлено, что игра выйдет на ПК осенью 2011 года. Игра вышла на ПК в России 11 ноября 2011 г.",
                Price = (decimal)4.99,
                UnitsInStock = 19,
                Discontinued = false,
                PublishDate = new DateTime(2011, 11, 8),
                UploadDate = DateTime.UtcNow,
                Publisher = rockstar,
                Genres = new List<Genre>() { action, tps, misc },
                PlatformTypes = new List<PlatformType>() { desktop, console },
                ImageReference = "~/Content/Images/Games/la-noire.jpg"
            };
            var skyrim = new Game
            {
                Key = "g11",
                NameEn = "The Elder Scrolls V: Skyrim",
                NameRu = "The Elder Scrolls V: Skyrim",
                DescriptionEn = "The Elder Scrolls V: Skyrim is an open world action role-playing video game developed by" +
                " Bethesda Game Studios and published by Bethesda Softworks. It is the fifth main installment in The Elder" +
                " Scrolls series, following The Elder Scrolls IV: Oblivion, and was released worldwide for Microsoft Windows," +
                " PlayStation 3 and Xbox 360 on November 11, 2011.",
                DescriptionRu = "The Elder Scrolls V: Skyrim — мультиплатформенная компьютерная ролевая игра с открытым миром," +
                                " разработанная студией Bethesda Game Studios и выпущенная компанией Bethesda Softworks. Это пятая часть в серии The Elder Scrolls.",
                Price = (decimal)18.99,
                UnitsInStock = 88,
                Discontinued = false,
                PublishDate = new DateTime(2011, 11, 11),
                UploadDate = DateTime.UtcNow,
                Publisher = bethesda,
                Genres = new List<Genre> { rpg, adventure },
                PlatformTypes = new List<PlatformType> { desktop, console },
                ImageReference = "~/Content/Images/Games/skyrim.jpg"
            };
            var fallout = new Game
            { 
                Key = "g12",
                NameEn = "Fallout 4",
                NameRu = "Fallout 4",
                DescriptionEn = "Fallout 4 is a post-apocalyptic action role-playing video game developed by Bethesda Game" +
                " Studios and published by Bethesda Softworks. It is the fifth major installment in the Fallout series," +
                " and was released worldwide on November 10, 2015, for Microsoft Windows, PlayStation 4 and Xbox One. ",
                DescriptionRu = "Fallout 4 — компьютерная игра от Bethesda Softworks производства Bethesda Game Studios, сиквел Fallout 3." +
                                " Игра является пятой частью серии, и была выпущена 10 ноября 2015 года на Windows, PlayStation 4 и Xbox One",
                Price = (decimal)29.99,
                UnitsInStock = 16,
                Discontinued = true,
                PublishDate = new DateTime(2015, 11, 10),
                UploadDate = DateTime.UtcNow,
                Publisher = bethesda,
                Genres = new List<Genre> { rpg, action, tps, adventure },
                PlatformTypes = new List<PlatformType> { desktop, console },
                ImageReference = "~/Content/Images/Games/fallout4.jpeg"
            };
            var theLasOfUs = new Game
            {
                Key = "g13",
                NameEn = "The Last Of Us",
                NameRu = "Одни из нас",
                DescriptionEn = "The Last of Us is an action-adventure survival horror video game developed by Naughty Dog " +
                "and published by Sony Computer Entertainment. It was released for the PlayStation 3 worldwide on June " +
                "14, 2013. Players control Joel, a smuggler tasked with escorting a teenage girl named Ellie across a " +
                "post-apocalyptic United States.",
                DescriptionRu = "The Last of Us — компьютерная игра в жанре action-adventure с элементами survival horror и стелс-экшена, разработанная студией Naughty Dog" +
                                " и изданная Sony Computer Entertainment эксклюзивно для игровой консоли PlayStation 3 в 2013 году. 29 июля 2014 года вышла дополненная версия " +
                                "игры для игровой консоли PlayStation 4 — The Last of Us Remastered.",
                Price = (decimal)19.99,
                UnitsInStock = 5,
                Discontinued = false,
                PublishDate = new DateTime(2013, 6, 14),
                UploadDate = DateTime.UtcNow,
                Publisher = naughtyDog,
                Genres = new List<Genre>() { action, tps, misc, adventure },
                PlatformTypes = new List<PlatformType>() { console },
                ImageReference = "~/Content/Images/Games/the-last-of-us.jpg"
            };
            var walkingDead = new Game
            {
                Key = "g14",
                NameEn = "The Walking Dead",
                NameRu = "Ходячие мертвецы",
                DescriptionEn = "The Walking Dead (also known as The Walking Dead: The Game and The Walking Dead: Season One)" +
                " is an episodic interactive drama graphic adventure survival horror video game developed and published by " +
                "Telltale Games. ",
                DescriptionRu = "The Walking Dead: The Game — это эпизодическая видеоигра по мотивам комикса Роберта Киркмана «Ходячие мертвецы»." +
                                " Игра разработана студией Telltale Games. Изначально выход планировался на последние месяцы 2012 года." +
                                " Позже релиз The Walking Dead: The Game перенесли на 24 апреля 2012 года (США). Игра выпущена в формате 5 эпизодов," +
                                " выходящих с интервалом в один-два месяца, и дополнительным 6 эпизодом, выход которого состоялся 3 июля 2013. ",
                Price = (decimal)8.75,
                UnitsInStock = 43,
                Discontinued = false,
                PublishDate = new DateTime(2012, 4, 24),
                UploadDate = DateTime.UtcNow,
                Publisher = telltale,
                Genres = new List<Genre>() { action, misc },
                PlatformTypes = new List<PlatformType>() { desktop, console, mobile },
                ImageReference = "~/Content/Images/Games/the-walking-dead.jpg"
            };
            var stalker = new Game
            {
                Key = "g15",
                NameEn = "S.T.A.L.K.E.R.: Shadow of Chernobyl",
                NameRu = "S.T.A.L.K.E.R.: Тень Чернобыля",
                DescriptionEn = "S.T.A.L.K.E.R.: Shadow of Chernobyl is a first-person shooter survival horror video " +
                "game developed by Ukrainian game developer GSC Game World and published by THQ. The game is set in an" +
                " alternative reality, where a second nuclear disaster occurs at the Chernobyl Nuclear Power Plant Exclusion" +
                " Zone in the near future and causes strange changes in the area around it.",
                DescriptionRu = "S.T.Á.L.K.E.R.: Тень Черно́быля, ранее известная как S.T.A.L.K.E.R.: Oblivion Lost — компьютерная игра в жанре шутер от первого лица," +
                                " разработанная украинской компанией GSC Game World и изданная 20 марта 2007 года в США и Канаде и 23 марта 2007 года — в Европе и СНГ",
                Price = 5,
                UnitsInStock = 10,
                Discontinued = true,
                PublishDate = new DateTime(2007, 3, 20),
                UploadDate = DateTime.UtcNow,
                Publisher = gsc,
                Genres = new List<Genre>() { action, fps },
                PlatformTypes = new List<PlatformType>() { desktop },
                ImageReference = "~/Content/Images/Games/stalker.jpg"
            };
            var battlefield = new Game
            {
                Key = "g16",
                NameEn = "Battlefield 1",
                NameRu = "Поля сражений 1",
                DescriptionEn = "Battlefield 1 is a first-person shooter video game developed by EA DICE and published by" +
                " Electronic Arts. Despite its name, Battlefield 1 is the fifteenth installment in the Battlefield series," +
                " and the first main entry in the series since Battlefield 4. It was released worldwide for Microsoft" +
                " Windows, PlayStation 4, and Xbox One on October 21, 2016.",
                DescriptionRu = "Battlefield 1 — мультиплатформенная компьютерная игра в жанре шутера от первого лица, четырнадцатая по счету из серии игр Battlefield," +
                                " разрабатываемая компанией DICE и издаваемая Electronic Arts для платформ Windows, PlayStation 4 и Xbox One. Игра была анонсирована 6 мая 2016 года," +
                                " а выход состоялся 21 октября 2016 года[2]. Игра основана на событиях Первой мировой войны.",
                Price = (decimal)59.99,
                UnitsInStock = 75,
                Discontinued = false,
                PublishDate = new DateTime(2016, 10, 21),
                UploadDate = DateTime.UtcNow,
                Publisher = electronicArts,
                Genres = new List<Genre>() { action, fps },
                PlatformTypes = new List<PlatformType> { desktop, console },
                ImageReference = "~/Content/Images/Games/battlefield1.jpg"
            };
            context.Games.AddRange(new List<Game>
            {
                halfLife, needForSpeed, angryBirds, assassinsCreed2, farcCry5, tomClancy, crew,
                fifa, gta, laNoire, skyrim, fallout, theLasOfUs, walkingDead, stalker, battlefield
            });
            context.Comments.Add(new Comment
            {
                Name = "John",
                Body = "I really like this game.",
                Game = halfLife
            });
            context.Comments.Add(new Comment
            {
                Name = "Mark",
                Body = "John, I agree with you.",
                Game = halfLife,
                ParentCommentId = 1
            });
            context.Comments.Add(new Comment
            {
                Name = "Bruce",
                Body = "Mark, I thing so.",
                Game = halfLife,
                ParentCommentId = 2
            });
            context.Comments.Add(new Comment
            {
                Name = "Tom",
                Body = "John, As for me, this is bad game!",
                Game = halfLife,
                ParentCommentId = 1
            });
            context.Comments.Add(new Comment
            {
                Name = "Bob",
                Body = "Did not like it, many bugs!",
                Game = angryBirds
            });
            var admin = new Role { Name = "Administrator" }; 
            var manager = new Role { Name = "Manager" }; 
            var moderator = new Role { Name = "Moderator" }; 
            var user = new Role { Name = "User" };
            var publisher = new Role { Name = "Publisher" };
            context.Roles.AddRange(new List<Role> { admin, manager, moderator, user, publisher });
            var password = EncryptPassword("123456");
            context.Users.Add(new User
            {
                Name = "admin@gamestore.com",
                Email = "admin@gamestore.com",
                PasswordHash = password,
                Roles = new List<Role> { admin }
            });
            context.Users.Add(new User
            {
                Name = "manager@gamestore.com",
                Email = "manager@gamestore.com",
                PasswordHash = password,
                Roles = new List<Role> { manager }
            });
            context.Users.Add(new User
            {
                Name = "moderator@gamestore.com",
                Email = "moderator@gamestore.com",
                PasswordHash = password,
                Roles = new List<Role> { moderator }
            });
            base.Seed(context);
        }

        private string EncryptPassword(string password)
        {
            var salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}