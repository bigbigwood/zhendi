/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     6/28/2016 6:39:53 PM                         */
/*==============================================================*/


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
   where r.fkeyid = object_id('DEVICE_ROLES_PERMISSIONS') and o.name = 'FK_DEVICE_P_REFERENCE_DEVICE_C')
alter table DEVICE_ROLES_PERMISSIONS
   drop constraint FK_DEVICE_P_REFERENCE_DEVICE_C
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
   where r.fkeyid = object_id('SYS_USER') and o.name = 'FK_SYS_USER_REFERENCE_SYS_DEPA')
alter table SYS_USER
   drop constraint FK_SYS_USER_REFERENCE_SYS_DEPA
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
           where  id = object_id('SYS_ROLE_PERMISSIONS')
            and   type = 'U')
   drop table SYS_ROLE_PERMISSIONS
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
           where  id = object_id('SYS_OPERATOR')
            and   type = 'U')
   drop table SYS_OPERATOR
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
   DeviceID             int                  identity(1,1),
   Mac                  nvarchar(100)        not null,
   Name           		nvarchar(100)        not null,
   Code           		nvarchar(100)        not null,
   SN                   nvarchar(100)        not null,
   Mode                 nvarchar(100)        null,
   CommunicationType      int                  not null,
   BaudRate             nvarchar(100)        null,
   SerialPort           nvarchar(100)        null,
   Password             nvarchar(100)        null,
   IP                   nvarchar(100)        null,
   Port                 nvarchar(100)        null,
   protocol             nvarchar(100)        null,
   Label                nvarchar(1024)       null,
   ServerURL            nvarchar(1024)       null,
   Remark               nvarchar(1024)       null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                   null,
   UpdateDate           datetime             null,
   DeviceParameterID    int                  not null,
   constraint PK_DEVICE_CONTROLLERS primary key nonclustered (DEVICEID)
)
go

/*==============================================================*/
/* Table: DEVICE_CONTROLLERS_PARAMETERS                         */
/*==============================================================*/
create table DEVICE_CONTROLLERS_PARAMETERS (
   DeviceParameterID    int                  identity(1,1),
   AuthticationType     int                  not null,
   AutoOpenTimeZone     int                  null,
   IsSneak              bit                  null,
   MultiPersonLock      bit                  null,
   Linkage              bit                  null,
   LaunchDuress         bit                  null,
   DuressFingerPrint    int                  null,
   DuressOpen           bit                  null,
   DuressAlarm          bit                  null,
   DuressPassword       nvarchar(100)        null,
   constraint PK_DEVICE_CONTROLLERS_PARAMETE primary key nonclustered (DEVICEPARAMETERID)
)
go

/*==============================================================*/
/* Table: DEVICE_DOORS                                          */
/*==============================================================*/
create table DEVICE_DOORS (
   DeviceDoorID         int                  identity(1,1),
   DeviceID             int                  not null,
   Name                 nvarchar(100)        not null,
   Code           		nvarchar(100)        not null,
   ElectricalAppliances int                  null,
   OpenType             int                  null,
   Status               int                  null,
   Remark               nvarchar(1024)       null,
   DelayTime            int                  null,
   AlertType            int                  null,
   OverTimeOpen         int                  null,
   IsOverTime           bit                  null,
   ForceOpen            bit                  null,
   ConnectionAlerm      bit                  null,
   LaunchDuress         bit                  null,
   DuressFingerPrint    int                  null,
   DuressOpen           bit                  null,
   DuressAlarm          bit                  null,
   DuressPassword       nvarchar(100)        null,
   constraint PK_DEVICE_DOORS primary key nonclustered (DEVICEDOORID)
)
go

