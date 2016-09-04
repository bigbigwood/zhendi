@echo Call Visual Studio 2008 Command Promot
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\vsvars32.bat"

@echo Process Dispatch proxy
@echo wsdl.exe  /namespace:IMEITransmitProxy  /o:C:\IMEITransmitProxy.cs C:\Users\Wood\Desktop\m2wsveeservicesmgmt.wsdl

wsdl.exe  /namespace:Rld.Acs.WpfApplication.Service.DeviceService  /o:C:\DeviceServiceProxy.cs http://localhost:11727/DeviceService.svc?singlewsdl

pause


