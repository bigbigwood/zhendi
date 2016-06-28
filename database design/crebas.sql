/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     6/28/2016 5:57:27 PM                         */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DEVICE_CONTROLLERS_PARAMETERS') and o.name = 'FK_DEVICE_C_REFERENCE_DEVICE_C')
alter table DEVICE_CONTROLLERS_PARAMETERS
   drop constraint FK_DEVICE_C_REFERENCE_DEVICE_C
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DEVICE_DOORS') and o.name = 'FK_DEVICE_D_REFERENCE_DEVICE_R')
alter table DEVICE_DOORS
   drop constraint FK_DEVICE_D_REFERENCE_DEVICE_R
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DEVICE_HEADREADINGS') and o.name = 'FK_DEVICE_H_REFERENCE_DEVICE_R')
alter table DEVICE_HEADREADINGS
   drop constraint FK_DEVICE_H_REFERENCE_DEVICE_R
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DEVICE_PERMISSIONS') and o.name = 'FK_DEVICE_P_REFERENCE_DEVICE_C')
alter table DEVICE_PERMISSIONS
   drop constraint FK_DEVICE_P_REFERENCE_DEVICE_C
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DEVICE_ROLES_PERMISSIONS') and o.name = 'FK_DEVICE_R_REFERENCE_DEVICE_P')
alter table DEVICE_ROLES_PERMISSIONS
   drop constraint FK_DEVICE_R_REFERENCE_DEVICE_P
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DEVICE_ROLES_PERMISSIONS') and o.name = 'FK_DEVICE_R_REFERENCE_DEVICE_R')
alter table DEVICE_ROLES_PERMISSIONS
   drop constraint FK_DEVICE_R_REFERENCE_DEVICE_R
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_DEPARTMENT_DEVICES') and o.name = 'FK_SYS_DEPA_REFERENCE_SYS_DEPA')
alter table SYS_DEPARTMENT_DEVICES
   drop constraint FK_SYS_DEPA_REFERENCE_SYS_DEPA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_DEPARTMENT_DEVICES') and o.name = 'FK_SYS_DEPA_REFERENCE_DEVICE_C')
alter table SYS_DEPARTMENT_DEVICES
   drop constraint FK_SYS_DEPA_REFERENCE_DEVICE_C
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_MODULE_ELEMENTS') and o.name = 'FK_SYS_MODU_REFERENCE_SYS_MODU')
alter table SYS_MODULE_ELEMENTS
   drop constraint FK_SYS_MODU_REFERENCE_SYS_MODU
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_ROLE_MODULEELEMENT') and o.name = 'FK_SYS_ROLE_REFERENCE_SYS_MODU')
alter table SYS_ROLE_MODULEELEMENT
   drop constraint FK_SYS_ROLE_REFERENCE_SYS_MODU
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_ROLE_MODULEELEMENT') and o.name = 'FK_SYS_ROLE_REFERENCE_SYS_ROLE')
alter table SYS_ROLE_MODULEELEMENT
   drop constraint FK_SYS_ROLE_REFERENCE_SYS_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER') and o.name = 'FK_SYS_USER_REFERENCE_SYS_DEPA')
alter table SYS_USER
   drop constraint FK_SYS_USER_REFERENCE_SYS_DEPA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_AUTHENTICATION') and o.name = 'FK_AUTHENTICATION_USERID')
alter table SYS_USER_AUTHENTICATION
   drop constraint FK_AUTHENTICATION_USERID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_DEVICE_ROLES') and o.name = 'FK_SYS_USER_REFERENCE_DEVICE_R')
alter table SYS_USER_DEVICE_ROLES
   drop constraint FK_SYS_USER_REFERENCE_DEVICE_R
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_DEVICE_ROLES') and o.name = 'FK_DEVICE_ROLE_USERID')
alter table SYS_USER_DEVICE_ROLES
   drop constraint FK_DEVICE_ROLE_USERID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_OPERATOR') and o.name = 'FK_OPERATOR_USERID')
