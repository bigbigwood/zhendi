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
	
		
		
INSERT SYS_USER_PROPERTY VALUES (N'晓峰', N'张', 1, N'广州', '19880608', 0, 0, 2, N'020-88888888', N'James', N'卓勤信息', N'测试主管', N'T3', 1,  N'440440440440440440448', N'88888888', N'xiaofeng.zhang@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'大仁', N'李', 1, N'上海', '19880607', 0, 2, 2, N'020-77777777', N'Darren', N'卓勤信息', N'测试副主管', N'T3', 1,  N'440440440440440440447', N'77777777', N'daren.li@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'三炮', N'张', 1, N'深圳', '19880606', 0, 0, 3, N'020-66666666', N'SanPao', N'卓勤信息', N'测试副主管', N'T3', 1,  N'440440440440440440446', N'66666666', N'sanpao.zhang@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'明', N'黎',   1, N'香港', '19880605', 0, 0, 1, N'020-55555555', N'leo', N'卓勤信息', N'高级测试工程师', N'T2', 1,  N'440440440440440440445', N'55555555', N'leo.li@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'学友', N'张', 1, N'香港', '19880604', 1, 0, 3, N'020-44444444', N'Xueyou', N'卓勤信息', N'高级测试工程师', N'T2', 1,  N'440440440440440440444', N'44444444', N'xueyou.zhang@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'富城', N'郭', 1, N'香港', '19880603', 0, 0, 4, N'020-33333333', N'Alan', N'卓勤信息', N'高级测试工程师', N'T2', 1,  N'440440440440440440443', N'33333333', N'alan.guo@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'德华', N'刘', 1, N'香港', '19880602', 1, 0, 4, N'020-22222222', N'Andy', N'卓勤信息', N'高级测试工程师', N'T2', 1,  N'440440440440440440442', N'22222222', N'andy.lau@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'杰伦', N'周', 1, N'台湾', '19880601', 1, 0, 1, N'020-11111111', N'Jay', N'卓勤信息', N'测试工程师', N'T1', 1,  N'440440440440440440441', N'11111111', N'jay.zhou@163.com', N'广州市海珠区', N'510000', N'')
INSERT SYS_USER_PROPERTY VALUES (N'英', N'那',   4, N'北京', '19880609', 1, 0, 0, N'020-99999999', N'Anna', N'卓勤信息', N'访客', N'T1', 1,  N'440440440440440440449', N'99999999', N'anna@163.com', N'广州市海珠区', N'510000', N'')

INSERT SYS_USER VALUES (1, 1, N'RD001', N'张晓峰', 0, N'18612340000', N'', 1, N'', '2015-01-01',NULL,1)
INSERT SYS_USER VALUES (1, 1, N'RD002', N'李大仁', 0, N'18612340001', N'', 1, N'', '2015-01-01',NULL,2)
INSERT SYS_USER VALUES (1, 1, N'RD003', N'张三炮', 0, N'18612340002', N'', 1, N'', '2015-01-01',NULL,3)
INSERT SYS_USER VALUES (1, 1, N'RD004', N'黎明', 0, N'18612340003',   N'', 1, N'', '2015-01-01',NULL,4)
INSERT SYS_USER VALUES (1, 1, N'RD005', N'张学友', 0, N'18612340004', N'', 1, N'', '2015-01-01',NULL,5)
INSERT SYS_USER VALUES (1, 1, N'RD006', N'郭富城', 0, N'18612340005', N'', 1, N'', '2015-01-01','2017-01-01',6)
INSERT SYS_USER VALUES (1, 1, N'RD007', N'刘德华', 0, N'18612340006', N'', 1, N'', '2015-01-01',NULL,7)
INSERT SYS_USER VALUES (1, 1, N'RD008', N'周杰伦', 0, N'18612340007', N'', 1, N'', '2015-01-01',NULL,8)
INSERT SYS_USER VALUES (1, 2, N'RD009', N'那英', 1, N'18612340008',   N'', 0, N'', '2015-01-01',NULL,9)


INSERT SYS_USER_AUTHENTICATION VALUES  ( 1, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 1, 2, 2, 2, 2, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 2, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 3, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 4, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 5, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 6, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 7, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 8, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
INSERT SYS_USER_AUTHENTICATION VALUES  ( 9, 1, 1, 1, 1, N'123456', N'', 1, N'', 1, '2015-01-01', 1, NULL, NULL)
	

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



INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 1,  NULL, N'汉族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 2,  NULL, N'壮族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 3,  NULL, N'回族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 4,  NULL, N'满族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 5,  NULL, N'苗族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 6,  NULL, N'维吾尔族'   , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 7,  NULL, N'彝族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 8,  NULL, N'土族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 9,  NULL, N'蒙古族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 10, NULL, N'藏族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 11, NULL, N'侗族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 12, NULL, N'哈萨克族'   , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 13, NULL, N'畲族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 14, NULL, N'纳西族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 15, NULL, N'仫佬族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 16, NULL, N'仡佬族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 17, NULL, N'怒族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 18, NULL, N'保安族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 19, NULL, N'鄂伦春族'   , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 20, NULL, N'瑶族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 21, NULL, N'傣族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 22, NULL, N'高山族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 23, NULL, N'景颇族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 24, NULL, N'羌族'       , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 25, NULL, N'锡伯族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 26, NULL, N'乌孜别克'   , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 27, NULL, N'裕固族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 28, NULL, N'赫哲族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 29, NULL, N'布依族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 30, NULL, N'白族'       , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 31, NULL, N'黎族'       , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 32, NULL, N'拉祜族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 33, NULL, N'柯尔克孜族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 34, NULL, N'布朗族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 35, NULL, N'阿昌族'     , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 36, NULL, N'俄罗斯族'   , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 37, NULL, N'京族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 38, NULL, N'门巴族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 39, NULL, N'朝鲜族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 40, NULL, N'土家族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 41, NULL, N'傈僳族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 42, NULL, N'水族'      , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 43, NULL, N'撒拉族'    , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 44, NULL, N'普米族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 45, NULL, N'鄂温克族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 46, NULL, N'塔塔尔族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 47, NULL, N'珞巴族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 48, NULL, N'哈尼族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 49, NULL, N'佤族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 50, NULL, N'东乡族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 51, NULL, N'达斡尔族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 52, NULL, N'毛南族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 53, NULL, N'塔吉克族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 54, NULL, N'德昂族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 55, NULL, N'独龙族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 
INSERT SYS_DICTIONARY VALUES (N'民族', 10001, N'Nationality', 0, 2052, 0, 56, NULL, N'基诺族'  , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL) 

INSERT SYS_DICTIONARY VALUES (N'性别', 10002, N'Gender', 0, 2052, 0, 0,  NULL, N'男' , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)
INSERT SYS_DICTIONARY VALUES (N'性别', 10002, N'Gender', 0, 2052, 0, 1,  NULL, N'女' , NULL, NULL, '2015-01-01', 1, 1, NULL, NULL)