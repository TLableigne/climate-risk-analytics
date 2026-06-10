/*
 ----------------------------------------------------------------------------
             Génération d'une base de données pour
                        SQL Server 2005
                       (10/6/2026 8:54:46)
 ----------------------------------------------------------------------------
     Nom de la base : MLRAqui
     Projet : 
     Auteur : CRIIUT
     Date de dernière modification : 10/6/2026 8:53:58
 ----------------------------------------------------------------------------
*/

drop database MLRAqui
go

/* -----------------------------------------------------------------------------
      OUVERTURE DE LA BASE MLRAqui
----------------------------------------------------------------------------- */

create database MLRAqui
go

use MLRAqui
go



/* -----------------------------------------------------------------------------
      TABLE : RISQUE_COMMUNE
----------------------------------------------------------------------------- */

create table RISQUE_COMMUNE
  (
     CODE_COMMUNE char(32)  not null  ,
     NUM_RISQUE char(3)  null  
     ,
     constraint PK_RISQUE_COMMUNE primary key (CODE_COMMUNE)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : REGION
----------------------------------------------------------------------------- */

create table REGION
  (
     CODE_REGION char(2)  not null  ,
     LIB_REGION char(100)  null  
     ,
     constraint PK_REGION primary key (CODE_REGION)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : DEPARTEMENT
----------------------------------------------------------------------------- */

create table DEPARTEMENT
  (
     CODE_DEPARTEMENT char(3)  not null  ,
     CODE_REGION_1 char(2)  not null  ,
     CODE_COMMUNE char(32)  not null  ,
     LIB_DEPARTEMENT char(100)  null  ,
     CODE_REGION char(2)  null  
     ,
     constraint PK_DEPARTEMENT primary key (CODE_DEPARTEMENT)
  ) 
go



/*      INDEX DE DEPARTEMENT      */



/* -----------------------------------------------------------------------------
      TABLE : TYPE_RISQUE
----------------------------------------------------------------------------- */

create table TYPE_RISQUE
  (
     NUM_RISQUE char(3)  not null  ,
     LIB_RISQUE char(150)  null  ,
     CATEGORIE char(50)  null  
     ,
     constraint PK_TYPE_RISQUE primary key (NUM_RISQUE)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : STATION
----------------------------------------------------------------------------- */

create table STATION
  (
     NUM_POSTE char(8)  not null  ,
     ID smallint  not null  ,
     NOM_USUEL char(100)  null  ,
     LATITUDE decimal(9,6)  null  ,
     LONGITUDE char(9)  null  ,
     ALTITUDE char(6)  null  ,
     CODE_COMMUNE char(5)  null  
     ,
     constraint PK_STATION primary key (NUM_POSTE)
  ) 
go



/*      INDEX DE STATION      */



/* -----------------------------------------------------------------------------
      TABLE : CATASTROPHE_NATURELLE
----------------------------------------------------------------------------- */

create table CATASTROPHE_NATURELLE
  (
     CODE_NAT_CATNAT char(12)  not null  ,
     CODE_COMMUNE char(5)  null  ,
     NUM_RISQUE char(3)  null  ,
     DATE_DEB datetime  null  ,
     DATE_FIN datetime  null  
     ,
     constraint PK_CATASTROPHE_NATURELLE primary key (CODE_NAT_CATNAT)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : COMMUNE
----------------------------------------------------------------------------- */

create table COMMUNE
  (
     CODE_COMMUNE char(32)  not null  ,
     NUM_POSTE char(8)  not null  ,
     CODE_COMMUNE_EXPOSÉ_À char(32)  not null  ,
     CODE_NAT_CATNAT char(12)  not null  ,
     LIB_COMMUNE char(32)  null  ,
     COD_REGION char(32)  null  
     ,
     constraint PK_COMMUNE primary key (CODE_COMMUNE)
  ) 
go



/*      INDEX DE COMMUNE      */



/* -----------------------------------------------------------------------------
      TABLE : MESURE
----------------------------------------------------------------------------- */

create table MESURE
  (
     ID smallint identity (1, 1)   ,
     NUM_POSTE char(8)  null  ,
     DATE_OBS datetime  null  ,
     VALEUR decimal(5,1)  null  ,
     UNITE char(50)  null  
     ,
     constraint PK_MESURE primary key (ID)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : CARACTÉRISE
----------------------------------------------------------------------------- */

create table CARACTÉRISE
  (
     NUM_RISQUE char(3)  not null  ,
     CODE_NAT_CATNAT char(12)  not null  
     ,
     constraint PK_CARACTÉRISE primary key (NUM_RISQUE, CODE_NAT_CATNAT)
  ) 
go



/*      INDEX DE CARACTÉRISE      */



/* -----------------------------------------------------------------------------
      TABLE : DÉFINIT
----------------------------------------------------------------------------- */

create table DÉFINIT
  (
     NUM_RISQUE char(3)  not null  ,
     CODE_COMMUNE char(32)  not null  
     ,
     constraint PK_DÉFINIT primary key (NUM_RISQUE, CODE_COMMUNE)
  ) 
go



/*      INDEX DE DÉFINIT      */



/* -----------------------------------------------------------------------------
      REFERENCES SUR LES TABLES
----------------------------------------------------------------------------- */



alter table DEPARTEMENT 
     add constraint FK_DEPARTEMENT_REGION foreign key (CODE_REGION_1) 
               references REGION (CODE_REGION)
go




alter table DEPARTEMENT 
     add constraint FK_DEPARTEMENT_COMMUNE foreign key (CODE_COMMUNE) 
               references COMMUNE (CODE_COMMUNE)
go




alter table STATION 
     add constraint FK_STATION_MESURE foreign key (ID) 
               references MESURE (ID)
go




alter table COMMUNE 
     add constraint FK_COMMUNE_STATION foreign key (NUM_POSTE) 
               references STATION (NUM_POSTE)
go




alter table COMMUNE 
     add constraint FK_COMMUNE_RISQUE_COMMUNE foreign key (CODE_COMMUNE_EXPOSÉ_À) 
               references RISQUE_COMMUNE (CODE_COMMUNE)
go




alter table COMMUNE 
     add constraint FK_COMMUNE_CATASTROPHE_NATURELLE foreign key (CODE_NAT_CATNAT) 
               references CATASTROPHE_NATURELLE (CODE_NAT_CATNAT)
go




alter table CARACTÉRISE 
     add constraint FK_CARACTÉRISE_TYPE_RISQUE foreign key (NUM_RISQUE) 
               references TYPE_RISQUE (NUM_RISQUE)
go




alter table CARACTÉRISE 
     add constraint FK_CARACTÉRISE_CATASTROPHE_NATURELLE foreign key (CODE_NAT_CATNAT) 
               references CATASTROPHE_NATURELLE (CODE_NAT_CATNAT)
go




alter table DÉFINIT 
     add constraint FK_DÉFINIT_TYPE_RISQUE foreign key (NUM_RISQUE) 
               references TYPE_RISQUE (NUM_RISQUE)
go




alter table DÉFINIT 
     add constraint FK_DÉFINIT_RISQUE_COMMUNE foreign key (CODE_COMMUNE) 
               references RISQUE_COMMUNE (CODE_COMMUNE)
go




/*
 -----------------------------------------------------------------------------
               FIN DE GENERATION
 -----------------------------------------------------------------------------
*/