alter table SYS_USER_OPERATOR
   drop constraint FK_OPERATOR_USERID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_PROPERTY') and o.name = 'FK_PROPERTY_USERID')
alter table SYS_USER_PROPERTY
   drop constraint FK_PROPERTY_USERID
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_ROLE') and o.name = 'FK_SYS_USER_REFERENCE_SYS_ROLE')
alter table SYS_USER_ROLE
   drop constraint FK_SYS_USER_REFERENCE_SYS_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SYS_USER_ROLE') and o.name = 'FK_SYS_USER_REFERENCE_SYS_USER')
alter table SYS_USER_ROLE
   drop constraint FK_SYS_USER_REFERENCE_SYS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TIME_GROUP_SEGMENT') and o.name = 'FK_TIME_GRO_REFERENCE_TIME_GRO')
alter table TIME_GROUP_SEGMENT
   drop constraint FK_TIME_GRO_REFERENCE_TIME_GRO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TIME_GROUP_SEGMENT') and o.name = 'FK_TIME_GRO_REFERENCE_TIME_SEG')
alter table TIME_GROUP_SEGMENT
   drop constraint FK_TIME_GRO_REFERENCE_TIME_SEG
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TIME_ZONE_GROUP') and o.name = 'FK_TIME_ZON_REFERENCE_TIME_ZON')
alter table TIME_ZONE_GROUP
   drop constraint FK_TIME_ZON_REFERENCE_TIME_ZON
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TIME_ZONE_GROUP') and o.name = 'FK_TIME_ZON_REFERENCE_TIME_GRO')
alter table TIME_ZONE_GROUP
   drop constraint FK_TIME_ZON_REFERENCE_TIME_GRO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_CONTROLLERS')
            and   type = 'U')
   drop table DEVICE_CONTROLLERS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_CONTROLLERS_PARAMETERS')
            and   type = 'U')
   drop table DEVICE_CONTROLLERS_PARAMETERS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_DOORS')
            and   type = 'U')
   drop table DEVICE_DOORS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_HEADREADINGS')
            and   type = 'U')
   drop table DEVICE_HEADREADINGS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_OPERATION_LOG')
            and   type = 'U')
   drop table DEVICE_OPERATION_LOG
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_PERMISSIONS')
            and   type = 'U')
   drop table DEVICE_PERMISSIONS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_ROLES')
            and   type = 'U')
   drop table DEVICE_ROLES
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_ROLES_PERMISSIONS')
            and   type = 'U')
   drop table DEVICE_ROLES_PERMISSIONS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_STATE_HISTORY')
            and   type = 'U')
   drop table DEVICE_STATE_HISTORY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DEVICE_TRAFFIC_LOG')
            and   type = 'U')
   drop table DEVICE_TRAFFIC_LOG
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_DEPARTMENT')
            and   type = 'U')
   drop table SYS_DEPARTMENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_DEPARTMENT_DEVICES')
            and   type = 'U')
   drop table SYS_DEPARTMENT_DEVICES
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_DICTIONARY')
            and   type = 'U')
   drop table SYS_DICTIONARY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_MODULE')
            and   type = 'U')
   drop table SYS_MODULE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_MODULE_ELEMENTS')
            and   type = 'U')
   drop table SYS_MODULE_ELEMENTS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_OPERATION_LOG')
            and   type = 'U')
   drop table SYS_OPERATION_LOG
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_ROLE')
            and   type = 'U')
   drop table SYS_ROLE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_ROLE_MODULEELEMENT')
            and   type = 'U')
   drop table SYS_ROLE_MODULEELEMENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_USER')
            and   type = 'U')
   drop table SYS_USER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_USER_AUTHENTICATION')
            and   type = 'U')
   drop table SYS_USER_AUTHENTICATION
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_USER_DEVICE_ROLES')
            and   type = 'U')
   drop table SYS_USER_DEVICE_ROLES
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_USER_OPERATOR')
            and   type = 'U')
   drop table SYS_USER_OPERATOR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_USER_PROPERTY')
            and   type = 'U')
   drop table SYS_USER_PROPERTY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_USER_ROLE')
            and   type = 'U')
   drop table SYS_USER_ROLE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TIME_GROUPS')
            and   type = 'U')
   drop table TIME_GROUPS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TIME_GROUP_SEGMENT')
            and   type = 'U')
   drop table TIME_GROUP_SEGMENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TIME_SEGMENTS')
            and   type = 'U')
   drop table TIME_SEGMENTS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TIME_ZONE')
            and   type = 'U')
   drop table TIME_ZONE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TIME_ZONE_GROUP')
            and   type = 'U')
   drop table TIME_ZONE_GROUP
