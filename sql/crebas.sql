IF OBJECT_ID('Konobar_Porudzbina') IS NOT NULL DROP TABLE Konobar_Porudzbina; 
IF OBJECT_ID('StavkaPorudzbine') IS NOT NULL DROP TABLE StavkaPorudzbine; 
IF OBJECT_ID('Porudzbina') IS NOT NULL DROP TABLE Porudzbina;
IF OBJECT_ID('Rezervacija') IS NOT NULL DROP TABLE Rezervacija; 
IF OBJECT_ID('Sto') IS NOT NULL DROP TABLE Sto; 
IF OBJECT_ID('Jelo') IS NOT NULL DROP TABLE Jelo; 
IF OBJECT_ID('Konobar') IS NOT NULL DROP TABLE Konobar;
IF OBJECT_ID('Gost') IS NOT NULL DROP TABLE Gost;
IF OBJECT_ID('Restoran') IS NOT NULL DROP TABLE Restoran; 
GO
/*==============================================================*/ /* KREIRANJE TABELA */ /*==============================================================*/ 
--GOST
CREATE TABLE Gost ( IDGosta INT NOT NULL, 
ImeGosta NVARCHAR(50)NOT NULL,
PrezimeGosta NVARCHAR(50) NOT NULL,
Telefon NVARCHAR(50), 
Email NVARCHAR(50), 
CONSTRAINT PK_Gost PRIMARY KEY (IDGosta)
);
GO 
-- RESTORAN 
CREATE TABLE Restoran(
IDRestorana INT NOT NULL, 
NazivRestorana NVARCHAR(50) NOT NULL, 
Adresa NVARCHAR(50) NOT NULL,
Telefon NVARCHAR(50) NOT NULL, 
CONSTRAINT PK_Restoran PRIMARY KEY (IDRestorana)
); 
GO
-- KONOBAR
CREATE TABLE Konobar ( 
IDKonobara INT NOT NULL,
ImeKonobara NVARCHAR(30) NOT NULL, 
PrezimeKonobara NVARCHAR(30) NOT NULL,
GodineIskustva INT, 
Plata DECIMAL(10, 2), 
CONSTRAINT PK_Konobar PRIMARY KEY (IDKonobara)
); 
GO
-- JELO
CREATE TABLE Jelo ( 
IDJela INT NOT NULL,
NazivJela NVARCHAR(50) NOT NULL,
Opis NVARCHAR(200),
Cena DECIMAL(10, 2) NOT NULL,
Kategorija NVARCHAR(50) NOT NULL,  
CONSTRAINT PK_Jelo PRIMARY KEY (IDJela) );
GO
-- STO
CREATE TABLE Sto (
IDStola INT NOT NULL,
IDRestorana INT NOT NULL,
BrojStola INT NOT NULL, 
BrojMesta INT NOT NULL,
Lokacija NVARCHAR(20),
CONSTRAINT PK_Sto PRIMARY KEY (IDStola),
CONSTRAINT FK_Sto_Restoran FOREIGN KEY (IDRestorana) 
REFERENCES Restoran(IDRestorana)
);
GO 
-- REZERVACIJA 
CREATE TABLE Rezervacija (
IDRezervacije INT NOT NULL,
IDGosta INT NOT NULL, 
IDStola INT NOT NULL, 
Datum DATE NOT NULL,
Vreme TIME NOT NULL,
BrojOsoba INT NOT NULL,
CONSTRAINT PK_Rezervacija PRIMARY KEY (IDRezervacije), 
CONSTRAINT FK_Rezervacija_Gost FOREIGN KEY (IDGosta)
REFERENCES Gost(IDGosta), 
CONSTRAINT FK_Rezervacija_Sto FOREIGN KEY (IDStola)
REFERENCES Sto(IDStola) 
);
GO
-- PORUDZBINA
CREATE TABLE Porudzbina ( 
IDPorudzbine INT NOT NULL,
IDGosta INT NOT NULL, 
IDStola INT NOT NULL, 
Datum DATE NOT NULL,
Vreme TIME NOT NULL,
CONSTRAINT PK_Porudzbina PRIMARY KEY (IDPorudzbine),
CONSTRAINT FK_Porudzbina_Gost FOREIGN KEY (IDGosta) 
REFERENCES Gost(IDGosta), 
CONSTRAINT FK_Porudzbina_Sto FOREIGN KEY (IDStola) 
REFERENCES Sto(IDStola) 
); 
GO
-- KONOBAR_PORUDZBINA 
CREATE TABLE Konobar_Porudzbina ( 
IDKonobara INT NOT NULL,
IDPorudzbine INT NOT NULL,
CONSTRAINT PK_Konobar_Porudzbina PRIMARY KEY (IDKonobara, IDPorudzbine),
CONSTRAINT FK_KP_Konobar FOREIGN KEY (IDKonobara)
REFERENCES Konobar(IDKonobara), 
CONSTRAINT FK_KP_Porudzbina FOREIGN KEY (IDPorudzbine)
REFERENCES Porudzbina(IDPorudzbine) 
); 
GO
-- STAVKAPORUDZBINE
CREATE TABLE StavkaPorudzbine (
IDPorudzbine INT NOT NULL,
IDJela INT NOT NULL,
Kolicina INT NOT NULL, 
CONSTRAINT PK_StavkaPorudzbine PRIMARY KEY (IDPorudzbine, IDJela),
CONSTRAINT FK_SP_Porudzbina FOREIGN KEY (IDPorudzbine) 
REFERENCES Porudzbina(IDPorudzbine),
CONSTRAINT FK_SP_Jelo FOREIGN KEY (IDJela)
REFERENCES Jelo(IDJela) 
);
GO
/*==============================================================*/ /* INICIJALNI PODACI */ /*=====================================

=========================*/
INSERT INTO Restoran (IDRestorana, NazivRestorana, Adresa, Telefon)
VALUES (1, 'Restoran Kod Maltera', 'Glavna 10', '0601234567');
GO