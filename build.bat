@echo off
color 66

echo =======================================
echo = AtNet Cms .NET ! ���ĳ������ɹ��� =
echo =======================================


set dir=%~dp0
set megdir=%dir%\refrence.dll\

if exist "%megdir%merge.exe" (

  echo ������,���Ե�...
  cd %dir%bin\

echo  /keyfile:%dir%\Source_Code\AtNet.Cms.Core\atnet.cms.snk>nul

"%megdir%merge.exe" /closed /ndebug /targetplatform:v4 /target:dll /out:%dir%dist\atnet.cms.dll^
 AtNet.Cms.Core.dll AtNet.Cms.BLL.dll AtNet.Cms.DAL.dll AtNet.Cms.Domain.Interface.dll^
 AtNet.Cms.CacheService.dll AtNet.Cms.DataTransfer.dll AtNet.Cms.Domain.Implement.Content.dll^
 AtNet.Cms.DB.dll AtNet.Cms.Cache.dll AtNet.Cms.Domain.Implement.Site.dll AtNet.Cms.Infrastructure.dll ^
 AtNet.Cms.Service.dll AtNet.Cms.ServiceContract.dll^
 AtNet.Cms.ServiceUtil.dll AtNet.Cms.ServiceRepository.dll AtNet.Cms.IDAL.dll^
 AtNet.Cms.Sql.dll AtNet.Cms.Utility.dll StructureMap.dll AtNet.Cms.Web.dll


  echo ���!�����:%dir%dist\atnet.cms.dll

)


pause