/*==============================================================*/
/* Table: DEVICE_HEADREADINGS                                   */
/*==============================================================*/
create table DEVICE_HEADREADINGS (
   DeviceHeadReadingID  int                  identity(1,1),
   DeviceID             int                  not null,
   Name           		nvarchar(100)        not null,
   Code           		nvarchar(100)        not null,
   Mac                  nvarchar(100)        not null,
   HeadReadingSN        nvarchar(100)        not null,
   HeadReadingType      int                  not null,
   HeadReadingPerformance nvarchar(100)        null,
   Status               int                  not null,
   constraint PK_DEVICE_HEADREADINGS primary key nonclustered (DEVICEHEADREADINGID)
)
go

/*==============================================================*/
/* Table: DEVICE_OPERATION_LOG                                  */
/*==============================================================*/
create table DEVICE_OPERATION_LOG (
   LogID                int                  identity(1,1),
   RegisterID           int                  null,
   DeviceID             int                  not null,
   DeviceType           int                  not null,
   OperationType        int                  not null,
   OperatorID           int                  not null,
   OperationContent     nvarchar(1024)       null,
   OperationTime        nvarchar(1024)       not null,
   OperationUploadTime  nvarchar(1024)       null,
   constraint PK_DEVICE_OPERATION_LOG primary key nonclustered (LOGID)
)
go

/*==============================================================*/
/* Table: DEVICE_ROLES_PERMISSIONS                                    */
/*==============================================================*/
create table DEVICE_ROLES_PERMISSIONS (
   DeviceRolePermissionID   int                  identity(1,1),
   DeviceRoleID         int                  not null,
   DeviceID             int                  not null,
   Enable               bit                  not null,
   Remark               nvarchar(1024)       null,
   PermissionAction     int                  not null,
   UserGroupVM          nvarchar(1024)       null,
   AllowedAccessTimeZoneID       int                  not null,
   STARTDATE            datetime             not null,
   Enddate              datetime             null,
   constraint PK_DEVICE_ROLES_PERMISSIONS primary key nonclustered (DeviceRolePermissionID)
)
go

/*==============================================================*/
/* Table: DEVICE_ROLES                                          */
/*==============================================================*/
create table DEVICE_ROLES (
   DeviceRoleID         int                  identity(1,1),
   RoleName             nvarchar(100)        not null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_DEVICE_ROLES primary key nonclustered (DEVICEROLEID)
)
go


/*==============================================================*/
/* Table: DEVICE_STATE_HISTORY                                  */
/*==============================================================*/
create table DEVICE_STATE_HISTORY (
   DeviceStateHistoryID int                  identity(1,1),
   DeviceID             int                  not null,
   DeviceType           int                  not null,
   DeviceSN             nvarchar(100)        not null,
   RecordType           int                  null,
   RecordTime           datetime             null,
   DoorStatus           int                  not null,
   Remark               nvarchar(1024)       null,
   constraint PK_DEVICE_STATE_HISTORY primary key nonclustered (DEVICESTATEHISTORYID)
)
go

/*==============================================================*/
/* Table: DEVICE_TRAFFIC_LOG                                    */
/*==============================================================*/
create table DEVICE_TRAFFIC_LOG (
   TrafficID            int                  identity(1,1),
   DeviceID             int                  not null,
   DeviceUserID         int                  not null,
   DeviceType           int                  not null,
   DeviceSN             nvarchar(100)        not null,
   RecordType           nvarchar(1024)       null,
   RecordTime           datetime             null,
   RecordUploadTime     datetime             null,
   AuthenticationType   int                  null,
   Remark               nvarchar(1024)       null,
   constraint PK_DEVICE_TRAFFIC_LOG primary key nonclustered (TRAFFICID)
)
go

/*==============================================================*/
/* Table: SYS_DEPARTMENT                                        */
/*==============================================================*/
create table SYS_DEPARTMENT (
   DepartmentID         int                  identity(1,1),
   Name                 nvarchar(100)        not null,
   DepartmentCode       nvarchar(25)         not null,
   ParentID             int                  not null,
   DeviceRoleID         int                  not null,
   Remark               nvarchar(1024)       null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_SYS_DEPARTMENT primary key nonclustered (DEPARTMENTID)
)
go

