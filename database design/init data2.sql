INSERT TIME_SEGMENTS VALUES ( N'上午',  N'08:30', N'12:00', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_SEGMENTS VALUES ( N'下午',  N'13:30', N'18:00', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_SEGMENTS VALUES ( N'晚上',  N'19:30', N'21:00', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_SEGMENTS VALUES ( N'全天',  N'00:00', N'24:00', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)

INSERT TIME_GROUPS VALUES  ( N'上班日', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_GROUPS VALUES  ( N'加班日', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_GROUPS VALUES  ( N'休息日', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_GROUPS VALUES  ( N'上半日', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_GROUPS VALUES  ( N'下半日', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)

INSERT TIME_ZONE VALUES ( N'时间区一', 1,  '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_ZONE VALUES ( N'时间区二', 1,  '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT TIME_ZONE VALUES ( N'时间区三', 1,  '2016-01-01 00:00:00.000', 1, NULL, NULL)

INSERT TIME_GROUP_SEGMENT VALUES (1, 1)
INSERT TIME_GROUP_SEGMENT VALUES (1, 2)
INSERT TIME_GROUP_SEGMENT VALUES (2, 1)
INSERT TIME_GROUP_SEGMENT VALUES (2, 2)
INSERT TIME_GROUP_SEGMENT VALUES (2, 3)
INSERT TIME_GROUP_SEGMENT VALUES (4, 1)
INSERT TIME_GROUP_SEGMENT VALUES (5, 2)

INSERT TIME_ZONE_GROUP VALUES (1, 1, '星期一', 1)
INSERT TIME_ZONE_GROUP VALUES (1, 1, '星期二', 2)
INSERT TIME_ZONE_GROUP VALUES (1, 1, '星期三', 3)
INSERT TIME_ZONE_GROUP VALUES (1, 1, '星期四', 4)
INSERT TIME_ZONE_GROUP VALUES (1, 1, '星期五', 5)
INSERT TIME_ZONE_GROUP VALUES (1, 2, '星期六', 6)
INSERT TIME_ZONE_GROUP VALUES (1, 3, '星期日', 7)

INSERT TIME_ZONE_GROUP VALUES (2, 1, '星期一', 1)
INSERT TIME_ZONE_GROUP VALUES (2, 2, '星期二', 2)
INSERT TIME_ZONE_GROUP VALUES (2, 1, '星期三', 3)
INSERT TIME_ZONE_GROUP VALUES (2, 2, '星期四', 4)
INSERT TIME_ZONE_GROUP VALUES (2, 1, '星期五', 5)
INSERT TIME_ZONE_GROUP VALUES (2, 2, '星期六', 6)
INSERT TIME_ZONE_GROUP VALUES (2, 3, '星期日', 7)

INSERT TIME_ZONE_GROUP VALUES (3, 2, '星期一', 1)
INSERT TIME_ZONE_GROUP VALUES (3, 2, '星期二', 2)
INSERT TIME_ZONE_GROUP VALUES (3, 2, '星期三', 3)
INSERT TIME_ZONE_GROUP VALUES (3, 2, '星期四', 4)
INSERT TIME_ZONE_GROUP VALUES (3, 1, '星期五', 5)
INSERT TIME_ZONE_GROUP VALUES (3, 3, '星期六', 6)
INSERT TIME_ZONE_GROUP VALUES (3, 3, '星期日', 7)



INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 1', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,1)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 2', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,2)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 3', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,3)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 4', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,4)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 5', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,5)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 6', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,6)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 7', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,7)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 8', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,8)
INSERT DEVICE_CONTROLLERS_PARAMETERS VALUES  ( 1, 1,1,1,1,1,1,1,1, N'000000')
INSERT DEVICE_CONTROLLERS VALUES (N'00-00-00-00-00-00', N'Device 9', N'000000', N'AUTO',1, N'', N'', N'', N'', N'', N'', N'', N'', N'',1,'2016-01-01 00:00:00.000',1,NULL,NULL,9)
				

INSERT DEVICE_DOORS
        ( DeviceID ,
          Name ,
          ElectricalAppliances ,
          OpenType ,
          Status ,
          Remark ,
          DelayTime ,
          AlertType ,
          OverTimeOpen ,
          IsOverTime ,
          ForceOpen ,
          ConnectionAlerm ,
          LaunchDuress ,
          DuressFingerPrint ,
          DuressOpen ,
          DuressAlarm ,
          DuressPassword
        )
VALUES  ( 1 , -- DeviceID - int
          N'D1A1' , -- Name - nvarchar(100)
          0 , -- ElectricalAppliances - int
          1 , -- OpenType - int
          1 , -- Status - int
          N'' , -- Remark - nvarchar(1024)
          0 , -- DelayTime - int
          0 , -- AlertType - int
          0 , -- OverTimeOpen - int
          NULL , -- IsOverTime - bit
          NULL , -- ForceOpen - bit
          NULL , -- ConnectionAlerm - bit
          NULL , -- LaunchDuress - bit
          0 , -- DuressFingerPrint - int
          NULL , -- DuressOpen - bit
          NULL , -- DuressAlarm - bit
          N'000000'  -- DuressPassword - nvarchar(100)
        )

INSERT DEVICE_DOORS
        ( DeviceID ,
          Name ,
          ElectricalAppliances ,
          OpenType ,
          Status ,
          Remark ,
          DelayTime ,
          AlertType ,
          OverTimeOpen ,
          IsOverTime ,
          ForceOpen ,
          ConnectionAlerm ,
          LaunchDuress ,
          DuressFingerPrint ,
          DuressOpen ,
          DuressAlarm ,
          DuressPassword
        )
VALUES  ( 1 , -- DeviceID - int
          N'D1A2' , -- Name - nvarchar(100)
          0 , -- ElectricalAppliances - int
          1 , -- OpenType - int
          1 , -- Status - int
          N'' , -- Remark - nvarchar(1024)
          0 , -- DelayTime - int
          0 , -- AlertType - int
          0 , -- OverTimeOpen - int
          NULL , -- IsOverTime - bit
          NULL , -- ForceOpen - bit
          NULL , -- ConnectionAlerm - bit
          NULL , -- LaunchDuress - bit
          0 , -- DuressFingerPrint - int
          NULL , -- DuressOpen - bit
          NULL , -- DuressAlarm - bit
          N'000000'  -- DuressPassword - nvarchar(100)
        )

INSERT DEVICE_DOORS
        ( DeviceID ,
          Name ,
          ElectricalAppliances ,
          OpenType ,
          Status ,
          Remark ,
          DelayTime ,
          AlertType ,
          OverTimeOpen ,
          IsOverTime ,
          ForceOpen ,
          ConnectionAlerm ,
          LaunchDuress ,
          DuressFingerPrint ,
          DuressOpen ,
          DuressAlarm ,
          DuressPassword
        )
VALUES  ( 2 , -- DeviceID - int
          N'D2A1' , -- Name - nvarchar(100)
          0 , -- ElectricalAppliances - int
          1 , -- OpenType - int
          1 , -- Status - int
          N'' , -- Remark - nvarchar(1024)
          0 , -- DelayTime - int
          0 , -- AlertType - int
          0 , -- OverTimeOpen - int
          NULL , -- IsOverTime - bit
          NULL , -- ForceOpen - bit
          NULL , -- ConnectionAlerm - bit
          NULL , -- LaunchDuress - bit
          0 , -- DuressFingerPrint - int
          NULL , -- DuressOpen - bit
          NULL , -- DuressAlarm - bit
          N'000000'  -- DuressPassword - nvarchar(100)
        )
		

INSERT DEVICE_HEADREADINGS
        ( DeviceID ,
          Mac ,
          HeadReadingSN ,
          HeadReadingType ,
          HeadReadingPerformance ,
          Status
        )
VALUES  ( 1 , -- DeviceID - int
          N'00-00-00-00-00-00' , -- Mac - nvarchar(100)
          N'' , -- HeadReadingSN - nvarchar(100)
          1 , -- HeadReadingType - int
          N'' , -- HeadReadingPerformance - nvarchar(100)
          1  -- Status - int
        )

INSERT DEVICE_HEADREADINGS
        ( DeviceID ,
          Mac ,
          HeadReadingSN ,
          HeadReadingType ,
          HeadReadingPerformance ,
          Status
        )
VALUES  ( 1 , -- DeviceID - int
          N'00-00-00-00-00-00' , -- Mac - nvarchar(100)
          N'' , -- HeadReadingSN - nvarchar(100)
          1 , -- HeadReadingType - int
          N'' , -- HeadReadingPerformance - nvarchar(100)
          1  -- Status - int
        )

INSERT DEVICE_HEADREADINGS
        ( DeviceID ,
          Mac ,
          HeadReadingSN ,
          HeadReadingType ,
          HeadReadingPerformance ,
          Status
        )
VALUES  ( 2 , -- DeviceID - int
          N'00-00-00-00-00-00' , -- Mac - nvarchar(100)
          N'' , -- HeadReadingSN - nvarchar(100)
          1 , -- HeadReadingType - int
          N'' , -- HeadReadingPerformance - nvarchar(100)
          1  -- Status - int
        )

INSERT DEVICE_ROLES VALUES (N'Admin', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT DEVICE_ROLES VALUES (N'Manager', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
INSERT DEVICE_ROLES VALUES (N'General Stuff', 1, '2016-01-01 00:00:00.000', 1, NULL, NULL)
 
INSERT DEVICE_ROLES_PERMISSIONS VALUES (1, 1, 1, '', 1, '', 1, '2016-01-01 00:00:00.000', NULL)
INSERT DEVICE_ROLES_PERMISSIONS VALUES (1, 2, 1, '', 1, '', 1, '2016-01-01 00:00:00.000', NULL)
INSERT DEVICE_ROLES_PERMISSIONS VALUES (2, 1, 1, '', 1, '', 1, '2016-01-01 00:00:00.000', NULL)
INSERT DEVICE_ROLES_PERMISSIONS VALUES (2, 2, 1, '', 1, '', 1, '2016-01-01 00:00:00.000', NULL)
INSERT DEVICE_ROLES_PERMISSIONS VALUES (3, 1, 1, '', 1, '', 1, '2016-01-01 00:00:00.000', NULL)
		
INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'R&D' , -- Name - nvarchar(100)
          N'RD' , -- DepartmentCode - nvarchar(25)
          -1 , -- ParentID - int
          3 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )

INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'Finance Dept' , -- Name - nvarchar(100)
          N'FD' , -- DepartmentCode - nvarchar(25)
          -1 , -- ParentID - int
          3 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )
INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'Secure Dept' , -- Name - nvarchar(100)
          N'SD' , -- DepartmentCode - nvarchar(25)
          -1 , -- ParentID - int
          1 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )
		

	
INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'Development Team 1' , -- Name - nvarchar(100)
          N'RDD1' , -- DepartmentCode - nvarchar(25)
          1 , -- ParentID - int
          3 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )


INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'Development Team 2' , -- Name - nvarchar(100)
          N'RDD2' , -- DepartmentCode - nvarchar(25)
          1 , -- ParentID - int
          3 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )


INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'Tester Team 1' , -- Name - nvarchar(100)
          N'RDT1' , -- DepartmentCode - nvarchar(25)
          1 , -- ParentID - int
          3 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )


