ALTER TABLE Data
ADD Id INT IDENTITY(1,1) PRIMARY KEY;

Alter table Data add foreign key (Kom_nr) references Kommune;
Alter table Data add foreign key (GruppeId) references Keynummer;
Alter table Data add foreign key (Aarstal) references Aarstal;



Select Data.Id, Data.Kom_nr, City, Gruppe, Aarstal, tal
From Data Join Keynummer on Data.GruppeId = Keynummer.Id
Join Kommune on Data.Kom_nr = Kommune.Kom_nr