/*==============================================================*/
/* Table: SYS_DEPARTMENT_DEVICES                                */
/*==============================================================*/
create table SYS_DEPARTMENT_DEVICES (
   DepartmentDeviceID   int                  identity(1,1),
   DepartmentID         int                  not null,
   DeviceID             int                  not null,
   constraint PK_SYS_DEPARTMENT_DEVICES primary key nonclustered (DEPARTMENTDEVICEID)
)
go

/*==============================================================*/
/* Table: SYS_DICTIONARY                                        */
/*==============================================================*/
create table SYS_DICTIONARY (
   DictionaryID         int                  identity(1,1),
   Name                 nvarchar(100)        not null,
   TypeID               int                  null,
   TypeName             nvarchar(100)        null,
   ParentID             int                  null,
   LanguageID           int                  null,
   Level                int                  null,
   ItemID               int                  null,
   ItemProperty         nvarchar(1024)       null,
   ItemValue            nvarchar(1024)       null,
   Description          nvarchar(1024)       null,
   Remark               nvarchar(1024)       null,
   CreateDate           datetime             not null,
   CreateUserID         int                  not null,
   Status               int                  not null,
   UpdateDate           datetime             null,
   UpdateUserID         int                  null,
   constraint PK_SYS_DICTIONARY primary key nonclustered (DICTIONARYID)
)
go

/*==============================================================*/
/* Table: SYS_MODULE                                            */
/*==============================================================*/
create table SYS_MODULE (
   ModuleID             int                  identity(1,1),
   ModuleName           nvarchar(100)        not null,
   Description          nvarchar(1024)       null,
   ParentID             int                  not null,
   LinkURL              nvarchar(1024)       null,
   FullClassName        nvarchar(1024)       not null,
   ModuleLevel          int                  not null,
   Remark               nvarchar(1024)       null,
   CreateDate           datetime             not null,
   CreateUserID         int                  not null,
   Status               int                  not null,
   UpdateDate           datetime             null,
   UpdateUserID         int                  null,
   constraint PK_SYS_MODULE primary key nonclustered (MODULEID)
)
go

/*==============================================================*/
/* Table: SYS_MODULE_ELEMENTS                                   */
/*==============================================================*/
create table SYS_MODULE_ELEMENTS (
   ElementID            int                  identity(1,1),
   ElementName          nvarchar(100)        not null,
   ModuleID             int                  not null,
   Description          nvarchar(1024)       null,
   Remark               nvarchar(1024)       null,
   CreateDate           datetime             not null,
   CreateUserID         int                  not null,
   Status               int                  not null,
   UpdateDate           datetime             null,
   UpdateUserID         int                  null,
   constraint PK_SYS_MODULE_ELEMENTS primary key nonclustered (ELEMENTID)
)
go

/*==============================================================*/
/* Table: SYS_OPERATION_LOG                                     */
/*==============================================================*/
create table SYS_OPERATION_LOG (
   LogID                int                  identity(1,1),
   DepartmentID         int                  null,
   UserID               int                  null,
   UserName             nvarchar(100)        null,
   OperationCode        nvarchar(100)        null,
   OperationName        nvarchar(1024)       null,
   Detail               nvarchar(1024)       null,
   Remark               nvarchar(1024)       null,
   CreateDate           datetime             null,
   constraint PK_SYS_OPERATION_LOG primary key nonclustered (LOGID)
)
go

/*==============================================================*/
/* Table: SYS_ROLE                                              */
/*==============================================================*/
create table SYS_ROLE (
   RoleID               int                  identity(1,1),
   RoleName             nvarchar(100)        not null,
   Description          nvarchar(1024)       null,
   Remark               nvarchar(1024)       null,
   CreateDate           datetime             not null,
   CreateUserID         int                  not null,
   Status               int                  not null,
   UpdateDate           datetime             null,
   UpdateUserID         int                  null,
   constraint PK_SYS_ROLE primary key nonclustered (ROLEID)
)
go

