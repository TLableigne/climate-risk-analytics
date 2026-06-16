/*
 ----------------------------------------------------------------------------
             Génération d'une base de données pour
                        SQL Server 2005
                       (11/6/2026 9:26:09)
 ----------------------------------------------------------------------------
     Nom de la base : AquiAlerts
     Projet : AquiALertsTravl
     Auteur : CRIIUT
     Date de dernière modification : 11/6/2026 9:25:52
 ----------------------------------------------------------------------------
*/
use master 
go

ALTER DATABASE AquiAlerts SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
go


drop database AquiAlerts
go

/* -----------------------------------------------------------------------------
      OUVERTURE DE LA BASE AquiAlerts
----------------------------------------------------------------------------- */

create database AquiAlerts
go

use AquiAlerts
go

create user ETD
go

create user [IUTBX\cjenipan]
go

create user [IUTBX\lleguennec]
go

create user [IUTBX\lganchegui]
go

create user [IUTBX\tlableigne]
go

create user [IUTBX\astyzinski]
go


grant view definition, insert, update, delete, select to ETD
go

grant view definition, insert, update, delete, select to [IUTBX\cjenipan]
go

grant view definition, insert, update, delete, select to [IUTBX\lleguennec]
go

grant view definition, insert, update, delete, select to [IUTBX\lganchegui]
go

grant view definition, insert, update, delete, select to [IUTBX\tlableigne]
go

grant view definition, insert, update, delete, select to [IUTBX\astyzinski]
go


/* -----------------------------------------------------------------------------
      TABLE : T_DEPARTEMENT_DEP
----------------------------------------------------------------------------- */

create table T_DEPARTEMENT_DEP
  (
     DEP_ID smallint  not null  ,
     DEP_LIB char(80)  null  
     ,
     constraint PK_T_DEPARTEMENT_DEP primary key (DEP_ID)
  ) 
go



/*      INDEX DE T_DEPARTEMENT_DEP      */



/* -----------------------------------------------------------------------------
      TABLE : T_MESURE_MES
----------------------------------------------------------------------------- */

create table T_MESURE_MES
  (
     ID_MES int identity (1, 1)   ,
	 MES_VALEUR float  null  ,
     COM_REF_MES int  null  ,
     TYP_REF_MES int  null  ,
     DAT_REF_MES smallint  null  
     ,
     constraint PK_T_MESURE_MES primary key (ID_MES)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : T_COMMUNE_COM
----------------------------------------------------------------------------- */

create table T_COMMUNE_COM
  (
     COM_ID int  not null  ,
     COM_LIB char(80)  null  ,
     DEP_REF_COM smallint  null  
     ,
     constraint PK_T_COMMUNE_COM primary key (COM_ID)
  ) 
go



/*      INDEX DE T_COMMUNE_COM      */



/* -----------------------------------------------------------------------------
      TABLE : T_DATE_DAT
----------------------------------------------------------------------------- */

create table T_DATE_DAT
  (
     DAT_ID smallint identity (1, 1)   ,
     DAT_DATE_DEB datetime  null  ,
     DAT_DUREE smallint  null  
     ,
     constraint PK_T_DATE_DAT primary key (DAT_ID)
  ) 
go



/*      INDEX DE T_DATE_DAT      */



/* -----------------------------------------------------------------------------
      TABLE : T_RISQUE_RIS
----------------------------------------------------------------------------- */

create table T_RISQUE_RIS
  (
     ID_RIS smallint identity (1, 1)   ,
     COM_REF_RIS int  null  ,
     TYP_REF_RIS int  null  ,
     DAT_REF_RIS smallint  null  
     ,
     constraint PK_T_RISQUE_RIS primary key (ID_RIS)
  ) 
go



/* -----------------------------------------------------------------------------
      TABLE : T_TYPE_TYP
----------------------------------------------------------------------------- */

create table T_TYPE_TYP
  (
     TYP_ID int identity (1, 1)   ,
     TYP_LIB char(80)  null  
     ,
     constraint PK_T_TYPE_TYP primary key (TYP_ID)
  ) 
go



/*      INDEX DE T_TYPE_TYP      */



/* -----------------------------------------------------------------------------
      REFERENCES SUR LES TABLES
----------------------------------------------------------------------------- */



alter table T_COMMUNE_COM 
     add constraint FK_T_COMMUNE_COM_T_DEPARTEMENT_DEP foreign key (DEP_REF_COM) 
               references T_DEPARTEMENT_DEP (DEP_ID)
go

alter table T_MESURE_MES 
     add constraint FK_T_MESURE_MES_T_COMMUNE_COM foreign key (COM_REF_MES) 
               references T_COMMUNE_COM (COM_ID)
go

alter table T_MESURE_MES 
     add constraint FK_T_MESURE_MES_T_TYPE_TYP foreign key (TYP_REF_MES) 
               references T_TYPE_TYP (TYP_ID)
go

alter table T_MESURE_MES 
     add constraint FK_T_MESURE_MES_T_DATE_DAT foreign key (DAT_REF_MES) 
               references T_DATE_DAT (DAT_ID)
go

alter table T_RISQUE_RIS 
     add constraint FK_T_RISQUE_RIS_T_COMMUNE_COM foreign key (COM_REF_RIS) 
               references T_COMMUNE_COM (COM_ID)
go

alter table T_RISQUE_RIS 
     add constraint FK_T_RISQUE_RIS_T_TYPE_TYP foreign key (TYP_REF_RIS) 
               references T_TYPE_TYP (TYP_ID)
go

alter table T_RISQUE_RIS 
     add constraint FK_T_RISQUE_RIS_T_DATE_DAT foreign key (DAT_REF_RIS) 
               references T_DATE_DAT (DAT_ID)
go




/*
 -----------------------------------------------------------------------------
               FIN DE GENERATION
 -----------------------------------------------------------------------------
*/