go

/*==============================================================*/
/* Table: DEVICE_CONTROLLERS                                    */
/*==============================================================*/
create table DEVICE_CONTROLLERS (
   DEVICEID             int                  not null,
   MAC                  nvarchar(100)        not null,
   DEVICECODE           nvarchar(100)        not null,
   SN                   nvarchar(100)        not null,
   MODE                 nvarchar(100)        null,
   COMMUNITIONTYPE      int                  not null,
   BAUDRATE             nvarchar(100)        null,
   SERIALPORT           nvarchar(100)        null,
   PASSWORD             nvarchar(100)        null,
   IP                   nvarchar(100)        null,
   PORT                 nvarchar(100)        null,
   PROTOCOL             nvarchar(100)        null,
   LABEL                nvarchar(1024)       null,
   SERVERURL            nvarchar(1024)       null,
   REMARK               nvarchar(1024)       null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         datetime             null,
   UPDATEDATE           datetime             null,
   constraint PK_DEVICE_CONTROLLERS primary key nonclustered (DEVICEID)
)
go

/*==============================================================*/
/* Table: DEVICE_CONTROLLERS_PARAMETERS                         */
/*==============================================================*/
create table DEVICE_CONTROLLERS_PARAMETERS (
   DEVICEPARAMETERID    int                  not null,
   DEVICEID             int                  null,
   AUTHTICATIONTYPE     int                  not null,
   AUTOOPENTIMEZONE     int                  null,
   ISSNEAK              bit                  null,
   MULTIPERSONLOCK      bit                  null,
   LINKAGE              bit                  null,
   LAUNCHDURESS_        bit                  null,
   DURESSFINGERPRINT    int                  null,
   DURESSOPEN           bit                  null,
   DURESSALARM          bit                  null,
   DURESSPASSWORD       nvarchar(100)        null,
   constraint PK_DEVICE_CONTROLLERS_PARAMETE primary key nonclustered (DEVICEPARAMETERID)
)
go

/*==============================================================*/
/* Table: DEVICE_DOORS                                          */
/*==============================================================*/
create table DEVICE_DOORS (
   DEVICEDOORID         int                  not null,
   DEVICEROLEID         int                  null,
   DEVICEID             int                  not null,
   NAME                 nvarchar(100)        not null,
   ELECTRICALAPPLIANCES int                  null,
   OPENTYPE             int                  null,
   STATUS               int                  null,
   REMARK               nvarchar(1024)       null,
   DELAYTIME            int                  null,
   ALERTTYPE            int                  null,
   OVERTIMEOPEN         int                  null,
   ISOVERTIME           bit                  null,
   FORCEOPEN            bit                  null,
   CONNECTIONALERM      bit                  null,
   LAUNCHDURESS_        bit                  null,
   DURESSFINGERPRINT    int                  null,
   DURESSOPEN           bit                  null,
   DURESSALARM          bit                  null,
   DURESSPASSWORD       nvarchar(100)        null,
   constraint PK_DEVICE_DOORS primary key nonclustered (DEVICEDOORID)
)
go

/*==============================================================*/
/* Table: DEVICE_HEADREADINGS                                   */
/*==============================================================*/
create table DEVICE_HEADREADINGS (
   DEVICEHEADREADINGID  int                  not null,
   DEVICEROLEID         int                  null,
   DEVICEID             int                  not null,
   MAC                  nvarchar(100)        not null,
   HEADREADINGSN        nvarchar(100)        not null,
   HEADREADINGTYPE      int                  not null,
   HEADREADINGPERFORMANCE nvarchar(100)        null,
   STATUS               int                  not null,
   constraint PK_DEVICE_HEADREADINGS primary key nonclustered (DEVICEHEADREADINGID)
)
go