/*==============================================================*/
/* Table: SYS_ROLE_PERMISSIONS                                */
/*==============================================================*/
create table SYS_ROLE_PERMISSIONS (
   SysRolePermissionID     int                  identity(1,1),
   RoleID               int                  not null,
   ElementID            int                  null,
   ModuleID             int                  null,
   Visible              bit                  not null,
   Enabled              bit                  not null,
   constraint PK_SYS_ROLE_MODULEELEMENT primary key nonclustered (SysRolePermissionID)
)
go

/*==============================================================*/
/* Table: SYS_USER                                              */
/*==============================================================*/
create table SYS_USER (
   UserID               int                  identity(1,1),
   DepartmentID         int                  null,
   Type                 int                  not null,
   UserCode             nvarchar(25)         not null,
   Name                 nvarchar(100)        not null,
   Gender               int                  not null,
   Phone                nvarchar(255)        null,
   Photo                nvarchar(1024)       null,
   Status               int                  not null,
   Remark               nvarchar(1024)       null,
   StartDate            datetime             not null,
   EndDate              datetime             null,
   UserPropertyID       int                  not null,
   constraint PK_SYS_USER primary key nonclustered (USERID)
)
go

/*==============================================================*/
/* Table: SYS_USER_AUTHENTICATION                               */
/*==============================================================*/
create table SYS_USER_AUTHENTICATION (
   UserAuthenticationID int                  identity(1,1),
   UserID               int                  not null,
   DeviceUserID         int                  not null,
   DeviceID             int                  not null,
   DeviceType           int                  not null,
   AuthenticationType   int                  not null,
   AuthenticationData   nvarchar(1024)       not null,
   Version              nvarchar(50)         null,
   IsDuress             bit                  not null,
   Remark               nvarchar(1024)       null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_SYS_USER_AUTHENTICATION primary key nonclustered (USERAUTHENTICATIONID)
)
go

/*==============================================================*/
/* Table: SYS_USER_DEVICE_ROLES                                 */
/*==============================================================*/
create table SYS_USER_DEVICE_ROLES (
   UserDeviceRoleID     int                  identity(1,1),
   UserID               int                  not null,
   DeviceRoleID         int                  not null,
   constraint PK_SYS_USER_DEVICE_ROLES primary key nonclustered (USERDEVICEROLEID)
)
go

/*==============================================================*/
/* Table: SYS_OPERATOR                                     */
/*==============================================================*/
create table SYS_OPERATOR (
   OperatorID           int                  identity(1,1),
   UserID               int                  null,
   LoginName            nvarchar(100)        not null,
   Password             nvarchar(1024)       not null,
   Salt                 nvarchar(1024)       not null,
   LanguageID           int                  not null,
   Photo                nvarchar(1024)       null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_SYS_OPERATOR primary key nonclustered (OPERATORID)
)
go

/*==============================================================*/
/* Table: SYS_USER_PROPERTY                                     */
/*==============================================================*/
create table SYS_USER_PROPERTY (
   UserPropertyID       int                  identity(1,1),
   LastName             nvarchar(25)         null,
   FirstName            nvarchar(25)         null,
   Nationality          int                  null,
   NativePlace          nvarchar(100)        null,
   Birthday             datetime             not null,
   Marriage             int                  null,
   PoliticalStatus      int                  null,
   Degree               int                  null,
   HomeNumber           nvarchar(50)         null,
   EnglishName          nvarchar(50)         null,
   Company              nvarchar(100)        null,
   TechnicalTitle       nvarchar(50)         null,
   TechnicalLevel       nvarchar(50)         null,
   IDType               int                  not null,
   IDNumber             nvarchar(50)         not null,
   SocialNumber         nvarchar(50)         null,
   Email                nvarchar(100)        null,
   Address              nvarchar(1024)       null,
   Postcode             nvarchar(50)         null,
   Remark               nvarchar(1024)       null,
   constraint PK_SYS_USER_PROPERTY primary key nonclustered (USERPROPERTYID)
)
go

