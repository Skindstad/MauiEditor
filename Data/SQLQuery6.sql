Alter table Data add foreign key (Kom_nr) references Kommune;
Alter table Data add foreign key (GruppeId) references Keynummer;
Alter table Data add foreign key (Aarstal) references Aarstal;



Create view GruppeRef As
Select Data.Id, Data.Kom_nr, City, Gruppe, Aarstal, tal
From Data Join Keynummer on Data.GruppeId = Keynummer.Id
Join Kommune on Data.Kom_nr = Kommune.Kom_nr