/*==============================================================*/
/* Table: DEVICE_OPERATION_LOG                                  */
/*==============================================================*/
create table DEVICE_OPERATION_LOG (
   LOGID                int                  not null,
   REGISTERID           int                  null,
   DEVICEID             int                  not null,
   DEVICETYPE           int                  not null,
   OPERATIONTYPE        int                  not null,
   OPERATORID           int                  not null,
   OPERATIONCONTENT     nvarchar(1024)       null,
   OPERATIONTIME        nvarchar(1024)       not null,
   OPERATIONUPLOADTIME  nvarchar(1024)       null,
   constraint PK_DEVICE_OPERATION_LOG primary key nonclustered (LOGID)
)
go

/*==============================================================*/
/* Table: DEVICE_PERMISSIONS                                    */
/*==============================================================*/
create table DEVICE_PERMISSIONS (
   DEVICEPERMISSIONID   int                  not null,
   DEVICEID             int                  not null,
   ENABLE               bit                  not null,
   REMARK               nvarchar(1024)       null,
   PERMISSIONACTION     int                  not null,
   USERGROUPVM          nvarchar(1024)       null,
   ACCESSTIMEZONE       int                  not null,
   STARTDATE            datetime             not null,
   ENDDATE              datetime             null,
   constraint PK_DEVICE_PERMISSIONS primary key nonclustered (DEVICEPERMISSIONID)
)
go

/*==============================================================*/
/* Table: DEVICE_ROLES                                          */
/*==============================================================*/
create table DEVICE_ROLES (
   DEVICEROLEID         int                  not null,
   ROLENAME             nvarchar(100)        not null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_DEVICE_ROLES primary key nonclustered (DEVICEROLEID)
)
go

/*==============================================================*/
/* Table: DEVICE_ROLES_PERMISSIONS                              */
/*==============================================================*/
create table DEVICE_ROLES_PERMISSIONS (
   DEVICEROLEPERMISSIONID int                  not null,
   DEVICEROLEID         int                  not null,
   DEVICEPERMISSIONID   int                  not null,
   constraint PK_DEVICE_ROLES_PERMISSIONS primary key nonclustered (DEVICEROLEPERMISSIONID)
)
go

/*==============================================================*/
/* Table: DEVICE_STATE_HISTORY                                  */
/*==============================================================*/
create table DEVICE_STATE_HISTORY (
   DEVICESTATEHISTORYID int                  not null,
   DEVICEID             int                  not null,
   DEVICETYPE           int                  not null,
   DEVICESN             nvarchar(100)        not null,
   RECORDTYPE           int                  null,
   RECORDTIME           datetime             null,
   DOORSTATUS           int                  not null,
   REMARK               nvarchar(1024)       null,
   constraint PK_DEVICE_STATE_HISTORY primary key nonclustered (DEVICESTATEHISTORYID)
)
go

/*==============================================================*/
/* Table: DEVICE_TRAFFIC_LOG                                    */
/*==============================================================*/
create table DEVICE_TRAFFIC_LOG (
   TRAFFICID            int                  not null,
   DEVICEID             int                  not null,
   DEVICETYPE           int                  not null,
   DEVICESN             nvarchar(100)        not null,
   RECORDTYPE           nvarchar(1024)       null,
   RECORDTIME           datetime             null,
   RECORDUPLOADTIME     datetime             null,
   AUTHENTICATIONTYPE   int                  null,
   REMARK               nvarchar(1024)       null,
   constraint PK_DEVICE_TRAFFIC_LOG primary key nonclustered (TRAFFICID)
)
go

/*==============================================================*/
/* Table: SYS_DEPARTMENT                                        */
/*==============================================================*/
create table SYS_DEPARTMENT (
   DEPARTMENTID         int                  not null,
   NAME                 nvarchar(100)        not null,
   DEPARTMENTCODE       nvarchar(25)         not null,
   PARENTID             int                  not null,
   DEVICEROLEID         int                  not null,
   REMARK               nvarchar(1024)       null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_SYS_DEPARTMENT primary key nonclustered (DEPARTMENTID)
)
go