/*==============================================================*/
/* Table: SYS_USER_ROLE                                         */
/*==============================================================*/
create table SYS_USER_ROLE (
   SysUserRoleID        int                  identity(1,1),
   UserID               int                  not null,
   RoleID               int                  not null,
   constraint PK_SYS_USER_ROLE primary key nonclustered (SYSUSERROLEID)
)
go

/*==============================================================*/
/* Table: TIME_GROUPS                                           */
/*==============================================================*/
create table TIME_GROUPS (
   TimeGroupID          int                  identity(1,1),
   TimeGroupName        nvarchar(100)        not null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_TIME_GROUPS primary key nonclustered (TIMEGROUPID)
)
go

/*==============================================================*/
/* Table: TIME_GROUP_SEGMENT                                    */
/*==============================================================*/
create table TIME_GROUP_SEGMENT (
   TimeGroupSegmentID   int                  identity(1,1),
   TimeGroupID          int                  not null,
   TimeSegmentID        int                  not null,
   constraint PK_TIME_GROUP_SEGMENT primary key nonclustered (TIMEGROUPSEGMENTID)
)
go

/*==============================================================*/
/* Table: TIME_SEGMENTS                                         */
/*==============================================================*/
create table TIME_SEGMENTS (
   TimeSegmentID        int                  identity(1,1),
   TimeSegmentName      nvarchar(100)        not null,
   BeginTime            nvarchar(100)        not null,
   EndTime              nvarchar(100)        not null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_TIME_SEGMENTS primary key nonclustered (TIMESEGMENTID)
)
go

/*==============================================================*/
/* Table: TIME_ZONE                                             */
/*==============================================================*/
create table TIME_ZONE (
   TimeZoneID           int                  identity(1,1),
   TimeZoneName         nvarchar(100)        not null,
   CreateUserID         int                  not null,
   CreateDate           datetime             not null,
   Status               int                  not null,
   UpdateUserID         int                  null,
   UpdateDate           datetime             null,
   constraint PK_TIME_ZONE primary key nonclustered (TIMEZONEID)
)
go

/*==============================================================*/
/* Table: TIME_ZONE_GROUP                                       */
/*==============================================================*/
create table TIME_ZONE_GROUP (
   TimeZoneGroupID      int                  identity(1,1),
   TimeZoneID           int                  not null,
   TimeGroupID          int                  not null,
   MappingName          nvarchar(100)        not null,
   DisplayOrder         int                  not null,
   constraint PK_TIME_ZONE_GROUP primary key nonclustered (TIMEZONEGROUPID)
)
go


alter table DEVICE_DOORS
   add constraint FK_DEVICE_D_REFERENCE_DEVICE_R foreign key (DEVICEID)
      references DEVICE_CONTROLLERS (DEVICEID)
go

alter table DEVICE_HEADREADINGS
   add constraint FK_DEVICE_H_REFERENCE_DEVICE_R foreign key (DEVICEID)
      references DEVICE_CONTROLLERS (DEVICEID)
go

alter table DEVICE_ROLES_PERMISSIONS
   add constraint FK_DEVICE_P_REFERENCE_DEVICE_C foreign key (DEVICEID)
      references DEVICE_CONTROLLERS (DEVICEID)
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

alter table SYS_USER
   add constraint FK_SYS_USER_REFERENCE_SYS_DEPA foreign key (DEPARTMENTID)
      references SYS_DEPARTMENT (DEPARTMENTID)
go


alter table SYS_USER_DEVICE_ROLES
   add constraint FK_SYS_USER_REFERENCE_DEVICE_R foreign key (DEVICEROLEID)
      references DEVICE_ROLES (DEVICEROLEID)
go

alter table SYS_USER_DEVICE_ROLES
   add constraint FK_DEVICE_ROLE_USERID foreign key (USERID)
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