INSERT SYS_DEPARTMENT
        ( Name ,
          DepartmentCode ,
          ParentID ,
          DeviceRoleID ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( N'Tester Team 2' , -- Name - nvarchar(100)
          N'RDT2' , -- DepartmentCode - nvarchar(25)
          1 , -- ParentID - int
          3 , -- DeviceRoleID - int
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )

INSERT SYS_DEPARTMENT_DEVICES VALUES (1, 1)
INSERT SYS_DEPARTMENT_DEVICES VALUES (2, 1)
INSERT SYS_DEPARTMENT_DEVICES VALUES (2, 2)
INSERT SYS_DEPARTMENT_DEVICES VALUES (3, 1)
		
INSERT SYS_USER_PROPERTY
        ( LastName ,
          FirstName ,
          Nationality ,
          NativePlace ,
          Birthday ,
          Marriage ,
          PoliticalStatus ,
          Degree ,
          HomeNumber ,
          EnglishName ,
          Company ,
          TechnicalTitle ,
          TechnicalLevel ,
          IDType ,
          IDNumber ,
          SocialNumber ,
          Email ,
          Address ,
          Postcode ,
          Remark
        )
VALUES  ( N'AAA' , -- LastName - nvarchar(25)
          N'BBB' , -- FirstName - nvarchar(25)
          N'' , -- Nationality - nvarchar(10)
          N'' , -- NativePlace - nvarchar(100)
          '19880606' , -- Birthday - datetime
          0 , -- Marriage - int
          0 , -- PoliticalStatus - int
          0 , -- Degree - int
          N'' , -- HomeNumber - nvarchar(50)
          N'' , -- EnglishName - nvarchar(50)
          N'' , -- Company - nvarchar(100)
          N'' , -- TechnicalTitle - nvarchar(50)
          N'' , -- TechnicalLevel - nvarchar(50)
          1, -- IDType - int
          N'2222222222' , -- IDNumber - nvarchar(50)
          N'' , -- SocialNumber - nvarchar(50)
          N'' , -- Email - nvarchar(100)
          N'' , -- Address - nvarchar(1024)
          N'' , -- Postcode - nvarchar(50)
          N''  -- Remark - nvarchar(1024)
        )
		
INSERT SYS_USER_PROPERTY
        ( LastName ,
          FirstName ,
          Nationality ,
          NativePlace ,
          Birthday ,
          Marriage ,
          PoliticalStatus ,
          Degree ,
          HomeNumber ,
          EnglishName ,
          Company ,
          TechnicalTitle ,
          TechnicalLevel ,
          IDType ,
          IDNumber ,
          SocialNumber ,
          Email ,
          Address ,
          Postcode ,
          Remark
        )
VALUES  ( N'CCC' , -- LastName - nvarchar(25)
          N'DDD' , -- FirstName - nvarchar(25)
          N'' , -- Nationality - nvarchar(10)
          N'' , -- NativePlace - nvarchar(100)
          '19900606' , -- Birthday - datetime
          0 , -- Marriage - int
          0 , -- PoliticalStatus - int
          0 , -- Degree - int
          N'' , -- HomeNumber - nvarchar(50)
          N'' , -- EnglishName - nvarchar(50)
          N'' , -- Company - nvarchar(100)
          N'' , -- TechnicalTitle - nvarchar(50)
          N'' , -- TechnicalLevel - nvarchar(50)
          1, -- IDType - int
          N'1111111111' , -- IDNumber - nvarchar(50)
          N'' , -- SocialNumber - nvarchar(50)
          N'' , -- Email - nvarchar(100)
          N'' , -- Address - nvarchar(1024)
          N'' , -- Postcode - nvarchar(50)
          N''  -- Remark - nvarchar(1024)
        )
		

INSERT SYS_USER
        ( DepartmentID ,
          Type ,
          UserCode ,
          Name ,
          Gender ,
          Phone ,
          Photo ,
          Status ,
          Remark ,
          StartDate ,
          EndDate ,
          UserPropertyID
        )
VALUES  ( 1 , -- DepartmentID - int
          1 , -- Type - int
          N'RD001' , -- UserCode - nvarchar(25)
          N'AAA.BBB' , -- Name - nvarchar(100)
          1 , -- Gender - int
          N'18612340000' , -- Phone - nvarchar(255)
          N'' , -- Photo - nvarchar(1024)
          1 , -- Status - int
          N'' , -- Remark - nvarchar(1024)
          '2015-01-01' , -- StartDate - datetime
          NULL , -- EndDate - datetime
          1  -- UserPropertyID - int
        )

INSERT SYS_USER
        ( DepartmentID ,
          Type ,
          UserCode ,
          Name ,
          Gender ,
          Phone ,
          Photo ,
          Status ,
          Remark ,
          StartDate ,
          EndDate ,
          UserPropertyID
        )
VALUES  ( 3 , -- DepartmentID - int
          1 , -- Type - int
          N'SD001' , -- UserCode - nvarchar(25)
          N'CCC.DDD' , -- Name - nvarchar(100)
          1 , -- Gender - int
          N'18612341111' , -- Phone - nvarchar(255)
          N'' , -- Photo - nvarchar(1024)
          1 , -- Status - int
          N'' , -- Remark - nvarchar(1024)
          '2015-01-01' , -- StartDate - datetime
          NULL , -- EndDate - datetime
          2  -- UserPropertyID - int
        )
		

INSERT SYS_USER_AUTHENTICATION
        ( UserID ,
          DeviceUserID ,
          DeviceID ,
          DeviceType ,
          AuthenticationType ,
          AuthenticationData ,
          Version ,
          IsDuress ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( 1 , -- UserID - int
          1 , -- DeviceUserID - int
          1 , -- DeviceID - int
          1 , -- DeviceType - int
          1 , -- AuthenticationType - int
          N'123456' , -- AuthenticationData - nvarchar(1024)
          N'' , -- Version - nvarchar(50)
          1 , -- IsDuress - bit
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )

INSERT SYS_USER_AUTHENTICATION
        ( UserID ,
          DeviceUserID ,
          DeviceID ,
          DeviceType ,
          AuthenticationType ,
          AuthenticationData ,
          Version ,
          IsDuress ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( 2 , -- UserID - int
          2 , -- DeviceUserID - int
          1 , -- DeviceID - int
          1 , -- DeviceType - int
          1 , -- AuthenticationType - int
          N'123456' , -- AuthenticationData - nvarchar(1024)
          N'' , -- Version - nvarchar(50)
          1 , -- IsDuress - bit
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )

INSERT SYS_USER_AUTHENTICATION
        ( UserID ,
          DeviceUserID ,
          DeviceID ,
          DeviceType ,
          AuthenticationType ,
          AuthenticationData ,
          Version ,
          IsDuress ,
          Remark ,
          CreateUserID ,
          CreateDate ,
          Status ,
          UpdateUserID ,
          UpdateDate
        )
VALUES  ( 2 , -- UserID - int
          2 , -- DeviceUserID - int
          2 , -- DeviceID - int
          2 , -- DeviceType - int
          2 , -- AuthenticationType - int
          N'######' , -- AuthenticationData - nvarchar(1024)
          N'' , -- Version - nvarchar(50)
          1 , -- IsDuress - bit
          N'' , -- Remark - nvarchar(1024)
          1 , -- CreateUserID - int
          '2016-01-01' , -- CreateDate - datetime
          1 , -- Status - int
          NULL , -- UpdateUserID - int
          NULL  -- UpdateDate - datetime
        )
		
		
		

INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Stuff Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          -1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'Stuff Management' , -- FullClassName - nvarchar(1024)
          1 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )

INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Device Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          -1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'Device Management' , -- FullClassName - nvarchar(1024)
          1 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )

INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Dashboard' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          -1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'Dashboard' , -- FullClassName - nvarchar(1024)
          1 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )

INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Mantance' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          -1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'Mantance' , -- FullClassName - nvarchar(1024)
          1 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )

INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Backend Service' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          -1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'Backend Service' , -- FullClassName - nvarchar(1024)
          1 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )
		


INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'dept Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'dept Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'empolyee Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          1 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'empolyee Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'device Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          2 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'device Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )



INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'door Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          2 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'door Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'timesegment Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          2 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'timesegment Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'timegroup Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          2 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'timegroup Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE
        ( ModuleName ,
          Description ,
          ParentID ,
          LinkURL ,
          FullClassName ,
          ModuleLevel ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'timezone Management' , -- ModuleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          2 , -- ParentID - int
          N'' , -- LinkURL - nvarchar(1024)
          N'timezone Management' , -- FullClassName - nvarchar(1024)
          2 , -- ModuleLevel - int
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000'  , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )
		

INSERT SYS_MODULE_ELEMENTS
        ( ElementName ,
          ModuleID ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Add empolyee' , -- ElementName - nvarchar(100)
          7 , -- ModuleID - int
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE_ELEMENTS
        ( ElementName ,
          ModuleID ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'modify empolyee' , -- ElementName - nvarchar(100)
          7 , -- ModuleID - int
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )

INSERT SYS_MODULE_ELEMENTS
        ( ElementName ,
          ModuleID ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'delete empolyee' , -- ElementName - nvarchar(100)
          7 , -- ModuleID - int
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_MODULE_ELEMENTS
        ( ElementName ,
          ModuleID ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'query empolyee' , -- ElementName - nvarchar(100)
          7 , -- ModuleID - int
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )
		
		

INSERT SYS_ROLE
        ( RoleName ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Admin' , -- RoleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )


INSERT SYS_ROLE
        ( RoleName ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Operator' , -- RoleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )

INSERT SYS_ROLE
        ( RoleName ,
          Description ,
          Remark ,
          CreateDate ,
          CreateUserID ,
          Status ,
          UpdateDate ,
          UpdateUserID
        )
VALUES  ( N'Monitor' , -- RoleName - nvarchar(100)
          N'' , -- Description - nvarchar(1024)
          N'' , -- Remark - nvarchar(1024)
          '2016-01-01 00:00:00.000' , -- CreateDate - datetime
          1 , -- CreateUserID - int
          1 , -- Status - int
          NULL , -- UpdateDate - datetime
          NULL  -- UpdateUserID - int
        )
		
		
		
--admin
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 1, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 2, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 3, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 4, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 5, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 6, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 7, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 8, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 9, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 10, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 11, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, NULL, 12, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, 1, NULL, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, 2, NULL, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, 3, NULL, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (1, 4, NULL, 1, 1)

--operator
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 1, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 2, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 3, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 4, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 5, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 6, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 7, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 8, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 9, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 10, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 11, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, NULL, 12, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, 1, NULL, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, 2, NULL, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, 3, NULL, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (2, 4, NULL, 1, 1)

--monitor
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 1, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 2, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 3, 1, 1)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 4, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 5, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 6, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 7, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 8, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 9, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 10, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 11, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, NULL, 12, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, 1, NULL, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, 2, NULL, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, 3, NULL, 0, 0)
INSERT SYS_ROLE_PERMISSIONS VALUES (3, 4, NULL, 0, 0)