/*==============================================================*/
/* Table: SYS_DEPARTMENT_DEVICES                                */
/*==============================================================*/
create table SYS_DEPARTMENT_DEVICES (
   DEPARTMENTDEVICEID   int                  not null,
   DEPARTMENTID         int                  not null,
   DEVICEID             int                  not null,
   REMARK               nvarchar(1024)       null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_SYS_DEPARTMENT_DEVICES primary key nonclustered (DEPARTMENTDEVICEID)
)
go

/*==============================================================*/
/* Table: SYS_DICTIONARY                                        */
/*==============================================================*/
create table SYS_DICTIONARY (
   DICTIONARYID         int                  not null,
   NAME                 nvarchar(100)        not null,
   TYPEID               int                  null,
   TYPENAME             nvarchar(100)        null,
   PARENTID             int                  null,
   LANGUAGEID           int                  null,
   LEVEL                int                  null,
   ITEMID               int                  null,
   ITEMPROPERTY         nvarchar(1024)       null,
   ITEMVALUE            nvarchar(1024)       null,
   DESCRIPTION          nvarchar(1024)       null,
   REMARK               nvarchar(1024)       null,
   CREATEDATE           datetime             not null,
   CREATEUSERID         int                  not null,
   STATUS               int                  not null,
   UPDATEDATE           datetime             null,
   UPDATEUSERID         int                  null,
   constraint PK_SYS_DICTIONARY primary key nonclustered (DICTIONARYID)
)
go

/*==============================================================*/
/* Table: SYS_MODULE                                            */
/*==============================================================*/
create table SYS_MODULE (
   MODULEID             int                  not null,
   MODULENAME           nvarchar(100)        not null,
   DESCRIPTION          nvarchar(1024)       null,
   PARENTID             int                  not null,
   LINKURL              nvarchar(1024)       null,
   FULLCLASSNAME        nvarchar(1024)       not null,
   MODULELEVEL          int                  not null,
   REMARK               nvarchar(1024)       null,
   CREATEDATE           datetime             not null,
   CREATEUSERID         int                  not null,
   STATUS               int                  not null,
   UPDATEDATE           datetime             null,
   UPDATEUSERID         int                  null,
   constraint PK_SYS_MODULE primary key nonclustered (MODULEID)
)
go

/*==============================================================*/
/* Table: SYS_MODULE_ELEMENTS                                   */
/*==============================================================*/
create table SYS_MODULE_ELEMENTS (
   ELEMENTID            int                  not null,
   MODULEID             int                  not null,
   VISIBLE              bit                  not null,
   ENABLED              bit                  not null,
   DESCRIPTION          nvarchar(1024)       null,
   REMARK               nvarchar(1024)       null,
   CREATEDATE           datetime             not null,
   CREATEUSERID         int                  not null,
   STATUS               int                  not null,
   UPDATEDATE           datetime             null,
   UPDATEUSERID         int                  null,
   constraint PK_SYS_MODULE_ELEMENTS primary key nonclustered (ELEMENTID)
)
go

/*==============================================================*/
/* Table: SYS_OPERATION_LOG                                     */
/*==============================================================*/
create table SYS_OPERATION_LOG (
   LOGID                int                  not null,
   DEPARTMENTID         int                  null,
   USERID               int                  null,
   USERNAME             nvarchar(100)        null,
   OPERATIONCODE        nvarchar(100)        null,
   OPERATIONNAME        nvarchar(1024)       null,
   DETAIL               nvarchar(1024)       null,
   REMARK               nvarchar(1024)       null,
   CREATEDATE           datetime             null,
   constraint PK_SYS_OPERATION_LOG primary key nonclustered (LOGID)
)
go

