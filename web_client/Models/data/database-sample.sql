insert into SchoolTable (name, address) values ("VŠB-TUO", "17. listopadu 2172/15, 708 00 Ostrava 8");
insert into SchoolTable (name, address) values ("Masarykova univerzita", "Žerotínovo nám. 617/9, 601 77 Brno");
insert into SchoolTable (name, address) values ("Ostravská univerzita", "Dvořákova 138/7, 701 03 Moravská Ostrava a Přívoz");

insert into ProgramTable (SchoolId, Name, Description, Capacity) values (1, 'Informatika', 'Studijní program Informatika je logickým pokračováním bakalářského programu stejného zaměření. Studenti mají možnost specializovat se na některou z definovaných oblastí informatiky. Oblasti, kterým se studenti mohou věnovat, zahrnují strojové učení, hluboké učení, analýzy sociálních sítí, strojové vidění, paralelní programování, vývoj softwarových systémů, teoretickou informatiku počítačové sítě a systémy, biologicky inspirované algoritmy a mnoho dalších. Poměr praktických a teoretických znalostí je vyvážen tak, aby absolventi byli schopni být platnými členy vývojových i výzkumných týmů v softwarových i jiných firmách.', 300);
insert into ProgramTable (SchoolId, Name, Description, Capacity) values (1, 'Komunikační a informační technologie', 'Absolvent tříletého studijního programu Komunikační a informační technologie (KIT) rozvíjí dovednosti a znalosti v zaměřeních Komunikační sítě, Mobilní a rádiové komunikace (MaRK) a Optické komunikace a senzory (OKaS). Vyniká širokými teoretickými a praktickými znalostmi v oblastech výše uvedených zaměření, které dokáže uplatnit jak na technických, tak na řídících pozicích.', 100);
insert into ProgramTable (SchoolId, Name, Description, Capacity) values (1, 'Aplikovaná fyzika', 'Aplikovaná fyzika na VŠB - TU Ostrava nabízí nejen široké teoretické znalosti v oblasti fyziky, ale hlavně praktické zapojení do práce ve firmách. Součástí studijních plánů jsou praxe ve firmách svázaných s vědeckovýzkumnými zaměřeními klíčových odborníků Katedry fyziky FEI VŠB - TU Ostrava. Na problematiku těchto firem jsou také navázána témata kvalifikačních prací. Experimentální část kvalifikační (bakalářské) práce tak studenti řeší přímo v příslušné firmě, která má zájem na tom, aby se student po absolvování studia stal jejím zaměstnancem. Hlavními tématy jsou jaderná fyzika, magnetismus, optika a technologie porušování materiálů.', 2);
insert into ProgramTable (SchoolId, Name, Description, Capacity) values (2, 'Informatika', 'Studiem bakalářského studijního programu Informatika získáte základní vzdělání v tomto vědním oboru. Čeká vás studium programovacích jazyků, principů návrhu a implementace algoritmů, ale také studium nezbytných technologií týkajících se například počítačových sítí a informační bezpečnosti. Seznámíte se také s nezbytnou teorií výpočetní složitosti a narazíte i na problémy, které žádný počítač na světě nikdy nespočítá. Poznáte mnoho dalších metod a přístupů souvisejících s návrhem a analýzou komplexních systémů. Na naší fakultě se z vás stane odborník s odpovídajícím analytickým myšlením a komplexním nahlížením na řešení celé řady informatických aspektů, ale i problémů přesahujících do běžného civilního života.', 120);
insert into ProgramTable (SchoolId, Name, Description, Capacity) values (2, 'Kyberbezpečnost', 'Cítíte příležitost na rychle rostoucím pracovním trhu odborníků na kyberbezpečnost? Baví vás dívat se počítačovým systémům "pod kůži", dozvědět se co nejvíce o jejich podstatě, vlastnostech a chování? Chcete znát motivace kyberútočníků a orientovat se v právním prostředí IT? Chcete studovat na prestižní škole disponující vlastním Kybernetickým polygonem a spolupracující se špičkovými firmami a vedoucími institucemi místní, národní i mezinárodní úrovně? Pak je profesní bakalářský program Kyberbezpečnost tou pravou volbou. V českém prostředí unikátní multidisciplinární tříleté profesní bakalářské studium zahrnuje řízenou odbornou stáž a připravuje studenty pro bezprostřední nástup do praxe, přičemž zájemci mohou pokračovat v navazujícím magisterském studiu.', 120);

insert into StudentTable (Name, Address, Email, Password) values ("Jan Novák", "Adresní 69, Praha 694 20", "jan.novak@seznam.cz", "password");
insert into StudentTable (Name, Address, Email, Password) values ("Jiří Dvořák", "Vymyšlená 420, Brno 420 69", "jiri.dvorak@gmail.com", "P@ssw0rd");
insert into StudentTable (Name, Address, Email, Password) values ("TEST", "TEST", "TEST@TEST.TEST", "TEST");

insert into ApplicationTable (StudentId, ProgramId) values (3, 3);
insert into ApplicationTable (StudentId, ProgramId) values (2, 3);

insert into ApplicationTable (StudentId, ProgramId) values (1, 2);
insert into ApplicationTable (StudentId, ProgramId) values (2, 2);
insert into ApplicationTable (StudentId, ProgramId) values (3, 2);

insert into ApplicationTable (StudentId, ProgramId) values (1, 4);
insert into ApplicationTable (StudentId, ProgramId) values (1, 5);

insert into ApplicationTable (StudentId, ProgramId) values (3, 5);
