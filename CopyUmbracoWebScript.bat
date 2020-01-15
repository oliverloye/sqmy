REM set default values

SET dest="C:\inetpub\UmbracoPraktikant2"

REM set custom values
REM %COMPUTERNAME% is all caps in console, even if not in GUI

IF "%COMPUTERNAME%"=="Praktik1" (
	SET dest="C:\inetpub\UmbracoPraktikant2"
)
IF "%COMPUTERNAME%"=="Praktik7" (
	SET dest="C:\inetpub\UmbracoPraktikant2"
)

IF "%COMPUTERNAME%"=="Praktik6" (
	SET dest="C:\inetpub\UmbracoPraktikant2"
)
IF "%COMPUTERNAME%"=="ALPHA-PRAK04" (
	SET dest="C:\inetpub\UmbracoPraktikant2"
)
IF "%COMPUTERNAME%"=="ALPHA-PRAK03" (
	SET dest="C:\inetpub\UmbracoPraktikant2"
)


rem Website
robocopy .\UmbracoWebDev\App_Code %dest%\App_Code *.cs /E
robocopy .\UmbracoWebDev\Controllers %dest%\Controllers *.* /E
robocopy .\UmbracoWebDev\App_Plugins %dest%\App_Plugins *.* /E
robocopy .\UmbracoWebDev\Scripts %dest%\Scripts *.* /E
robocopy .\UmbracoWebDev\Views %dest%\Views *.* /E
robocopy .\UmbracoWebDev\Models %dest%\Models *.* /E
robocopy .\UmbracoWebDev\Media %dest%\Media *.* /E
robocopy .\UmbracoWebDev\css %dest%\css *.* /E
robocopy .\UmbracoWebDev\bin %dest%\bin *.* /E /XC /XN /XO
robocopy .\UmbracoWebDev\bin %dest%\bin\ UmbracoWebDev.dll /E
robocopy .\UmbracoWebDev\bin %dest%\bin\ Importer.exe /E
robocopy .\UmbracoWebDev\bin %dest%\bin\ Importer.pdb /E
robocopy .\UmbracoWebDev\bin %dest%\bin\ Importer.exe.config /E
robocopy .\UmbracoWebDev\bin %dest%\bin\ ClassLibrary.dll /E
robocopy .\UmbracoWebDev\bin %dest%\bin\ ClassLibrary.pdb /E
robocopy .\UmbracoWebDev\bin %dest%\bin\ WebControllers.dll /E
//robocopy .\UmbracoWebDev\ %dest%\ Web.config *.aspx *.asax




pause