/*==============================================================*/
/* Table: SYS_ROLE                                              */
/*==============================================================*/
create table SYS_ROLE (
   ROLEID               int                  not null,
   ROLENAME             nvarchar(100)        not null,
   DESCRIPTION          nvarchar(1024)       null,
   REMARK               nvarchar(1024)       null,
   CREATEDATE           datetime             not null,
   CREATEUSERID         int                  not null,
   STATUS               int                  not null,
   UPDATEDATE           datetime             null,
   UPDATEUSERID         int                  null,
   constraint PK_SYS_ROLE primary key nonclustered (ROLEID)
)
go

/*==============================================================*/
/* Table: SYS_ROLE_MODULEELEMENT                                */
/*==============================================================*/
create table SYS_ROLE_MODULEELEMENT (
   SYSROLEELEMENTID     int                  not null,
   ELEMENTID            int                  not null,
   ROLEID               int                  not null,
   constraint PK_SYS_ROLE_MODULEELEMENT primary key nonclustered (SYSROLEELEMENTID)
)
go

/*==============================================================*/
/* Table: SYS_USER                                              */
/*==============================================================*/
create table SYS_USER (
   USERID               int                  not null,
   DEPARTMENTID         int                  null,
   TYPE                 int                  not null,
   USERCODE             nvarchar(25)         not null,
   NAME                 nvarchar(100)        not null,
   GENDER               int                  not null,
   PHONE                nvarchar(255)        null,
   DEPORTMENTID         int                  not null,
   PHOTO                nvarchar(1024)       null,
   STATUS               int                  not null,
   REMARK               nvarchar(1024)       null,
   STARTDATE            datetime             not null,
   ENDDATE              datetime             null,
   constraint PK_SYS_USER primary key nonclustered (USERID)
)
go

/*==============================================================*/
/* Table: SYS_USER_AUTHENTICATION                               */
/*==============================================================*/
create table SYS_USER_AUTHENTICATION (
   USERAUTHENTICATIONID int                  not null,
   USERID               int                  not null,
   DEVICEUSERID         int                  not null,
   DEVICETYPE           int                  not null,
   AUTHENTICATIONTYPE   int                  not null,
   AUTHENTICATIONDATA   nvarchar(1024)       not null,
   VERSION              nvarchar(50)         null,
   ISDURESS_            bit                  not null,
   REMARK               nvarchar(1024)       null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_SYS_USER_AUTHENTICATION primary key nonclustered (USERAUTHENTICATIONID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1:?? 2:?? 3:?? 4:??  ???????????,0-9??,10 ????',
   'user', @CurrentUser, 'table', 'SYS_USER_AUTHENTICATION', 'column', 'AUTHENTICATIONTYPE'
go

/*==============================================================*/
/* Table: SYS_USER_DEVICE_ROLES                                 */
/*==============================================================*/
create table SYS_USER_DEVICE_ROLES (
   USERDEVICEROLEID     int                  not null,
   USERID               int                  not null,
   DEVICEROLEID         int                  not null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         datetime             null,
   UPDATEDATE           datetime             null,
   constraint PK_SYS_USER_DEVICE_ROLES primary key nonclustered (USERDEVICEROLEID)
)
go

/*==============================================================*/
/* Table: SYS_USER_OPERATOR                                     */
/*==============================================================*/
create table SYS_USER_OPERATOR (
   OPERATORID           int                  not null,
   USERID               int                  not null,
   LOGINNAME            nvarchar(100)        not null,
   PASSWORD             nvarchar(1024)       not null,
   LANGUAGEID           int                  not null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_SYS_USER_OPERATOR primary key nonclustered (OPERATORID)
)
go

/*==============================================================*/
/* Table: SYS_USER_PROPERTY                                     */
/*==============================================================*/
create table SYS_USER_PROPERTY (
   _USERPROPERTYID      int                  not null,
   USERID               int                  not null,
   LASTNAME             nvarchar(25)         not null,
   FIRSTNAME            nvarchar(25)         not null,
   NATIONALITY          nvarchar(10)         not null,
   NATIVEPLACE          nvarchar(100)        null,
   BIRTHDAY             datetime             not null,
   MARRIAGE             int                  null,
   POLITICALSTATUS      int                  null,
   DEGREE               int                  null,
   HOMENUMBER           nvarchar(50)         null,
   ENGLISHNAME          nvarchar(50)         null,
   COMPANY              nvarchar(100)        null,
   TECHNICALTITLE       nvarchar(50)         null,
   TECHNICALLEVEL       nvarchar(50)         null,
   IDTYPE               int                  not null,
   IDNUMBER             nvarchar(50)         not null,
   SOCIALNUMBER         nvarchar(50)         null,
   EMAIL                nvarchar(100)        null,
   ADDRESS              nvarchar(1024)       null,
   POSTCODE             nvarchar(50)         null,
   REMARK               nvarchar(1024)       null,
   constraint PK_SYS_USER_PROPERTY primary key nonclustered (_USERPROPERTYID)
)
go

/*==============================================================*/
/* Table: SYS_USER_ROLE                                         */
/*==============================================================*/
create table SYS_USER_ROLE (
   SYSUSERROLEID        int                  not null,
   USERID               int                  not null,
   ROLEID               int                  not null,
   constraint PK_SYS_USER_ROLE primary key nonclustered (SYSUSERROLEID)
)
go

/*==============================================================*/
/* Table: TIME_GROUPS                                           */
/*==============================================================*/
create table TIME_GROUPS (
   TIMEGROUPID          int                  not null,
   TIMEGROUPNAME        nvarchar(100)        not null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_TIME_GROUPS primary key nonclustered (TIMEGROUPID)
)
go

/*==============================================================*/
/* Table: TIME_GROUP_SEGMENT                                    */
/*==============================================================*/
create table TIME_GROUP_SEGMENT (
   TIMEZONEGROUPID      int                  not null,
   TIMEGROUPID          int                  not null,
   TIMESEGMENTID        int                  not null,
   constraint PK_TIME_GROUP_SEGMENT primary key nonclustered (TIMEZONEGROUPID)
)
go

/*==============================================================*/
/* Table: TIME_SEGMENTS                                         */
/*==============================================================*/
create table TIME_SEGMENTS (
   TIMESEGMENTID        int                  not null,
   BEGINTIME            datetime             not null,
   ENDTIME              datetime             not null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_TIME_SEGMENTS primary key nonclustered (TIMESEGMENTID)
)
go

/*==============================================================*/
/* Table: TIME_ZONE                                             */
/*==============================================================*/
create table TIME_ZONE (
   TIMEZONEID           int                  not null,
   TIMEZONENAME         nvarchar(100)        not null,
   CREATEUSERID         int                  not null,
   CREATEDATE           datetime             not null,
   STATUS               int                  not null,
   UPDATEUSERID         int                  null,
   UPDATEDATE           datetime             null,
   constraint PK_TIME_ZONE primary key nonclustered (TIMEZONEID)
)
go

/*==============================================================*/
/* Table: TIME_ZONE_GROUP                                       */
/*==============================================================*/
create table TIME_ZONE_GROUP (
   TIMEZONEGROUPID      int                  not null,
   TIMEZONEID           int                  not null,
   TIMEGROUPID          int                  not null,
   constraint PK_TIME_ZONE_GROUP primary key nonclustered (TIMEZONEGROUPID)
)
go

alter table DEVICE_CONTROLLERS_PARAMETERS
   add constraint FK_DEVICE_C_REFERENCE_DEVICE_C foreign key (DEVICEID)
      references DEVICE_CONTROLLERS (DEVICEID)
go

alter table DEVICE_DOORS
   add constraint FK_DEVICE_D_REFERENCE_DEVICE_R foreign key (DEVICEROLEID)
      references DEVICE_ROLES (DEVICEROLEID)
go

alter table DEVICE_HEADREADINGS
   add constraint FK_DEVICE_H_REFERENCE_DEVICE_R foreign key (DEVICEROLEID)
      references DEVICE_ROLES (DEVICEROLEID)
go

alter table DEVICE_PERMISSIONS
   add constraint FK_DEVICE_P_REFERENCE_DEVICE_C foreign key (DEVICEID)
      references DEVICE_CONTROLLERS (DEVICEID)
go

alter table DEVICE_ROLES_PERMISSIONS
   add constraint FK_DEVICE_R_REFERENCE_DEVICE_P foreign key (DEVICEPERMISSIONID)
      references DEVICE_PERMISSIONS (DEVICEPERMISSIONID)
go

alter table DEVICE_ROLES_PERMISSIONS
   add constraint FK_DEVICE_R_REFERENCE_DEVICE_R foreign key (DEVICEROLEID)
      references DEVICE_ROLES (DEVICEROLEID)
go

alter table SYS_DEPARTMENT_DEVICES
   add constraint FK_SYS_DEPA_REFERENCE_SYS_DEPA foreign key (DEPARTMENTID)
      references SYS_DEPARTMENT (DEPARTMENTID)
go

alter table SYS_DEPARTMENT_DEVICES
   add constraint FK_SYS_DEPA_REFERENCE_DEVICE_C foreign key (DEVICEID)
      references DEVICE_CONTROLLERS (DEVICEID)
go

alter table SYS_MODULE_ELEMENTS
   add constraint FK_SYS_MODU_REFERENCE_SYS_MODU foreign key (MODULEID)
      references SYS_MODULE (MODULEID)
go

alter table SYS_ROLE_MODULEELEMENT
   add constraint FK_SYS_ROLE_REFERENCE_SYS_MODU foreign key (ELEMENTID)
      references SYS_MODULE_ELEMENTS (ELEMENTID)
go

alter table SYS_ROLE_MODULEELEMENT
   add constraint FK_SYS_ROLE_REFERENCE_SYS_ROLE foreign key (ROLEID)
      references SYS_ROLE (ROLEID)
go

alter table SYS_USER
   add constraint FK_SYS_USER_REFERENCE_SYS_DEPA foreign key (DEPARTMENTID)
      references SYS_DEPARTMENT (DEPARTMENTID)
go

alter table SYS_USER_AUTHENTICATION
   add constraint FK_AUTHENTICATION_USERID foreign key (USERID)
      references SYS_USER (USERID)
go

alter table SYS_USER_DEVICE_ROLES
   add constraint FK_SYS_USER_REFERENCE_DEVICE_R foreign key (DEVICEROLEID)
      references DEVICE_ROLES (DEVICEROLEID)
go

alter table SYS_USER_DEVICE_ROLES
   add constraint FK_DEVICE_ROLE_USERID foreign key (USERID)
      references SYS_USER (USERID)
go

alter table SYS_USER_OPERATOR
   add constraint FK_OPERATOR_USERID foreign key (USERID)
      references SYS_USER (USERID)
go

alter table SYS_USER_PROPERTY
   add constraint FK_PROPERTY_USERID foreign key (USERID)
      references SYS_USER (USERID)
go

alter table SYS_USER_ROLE
   add constraint FK_SYS_USER_REFERENCE_SYS_ROLE foreign key (ROLEID)
      references SYS_ROLE (ROLEID)
go

alter table SYS_USER_ROLE
   add constraint FK_SYS_USER_REFERENCE_SYS_USER foreign key (USERID)
      references SYS_USER (USERID)
go

alter table TIME_GROUP_SEGMENT
   add constraint FK_TIME_GRO_REFERENCE_TIME_GRO foreign key (TIMEGROUPID)
      references TIME_GROUPS (TIMEGROUPID)
go

alter table TIME_GROUP_SEGMENT
   add constraint FK_TIME_GRO_REFERENCE_TIME_SEG foreign key (TIMESEGMENTID)
      references TIME_SEGMENTS (TIMESEGMENTID)
go

alter table TIME_ZONE_GROUP
   add constraint FK_TIME_ZON_REFERENCE_TIME_ZON foreign key (TIMEZONEID)
      references TIME_ZONE (TIMEZONEID)
go

alter table TIME_ZONE_GROUP
   add constraint FK_TIME_ZON_REFERENCE_TIME_GRO foreign key (TIMEGROUPID)
      references TIME_GROUPS (TIMEGROUPID)
go

